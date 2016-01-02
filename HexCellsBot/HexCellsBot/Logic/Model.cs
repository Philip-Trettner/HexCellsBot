using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GlmSharp;
using Tesseract;

namespace HexCellsBot.Logic
{
    public class Model
    {
        public const int ColorYellow = 0xFFAF29;
        public const int ColorDeepYellow = 0xFF9F00;

        public const int ColorBlack = 0x3E3E3E;
        public const int ColorDeepBlack = 0x2C2F31;

        public const int ColorBlue = 0x05A4EB;
        public const int ColorDeepBlue = 0x149CD8;

        public const int RgbMask = 0x00FFFFFF;

        public readonly List<Cell> Cells = new List<Cell>();

        private static readonly TesseractEngine Tengine = new TesseractEngine("./tessdata", "eng", EngineMode.Default);

        public List<NumberConstraint> NumberConstraints = new List<NumberConstraint>();

        public readonly List<SolveStep> Steps = new List<SolveStep>();

        public static Model Analyze(Bitmap bmp)
        {
            var h = bmp.Height;
            var w = bmp.Width;
            var data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var colors = new int[w * h];
            Marshal.Copy(data.Scan0, colors, 0, w * h);
            bmp.UnlockBits(data);
            for (var i = 0; i < w * h; ++i)
                colors[i] &= RgbMask;

            var m = new Model();

            m.CheckCells(CellState.Yellow, ColorYellow, ColorDeepYellow, w, h, colors);
            m.CheckCells(CellState.Blue, ColorBlue, ColorDeepBlue, w, h, colors);
            m.CheckCells(CellState.Black, ColorBlack, ColorDeepBlack, w, h, colors);
            for (var i = 0; i < m.Cells.Count; ++i)
                m.Cells[i].Nr = i;

            m.ConnectNeigbors();

            m.CheckText(bmp);

            m.CollectRules();

            m.Solve();

            return m;
        }

        private void Solve()
        {
            // low hanging stuff
            foreach (var rule in NumberConstraints)
            {
                // Count == number of yellow + blue => all blue
                if (rule.Count == rule.Cells.Count(c => c.State != CellState.Black))
                {
                    foreach (var c in rule.Cells.Where(c => c.MaySolve))
                        Steps.Add(new SolveStep(c, CellState.Blue, $"{rule.IDString} has exact number match"));
                }

                // Count == number of blue => all black
                if (rule.Count == rule.Cells.Count(c => c.State == CellState.Blue))
                {
                    foreach (var c in rule.Cells.Where(c => c.MaySolve))
                        Steps.Add(new SolveStep(c, CellState.Black, $"{rule.IDString} is already full, everything else is black"));
                }

                // Connection model and special types
                switch (rule.Type)
                {
                    case ConstraintType.Connected:
                    case ConstraintType.NonConnected:
                        Steps.AddRange(rule.ConnectionModel.GenerateStepsFor(rule));
                        break;

                    default:
                        break; // nothing
                }
            }
        }

        private bool AddRuleChecked(NumberConstraint nc)
        {
            if (nc == null)
                return false;

            if (!nc.IsRelevant)
                return false;

            if (NumberConstraints.Any(nc.SameAs))
                return false;

            NumberConstraints.Add(nc);
            return true;
        }

        private bool RuleInference()
        {
            var changed = false;

            // add pure rules
            foreach (var nc in NumberConstraints.ToArray())
                changed |= AddRuleChecked(nc.PureRule);

            foreach (var nc1 in NumberConstraints.ToArray())
                foreach (var nc2 in NumberConstraints.ToArray())
                {
                    if (nc1 == nc2)
                        continue;

                    // TODO: check with special types

                    // check if nc1 is subset of nc2
                    if (nc1.Type == ConstraintType.Vanilla && nc2.Type == ConstraintType.Vanilla)
                        if (nc1.IsStrictSubsetOf(nc2))
                            changed |= AddRuleChecked(nc2.Without(nc1));

                    // check cutting stuff
                    // WORKS WITH CONN AND NONCONN
                    if (nc1.HasCutWith(nc2))
                        changed |= AddRuleChecked(nc1.ProbeCutWith(nc2));
                }

            return changed;
        }

        private void CollectRules()
        {
            // get initial rules
            foreach (var cell in Cells)
            {
                var nc = cell.Constraint;
                if (nc != null)
                    NumberConstraints.Add(nc);
            }

            // remove irrelevant ones
            NumberConstraints.RemoveAll(nc => !nc.IsRelevant);

            // infer rules
            while (RuleInference())
            {
            }

            // remove empty rules
            NumberConstraints.RemoveAll(nc => nc.Cells.Count == 0);
        }

        private void CheckText(Bitmap bmp)
        {
            foreach (var c in Cells)
            {
                if (c.State == CellState.Yellow)
                    continue;

                var start = c.Center - c.Radius;
                var end = c.Center + c.Radius;
                var size = end - start;

                /*var page = Tengine.Process(bmp);//, new Rect(start.x, start.y, size.x, size.y));
                c.InnerText = page.GetText() + page.GetMeanConfidence();
                page.Dispose();*/

                var subbmp = new Bitmap(size.x, size.y);
                for (var y = 0; y < size.y; ++y)
                    for (var x = 0; x < size.x; ++x)
                    {
                        var color = bmp.GetPixel(start.x + x, start.y + y);
                        var whiteDis = (255 - color.R) + (255 - color.G) + (255 - color.B);
                        //whiteDis *= 5;
                        if (whiteDis > 100) whiteDis = 255;
                        else whiteDis = 0;
                        if (whiteDis > 255) whiteDis = 255;
                        var dx = x - size.x/2;
                        var dy = y - size.y/2;
                        if (dx*dx + dy*dy > c.Radius*c.Radius)
                            whiteDis = 255;
                        subbmp.SetPixel(x, y, Color.FromArgb(whiteDis, whiteDis, whiteDis));
                    }
                subbmp.Save($"C:\\Temp\\cell{c.Center.x}-{c.Center.y}.png");
                var page = Tengine.Process(subbmp, PageSegMode.SingleLine); //, new Rect(start.x, start.y, size.x, size.y));
                c.InnerText = page.GetText().Trim();
                if (page.GetMeanConfidence() < .1)
                    c.InnerText = "";

                //  if (c.State == CellState.Black)
                //        Debugger.Break();
                page.Dispose();
                subbmp.Dispose();
            }
        }

        private void ConnectNeigbors()
        {
            foreach (var c1 in Cells)
            {
                foreach (var c2 in Cells)
                {
                    if (c1 == c2)
                        continue;

                    if (ivec2.Distance(c1.Center, c2.Center) > (c1.Radius + c2.Radius)*1.8f)
                        continue;

                    c1.AllNeighbors.Add(c2);
                    if (c1.AllNeighbors.Count > 6)
                        throw new InvalidOperationException("too many neighbors");

                    var dir = c2.Center - c1.Center;
                    var angle = Math.Atan2(dir.y, dir.x)/Math.PI*180;

                    const int eps = 5;

                    if (Math.Abs(30 - angle) < eps)
                        c1.Neighbors[(int) CellNeighbor.BottomRight] = c2;
                    else if (Math.Abs(90 - angle) < eps)
                        c1.Neighbors[(int) CellNeighbor.Bottom] = c2;
                    else if (Math.Abs(150 - angle) < eps)
                        c1.Neighbors[(int) CellNeighbor.BottomLeft] = c2;
                    else if (Math.Abs(-150 - angle) < eps)
                        c1.Neighbors[(int) CellNeighbor.TopLeft] = c2;
                    else if (Math.Abs(-90 - angle) < eps)
                        c1.Neighbors[(int) CellNeighbor.Top] = c2;
                    else if (Math.Abs(-30 - angle) < eps)
                        c1.Neighbors[(int) CellNeighbor.TopRight] = c2;
                    else throw new InvalidOperationException("Invalid neighbor");
                }
            }

            // sanity
            foreach (var c1 in Cells)
            {
                for (var i = 0; i < 6; ++i)
                {
                    if (c1.Neighbors[i] != null)
                    {
                        var c2 = c1.Neighbors[i];
                        if (c2.Neighbors[(i + 3)%6] != c1)
                            throw new InvalidOperationException("Inconsistent neighbors");
                    }
                }
            }
        }

        public void CheckCells(CellState state, int stateColor, int stateDeepColor, int w, int h, int[] colors)
        {
            const int xOffset = 6;
            for (var y = 0; y < h; ++y)
            {
                for (var x = 0; x < w - 10; ++x)
                {
                    // start of cell
                    if (colors[y*w + x] == stateDeepColor && colors[y*w + x + xOffset] == stateColor && colors[y*w + x + xOffset + 1] == stateColor && colors[y*w + x + xOffset + 2] == stateColor)
                    {
                        // start and end x
                        var sx = x + xOffset;
                        var ex = x + xOffset;
                        while (colors[y*w + sx] != stateDeepColor) --sx;
                        while (colors[y*w + ex] != stateDeepColor) ++ex;

                        var mx = (sx + ex)/2;

                        // start and end y of mid
                        var sy = y;
                        var ey = y;
                        while (colors[sy*w + mx] != stateDeepColor) --sy;
                        while (colors[ey*w + mx] != stateDeepColor) ++ey;

                        // cell center
                        var my = (sy + ey)/2;
                        var rad = (ey - sy)/2;

                        var center = new ivec2(mx, my);

                        // new cell found
                        if (Cells.All(c => ivec2.Distance(c.Center, center) > rad))
                        {
                            var c = new Cell
                            {
                                State = state, Center = center, Radius = rad, Model = this
                            };
                            Cells.Add(c);
                        }

                        // set to end
                        x = ex;
                    }
                }
            }
        }

        public Bitmap Print(int w, int h)
        {
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                var f = new Font("Arial", 30, FontStyle.Bold);
                var fId = new Font("Arial", 16, FontStyle.Bold);

                foreach (var cell in Cells)
                {
                    Color color;

                    switch (cell.State)
                    {
                        case CellState.Black:
                            color = Color.Black;
                            break;

                        case CellState.Blue:
                            color = Color.FromArgb(6, 164, 235);
                            break;

                        case CellState.Yellow:
                            color = Color.FromArgb(255, 175, 41);
                            break;

                        default:
                            throw new InvalidOperationException("unknown state");
                    }

                    var bgpen = new Pen(Color.White, 5);
                    var pen = new Pen(color, 3);
                    g.DrawEllipse(bgpen, cell.Center.x - cell.Radius, cell.Center.y - cell.Radius, 2*cell.Radius + 1, 2*cell.Radius + 1);
                    g.DrawEllipse(pen, cell.Center.x - cell.Radius, cell.Center.y - cell.Radius, 2*cell.Radius + 1, 2*cell.Radius + 1);

                    foreach (var n in cell.AllNeighbors)
                    {
                        var dir = n.Center - cell.Center;
                        dir = dir*5/11;

                        g.DrawLine(Pens.Black, cell.Center.x, cell.Center.y, cell.Center.x + dir.x, cell.Center.y + dir.y);
                    }

                    {
                        var tsize = g.MeasureString(cell.InnerText, f);
                        g.DrawString(cell.InnerText, f, Brushes.Black, cell.Center.x - tsize.Width/2f + 1, cell.Center.y - tsize.Height/4f + 1);
                        g.DrawString(cell.InnerText, f, Brushes.Red, cell.Center.x - tsize.Width/2f, cell.Center.y - tsize.Height/4f);
                    }
                    {
                        var s = "#" + cell.Nr;
                        var tsize = g.MeasureString(s, fId);
                        g.DrawString(s, fId, Brushes.Black, cell.Center.x - tsize.Width/2f + 1, cell.Center.y - tsize.Height/1f + 1);
                        g.DrawString(s, fId, Brushes.Lime, cell.Center.x - tsize.Width/2f, cell.Center.y - tsize.Height/1f);
                    }
                }
            }
            return bmp;
        }
    }
}

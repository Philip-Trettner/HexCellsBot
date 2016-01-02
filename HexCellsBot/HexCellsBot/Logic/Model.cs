using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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
        public List<ColumnConstraint> ColumnConstraints = new List<ColumnConstraint>();

        public readonly List<SolveStep> Steps = new List<SolveStep>();

        public int RemainingCount;
        private static readonly Rectangle RemainingRectangle = new Rectangle(1741, 78, 155, 45);

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

            Tengine.SetVariable("classify_enable_learning", false);
            Tengine.SetVariable("classify_enable_adaptive_matcher", false);
            Tengine.SetVariable("tessedit_char_whitelist", "1234567890-{}?._");

            // remaining rect
            var bx = 0;
            var by = 0;
            var bh = 0;
            {
                for (var y = 0; y < h; ++y)
                    for (var x = 0; x < w; ++x)
                        if (colors[y * w + x] == ColorBlue)
                        {
                            if (x > bx)
                            {
                                bx = x;
                                by = y;
                            }
                        }
                while (colors[(by + bh) * w + bx] == ColorBlue)
                    ++bh;
            }

            var m = new Model();

            m.CheckCells(CellState.Yellow, ColorYellow, ColorDeepYellow, w, h, colors);
            m.CheckCells(CellState.Blue, ColorBlue, ColorDeepBlue, w, h, colors);
            m.CheckCells(CellState.Black, ColorBlack, ColorDeepBlack, w, h, colors);
            for (var i = 0; i < m.Cells.Count; ++i)
                m.Cells[i].Nr = i;

            m.ConnectNeigbors();

            m.CheckText(bmp);
            m.ReadRemaining(bmp, bx, by, bh);

            if (Debugger.IsAttached)
                m.CollectRules();
            else
            {
                try
                {
                    m.CollectRules();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error: " + e.Message);
                }
            }

            m.Solve();

            return m;
        }

        private void ReadRemaining(Bitmap bmp, int bx, int by, int bh)
        {
            var remainS = OCROf(bmp, bx - 150, by + bh - 45, 150, 45, 10000);
            RemainingCount = int.Parse(remainS);
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
            // add positive rules
            foreach (var nc in NumberConstraints.ToArray())
                changed |= AddRuleChecked(nc.PositiveRule);

            // special derivatives
            foreach (var nc in NumberConstraints.ToArray())
                foreach (var newc in nc.DerivativeRules)
                    changed |= AddRuleChecked(newc);

            foreach (var nc1 in NumberConstraints.ToArray())
                foreach (var nc2 in NumberConstraints.ToArray())
                {
                    if (nc1 == nc2)
                        continue;

                    // TODO: check with special types

                    // check if nc1 is subset of nc2
                    if (nc1.Type == ConstraintType.Equal && nc2.Type == ConstraintType.Equal)
                        if (nc1.IsStrictSubsetOf(nc2))
                            changed |= AddRuleChecked(nc2.Without(nc1));

                    // check cutting stuff
                    // WORKS WITH CONN AND NONCONN
                    if (nc1.HasCutWith(nc2))
                        changed |= AddRuleChecked(nc1.ProbeCutWith(nc2));
                }

            Console.WriteLine(NumberConstraints.Count + " Rules after inference");
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
            foreach (var cc in ColumnConstraints)
                NumberConstraints.Add(cc.Constraint);

            // Remaining cells
            if (RemainingCount < 6)
                NumberConstraints.Add(new NumberConstraint(RemainingCount, Cells.Where(c => c.State == CellState.Yellow), ConstraintType.Equal, null));

            // remove irrelevant ones
            NumberConstraints.RemoveAll(nc => !nc.IsRelevant);

            // infer rules
            while (RuleInference())
            {
            }

            // remove empty rules
            NumberConstraints.RemoveAll(nc => nc.Cells.Count == 0);
        }

        private string OCROf(Bitmap bmp, int sx, int sy, int w, int h, int excludeRadius)
        {
            if (sx < 0 || sy < 0)
                return "0";
            var subbmp = new Bitmap(w, h);
            for (var y = 0; y < h; ++y)
                for (var x = 0; x < w; ++x)
                {
                    var color = bmp.GetPixel(sx + x, sy + y);
                    var whiteDis = (255 - color.R) + (255 - color.G) + (255 - color.B);
                    whiteDis *= 1;
                    //if (whiteDis > 100) whiteDis = 255;
                    //else whiteDis = 0;
                    if (whiteDis > 255) whiteDis = 255;
                    var dx = x - w / 2;
                    var dy = y - h / 2;
                    if (dx * dx + dy * dy > excludeRadius * excludeRadius)
                        whiteDis = 255;
                    subbmp.SetPixel(x, y, Color.FromArgb(whiteDis, whiteDis, whiteDis));
                }
            subbmp.Save($"C:\\Temp\\ocr\\{sx + w / 2}-{sy + h / 2}.png");
            var page = Tengine.Process(subbmp, PageSegMode.SingleLine); //, new Rect(start.x, start.y, size.x, size.y));
            var s = page.GetText().Trim();
            if (page.GetMeanConfidence() < .1)
                s = "";

            page.Dispose();
            subbmp.Dispose();
            return s;
        }

        private class ColumnProposal
        {
            public ivec2 CellCenter;
            public ivec2 ColCenter;
            public Cell Seed;
            public int AngleInDeg;
            public int BlackCount;
        }

        private void CheckText(Bitmap bmp)
        {
            var colProps = new List<ColumnProposal>();

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

                c.InnerText = OCROf(bmp, start.x, start.y, size.x, size.y, c.Radius);
            }

            // Column text
            foreach (var c in Cells)
                for (var angle = -60; angle <= 60; angle += 60)
                {
                    var dir = new vec2(0, -c.Radius * 2.2f);
                    dir = vec2.FromAngle(dir.Angle + angle / 180.0 * Math.PI) * dir.Length;
                    var cp = c.Center + (ivec2)dir;

                    var cc = c.Center + (ivec2)(dir / 2.2f * 2.5f);

                    if (Cells.All(c2 => ivec2.Distance(c2.Center, cp) > c2.Radius))
                    {
                        var bc = 0;
                        const int dw = 10;
                        const int dh = 15;
                        for (int dy = -dh; dy <= dh; ++dy)
                            for (int dx = -dw; dx <= dw; ++dx)
                                if (bmp.GetPixel(cp.x + dx, cp.y + dy).GetBrightness() < .5)
                                    ++bc;

                        //using (var g = Graphics.FromImage(bmp))
                        //   g.DrawRectangle(Pens.White, cp.x - dw, cp.y - dh, 2 * dw + 1, 2 * dh + 1);

                        if (bc > 15)
                        {
                            var prop = new ColumnProposal
                            {
                                AngleInDeg = angle,
                                BlackCount = bc,
                                CellCenter = cc,
                                ColCenter = cp,
                                Seed = c
                            };
                            for (var i = 0; i < colProps.Count; ++i)
                                if (vec2.Distance(prop.CellCenter, colProps[i].CellCenter) < c.Radius)
                                {
                                    if (colProps[i].BlackCount < prop.BlackCount)
                                        colProps[i] = prop;

                                    prop = null;
                                    break;
                                }
                            if (prop != null)
                                colProps.Add(prop);
                        }
                    }
                }
            bmp.Save(@"C:\Temp\ocr\cols\test.png");

            foreach (var cp in colProps)
            {
                const int bw = 45;
                const int bh = 30;
                var subbmp = new Bitmap(bw, bh);
                using (var g = Graphics.FromImage(subbmp))
                {
                    g.TranslateTransform(subbmp.Width / 2f, subbmp.Height / 2f);
                    g.RotateTransform(-cp.AngleInDeg, MatrixOrder.Prepend);
                    g.TranslateTransform(-cp.ColCenter.x, -cp.ColCenter.y);
                    g.DrawImageUnscaled(bmp, 0, 0);
                }

                for (var y = 0; y < bh; ++y)
                    for (var x = 0; x < bw; ++x)
                    {
                        float b = subbmp.GetPixel(x, y).GetBrightness();
                        var v = (int)((b - .3) / (.6 - .3) * 255);
                        if (v < 0) v = 0;
                        if (v > 255) v = 255;
                        subbmp.SetPixel(x, y, Color.FromArgb(v, v, v));
                    }

                subbmp.Save(@"C:\Temp\ocr\cols\" + cp.ColCenter.x + "-" + cp.ColCenter.y + ".png");
                var page = Tengine.Process(subbmp, PageSegMode.SingleLine);

                var colText = page.GetText().Trim();
                if (page.GetMeanConfidence() < .1)
                    colText = "";
                page.Dispose();
                subbmp.Dispose();

                if (!string.IsNullOrEmpty(colText))
                    ColumnConstraints.Add(new ColumnConstraint(cp.Seed, cp.AngleInDeg, colText, this, cp.ColCenter));
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

                    if (ivec2.Distance(c1.Center, c2.Center) > (c1.Radius + c2.Radius) * 1.8f)
                        continue;

                    c1.AllNeighbors.Add(c2);
                    if (c1.AllNeighbors.Count > 6)
                        throw new InvalidOperationException("too many neighbors");

                    var dir = c2.Center - c1.Center;
                    var angle = Math.Atan2(dir.y, dir.x) / Math.PI * 180;

                    const int eps = 5;

                    if (Math.Abs(30 - angle) < eps)
                        c1.Neighbors[(int)CellNeighbor.BottomRight] = c2;
                    else if (Math.Abs(90 - angle) < eps)
                        c1.Neighbors[(int)CellNeighbor.Bottom] = c2;
                    else if (Math.Abs(150 - angle) < eps)
                        c1.Neighbors[(int)CellNeighbor.BottomLeft] = c2;
                    else if (Math.Abs(-150 - angle) < eps)
                        c1.Neighbors[(int)CellNeighbor.TopLeft] = c2;
                    else if (Math.Abs(-90 - angle) < eps)
                        c1.Neighbors[(int)CellNeighbor.Top] = c2;
                    else if (Math.Abs(-30 - angle) < eps)
                        c1.Neighbors[(int)CellNeighbor.TopRight] = c2;
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
                        if (c2.Neighbors[(i + 3) % 6] != c1)
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
                    if (colors[y * w + x] == stateDeepColor && colors[y * w + x + xOffset] == stateColor && colors[y * w + x + xOffset + 1] == stateColor && colors[y * w + x + xOffset + 2] == stateColor)
                    {
                        // start and end x
                        var sx = x + xOffset;
                        var ex = x + xOffset;
                        while (colors[y * w + sx] != stateDeepColor) --sx;
                        while (colors[y * w + ex] != stateDeepColor) ++ex;

                        var mx = (sx + ex) / 2;

                        // start and end y of mid
                        var sy = y;
                        var ey = y;
                        while (colors[sy * w + mx] != stateDeepColor) --sy;
                        while (colors[ey * w + mx] != stateDeepColor) ++ey;

                        // cell center
                        var my = (sy + ey) / 2;
                        var rad = (ey - sy) / 2;

                        var center = new ivec2(mx, my);

                        // new cell found
                        if (Cells.All(c => ivec2.Distance(c.Center, center) > rad))
                        {
                            var c = new Cell
                            {
                                State = state,
                                Center = center,
                                Radius = rad,
                                Model = this
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

                foreach (var cc in ColumnConstraints)
                {
                    var dir = vec2.FromAngle((-vec2.UnitY).Angle + cc.AngleInDeg / 180.0 * Math.PI) * 30;
                    var e = (ivec2)(cc.TextCenter - dir);
                    foreach (var c in cc.Cells)
                    {
                        var newe = c.Center;
                        g.DrawLine(new Pen(Color.FromArgb(100, 0, 0, 255), 9f), e.x, e.y, newe.x, newe.y);
                        e = newe;
                    }
                }

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
                    g.DrawEllipse(bgpen, cell.Center.x - cell.Radius, cell.Center.y - cell.Radius, 2 * cell.Radius + 1, 2 * cell.Radius + 1);
                    g.DrawEllipse(pen, cell.Center.x - cell.Radius, cell.Center.y - cell.Radius, 2 * cell.Radius + 1, 2 * cell.Radius + 1);

                    foreach (var n in cell.AllNeighbors)
                    {
                        var dir = n.Center - cell.Center;
                        dir = dir * 5 / 11;

                        g.DrawLine(Pens.Black, cell.Center.x, cell.Center.y, cell.Center.x + dir.x, cell.Center.y + dir.y);
                    }

                    {
                        var tsize = g.MeasureString(cell.InnerText, f);
                        g.DrawString(cell.InnerText, f, Brushes.Black, cell.Center.x - tsize.Width / 2f + 1, cell.Center.y - tsize.Height / 4f + 1);
                        g.DrawString(cell.InnerText, f, Brushes.Red, cell.Center.x - tsize.Width / 2f, cell.Center.y - tsize.Height / 4f);
                    }
                    {
                        var s = "#" + cell.Nr;
                        var tsize = g.MeasureString(s, fId);
                        g.DrawString(s, fId, Brushes.Black, cell.Center.x - tsize.Width / 2f + 1, cell.Center.y - tsize.Height / 1f + 1);
                        g.DrawString(s, fId, Brushes.Lime, cell.Center.x - tsize.Width / 2f, cell.Center.y - tsize.Height / 1f);
                    }
                }

                foreach (var cc in ColumnConstraints)
                {
                    var dir = vec2.FromAngle((-vec2.UnitY).Angle + cc.AngleInDeg / 180.0 * Math.PI) * 30;
                    var s = (ivec2) (cc.TextCenter + dir);
                    var e = (ivec2) (cc.TextCenter - dir);
                    g.DrawLine(Pens.Blue, s.x, s.y, e.x, e.y);

                    {
                        var tsize = g.MeasureString(cc.Text, f);
                        g.DrawString(cc.Text, f, Brushes.Black, cc.TextCenter.x - tsize.Width / 2f + 1, cc.TextCenter.y - tsize.Height / 4f + 1);
                        g.DrawString(cc.Text, f, Brushes.Blue, cc.TextCenter.x - tsize.Width / 2f, cc.TextCenter.y - tsize.Height / 4f);
                    }
                }
            }
            return bmp;
        }
    }
}

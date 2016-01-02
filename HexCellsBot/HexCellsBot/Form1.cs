using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HexCellsBot.Logic;

namespace HexCellsBot
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        // For Windows Mobile, replace user32.dll with coredll.dll
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]

        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoLeftMouseClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        public void DoRightMouseClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
        }

        private IntPtr Hwnd;

        public static Bitmap PrintWindow(IntPtr hwnd)
        {
            RECT rc;
            GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateModel()
        {
            Hwnd = FindWindow(null, "Hexcells Infinite");
            if (Hwnd == IntPtr.Zero)
            {
                Text = "HexCells Bot [PROCESS NOT FOUND]";
                return;
            }

            Text = $"HexCells Bot [CONNECTED {Hwnd}]";

            // update bg image
            var bmp = PrintWindow(Hwnd);
            pbCapture.Image?.Dispose();
            pbCapture.Image = bmp;
            pbModel.BackgroundImage = cbBG.Checked ? bmp : null;

            // create model
            var m = Model.Analyze(bmp);
            pbModel.Image?.Dispose();
            pbModel.Image = m.Print(bmp.Width, bmp.Height);

            // rules
            lbRules.Items.Clear();
            lbRules.Items.AddRange(m.NumberConstraints.Cast<object>().ToArray());

            // solving
            lbSolver.Items.Clear();
            lbSolver.Items.AddRange(m.Steps.Cast<object>().ToArray());
        }

        private void tTick_Tick(object sender, EventArgs e)
        {
            tTick.Enabled = false;
            UpdateModel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          //  UpdateModel();
        }

        private void cbBG_CheckedChanged(object sender, EventArgs e)
        {
            pbModel.BackgroundImage = cbBG.Checked ? pbCapture.Image : null;
        }

        private void captureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateModel();
        }

        public bool SolveStep(SolveStep step)
        {
            var shouldWait = false;
            var savPos = Cursor.Position;
            RECT rc;
            GetWindowRect(Hwnd, out rc);
            Thread.Sleep(100);
            Cursor.Position = new Point(rc.Left + step.Cell.Center.x, rc.Top + step.Cell.Center.y);
            switch (step.NewState)
            {
                case CellState.Blue:
                    DoLeftMouseClick();
                    break;

                case CellState.Black:
                    shouldWait = true;
                    DoRightMouseClick();
                    break;
            }

            Cursor.Position = savPos;
            return shouldWait;
        }

        public bool SolveSteps()
        {
            var shouldWait = false;

            foreach (SolveStep step in lbSolver.Items)
                shouldWait |= SolveStep(step);

            return shouldWait;
        }

        private void applyStepsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Hwnd == IntPtr.Zero)
                return;
            BringWindowToTop(Hwnd);

            SolveSteps();
        }

        private void solveContinuouslyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Hwnd == IntPtr.Zero)
                return;
            BringWindowToTop(Hwnd);

            while (lbSolver.Items.Count > 0)
            {
                if (SolveSteps())
                    Thread.Sleep(500);
                else Thread.Sleep(200);

                UpdateModel();
                Application.DoEvents();
                
            }
        }

        private void applyAndRecaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Hwnd == IntPtr.Zero)
                return;
            BringWindowToTop(Hwnd);

            if (SolveSteps())
                Thread.Sleep(1500);
            else Thread.Sleep(200);

            UpdateModel();
        }

        private void applyOneStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Hwnd == IntPtr.Zero)
                return;
            BringWindowToTop(Hwnd);

            //SolveStep(1);

            if (lbSolver.Items.Count > 0)
            {
                var step = lbSolver.Items[0] as SolveStep;
                SolveStep(step);
                lbSolver.Items.RemoveAt(0);
            }
        }
    }
}

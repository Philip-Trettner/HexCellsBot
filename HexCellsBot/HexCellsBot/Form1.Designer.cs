namespace HexCellsBot
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpModel = new System.Windows.Forms.TabPage();
            this.tpRules = new System.Windows.Forms.TabPage();
            this.pbModel = new System.Windows.Forms.PictureBox();
            this.lbRules = new System.Windows.Forms.ListBox();
            this.tpSolver = new System.Windows.Forms.TabPage();
            this.tTick = new System.Windows.Forms.Timer(this.components);
            this.tpCapture = new System.Windows.Forms.TabPage();
            this.pbCapture = new System.Windows.Forms.PictureBox();
            this.cbBG = new System.Windows.Forms.CheckBox();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.captureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbSolver = new System.Windows.Forms.ListBox();
            this.applyStepsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solveContinuouslyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyAndRecaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tpModel.SuspendLayout();
            this.tpRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbModel)).BeginInit();
            this.tpSolver.SuspendLayout();
            this.tpCapture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).BeginInit();
            this.msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpCapture);
            this.tabControl1.Controls.Add(this.tpModel);
            this.tabControl1.Controls.Add(this.tpRules);
            this.tabControl1.Controls.Add(this.tpSolver);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1422, 769);
            this.tabControl1.TabIndex = 0;
            // 
            // tpModel
            // 
            this.tpModel.Controls.Add(this.cbBG);
            this.tpModel.Controls.Add(this.pbModel);
            this.tpModel.Location = new System.Drawing.Point(4, 22);
            this.tpModel.Name = "tpModel";
            this.tpModel.Padding = new System.Windows.Forms.Padding(3);
            this.tpModel.Size = new System.Drawing.Size(1414, 743);
            this.tpModel.TabIndex = 0;
            this.tpModel.Text = "Model";
            this.tpModel.UseVisualStyleBackColor = true;
            // 
            // tpRules
            // 
            this.tpRules.Controls.Add(this.lbRules);
            this.tpRules.Location = new System.Drawing.Point(4, 22);
            this.tpRules.Name = "tpRules";
            this.tpRules.Padding = new System.Windows.Forms.Padding(3);
            this.tpRules.Size = new System.Drawing.Size(824, 612);
            this.tpRules.TabIndex = 1;
            this.tpRules.Text = "Rules";
            this.tpRules.UseVisualStyleBackColor = true;
            // 
            // pbModel
            // 
            this.pbModel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbModel.Location = new System.Drawing.Point(3, 3);
            this.pbModel.Name = "pbModel";
            this.pbModel.Size = new System.Drawing.Size(1408, 737);
            this.pbModel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbModel.TabIndex = 0;
            this.pbModel.TabStop = false;
            // 
            // lbRules
            // 
            this.lbRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRules.FormattingEnabled = true;
            this.lbRules.Location = new System.Drawing.Point(3, 3);
            this.lbRules.Name = "lbRules";
            this.lbRules.Size = new System.Drawing.Size(818, 606);
            this.lbRules.TabIndex = 0;
            // 
            // tpSolver
            // 
            this.tpSolver.Controls.Add(this.lbSolver);
            this.tpSolver.Location = new System.Drawing.Point(4, 22);
            this.tpSolver.Name = "tpSolver";
            this.tpSolver.Padding = new System.Windows.Forms.Padding(3);
            this.tpSolver.Size = new System.Drawing.Size(1414, 743);
            this.tpSolver.TabIndex = 2;
            this.tpSolver.Text = "Solver";
            this.tpSolver.UseVisualStyleBackColor = true;
            // 
            // tTick
            // 
            this.tTick.Enabled = true;
            this.tTick.Tick += new System.EventHandler(this.tTick_Tick);
            // 
            // tpCapture
            // 
            this.tpCapture.Controls.Add(this.pbCapture);
            this.tpCapture.Location = new System.Drawing.Point(4, 22);
            this.tpCapture.Name = "tpCapture";
            this.tpCapture.Padding = new System.Windows.Forms.Padding(3);
            this.tpCapture.Size = new System.Drawing.Size(1414, 767);
            this.tpCapture.TabIndex = 3;
            this.tpCapture.Text = "Capture";
            this.tpCapture.UseVisualStyleBackColor = true;
            // 
            // pbCapture
            // 
            this.pbCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCapture.Location = new System.Drawing.Point(3, 3);
            this.pbCapture.Name = "pbCapture";
            this.pbCapture.Size = new System.Drawing.Size(1408, 761);
            this.pbCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCapture.TabIndex = 0;
            this.pbCapture.TabStop = false;
            // 
            // cbBG
            // 
            this.cbBG.AutoSize = true;
            this.cbBG.Location = new System.Drawing.Point(8, 6);
            this.cbBG.Name = "cbBG";
            this.cbBG.Size = new System.Drawing.Size(71, 17);
            this.cbBG.TabIndex = 1;
            this.cbBG.Text = "Show BG";
            this.cbBG.UseVisualStyleBackColor = true;
            this.cbBG.CheckedChanged += new System.EventHandler(this.cbBG_CheckedChanged);
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureToolStripMenuItem,
            this.applyStepsToolStripMenuItem,
            this.solveContinuouslyToolStripMenuItem,
            this.applyAndRecaptureToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(1422, 24);
            this.msMain.TabIndex = 1;
            this.msMain.Text = "menuStrip1";
            // 
            // captureToolStripMenuItem
            // 
            this.captureToolStripMenuItem.Name = "captureToolStripMenuItem";
            this.captureToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.captureToolStripMenuItem.Text = "Capture";
            this.captureToolStripMenuItem.Click += new System.EventHandler(this.captureToolStripMenuItem_Click);
            // 
            // lbSolver
            // 
            this.lbSolver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSolver.FormattingEnabled = true;
            this.lbSolver.Location = new System.Drawing.Point(3, 3);
            this.lbSolver.Name = "lbSolver";
            this.lbSolver.Size = new System.Drawing.Size(1408, 737);
            this.lbSolver.TabIndex = 0;
            // 
            // applyStepsToolStripMenuItem
            // 
            this.applyStepsToolStripMenuItem.Name = "applyStepsToolStripMenuItem";
            this.applyStepsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.applyStepsToolStripMenuItem.Text = "Apply Steps";
            this.applyStepsToolStripMenuItem.Click += new System.EventHandler(this.applyStepsToolStripMenuItem_Click);
            // 
            // solveContinuouslyToolStripMenuItem
            // 
            this.solveContinuouslyToolStripMenuItem.Name = "solveContinuouslyToolStripMenuItem";
            this.solveContinuouslyToolStripMenuItem.Size = new System.Drawing.Size(121, 20);
            this.solveContinuouslyToolStripMenuItem.Text = "Solve Continuously";
            this.solveContinuouslyToolStripMenuItem.Click += new System.EventHandler(this.solveContinuouslyToolStripMenuItem_Click);
            // 
            // applyAndRecaptureToolStripMenuItem
            // 
            this.applyAndRecaptureToolStripMenuItem.Name = "applyAndRecaptureToolStripMenuItem";
            this.applyAndRecaptureToolStripMenuItem.Size = new System.Drawing.Size(129, 20);
            this.applyAndRecaptureToolStripMenuItem.Text = "Apply and Recapture";
            this.applyAndRecaptureToolStripMenuItem.Click += new System.EventHandler(this.applyAndRecaptureToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1422, 793);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.msMain);
            this.MainMenuStrip = this.msMain;
            this.Name = "Form1";
            this.Text = "HexCells Bot";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpModel.ResumeLayout(false);
            this.tpModel.PerformLayout();
            this.tpRules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbModel)).EndInit();
            this.tpSolver.ResumeLayout(false);
            this.tpCapture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).EndInit();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpModel;
        private System.Windows.Forms.PictureBox pbModel;
        private System.Windows.Forms.TabPage tpRules;
        private System.Windows.Forms.ListBox lbRules;
        private System.Windows.Forms.TabPage tpSolver;
        private System.Windows.Forms.Timer tTick;
        private System.Windows.Forms.TabPage tpCapture;
        private System.Windows.Forms.PictureBox pbCapture;
        private System.Windows.Forms.CheckBox cbBG;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem captureToolStripMenuItem;
        private System.Windows.Forms.ListBox lbSolver;
        private System.Windows.Forms.ToolStripMenuItem applyStepsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solveContinuouslyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyAndRecaptureToolStripMenuItem;
    }
}


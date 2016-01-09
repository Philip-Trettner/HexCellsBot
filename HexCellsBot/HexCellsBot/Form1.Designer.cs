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
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tpCapture = new System.Windows.Forms.TabPage();
            this.pbCapture = new System.Windows.Forms.PictureBox();
            this.tpModel = new System.Windows.Forms.TabPage();
            this.cbBG = new System.Windows.Forms.CheckBox();
            this.tTick = new System.Windows.Forms.Timer(this.components);
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.captureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyStepsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solveContinuouslyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyAndRecaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyOneStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pbModel = new System.Windows.Forms.PictureBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lStatusTiming = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvRules = new System.Windows.Forms.ListView();
            this.lvSteps = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tcTabs.SuspendLayout();
            this.tpCapture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).BeginInit();
            this.tpModel.SuspendLayout();
            this.msMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcTabs
            // 
            this.tcTabs.Controls.Add(this.tpCapture);
            this.tcTabs.Controls.Add(this.tpModel);
            this.tcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTabs.Location = new System.Drawing.Point(0, 24);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new System.Drawing.Size(1422, 769);
            this.tcTabs.TabIndex = 1;
            // 
            // tpCapture
            // 
            this.tpCapture.Controls.Add(this.pbCapture);
            this.tpCapture.Location = new System.Drawing.Point(4, 22);
            this.tpCapture.Name = "tpCapture";
            this.tpCapture.Padding = new System.Windows.Forms.Padding(3);
            this.tpCapture.Size = new System.Drawing.Size(1414, 743);
            this.tpCapture.TabIndex = 3;
            this.tpCapture.Text = "Capture";
            this.tpCapture.UseVisualStyleBackColor = true;
            // 
            // pbCapture
            // 
            this.pbCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCapture.Location = new System.Drawing.Point(3, 3);
            this.pbCapture.Name = "pbCapture";
            this.pbCapture.Size = new System.Drawing.Size(1408, 737);
            this.pbCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCapture.TabIndex = 0;
            this.pbCapture.TabStop = false;
            // 
            // tpModel
            // 
            this.tpModel.Controls.Add(this.splitContainer1);
            this.tpModel.Location = new System.Drawing.Point(4, 22);
            this.tpModel.Name = "tpModel";
            this.tpModel.Padding = new System.Windows.Forms.Padding(3);
            this.tpModel.Size = new System.Drawing.Size(1414, 743);
            this.tpModel.TabIndex = 0;
            this.tpModel.Text = "Model";
            this.tpModel.UseVisualStyleBackColor = true;
            // 
            // cbBG
            // 
            this.cbBG.AutoSize = true;
            this.cbBG.Location = new System.Drawing.Point(3, 3);
            this.cbBG.Name = "cbBG";
            this.cbBG.Size = new System.Drawing.Size(71, 17);
            this.cbBG.TabIndex = 1;
            this.cbBG.Text = "Show BG";
            this.cbBG.UseVisualStyleBackColor = true;
            this.cbBG.CheckedChanged += new System.EventHandler(this.cbBG_CheckedChanged);
            // 
            // tTick
            // 
            this.tTick.Enabled = true;
            this.tTick.Tick += new System.EventHandler(this.tTick_Tick);
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureToolStripMenuItem,
            this.applyStepsToolStripMenuItem,
            this.solveContinuouslyToolStripMenuItem,
            this.applyAndRecaptureToolStripMenuItem,
            this.applyOneStepToolStripMenuItem});
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
            // applyOneStepToolStripMenuItem
            // 
            this.applyOneStepToolStripMenuItem.Name = "applyOneStepToolStripMenuItem";
            this.applyOneStepToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.applyOneStepToolStripMenuItem.Text = "Apply One Step";
            this.applyOneStepToolStripMenuItem.Click += new System.EventHandler(this.applyOneStepToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cbBG);
            this.splitContainer1.Panel1.Controls.Add(this.pbModel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1408, 737);
            this.splitContainer1.SplitterDistance = 695;
            this.splitContainer1.TabIndex = 2;
            // 
            // pbModel
            // 
            this.pbModel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbModel.Location = new System.Drawing.Point(0, 0);
            this.pbModel.Name = "pbModel";
            this.pbModel.Size = new System.Drawing.Size(695, 737);
            this.pbModel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbModel.TabIndex = 1;
            this.pbModel.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(709, 737);
            this.splitContainer2.SplitterDistance = 353;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvRules);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(709, 353);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rules";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvSteps);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(709, 380);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Solver";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lStatusTiming});
            this.statusStrip1.Location = new System.Drawing.Point(0, 771);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1422, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lStatusTiming
            // 
            this.lStatusTiming.Name = "lStatusTiming";
            this.lStatusTiming.Size = new System.Drawing.Size(138, 17);
            this.lStatusTiming.Text = "XXX ms for last inference";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1269, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // lvRules
            // 
            this.lvRules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader9,
            this.columnHeader4,
            this.columnHeader5});
            this.lvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRules.Location = new System.Drawing.Point(3, 16);
            this.lvRules.Name = "lvRules";
            this.lvRules.Size = new System.Drawing.Size(703, 334);
            this.lvRules.TabIndex = 3;
            this.lvRules.UseCompatibleStateImageBehavior = false;
            this.lvRules.View = System.Windows.Forms.View.Details;
            // 
            // lvSteps
            // 
            this.lvSteps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lvSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSteps.Location = new System.Drawing.Point(3, 16);
            this.lvSteps.Name = "lvSteps";
            this.lvSteps.Size = new System.Drawing.Size(703, 361);
            this.lvSteps.TabIndex = 0;
            this.lvSteps.UseCompatibleStateImageBehavior = false;
            this.lvSteps.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Gen";
            this.columnHeader2.Width = 35;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Constraint";
            this.columnHeader3.Width = 130;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Cells";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 200;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Extra Info";
            this.columnHeader5.Width = 200;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Cell";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "State";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Explanation";
            this.columnHeader8.Width = 400;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "#Cells";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1422, 793);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tcTabs);
            this.Controls.Add(this.msMain);
            this.MainMenuStrip = this.msMain;
            this.Name = "Form1";
            this.Text = "HexCells Bot";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tcTabs.ResumeLayout(false);
            this.tpCapture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCapture)).EndInit();
            this.tpModel.ResumeLayout(false);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbModel)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcTabs;
        private System.Windows.Forms.TabPage tpModel;
        private System.Windows.Forms.Timer tTick;
        private System.Windows.Forms.TabPage tpCapture;
        private System.Windows.Forms.PictureBox pbCapture;
        private System.Windows.Forms.CheckBox cbBG;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem captureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyStepsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solveContinuouslyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyAndRecaptureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyOneStepToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pbModel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lStatusTiming;
        private System.Windows.Forms.ListView lvRules;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ListView lvSteps;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
    }
}


namespace GoogleScholarParser
{
    partial class MainForm
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
            this.textBoxRequest = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.statusStripResponse = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelResponse = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelProxy = new System.Windows.Forms.ToolStripStatusLabel();
            this.numericUpDownCount = new System.Windows.Forms.NumericUpDown();
            this.labelCount = new System.Windows.Forms.Label();
            this.checkBoxCount = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemData = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpenRequests = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripResponse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxRequest
            // 
            this.textBoxRequest.Location = new System.Drawing.Point(12, 27);
            this.textBoxRequest.Name = "textBoxRequest";
            this.textBoxRequest.Size = new System.Drawing.Size(401, 20);
            this.textBoxRequest.TabIndex = 0;
            this.textBoxRequest.Text = "Сварка взрывом";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(603, 25);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // statusStripResponse
            // 
            this.statusStripResponse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelResponse,
            this.toolStripStatusLabelProxy});
            this.statusStripResponse.Location = new System.Drawing.Point(0, 240);
            this.statusStripResponse.Name = "statusStripResponse";
            this.statusStripResponse.Size = new System.Drawing.Size(691, 22);
            this.statusStripResponse.TabIndex = 3;
            this.statusStripResponse.Text = "statusStrip1";
            // 
            // toolStripStatusLabelResponse
            // 
            this.toolStripStatusLabelResponse.Name = "toolStripStatusLabelResponse";
            this.toolStripStatusLabelResponse.Size = new System.Drawing.Size(50, 17);
            this.toolStripStatusLabelResponse.Text = "Loading";
            // 
            // toolStripStatusLabelProxy
            // 
            this.toolStripStatusLabelProxy.Name = "toolStripStatusLabelProxy";
            this.toolStripStatusLabelProxy.Size = new System.Drawing.Size(13, 17);
            this.toolStripStatusLabelProxy.Text = "0";
            // 
            // numericUpDownCount
            // 
            this.numericUpDownCount.Location = new System.Drawing.Point(541, 28);
            this.numericUpDownCount.Name = "numericUpDownCount";
            this.numericUpDownCount.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownCount.TabIndex = 4;
            this.numericUpDownCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(495, 30);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(40, 13);
            this.labelCount.TabIndex = 5;
            this.labelCount.Text = "Pages:";
            // 
            // checkBoxCount
            // 
            this.checkBoxCount.AutoSize = true;
            this.checkBoxCount.Location = new System.Drawing.Point(419, 29);
            this.checkBoxCount.Name = "checkBoxCount";
            this.checkBoxCount.Size = new System.Drawing.Size(70, 17);
            this.checkBoxCount.TabIndex = 6;
            this.checkBoxCount.Text = "All Pages";
            this.checkBoxCount.UseVisualStyleBackColor = true;
            this.checkBoxCount.CheckedChanged += new System.EventHandler(this.checkBoxCount_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemData,
            this.toolStripMenuItemSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(691, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemData
            // 
            this.toolStripMenuItemData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadCSVToolStripMenuItem,
            this.toolStripMenuItemOpenRequests});
            this.toolStripMenuItemData.Name = "toolStripMenuItemData";
            this.toolStripMenuItemData.Size = new System.Drawing.Size(43, 20);
            this.toolStripMenuItemData.Text = "Data";
            // 
            // loadCSVToolStripMenuItem
            // 
            this.loadCSVToolStripMenuItem.Name = "loadCSVToolStripMenuItem";
            this.loadCSVToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.loadCSVToolStripMenuItem.Text = "Load CSV";
            this.loadCSVToolStripMenuItem.Click += new System.EventHandler(this.loadCSVToolStripMenuItem_Click);
            // 
            // toolStripMenuItemSettings
            // 
            this.toolStripMenuItemSettings.Name = "toolStripMenuItemSettings";
            this.toolStripMenuItemSettings.Size = new System.Drawing.Size(61, 20);
            this.toolStripMenuItemSettings.Text = "Settings";
            this.toolStripMenuItemSettings.Click += new System.EventHandler(this.toolStripMenuItemSettings_Click);
            // 
            // toolStripMenuItemOpenRequests
            // 
            this.toolStripMenuItemOpenRequests.Name = "toolStripMenuItemOpenRequests";
            this.toolStripMenuItemOpenRequests.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItemOpenRequests.Text = "Open Requests";
            this.toolStripMenuItemOpenRequests.Click += new System.EventHandler(this.toolStripMenuItemOpenRequests_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 262);
            this.Controls.Add(this.checkBoxCount);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.numericUpDownCount);
            this.Controls.Add(this.statusStripResponse);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxRequest);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Google Scholar Parser";
            this.statusStripResponse.ResumeLayout(false);
            this.statusStripResponse.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxRequest;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.StatusStrip statusStripResponse;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelResponse;
        private System.Windows.Forms.NumericUpDown numericUpDownCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.CheckBox checkBoxCount;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemData;
        private System.Windows.Forms.ToolStripMenuItem loadCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProxy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenRequests;
    }
}
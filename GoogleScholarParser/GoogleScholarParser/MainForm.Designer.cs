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
            this.buttonFind = new System.Windows.Forms.Button();
            this.listViewResult = new System.Windows.Forms.ListView();
            this.statusStripResponse = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelResponse = new System.Windows.Forms.ToolStripStatusLabel();
            this.numericUpDownCount = new System.Windows.Forms.NumericUpDown();
            this.labelCount = new System.Windows.Forms.Label();
            this.checkBoxCount = new System.Windows.Forms.CheckBox();
            this.statusStripResponse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxRequest
            // 
            this.textBoxRequest.Location = new System.Drawing.Point(13, 13);
            this.textBoxRequest.Name = "textBoxRequest";
            this.textBoxRequest.Size = new System.Drawing.Size(401, 20);
            this.textBoxRequest.TabIndex = 0;
            this.textBoxRequest.Text = "Сварка взрывом";
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(604, 11);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 1;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // listViewResult
            // 
            this.listViewResult.Location = new System.Drawing.Point(13, 40);
            this.listViewResult.Name = "listViewResult";
            this.listViewResult.Size = new System.Drawing.Size(666, 197);
            this.listViewResult.TabIndex = 2;
            this.listViewResult.UseCompatibleStateImageBehavior = false;
            // 
            // statusStripResponse
            // 
            this.statusStripResponse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelResponse});
            this.statusStripResponse.Location = new System.Drawing.Point(0, 240);
            this.statusStripResponse.Name = "statusStripResponse";
            this.statusStripResponse.Size = new System.Drawing.Size(691, 22);
            this.statusStripResponse.TabIndex = 3;
            this.statusStripResponse.Text = "statusStrip1";
            // 
            // toolStripStatusLabelResponse
            // 
            this.toolStripStatusLabelResponse.Name = "toolStripStatusLabelResponse";
            this.toolStripStatusLabelResponse.Size = new System.Drawing.Size(31, 17);
            this.toolStripStatusLabelResponse.Text = "Start";
            // 
            // numericUpDownCount
            // 
            this.numericUpDownCount.Location = new System.Drawing.Point(542, 14);
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
            this.labelCount.Location = new System.Drawing.Point(496, 16);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(40, 13);
            this.labelCount.TabIndex = 5;
            this.labelCount.Text = "Pages:";
            // 
            // checkBoxCount
            // 
            this.checkBoxCount.AutoSize = true;
            this.checkBoxCount.Location = new System.Drawing.Point(420, 15);
            this.checkBoxCount.Name = "checkBoxCount";
            this.checkBoxCount.Size = new System.Drawing.Size(70, 17);
            this.checkBoxCount.TabIndex = 6;
            this.checkBoxCount.Text = "All Pages";
            this.checkBoxCount.UseVisualStyleBackColor = true;
            this.checkBoxCount.CheckedChanged += new System.EventHandler(this.checkBoxCount_CheckedChanged);
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
            this.Controls.Add(this.listViewResult);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.textBoxRequest);
            this.Name = "MainForm";
            this.Text = "Google Scholar Parser";
            this.statusStripResponse.ResumeLayout(false);
            this.statusStripResponse.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxRequest;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.ListView listViewResult;
        private System.Windows.Forms.StatusStrip statusStripResponse;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelResponse;
        private System.Windows.Forms.NumericUpDown numericUpDownCount;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.CheckBox checkBoxCount;
    }
}
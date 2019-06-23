namespace BIMtrovert.BS_Customs
{
    partial class ParameterSearcher
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
            this.categoryBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.paramText = new System.Windows.Forms.TextBox();
            this.searchText = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.viewBtn = new System.Windows.Forms.RadioButton();
            this.projectBtn = new System.Windows.Forms.RadioButton();
            this.partialMatch = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // categoryBox
            // 
            this.categoryBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryBox.FormattingEnabled = true;
            this.categoryBox.Location = new System.Drawing.Point(12, 52);
            this.categoryBox.Name = "categoryBox";
            this.categoryBox.Size = new System.Drawing.Size(404, 28);
            this.categoryBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Revit Category";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Parameter Name";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Search Term";
            // 
            // paramText
            // 
            this.paramText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.paramText.Location = new System.Drawing.Point(12, 132);
            this.paramText.Name = "paramText";
            this.paramText.Size = new System.Drawing.Size(404, 26);
            this.paramText.TabIndex = 1;
            // 
            // searchText
            // 
            this.searchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchText.Location = new System.Drawing.Point(12, 198);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(404, 26);
            this.searchText.TabIndex = 2;
            this.searchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyDown);
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.searchBtn.Location = new System.Drawing.Point(12, 261);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(101, 51);
            this.searchBtn.TabIndex = 5;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.Location = new System.Drawing.Point(315, 261);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(101, 51);
            this.cancelBtn.TabIndex = 6;
            this.cancelBtn.Text = "Close";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // viewBtn
            // 
            this.viewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewBtn.AutoSize = true;
            this.viewBtn.Checked = true;
            this.viewBtn.Location = new System.Drawing.Point(136, 261);
            this.viewBtn.Name = "viewBtn";
            this.viewBtn.Size = new System.Drawing.Size(103, 24);
            this.viewBtn.TabIndex = 3;
            this.viewBtn.TabStop = true;
            this.viewBtn.Text = "View Only";
            this.viewBtn.UseVisualStyleBackColor = true;
            this.viewBtn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.viewBtn_KeyDown);
            // 
            // projectBtn
            // 
            this.projectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectBtn.AutoSize = true;
            this.projectBtn.Location = new System.Drawing.Point(136, 291);
            this.projectBtn.Name = "projectBtn";
            this.projectBtn.Size = new System.Drawing.Size(129, 24);
            this.projectBtn.TabIndex = 11;
            this.projectBtn.Text = "Entire Project";
            this.projectBtn.UseVisualStyleBackColor = true;
            // 
            // partialMatch
            // 
            this.partialMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.partialMatch.AutoSize = true;
            this.partialMatch.Location = new System.Drawing.Point(12, 336);
            this.partialMatch.Name = "partialMatch";
            this.partialMatch.Size = new System.Drawing.Size(204, 24);
            this.partialMatch.TabIndex = 4;
            this.partialMatch.Text = "Search for partial match";
            this.partialMatch.UseVisualStyleBackColor = true;
            this.partialMatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.partialMatch_KeyDown);
            // 
            // ParameterSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 384);
            this.ControlBox = false;
            this.Controls.Add(this.viewBtn);
            this.Controls.Add(this.projectBtn);
            this.Controls.Add(this.partialMatch);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.paramText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.categoryBox);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 440);
            this.Name = "ParameterSearcher";
            this.Text = "Parameter Searcher";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ParameterSearcher_KeyDown_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox categoryBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox paramText;
        private System.Windows.Forms.TextBox searchText;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.RadioButton viewBtn;
        private System.Windows.Forms.RadioButton projectBtn;
        private System.Windows.Forms.CheckBox partialMatch;
    }
}
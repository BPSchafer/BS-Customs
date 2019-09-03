namespace BIMtrovert.BS_Customs
{
    partial class SearchByCategoryForm
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
            this.catList = new System.Windows.Forms.CheckedListBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.filterBtn = new System.Windows.Forms.Button();
            this.ViewRadio = new System.Windows.Forms.RadioButton();
            this.ProjRadio = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // catList
            // 
            this.catList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.catList.CheckOnClick = true;
            this.catList.FormattingEnabled = true;
            this.catList.Location = new System.Drawing.Point(13, 13);
            this.catList.Name = "catList";
            this.catList.Size = new System.Drawing.Size(775, 382);
            this.catList.TabIndex = 0;
            this.catList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.catList_KeyDown);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.Location = new System.Drawing.Point(668, 401);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(120, 40);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // filterBtn
            // 
            this.filterBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.filterBtn.Location = new System.Drawing.Point(13, 401);
            this.filterBtn.Name = "filterBtn";
            this.filterBtn.Size = new System.Drawing.Size(120, 40);
            this.filterBtn.TabIndex = 1;
            this.filterBtn.Text = "Select";
            this.filterBtn.UseVisualStyleBackColor = true;
            this.filterBtn.Click += new System.EventHandler(this.filterBtn_Click);
            // 
            // ViewRadio
            // 
            this.ViewRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ViewRadio.AutoSize = true;
            this.ViewRadio.Checked = true;
            this.ViewRadio.Location = new System.Drawing.Point(315, 409);
            this.ViewRadio.Name = "ViewRadio";
            this.ViewRadio.Size = new System.Drawing.Size(68, 24);
            this.ViewRadio.TabIndex = 2;
            this.ViewRadio.TabStop = true;
            this.ViewRadio.Text = "View";
            this.ViewRadio.UseVisualStyleBackColor = true;
            // 
            // ProjRadio
            // 
            this.ProjRadio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ProjRadio.AutoSize = true;
            this.ProjRadio.Location = new System.Drawing.Point(389, 409);
            this.ProjRadio.Name = "ProjRadio";
            this.ProjRadio.Size = new System.Drawing.Size(83, 24);
            this.ProjRadio.TabIndex = 3;
            this.ProjRadio.Text = "Project";
            this.ProjRadio.UseVisualStyleBackColor = true;
            // 
            // SearchByCategoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ProjRadio);
            this.Controls.Add(this.ViewRadio);
            this.Controls.Add(this.filterBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.catList);
            this.Name = "SearchByCategoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search By Category";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox catList;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button filterBtn;
        private System.Windows.Forms.RadioButton ViewRadio;
        private System.Windows.Forms.RadioButton ProjRadio;
    }
}
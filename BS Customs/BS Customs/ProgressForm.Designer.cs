namespace BIMtrovert.BS_Customs
{
    partial class ProgressForm
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
            this.globalLabel = new System.Windows.Forms.Label();
            this.localLabel = new System.Windows.Forms.Label();
            this.localProgress = new System.Windows.Forms.ProgressBar();
            this.globalProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // globalLabel
            // 
            this.globalLabel.AutoSize = true;
            this.globalLabel.Location = new System.Drawing.Point(15, 117);
            this.globalLabel.Name = "globalLabel";
            this.globalLabel.Size = new System.Drawing.Size(0, 20);
            this.globalLabel.TabIndex = 1;
            // 
            // localLabel
            // 
            this.localLabel.AutoSize = true;
            this.localLabel.Location = new System.Drawing.Point(15, 31);
            this.localLabel.Name = "localLabel";
            this.localLabel.Size = new System.Drawing.Size(0, 20);
            this.localLabel.TabIndex = 1;
            // 
            // localProgress
            // 
            this.localProgress.Location = new System.Drawing.Point(12, 74);
            this.localProgress.Name = "localProgress";
            this.localProgress.Size = new System.Drawing.Size(639, 23);
            this.localProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.localProgress.TabIndex = 2;
            // 
            // globalProgress
            // 
            this.globalProgress.Location = new System.Drawing.Point(12, 159);
            this.globalProgress.Name = "globalProgress";
            this.globalProgress.Size = new System.Drawing.Size(639, 23);
            this.globalProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.globalProgress.TabIndex = 3;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 212);
            this.Controls.Add(this.globalProgress);
            this.Controls.Add(this.localProgress);
            this.Controls.Add(this.localLabel);
            this.Controls.Add(this.globalLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Processing";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label globalLabel;
        private System.Windows.Forms.Label localLabel;
        private System.Windows.Forms.ProgressBar localProgress;
        private System.Windows.Forms.ProgressBar globalProgress;
    }
}
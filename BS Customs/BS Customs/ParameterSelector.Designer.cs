namespace BIMtrovert.BS_Customs
{
    partial class ParameterSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterSelector));
            this.filterButton = new System.Windows.Forms.Button();
            this.filterBox = new System.Windows.Forms.GroupBox();
            this.orRadio = new System.Windows.Forms.RadioButton();
            this.andRadio = new System.Windows.Forms.RadioButton();
            this.checksParam = new System.Windows.Forms.CheckedListBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.filterBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(12, 19);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(100, 60);
            this.filterButton.TabIndex = 3;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
            // 
            // filterBox
            // 
            this.filterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterBox.Controls.Add(this.orRadio);
            this.filterBox.Controls.Add(this.andRadio);
            this.filterBox.Location = new System.Drawing.Point(308, 12);
            this.filterBox.Name = "filterBox";
            this.filterBox.Size = new System.Drawing.Size(108, 89);
            this.filterBox.TabIndex = 2;
            this.filterBox.TabStop = false;
            this.filterBox.Text = "Filter Type";
            // 
            // orRadio
            // 
            this.orRadio.AutoSize = true;
            this.orRadio.Location = new System.Drawing.Point(7, 58);
            this.orRadio.Name = "orRadio";
            this.orRadio.Size = new System.Drawing.Size(51, 24);
            this.orRadio.TabIndex = 1;
            this.orRadio.Text = "Or";
            this.orRadio.UseVisualStyleBackColor = true;
            // 
            // andRadio
            // 
            this.andRadio.AutoSize = true;
            this.andRadio.Checked = true;
            this.andRadio.Location = new System.Drawing.Point(7, 26);
            this.andRadio.Name = "andRadio";
            this.andRadio.Size = new System.Drawing.Size(63, 24);
            this.andRadio.TabIndex = 0;
            this.andRadio.TabStop = true;
            this.andRadio.Text = "And";
            this.andRadio.UseVisualStyleBackColor = true;
            // 
            // checksParam
            // 
            this.checksParam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checksParam.BackColor = System.Drawing.SystemColors.Window;
            this.checksParam.CheckOnClick = true;
            this.checksParam.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checksParam.FormattingEnabled = true;
            this.checksParam.Location = new System.Drawing.Point(0, 123);
            this.checksParam.Name = "checksParam";
            this.checksParam.Size = new System.Drawing.Size(428, 554);
            this.checksParam.TabIndex = 1;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(123, 19);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(100, 60);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ParameterSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 694);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.checksParam);
            this.Controls.Add(this.filterBox);
            this.Controls.Add(this.filterButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 750);
            this.Name = "ParameterSelector";
            this.Text = "Parameter Selector";
            this.TopMost = true;
            this.filterBox.ResumeLayout(false);
            this.filterBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.GroupBox filterBox;
        private System.Windows.Forms.RadioButton orRadio;
        private System.Windows.Forms.RadioButton andRadio;
        private System.Windows.Forms.CheckedListBox checksParam;
        private System.Windows.Forms.Button CancelBtn;
    }
}
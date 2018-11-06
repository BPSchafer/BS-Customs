namespace BIMtrovert.BS_Customs
{
    partial class AssemblyOptionsForm
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
            this.SaveBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.TemplCheck = new System.Windows.Forms.CheckBox();
            this.vPaCheck = new System.Windows.Forms.CheckBox();
            this.vPlCheck = new System.Windows.Forms.CheckBox();
            this.vElCheck = new System.Windows.Forms.CheckBox();
            this.TemplCombo = new System.Windows.Forms.ComboBox();
            this.v3DCheck = new System.Windows.Forms.CheckBox();
            this.vPaCombo = new System.Windows.Forms.ComboBox();
            this.vPlCombo = new System.Windows.Forms.ComboBox();
            this.vElCombo = new System.Windows.Forms.ComboBox();
            this.v3dCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // SaveBtn
            // 
            this.SaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveBtn.Location = new System.Drawing.Point(12, 390);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(201, 42);
            this.SaveBtn.TabIndex = 2;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(375, 390);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(201, 42);
            this.button1.TabIndex = 3;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TemplCheck
            // 
            this.TemplCheck.AutoSize = true;
            this.TemplCheck.Checked = global::BIMtrovert.BS_Customs.Properties.Settings.Default.TemplateCheck;
            this.TemplCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TemplCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "TemplateCheck", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TemplCheck.Location = new System.Drawing.Point(12, 166);
            this.TemplCheck.Name = "TemplCheck";
            this.TemplCheck.Size = new System.Drawing.Size(101, 24);
            this.TemplCheck.TabIndex = 1;
            this.TemplCheck.Text = "Parts List";
            this.TemplCheck.UseVisualStyleBackColor = true;
            // 
            // vPaCheck
            // 
            this.vPaCheck.AutoSize = true;
            this.vPaCheck.Checked = global::BIMtrovert.BS_Customs.Properties.Settings.Default.CheckedPart;
            this.vPaCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.vPaCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "CheckedPart", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.vPaCheck.Location = new System.Drawing.Point(12, 132);
            this.vPaCheck.Name = "vPaCheck";
            this.vPaCheck.Size = new System.Drawing.Size(101, 24);
            this.vPaCheck.TabIndex = 1;
            this.vPaCheck.Text = "Parts List";
            this.vPaCheck.UseVisualStyleBackColor = true;
            // 
            // vPlCheck
            // 
            this.vPlCheck.AutoSize = true;
            this.vPlCheck.Checked = global::BIMtrovert.BS_Customs.Properties.Settings.Default.CheckedPlan;
            this.vPlCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.vPlCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "CheckedPlan", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.vPlCheck.Location = new System.Drawing.Point(12, 98);
            this.vPlCheck.Name = "vPlCheck";
            this.vPlCheck.Size = new System.Drawing.Size(125, 24);
            this.vPlCheck.TabIndex = 1;
            this.vPlCheck.Text = "Bottom View";
            this.vPlCheck.UseVisualStyleBackColor = true;
            // 
            // vElCheck
            // 
            this.vElCheck.AutoSize = true;
            this.vElCheck.Checked = global::BIMtrovert.BS_Customs.Properties.Settings.Default.CheckedElev;
            this.vElCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.vElCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "CheckedElev", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.vElCheck.Location = new System.Drawing.Point(12, 64);
            this.vElCheck.Name = "vElCheck";
            this.vElCheck.Size = new System.Drawing.Size(138, 24);
            this.vElCheck.TabIndex = 1;
            this.vElCheck.Text = "Elevation View";
            this.vElCheck.UseVisualStyleBackColor = true;
            // 
            // TemplCombo
            // 
            this.TemplCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TemplCombo.DataBindings.Add(new System.Windows.Forms.Binding("ValueMember", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "TemplTemplate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TemplCombo.FormattingEnabled = true;
            this.TemplCombo.Location = new System.Drawing.Point(156, 166);
            this.TemplCombo.Name = "TemplCombo";
            this.TemplCombo.Size = new System.Drawing.Size(420, 28);
            this.TemplCombo.TabIndex = 0;
            this.TemplCombo.ValueMember = global::BIMtrovert.BS_Customs.Properties.Settings.Default.TemplTemplate;
            // 
            // v3DCheck
            // 
            this.v3DCheck.AutoSize = true;
            this.v3DCheck.Checked = global::BIMtrovert.BS_Customs.Properties.Settings.Default.Check3DView;
            this.v3DCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.v3DCheck.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "Check3DView", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.v3DCheck.Location = new System.Drawing.Point(12, 30);
            this.v3DCheck.Name = "v3DCheck";
            this.v3DCheck.Size = new System.Drawing.Size(94, 24);
            this.v3DCheck.TabIndex = 1;
            this.v3DCheck.Text = "3D View";
            this.v3DCheck.UseVisualStyleBackColor = true;
            // 
            // vPaCombo
            // 
            this.vPaCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vPaCombo.DataBindings.Add(new System.Windows.Forms.Binding("ValueMember", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "ViewPaTemplate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.vPaCombo.FormattingEnabled = true;
            this.vPaCombo.Location = new System.Drawing.Point(156, 132);
            this.vPaCombo.Name = "vPaCombo";
            this.vPaCombo.Size = new System.Drawing.Size(420, 28);
            this.vPaCombo.TabIndex = 0;
            this.vPaCombo.ValueMember = global::BIMtrovert.BS_Customs.Properties.Settings.Default.ViewPaTemplate;
            // 
            // vPlCombo
            // 
            this.vPlCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vPlCombo.DataBindings.Add(new System.Windows.Forms.Binding("ValueMember", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "ViewPlTemplate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.vPlCombo.FormattingEnabled = true;
            this.vPlCombo.Location = new System.Drawing.Point(156, 98);
            this.vPlCombo.Name = "vPlCombo";
            this.vPlCombo.Size = new System.Drawing.Size(420, 28);
            this.vPlCombo.TabIndex = 0;
            this.vPlCombo.ValueMember = global::BIMtrovert.BS_Customs.Properties.Settings.Default.ViewPlTemplate;
            // 
            // vElCombo
            // 
            this.vElCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vElCombo.DataBindings.Add(new System.Windows.Forms.Binding("ValueMember", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "ViewElTemplate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.vElCombo.FormattingEnabled = true;
            this.vElCombo.Location = new System.Drawing.Point(156, 64);
            this.vElCombo.Name = "vElCombo";
            this.vElCombo.Size = new System.Drawing.Size(420, 28);
            this.vElCombo.TabIndex = 0;
            this.vElCombo.ValueMember = global::BIMtrovert.BS_Customs.Properties.Settings.Default.ViewElTemplate;
            // 
            // v3dCombo
            // 
            this.v3dCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.v3dCombo.DataBindings.Add(new System.Windows.Forms.Binding("ValueMember", global::BIMtrovert.BS_Customs.Properties.Settings.Default, "View3DTemplate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.v3dCombo.FormattingEnabled = true;
            this.v3dCombo.Location = new System.Drawing.Point(156, 30);
            this.v3dCombo.Name = "v3dCombo";
            this.v3dCombo.Size = new System.Drawing.Size(420, 28);
            this.v3dCombo.TabIndex = 0;
            this.v3dCombo.ValueMember = global::BIMtrovert.BS_Customs.Properties.Settings.Default.View3DTemplate;
            // 
            // AssemblyOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 444);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.TemplCheck);
            this.Controls.Add(this.vPaCheck);
            this.Controls.Add(this.vPlCheck);
            this.Controls.Add(this.vElCheck);
            this.Controls.Add(this.TemplCombo);
            this.Controls.Add(this.v3DCheck);
            this.Controls.Add(this.vPaCombo);
            this.Controls.Add(this.vPlCombo);
            this.Controls.Add(this.vElCombo);
            this.Controls.Add(this.v3dCombo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "AssemblyOptionsForm";
            this.Text = "Assembly Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox v3dCombo;
        private System.Windows.Forms.CheckBox v3DCheck;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox vElCombo;
        private System.Windows.Forms.CheckBox vElCheck;
        private System.Windows.Forms.ComboBox vPlCombo;
        private System.Windows.Forms.CheckBox vPlCheck;
        private System.Windows.Forms.ComboBox vPaCombo;
        private System.Windows.Forms.CheckBox vPaCheck;
        private System.Windows.Forms.ComboBox TemplCombo;
        private System.Windows.Forms.CheckBox TemplCheck;
    }
}
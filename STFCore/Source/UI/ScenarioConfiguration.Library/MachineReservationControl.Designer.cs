namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class MachineReservationControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.platform_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.name_Label = new System.Windows.Forms.Label();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.platform_ComboBox = new System.Windows.Forms.ComboBox();
            this.instanceCount_Label = new System.Windows.Forms.Label();
            this.instanceCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.package_ComboBox = new System.Windows.Forms.ComboBox();
            this.package_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fieldValidator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.instanceCount_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // platform_Label
            // 
            this.platform_Label.AutoSize = true;
            this.platform_Label.Location = new System.Drawing.Point(48, 74);
            this.platform_Label.Name = "platform_Label";
            this.platform_Label.Size = new System.Drawing.Size(53, 15);
            this.platform_Label.TabIndex = 24;
            this.platform_Label.Text = "Platform";
            // 
            // description_Label
            // 
            this.description_Label.AutoSize = true;
            this.description_Label.Location = new System.Drawing.Point(34, 45);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(67, 15);
            this.description_Label.TabIndex = 25;
            this.description_Label.Text = "Description";
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Location = new System.Drawing.Point(62, 16);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(39, 15);
            this.name_Label.TabIndex = 26;
            this.name_Label.Text = "Name";
            // 
            // name_TextBox
            // 
            this.name_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name_TextBox.Location = new System.Drawing.Point(107, 13);
            this.name_TextBox.MaxLength = 255;
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(483, 23);
            this.name_TextBox.TabIndex = 27;
            // 
            // description_TextBox
            // 
            this.description_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description_TextBox.Location = new System.Drawing.Point(107, 42);
            this.description_TextBox.MaxLength = 500;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(483, 23);
            this.description_TextBox.TabIndex = 28;
            // 
            // platform_ComboBox
            // 
            this.platform_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platform_ComboBox.FormattingEnabled = true;
            this.platform_ComboBox.Location = new System.Drawing.Point(107, 71);
            this.platform_ComboBox.Name = "platform_ComboBox";
            this.platform_ComboBox.Size = new System.Drawing.Size(358, 23);
            this.platform_ComboBox.TabIndex = 29;
            // 
            // instanceCount_Label
            // 
            this.instanceCount_Label.AutoSize = true;
            this.instanceCount_Label.Location = new System.Drawing.Point(12, 131);
            this.instanceCount_Label.Name = "instanceCount_Label";
            this.instanceCount_Label.Size = new System.Drawing.Size(89, 15);
            this.instanceCount_Label.TabIndex = 82;
            this.instanceCount_Label.Text = "Machine Count";
            // 
            // instanceCount_NumericUpDown
            // 
            this.instanceCount_NumericUpDown.Location = new System.Drawing.Point(107, 129);
            this.instanceCount_NumericUpDown.Name = "instanceCount_NumericUpDown";
            this.instanceCount_NumericUpDown.Size = new System.Drawing.Size(53, 23);
            this.instanceCount_NumericUpDown.TabIndex = 83;
            // 
            // package_ComboBox
            // 
            this.package_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.package_ComboBox.FormattingEnabled = true;
            this.package_ComboBox.Location = new System.Drawing.Point(107, 100);
            this.package_ComboBox.Name = "package_ComboBox";
            this.package_ComboBox.Size = new System.Drawing.Size(358, 23);
            this.package_ComboBox.TabIndex = 84;
            // 
            // package_Label
            // 
            this.package_Label.AutoSize = true;
            this.package_Label.Location = new System.Drawing.Point(6, 103);
            this.package_Label.Name = "package_Label";
            this.package_Label.Size = new System.Drawing.Size(95, 15);
            this.package_Label.TabIndex = 85;
            this.package_Label.Text = "Installer Package";
            // 
            // MachineReservationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.Controls.Add(this.package_Label);
            this.Controls.Add(this.package_ComboBox);
            this.Controls.Add(this.instanceCount_NumericUpDown);
            this.Controls.Add(this.instanceCount_Label);
            this.Controls.Add(this.platform_ComboBox);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.platform_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MachineReservationControl";
            this.Size = new System.Drawing.Size(593, 446);
            ((System.ComponentModel.ISupportInitialize)(this.fieldValidator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.instanceCount_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label platform_Label;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.ComboBox platform_ComboBox;
        private System.Windows.Forms.Label instanceCount_Label;
        private System.Windows.Forms.NumericUpDown instanceCount_NumericUpDown;
        private System.Windows.Forms.ComboBox package_ComboBox;
        private System.Windows.Forms.Label package_Label;
    }
}

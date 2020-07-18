namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    partial class PrintDriverSelector
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
			this.components = new System.ComponentModel.Container();
			this.driverDetails_GroupBox = new System.Windows.Forms.GroupBox();
			this.driverModel_ComboBox = new System.Windows.Forms.ComboBox();
			this.driverPackage_TextBox = new System.Windows.Forms.TextBox();
			this.model_Label = new System.Windows.Forms.Label();
			this.package_Label = new System.Windows.Forms.Label();
			this.driverFolder_BrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.driver_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.driverDetails_FieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
			this.driverDetails_GroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// driverDetails_GroupBox
			// 
			this.driverDetails_GroupBox.Controls.Add(this.driverModel_ComboBox);
			this.driverDetails_GroupBox.Controls.Add(this.driverPackage_TextBox);
			this.driverDetails_GroupBox.Controls.Add(this.model_Label);
			this.driverDetails_GroupBox.Controls.Add(this.package_Label);
			this.driverDetails_GroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.driverDetails_GroupBox.Location = new System.Drawing.Point(0, 0);
			this.driverDetails_GroupBox.Name = "driverDetails_GroupBox";
			this.driverDetails_GroupBox.Size = new System.Drawing.Size(314, 99);
			this.driverDetails_GroupBox.TabIndex = 0;
			this.driverDetails_GroupBox.TabStop = false;
			this.driverDetails_GroupBox.Text = "Driver Details";
			// 
			// driverModel_ComboBox
			// 
			this.driverModel_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.driverModel_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.driverModel_ComboBox.FormattingEnabled = true;
			this.driverModel_ComboBox.Location = new System.Drawing.Point(77, 61);
			this.driverModel_ComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
			this.driverModel_ComboBox.Name = "driverModel_ComboBox";
			this.driverModel_ComboBox.Size = new System.Drawing.Size(174, 21);
			this.driverModel_ComboBox.TabIndex = 5;
			this.driver_toolTip.SetToolTip(this.driverModel_ComboBox, "Select driver model");
			this.driverModel_ComboBox.SelectedIndexChanged += new System.EventHandler(this.driverModel_ComboBox_SelectedIndexChanged);
			this.driverModel_ComboBox.Enter += new System.EventHandler(this.driverModel_ComboBox_Enter);
			// 
			// driverPackage_TextBox
			// 
			this.driverPackage_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.driverPackage_TextBox.Location = new System.Drawing.Point(77, 25);
			this.driverPackage_TextBox.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
			this.driverPackage_TextBox.Name = "driverPackage_TextBox";
			this.driverPackage_TextBox.ReadOnly = true;
			this.driverPackage_TextBox.Size = new System.Drawing.Size(174, 20);
			this.driverPackage_TextBox.TabIndex = 2;
			this.driverPackage_TextBox.TextChanged += new System.EventHandler(this.driverPackage_TextBox_TextChanged);
			// 
			// model_Label
			// 
			this.model_Label.AutoSize = true;
			this.model_Label.Location = new System.Drawing.Point(18, 64);
			this.model_Label.Name = "model_Label";
			this.model_Label.Size = new System.Drawing.Size(39, 13);
			this.model_Label.TabIndex = 1;
			this.model_Label.Text = "Model:";
			// 
			// package_Label
			// 
			this.package_Label.AutoSize = true;
			this.package_Label.Location = new System.Drawing.Point(18, 29);
			this.package_Label.Name = "package_Label";
			this.package_Label.Size = new System.Drawing.Size(53, 13);
			this.package_Label.TabIndex = 0;
			this.package_Label.Text = "Package:";
			// 
			// PrintDriverSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.driverDetails_GroupBox);
			this.MinimumSize = new System.Drawing.Size(314, 99);
			this.Name = "PrintDriverSelector";
			this.Size = new System.Drawing.Size(314, 99);
			this.driverDetails_GroupBox.ResumeLayout(false);
			this.driverDetails_GroupBox.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox driverDetails_GroupBox;
        private System.Windows.Forms.Label package_Label;
        private System.Windows.Forms.TextBox driverPackage_TextBox;
        private System.Windows.Forms.Label model_Label;
        private System.Windows.Forms.ComboBox driverModel_ComboBox;
        private System.Windows.Forms.FolderBrowserDialog driverFolder_BrowserDialog;
        private System.Windows.Forms.ToolTip driver_toolTip;
        private Framework.UI.FieldValidator driverDetails_FieldValidator;
    }
}

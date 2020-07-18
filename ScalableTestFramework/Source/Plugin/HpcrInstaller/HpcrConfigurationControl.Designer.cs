namespace HP.ScalableTest.Plugin.HpcrInstaller
{
    partial class HpcrConfigurationControl
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
            this.hpcr_GroupBox = new System.Windows.Forms.GroupBox();
            this.deviceGroup_ComboBox = new System.Windows.Forms.ComboBox();
            this.deviceGroup_Label = new System.Windows.Forms.Label();
            this.hpcrAction_ComboBox = new System.Windows.Forms.ComboBox();
            this.hpcrAction_Label = new System.Windows.Forms.Label();
            this.hpcr_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.hpcrServer_Label = new System.Windows.Forms.Label();
            this.bundle_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.hpcr_AssetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.hpcr_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // hpcr_GroupBox
            // 
            this.hpcr_GroupBox.Controls.Add(this.deviceGroup_ComboBox);
            this.hpcr_GroupBox.Controls.Add(this.deviceGroup_Label);
            this.hpcr_GroupBox.Controls.Add(this.hpcrAction_ComboBox);
            this.hpcr_GroupBox.Controls.Add(this.hpcrAction_Label);
            this.hpcr_GroupBox.Controls.Add(this.hpcr_ServerComboBox);
            this.hpcr_GroupBox.Controls.Add(this.hpcrServer_Label);
            this.hpcr_GroupBox.Location = new System.Drawing.Point(3, 246);
            this.hpcr_GroupBox.Name = "hpcr_GroupBox";
            this.hpcr_GroupBox.Size = new System.Drawing.Size(656, 388);
            this.hpcr_GroupBox.TabIndex = 1;
            this.hpcr_GroupBox.TabStop = false;
            this.hpcr_GroupBox.Text = "Hpcr Administration";
            // 
            // deviceGroup_ComboBox
            // 
            this.deviceGroup_ComboBox.FormattingEnabled = true;
            this.deviceGroup_ComboBox.Location = new System.Drawing.Point(146, 156);
            this.deviceGroup_ComboBox.Name = "deviceGroup_ComboBox";
            this.deviceGroup_ComboBox.Size = new System.Drawing.Size(210, 28);
            this.deviceGroup_ComboBox.TabIndex = 25;
            // 
            // deviceGroup_Label
            // 
            this.deviceGroup_Label.AutoSize = true;
            this.deviceGroup_Label.Location = new System.Drawing.Point(10, 159);
            this.deviceGroup_Label.Name = "deviceGroup_Label";
            this.deviceGroup_Label.Size = new System.Drawing.Size(102, 20);
            this.deviceGroup_Label.TabIndex = 24;
            this.deviceGroup_Label.Text = "Device Group:";
            // 
            // hpcrAction_ComboBox
            // 
            this.hpcrAction_ComboBox.FormattingEnabled = true;
            this.hpcrAction_ComboBox.Location = new System.Drawing.Point(146, 93);
            this.hpcrAction_ComboBox.Name = "hpcrAction_ComboBox";
            this.hpcrAction_ComboBox.Size = new System.Drawing.Size(210, 28);
            this.hpcrAction_ComboBox.TabIndex = 18;
            // 
            // hpcrAction_Label
            // 
            this.hpcrAction_Label.AutoSize = true;
            this.hpcrAction_Label.Location = new System.Drawing.Point(10, 96);
            this.hpcrAction_Label.Name = "hpcrAction_Label";
            this.hpcrAction_Label.Size = new System.Drawing.Size(91, 20);
            this.hpcrAction_Label.TabIndex = 17;
            this.hpcrAction_Label.Text = "Hpcr Action:";
            // 
            // hpcr_ServerComboBox
            // 
            this.hpcr_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpcr_ServerComboBox.Location = new System.Drawing.Point(146, 44);
            this.hpcr_ServerComboBox.Name = "hpcr_ServerComboBox";
            this.hpcr_ServerComboBox.Size = new System.Drawing.Size(210, 23);
            this.hpcr_ServerComboBox.TabIndex = 16;
            this.hpcr_ServerComboBox.SelectionChanged += new System.EventHandler(this.hpcr_ServerComboBox_SelectionChanged);
            // 
            // hpcrServer_Label
            // 
            this.hpcrServer_Label.AutoSize = true;
            this.hpcrServer_Label.Location = new System.Drawing.Point(10, 44);
            this.hpcrServer_Label.Name = "hpcrServer_Label";
            this.hpcrServer_Label.Size = new System.Drawing.Size(89, 20);
            this.hpcrServer_Label.TabIndex = 14;
            this.hpcrServer_Label.Text = "Hpcr Server:";
            // 
            // hpcr_AssetSelectionControl
            // 
            this.hpcr_AssetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpcr_AssetSelectionControl.Location = new System.Drawing.Point(3, 12);
            this.hpcr_AssetSelectionControl.Name = "hpcr_AssetSelectionControl";
            this.hpcr_AssetSelectionControl.Size = new System.Drawing.Size(656, 228);
            this.hpcr_AssetSelectionControl.TabIndex = 0;
            // 
            // HpcrConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hpcr_GroupBox);
            this.Controls.Add(this.hpcr_AssetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HpcrConfigurationControl";
            this.Size = new System.Drawing.Size(668, 640);
            this.hpcr_GroupBox.ResumeLayout(false);
            this.hpcr_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl hpcr_AssetSelectionControl;
        private System.Windows.Forms.GroupBox hpcr_GroupBox;
        private System.Windows.Forms.OpenFileDialog bundle_openFileDialog;
        private System.Windows.Forms.Label hpcrServer_Label;
        private Framework.UI.ServerComboBox hpcr_ServerComboBox;
        private System.Windows.Forms.ComboBox hpcrAction_ComboBox;
        private System.Windows.Forms.Label hpcrAction_Label;
        private System.Windows.Forms.ComboBox deviceGroup_ComboBox;
        private System.Windows.Forms.Label deviceGroup_Label;
    }
}

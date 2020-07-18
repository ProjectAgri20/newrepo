namespace HP.ScalableTest.Plugin.HpecInstaller
{
    partial class HpecInstallerConfigurationControl
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
            this.hpec_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.hpec_serverComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.server_label = new System.Windows.Forms.Label();
            this.workflow_label = new System.Windows.Forms.Label();
            this.browse_button = new System.Windows.Forms.Button();
            this.workflowFile_textBox = new System.Windows.Forms.TextBox();
            this.installerAction_comboBox = new System.Windows.Forms.ComboBox();
            this.installerAction_label = new System.Windows.Forms.Label();
            this.hpec_groupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.version_textBox = new System.Windows.Forms.TextBox();
            this.hpec_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // hpec_assetSelectionControl
            // 
            this.hpec_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpec_assetSelectionControl.Location = new System.Drawing.Point(3, 3);
            this.hpec_assetSelectionControl.Name = "hpec_assetSelectionControl";
            this.hpec_assetSelectionControl.Size = new System.Drawing.Size(742, 292);
            this.hpec_assetSelectionControl.TabIndex = 0;
            // 
            // hpec_serverComboBox
            // 
            this.hpec_serverComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpec_serverComboBox.Location = new System.Drawing.Point(0, 343);
            this.hpec_serverComboBox.Name = "hpec_serverComboBox";
            this.hpec_serverComboBox.Size = new System.Drawing.Size(205, 23);
            this.hpec_serverComboBox.TabIndex = 1;
            // 
            // server_label
            // 
            this.server_label.AutoSize = true;
            this.server_label.Location = new System.Drawing.Point(3, 324);
            this.server_label.Name = "server_label";
            this.server_label.Size = new System.Drawing.Size(72, 15);
            this.server_label.TabIndex = 2;
            this.server_label.Text = "HPEC Server";
            // 
            // workflow_label
            // 
            this.workflow_label.AutoSize = true;
            this.workflow_label.Location = new System.Drawing.Point(194, 23);
            this.workflow_label.Name = "workflow_label";
            this.workflow_label.Size = new System.Drawing.Size(110, 15);
            this.workflow_label.TabIndex = 5;
            this.workflow_label.Text = "Hpec Workflow File";
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(438, 71);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(75, 23);
            this.browse_button.TabIndex = 4;
            this.browse_button.Text = "Browse...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // workflowFile_textBox
            // 
            this.workflowFile_textBox.BackColor = System.Drawing.Color.White;
            this.workflowFile_textBox.Location = new System.Drawing.Point(197, 42);
            this.workflowFile_textBox.Name = "workflowFile_textBox";
            this.workflowFile_textBox.ReadOnly = true;
            this.workflowFile_textBox.Size = new System.Drawing.Size(316, 23);
            this.workflowFile_textBox.TabIndex = 3;
            // 
            // installerAction_comboBox
            // 
            this.installerAction_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.installerAction_comboBox.FormattingEnabled = true;
            this.installerAction_comboBox.Location = new System.Drawing.Point(9, 42);
            this.installerAction_comboBox.Name = "installerAction_comboBox";
            this.installerAction_comboBox.Size = new System.Drawing.Size(170, 23);
            this.installerAction_comboBox.TabIndex = 6;
            // 
            // installerAction_label
            // 
            this.installerAction_label.AutoSize = true;
            this.installerAction_label.Location = new System.Drawing.Point(6, 23);
            this.installerAction_label.Name = "installerAction_label";
            this.installerAction_label.Size = new System.Drawing.Size(117, 15);
            this.installerAction_label.TabIndex = 7;
            this.installerAction_label.Text = "Hpec Installer Action";
            // 
            // hpec_groupBox
            // 
            this.hpec_groupBox.Controls.Add(this.installerAction_label);
            this.hpec_groupBox.Controls.Add(this.workflowFile_textBox);
            this.hpec_groupBox.Controls.Add(this.installerAction_comboBox);
            this.hpec_groupBox.Controls.Add(this.browse_button);
            this.hpec_groupBox.Controls.Add(this.workflow_label);
            this.hpec_groupBox.Location = new System.Drawing.Point(214, 301);
            this.hpec_groupBox.Name = "hpec_groupBox";
            this.hpec_groupBox.Size = new System.Drawing.Size(531, 131);
            this.hpec_groupBox.TabIndex = 8;
            this.hpec_groupBox.TabStop = false;
            this.hpec_groupBox.Text = "Hpec Installer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 380);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "HPEC Server Version";
            // 
            // version_textBox
            // 
            this.version_textBox.Location = new System.Drawing.Point(0, 398);
            this.version_textBox.Name = "version_textBox";
            this.version_textBox.Size = new System.Drawing.Size(205, 23);
            this.version_textBox.TabIndex = 8;
            // 
            // HpecInstallerConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.version_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hpec_groupBox);
            this.Controls.Add(this.server_label);
            this.Controls.Add(this.hpec_serverComboBox);
            this.Controls.Add(this.hpec_assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HpecInstallerConfigurationControl";
            this.Size = new System.Drawing.Size(754, 444);
            this.hpec_groupBox.ResumeLayout(false);
            this.hpec_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl hpec_assetSelectionControl;
        private Framework.UI.ServerComboBox hpec_serverComboBox;
        private System.Windows.Forms.Label server_label;
        private System.Windows.Forms.Label workflow_label;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox workflowFile_textBox;
        private System.Windows.Forms.ComboBox installerAction_comboBox;
        private System.Windows.Forms.Label installerAction_label;
        private System.Windows.Forms.GroupBox hpec_groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox version_textBox;
    }
}

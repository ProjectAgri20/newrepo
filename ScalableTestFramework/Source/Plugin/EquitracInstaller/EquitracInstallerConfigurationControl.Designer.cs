namespace HP.ScalableTest.Plugin.EquitracInstaller
{
    partial class EquitracInstallerConfigurationControl
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.equitrac_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.equitrac_groupBox = new System.Windows.Forms.GroupBox();
            this.bundle_label = new System.Windows.Forms.Label();
            this.browse_button = new System.Windows.Forms.Button();
            this.bundleFile_textBox = new System.Windows.Forms.TextBox();
            this.equitrac_serverComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.equitracserver_label = new System.Windows.Forms.Label();
            this.tasks_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.equitrac_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // equitrac_assetSelectionControl
            // 
            this.equitrac_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equitrac_assetSelectionControl.Location = new System.Drawing.Point(4, 3);
            this.equitrac_assetSelectionControl.Name = "equitrac_assetSelectionControl";
            this.equitrac_assetSelectionControl.Size = new System.Drawing.Size(722, 341);
            this.equitrac_assetSelectionControl.TabIndex = 0;
            // 
            // equitrac_groupBox
            // 
            this.equitrac_groupBox.Controls.Add(this.equitrac_serverComboBox);
            this.equitrac_groupBox.Controls.Add(this.equitracserver_label);
            this.equitrac_groupBox.Controls.Add(this.tasks_comboBox);
            this.equitrac_groupBox.Controls.Add(this.label2);
            this.equitrac_groupBox.Controls.Add(this.bundle_label);
            this.equitrac_groupBox.Controls.Add(this.browse_button);
            this.equitrac_groupBox.Controls.Add(this.bundleFile_textBox);
            this.equitrac_groupBox.Location = new System.Drawing.Point(4, 350);
            this.equitrac_groupBox.Name = "equitrac_groupBox";
            this.equitrac_groupBox.Size = new System.Drawing.Size(722, 196);
            this.equitrac_groupBox.TabIndex = 1;
            this.equitrac_groupBox.TabStop = false;
            this.equitrac_groupBox.Text = "Equitrac Administration";
            // 
            // bundle_label
            // 
            this.bundle_label.AutoSize = true;
            this.bundle_label.Location = new System.Drawing.Point(297, 15);
            this.bundle_label.Name = "bundle_label";
            this.bundle_label.Size = new System.Drawing.Size(111, 15);
            this.bundle_label.TabIndex = 5;
            this.bundle_label.Text = "Equitrac Bundle File";
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(638, 33);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(75, 23);
            this.browse_button.TabIndex = 4;
            this.browse_button.Text = "Browse...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // bundleFile_textBox
            // 
            this.bundleFile_textBox.BackColor = System.Drawing.Color.White;
            this.bundleFile_textBox.Location = new System.Drawing.Point(300, 33);
            this.bundleFile_textBox.Name = "bundleFile_textBox";
            this.bundleFile_textBox.ReadOnly = true;
            this.bundleFile_textBox.Size = new System.Drawing.Size(335, 23);
            this.bundleFile_textBox.TabIndex = 3;
            // 
            // equitrac_serverComboBox
            // 
            this.equitrac_serverComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equitrac_serverComboBox.Location = new System.Drawing.Point(5, 86);
            this.equitrac_serverComboBox.Name = "equitrac_serverComboBox";
            this.equitrac_serverComboBox.Size = new System.Drawing.Size(210, 23);
            this.equitrac_serverComboBox.TabIndex = 20;
            // 
            // equitracserver_label
            // 
            this.equitracserver_label.AutoSize = true;
            this.equitracserver_label.Location = new System.Drawing.Point(8, 68);
            this.equitracserver_label.Name = "equitracserver_label";
            this.equitracserver_label.Size = new System.Drawing.Size(85, 15);
            this.equitracserver_label.TabIndex = 19;
            this.equitracserver_label.Text = "Equitrac Server";
            // 
            // tasks_comboBox
            // 
            this.tasks_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tasks_comboBox.FormattingEnabled = true;
            this.tasks_comboBox.Location = new System.Drawing.Point(6, 34);
            this.tasks_comboBox.Name = "tasks_comboBox";
            this.tasks_comboBox.Size = new System.Drawing.Size(184, 23);
            this.tasks_comboBox.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Administration Task";
            // 
            // EquitracInstallerConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.equitrac_groupBox);
            this.Controls.Add(this.equitrac_assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EquitracInstallerConfigurationControl";
            this.Size = new System.Drawing.Size(732, 549);
            this.equitrac_groupBox.ResumeLayout(false);
            this.equitrac_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl equitrac_assetSelectionControl;
        private System.Windows.Forms.GroupBox equitrac_groupBox;
        private System.Windows.Forms.Label bundle_label;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox bundleFile_textBox;
        private Framework.UI.ServerComboBox equitrac_serverComboBox;
        private System.Windows.Forms.Label equitracserver_label;
        private System.Windows.Forms.ComboBox tasks_comboBox;
        private System.Windows.Forms.Label label2;
    }
}

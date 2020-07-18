namespace HP.ScalableTest.Plugin.HpEasyStart
{
    partial class HpEasyStartConfigControl
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
            this.labelInstallerPath = new System.Windows.Forms.Label();
            this.textBoxInstallerPath = new System.Windows.Forms.TextBox();
            this.buttonPathSelection = new System.Windows.Forms.Button();
            this.checkBoxTestPage = new System.Windows.Forms.CheckBox();
            this.checkBoxSetDefault = new System.Windows.Forms.CheckBox();
            this.fullInstallation_RadioButton = new System.Windows.Forms.RadioButton();
            this.webPackInstallation_RadioButton = new System.Windows.Forms.RadioButton();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.SuspendLayout();
            // 
            // labelInstallerPath
            // 
            this.labelInstallerPath.AutoSize = true;
            this.labelInstallerPath.Location = new System.Drawing.Point(24, 47);
            this.labelInstallerPath.Name = "labelInstallerPath";
            this.labelInstallerPath.Size = new System.Drawing.Size(137, 13);
            this.labelInstallerPath.TabIndex = 2;
            this.labelInstallerPath.Text = "HP Easy Start Installer Path";
            // 
            // textBoxInstallerPath
            // 
            this.textBoxInstallerPath.BackColor = System.Drawing.Color.White;
            this.textBoxInstallerPath.Location = new System.Drawing.Point(189, 44);
            this.textBoxInstallerPath.Name = "textBoxInstallerPath";
            this.textBoxInstallerPath.ReadOnly = true;
            this.textBoxInstallerPath.Size = new System.Drawing.Size(360, 20);
            this.textBoxInstallerPath.TabIndex = 6;
            // 
            // buttonPathSelection
            // 
            this.buttonPathSelection.Location = new System.Drawing.Point(555, 42);
            this.buttonPathSelection.Name = "buttonPathSelection";
            this.buttonPathSelection.Size = new System.Drawing.Size(36, 23);
            this.buttonPathSelection.TabIndex = 7;
            this.buttonPathSelection.Text = "...";
            this.buttonPathSelection.UseVisualStyleBackColor = true;
            this.buttonPathSelection.Click += new System.EventHandler(this.buttonPathSelection_Click);
            // 
            // checkBoxTestPage
            // 
            this.checkBoxTestPage.AutoSize = true;
            this.checkBoxTestPage.Location = new System.Drawing.Point(27, 107);
            this.checkBoxTestPage.Name = "checkBoxTestPage";
            this.checkBoxTestPage.Size = new System.Drawing.Size(255, 17);
            this.checkBoxTestPage.TabIndex = 14;
            this.checkBoxTestPage.Text = "Print Test Page after the installation is completed";
            this.checkBoxTestPage.UseVisualStyleBackColor = true;
            // 
            // checkBoxSetDefault
            // 
            this.checkBoxSetDefault.AutoSize = true;
            this.checkBoxSetDefault.Location = new System.Drawing.Point(27, 75);
            this.checkBoxSetDefault.Name = "checkBoxSetDefault";
            this.checkBoxSetDefault.Size = new System.Drawing.Size(198, 17);
            this.checkBoxSetDefault.TabIndex = 15;
            this.checkBoxSetDefault.Text = "Set this Driver as Default Print Driver";
            this.checkBoxSetDefault.UseVisualStyleBackColor = true;
            // 
            // fullInstallation_RadioButton
            // 
            this.fullInstallation_RadioButton.AutoSize = true;
            this.fullInstallation_RadioButton.Checked = true;
            this.fullInstallation_RadioButton.Location = new System.Drawing.Point(27, 16);
            this.fullInstallation_RadioButton.Name = "fullInstallation_RadioButton";
            this.fullInstallation_RadioButton.Size = new System.Drawing.Size(94, 17);
            this.fullInstallation_RadioButton.TabIndex = 17;
            this.fullInstallation_RadioButton.TabStop = true;
            this.fullInstallation_RadioButton.Text = "Full Installation";
            this.fullInstallation_RadioButton.UseVisualStyleBackColor = true;
            // 
            // webPackInstallation_RadioButton
            // 
            this.webPackInstallation_RadioButton.AutoSize = true;
            this.webPackInstallation_RadioButton.Location = new System.Drawing.Point(189, 16);
            this.webPackInstallation_RadioButton.Name = "webPackInstallation_RadioButton";
            this.webPackInstallation_RadioButton.Size = new System.Drawing.Size(126, 17);
            this.webPackInstallation_RadioButton.TabIndex = 18;
            this.webPackInstallation_RadioButton.Text = "WebPack Installation";
            this.webPackInstallation_RadioButton.UseVisualStyleBackColor = true;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(18, 130);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(701, 241);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // HpEasyStartConfigControl
            // 
            this.Controls.Add(this.webPackInstallation_RadioButton);
            this.Controls.Add(this.fullInstallation_RadioButton);
            this.Controls.Add(this.checkBoxSetDefault);
            this.Controls.Add(this.checkBoxTestPage);
            this.Controls.Add(this.buttonPathSelection);
            this.Controls.Add(this.textBoxInstallerPath);
            this.Controls.Add(this.labelInstallerPath);
            this.Controls.Add(this.assetSelectionControl);
            this.Name = "HpEasyStartConfigControl";
            this.Size = new System.Drawing.Size(727, 420);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.Label labelInstallerPath;
        private System.Windows.Forms.TextBox textBoxInstallerPath;
        private System.Windows.Forms.Button buttonPathSelection;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.CheckBox checkBoxTestPage;
        private System.Windows.Forms.CheckBox checkBoxSetDefault;
        private System.Windows.Forms.RadioButton fullInstallation_RadioButton;
        private System.Windows.Forms.RadioButton webPackInstallation_RadioButton;
    }
}

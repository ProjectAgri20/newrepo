namespace HP.ScalableTest.Plugin.PaperCut
{
    partial class PaperCutConfigurationControl
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
            this.tabPage_Print = new System.Windows.Forms.TabPage();
            this.printingConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.tabPage_PullPrint = new System.Windows.Forms.TabPage();
            this.groupBoxMemoryPofile = new System.Windows.Forms.GroupBox();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_PullPrintConfig = new System.Windows.Forms.GroupBox();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.radioButton_PaperCut = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.checkBox_ReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabControl_Main = new System.Windows.Forms.TabControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabPage_Print.SuspendLayout();
            this.tabPage_PullPrint.SuspendLayout();
            this.groupBoxMemoryPofile.SuspendLayout();
            this.groupBox_PullPrintConfig.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_Device.SuspendLayout();
            this.tabControl_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage_Print
            // 
            this.tabPage_Print.Controls.Add(this.printingConfigurationControl);
            this.tabPage_Print.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Print.Name = "tabPage_Print";
            this.tabPage_Print.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Print.Size = new System.Drawing.Size(690, 528);
            this.tabPage_Print.TabIndex = 1;
            this.tabPage_Print.Text = "Printing";
            this.tabPage_Print.UseVisualStyleBackColor = true;
            // 
            // printingConfigurationControl
            // 
            this.printingConfigurationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printingConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingConfigurationControl.Location = new System.Drawing.Point(3, 3);
            this.printingConfigurationControl.Name = "printingConfigurationControl";
            this.printingConfigurationControl.Size = new System.Drawing.Size(684, 522);
            this.printingConfigurationControl.TabIndex = 2;
            // 
            // tabPage_PullPrint
            // 
            this.tabPage_PullPrint.Controls.Add(this.groupBoxMemoryPofile);
            this.tabPage_PullPrint.Controls.Add(this.groupBox_PullPrintConfig);
            this.tabPage_PullPrint.Controls.Add(this.groupBox_Authentication);
            this.tabPage_PullPrint.Controls.Add(this.groupBox_Device);
            this.tabPage_PullPrint.Location = new System.Drawing.Point(4, 22);
            this.tabPage_PullPrint.Name = "tabPage_PullPrint";
            this.tabPage_PullPrint.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_PullPrint.Size = new System.Drawing.Size(690, 528);
            this.tabPage_PullPrint.TabIndex = 0;
            this.tabPage_PullPrint.Text = "Pull Printing";
            this.tabPage_PullPrint.UseVisualStyleBackColor = true;
            // 
            // groupBoxMemoryPofile
            // 
            this.groupBoxMemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBoxMemoryPofile.Location = new System.Drawing.Point(6, 413);
            this.groupBoxMemoryPofile.Name = "groupBoxMemoryPofile";
            this.groupBoxMemoryPofile.Size = new System.Drawing.Size(678, 105);
            this.groupBoxMemoryPofile.TabIndex = 5;
            this.groupBoxMemoryPofile.TabStop = false;
            this.groupBoxMemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(7, 20);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(317, 84);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // groupBox_PullPrintConfig
            // 
            this.groupBox_PullPrintConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrintConfig.Controls.Add(this.checkBox_SelectAll);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrintConfig.Location = new System.Drawing.Point(6, 327);
            this.groupBox_PullPrintConfig.Name = "groupBox_PullPrintConfig";
            this.groupBox_PullPrintConfig.Size = new System.Drawing.Size(678, 80);
            this.groupBox_PullPrintConfig.TabIndex = 4;
            this.groupBox_PullPrintConfig.TabStop = false;
            this.groupBox_PullPrintConfig.Text = "Pull Print Configuration";
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(211, 36);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(56, 17);
            this.radioButton_Delete.TabIndex = 9;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Checked = true;
            this.checkBox_SelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(20, 37);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBox_SelectAll.TabIndex = 8;
            this.checkBox_SelectAll.Text = "Select All";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(122, 36);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(46, 17);
            this.radioButton_Print.TabIndex = 2;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.radioButton_PaperCut);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Location = new System.Drawing.Point(6, 221);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(678, 100);
            this.groupBox_Authentication.TabIndex = 3;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication Configuration";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(17, 64);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(114, 13);
            this.label_AuthMethod.TabIndex = 23;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(168, 61);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 21);
            this.comboBox_AuthProvider.TabIndex = 2;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // radioButton_PaperCut
            // 
            this.radioButton_PaperCut.AutoSize = true;
            this.radioButton_PaperCut.Location = new System.Drawing.Point(169, 33);
            this.radioButton_PaperCut.Name = "radioButton_PaperCut";
            this.radioButton_PaperCut.Size = new System.Drawing.Size(135, 17);
            this.radioButton_PaperCut.TabIndex = 1;
            this.radioButton_PaperCut.Text = "PaperCut Print Release";
            this.radioButton_PaperCut.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(20, 33);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(92, 17);
            this.radioButton_SignInButton.TabIndex = 0;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In Button";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // groupBox_Device
            // 
            this.groupBox_Device.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Device.Controls.Add(this.checkBox_ReleaseOnSignIn);
            this.groupBox_Device.Controls.Add(this.lockTimeoutControl);
            this.groupBox_Device.Controls.Add(this.assetSelectionControl);
            this.groupBox_Device.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Device.Name = "groupBox_Device";
            this.groupBox_Device.Size = new System.Drawing.Size(678, 209);
            this.groupBox_Device.TabIndex = 2;
            this.groupBox_Device.TabStop = false;
            this.groupBox_Device.Text = "Device Configuration";
            // 
            // checkBox_ReleaseOnSignIn
            // 
            this.checkBox_ReleaseOnSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_ReleaseOnSignIn.AutoSize = true;
            this.checkBox_ReleaseOnSignIn.Location = new System.Drawing.Point(316, 169);
            this.checkBox_ReleaseOnSignIn.Name = "checkBox_ReleaseOnSignIn";
            this.checkBox_ReleaseOnSignIn.Size = new System.Drawing.Size(275, 17);
            this.checkBox_ReleaseOnSignIn.TabIndex = 2;
            this.checkBox_ReleaseOnSignIn.Text = "Device is configured to release documents on sign in";
            this.checkBox_ReleaseOnSignIn.UseVisualStyleBackColor = true;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(7, 151);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 19);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(666, 126);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabControl_Main
            // 
            this.tabControl_Main.Controls.Add(this.tabPage_PullPrint);
            this.tabControl_Main.Controls.Add(this.tabPage_Print);
            this.tabControl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Main.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Main.Name = "tabControl_Main";
            this.tabControl_Main.SelectedIndex = 0;
            this.tabControl_Main.Size = new System.Drawing.Size(698, 554);
            this.tabControl_Main.TabIndex = 1;
            // 
            // PaperCutConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_Main);
            this.Name = "PaperCutConfigurationControl";
            this.Size = new System.Drawing.Size(698, 554);
            this.tabPage_Print.ResumeLayout(false);
            this.tabPage_PullPrint.ResumeLayout(false);
            this.groupBoxMemoryPofile.ResumeLayout(false);
            this.groupBox_PullPrintConfig.ResumeLayout(false);
            this.groupBox_PullPrintConfig.PerformLayout();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_Device.ResumeLayout(false);
            this.groupBox_Device.PerformLayout();
            this.tabControl_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage_Print;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingConfigurationControl;
        private System.Windows.Forms.TabPage tabPage_PullPrint;
        private System.Windows.Forms.GroupBox groupBoxMemoryPofile;
        private System.Windows.Forms.GroupBox groupBox_PullPrintConfig;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.RadioButton radioButton_PaperCut;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.TabControl tabControl_Main;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private System.Windows.Forms.CheckBox checkBox_ReleaseOnSignIn;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
    }
}

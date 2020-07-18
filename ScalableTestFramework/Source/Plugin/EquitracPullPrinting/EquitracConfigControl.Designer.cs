namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    partial class EquitracConfigControl
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
            this.tabControlEquitrac = new System.Windows.Forms.TabControl();
            this.tabPagePullPrint = new System.Windows.Forms.TabPage();
            this.groupBoxMemoryPofile = new System.Windows.Forms.GroupBox();
            this.groupBoxEquitracConfiguration = new System.Windows.Forms.GroupBox();
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.labelCopies = new System.Windows.Forms.Label();
            this.numericUpDownCopies = new System.Windows.Forms.NumericUpDown();
            this.radioButtonDelete = new System.Windows.Forms.RadioButton();
            this.radioButtonPrintSave = new System.Windows.Forms.RadioButton();
            this.radioButtonPrint = new System.Windows.Forms.RadioButton();
            this.checkBoxServerSelection = new System.Windows.Forms.CheckBox();
            this.groupBoxAuthentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.radioButtonEquitrac = new System.Windows.Forms.RadioButton();
            this.radioButtonSignInButton = new System.Windows.Forms.RadioButton();
            this.groupBoxDeviceConfiguration = new System.Windows.Forms.GroupBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.printingTaskConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.tabControlEquitrac.SuspendLayout();
            this.tabPagePullPrint.SuspendLayout();
            this.groupBoxMemoryPofile.SuspendLayout();
            this.groupBoxEquitracConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCopies)).BeginInit();
            this.groupBoxAuthentication.SuspendLayout();
            this.groupBoxDeviceConfiguration.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlEquitrac
            // 
            this.tabControlEquitrac.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEquitrac.Controls.Add(this.tabPagePullPrint);
            this.tabControlEquitrac.Controls.Add(this.tabPage2);
            this.tabControlEquitrac.Location = new System.Drawing.Point(4, 4);
            this.tabControlEquitrac.Name = "tabControlEquitrac";
            this.tabControlEquitrac.SelectedIndex = 0;
            this.tabControlEquitrac.Size = new System.Drawing.Size(685, 557);
            this.tabControlEquitrac.TabIndex = 0;
            // 
            // tabPagePullPrint
            // 
            this.tabPagePullPrint.Controls.Add(this.groupBoxMemoryPofile);
            this.tabPagePullPrint.Controls.Add(this.groupBoxEquitracConfiguration);
            this.tabPagePullPrint.Controls.Add(this.groupBoxAuthentication);
            this.tabPagePullPrint.Controls.Add(this.groupBoxDeviceConfiguration);
            this.tabPagePullPrint.Location = new System.Drawing.Point(4, 22);
            this.tabPagePullPrint.Name = "tabPagePullPrint";
            this.tabPagePullPrint.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePullPrint.Size = new System.Drawing.Size(677, 531);
            this.tabPagePullPrint.TabIndex = 0;
            this.tabPagePullPrint.Text = "Pull Printing";
            this.tabPagePullPrint.UseVisualStyleBackColor = true;
            // 
            // groupBoxMemoryPofile
            // 
            this.groupBoxMemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBoxMemoryPofile.Location = new System.Drawing.Point(4, 414);
            this.groupBoxMemoryPofile.Name = "groupBoxMemoryPofile";
            this.groupBoxMemoryPofile.Size = new System.Drawing.Size(667, 105);
            this.groupBoxMemoryPofile.TabIndex = 3;
            this.groupBoxMemoryPofile.TabStop = false;
            this.groupBoxMemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // groupBoxEquitracConfiguration
            // 
            this.groupBoxEquitracConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxEquitracConfiguration.Controls.Add(this.checkBoxSelectAll);
            this.groupBoxEquitracConfiguration.Controls.Add(this.labelCopies);
            this.groupBoxEquitracConfiguration.Controls.Add(this.numericUpDownCopies);
            this.groupBoxEquitracConfiguration.Controls.Add(this.radioButtonDelete);
            this.groupBoxEquitracConfiguration.Controls.Add(this.radioButtonPrintSave);
            this.groupBoxEquitracConfiguration.Controls.Add(this.radioButtonPrint);
            this.groupBoxEquitracConfiguration.Controls.Add(this.checkBoxServerSelection);
            this.groupBoxEquitracConfiguration.Location = new System.Drawing.Point(5, 329);
            this.groupBoxEquitracConfiguration.Name = "groupBoxEquitracConfiguration";
            this.groupBoxEquitracConfiguration.Size = new System.Drawing.Size(666, 80);
            this.groupBoxEquitracConfiguration.TabIndex = 2;
            this.groupBoxEquitracConfiguration.TabStop = false;
            this.groupBoxEquitracConfiguration.Text = "Pull Print Configuration";
            // 
            // checkBoxSelectAll
            // 
            this.checkBoxSelectAll.AutoSize = true;
            this.checkBoxSelectAll.Location = new System.Drawing.Point(7, 21);
            this.checkBoxSelectAll.Name = "checkBoxSelectAll";
            this.checkBoxSelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBoxSelectAll.TabIndex = 7;
            this.checkBoxSelectAll.Text = "Select All";
            this.checkBoxSelectAll.UseVisualStyleBackColor = true;
            // 
            // labelCopies
            // 
            this.labelCopies.AutoSize = true;
            this.labelCopies.Location = new System.Drawing.Point(214, 22);
            this.labelCopies.Name = "labelCopies";
            this.labelCopies.Size = new System.Drawing.Size(93, 13);
            this.labelCopies.TabIndex = 6;
            this.labelCopies.Text = "Number of copies:";
            // 
            // numericUpDownCopies
            // 
            this.numericUpDownCopies.Location = new System.Drawing.Point(313, 18);
            this.numericUpDownCopies.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCopies.Name = "numericUpDownCopies";
            this.numericUpDownCopies.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownCopies.TabIndex = 5;
            this.numericUpDownCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButtonDelete
            // 
            this.radioButtonDelete.AutoSize = true;
            this.radioButtonDelete.Location = new System.Drawing.Point(212, 53);
            this.radioButtonDelete.Name = "radioButtonDelete";
            this.radioButtonDelete.Size = new System.Drawing.Size(56, 17);
            this.radioButtonDelete.TabIndex = 3;
            this.radioButtonDelete.TabStop = true;
            this.radioButtonDelete.Text = "Delete";
            this.radioButtonDelete.UseVisualStyleBackColor = true;
            this.radioButtonDelete.CheckedChanged += new System.EventHandler(this.RadioButtonDocumentProcessCheckedChanged);
            // 
            // radioButtonPrintSave
            // 
            this.radioButtonPrintSave.AutoSize = true;
            this.radioButtonPrintSave.Location = new System.Drawing.Point(91, 53);
            this.radioButtonPrintSave.Name = "radioButtonPrintSave";
            this.radioButtonPrintSave.Size = new System.Drawing.Size(83, 17);
            this.radioButtonPrintSave.TabIndex = 2;
            this.radioButtonPrintSave.TabStop = true;
            this.radioButtonPrintSave.Text = "Print && Save";
            this.radioButtonPrintSave.UseVisualStyleBackColor = true;
            this.radioButtonPrintSave.CheckedChanged += new System.EventHandler(this.RadioButtonDocumentProcessCheckedChanged);
            // 
            // radioButtonPrint
            // 
            this.radioButtonPrint.AutoSize = true;
            this.radioButtonPrint.Location = new System.Drawing.Point(7, 53);
            this.radioButtonPrint.Name = "radioButtonPrint";
            this.radioButtonPrint.Size = new System.Drawing.Size(46, 17);
            this.radioButtonPrint.TabIndex = 1;
            this.radioButtonPrint.TabStop = true;
            this.radioButtonPrint.Text = "Print";
            this.radioButtonPrint.UseVisualStyleBackColor = true;
            this.radioButtonPrint.CheckedChanged += new System.EventHandler(this.RadioButtonDocumentProcessCheckedChanged);
            // 
            // checkBoxServerSelection
            // 
            this.checkBoxServerSelection.AutoSize = true;
            this.checkBoxServerSelection.Enabled = false;
            this.checkBoxServerSelection.Location = new System.Drawing.Point(91, 21);
            this.checkBoxServerSelection.Name = "checkBoxServerSelection";
            this.checkBoxServerSelection.Size = new System.Drawing.Size(104, 17);
            this.checkBoxServerSelection.TabIndex = 0;
            this.checkBoxServerSelection.Text = "Server Selection";
            this.checkBoxServerSelection.UseVisualStyleBackColor = true;
            // 
            // groupBoxAuthentication
            // 
            this.groupBoxAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAuthentication.Controls.Add(this.label_AuthMethod);
            this.groupBoxAuthentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBoxAuthentication.Controls.Add(this.radioButtonEquitrac);
            this.groupBoxAuthentication.Controls.Add(this.radioButtonSignInButton);
            this.groupBoxAuthentication.Location = new System.Drawing.Point(5, 224);
            this.groupBoxAuthentication.Name = "groupBoxAuthentication";
            this.groupBoxAuthentication.Size = new System.Drawing.Size(666, 100);
            this.groupBoxAuthentication.TabIndex = 1;
            this.groupBoxAuthentication.TabStop = false;
            this.groupBoxAuthentication.Text = "Authentication Configuration";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(17, 71);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(114, 13);
            this.label_AuthMethod.TabIndex = 25;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(168, 68);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 21);
            this.comboBox_AuthProvider.TabIndex = 24;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // radioButtonEquitrac
            // 
            this.radioButtonEquitrac.AutoSize = true;
            this.radioButtonEquitrac.Location = new System.Drawing.Point(162, 33);
            this.radioButtonEquitrac.Name = "radioButtonEquitrac";
            this.radioButtonEquitrac.Size = new System.Drawing.Size(157, 17);
            this.radioButtonEquitrac.TabIndex = 1;
            this.radioButtonEquitrac.Text = "Equitrac Follow-You Printing";
            this.radioButtonEquitrac.UseVisualStyleBackColor = true;
            // 
            // radioButtonSignInButton
            // 
            this.radioButtonSignInButton.AutoSize = true;
            this.radioButtonSignInButton.Checked = true;
            this.radioButtonSignInButton.Location = new System.Drawing.Point(20, 33);
            this.radioButtonSignInButton.Name = "radioButtonSignInButton";
            this.radioButtonSignInButton.Size = new System.Drawing.Size(92, 17);
            this.radioButtonSignInButton.TabIndex = 0;
            this.radioButtonSignInButton.TabStop = true;
            this.radioButtonSignInButton.Text = "Sign In Button";
            this.radioButtonSignInButton.UseVisualStyleBackColor = true;
            // 
            // groupBoxDeviceConfiguration
            // 
            this.groupBoxDeviceConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDeviceConfiguration.Controls.Add(this.lockTimeoutControl);
            this.groupBoxDeviceConfiguration.Controls.Add(this.assetSelectionControl);
            this.groupBoxDeviceConfiguration.Location = new System.Drawing.Point(5, 6);
            this.groupBoxDeviceConfiguration.Name = "groupBoxDeviceConfiguration";
            this.groupBoxDeviceConfiguration.Size = new System.Drawing.Size(666, 212);
            this.groupBoxDeviceConfiguration.TabIndex = 0;
            this.groupBoxDeviceConfiguration.TabStop = false;
            this.groupBoxDeviceConfiguration.Text = "Device Configuration";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(7, 154);
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
            this.assetSelectionControl.Size = new System.Drawing.Size(654, 129);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.printingTaskConfigurationControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(677, 531);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Printing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // printingTaskConfigurationControl
            // 
            this.printingTaskConfigurationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printingTaskConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingTaskConfigurationControl.Location = new System.Drawing.Point(3, 3);
            this.printingTaskConfigurationControl.Name = "printingTaskConfigurationControl";
            this.printingTaskConfigurationControl.Size = new System.Drawing.Size(671, 525);
            this.printingTaskConfigurationControl.TabIndex = 1;
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(7, 20);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(282, 84);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // EquitracConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlEquitrac);
            this.Name = "EquitracConfigControl";
            this.Size = new System.Drawing.Size(698, 574);
            this.tabControlEquitrac.ResumeLayout(false);
            this.tabPagePullPrint.ResumeLayout(false);
            this.groupBoxMemoryPofile.ResumeLayout(false);
            this.groupBoxEquitracConfiguration.ResumeLayout(false);
            this.groupBoxEquitracConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCopies)).EndInit();
            this.groupBoxAuthentication.ResumeLayout(false);
            this.groupBoxAuthentication.PerformLayout();
            this.groupBoxDeviceConfiguration.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlEquitrac;
        private System.Windows.Forms.TabPage tabPagePullPrint;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBoxDeviceConfiguration;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.GroupBox groupBoxAuthentication;
        private System.Windows.Forms.RadioButton radioButtonEquitrac;
        private System.Windows.Forms.RadioButton radioButtonSignInButton;
        private System.Windows.Forms.GroupBox groupBoxEquitracConfiguration;
        private System.Windows.Forms.RadioButton radioButtonDelete;
        private System.Windows.Forms.RadioButton radioButtonPrintSave;
        private System.Windows.Forms.RadioButton radioButtonPrint;
        private System.Windows.Forms.CheckBox checkBoxServerSelection;
        private System.Windows.Forms.GroupBox groupBoxMemoryPofile;
        private System.Windows.Forms.Label labelCopies;
        private System.Windows.Forms.NumericUpDown numericUpDownCopies;
        private System.Windows.Forms.CheckBox checkBoxSelectAll;
        private Framework.UI.FieldValidator fieldValidator;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingTaskConfigurationControl;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
    }
}

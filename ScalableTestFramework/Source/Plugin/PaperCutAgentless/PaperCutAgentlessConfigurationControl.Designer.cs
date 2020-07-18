namespace HP.ScalableTest.Plugin.PaperCutAgentless
{
    partial class PaperCutAgentlessConfigurationControl
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
            this.tabControl_Main = new System.Windows.Forms.TabControl();
            this.tabPage_PullPrinting = new System.Windows.Forms.TabPage();
            this.groupBox_MemoryPofile = new System.Windows.Forms.GroupBox();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_PullPrintConfig = new System.Windows.Forms.GroupBox();
            this.checkBox_UseSingleJobOptions = new System.Windows.Forms.CheckBox();
            this.checkBox_Force2sided = new System.Windows.Forms.CheckBox();
            this.checkBox_ForceGrayscale = new System.Windows.Forms.CheckBox();
            this.groupBox_SingleJobOptions = new System.Windows.Forms.GroupBox();
            this.panel_ColorMode = new System.Windows.Forms.Panel();
            this.radioButton_GrayScale = new System.Windows.Forms.RadioButton();
            this.label_ColorMode = new System.Windows.Forms.Label();
            this.radioButton_Color = new System.Windows.Forms.RadioButton();
            this.panel_duplex = new System.Windows.Forms.Panel();
            this.label_Duplex = new System.Windows.Forms.Label();
            this.radioButton_2sided = new System.Windows.Forms.RadioButton();
            this.radioButton_1sided = new System.Windows.Forms.RadioButton();
            this.numericUpDown_Copies = new System.Windows.Forms.NumericUpDown();
            this.label_Copies = new System.Windows.Forms.Label();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.checkBox_ReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabPage_Printing = new System.Windows.Forms.TabPage();
            this.printingConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabControl_Main.SuspendLayout();
            this.tabPage_PullPrinting.SuspendLayout();
            this.groupBox_MemoryPofile.SuspendLayout();
            this.groupBox_PullPrintConfig.SuspendLayout();
            this.groupBox_SingleJobOptions.SuspendLayout();
            this.panel_ColorMode.SuspendLayout();
            this.panel_duplex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Copies)).BeginInit();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_Device.SuspendLayout();
            this.tabPage_Printing.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Main
            // 
            this.tabControl_Main.Controls.Add(this.tabPage_PullPrinting);
            this.tabControl_Main.Controls.Add(this.tabPage_Printing);
            this.tabControl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Main.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Main.Name = "tabControl_Main";
            this.tabControl_Main.SelectedIndex = 0;
            this.tabControl_Main.Size = new System.Drawing.Size(698, 554);
            this.tabControl_Main.TabIndex = 0;
            // 
            // tabPage_PullPrinting
            // 
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_MemoryPofile);
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_PullPrintConfig);
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_Authentication);
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_Device);
            this.tabPage_PullPrinting.Location = new System.Drawing.Point(4, 24);
            this.tabPage_PullPrinting.Name = "tabPage_PullPrinting";
            this.tabPage_PullPrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_PullPrinting.Size = new System.Drawing.Size(690, 526);
            this.tabPage_PullPrinting.TabIndex = 0;
            this.tabPage_PullPrinting.Text = "Pull Printing";
            this.tabPage_PullPrinting.UseVisualStyleBackColor = true;
            // 
            // groupBox_MemoryPofile
            // 
            this.groupBox_MemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_MemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBox_MemoryPofile.Location = new System.Drawing.Point(6, 405);
            this.groupBox_MemoryPofile.Name = "groupBox_MemoryPofile";
            this.groupBox_MemoryPofile.Size = new System.Drawing.Size(678, 109);
            this.groupBox_MemoryPofile.TabIndex = 6;
            this.groupBox_MemoryPofile.TabStop = false;
            this.groupBox_MemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(6, 12);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(432, 91);
            this.deviceMemoryProfilerControl.TabIndex = 2;
            // 
            // groupBox_PullPrintConfig
            // 
            this.groupBox_PullPrintConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_PullPrintConfig.Controls.Add(this.checkBox_UseSingleJobOptions);
            this.groupBox_PullPrintConfig.Controls.Add(this.checkBox_Force2sided);
            this.groupBox_PullPrintConfig.Controls.Add(this.checkBox_ForceGrayscale);
            this.groupBox_PullPrintConfig.Controls.Add(this.groupBox_SingleJobOptions);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrintConfig.Controls.Add(this.checkBox_SelectAll);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrintConfig.Location = new System.Drawing.Point(6, 278);
            this.groupBox_PullPrintConfig.Name = "groupBox_PullPrintConfig";
            this.groupBox_PullPrintConfig.Size = new System.Drawing.Size(678, 121);
            this.groupBox_PullPrintConfig.TabIndex = 5;
            this.groupBox_PullPrintConfig.TabStop = false;
            this.groupBox_PullPrintConfig.Text = "Pull Print Configuration";
            // 
            // checkBox_UseSingleJobOptions
            // 
            this.checkBox_UseSingleJobOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_UseSingleJobOptions.AutoSize = true;
            this.checkBox_UseSingleJobOptions.Location = new System.Drawing.Point(389, 14);
            this.checkBox_UseSingleJobOptions.Name = "checkBox_UseSingleJobOptions";
            this.checkBox_UseSingleJobOptions.Size = new System.Drawing.Size(153, 19);
            this.checkBox_UseSingleJobOptions.TabIndex = 13;
            this.checkBox_UseSingleJobOptions.Text = "Using single job options";
            this.checkBox_UseSingleJobOptions.UseVisualStyleBackColor = true;
            this.checkBox_UseSingleJobOptions.CheckedChanged += new System.EventHandler(this.checkBox_UseSingleJobOptions_CheckedChanged);
            // 
            // checkBox_Force2sided
            // 
            this.checkBox_Force2sided.AutoSize = true;
            this.checkBox_Force2sided.Location = new System.Drawing.Point(132, 75);
            this.checkBox_Force2sided.Name = "checkBox_Force2sided";
            this.checkBox_Force2sided.Size = new System.Drawing.Size(97, 19);
            this.checkBox_Force2sided.TabIndex = 12;
            this.checkBox_Force2sided.Text = "Force 2-sided";
            this.checkBox_Force2sided.UseVisualStyleBackColor = true;
            // 
            // checkBox_ForceGrayscale
            // 
            this.checkBox_ForceGrayscale.AutoSize = true;
            this.checkBox_ForceGrayscale.Location = new System.Drawing.Point(132, 50);
            this.checkBox_ForceGrayscale.Name = "checkBox_ForceGrayscale";
            this.checkBox_ForceGrayscale.Size = new System.Drawing.Size(108, 19);
            this.checkBox_ForceGrayscale.TabIndex = 11;
            this.checkBox_ForceGrayscale.Text = "Force Grayscale";
            this.checkBox_ForceGrayscale.UseVisualStyleBackColor = true;
            // 
            // groupBox_SingleJobOptions
            // 
            this.groupBox_SingleJobOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_SingleJobOptions.Controls.Add(this.panel_ColorMode);
            this.groupBox_SingleJobOptions.Controls.Add(this.panel_duplex);
            this.groupBox_SingleJobOptions.Controls.Add(this.numericUpDown_Copies);
            this.groupBox_SingleJobOptions.Controls.Add(this.label_Copies);
            this.groupBox_SingleJobOptions.Enabled = false;
            this.groupBox_SingleJobOptions.Location = new System.Drawing.Point(380, 14);
            this.groupBox_SingleJobOptions.Name = "groupBox_SingleJobOptions";
            this.groupBox_SingleJobOptions.Size = new System.Drawing.Size(293, 101);
            this.groupBox_SingleJobOptions.TabIndex = 10;
            this.groupBox_SingleJobOptions.TabStop = false;
            // 
            // panel_ColorMode
            // 
            this.panel_ColorMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_ColorMode.Controls.Add(this.radioButton_GrayScale);
            this.panel_ColorMode.Controls.Add(this.label_ColorMode);
            this.panel_ColorMode.Controls.Add(this.radioButton_Color);
            this.panel_ColorMode.Location = new System.Drawing.Point(9, 71);
            this.panel_ColorMode.Name = "panel_ColorMode";
            this.panel_ColorMode.Size = new System.Drawing.Size(277, 25);
            this.panel_ColorMode.TabIndex = 7;
            // 
            // radioButton_GrayScale
            // 
            this.radioButton_GrayScale.AutoSize = true;
            this.radioButton_GrayScale.Location = new System.Drawing.Point(191, 3);
            this.radioButton_GrayScale.Name = "radioButton_GrayScale";
            this.radioButton_GrayScale.Size = new System.Drawing.Size(75, 19);
            this.radioButton_GrayScale.TabIndex = 4;
            this.radioButton_GrayScale.Text = "Grayscale";
            this.radioButton_GrayScale.UseVisualStyleBackColor = true;
            // 
            // label_ColorMode
            // 
            this.label_ColorMode.AutoSize = true;
            this.label_ColorMode.Location = new System.Drawing.Point(3, 5);
            this.label_ColorMode.Name = "label_ColorMode";
            this.label_ColorMode.Size = new System.Drawing.Size(70, 15);
            this.label_ColorMode.TabIndex = 5;
            this.label_ColorMode.Text = "Color Mode";
            // 
            // radioButton_Color
            // 
            this.radioButton_Color.AutoSize = true;
            this.radioButton_Color.Checked = true;
            this.radioButton_Color.Location = new System.Drawing.Point(121, 3);
            this.radioButton_Color.Name = "radioButton_Color";
            this.radioButton_Color.Size = new System.Drawing.Size(54, 19);
            this.radioButton_Color.TabIndex = 3;
            this.radioButton_Color.TabStop = true;
            this.radioButton_Color.Text = "Color";
            this.radioButton_Color.UseVisualStyleBackColor = true;
            // 
            // panel_duplex
            // 
            this.panel_duplex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_duplex.Controls.Add(this.label_Duplex);
            this.panel_duplex.Controls.Add(this.radioButton_2sided);
            this.panel_duplex.Controls.Add(this.radioButton_1sided);
            this.panel_duplex.Location = new System.Drawing.Point(9, 43);
            this.panel_duplex.Name = "panel_duplex";
            this.panel_duplex.Size = new System.Drawing.Size(277, 25);
            this.panel_duplex.TabIndex = 6;
            // 
            // label_Duplex
            // 
            this.label_Duplex.AutoSize = true;
            this.label_Duplex.Location = new System.Drawing.Point(3, 5);
            this.label_Duplex.Name = "label_Duplex";
            this.label_Duplex.Size = new System.Drawing.Size(43, 15);
            this.label_Duplex.TabIndex = 2;
            this.label_Duplex.Text = "Duplex";
            // 
            // radioButton_2sided
            // 
            this.radioButton_2sided.AutoSize = true;
            this.radioButton_2sided.Location = new System.Drawing.Point(191, 3);
            this.radioButton_2sided.Name = "radioButton_2sided";
            this.radioButton_2sided.Size = new System.Drawing.Size(64, 19);
            this.radioButton_2sided.TabIndex = 4;
            this.radioButton_2sided.Text = "2-sided";
            this.radioButton_2sided.UseVisualStyleBackColor = true;
            // 
            // radioButton_1sided
            // 
            this.radioButton_1sided.AutoSize = true;
            this.radioButton_1sided.Checked = true;
            this.radioButton_1sided.Location = new System.Drawing.Point(121, 3);
            this.radioButton_1sided.Name = "radioButton_1sided";
            this.radioButton_1sided.Size = new System.Drawing.Size(64, 19);
            this.radioButton_1sided.TabIndex = 3;
            this.radioButton_1sided.TabStop = true;
            this.radioButton_1sided.Text = "1-sided";
            this.radioButton_1sided.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_Copies
            // 
            this.numericUpDown_Copies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_Copies.Location = new System.Drawing.Point(172, 17);
            this.numericUpDown_Copies.Name = "numericUpDown_Copies";
            this.numericUpDown_Copies.Size = new System.Drawing.Size(114, 23);
            this.numericUpDown_Copies.TabIndex = 1;
            this.numericUpDown_Copies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label_Copies
            // 
            this.label_Copies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Copies.AutoSize = true;
            this.label_Copies.Location = new System.Drawing.Point(12, 19);
            this.label_Copies.Name = "label_Copies";
            this.label_Copies.Size = new System.Drawing.Size(43, 15);
            this.label_Copies.TabIndex = 0;
            this.label_Copies.Text = "Copies";
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(247, 23);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(58, 19);
            this.radioButton_Delete.TabIndex = 9;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            this.radioButton_Delete.CheckedChanged += new System.EventHandler(this.radioButton_DocumentAction_CheckedChanged);
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Checked = true;
            this.checkBox_SelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(21, 23);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(74, 19);
            this.checkBox_SelectAll.TabIndex = 8;
            this.checkBox_SelectAll.Text = "Select All";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(123, 22);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(50, 19);
            this.radioButton_Print.TabIndex = 2;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            this.radioButton_Print.CheckedChanged += new System.EventHandler(this.radioButton_DocumentAction_CheckedChanged);
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Location = new System.Drawing.Point(6, 216);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(678, 56);
            this.groupBox_Authentication.TabIndex = 4;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication Configuration";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(17, 28);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 23;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(168, 25);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 23);
            this.comboBox_AuthProvider.TabIndex = 2;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // groupBox_Device
            // 
            this.groupBox_Device.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Device.Controls.Add(this.checkBox_ReleaseOnSignIn);
            this.groupBox_Device.Controls.Add(this.lockTimeoutControl);
            this.groupBox_Device.Controls.Add(this.assetSelectionControl);
            this.groupBox_Device.Location = new System.Drawing.Point(6, 3);
            this.groupBox_Device.Name = "groupBox_Device";
            this.groupBox_Device.Size = new System.Drawing.Size(678, 207);
            this.groupBox_Device.TabIndex = 3;
            this.groupBox_Device.TabStop = false;
            this.groupBox_Device.Text = "Device Configuration";
            // 
            // checkBox_ReleaseOnSignIn
            // 
            this.checkBox_ReleaseOnSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_ReleaseOnSignIn.AutoSize = true;
            this.checkBox_ReleaseOnSignIn.Location = new System.Drawing.Point(316, 165);
            this.checkBox_ReleaseOnSignIn.Name = "checkBox_ReleaseOnSignIn";
            this.checkBox_ReleaseOnSignIn.Size = new System.Drawing.Size(304, 19);
            this.checkBox_ReleaseOnSignIn.TabIndex = 2;
            this.checkBox_ReleaseOnSignIn.Text = "Device is configured to release documents on sign in";
            this.checkBox_ReleaseOnSignIn.UseVisualStyleBackColor = true;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(7, 149);
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
            this.assetSelectionControl.Size = new System.Drawing.Size(666, 124);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabPage_Printing
            // 
            this.tabPage_Printing.Controls.Add(this.printingConfigurationControl);
            this.tabPage_Printing.Location = new System.Drawing.Point(4, 24);
            this.tabPage_Printing.Name = "tabPage_Printing";
            this.tabPage_Printing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Printing.Size = new System.Drawing.Size(690, 526);
            this.tabPage_Printing.TabIndex = 1;
            this.tabPage_Printing.Text = "Printing";
            this.tabPage_Printing.UseVisualStyleBackColor = true;
            // 
            // printingConfigurationControl
            // 
            this.printingConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingConfigurationControl.Location = new System.Drawing.Point(6, 6);
            this.printingConfigurationControl.Name = "printingConfigurationControl";
            this.printingConfigurationControl.Size = new System.Drawing.Size(667, 500);
            this.printingConfigurationControl.TabIndex = 0;
            // 
            // PaperCutAgentlessConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_Main);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PaperCutAgentlessConfigurationControl";
            this.Size = new System.Drawing.Size(698, 554);
            this.tabControl_Main.ResumeLayout(false);
            this.tabPage_PullPrinting.ResumeLayout(false);
            this.groupBox_MemoryPofile.ResumeLayout(false);
            this.groupBox_PullPrintConfig.ResumeLayout(false);
            this.groupBox_PullPrintConfig.PerformLayout();
            this.groupBox_SingleJobOptions.ResumeLayout(false);
            this.groupBox_SingleJobOptions.PerformLayout();
            this.panel_ColorMode.ResumeLayout(false);
            this.panel_ColorMode.PerformLayout();
            this.panel_duplex.ResumeLayout(false);
            this.panel_duplex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Copies)).EndInit();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_Device.ResumeLayout(false);
            this.groupBox_Device.PerformLayout();
            this.tabPage_Printing.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabControl tabControl_Main;
        private System.Windows.Forms.TabPage tabPage_PullPrinting;
        private System.Windows.Forms.TabPage tabPage_Printing;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private System.Windows.Forms.CheckBox checkBox_ReleaseOnSignIn;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.GroupBox groupBox_PullPrintConfig;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.GroupBox groupBox_MemoryPofile;
        private System.Windows.Forms.GroupBox groupBox_SingleJobOptions;
        private System.Windows.Forms.Label label_Copies;
        private System.Windows.Forms.NumericUpDown numericUpDown_Copies;
        private System.Windows.Forms.Label label_Duplex;
        private System.Windows.Forms.RadioButton radioButton_1sided;
        private System.Windows.Forms.RadioButton radioButton_2sided;
        private System.Windows.Forms.Label label_ColorMode;
        private System.Windows.Forms.Panel panel_duplex;
        private System.Windows.Forms.Panel panel_ColorMode;
        private System.Windows.Forms.RadioButton radioButton_GrayScale;
        private System.Windows.Forms.RadioButton radioButton_Color;
        private System.Windows.Forms.CheckBox checkBox_ForceGrayscale;
        private System.Windows.Forms.CheckBox checkBox_Force2sided;
        private System.Windows.Forms.CheckBox checkBox_UseSingleJobOptions;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingConfigurationControl;
    }
}

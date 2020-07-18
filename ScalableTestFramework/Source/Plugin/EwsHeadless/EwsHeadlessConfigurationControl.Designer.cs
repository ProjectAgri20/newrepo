namespace HP.ScalableTest.Plugin.EwsHeadless
{
    partial class EwsHeadlessConfigurationControl
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
            this.operation_comboBox = new System.Windows.Forms.ComboBox();
            this.operation_groupBox = new System.Windows.Forms.GroupBox();
            this.deviceType_label = new System.Windows.Forms.Label();
            this.deviceType_comboBox = new System.Windows.Forms.ComboBox();
            this.ews_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.operation_label = new System.Windows.Forms.Label();
            this.device_TabControl = new System.Windows.Forms.TabControl();
            this.Sirius_Tab = new System.Windows.Forms.TabPage();
            this.Sirius_groupBox = new System.Windows.Forms.GroupBox();
            this.siriusOperation_label = new System.Windows.Forms.Label();
            this.siriusOperation_comboBox = new System.Windows.Forms.ComboBox();
            this.Phoenix_Tab = new System.Windows.Forms.TabPage();
            this.Phoenix_groupBox = new System.Windows.Forms.GroupBox();
            this.phoenixOperation_label = new System.Windows.Forms.Label();
            this.phoenixOperation_comboBox = new System.Windows.Forms.ComboBox();
            this.Oz_Tab = new System.Windows.Forms.TabPage();
            this.Oz_groupBox = new System.Windows.Forms.GroupBox();
            this.ozOperation_label = new System.Windows.Forms.Label();
            this.ozOperation_comboBox = new System.Windows.Forms.ComboBox();
            this.Jedi_Tab = new System.Windows.Forms.TabPage();
            this.Jedi_groupBox = new System.Windows.Forms.GroupBox();
            this.jediOperation_label = new System.Windows.Forms.Label();
            this.jediOperation_comboBox = new System.Windows.Forms.ComboBox();
            this.ews_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.fieldValidator1 = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.Omni_Tab = new System.Windows.Forms.TabPage();
            this.Omni_groupBox = new System.Windows.Forms.GroupBox();
            this.omniOperation_label = new System.Windows.Forms.Label();
            this.omniOperation_comboBox = new System.Windows.Forms.ComboBox();
            this.operation_groupBox.SuspendLayout();
            this.device_TabControl.SuspendLayout();
            this.Sirius_Tab.SuspendLayout();
            this.Phoenix_Tab.SuspendLayout();
            this.Oz_Tab.SuspendLayout();
            this.Jedi_Tab.SuspendLayout();
            this.Omni_Tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // operation_comboBox
            // 
            this.operation_comboBox.FormattingEnabled = true;
            this.operation_comboBox.Location = new System.Drawing.Point(296, 169);
            this.operation_comboBox.Name = "operation_comboBox";
            this.operation_comboBox.Size = new System.Drawing.Size(247, 21);
            this.operation_comboBox.TabIndex = 0;
            this.operation_comboBox.SelectedIndexChanged += new System.EventHandler(this.operation_comboBox_SelectedIndexChanged);
            // 
            // operation_groupBox
            // 
            this.operation_groupBox.Controls.Add(this.deviceType_label);
            this.operation_groupBox.Controls.Add(this.deviceType_comboBox);
            this.operation_groupBox.Controls.Add(this.ews_assetSelectionControl);
            this.operation_groupBox.Controls.Add(this.operation_label);
            this.operation_groupBox.Controls.Add(this.operation_comboBox);
            this.operation_groupBox.Location = new System.Drawing.Point(3, 3);
            this.operation_groupBox.Name = "operation_groupBox";
            this.operation_groupBox.Size = new System.Drawing.Size(561, 198);
            this.operation_groupBox.TabIndex = 1;
            this.operation_groupBox.TabStop = false;
            this.operation_groupBox.Text = "Printer";
            // 
            // deviceType_label
            // 
            this.deviceType_label.AutoSize = true;
            this.deviceType_label.Location = new System.Drawing.Point(6, 172);
            this.deviceType_label.Name = "deviceType_label";
            this.deviceType_label.Size = new System.Drawing.Size(71, 13);
            this.deviceType_label.TabIndex = 70;
            this.deviceType_label.Text = "Device Type:";
            // 
            // deviceType_comboBox
            // 
            this.deviceType_comboBox.FormattingEnabled = true;
            this.deviceType_comboBox.Location = new System.Drawing.Point(86, 169);
            this.deviceType_comboBox.Name = "deviceType_comboBox";
            this.deviceType_comboBox.Size = new System.Drawing.Size(96, 21);
            this.deviceType_comboBox.TabIndex = 69;
            this.deviceType_comboBox.SelectedIndexChanged += new System.EventHandler(this.deviceType_comboBox_SelectedIndexChanged);
            // 
            // ews_assetSelectionControl
            // 
            this.ews_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ews_assetSelectionControl.Location = new System.Drawing.Point(10, 19);
            this.ews_assetSelectionControl.Name = "ews_assetSelectionControl";
            this.ews_assetSelectionControl.Size = new System.Drawing.Size(534, 144);
            this.ews_assetSelectionControl.TabIndex = 57;
            // 
            // operation_label
            // 
            this.operation_label.AutoSize = true;
            this.operation_label.Location = new System.Drawing.Point(198, 172);
            this.operation_label.Name = "operation_label";
            this.operation_label.Size = new System.Drawing.Size(92, 13);
            this.operation_label.TabIndex = 56;
            this.operation_label.Text = "Select Operation :";
            // 
            // device_TabControl
            // 
            this.device_TabControl.Controls.Add(this.Sirius_Tab);
            this.device_TabControl.Controls.Add(this.Phoenix_Tab);
            this.device_TabControl.Controls.Add(this.Oz_Tab);
            this.device_TabControl.Controls.Add(this.Jedi_Tab);
            this.device_TabControl.Controls.Add(this.Omni_Tab);
            this.device_TabControl.Location = new System.Drawing.Point(3, 207);
            this.device_TabControl.Name = "device_TabControl";
            this.device_TabControl.SelectedIndex = 0;
            this.device_TabControl.Size = new System.Drawing.Size(564, 330);
            this.device_TabControl.TabIndex = 3;
            // 
            // Sirius_Tab
            // 
            this.Sirius_Tab.AutoScroll = true;
            this.Sirius_Tab.Controls.Add(this.Sirius_groupBox);
            this.Sirius_Tab.Controls.Add(this.siriusOperation_label);
            this.Sirius_Tab.Controls.Add(this.siriusOperation_comboBox);
            this.Sirius_Tab.Location = new System.Drawing.Point(4, 22);
            this.Sirius_Tab.Name = "Sirius_Tab";
            this.Sirius_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Sirius_Tab.Size = new System.Drawing.Size(556, 304);
            this.Sirius_Tab.TabIndex = 0;
            this.Sirius_Tab.Text = "Sirius";
            this.Sirius_Tab.UseVisualStyleBackColor = true;
            // 
            // Sirius_groupBox
            // 
            this.Sirius_groupBox.AutoSize = true;
            this.Sirius_groupBox.Location = new System.Drawing.Point(6, 38);
            this.Sirius_groupBox.Name = "Sirius_groupBox";
            this.Sirius_groupBox.Size = new System.Drawing.Size(547, 263);
            this.Sirius_groupBox.TabIndex = 61;
            this.Sirius_groupBox.TabStop = false;
            this.Sirius_groupBox.Text = "Sirius Inputs";
            // 
            // siriusOperation_label
            // 
            this.siriusOperation_label.AutoSize = true;
            this.siriusOperation_label.Location = new System.Drawing.Point(6, 15);
            this.siriusOperation_label.Name = "siriusOperation_label";
            this.siriusOperation_label.Size = new System.Drawing.Size(87, 13);
            this.siriusOperation_label.TabIndex = 58;
            this.siriusOperation_label.Text = "Sirius Operation :";
            // 
            // siriusOperation_comboBox
            // 
            this.siriusOperation_comboBox.FormattingEnabled = true;
            this.siriusOperation_comboBox.Location = new System.Drawing.Point(104, 12);
            this.siriusOperation_comboBox.Name = "siriusOperation_comboBox";
            this.siriusOperation_comboBox.Size = new System.Drawing.Size(191, 21);
            this.siriusOperation_comboBox.TabIndex = 57;
            this.siriusOperation_comboBox.SelectedIndexChanged += new System.EventHandler(this.siriusOperation_comboBox_SelectedIndexChanged);
            // 
            // Phoenix_Tab
            // 
            this.Phoenix_Tab.AutoScroll = true;
            this.Phoenix_Tab.Controls.Add(this.Phoenix_groupBox);
            this.Phoenix_Tab.Controls.Add(this.phoenixOperation_label);
            this.Phoenix_Tab.Controls.Add(this.phoenixOperation_comboBox);
            this.Phoenix_Tab.Location = new System.Drawing.Point(4, 22);
            this.Phoenix_Tab.Name = "Phoenix_Tab";
            this.Phoenix_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Phoenix_Tab.Size = new System.Drawing.Size(556, 304);
            this.Phoenix_Tab.TabIndex = 1;
            this.Phoenix_Tab.Text = "Phoenix";
            this.Phoenix_Tab.UseVisualStyleBackColor = true;
            // 
            // Phoenix_groupBox
            // 
            this.Phoenix_groupBox.AutoSize = true;
            this.Phoenix_groupBox.Location = new System.Drawing.Point(8, 38);
            this.Phoenix_groupBox.Name = "Phoenix_groupBox";
            this.Phoenix_groupBox.Size = new System.Drawing.Size(545, 249);
            this.Phoenix_groupBox.TabIndex = 60;
            this.Phoenix_groupBox.TabStop = false;
            this.Phoenix_groupBox.Text = "Phoenix Inputs";
            // 
            // phoenixOperation_label
            // 
            this.phoenixOperation_label.AutoSize = true;
            this.phoenixOperation_label.Location = new System.Drawing.Point(2, 14);
            this.phoenixOperation_label.Name = "phoenixOperation_label";
            this.phoenixOperation_label.Size = new System.Drawing.Size(100, 13);
            this.phoenixOperation_label.TabIndex = 58;
            this.phoenixOperation_label.Text = "Phoenix Operation :";
            // 
            // phoenixOperation_comboBox
            // 
            this.phoenixOperation_comboBox.FormattingEnabled = true;
            this.phoenixOperation_comboBox.Location = new System.Drawing.Point(113, 11);
            this.phoenixOperation_comboBox.Name = "phoenixOperation_comboBox";
            this.phoenixOperation_comboBox.Size = new System.Drawing.Size(191, 21);
            this.phoenixOperation_comboBox.TabIndex = 57;
            this.phoenixOperation_comboBox.SelectedIndexChanged += new System.EventHandler(this.phoenixOperation_comboBox_SelectedIndexChanged);
            // 
            // Oz_Tab
            // 
            this.Oz_Tab.AutoScroll = true;
            this.Oz_Tab.Controls.Add(this.Oz_groupBox);
            this.Oz_Tab.Controls.Add(this.ozOperation_label);
            this.Oz_Tab.Controls.Add(this.ozOperation_comboBox);
            this.Oz_Tab.Location = new System.Drawing.Point(4, 22);
            this.Oz_Tab.Name = "Oz_Tab";
            this.Oz_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Oz_Tab.Size = new System.Drawing.Size(556, 304);
            this.Oz_Tab.TabIndex = 2;
            this.Oz_Tab.Text = "Oz";
            this.Oz_Tab.UseVisualStyleBackColor = true;
            // 
            // Oz_groupBox
            // 
            this.Oz_groupBox.AutoSize = true;
            this.Oz_groupBox.Location = new System.Drawing.Point(5, 39);
            this.Oz_groupBox.Name = "Oz_groupBox";
            this.Oz_groupBox.Size = new System.Drawing.Size(533, 265);
            this.Oz_groupBox.TabIndex = 60;
            this.Oz_groupBox.TabStop = false;
            this.Oz_groupBox.Text = "Oz Inputs";
            // 
            // ozOperation_label
            // 
            this.ozOperation_label.AutoSize = true;
            this.ozOperation_label.Location = new System.Drawing.Point(6, 15);
            this.ozOperation_label.Name = "ozOperation_label";
            this.ozOperation_label.Size = new System.Drawing.Size(75, 13);
            this.ozOperation_label.TabIndex = 58;
            this.ozOperation_label.Text = "Oz Operation :";
            // 
            // ozOperation_comboBox
            // 
            this.ozOperation_comboBox.FormattingEnabled = true;
            this.ozOperation_comboBox.Location = new System.Drawing.Point(104, 12);
            this.ozOperation_comboBox.Name = "ozOperation_comboBox";
            this.ozOperation_comboBox.Size = new System.Drawing.Size(191, 21);
            this.ozOperation_comboBox.TabIndex = 57;
            this.ozOperation_comboBox.SelectedIndexChanged += new System.EventHandler(this.ozOperation_comboBox_SelectedIndexChanged);
            // 
            // Jedi_Tab
            // 
            this.Jedi_Tab.AutoScroll = true;
            this.Jedi_Tab.Controls.Add(this.Jedi_groupBox);
            this.Jedi_Tab.Controls.Add(this.jediOperation_label);
            this.Jedi_Tab.Controls.Add(this.jediOperation_comboBox);
            this.Jedi_Tab.Location = new System.Drawing.Point(4, 22);
            this.Jedi_Tab.Name = "Jedi_Tab";
            this.Jedi_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Jedi_Tab.Size = new System.Drawing.Size(556, 304);
            this.Jedi_Tab.TabIndex = 3;
            this.Jedi_Tab.Text = "Jedi";
            this.Jedi_Tab.UseVisualStyleBackColor = true;
            // 
            // Jedi_groupBox
            // 
            this.Jedi_groupBox.AutoSize = true;
            this.Jedi_groupBox.Location = new System.Drawing.Point(9, 37);
            this.Jedi_groupBox.Name = "Jedi_groupBox";
            this.Jedi_groupBox.Size = new System.Drawing.Size(529, 264);
            this.Jedi_groupBox.TabIndex = 60;
            this.Jedi_groupBox.TabStop = false;
            this.Jedi_groupBox.Text = "Jedi Inputs";
            // 
            // jediOperation_label
            // 
            this.jediOperation_label.AutoSize = true;
            this.jediOperation_label.Location = new System.Drawing.Point(6, 13);
            this.jediOperation_label.Name = "jediOperation_label";
            this.jediOperation_label.Size = new System.Drawing.Size(81, 13);
            this.jediOperation_label.TabIndex = 58;
            this.jediOperation_label.Text = "Jedi Operation :";
            // 
            // jediOperation_comboBox
            // 
            this.jediOperation_comboBox.FormattingEnabled = true;
            this.jediOperation_comboBox.Location = new System.Drawing.Point(104, 10);
            this.jediOperation_comboBox.Name = "jediOperation_comboBox";
            this.jediOperation_comboBox.Size = new System.Drawing.Size(191, 21);
            this.jediOperation_comboBox.TabIndex = 57;
            this.jediOperation_comboBox.SelectedIndexChanged += new System.EventHandler(this.jediOperation_comboBox_SelectedIndexChanged);
            // 
            // Omni_Tab
            // 
            this.Omni_Tab.AutoScroll = true;
            this.Omni_Tab.Controls.Add(this.Omni_groupBox);
            this.Omni_Tab.Controls.Add(this.omniOperation_label);
            this.Omni_Tab.Controls.Add(this.omniOperation_comboBox);
            this.Omni_Tab.Location = new System.Drawing.Point(4, 22);
            this.Omni_Tab.Name = "Omni_Tab";
            this.Omni_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.Omni_Tab.Size = new System.Drawing.Size(556, 304);
            this.Omni_Tab.TabIndex = 4;
            this.Omni_Tab.Text = "Omni";
            this.Omni_Tab.UseVisualStyleBackColor = true;
            // 
            // Omni_groupBox
            // 
            this.Omni_groupBox.AutoSize = true;
            this.Omni_groupBox.Location = new System.Drawing.Point(15, 34);
            this.Omni_groupBox.Name = "Omni_groupBox";
            this.Omni_groupBox.Size = new System.Drawing.Size(529, 264);
            this.Omni_groupBox.TabIndex = 63;
            this.Omni_groupBox.TabStop = false;
            this.Omni_groupBox.Text = "Omni Inputs";
            // 
            // OmniOperation_label
            // 
            this.omniOperation_label.AutoSize = true;
            this.omniOperation_label.Location = new System.Drawing.Point(12, 10);
            this.omniOperation_label.Name = "OmniOperation_label";
            this.omniOperation_label.Size = new System.Drawing.Size(86, 13);
            this.omniOperation_label.TabIndex = 62;
            this.omniOperation_label.Text = "Omni Operation :";
            // 
            // OmniOperation_comboBox
            // 
            this.omniOperation_comboBox.FormattingEnabled = true;
            this.omniOperation_comboBox.Location = new System.Drawing.Point(110, 7);
            this.omniOperation_comboBox.Name = "OmniOperation_comboBox";
            this.omniOperation_comboBox.Size = new System.Drawing.Size(191, 21);
            this.omniOperation_comboBox.TabIndex = 61;
            this.omniOperation_comboBox.SelectedIndexChanged += new System.EventHandler(this.omniOperation_comboBox_SelectedIndexChanged);
            // 
            // EwsHeadlessConfigurationControl
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonMenu;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.device_TabControl);
            this.Controls.Add(this.operation_groupBox);
            this.Name = "EwsHeadlessConfigurationControl";
            this.Size = new System.Drawing.Size(575, 549);
            this.operation_groupBox.ResumeLayout(false);
            this.operation_groupBox.PerformLayout();
            this.device_TabControl.ResumeLayout(false);
            this.Sirius_Tab.ResumeLayout(false);
            this.Sirius_Tab.PerformLayout();
            this.Phoenix_Tab.ResumeLayout(false);
            this.Phoenix_Tab.PerformLayout();
            this.Oz_Tab.ResumeLayout(false);
            this.Oz_Tab.PerformLayout();
            this.Jedi_Tab.ResumeLayout(false);
            this.Jedi_Tab.PerformLayout();
            this.Omni_Tab.ResumeLayout(false);
            this.Omni_Tab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox operation_comboBox;
        private System.Windows.Forms.GroupBox operation_groupBox;
        private System.Windows.Forms.Label operation_label;
        private System.Windows.Forms.TabControl device_TabControl;
        private System.Windows.Forms.TabPage Sirius_Tab;
        private System.Windows.Forms.TabPage Phoenix_Tab;
        private System.Windows.Forms.TabPage Oz_Tab;
        private System.Windows.Forms.TabPage Jedi_Tab;
        private System.Windows.Forms.Label siriusOperation_label;
        private System.Windows.Forms.ComboBox siriusOperation_comboBox;
        private System.Windows.Forms.Label phoenixOperation_label;
        private System.Windows.Forms.ComboBox phoenixOperation_comboBox;
        private System.Windows.Forms.Label ozOperation_label;
        private System.Windows.Forms.ComboBox ozOperation_comboBox;
        private System.Windows.Forms.Label jediOperation_label;
        private System.Windows.Forms.ComboBox jediOperation_comboBox;
        private System.Windows.Forms.GroupBox Phoenix_groupBox;
        private System.Windows.Forms.GroupBox Oz_groupBox;
        private System.Windows.Forms.GroupBox Jedi_groupBox;
        private System.Windows.Forms.GroupBox Sirius_groupBox;
        private Framework.UI.FieldValidator ews_fieldValidator;
        private Framework.UI.AssetSelectionControl ews_assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator1;
        private System.Windows.Forms.Label deviceType_label;
        private System.Windows.Forms.ComboBox deviceType_comboBox;
        private System.Windows.Forms.TabPage Omni_Tab;
        private System.Windows.Forms.GroupBox Omni_groupBox;
        private System.Windows.Forms.Label omniOperation_label;
        private System.Windows.Forms.ComboBox omniOperation_comboBox;
    }
}

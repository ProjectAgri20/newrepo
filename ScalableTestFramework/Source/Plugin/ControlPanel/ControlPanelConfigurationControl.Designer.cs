namespace HP.ScalableTest.Plugin.ControlPanel
{
    partial class ControlPanelConfigurationControl
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.controlPanel_groupBox = new System.Windows.Forms.GroupBox();
            this.description_textBox = new System.Windows.Forms.TextBox();
            this.controlpaneltype_label = new System.Windows.Forms.Label();
            this.controlpaneltypes_comboBox = new System.Windows.Forms.ComboBox();
            this.settings_groupBox = new System.Windows.Forms.GroupBox();
            this.controlPanelOptions_comboBox = new System.Windows.Forms.ComboBox();
            this.controlpaneloptions_label = new System.Windows.Forms.Label();
            this.controlpanel_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.controlpanel_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.controlPanel_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlPanel_groupBox
            // 
            this.controlPanel_groupBox.Controls.Add(this.description_textBox);
            this.controlPanel_groupBox.Controls.Add(this.controlpaneltype_label);
            this.controlPanel_groupBox.Controls.Add(this.controlpaneltypes_comboBox);
            this.controlPanel_groupBox.Controls.Add(this.settings_groupBox);
            this.controlPanel_groupBox.Controls.Add(this.controlPanelOptions_comboBox);
            this.controlPanel_groupBox.Controls.Add(this.controlpaneloptions_label);
            this.controlPanel_groupBox.Location = new System.Drawing.Point(11, 211);
            this.controlPanel_groupBox.Name = "controlPanel_groupBox";
            this.controlPanel_groupBox.Size = new System.Drawing.Size(626, 292);
            this.controlPanel_groupBox.TabIndex = 54;
            this.controlPanel_groupBox.TabStop = false;
            this.controlPanel_groupBox.Text = " ";
            // 
            // description_textBox
            // 
            this.description_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.description_textBox.Location = new System.Drawing.Point(121, 50);
            this.description_textBox.Name = "description_textBox";
            this.description_textBox.ReadOnly = true;
            this.description_textBox.Size = new System.Drawing.Size(482, 13);
            this.description_textBox.TabIndex = 55;
            this.description_textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // controlpaneltype_label
            // 
            this.controlpaneltype_label.AutoSize = true;
            this.controlpaneltype_label.Location = new System.Drawing.Point(6, 16);
            this.controlpaneltype_label.Name = "controlpaneltype_label";
            this.controlpaneltype_label.Size = new System.Drawing.Size(100, 13);
            this.controlpaneltype_label.TabIndex = 54;
            this.controlpaneltype_label.Text = "Control Panel Type:";
            // 
            // controlpaneltypes_comboBox
            // 
            this.controlpaneltypes_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controlpaneltypes_comboBox.FormattingEnabled = true;
            this.controlpaneltypes_comboBox.Items.AddRange(new object[] {
            "Jedi",
            "Omni",
            "Phoenix",
            "SiriusPentane",
            "SiriusTriptane"});
            this.controlpaneltypes_comboBox.Location = new System.Drawing.Point(121, 13);
            this.controlpaneltypes_comboBox.Name = "controlpaneltypes_comboBox";
            this.controlpaneltypes_comboBox.Size = new System.Drawing.Size(130, 21);
            this.controlpaneltypes_comboBox.TabIndex = 53;
            this.controlpaneltypes_comboBox.SelectedIndexChanged += new System.EventHandler(this.controlpaneltypes_comboBox_SelectedIndexChanged);
            // 
            // settings_groupBox
            // 
            this.settings_groupBox.Location = new System.Drawing.Point(9, 66);
            this.settings_groupBox.Name = "settings_groupBox";
            this.settings_groupBox.Size = new System.Drawing.Size(594, 220);
            this.settings_groupBox.TabIndex = 52;
            this.settings_groupBox.TabStop = false;
            // 
            // controlPanelOptions_comboBox
            // 
            this.controlPanelOptions_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controlPanelOptions_comboBox.FormattingEnabled = true;
            this.controlPanelOptions_comboBox.Location = new System.Drawing.Point(349, 13);
            this.controlPanelOptions_comboBox.Name = "controlPanelOptions_comboBox";
            this.controlPanelOptions_comboBox.Size = new System.Drawing.Size(254, 21);
            this.controlPanelOptions_comboBox.TabIndex = 11;
            this.controlPanelOptions_comboBox.SelectedIndexChanged += new System.EventHandler(this.controlPanelOptions_comboBox_SelectedIndexChanged);
            // 
            // controlpaneloptions_label
            // 
            this.controlpaneloptions_label.AutoSize = true;
            this.controlpaneloptions_label.Location = new System.Drawing.Point(266, 16);
            this.controlpaneloptions_label.Name = "controlpaneloptions_label";
            this.controlpaneloptions_label.Size = new System.Drawing.Size(60, 13);
            this.controlpaneloptions_label.TabIndex = 47;
            this.controlpaneloptions_label.Text = "Workflows:";
            // 
            // controlpanel_assetSelectionControl
            // 
            this.controlpanel_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlpanel_assetSelectionControl.Location = new System.Drawing.Point(11, 12);
            this.controlpanel_assetSelectionControl.Name = "controlpanel_assetSelectionControl";
            this.controlpanel_assetSelectionControl.Size = new System.Drawing.Size(626, 180);
            this.controlpanel_assetSelectionControl.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(325, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "NOTE: Only the first device (enabled) will be selected for execution.";
            // 
            // ControlPanelConfigurationControl
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.controlpanel_assetSelectionControl);
            this.Controls.Add(this.controlPanel_groupBox);
            this.Name = "ControlPanelConfigurationControl";
            this.Size = new System.Drawing.Size(640, 521);
            this.controlPanel_groupBox.ResumeLayout(false);
            this.controlPanel_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.Windows.Forms.GroupBox controlPanel_groupBox;
        private System.Windows.Forms.ComboBox controlPanelOptions_comboBox;
        private System.Windows.Forms.Label controlpaneloptions_label;
        private System.Windows.Forms.GroupBox settings_groupBox;
        private Framework.UI.AssetSelectionControl controlpanel_assetSelectionControl;
        private Framework.UI.FieldValidator controlpanel_fieldValidator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label controlpaneltype_label;
        private System.Windows.Forms.ComboBox controlpaneltypes_comboBox;
        private System.Windows.Forms.TextBox description_textBox;
    }
}

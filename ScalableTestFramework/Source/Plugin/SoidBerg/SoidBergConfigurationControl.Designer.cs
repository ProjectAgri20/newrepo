namespace HP.ScalableTest.Plugin.SoidBerg
{
    partial class SoidBergConfigurationControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.soid_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.label1 = new System.Windows.Forms.Label();
            this.Comments = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.snmpvalues_groupBox = new System.Windows.Forms.GroupBox();
            this.oidtype_comboBox = new System.Windows.Forms.ComboBox();
            this.Setvalue_label = new System.Windows.Forms.Label();
            this.oidvalue_textbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Custom_oid_label = new System.Windows.Forms.Label();
            this.Comments_label = new System.Windows.Forms.Label();
            this.Snmpoid_label = new System.Windows.Forms.Label();
            this.snmpcomments_textbox = new System.Windows.Forms.TextBox();
            this.snmp_textBox = new System.Windows.Forms.TextBox();
            this.snmpCustom_radioButton = new System.Windows.Forms.RadioButton();
            this.snmp_comboBox = new System.Windows.Forms.ComboBox();
            this.snmpCombo_radioButton = new System.Windows.Forms.RadioButton();
            this.soid_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.groupBox1.SuspendLayout();
            this.snmpvalues_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // soid_assetSelectionControl
            // 
            this.soid_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.soid_assetSelectionControl.Location = new System.Drawing.Point(3, 3);
            this.soid_assetSelectionControl.Name = "soid_assetSelectionControl";
            this.soid_assetSelectionControl.Size = new System.Drawing.Size(597, 306);
            this.soid_assetSelectionControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 367);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // Comments
            // 
            this.Comments.AutoSize = true;
            this.Comments.Location = new System.Drawing.Point(3, 422);
            this.Comments.Name = "Comments";
            this.Comments.Size = new System.Drawing.Size(0, 13);
            this.Comments.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.snmpvalues_groupBox);
            this.groupBox1.Controls.Add(this.Custom_oid_label);
            this.groupBox1.Controls.Add(this.Comments_label);
            this.groupBox1.Controls.Add(this.Snmpoid_label);
            this.groupBox1.Controls.Add(this.snmpcomments_textbox);
            this.groupBox1.Controls.Add(this.snmp_textBox);
            this.groupBox1.Controls.Add(this.snmpCustom_radioButton);
            this.groupBox1.Controls.Add(this.snmp_comboBox);
            this.groupBox1.Controls.Add(this.snmpCombo_radioButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 141);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SNMP Command";
            // 
            // snmpvalues_groupBox
            // 
            this.snmpvalues_groupBox.Controls.Add(this.oidtype_comboBox);
            this.snmpvalues_groupBox.Controls.Add(this.Setvalue_label);
            this.snmpvalues_groupBox.Controls.Add(this.oidvalue_textbox);
            this.snmpvalues_groupBox.Controls.Add(this.label2);
            this.snmpvalues_groupBox.Location = new System.Drawing.Point(379, 19);
            this.snmpvalues_groupBox.Name = "snmpvalues_groupBox";
            this.snmpvalues_groupBox.Size = new System.Drawing.Size(210, 115);
            this.snmpvalues_groupBox.TabIndex = 24;
            this.snmpvalues_groupBox.TabStop = false;
            // 
            // oidtype_comboBox
            // 
            this.oidtype_comboBox.FormattingEnabled = true;
            this.oidtype_comboBox.Location = new System.Drawing.Point(6, 26);
            this.oidtype_comboBox.Name = "oidtype_comboBox";
            this.oidtype_comboBox.Size = new System.Drawing.Size(203, 21);
            this.oidtype_comboBox.TabIndex = 22;
            this.oidtype_comboBox.SelectedIndexChanged += new System.EventHandler(this.oidtype_comboBox_SelectedIndexChanged);
            // 
            // Setvalue_label
            // 
            this.Setvalue_label.AutoSize = true;
            this.Setvalue_label.Location = new System.Drawing.Point(6, 56);
            this.Setvalue_label.Name = "Setvalue_label";
            this.Setvalue_label.Size = new System.Drawing.Size(56, 13);
            this.Setvalue_label.TabIndex = 20;
            this.Setvalue_label.Text = "OID Value";
            // 
            // oidvalue_textbox
            // 
            this.oidvalue_textbox.Location = new System.Drawing.Point(6, 70);
            this.oidvalue_textbox.Multiline = true;
            this.oidvalue_textbox.Name = "oidvalue_textbox";
            this.oidvalue_textbox.Size = new System.Drawing.Size(203, 20);
            this.oidvalue_textbox.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "OID Type";
            // 
            // Custom_oid_label
            // 
            this.Custom_oid_label.AutoSize = true;
            this.Custom_oid_label.Location = new System.Drawing.Point(160, 26);
            this.Custom_oid_label.Name = "Custom_oid_label";
            this.Custom_oid_label.Size = new System.Drawing.Size(64, 13);
            this.Custom_oid_label.TabIndex = 12;
            this.Custom_oid_label.Text = "Custom OID";
            // 
            // Comments_label
            // 
            this.Comments_label.AutoSize = true;
            this.Comments_label.Location = new System.Drawing.Point(6, 75);
            this.Comments_label.Name = "Comments_label";
            this.Comments_label.Size = new System.Drawing.Size(56, 13);
            this.Comments_label.TabIndex = 11;
            this.Comments_label.Text = "Comments";
            // 
            // Snmpoid_label
            // 
            this.Snmpoid_label.AutoSize = true;
            this.Snmpoid_label.Location = new System.Drawing.Point(6, 26);
            this.Snmpoid_label.Name = "Snmpoid_label";
            this.Snmpoid_label.Size = new System.Drawing.Size(72, 13);
            this.Snmpoid_label.TabIndex = 9;
            this.Snmpoid_label.Text = "Standard OID";
            // 
            // snmpcomments_textbox
            // 
            this.snmpcomments_textbox.Location = new System.Drawing.Point(9, 89);
            this.snmpcomments_textbox.Multiline = true;
            this.snmpcomments_textbox.Name = "snmpcomments_textbox";
            this.snmpcomments_textbox.Size = new System.Drawing.Size(364, 31);
            this.snmpcomments_textbox.TabIndex = 4;
            // 
            // snmp_textBox
            // 
            this.snmp_textBox.Location = new System.Drawing.Point(183, 43);
            this.snmp_textBox.Name = "snmp_textBox";
            this.snmp_textBox.Size = new System.Drawing.Size(190, 20);
            this.snmp_textBox.TabIndex = 3;
            // 
            // snmpCustom_radioButton
            // 
            this.snmpCustom_radioButton.AutoSize = true;
            this.snmpCustom_radioButton.Location = new System.Drawing.Point(163, 45);
            this.snmpCustom_radioButton.Name = "snmpCustom_radioButton";
            this.snmpCustom_radioButton.Size = new System.Drawing.Size(14, 13);
            this.snmpCustom_radioButton.TabIndex = 2;
            this.snmpCustom_radioButton.TabStop = true;
            this.snmpCustom_radioButton.UseVisualStyleBackColor = true;
            // 
            // snmp_comboBox
            // 
            this.snmp_comboBox.FormattingEnabled = true;
            this.snmp_comboBox.Location = new System.Drawing.Point(29, 42);
            this.snmp_comboBox.Name = "snmp_comboBox";
            this.snmp_comboBox.Size = new System.Drawing.Size(121, 21);
            this.snmp_comboBox.TabIndex = 1;
            this.snmp_comboBox.SelectedIndexChanged += new System.EventHandler(this.snmp_comboBox_SelectedIndexChanged_1);
            // 
            // snmpCombo_radioButton
            // 
            this.snmpCombo_radioButton.AutoSize = true;
            this.snmpCombo_radioButton.Location = new System.Drawing.Point(9, 45);
            this.snmpCombo_radioButton.Name = "snmpCombo_radioButton";
            this.snmpCombo_radioButton.Size = new System.Drawing.Size(14, 13);
            this.snmpCombo_radioButton.TabIndex = 0;
            this.snmpCombo_radioButton.TabStop = true;
            this.snmpCombo_radioButton.UseVisualStyleBackColor = true;
            // 
            // SoidBergConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Comments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.soid_assetSelectionControl);
            this.Name = "SoidBergConfigurationControl";
            this.Size = new System.Drawing.Size(614, 466);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.snmpvalues_groupBox.ResumeLayout(false);
            this.snmpvalues_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HP.ScalableTest.Framework.UI.AssetSelectionControl soid_assetSelectionControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Comments;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox snmp_comboBox;
        private System.Windows.Forms.RadioButton snmpCombo_radioButton;
        private System.Windows.Forms.TextBox snmpcomments_textbox;
        private System.Windows.Forms.TextBox snmp_textBox;
        private System.Windows.Forms.RadioButton snmpCustom_radioButton;
        private System.Windows.Forms.Label Comments_label;
        private System.Windows.Forms.Label Snmpoid_label;
        private System.Windows.Forms.Label Custom_oid_label;
        private System.Windows.Forms.Label Setvalue_label;
        private System.Windows.Forms.TextBox oidvalue_textbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox oidtype_comboBox;
        private System.Windows.Forms.GroupBox snmpvalues_groupBox;
        private Framework.UI.FieldValidator soid_fieldValidator;
    }
}
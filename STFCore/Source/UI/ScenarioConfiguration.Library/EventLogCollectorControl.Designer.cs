namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class EventLogCollectorControl
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
            this.hostName_Label = new System.Windows.Forms.Label();
            this.components_Label = new System.Windows.Forms.Label();
            this.interval_Label = new System.Windows.Forms.Label();
            this.platform_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.name_Label = new System.Windows.Forms.Label();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.platform_ComboBox = new System.Windows.Forms.ComboBox();
            this.components_ListBox = new System.Windows.Forms.ListBox();
            this.interval_TextBox = new System.Windows.Forms.TextBox();
            this.hourMin_Label = new System.Windows.Forms.Label();
            this.entryTypes_ListBox = new System.Windows.Forms.ListBox();
            this.level_Label = new System.Windows.Forms.Label();
            this.serverComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.SuspendLayout();
            // 
            // hostName_Label
            // 
            this.hostName_Label.AutoSize = true;
            this.hostName_Label.Location = new System.Drawing.Point(22, 132);
            this.hostName_Label.Name = "hostName_Label";
            this.hostName_Label.Size = new System.Drawing.Size(67, 15);
            this.hostName_Label.TabIndex = 18;
            this.hostName_Label.Text = "Host Name";
            // 
            // components_Label
            // 
            this.components_Label.AutoSize = true;
            this.components_Label.Location = new System.Drawing.Point(15, 181);
            this.components_Label.Name = "components_Label";
            this.components_Label.Size = new System.Drawing.Size(123, 15);
            this.components_Label.TabIndex = 19;
            this.components_Label.Text = "Components/Services";
            // 
            // interval_Label
            // 
            this.interval_Label.AutoSize = true;
            this.interval_Label.Location = new System.Drawing.Point(3, 103);
            this.interval_Label.Name = "interval_Label";
            this.interval_Label.Size = new System.Drawing.Size(86, 15);
            this.interval_Label.TabIndex = 20;
            this.interval_Label.Text = "Polling Interval";
            // 
            // platform_Label
            // 
            this.platform_Label.AutoSize = true;
            this.platform_Label.Location = new System.Drawing.Point(36, 74);
            this.platform_Label.Name = "platform_Label";
            this.platform_Label.Size = new System.Drawing.Size(53, 15);
            this.platform_Label.TabIndex = 24;
            this.platform_Label.Text = "Platform";
            // 
            // description_Label
            // 
            this.description_Label.AutoSize = true;
            this.description_Label.Location = new System.Drawing.Point(22, 45);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(67, 15);
            this.description_Label.TabIndex = 25;
            this.description_Label.Text = "Description";
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Location = new System.Drawing.Point(50, 16);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(39, 15);
            this.name_Label.TabIndex = 26;
            this.name_Label.Text = "Name";
            // 
            // name_TextBox
            // 
            this.name_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name_TextBox.Location = new System.Drawing.Point(95, 13);
            this.name_TextBox.MaxLength = 255;
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(602, 23);
            this.name_TextBox.TabIndex = 27;
            // 
            // description_TextBox
            // 
            this.description_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description_TextBox.Location = new System.Drawing.Point(95, 42);
            this.description_TextBox.MaxLength = 500;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(602, 23);
            this.description_TextBox.TabIndex = 28;
            // 
            // platform_ComboBox
            // 
            this.platform_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platform_ComboBox.FormattingEnabled = true;
            this.platform_ComboBox.Location = new System.Drawing.Point(95, 71);
            this.platform_ComboBox.Name = "platform_ComboBox";
            this.platform_ComboBox.Size = new System.Drawing.Size(390, 23);
            this.platform_ComboBox.TabIndex = 29;
            // 
            // components_ListBox
            // 
            this.components_ListBox.FormattingEnabled = true;
            this.components_ListBox.ItemHeight = 15;
            this.components_ListBox.Location = new System.Drawing.Point(18, 199);
            this.components_ListBox.Name = "components_ListBox";
            this.components_ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.components_ListBox.Size = new System.Drawing.Size(333, 94);
            this.components_ListBox.TabIndex = 30;
            this.components_ListBox.Validating += new System.ComponentModel.CancelEventHandler(this.components_ListBox_Validating);
            // 
            // interval_TextBox
            // 
            this.interval_TextBox.Location = new System.Drawing.Point(95, 100);
            this.interval_TextBox.Name = "interval_TextBox";
            this.interval_TextBox.Size = new System.Drawing.Size(77, 23);
            this.interval_TextBox.TabIndex = 35;
            this.interval_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.interval_TextBox_Validating);
            // 
            // hourMin_Label
            // 
            this.hourMin_Label.Location = new System.Drawing.Point(178, 103);
            this.hourMin_Label.Name = "hourMin_Label";
            this.hourMin_Label.Size = new System.Drawing.Size(82, 20);
            this.hourMin_Label.TabIndex = 36;
            this.hourMin_Label.Text = "(hh:mm)";
            // 
            // entryTypes_ListBox
            // 
            this.entryTypes_ListBox.FormattingEnabled = true;
            this.entryTypes_ListBox.ItemHeight = 15;
            this.entryTypes_ListBox.Items.AddRange(new object[] {
            "Error",
            "Warning",
            "Information",
            "SuccessAudit",
            "FailureAudit"});
            this.entryTypes_ListBox.Location = new System.Drawing.Point(365, 199);
            this.entryTypes_ListBox.Name = "entryTypes_ListBox";
            this.entryTypes_ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.entryTypes_ListBox.Size = new System.Drawing.Size(204, 94);
            this.entryTypes_ListBox.TabIndex = 39;
            this.entryTypes_ListBox.Validating += new System.ComponentModel.CancelEventHandler(this.entryTypes_ListBox_Validating);
            // 
            // level_Label
            // 
            this.level_Label.AutoSize = true;
            this.level_Label.Location = new System.Drawing.Point(362, 181);
            this.level_Label.Name = "level_Label";
            this.level_Label.Size = new System.Drawing.Size(123, 15);
            this.level_Label.TabIndex = 40;
            this.level_Label.Text = "Event Log Entry Types";
            // 
            // serverComboBox
            // 
            this.serverComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverComboBox.Location = new System.Drawing.Point(95, 129);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(390, 23);
            this.serverComboBox.TabIndex = 83;
            // 
            // EventLogCollectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.Controls.Add(this.serverComboBox);
            this.Controls.Add(this.level_Label);
            this.Controls.Add(this.entryTypes_ListBox);
            this.Controls.Add(this.hourMin_Label);
            this.Controls.Add(this.interval_TextBox);
            this.Controls.Add(this.components_ListBox);
            this.Controls.Add(this.platform_ComboBox);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.platform_Label);
            this.Controls.Add(this.hostName_Label);
            this.Controls.Add(this.interval_Label);
            this.Controls.Add(this.components_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EventLogCollectorControl";
            this.Size = new System.Drawing.Size(700, 411);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label hostName_Label;
        private System.Windows.Forms.Label components_Label;
        private System.Windows.Forms.Label interval_Label;
        private System.Windows.Forms.Label platform_Label;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.ComboBox platform_ComboBox;
        private System.Windows.Forms.ListBox components_ListBox;
        private System.Windows.Forms.TextBox interval_TextBox;
        private System.Windows.Forms.Label hourMin_Label;
        private System.Windows.Forms.ListBox entryTypes_ListBox;
        private System.Windows.Forms.Label level_Label;
        private ScalableTest.Framework.UI.ServerComboBox serverComboBox;
    }
}

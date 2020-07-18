namespace HP.ScalableTest.Utility.BtfMetadataHelper
{
    partial class MetadataForm
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
            this.selectedScenario_TextBox = new System.Windows.Forms.TextBox();
            this.scenario_Label = new System.Windows.Forms.Label();
            this.scenarioSelection_Button = new System.Windows.Forms.Button();
            this.loggedIn_textBox = new System.Windows.Forms.TextBox();
            this.generate_button = new System.Windows.Forms.Button();
            this.test_groupBox = new System.Windows.Forms.GroupBox();
            this.owner_comboBox = new System.Windows.Forms.ComboBox();
            this.pillar_label = new System.Windows.Forms.Label();
            this.classification_comboBox = new System.Windows.Forms.ComboBox();
            this.classification_label = new System.Windows.Forms.Label();
            this.status_comboBox = new System.Windows.Forms.ComboBox();
            this.status_label = new System.Windows.Forms.Label();
            this.purpose_textBox = new System.Windows.Forms.TextBox();
            this.purpose_label = new System.Windows.Forms.Label();
            this.test_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectedScenario_TextBox
            // 
            this.selectedScenario_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedScenario_TextBox.Location = new System.Drawing.Point(158, 44);
            this.selectedScenario_TextBox.Name = "selectedScenario_TextBox";
            this.selectedScenario_TextBox.ReadOnly = true;
            this.selectedScenario_TextBox.Size = new System.Drawing.Size(424, 20);
            this.selectedScenario_TextBox.TabIndex = 4;
            // 
            // scenario_Label
            // 
            this.scenario_Label.Location = new System.Drawing.Point(86, 44);
            this.scenario_Label.Name = "scenario_Label";
            this.scenario_Label.Size = new System.Drawing.Size(66, 20);
            this.scenario_Label.TabIndex = 3;
            this.scenario_Label.Text = "Scenario";
            this.scenario_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // scenarioSelection_Button
            // 
            this.scenarioSelection_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioSelection_Button.Location = new System.Drawing.Point(588, 44);
            this.scenarioSelection_Button.Name = "scenarioSelection_Button";
            this.scenarioSelection_Button.Size = new System.Drawing.Size(27, 20);
            this.scenarioSelection_Button.TabIndex = 5;
            this.scenarioSelection_Button.Text = "...";
            this.scenarioSelection_Button.UseVisualStyleBackColor = true;
            this.scenarioSelection_Button.Click += new System.EventHandler(this.scenarioSelection_Button_Click);
            // 
            // loggedIn_textBox
            // 
            this.loggedIn_textBox.Location = new System.Drawing.Point(-1, 542);
            this.loggedIn_textBox.Name = "loggedIn_textBox";
            this.loggedIn_textBox.ReadOnly = true;
            this.loggedIn_textBox.Size = new System.Drawing.Size(284, 20);
            this.loggedIn_textBox.TabIndex = 6;
            // 
            // generate_button
            // 
            this.generate_button.Location = new System.Drawing.Point(327, 229);
            this.generate_button.Name = "generate_button";
            this.generate_button.Size = new System.Drawing.Size(127, 23);
            this.generate_button.TabIndex = 7;
            this.generate_button.Text = "Generate Metadata";
            this.generate_button.UseVisualStyleBackColor = true;
            this.generate_button.Click += new System.EventHandler(this.generate_button_Click);
            // 
            // test_groupBox
            // 
            this.test_groupBox.Controls.Add(this.owner_comboBox);
            this.test_groupBox.Controls.Add(this.pillar_label);
            this.test_groupBox.Controls.Add(this.classification_comboBox);
            this.test_groupBox.Controls.Add(this.classification_label);
            this.test_groupBox.Controls.Add(this.status_comboBox);
            this.test_groupBox.Controls.Add(this.status_label);
            this.test_groupBox.Controls.Add(this.purpose_textBox);
            this.test_groupBox.Controls.Add(this.purpose_label);
            this.test_groupBox.Location = new System.Drawing.Point(104, 85);
            this.test_groupBox.Name = "test_groupBox";
            this.test_groupBox.Size = new System.Drawing.Size(545, 122);
            this.test_groupBox.TabIndex = 8;
            this.test_groupBox.TabStop = false;
            this.test_groupBox.Text = "Test Options";
            // 
            // owner_comboBox
            // 
            this.owner_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.owner_comboBox.FormattingEnabled = true;
            this.owner_comboBox.Items.AddRange(new object[] {
            "CTC",
            "UPD",
            "ETL"});
            this.owner_comboBox.Location = new System.Drawing.Point(350, 24);
            this.owner_comboBox.Name = "owner_comboBox";
            this.owner_comboBox.Size = new System.Drawing.Size(161, 21);
            this.owner_comboBox.TabIndex = 9;
            // 
            // pillar_label
            // 
            this.pillar_label.AutoSize = true;
            this.pillar_label.Location = new System.Drawing.Point(278, 28);
            this.pillar_label.Name = "pillar_label";
            this.pillar_label.Size = new System.Drawing.Size(62, 13);
            this.pillar_label.TabIndex = 8;
            this.pillar_label.Text = "Test Owner";
            // 
            // classification_comboBox
            // 
            this.classification_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classification_comboBox.FormattingEnabled = true;
            this.classification_comboBox.Items.AddRange(new object[] {
            "Unit",
            "Package",
            "System",
            "Duration",
            "Memory"});
            this.classification_comboBox.Location = new System.Drawing.Point(106, 83);
            this.classification_comboBox.Name = "classification_comboBox";
            this.classification_comboBox.Size = new System.Drawing.Size(161, 21);
            this.classification_comboBox.TabIndex = 5;
            // 
            // classification_label
            // 
            this.classification_label.AutoSize = true;
            this.classification_label.Location = new System.Drawing.Point(12, 86);
            this.classification_label.Name = "classification_label";
            this.classification_label.Size = new System.Drawing.Size(92, 13);
            this.classification_label.TabIndex = 4;
            this.classification_label.Text = "Test Classification";
            // 
            // status_comboBox
            // 
            this.status_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.status_comboBox.FormattingEnabled = true;
            this.status_comboBox.Items.AddRange(new object[] {
            "In Development",
            "Stable",
            "Repair"});
            this.status_comboBox.Location = new System.Drawing.Point(106, 56);
            this.status_comboBox.Name = "status_comboBox";
            this.status_comboBox.Size = new System.Drawing.Size(161, 21);
            this.status_comboBox.TabIndex = 3;
            // 
            // status_label
            // 
            this.status_label.AutoSize = true;
            this.status_label.Location = new System.Drawing.Point(12, 59);
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(61, 13);
            this.status_label.TabIndex = 2;
            this.status_label.Text = "Test Status";
            // 
            // purpose_textBox
            // 
            this.purpose_textBox.Location = new System.Drawing.Point(106, 25);
            this.purpose_textBox.Name = "purpose_textBox";
            this.purpose_textBox.Size = new System.Drawing.Size(161, 20);
            this.purpose_textBox.TabIndex = 1;
            // 
            // purpose_label
            // 
            this.purpose_label.AutoSize = true;
            this.purpose_label.Location = new System.Drawing.Point(12, 28);
            this.purpose_label.Name = "purpose_label";
            this.purpose_label.Size = new System.Drawing.Size(46, 13);
            this.purpose_label.TabIndex = 0;
            this.purpose_label.Text = "Purpose";
            // 
            // MetadataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.test_groupBox);
            this.Controls.Add(this.generate_button);
            this.Controls.Add(this.loggedIn_textBox);
            this.Controls.Add(this.selectedScenario_TextBox);
            this.Controls.Add(this.scenario_Label);
            this.Controls.Add(this.scenarioSelection_Button);
            this.Name = "MetadataForm";
            this.Text = "BTF Metadata Helper";
            this.test_groupBox.ResumeLayout(false);
            this.test_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        protected System.Windows.Forms.TextBox selectedScenario_TextBox;
        protected System.Windows.Forms.Label scenario_Label;
        protected System.Windows.Forms.Button scenarioSelection_Button;
        private System.Windows.Forms.TextBox loggedIn_textBox;
        private System.Windows.Forms.Button generate_button;
        private System.Windows.Forms.GroupBox test_groupBox;
        private System.Windows.Forms.ComboBox status_comboBox;
        private System.Windows.Forms.Label status_label;
        private System.Windows.Forms.TextBox purpose_textBox;
        private System.Windows.Forms.Label purpose_label;
        private System.Windows.Forms.ComboBox classification_comboBox;
        private System.Windows.Forms.Label classification_label;
        private System.Windows.Forms.ComboBox owner_comboBox;
        private System.Windows.Forms.Label pillar_label;
    }
}


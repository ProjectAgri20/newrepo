namespace HP.ScalableTest.Plugin.HpcrSimulation
{
    partial class HpcrSimulationConfigControl
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
            this.server_Label = new System.Windows.Forms.Label();
            this.groupBoxDocument = new System.Windows.Forms.GroupBox();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.serverComboBoxHpcr = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.test_GroupBox = new HP.ScalableTest.Plugin.HpcrSimulation.RadioGroupBox();
            this.numericUpDownToCount = new System.Windows.Forms.NumericUpDown();
            this.comboBox_EmailOriginator = new System.Windows.Forms.ComboBox();
            this.comboBox_DistributionOriginator = new System.Windows.Forms.ComboBox();
            this.comboBox_Distributions = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.user_Label = new System.Windows.Forms.Label();
            this.distribute_RadioButton = new System.Windows.Forms.RadioButton();
            this.email_RadioButton = new System.Windows.Forms.RadioButton();
            this.groupBoxDocument.SuspendLayout();
            this.test_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToCount)).BeginInit();
            this.SuspendLayout();
            // 
            // server_Label
            // 
            this.server_Label.AutoSize = true;
            this.server_Label.Location = new System.Drawing.Point(15, 11);
            this.server_Label.Name = "server_Label";
            this.server_Label.Size = new System.Drawing.Size(74, 13);
            this.server_Label.TabIndex = 19;
            this.server_Label.Text = "HPCR Server:";
            // 
            // groupBoxDocument
            // 
            this.groupBoxDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDocument.Controls.Add(this.documentSelectionControl);
            this.groupBoxDocument.Location = new System.Drawing.Point(15, 226);
            this.groupBoxDocument.Name = "groupBoxDocument";
            this.groupBoxDocument.Size = new System.Drawing.Size(397, 471);
            this.groupBoxDocument.TabIndex = 25;
            this.groupBoxDocument.TabStop = false;
            this.groupBoxDocument.Text = "Document";
            this.toolTip1.SetToolTip(this.groupBoxDocument, "Note: If more than 2 documents are chosen then only 2 from the resulting selectio" +
        "n will be sent (picked at random).");
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(15, 20);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(376, 438);
            this.documentSelectionControl.TabIndex = 0;
            // 
            // serverComboBoxHpcr
            // 
            this.serverComboBoxHpcr.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverComboBoxHpcr.Location = new System.Drawing.Point(96, 11);
            this.serverComboBoxHpcr.Name = "serverComboBoxHpcr";
            this.serverComboBoxHpcr.Size = new System.Drawing.Size(210, 23);
            this.serverComboBoxHpcr.TabIndex = 26;
            this.serverComboBoxHpcr.SelectionChanged += new System.EventHandler(this.serverComboBoxHpcr_SelectionChanged);
            // 
            // test_GroupBox
            // 
            this.test_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.test_GroupBox.Controls.Add(this.numericUpDownToCount);
            this.test_GroupBox.Controls.Add(this.comboBox_EmailOriginator);
            this.test_GroupBox.Controls.Add(this.comboBox_DistributionOriginator);
            this.test_GroupBox.Controls.Add(this.comboBox_Distributions);
            this.test_GroupBox.Controls.Add(this.label3);
            this.test_GroupBox.Controls.Add(this.label1);
            this.test_GroupBox.Controls.Add(this.label2);
            this.test_GroupBox.Controls.Add(this.user_Label);
            this.test_GroupBox.Controls.Add(this.distribute_RadioButton);
            this.test_GroupBox.Controls.Add(this.email_RadioButton);
            this.test_GroupBox.Location = new System.Drawing.Point(15, 35);
            this.test_GroupBox.Name = "test_GroupBox";
            this.test_GroupBox.Selected = 1;
            this.test_GroupBox.Size = new System.Drawing.Size(397, 185);
            this.test_GroupBox.TabIndex = 16;
            this.test_GroupBox.TabStop = false;
            this.test_GroupBox.Text = "Test Type";
            // 
            // numericUpDownToCount
            // 
            this.numericUpDownToCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownToCount.Location = new System.Drawing.Point(214, 62);
            this.numericUpDownToCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownToCount.Name = "numericUpDownToCount";
            this.numericUpDownToCount.Size = new System.Drawing.Size(177, 20);
            this.numericUpDownToCount.TabIndex = 27;
            this.numericUpDownToCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBox_EmailOriginator
            // 
            this.comboBox_EmailOriginator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_EmailOriginator.FormattingEnabled = true;
            this.comboBox_EmailOriginator.Items.AddRange(new object[] {
            "",
            "[CurrentUser]"});
            this.comboBox_EmailOriginator.Location = new System.Drawing.Point(115, 36);
            this.comboBox_EmailOriginator.Name = "comboBox_EmailOriginator";
            this.comboBox_EmailOriginator.Size = new System.Drawing.Size(276, 21);
            this.comboBox_EmailOriginator.TabIndex = 26;
            // 
            // comboBox_DistributionOriginator
            // 
            this.comboBox_DistributionOriginator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_DistributionOriginator.Enabled = false;
            this.comboBox_DistributionOriginator.FormattingEnabled = true;
            this.comboBox_DistributionOriginator.Location = new System.Drawing.Point(115, 122);
            this.comboBox_DistributionOriginator.Name = "comboBox_DistributionOriginator";
            this.comboBox_DistributionOriginator.Size = new System.Drawing.Size(276, 21);
            this.comboBox_DistributionOriginator.TabIndex = 26;
            this.comboBox_DistributionOriginator.SelectedValueChanged += new System.EventHandler(this.comboBox_DistributionOriginator_SelectedValueChanged);
            // 
            // comboBox_Distributions
            // 
            this.comboBox_Distributions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Distributions.Enabled = false;
            this.comboBox_Distributions.FormattingEnabled = true;
            this.comboBox_Distributions.Location = new System.Drawing.Point(115, 149);
            this.comboBox_Distributions.Name = "comboBox_Distributions";
            this.comboBox_Distributions.Size = new System.Drawing.Size(276, 21);
            this.comboBox_Distributions.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Number of random recipients:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Distribution:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Originator:";
            // 
            // user_Label
            // 
            this.user_Label.AutoSize = true;
            this.user_Label.Location = new System.Drawing.Point(45, 125);
            this.user_Label.Name = "user_Label";
            this.user_Label.Size = new System.Drawing.Size(55, 13);
            this.user_Label.TabIndex = 21;
            this.user_Label.Text = "Originator:";
            // 
            // distribute_RadioButton
            // 
            this.distribute_RadioButton.AutoSize = true;
            this.distribute_RadioButton.Location = new System.Drawing.Point(15, 99);
            this.distribute_RadioButton.Name = "distribute_RadioButton";
            this.distribute_RadioButton.Size = new System.Drawing.Size(201, 17);
            this.distribute_RadioButton.TabIndex = 0;
            this.distribute_RadioButton.Tag = "0";
            this.distribute_RadioButton.Text = "Deliver document to users distribution";
            this.distribute_RadioButton.UseVisualStyleBackColor = true;
            this.distribute_RadioButton.CheckedChanged += new System.EventHandler(this.distribute_RadioButton_CheckedChanged);
            // 
            // email_RadioButton
            // 
            this.email_RadioButton.AutoSize = true;
            this.email_RadioButton.Checked = true;
            this.email_RadioButton.Location = new System.Drawing.Point(15, 19);
            this.email_RadioButton.Name = "email_RadioButton";
            this.email_RadioButton.Size = new System.Drawing.Size(147, 17);
            this.email_RadioButton.TabIndex = 2;
            this.email_RadioButton.TabStop = true;
            this.email_RadioButton.Tag = "1";
            this.email_RadioButton.Text = "Deliver document to email";
            this.toolTip1.SetToolTip(this.email_RadioButton, "Need one more office worker then recipients");
            this.email_RadioButton.UseVisualStyleBackColor = true;
            this.email_RadioButton.CheckedChanged += new System.EventHandler(this.email_RadioButton_CheckedChanged);
            // 
            // HpcrSimulationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.serverComboBoxHpcr);
            this.Controls.Add(this.groupBoxDocument);
            this.Controls.Add(this.server_Label);
            this.Controls.Add(this.test_GroupBox);
            this.Name = "HpcrSimulationConfigControl";
            this.Size = new System.Drawing.Size(427, 711);
            this.groupBoxDocument.ResumeLayout(false);
            this.test_GroupBox.ResumeLayout(false);
            this.test_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label user_Label;
        private System.Windows.Forms.Label server_Label;
        private RadioGroupBox test_GroupBox;
        private System.Windows.Forms.RadioButton distribute_RadioButton;
        private System.Windows.Forms.RadioButton email_RadioButton;
        private System.Windows.Forms.GroupBox groupBoxDocument;
        private System.Windows.Forms.ComboBox comboBox_DistributionOriginator;
        private System.Windows.Forms.ComboBox comboBox_Distributions;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownToCount;
        private System.Windows.Forms.ComboBox comboBox_EmailOriginator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private Framework.UI.DocumentSelectionControl documentSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.ServerComboBox serverComboBoxHpcr;
    }
}

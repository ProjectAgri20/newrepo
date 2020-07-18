namespace HP.ScalableTest.Framework.UI
{
    partial class SimulatorConfigurationForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pause_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.documentSelection_GroupBox = new System.Windows.Forms.GroupBox();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.label1 = new System.Windows.Forms.Label();
            this.scanFlatbed_RadioButton = new System.Windows.Forms.RadioButton();
            this.scanAdf_RadioButton = new System.Windows.Forms.RadioButton();
            this.ok_Button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pause_NumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.documentSelection_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.pause_NumericUpDown);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Automation Options";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(154, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(384, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "milliseconds after each automation step (to simulate real user behavior)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Pause for";
            // 
            // pause_NumericUpDown
            // 
            this.pause_NumericUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.pause_NumericUpDown.Location = new System.Drawing.Point(84, 26);
            this.pause_NumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.pause_NumericUpDown.Name = "pause_NumericUpDown";
            this.pause_NumericUpDown.Size = new System.Drawing.Size(64, 23);
            this.pause_NumericUpDown.TabIndex = 1;
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(565, 536);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.documentSelection_GroupBox);
            this.groupBox2.Controls.Add(this.scanFlatbed_RadioButton);
            this.groupBox2.Controls.Add(this.scanAdf_RadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(628, 440);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Page Selection";
            // 
            // documentSelection_GroupBox
            // 
            this.documentSelection_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentSelection_GroupBox.Controls.Add(this.documentSelectionControl);
            this.documentSelection_GroupBox.Controls.Add(this.label1);
            this.documentSelection_GroupBox.Enabled = false;
            this.documentSelection_GroupBox.Location = new System.Drawing.Point(25, 56);
            this.documentSelection_GroupBox.Name = "documentSelection_GroupBox";
            this.documentSelection_GroupBox.Size = new System.Drawing.Size(578, 366);
            this.documentSelection_GroupBox.TabIndex = 2;
            this.documentSelection_GroupBox.TabStop = false;
            this.documentSelection_GroupBox.Text = "ADF Document Selection";
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(3, 19);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(572, 304);
            this.documentSelectionControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(572, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Note: The total number of pages scanned is determined by the Page Count setting f" +
    "or this activity.\r\nPages will be selected from the above documents until the tot" +
    "al page count is met.";
            // 
            // scanFlatbed_RadioButton
            // 
            this.scanFlatbed_RadioButton.AutoSize = true;
            this.scanFlatbed_RadioButton.Checked = true;
            this.scanFlatbed_RadioButton.Location = new System.Drawing.Point(25, 31);
            this.scanFlatbed_RadioButton.Name = "scanFlatbed_RadioButton";
            this.scanFlatbed_RadioButton.Size = new System.Drawing.Size(188, 19);
            this.scanFlatbed_RadioButton.TabIndex = 0;
            this.scanFlatbed_RadioButton.TabStop = true;
            this.scanFlatbed_RadioButton.Text = "Scan default page from flatbed";
            this.scanFlatbed_RadioButton.UseVisualStyleBackColor = true;
            // 
            // scanAdf_RadioButton
            // 
            this.scanAdf_RadioButton.AutoSize = true;
            this.scanAdf_RadioButton.Location = new System.Drawing.Point(248, 31);
            this.scanAdf_RadioButton.Name = "scanAdf_RadioButton";
            this.scanAdf_RadioButton.Size = new System.Drawing.Size(182, 19);
            this.scanAdf_RadioButton.TabIndex = 1;
            this.scanAdf_RadioButton.Text = "Scan using the simulated ADF";
            this.scanAdf_RadioButton.UseVisualStyleBackColor = true;
            this.scanAdf_RadioButton.CheckedChanged += new System.EventHandler(this.scanAdf_RadioButton_CheckedChanged);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(480, 536);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // SimulatorConfigurationForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(652, 571);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SimulatorConfigurationForm";
            this.Text = "Simulator Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pause_NumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.documentSelection_GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown pause_NumericUpDown;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton scanAdf_RadioButton;
        private System.Windows.Forms.RadioButton scanFlatbed_RadioButton;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.GroupBox documentSelection_GroupBox;
        private System.Windows.Forms.Label label1;
        private DocumentSelectionControl documentSelectionControl;
    }
}
namespace HP.ScalableTest.Plugin.DriverlessPrinting
{
    partial class DriverlessPrintingConfigurationControl
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.print_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.print_documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.printoptions_groupbox = new System.Windows.Forms.GroupBox();
            this.printmethod_label = new System.Windows.Forms.Label();
            this.printProtocol_comboBox = new System.Windows.Forms.ComboBox();
            this.jobseparator_checkBox = new System.Windows.Forms.CheckBox();
            this.shuffle_CheckBox = new System.Windows.Forms.CheckBox();
            this.jobStorage_checkBox = new System.Windows.Forms.CheckBox();
            this.pin_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.printoptions_groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pin_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // print_assetSelectionControl
            // 
            this.print_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.print_assetSelectionControl.Location = new System.Drawing.Point(16, 16);
            this.print_assetSelectionControl.Name = "print_assetSelectionControl";
            this.print_assetSelectionControl.Size = new System.Drawing.Size(692, 174);
            this.print_assetSelectionControl.TabIndex = 0;
            // 
            // print_documentSelectionControl
            // 
            this.print_documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.print_documentSelectionControl.Location = new System.Drawing.Point(16, 282);
            this.print_documentSelectionControl.Name = "print_documentSelectionControl";
            this.print_documentSelectionControl.ShowDocumentBrowseControl = true;
            this.print_documentSelectionControl.ShowDocumentQueryControl = true;
            this.print_documentSelectionControl.ShowDocumentSetControl = true;
            this.print_documentSelectionControl.Size = new System.Drawing.Size(692, 336);
            this.print_documentSelectionControl.TabIndex = 1;
            // 
            // printoptions_groupbox
            // 
            this.printoptions_groupbox.Controls.Add(this.pin_numericUpDown);
            this.printoptions_groupbox.Controls.Add(this.jobStorage_checkBox);
            this.printoptions_groupbox.Controls.Add(this.printmethod_label);
            this.printoptions_groupbox.Controls.Add(this.printProtocol_comboBox);
            this.printoptions_groupbox.Controls.Add(this.jobseparator_checkBox);
            this.printoptions_groupbox.Controls.Add(this.shuffle_CheckBox);
            this.printoptions_groupbox.Location = new System.Drawing.Point(16, 196);
            this.printoptions_groupbox.Name = "printoptions_groupbox";
            this.printoptions_groupbox.Size = new System.Drawing.Size(692, 80);
            this.printoptions_groupbox.TabIndex = 2;
            this.printoptions_groupbox.TabStop = false;
            this.printoptions_groupbox.Text = "Print Options";
            // 
            // printmethod_label
            // 
            this.printmethod_label.AutoSize = true;
            this.printmethod_label.Location = new System.Drawing.Point(17, 29);
            this.printmethod_label.Name = "printmethod_label";
            this.printmethod_label.Size = new System.Drawing.Size(80, 15);
            this.printmethod_label.TabIndex = 58;
            this.printmethod_label.Text = "Print Protocol";
            // 
            // printProtocol_comboBox
            // 
            this.printProtocol_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.printProtocol_comboBox.FormattingEnabled = true;
            this.printProtocol_comboBox.Location = new System.Drawing.Point(20, 47);
            this.printProtocol_comboBox.Name = "printProtocol_comboBox";
            this.printProtocol_comboBox.Size = new System.Drawing.Size(157, 23);
            this.printProtocol_comboBox.TabIndex = 57;
            // 
            // jobseparator_checkBox
            // 
            this.jobseparator_checkBox.AutoSize = true;
            this.jobseparator_checkBox.Location = new System.Drawing.Point(260, 51);
            this.jobseparator_checkBox.Name = "jobseparator_checkBox";
            this.jobseparator_checkBox.Size = new System.Drawing.Size(123, 19);
            this.jobseparator_checkBox.TabIndex = 56;
            this.jobseparator_checkBox.Text = "Print job separator";
            this.jobseparator_checkBox.UseVisualStyleBackColor = true;
            // 
            // shuffle_CheckBox
            // 
            this.shuffle_CheckBox.Location = new System.Drawing.Point(260, 22);
            this.shuffle_CheckBox.Name = "shuffle_CheckBox";
            this.shuffle_CheckBox.Size = new System.Drawing.Size(200, 19);
            this.shuffle_CheckBox.TabIndex = 52;
            this.shuffle_CheckBox.Text = "Shuffle document printing order";
            // 
            // jobStorage_checkBox
            // 
            this.jobStorage_checkBox.AutoSize = true;
            this.jobStorage_checkBox.Location = new System.Drawing.Point(466, 22);
            this.jobStorage_checkBox.Name = "jobStorage_checkBox";
            this.jobStorage_checkBox.Size = new System.Drawing.Size(130, 19);
            this.jobStorage_checkBox.TabIndex = 59;
            this.jobStorage_checkBox.Text = "Send to Job Storage";
            this.jobStorage_checkBox.UseVisualStyleBackColor = true;
            // 
            // pin_numericUpDown
            // 
            this.pin_numericUpDown.Location = new System.Drawing.Point(466, 47);
            this.pin_numericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.pin_numericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.pin_numericUpDown.Name = "pin_numericUpDown";
            this.pin_numericUpDown.Size = new System.Drawing.Size(120, 23);
            this.pin_numericUpDown.TabIndex = 60;
            this.pin_numericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // DriverlessPrintingConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.printoptions_groupbox);
            this.Controls.Add(this.print_documentSelectionControl);
            this.Controls.Add(this.print_assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DriverlessPrintingConfigurationControl";
            this.Size = new System.Drawing.Size(717, 630);
            this.printoptions_groupbox.ResumeLayout(false);
            this.printoptions_groupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pin_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl print_assetSelectionControl;
        private Framework.UI.DocumentSelectionControl print_documentSelectionControl;
        private System.Windows.Forms.GroupBox printoptions_groupbox;
        private System.Windows.Forms.CheckBox shuffle_CheckBox;
        private System.Windows.Forms.CheckBox jobseparator_checkBox;
        private System.Windows.Forms.Label printmethod_label;
        private System.Windows.Forms.ComboBox printProtocol_comboBox;
        private System.Windows.Forms.NumericUpDown pin_numericUpDown;
        private System.Windows.Forms.CheckBox jobStorage_checkBox;
    }
}

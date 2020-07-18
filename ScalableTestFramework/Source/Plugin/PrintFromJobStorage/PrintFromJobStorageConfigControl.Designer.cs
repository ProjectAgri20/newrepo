namespace HP.ScalableTest.Plugin.PrintFromJobStorage
{
    partial class PrintFromJobStorageConfigControl
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
            this.pin_Label = new System.Windows.Forms.Label();
            this.pin_TextBox = new System.Windows.Forms.TextBox();
            this.printAll_CheckBox = new System.Windows.Forms.CheckBox();
            this.pinRequired_CheckBox = new System.Windows.Forms.CheckBox();
            this.pinDescription_label = new System.Windows.Forms.Label();
            this.jobStorage_AssetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.Copies_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.copies_Label = new System.Windows.Forms.Label();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_PrintFromJobStprage = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.folderName_Label = new System.Windows.Forms.Label();
            this.folderName_TextBox = new System.Windows.Forms.TextBox();
            this.deleteJobAfterPrint_Checkbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Copies_NumericUpDown)).BeginInit();
            this.groupBox_Authentication.SuspendLayout();
            this.SuspendLayout();
            // 
            // pin_Label
            // 
            this.pin_Label.AutoSize = true;
            this.pin_Label.Location = new System.Drawing.Point(3, 75);
            this.pin_Label.Name = "pin_Label";
            this.pin_Label.Size = new System.Drawing.Size(25, 13);
            this.pin_Label.TabIndex = 4;
            this.pin_Label.Text = "PIN";
            // 
            // pin_TextBox
            // 
            this.pin_TextBox.Enabled = false;
            this.pin_TextBox.Location = new System.Drawing.Point(34, 72);
            this.pin_TextBox.MaxLength = 4;
            this.pin_TextBox.Name = "pin_TextBox";
            this.pin_TextBox.Size = new System.Drawing.Size(129, 20);
            this.pin_TextBox.TabIndex = 5;
            this.pin_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pin_TextBox_KeyPress);
            // 
            // printAll_CheckBox
            // 
            this.printAll_CheckBox.AutoSize = true;
            this.printAll_CheckBox.Location = new System.Drawing.Point(6, 15);
            this.printAll_CheckBox.Name = "printAll_CheckBox";
            this.printAll_CheckBox.Size = new System.Drawing.Size(61, 17);
            this.printAll_CheckBox.TabIndex = 7;
            this.printAll_CheckBox.Text = "Print All";
            this.printAll_CheckBox.UseVisualStyleBackColor = true;
            this.printAll_CheckBox.CheckedChanged += new System.EventHandler(this.EnableDisableNumberOfCopies);
            // 
            // pinRequired_CheckBox
            // 
            this.pinRequired_CheckBox.AutoSize = true;
            this.pinRequired_CheckBox.Location = new System.Drawing.Point(6, 49);
            this.pinRequired_CheckBox.Name = "pinRequired_CheckBox";
            this.pinRequired_CheckBox.Size = new System.Drawing.Size(87, 17);
            this.pinRequired_CheckBox.TabIndex = 8;
            this.pinRequired_CheckBox.Text = "Pin Required";
            this.pinRequired_CheckBox.UseVisualStyleBackColor = true;
            this.pinRequired_CheckBox.CheckedChanged += new System.EventHandler(this.pinRequired_CheckBox_CheckedChanged);
            // 
            // pinDescription_label
            // 
            this.pinDescription_label.AutoSize = true;
            this.pinDescription_label.Location = new System.Drawing.Point(31, 95);
            this.pinDescription_label.Name = "pinDescription_label";
            this.pinDescription_label.Size = new System.Drawing.Size(132, 13);
            this.pinDescription_label.TabIndex = 23;
            this.pinDescription_label.Text = "numeric(0-9), 4 chars Max.";
            // 
            // jobStorage_AssetSelectionControl
            // 
            this.jobStorage_AssetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobStorage_AssetSelectionControl.Location = new System.Drawing.Point(0, 180);
            this.jobStorage_AssetSelectionControl.Name = "jobStorage_AssetSelectionControl";
            this.jobStorage_AssetSelectionControl.Size = new System.Drawing.Size(661, 265);
            this.jobStorage_AssetSelectionControl.TabIndex = 2;
            // 
            // Copies_NumericUpDown
            // 
            this.Copies_NumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Copies_NumericUpDown.Location = new System.Drawing.Point(306, 73);
            this.Copies_NumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.Copies_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Copies_NumericUpDown.Name = "Copies_NumericUpDown";
            this.Copies_NumericUpDown.Size = new System.Drawing.Size(107, 20);
            this.Copies_NumericUpDown.TabIndex = 42;
            this.Copies_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // copies_Label
            // 
            this.copies_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.copies_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copies_Label.Location = new System.Drawing.Point(201, 75);
            this.copies_Label.Name = "copies_Label";
            this.copies_Label.Size = new System.Drawing.Size(100, 24);
            this.copies_Label.TabIndex = 41;
            this.copies_Label.Text = " Number of copies";
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_PrintFromJobStprage);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(3, 125);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(658, 49);
            this.groupBox_Authentication.TabIndex = 97;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_PrintFromJobStprage
            // 
            this.radioButton_PrintFromJobStprage.AutoSize = true;
            this.radioButton_PrintFromJobStprage.Checked = true;
            this.radioButton_PrintFromJobStprage.Location = new System.Drawing.Point(116, 20);
            this.radioButton_PrintFromJobStprage.Name = "radioButton_PrintFromJobStprage";
            this.radioButton_PrintFromJobStprage.Size = new System.Drawing.Size(130, 17);
            this.radioButton_PrintFromJobStprage.TabIndex = 93;
            this.radioButton_PrintFromJobStprage.TabStop = true;
            this.radioButton_PrintFromJobStprage.Text = "Print From Job storage";
            this.radioButton_PrintFromJobStprage.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 20);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(58, 17);
            this.radioButton_SignInButton.TabIndex = 92;
            this.radioButton_SignInButton.Text = "Sign In";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(436, 17);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 21);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(296, 21);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(114, 13);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // folderName_Label
            // 
            this.folderName_Label.AutoSize = true;
            this.folderName_Label.Location = new System.Drawing.Point(436, 75);
            this.folderName_Label.Name = "folderName_Label";
            this.folderName_Label.Size = new System.Drawing.Size(65, 13);
            this.folderName_Label.TabIndex = 99;
            this.folderName_Label.Text = "Folder name";
            // 
            // folderName_TextBox
            // 
            this.folderName_TextBox.Location = new System.Drawing.Point(507, 73);
            this.folderName_TextBox.MaxLength = 16;
            this.folderName_TextBox.Name = "folderName_TextBox";
            this.folderName_TextBox.Size = new System.Drawing.Size(129, 20);
            this.folderName_TextBox.TabIndex = 100;
            // 
            // deleteJobAfterPrint_Checkbox
            // 
            this.deleteJobAfterPrint_Checkbox.AutoSize = true;
            this.deleteJobAfterPrint_Checkbox.Location = new System.Drawing.Point(73, 15);
            this.deleteJobAfterPrint_Checkbox.Name = "deleteJobAfterPrint_Checkbox";
            this.deleteJobAfterPrint_Checkbox.Size = new System.Drawing.Size(126, 17);
            this.deleteJobAfterPrint_Checkbox.TabIndex = 101;
            this.deleteJobAfterPrint_Checkbox.Text = "Delete Job After Print";
            this.deleteJobAfterPrint_Checkbox.UseVisualStyleBackColor = true;
            // 
            // PrintFromJobStorageConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteJobAfterPrint_Checkbox);
            this.Controls.Add(this.folderName_TextBox);
            this.Controls.Add(this.folderName_Label);
            this.Controls.Add(this.groupBox_Authentication);
            this.Controls.Add(this.Copies_NumericUpDown);
            this.Controls.Add(this.copies_Label);
            this.Controls.Add(this.pinDescription_label);
            this.Controls.Add(this.pinRequired_CheckBox);
            this.Controls.Add(this.printAll_CheckBox);
            this.Controls.Add(this.pin_TextBox);
            this.Controls.Add(this.pin_Label);
            this.Controls.Add(this.jobStorage_AssetSelectionControl);
            this.Name = "PrintFromJobStorageConfigControl";
            this.Size = new System.Drawing.Size(667, 448);
            ((System.ComponentModel.ISupportInitialize)(this.Copies_NumericUpDown)).EndInit();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.AssetSelectionControl jobStorage_AssetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label pin_Label;
        private System.Windows.Forms.TextBox pin_TextBox;
        private System.Windows.Forms.CheckBox printAll_CheckBox;
        private System.Windows.Forms.CheckBox pinRequired_CheckBox;
        private System.Windows.Forms.Label pinDescription_label;
        private System.Windows.Forms.NumericUpDown Copies_NumericUpDown;
        private System.Windows.Forms.Label copies_Label;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_PrintFromJobStprage;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.Label folderName_Label;
        private System.Windows.Forms.TextBox folderName_TextBox;
        private System.Windows.Forms.CheckBox deleteJobAfterPrint_Checkbox;
    }
}

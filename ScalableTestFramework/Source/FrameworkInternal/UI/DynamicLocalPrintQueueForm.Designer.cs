namespace HP.ScalableTest.Framework.UI
{
    partial class DynamicLocalPrintQueueForm
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
            this.printDevices_TextBox = new System.Windows.Forms.TextBox();
            this.selectPrinters_Button = new System.Windows.Forms.Button();
            this.devices_Label = new System.Windows.Forms.Label();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.portType_ComboBox = new System.Windows.Forms.ComboBox();
            this.portType_Label = new System.Windows.Forms.Label();
            this.queueName_Label = new System.Windows.Forms.Label();
            this.queueName_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.cfmFile_GroupBox = new System.Windows.Forms.GroupBox();
            this.filterCfmList_CheckBox = new System.Windows.Forms.CheckBox();
            this.shortcut_Label = new System.Windows.Forms.Label();
            this.shortcut_ComboBox = new System.Windows.Forms.ComboBox();
            this.cfmFile_Label = new System.Windows.Forms.Label();
            this.cfmFile_ComboBox = new System.Windows.Forms.ComboBox();
            this.printDriverSelectionControl = new HP.ScalableTest.Framework.UI.PrintDriverSelectionControl();
            this.cfmFile_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // printDevices_TextBox
            // 
            this.printDevices_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printDevices_TextBox.Location = new System.Drawing.Point(100, 12);
            this.printDevices_TextBox.Name = "printDevices_TextBox";
            this.printDevices_TextBox.ReadOnly = true;
            this.printDevices_TextBox.Size = new System.Drawing.Size(404, 23);
            this.printDevices_TextBox.TabIndex = 1;
            // 
            // selectPrinters_Button
            // 
            this.selectPrinters_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectPrinters_Button.Location = new System.Drawing.Point(510, 9);
            this.selectPrinters_Button.Name = "selectPrinters_Button";
            this.selectPrinters_Button.Size = new System.Drawing.Size(40, 27);
            this.selectPrinters_Button.TabIndex = 2;
            this.selectPrinters_Button.Text = "...";
            this.selectPrinters_Button.UseVisualStyleBackColor = true;
            this.selectPrinters_Button.Click += new System.EventHandler(this.selectPrinters_Button_Click);
            // 
            // devices_Label
            // 
            this.devices_Label.AutoSize = true;
            this.devices_Label.Location = new System.Drawing.Point(9, 15);
            this.devices_Label.Name = "devices_Label";
            this.devices_Label.Size = new System.Drawing.Size(83, 15);
            this.devices_Label.TabIndex = 0;
            this.devices_Label.Text = "Print Device(s)";
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(475, 307);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 10;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(394, 307);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 9;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // portType_ComboBox
            // 
            this.portType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portType_ComboBox.FormattingEnabled = true;
            this.portType_ComboBox.Location = new System.Drawing.Point(100, 120);
            this.portType_ComboBox.Name = "portType_ComboBox";
            this.portType_ComboBox.Size = new System.Drawing.Size(121, 23);
            this.portType_ComboBox.TabIndex = 5;
            this.portType_ComboBox.SelectedIndexChanged += new System.EventHandler(this.portType_ComboBox_SelectedIndexChanged);
            // 
            // portType_Label
            // 
            this.portType_Label.AutoSize = true;
            this.portType_Label.Location = new System.Drawing.Point(34, 123);
            this.portType_Label.Name = "portType_Label";
            this.portType_Label.Size = new System.Drawing.Size(57, 15);
            this.portType_Label.TabIndex = 4;
            this.portType_Label.Text = "Port Type";
            // 
            // queueName_Label
            // 
            this.queueName_Label.AutoSize = true;
            this.queueName_Label.Location = new System.Drawing.Point(248, 123);
            this.queueName_Label.Name = "queueName_Label";
            this.queueName_Label.Size = new System.Drawing.Size(77, 15);
            this.queueName_Label.TabIndex = 6;
            this.queueName_Label.Text = "Queue Name";
            // 
            // queueName_TextBox
            // 
            this.queueName_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queueName_TextBox.Location = new System.Drawing.Point(331, 120);
            this.queueName_TextBox.Name = "queueName_TextBox";
            this.queueName_TextBox.Size = new System.Drawing.Size(219, 23);
            this.queueName_TextBox.TabIndex = 7;
            // 
            // cfmFile_GroupBox
            // 
            this.cfmFile_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cfmFile_GroupBox.Controls.Add(this.filterCfmList_CheckBox);
            this.cfmFile_GroupBox.Controls.Add(this.shortcut_Label);
            this.cfmFile_GroupBox.Controls.Add(this.shortcut_ComboBox);
            this.cfmFile_GroupBox.Controls.Add(this.cfmFile_Label);
            this.cfmFile_GroupBox.Controls.Add(this.cfmFile_ComboBox);
            this.cfmFile_GroupBox.Location = new System.Drawing.Point(12, 160);
            this.cfmFile_GroupBox.Name = "cfmFile_GroupBox";
            this.cfmFile_GroupBox.Size = new System.Drawing.Size(538, 141);
            this.cfmFile_GroupBox.TabIndex = 8;
            this.cfmFile_GroupBox.TabStop = false;
            this.cfmFile_GroupBox.Text = "Optional Printer Configuration File";
            // 
            // filterCfmList_CheckBox
            // 
            this.filterCfmList_CheckBox.AutoSize = true;
            this.filterCfmList_CheckBox.Checked = true;
            this.filterCfmList_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.filterCfmList_CheckBox.Location = new System.Drawing.Point(92, 62);
            this.filterCfmList_CheckBox.Name = "filterCfmList_CheckBox";
            this.filterCfmList_CheckBox.Size = new System.Drawing.Size(261, 19);
            this.filterCfmList_CheckBox.TabIndex = 2;
            this.filterCfmList_CheckBox.Text = "Filter CFM list by selected product and driver";
            this.filterCfmList_CheckBox.UseVisualStyleBackColor = true;
            this.filterCfmList_CheckBox.CheckedChanged += new System.EventHandler(this.filterCfmList_CheckBox_CheckedChanged);
            // 
            // shortcut_Label
            // 
            this.shortcut_Label.AutoSize = true;
            this.shortcut_Label.Location = new System.Drawing.Point(6, 102);
            this.shortcut_Label.Name = "shortcut_Label";
            this.shortcut_Label.Size = new System.Drawing.Size(80, 15);
            this.shortcut_Label.TabIndex = 3;
            this.shortcut_Label.Text = "Print Shortcut";
            // 
            // shortcut_ComboBox
            // 
            this.shortcut_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shortcut_ComboBox.FormattingEnabled = true;
            this.shortcut_ComboBox.Location = new System.Drawing.Point(92, 99);
            this.shortcut_ComboBox.Name = "shortcut_ComboBox";
            this.shortcut_ComboBox.Size = new System.Drawing.Size(431, 23);
            this.shortcut_ComboBox.TabIndex = 4;
            // 
            // cfmFile_Label
            // 
            this.cfmFile_Label.AutoSize = true;
            this.cfmFile_Label.Location = new System.Drawing.Point(33, 30);
            this.cfmFile_Label.Name = "cfmFile_Label";
            this.cfmFile_Label.Size = new System.Drawing.Size(53, 15);
            this.cfmFile_Label.TabIndex = 0;
            this.cfmFile_Label.Text = "CFM File";
            // 
            // cfmFile_ComboBox
            // 
            this.cfmFile_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cfmFile_ComboBox.FormattingEnabled = true;
            this.cfmFile_ComboBox.Location = new System.Drawing.Point(92, 27);
            this.cfmFile_ComboBox.Name = "cfmFile_ComboBox";
            this.cfmFile_ComboBox.Size = new System.Drawing.Size(431, 23);
            this.cfmFile_ComboBox.TabIndex = 1;
            this.cfmFile_ComboBox.SelectedIndexChanged += new System.EventHandler(this.cfmFile_ComboBox_SelectedIndexChanged);
            // 
            // printDriverSelectionControl
            // 
            this.printDriverSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printDriverSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printDriverSelectionControl.Location = new System.Drawing.Point(12, 50);
            this.printDriverSelectionControl.Name = "printDriverSelectionControl";
            this.printDriverSelectionControl.Size = new System.Drawing.Size(538, 56);
            this.printDriverSelectionControl.TabIndex = 3;
            this.printDriverSelectionControl.SelectionChanged += new System.EventHandler(this.printDriverSelectionControl_SelectionChanged);
            // 
            // DynamicLocalPrintQueueForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(562, 342);
            this.Controls.Add(this.cfmFile_GroupBox);
            this.Controls.Add(this.queueName_TextBox);
            this.Controls.Add(this.queueName_Label);
            this.Controls.Add(this.portType_Label);
            this.Controls.Add(this.portType_ComboBox);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.devices_Label);
            this.Controls.Add(this.printDriverSelectionControl);
            this.Controls.Add(this.selectPrinters_Button);
            this.Controls.Add(this.printDevices_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DynamicLocalPrintQueueForm";
            this.Text = "Dynamic Local Print Queue Configuration";
            this.cfmFile_GroupBox.ResumeLayout(false);
            this.cfmFile_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox printDevices_TextBox;
        private System.Windows.Forms.Button selectPrinters_Button;
        private PrintDriverSelectionControl printDriverSelectionControl;
        private System.Windows.Forms.Label devices_Label;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.ComboBox portType_ComboBox;
        private System.Windows.Forms.Label portType_Label;
        private System.Windows.Forms.Label queueName_Label;
        private System.Windows.Forms.TextBox queueName_TextBox;
        private FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox cfmFile_GroupBox;
        private System.Windows.Forms.CheckBox filterCfmList_CheckBox;
        private System.Windows.Forms.Label shortcut_Label;
        private System.Windows.Forms.ComboBox shortcut_ComboBox;
        private System.Windows.Forms.Label cfmFile_Label;
        private System.Windows.Forms.ComboBox cfmFile_ComboBox;
    }
}
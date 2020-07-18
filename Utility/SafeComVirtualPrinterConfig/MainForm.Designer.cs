namespace SafeComVirtualPrinterConfig
{
    partial class MainForm
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
            this.label_Begin = new System.Windows.Forms.Label();
            this.textBox_Octet1 = new System.Windows.Forms.TextBox();
            this.textBox_Octet2 = new System.Windows.Forms.TextBox();
            this.textBox_Octet3 = new System.Windows.Forms.TextBox();
            this.groupBox_Address = new System.Windows.Forms.GroupBox();
            this.textBox_End4 = new System.Windows.Forms.TextBox();
            this.textBox_End3 = new System.Windows.Forms.TextBox();
            this.textBox_End1 = new System.Windows.Forms.TextBox();
            this.textBox_End2 = new System.Windows.Forms.TextBox();
            this.label_End = new System.Windows.Forms.Label();
            this.label_Count = new System.Windows.Forms.Label();
            this.textBox_Count = new System.Windows.Forms.TextBox();
            this.textBox_Octet4 = new System.Windows.Forms.TextBox();
            this.button_Close = new System.Windows.Forms.Button();
            this.button_Generate = new System.Windows.Forms.Button();
            this.label_OutputPath = new System.Windows.Forms.Label();
            this.label_Output = new System.Windows.Forms.Label();
            this.linkLabel_SetLocation = new System.Windows.Forms.LinkLabel();
            this.label_Exe = new System.Windows.Forms.Label();
            this.label_ExePath = new System.Windows.Forms.Label();
            this.label_Install = new System.Windows.Forms.Label();
            this.label_InstallPath = new System.Windows.Forms.Label();
            this.radioButton_File = new System.Windows.Forms.RadioButton();
            this.radioButton_New = new System.Windows.Forms.RadioButton();
            this.textBox_SourceFile = new System.Windows.Forms.TextBox();
            this.button_SetSourceFile = new System.Windows.Forms.Button();
            this.groupBox_Address.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Begin
            // 
            this.label_Begin.Location = new System.Drawing.Point(11, 25);
            this.label_Begin.Name = "label_Begin";
            this.label_Begin.Size = new System.Drawing.Size(137, 18);
            this.label_Begin.TabIndex = 4;
            this.label_Begin.Text = "Beginning IP Address:";
            this.label_Begin.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_Octet1
            // 
            this.textBox_Octet1.Location = new System.Drawing.Point(154, 22);
            this.textBox_Octet1.MaxLength = 3;
            this.textBox_Octet1.Name = "textBox_Octet1";
            this.textBox_Octet1.Size = new System.Drawing.Size(51, 20);
            this.textBox_Octet1.TabIndex = 8;
            this.textBox_Octet1.Text = "15";
            this.textBox_Octet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Octet1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumbersOnlyTextBox_KeyDown);
            this.textBox_Octet1.Leave += new System.EventHandler(this.textBox_Octet_Leave);
            this.textBox_Octet1.Validating += new System.ComponentModel.CancelEventHandler(this.IP_TextBox_Validating);
            // 
            // textBox_Octet2
            // 
            this.textBox_Octet2.Location = new System.Drawing.Point(211, 22);
            this.textBox_Octet2.MaxLength = 3;
            this.textBox_Octet2.Name = "textBox_Octet2";
            this.textBox_Octet2.Size = new System.Drawing.Size(51, 20);
            this.textBox_Octet2.TabIndex = 10;
            this.textBox_Octet2.Text = "199";
            this.textBox_Octet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Octet2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumbersOnlyTextBox_KeyDown);
            this.textBox_Octet2.Leave += new System.EventHandler(this.textBox_Octet_Leave);
            this.textBox_Octet2.Validating += new System.ComponentModel.CancelEventHandler(this.IP_TextBox_Validating);
            // 
            // textBox_Octet3
            // 
            this.textBox_Octet3.Location = new System.Drawing.Point(268, 22);
            this.textBox_Octet3.MaxLength = 3;
            this.textBox_Octet3.Name = "textBox_Octet3";
            this.textBox_Octet3.Size = new System.Drawing.Size(51, 20);
            this.textBox_Octet3.TabIndex = 11;
            this.textBox_Octet3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Octet3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumbersOnlyTextBox_KeyDown);
            this.textBox_Octet3.Leave += new System.EventHandler(this.textBox_Octet_Leave);
            this.textBox_Octet3.Validating += new System.ComponentModel.CancelEventHandler(this.IP_TextBox_Validating);
            // 
            // groupBox_Address
            // 
            this.groupBox_Address.Controls.Add(this.textBox_End4);
            this.groupBox_Address.Controls.Add(this.textBox_End3);
            this.groupBox_Address.Controls.Add(this.textBox_End1);
            this.groupBox_Address.Controls.Add(this.textBox_End2);
            this.groupBox_Address.Controls.Add(this.label_End);
            this.groupBox_Address.Controls.Add(this.label_Count);
            this.groupBox_Address.Controls.Add(this.textBox_Count);
            this.groupBox_Address.Controls.Add(this.textBox_Octet4);
            this.groupBox_Address.Controls.Add(this.label_Begin);
            this.groupBox_Address.Controls.Add(this.textBox_Octet3);
            this.groupBox_Address.Controls.Add(this.textBox_Octet1);
            this.groupBox_Address.Controls.Add(this.textBox_Octet2);
            this.groupBox_Address.Location = new System.Drawing.Point(83, 98);
            this.groupBox_Address.Name = "groupBox_Address";
            this.groupBox_Address.Size = new System.Drawing.Size(391, 105);
            this.groupBox_Address.TabIndex = 12;
            this.groupBox_Address.TabStop = false;
            this.groupBox_Address.Text = "Virtual Printer IP Address(es)";
            // 
            // textBox_End4
            // 
            this.textBox_End4.Location = new System.Drawing.Point(325, 74);
            this.textBox_End4.MaxLength = 3;
            this.textBox_End4.Name = "textBox_End4";
            this.textBox_End4.ReadOnly = true;
            this.textBox_End4.Size = new System.Drawing.Size(51, 20);
            this.textBox_End4.TabIndex = 19;
            this.textBox_End4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_End3
            // 
            this.textBox_End3.Location = new System.Drawing.Point(268, 74);
            this.textBox_End3.MaxLength = 3;
            this.textBox_End3.Name = "textBox_End3";
            this.textBox_End3.ReadOnly = true;
            this.textBox_End3.Size = new System.Drawing.Size(51, 20);
            this.textBox_End3.TabIndex = 18;
            this.textBox_End3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_End1
            // 
            this.textBox_End1.Location = new System.Drawing.Point(154, 74);
            this.textBox_End1.MaxLength = 3;
            this.textBox_End1.Name = "textBox_End1";
            this.textBox_End1.ReadOnly = true;
            this.textBox_End1.Size = new System.Drawing.Size(51, 20);
            this.textBox_End1.TabIndex = 16;
            this.textBox_End1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_End2
            // 
            this.textBox_End2.Location = new System.Drawing.Point(211, 74);
            this.textBox_End2.MaxLength = 3;
            this.textBox_End2.Name = "textBox_End2";
            this.textBox_End2.ReadOnly = true;
            this.textBox_End2.Size = new System.Drawing.Size(51, 20);
            this.textBox_End2.TabIndex = 17;
            this.textBox_End2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_End
            // 
            this.label_End.Location = new System.Drawing.Point(11, 77);
            this.label_End.Name = "label_End";
            this.label_End.Size = new System.Drawing.Size(137, 18);
            this.label_End.TabIndex = 15;
            this.label_End.Text = "Ending IP Address:";
            this.label_End.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_Count
            // 
            this.label_Count.Location = new System.Drawing.Point(100, 51);
            this.label_Count.Name = "label_Count";
            this.label_Count.Size = new System.Drawing.Size(47, 18);
            this.label_Count.TabIndex = 14;
            this.label_Count.Text = "Count:";
            this.label_Count.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBox_Count
            // 
            this.textBox_Count.Location = new System.Drawing.Point(154, 48);
            this.textBox_Count.MaxLength = 3;
            this.textBox_Count.Name = "textBox_Count";
            this.textBox_Count.Size = new System.Drawing.Size(51, 20);
            this.textBox_Count.TabIndex = 13;
            this.textBox_Count.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumbersOnlyTextBox_KeyDown);
            this.textBox_Count.Leave += new System.EventHandler(this.textBox_Count_Leave);
            this.textBox_Count.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Count_Validating);
            // 
            // textBox_Octet4
            // 
            this.textBox_Octet4.Location = new System.Drawing.Point(325, 22);
            this.textBox_Octet4.MaxLength = 3;
            this.textBox_Octet4.Name = "textBox_Octet4";
            this.textBox_Octet4.Size = new System.Drawing.Size(51, 20);
            this.textBox_Octet4.TabIndex = 12;
            this.textBox_Octet4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumbersOnlyTextBox_KeyDown);
            this.textBox_Octet4.Leave += new System.EventHandler(this.textBox_Octet_Leave);
            this.textBox_Octet4.Validating += new System.ComponentModel.CancelEventHandler(this.IP_TextBox_Validating);
            // 
            // button_Close
            // 
            this.button_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Close.Location = new System.Drawing.Point(403, 380);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(75, 23);
            this.button_Close.TabIndex = 13;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button_Generate
            // 
            this.button_Generate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Generate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Generate.Location = new System.Drawing.Point(275, 380);
            this.button_Generate.Name = "button_Generate";
            this.button_Generate.Size = new System.Drawing.Size(109, 23);
            this.button_Generate.TabIndex = 14;
            this.button_Generate.Text = "Generate Files";
            this.button_Generate.UseVisualStyleBackColor = true;
            this.button_Generate.Click += new System.EventHandler(this.button_Generate_Click);
            // 
            // label_OutputPath
            // 
            this.label_OutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_OutputPath.Location = new System.Drawing.Point(112, 290);
            this.label_OutputPath.Name = "label_OutputPath";
            this.label_OutputPath.Size = new System.Drawing.Size(291, 18);
            this.label_OutputPath.TabIndex = 15;
            this.label_OutputPath.Text = "C:\\VirtualResource\\SNMPSimulator\\Data";
            // 
            // label_Output
            // 
            this.label_Output.AutoSize = true;
            this.label_Output.Location = new System.Drawing.Point(11, 290);
            this.label_Output.Name = "label_Output";
            this.label_Output.Size = new System.Drawing.Size(74, 13);
            this.label_Output.TabIndex = 16;
            this.label_Output.Text = "Output Folder:";
            // 
            // linkLabel_SetLocation
            // 
            this.linkLabel_SetLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel_SetLocation.AutoSize = true;
            this.linkLabel_SetLocation.Location = new System.Drawing.Point(407, 244);
            this.linkLabel_SetLocation.Name = "linkLabel_SetLocation";
            this.linkLabel_SetLocation.Size = new System.Drawing.Size(67, 13);
            this.linkLabel_SetLocation.TabIndex = 18;
            this.linkLabel_SetLocation.TabStop = true;
            this.linkLabel_SetLocation.Text = "Set Location";
            this.linkLabel_SetLocation.Click += new System.EventHandler(this.linkLabel_SetLocation_Click);
            // 
            // label_Exe
            // 
            this.label_Exe.AutoSize = true;
            this.label_Exe.Location = new System.Drawing.Point(11, 270);
            this.label_Exe.Name = "label_Exe";
            this.label_Exe.Size = new System.Drawing.Size(97, 13);
            this.label_Exe.TabIndex = 20;
            this.label_Exe.Text = "Simulator Location:";
            // 
            // label_ExePath
            // 
            this.label_ExePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_ExePath.Location = new System.Drawing.Point(112, 270);
            this.label_ExePath.Name = "label_ExePath";
            this.label_ExePath.Size = new System.Drawing.Size(291, 20);
            this.label_ExePath.TabIndex = 19;
            this.label_ExePath.Text = "C:\\VirtualResource\\SNMPSimulator\\snmpsimd.exe";
            // 
            // label_Install
            // 
            this.label_Install.AutoSize = true;
            this.label_Install.Location = new System.Drawing.Point(9, 244);
            this.label_Install.Name = "label_Install";
            this.label_Install.Size = new System.Drawing.Size(92, 13);
            this.label_Install.TabIndex = 22;
            this.label_Install.Text = "Installation Folder:";
            // 
            // label_InstallPath
            // 
            this.label_InstallPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_InstallPath.Location = new System.Drawing.Point(110, 244);
            this.label_InstallPath.Name = "label_InstallPath";
            this.label_InstallPath.Size = new System.Drawing.Size(291, 18);
            this.label_InstallPath.TabIndex = 21;
            this.label_InstallPath.Text = "C:\\VirtualResource\\SNMPSimulator";
            // 
            // radioButton_File
            // 
            this.radioButton_File.AutoSize = true;
            this.radioButton_File.Location = new System.Drawing.Point(14, 12);
            this.radioButton_File.Name = "radioButton_File";
            this.radioButton_File.Size = new System.Drawing.Size(148, 17);
            this.radioButton_File.TabIndex = 23;
            this.radioButton_File.Text = "Generate from Source File";
            this.radioButton_File.UseVisualStyleBackColor = true;
            this.radioButton_File.CheckedChanged += new System.EventHandler(this.GenerateSetting_CheckedChanged);
            // 
            // radioButton_New
            // 
            this.radioButton_New.AutoSize = true;
            this.radioButton_New.Checked = true;
            this.radioButton_New.Location = new System.Drawing.Point(14, 75);
            this.radioButton_New.Name = "radioButton_New";
            this.radioButton_New.Size = new System.Drawing.Size(94, 17);
            this.radioButton_New.TabIndex = 24;
            this.radioButton_New.TabStop = true;
            this.radioButton_New.Text = "Generate New";
            this.radioButton_New.UseVisualStyleBackColor = true;
            // 
            // textBox_SourceFile
            // 
            this.textBox_SourceFile.Location = new System.Drawing.Point(83, 35);
            this.textBox_SourceFile.Name = "textBox_SourceFile";
            this.textBox_SourceFile.Size = new System.Drawing.Size(344, 20);
            this.textBox_SourceFile.TabIndex = 25;
            // 
            // button_SetSourceFile
            // 
            this.button_SetSourceFile.Location = new System.Drawing.Point(433, 33);
            this.button_SetSourceFile.Name = "button_SetSourceFile";
            this.button_SetSourceFile.Size = new System.Drawing.Size(26, 23);
            this.button_SetSourceFile.TabIndex = 26;
            this.button_SetSourceFile.Text = "...";
            this.button_SetSourceFile.UseVisualStyleBackColor = true;
            this.button_SetSourceFile.Click += new System.EventHandler(this.button_SetSourceFile_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.button_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 406);
            this.Controls.Add(this.button_SetSourceFile);
            this.Controls.Add(this.textBox_SourceFile);
            this.Controls.Add(this.radioButton_New);
            this.Controls.Add(this.radioButton_File);
            this.Controls.Add(this.label_Install);
            this.Controls.Add(this.label_InstallPath);
            this.Controls.Add(this.label_Exe);
            this.Controls.Add(this.label_ExePath);
            this.Controls.Add(this.linkLabel_SetLocation);
            this.Controls.Add(this.label_Output);
            this.Controls.Add(this.label_OutputPath);
            this.Controls.Add(this.button_Generate);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.groupBox_Address);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "SafeCom Virtual Printer Setup Tool";
            this.groupBox_Address.ResumeLayout(false);
            this.groupBox_Address.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Begin;
        private System.Windows.Forms.TextBox textBox_Octet1;
        private System.Windows.Forms.TextBox textBox_Octet2;
        private System.Windows.Forms.TextBox textBox_Octet3;
        private System.Windows.Forms.GroupBox groupBox_Address;
        private System.Windows.Forms.Label label_Count;
        private System.Windows.Forms.TextBox textBox_Count;
        private System.Windows.Forms.TextBox textBox_Octet4;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_Generate;
        private System.Windows.Forms.Label label_OutputPath;
        private System.Windows.Forms.Label label_Output;
        private System.Windows.Forms.Label label_End;
        private System.Windows.Forms.TextBox textBox_End4;
        private System.Windows.Forms.TextBox textBox_End3;
        private System.Windows.Forms.TextBox textBox_End1;
        private System.Windows.Forms.TextBox textBox_End2;
        private System.Windows.Forms.LinkLabel linkLabel_SetLocation;
        private System.Windows.Forms.Label label_Exe;
        private System.Windows.Forms.Label label_ExePath;
        private System.Windows.Forms.Label label_Install;
        private System.Windows.Forms.Label label_InstallPath;
        private System.Windows.Forms.RadioButton radioButton_File;
        private System.Windows.Forms.RadioButton radioButton_New;
        private System.Windows.Forms.TextBox textBox_SourceFile;
        private System.Windows.Forms.Button button_SetSourceFile;
    }
}


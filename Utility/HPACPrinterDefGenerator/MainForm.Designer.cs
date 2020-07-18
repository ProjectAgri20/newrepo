namespace HPACPrinterDefGenerator
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
            this.label_VPrintHost = new System.Windows.Forms.Label();
            this.label_Begin = new System.Windows.Forms.Label();
            this.label_Model = new System.Windows.Forms.Label();
            this.label_Port = new System.Windows.Forms.Label();
            this.textBox_VPrintHost = new System.Windows.Forms.TextBox();
            this.textBox_Octet1 = new System.Windows.Forms.TextBox();
            this.textBox_Model = new System.Windows.Forms.TextBox();
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
            this.label_FilePath = new System.Windows.Forms.Label();
            this.label_Destination = new System.Windows.Forms.Label();
            this.button_ChooseFolder = new System.Windows.Forms.Button();
            this.label_PortLabel = new System.Windows.Forms.Label();
            this.groupBox_Address.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_VPrintHost
            // 
            this.label_VPrintHost.Location = new System.Drawing.Point(5, 14);
            this.label_VPrintHost.Name = "label_VPrintHost";
            this.label_VPrintHost.Size = new System.Drawing.Size(155, 18);
            this.label_VPrintHost.TabIndex = 3;
            this.label_VPrintHost.Text = "Virtual Printer Host Name:";
            this.label_VPrintHost.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // label_Model
            // 
            this.label_Model.Location = new System.Drawing.Point(59, 173);
            this.label_Model.Name = "label_Model";
            this.label_Model.Size = new System.Drawing.Size(100, 18);
            this.label_Model.TabIndex = 5;
            this.label_Model.Text = "Printer Model:";
            this.label_Model.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label_Port
            // 
            this.label_Port.Location = new System.Drawing.Point(166, 205);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(236, 37);
            this.label_Port.TabIndex = 6;
            this.label_Port.Text = "9100  ( Changing manually may break communication to the virtual printer. )";
            // 
            // textBox_VPrintHost
            // 
            this.textBox_VPrintHost.Location = new System.Drawing.Point(166, 12);
            this.textBox_VPrintHost.Name = "textBox_VPrintHost";
            this.textBox_VPrintHost.Size = new System.Drawing.Size(237, 20);
            this.textBox_VPrintHost.TabIndex = 7;
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
            // textBox_Model
            // 
            this.textBox_Model.Location = new System.Drawing.Point(166, 171);
            this.textBox_Model.Name = "textBox_Model";
            this.textBox_Model.Size = new System.Drawing.Size(237, 20);
            this.textBox_Model.TabIndex = 9;
            this.textBox_Model.Text = "HP LaserJet color flow MFP M575              ";
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
            this.groupBox_Address.Location = new System.Drawing.Point(12, 47);
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
            this.button_Close.Location = new System.Drawing.Point(337, 298);
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
            this.button_Generate.Location = new System.Drawing.Point(209, 298);
            this.button_Generate.Name = "button_Generate";
            this.button_Generate.Size = new System.Drawing.Size(109, 23);
            this.button_Generate.TabIndex = 14;
            this.button_Generate.Text = "Generate Files";
            this.button_Generate.UseVisualStyleBackColor = true;
            this.button_Generate.Click += new System.EventHandler(this.button_Generate_Click);
            // 
            // label_FilePath
            // 
            this.label_FilePath.Location = new System.Drawing.Point(100, 259);
            this.label_FilePath.Name = "label_FilePath";
            this.label_FilePath.Size = new System.Drawing.Size(273, 38);
            this.label_FilePath.TabIndex = 15;
            this.label_FilePath.Text = "C:\\Program Files\\Hewlett-Packard\\HP Access Control\\spoolroot\\prtr ";
            // 
            // label_Destination
            // 
            this.label_Destination.AutoSize = true;
            this.label_Destination.Location = new System.Drawing.Point(12, 259);
            this.label_Destination.Name = "label_Destination";
            this.label_Destination.Size = new System.Drawing.Size(82, 13);
            this.label_Destination.TabIndex = 16;
            this.label_Destination.Text = "File Destination:";
            // 
            // button_ChooseFolder
            // 
            this.button_ChooseFolder.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_ChooseFolder.Location = new System.Drawing.Point(379, 254);
            this.button_ChooseFolder.Name = "button_ChooseFolder";
            this.button_ChooseFolder.Size = new System.Drawing.Size(23, 23);
            this.button_ChooseFolder.TabIndex = 17;
            this.button_ChooseFolder.Text = "...";
            this.button_ChooseFolder.UseVisualStyleBackColor = true;
            this.button_ChooseFolder.Click += new System.EventHandler(this.button_ChooseFolder_Click);
            // 
            // label_PortLabel
            // 
            this.label_PortLabel.AutoSize = true;
            this.label_PortLabel.Location = new System.Drawing.Point(130, 205);
            this.label_PortLabel.Name = "label_PortLabel";
            this.label_PortLabel.Size = new System.Drawing.Size(29, 13);
            this.label_PortLabel.TabIndex = 18;
            this.label_PortLabel.Text = "Port:";
            // 
            // MainForm
            // 
            this.AcceptButton = this.button_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 324);
            this.Controls.Add(this.label_PortLabel);
            this.Controls.Add(this.button_ChooseFolder);
            this.Controls.Add(this.label_Destination);
            this.Controls.Add(this.label_FilePath);
            this.Controls.Add(this.button_Generate);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.groupBox_Address);
            this.Controls.Add(this.textBox_Model);
            this.Controls.Add(this.textBox_VPrintHost);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.label_Model);
            this.Controls.Add(this.label_VPrintHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "HPAC Printer Definition File Generation Tool";
            this.groupBox_Address.ResumeLayout(false);
            this.groupBox_Address.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_VPrintHost;
        private System.Windows.Forms.Label label_Begin;
        private System.Windows.Forms.Label label_Model;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.TextBox textBox_VPrintHost;
        private System.Windows.Forms.TextBox textBox_Octet1;
        private System.Windows.Forms.TextBox textBox_Model;
        private System.Windows.Forms.TextBox textBox_Octet2;
        private System.Windows.Forms.TextBox textBox_Octet3;
        private System.Windows.Forms.GroupBox groupBox_Address;
        private System.Windows.Forms.Label label_Count;
        private System.Windows.Forms.TextBox textBox_Count;
        private System.Windows.Forms.TextBox textBox_Octet4;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_Generate;
        private System.Windows.Forms.Label label_FilePath;
        private System.Windows.Forms.Label label_Destination;
        private System.Windows.Forms.Button button_ChooseFolder;
        private System.Windows.Forms.Label label_PortLabel;
        private System.Windows.Forms.Label label_End;
        private System.Windows.Forms.TextBox textBox_End4;
        private System.Windows.Forms.TextBox textBox_End3;
        private System.Windows.Forms.TextBox textBox_End1;
        private System.Windows.Forms.TextBox textBox_End2;
    }
}


namespace HP.ScalableTest.LabConsole
{
    partial class FrameworkServerEditForm
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
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.operatingSystem_ComboBox = new System.Windows.Forms.ComboBox();
            this.hostname_TextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.disk_TextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.memory_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cores_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.processors_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.x64_RadioButton = new System.Windows.Forms.RadioButton();
            this.x86_RadioButton = new System.Windows.Forms.RadioButton();
            this.serverTypes_ListBox = new System.Windows.Forms.CheckedListBox();
            this.scanning_Button = new System.Windows.Forms.Button();
            this.scanning_PictureBox = new System.Windows.Forms.PictureBox();
            this.scanning_Label = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.status_ComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.contact_TextBox = new System.Windows.Forms.TextBox();
            this.active_CheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ipAddress_textBox = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.service_TextBox = new System.Windows.Forms.TextBox();
            this.settings_Button = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memory_NumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cores_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processors_NumericUpDown)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanning_PictureBox)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(921, 527);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(112, 32);
            this.cancel_Button.TabIndex = 0;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(800, 527);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(112, 32);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Hostname";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.operatingSystem_ComboBox);
            this.groupBox3.Location = new System.Drawing.Point(561, 74);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(479, 89);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Operating System";
            // 
            // operatingSystem_ComboBox
            // 
            this.operatingSystem_ComboBox.FormattingEnabled = true;
            this.operatingSystem_ComboBox.Location = new System.Drawing.Point(9, 35);
            this.operatingSystem_ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.operatingSystem_ComboBox.Name = "operatingSystem_ComboBox";
            this.operatingSystem_ComboBox.Size = new System.Drawing.Size(453, 23);
            this.operatingSystem_ComboBox.TabIndex = 12;
            // 
            // hostname_TextBox
            // 
            this.hostname_TextBox.Location = new System.Drawing.Point(111, 12);
            this.hostname_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.hostname_TextBox.Name = "hostname_TextBox";
            this.hostname_TextBox.Size = new System.Drawing.Size(400, 23);
            this.hostname_TextBox.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.disk_TextBox);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.memory_NumericUpDown);
            this.groupBox2.Location = new System.Drawing.Point(562, 308);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(479, 109);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Memory and Disk";
            // 
            // disk_TextBox
            // 
            this.disk_TextBox.Location = new System.Drawing.Point(145, 68);
            this.disk_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.disk_TextBox.Name = "disk_TextBox";
            this.disk_TextBox.Size = new System.Drawing.Size(317, 23);
            this.disk_TextBox.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(242, 35);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 15);
            this.label11.TabIndex = 12;
            this.label11.Text = "MB";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(53, 71);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 15);
            this.label10.TabIndex = 9;
            this.label10.Text = "Disk Space";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(74, 35);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "Memory";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // memory_NumericUpDown
            // 
            this.memory_NumericUpDown.Increment = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.memory_NumericUpDown.Location = new System.Drawing.Point(145, 32);
            this.memory_NumericUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.memory_NumericUpDown.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.memory_NumericUpDown.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.memory_NumericUpDown.Name = "memory_NumericUpDown";
            this.memory_NumericUpDown.Size = new System.Drawing.Size(87, 23);
            this.memory_NumericUpDown.TabIndex = 10;
            this.memory_NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.memory_NumericUpDown.ThousandsSeparator = true;
            this.memory_NumericUpDown.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cores_numericUpDown);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.processors_NumericUpDown);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(562, 172);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(479, 127);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Processor Architecture";
            // 
            // cores_numericUpDown
            // 
            this.cores_numericUpDown.Location = new System.Drawing.Point(395, 79);
            this.cores_numericUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.cores_numericUpDown.Name = "cores_numericUpDown";
            this.cores_numericUpDown.Size = new System.Drawing.Size(68, 23);
            this.cores_numericUpDown.TabIndex = 9;
            this.cores_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(243, 81);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 15);
            this.label8.TabIndex = 7;
            this.label8.Text = "Cores per Processor";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // processors_NumericUpDown
            // 
            this.processors_NumericUpDown.Location = new System.Drawing.Point(395, 43);
            this.processors_NumericUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.processors_NumericUpDown.Name = "processors_NumericUpDown";
            this.processors_NumericUpDown.Size = new System.Drawing.Size(68, 23);
            this.processors_NumericUpDown.TabIndex = 8;
            this.processors_NumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(231, 45);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 15);
            this.label7.TabIndex = 6;
            this.label7.Text = "Number of Processors";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.x64_RadioButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.x86_RadioButton, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(35, 38);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(153, 34);
            this.tableLayoutPanel2.TabIndex = 23;
            // 
            // x64_RadioButton
            // 
            this.x64_RadioButton.AutoSize = true;
            this.x64_RadioButton.Location = new System.Drawing.Point(54, 4);
            this.x64_RadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.x64_RadioButton.Name = "x64_RadioButton";
            this.x64_RadioButton.Size = new System.Drawing.Size(42, 19);
            this.x64_RadioButton.TabIndex = 1;
            this.x64_RadioButton.TabStop = true;
            this.x64_RadioButton.Text = "x64";
            this.x64_RadioButton.UseVisualStyleBackColor = true;
            this.x64_RadioButton.CheckedChanged += new System.EventHandler(this.architecture_RadioButton_CheckedChanged);
            // 
            // x86_RadioButton
            // 
            this.x86_RadioButton.AutoSize = true;
            this.x86_RadioButton.Location = new System.Drawing.Point(4, 4);
            this.x86_RadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.x86_RadioButton.Name = "x86_RadioButton";
            this.x86_RadioButton.Size = new System.Drawing.Size(42, 19);
            this.x86_RadioButton.TabIndex = 0;
            this.x86_RadioButton.TabStop = true;
            this.x86_RadioButton.Text = "x86";
            this.x86_RadioButton.UseVisualStyleBackColor = true;
            this.x86_RadioButton.CheckedChanged += new System.EventHandler(this.architecture_RadioButton_CheckedChanged);
            // 
            // serverTypes_ListBox
            // 
            this.serverTypes_ListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverTypes_ListBox.CheckOnClick = true;
            this.serverTypes_ListBox.FormattingEnabled = true;
            this.serverTypes_ListBox.Location = new System.Drawing.Point(8, 25);
            this.serverTypes_ListBox.Margin = new System.Windows.Forms.Padding(4);
            this.serverTypes_ListBox.Name = "serverTypes_ListBox";
            this.serverTypes_ListBox.Size = new System.Drawing.Size(523, 218);
            this.serverTypes_ListBox.TabIndex = 18;
            // 
            // scanning_Button
            // 
            this.scanning_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanning_Button.Location = new System.Drawing.Point(144, 527);
            this.scanning_Button.Margin = new System.Windows.Forms.Padding(4);
            this.scanning_Button.Name = "scanning_Button";
            this.scanning_Button.Size = new System.Drawing.Size(112, 32);
            this.scanning_Button.TabIndex = 19;
            this.scanning_Button.Text = "Rescan";
            this.scanning_Button.UseVisualStyleBackColor = true;
            this.scanning_Button.Click += new System.EventHandler(this.rescan_Button_Click);
            // 
            // scanning_PictureBox
            // 
            this.scanning_PictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanning_PictureBox.Location = new System.Drawing.Point(264, 515);
            this.scanning_PictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.scanning_PictureBox.Name = "scanning_PictureBox";
            this.scanning_PictureBox.Size = new System.Drawing.Size(48, 44);
            this.scanning_PictureBox.TabIndex = 20;
            this.scanning_PictureBox.TabStop = false;
            this.scanning_PictureBox.Visible = false;
            // 
            // scanning_Label
            // 
            this.scanning_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanning_Label.AutoSize = true;
            this.scanning_Label.Location = new System.Drawing.Point(320, 536);
            this.scanning_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.scanning_Label.Name = "scanning_Label";
            this.scanning_Label.Size = new System.Drawing.Size(99, 15);
            this.scanning_Label.TabIndex = 13;
            this.scanning_Label.Text = "Scanning server...";
            this.scanning_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.scanning_Label.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.status_ComboBox);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.contact_TextBox);
            this.groupBox4.Location = new System.Drawing.Point(15, 301);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(539, 116);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Server Status";
            // 
            // status_ComboBox
            // 
            this.status_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.status_ComboBox.FormattingEnabled = true;
            this.status_ComboBox.Location = new System.Drawing.Point(105, 70);
            this.status_ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.status_ComboBox.Name = "status_ComboBox";
            this.status_ComboBox.Size = new System.Drawing.Size(425, 23);
            this.status_ComboBox.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 73);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Status";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Contact";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contact_TextBox
            // 
            this.contact_TextBox.Location = new System.Drawing.Point(105, 34);
            this.contact_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.contact_TextBox.Name = "contact_TextBox";
            this.contact_TextBox.Size = new System.Drawing.Size(425, 23);
            this.contact_TextBox.TabIndex = 0;
            // 
            // active_CheckBox
            // 
            this.active_CheckBox.AutoSize = true;
            this.active_CheckBox.Location = new System.Drawing.Point(561, 12);
            this.active_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.active_CheckBox.Name = "active_CheckBox";
            this.active_CheckBox.Size = new System.Drawing.Size(289, 19);
            this.active_CheckBox.TabIndex = 22;
            this.active_CheckBox.Text = "Server is active and can be selected for use in tests";
            this.active_CheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.ipAddress_textBox);
            this.groupBox5.Location = new System.Drawing.Point(561, 426);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(479, 73);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Network Info";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 29);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 15);
            this.label12.TabIndex = 3;
            this.label12.Text = "IP Address";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipAddress_textBox
            // 
            this.ipAddress_textBox.Location = new System.Drawing.Point(105, 26);
            this.ipAddress_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.ipAddress_textBox.Name = "ipAddress_textBox";
            this.ipAddress_textBox.Size = new System.Drawing.Size(357, 23);
            this.ipAddress_textBox.TabIndex = 2;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.serverTypes_ListBox);
            this.groupBox6.Location = new System.Drawing.Point(15, 74);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(539, 225);
            this.groupBox6.TabIndex = 24;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Server Attributes (select all that apply)";
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.service_TextBox);
            this.groupBox7.Location = new System.Drawing.Point(14, 426);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox7.Size = new System.Drawing.Size(539, 73);
            this.groupBox7.TabIndex = 24;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Service Version (optional)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Version";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // service_TextBox
            // 
            this.service_TextBox.Location = new System.Drawing.Point(81, 26);
            this.service_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.service_TextBox.Name = "service_TextBox";
            this.service_TextBox.Size = new System.Drawing.Size(390, 23);
            this.service_TextBox.TabIndex = 2;
            // 
            // settings_Button
            // 
            this.settings_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settings_Button.Location = new System.Drawing.Point(24, 527);
            this.settings_Button.Margin = new System.Windows.Forms.Padding(4);
            this.settings_Button.Name = "settings_Button";
            this.settings_Button.Size = new System.Drawing.Size(112, 32);
            this.settings_Button.TabIndex = 25;
            this.settings_Button.Text = "Settings";
            this.settings_Button.UseVisualStyleBackColor = true;
            this.settings_Button.Click += new System.EventHandler(this.settings_Button_Click);
            // 
            // FrameworkServerEditForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(1052, 576);
            this.Controls.Add(this.settings_Button);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.active_CheckBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.scanning_Label);
            this.Controls.Add(this.scanning_PictureBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hostname_TextBox);
            this.Controls.Add(this.scanning_Button);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrameworkServerEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Properties";
            this.Load += new System.EventHandler(this.ServerInventoryEditForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memory_NumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cores_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processors_NumericUpDown)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanning_PictureBox)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox operatingSystem_ComboBox;
        private System.Windows.Forms.TextBox hostname_TextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox disk_TextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown memory_NumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown cores_numericUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown processors_NumericUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton x64_RadioButton;
        private System.Windows.Forms.RadioButton x86_RadioButton;
        private System.Windows.Forms.CheckedListBox serverTypes_ListBox;
        private System.Windows.Forms.Button scanning_Button;
        private System.Windows.Forms.PictureBox scanning_PictureBox;
        private System.Windows.Forms.Label scanning_Label;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox status_ComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox contact_TextBox;
        private System.Windows.Forms.CheckBox active_CheckBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ipAddress_textBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox service_TextBox;
        private System.Windows.Forms.Button settings_Button;
    }
}
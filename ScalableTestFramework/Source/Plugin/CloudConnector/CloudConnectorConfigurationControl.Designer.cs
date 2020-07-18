namespace HP.ScalableTest.Plugin.CloudConnector
{
    partial class CloudConnectorConfigurationControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_SignOut = new System.Windows.Forms.Label();
            this.comboBox_Logout = new System.Windows.Forms.ComboBox();
            this.label_SIO = new System.Windows.Forms.Label();
            this.comboBox_SIO = new System.Windows.Forms.ComboBox();
            this.lb_SharePointSites_Desc = new System.Windows.Forms.Label();
            this.tb_SharePointSites = new System.Windows.Forms.TextBox();
            this.lb_SharePointSites = new System.Windows.Forms.Label();
            this.lb_PWD = new System.Windows.Forms.Label();
            this.lb_ID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_PWD = new System.Windows.Forms.MaskedTextBox();
            this.tb_ID = new System.Windows.Forms.TextBox();
            this.cb_AppName = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_delete = new System.Windows.Forms.Button();
            this.lb_MPDesc = new System.Windows.Forms.Label();
            this.lb_SelectedFiles = new System.Windows.Forms.Label();
            this.btn_clear = new System.Windows.Forms.Button();
            this.lbx_FileNames = new System.Windows.Forms.ListBox();
            this.lb_MP_FileNames = new System.Windows.Forms.Label();
            this.btn_Add = new System.Windows.Forms.Button();
            this.lb_MP_FolderPath = new System.Windows.Forms.Label();
            this.tb_MP_FileNames = new System.Windows.Forms.TextBox();
            this.rb_MultipleFile = new System.Windows.Forms.RadioButton();
            this.tb_FolderPath = new System.Windows.Forms.TextBox();
            this.tb_MP_FolderPath = new System.Windows.Forms.TextBox();
            this.tb_FilePath = new System.Windows.Forms.TextBox();
            this.btn_Options = new System.Windows.Forms.Button();
            this.rb_Print = new System.Windows.Forms.RadioButton();
            this.rb_Scan = new System.Windows.Forms.RadioButton();
            this.lb_FilePath = new System.Windows.Forms.Label();
            this.lb_FolderPath = new System.Windows.Forms.Label();
            this.gb_PageCount = new System.Windows.Forms.GroupBox();
            this.lb_PageCount = new System.Windows.Forms.Label();
            this.nud_PageCount = new System.Windows.Forms.NumericUpDown();
            this.lb_PageCountDes = new System.Windows.Forms.Label();
            this.lb_FilePathDesc = new System.Windows.Forms.Label();
            this.lb_FolderPathDesc = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gb_PageCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PageCount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_SignOut);
            this.groupBox1.Controls.Add(this.comboBox_Logout);
            this.groupBox1.Controls.Add(this.label_SIO);
            this.groupBox1.Controls.Add(this.comboBox_SIO);
            this.groupBox1.Controls.Add(this.lb_SharePointSites_Desc);
            this.groupBox1.Controls.Add(this.tb_SharePointSites);
            this.groupBox1.Controls.Add(this.lb_SharePointSites);
            this.groupBox1.Controls.Add(this.lb_PWD);
            this.groupBox1.Controls.Add(this.lb_ID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_PWD);
            this.groupBox1.Controls.Add(this.tb_ID);
            this.groupBox1.Controls.Add(this.cb_AppName);
            this.groupBox1.Location = new System.Drawing.Point(23, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 274);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sign In / Sign Out";
            // 
            // label_SignOut
            // 
            this.label_SignOut.AutoSize = true;
            this.label_SignOut.Location = new System.Drawing.Point(15, 141);
            this.label_SignOut.Name = "label_SignOut";
            this.label_SignOut.Size = new System.Drawing.Size(59, 15);
            this.label_SignOut.TabIndex = 16;
            this.label_SignOut.Text = "Sign Out :";
            // 
            // comboBox_Logout
            // 
            this.comboBox_Logout.DisplayMember = "Value";
            this.comboBox_Logout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Logout.FormattingEnabled = true;
            this.comboBox_Logout.Location = new System.Drawing.Point(117, 138);
            this.comboBox_Logout.Name = "comboBox_Logout";
            this.comboBox_Logout.Size = new System.Drawing.Size(121, 23);
            this.comboBox_Logout.TabIndex = 0;
            this.comboBox_Logout.ValueMember = "Key";
            // 
            // label_SIO
            // 
            this.label_SIO.AutoSize = true;
            this.label_SIO.Location = new System.Drawing.Point(15, 28);
            this.label_SIO.Name = "label_SIO";
            this.label_SIO.Size = new System.Drawing.Size(49, 15);
            this.label_SIO.TabIndex = 15;
            this.label_SIO.Text = "Sign In :";
            // 
            // comboBox_SIO
            // 
            this.comboBox_SIO.DisplayMember = "Value";
            this.comboBox_SIO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SIO.FormattingEnabled = true;
            this.comboBox_SIO.Location = new System.Drawing.Point(117, 25);
            this.comboBox_SIO.Name = "comboBox_SIO";
            this.comboBox_SIO.Size = new System.Drawing.Size(121, 23);
            this.comboBox_SIO.TabIndex = 13;
            this.comboBox_SIO.ValueMember = "Key";
            this.comboBox_SIO.SelectedIndexChanged += new System.EventHandler(this.comboBox_SIO_SelectedIndexChanged);
            // 
            // lb_SharePointSites_Desc
            // 
            this.lb_SharePointSites_Desc.AutoSize = true;
            this.lb_SharePointSites_Desc.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_SharePointSites_Desc.Location = new System.Drawing.Point(24, 253);
            this.lb_SharePointSites_Desc.Name = "lb_SharePointSites_Desc";
            this.lb_SharePointSites_Desc.Size = new System.Drawing.Size(198, 15);
            this.lb_SharePointSites_Desc.TabIndex = 8;
            this.lb_SharePointSites_Desc.Text = "* Enter SharePoint Site Name exactly";
            this.lb_SharePointSites_Desc.Visible = false;
            // 
            // tb_SharePointSites
            // 
            this.tb_SharePointSites.Location = new System.Drawing.Point(117, 227);
            this.tb_SharePointSites.Name = "tb_SharePointSites";
            this.tb_SharePointSites.Size = new System.Drawing.Size(121, 23);
            this.tb_SharePointSites.TabIndex = 12;
            this.tb_SharePointSites.Visible = false;
            // 
            // lb_SharePointSites
            // 
            this.lb_SharePointSites.AutoSize = true;
            this.lb_SharePointSites.Location = new System.Drawing.Point(15, 230);
            this.lb_SharePointSites.Name = "lb_SharePointSites";
            this.lb_SharePointSites.Size = new System.Drawing.Size(89, 15);
            this.lb_SharePointSites.TabIndex = 11;
            this.lb_SharePointSites.Text = "SharePoint Site:";
            this.lb_SharePointSites.Visible = false;
            // 
            // lb_PWD
            // 
            this.lb_PWD.AutoSize = true;
            this.lb_PWD.Location = new System.Drawing.Point(15, 104);
            this.lb_PWD.Name = "lb_PWD";
            this.lb_PWD.Size = new System.Drawing.Size(63, 15);
            this.lb_PWD.TabIndex = 5;
            this.lb_PWD.Text = "Password :";
            // 
            // lb_ID
            // 
            this.lb_ID.AutoSize = true;
            this.lb_ID.Location = new System.Drawing.Point(15, 67);
            this.lb_ID.Name = "lb_ID";
            this.lb_ID.Size = new System.Drawing.Size(24, 15);
            this.lb_ID.TabIndex = 4;
            this.lb_ID.Text = "ID :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "App Name :";
            // 
            // tb_PWD
            // 
            this.tb_PWD.Enabled = false;
            this.tb_PWD.Location = new System.Drawing.Point(117, 101);
            this.tb_PWD.Name = "tb_PWD";
            this.tb_PWD.Size = new System.Drawing.Size(121, 23);
            this.tb_PWD.TabIndex = 2;
            // 
            // tb_ID
            // 
            this.tb_ID.Enabled = false;
            this.tb_ID.Location = new System.Drawing.Point(117, 64);
            this.tb_ID.Name = "tb_ID";
            this.tb_ID.Size = new System.Drawing.Size(121, 23);
            this.tb_ID.TabIndex = 1;
            // 
            // cb_AppName
            // 
            this.cb_AppName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_AppName.FormattingEnabled = true;
            this.cb_AppName.Location = new System.Drawing.Point(117, 178);
            this.cb_AppName.Name = "cb_AppName";
            this.cb_AppName.Size = new System.Drawing.Size(121, 23);
            this.cb_AppName.TabIndex = 0;
            this.cb_AppName.SelectedIndexChanged += new System.EventHandler(this.cb_AppName_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_delete);
            this.groupBox2.Controls.Add(this.lb_MPDesc);
            this.groupBox2.Controls.Add(this.lb_SelectedFiles);
            this.groupBox2.Controls.Add(this.btn_clear);
            this.groupBox2.Controls.Add(this.lbx_FileNames);
            this.groupBox2.Controls.Add(this.lb_MP_FileNames);
            this.groupBox2.Controls.Add(this.btn_Add);
            this.groupBox2.Controls.Add(this.lb_MP_FolderPath);
            this.groupBox2.Controls.Add(this.tb_MP_FileNames);
            this.groupBox2.Controls.Add(this.rb_MultipleFile);
            this.groupBox2.Controls.Add(this.tb_FolderPath);
            this.groupBox2.Controls.Add(this.tb_MP_FolderPath);
            this.groupBox2.Controls.Add(this.tb_FilePath);
            this.groupBox2.Controls.Add(this.btn_Options);
            this.groupBox2.Controls.Add(this.rb_Print);
            this.groupBox2.Controls.Add(this.rb_Scan);
            this.groupBox2.Controls.Add(this.lb_FilePath);
            this.groupBox2.Controls.Add(this.lb_FolderPath);
            this.groupBox2.Controls.Add(this.gb_PageCount);
            this.groupBox2.Controls.Add(this.lb_FilePathDesc);
            this.groupBox2.Controls.Add(this.lb_FolderPathDesc);
            this.groupBox2.Location = new System.Drawing.Point(279, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(464, 209);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(384, 86);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(63, 23);
            this.button_delete.TabIndex = 14;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = false;
            this.button_delete.Visible = false;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // lb_MPDesc
            // 
            this.lb_MPDesc.AutoSize = true;
            this.lb_MPDesc.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_MPDesc.Location = new System.Drawing.Point(152, 155);
            this.lb_MPDesc.Name = "lb_MPDesc";
            this.lb_MPDesc.Size = new System.Drawing.Size(291, 45);
            this.lb_MPDesc.TabIndex = 14;
            this.lb_MPDesc.Text = "* Enter print folder path to \"Folder path\". (Delimiter: /)\r\n Enter file name to \"" +
    "File name\", and push \"Add\". \r\n Each path must have a unique value.";
            this.lb_MPDesc.Visible = false;
            // 
            // lb_SelectedFiles
            // 
            this.lb_SelectedFiles.AutoSize = true;
            this.lb_SelectedFiles.Location = new System.Drawing.Point(148, 90);
            this.lb_SelectedFiles.Name = "lb_SelectedFiles";
            this.lb_SelectedFiles.Size = new System.Drawing.Size(51, 30);
            this.lb_SelectedFiles.TabIndex = 5;
            this.lb_SelectedFiles.Text = "Selected\r\nfiles :";
            this.lb_SelectedFiles.Visible = false;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(384, 128);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(63, 23);
            this.btn_clear.TabIndex = 13;
            this.btn_clear.Text = "Clear";
            this.btn_clear.UseVisualStyleBackColor = false;
            this.btn_clear.Visible = false;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // lbx_FileNames
            // 
            this.lbx_FileNames.FormattingEnabled = true;
            this.lbx_FileNames.ItemHeight = 15;
            this.lbx_FileNames.Location = new System.Drawing.Point(227, 87);
            this.lbx_FileNames.Name = "lbx_FileNames";
            this.lbx_FileNames.Size = new System.Drawing.Size(151, 64);
            this.lbx_FileNames.TabIndex = 9;
            this.lbx_FileNames.Visible = false;
            // 
            // lb_MP_FileNames
            // 
            this.lb_MP_FileNames.AutoSize = true;
            this.lb_MP_FileNames.Location = new System.Drawing.Point(148, 59);
            this.lb_MP_FileNames.Name = "lb_MP_FileNames";
            this.lb_MP_FileNames.Size = new System.Drawing.Size(67, 15);
            this.lb_MP_FileNames.TabIndex = 9;
            this.lb_MP_FileNames.Text = "File name : ";
            this.lb_MP_FileNames.Visible = false;
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(384, 56);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(63, 23);
            this.btn_Add.TabIndex = 12;
            this.btn_Add.Text = "Add";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Visible = false;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // lb_MP_FolderPath
            // 
            this.lb_MP_FolderPath.AutoSize = true;
            this.lb_MP_FolderPath.Location = new System.Drawing.Point(148, 28);
            this.lb_MP_FolderPath.Name = "lb_MP_FolderPath";
            this.lb_MP_FolderPath.Size = new System.Drawing.Size(73, 15);
            this.lb_MP_FolderPath.TabIndex = 5;
            this.lb_MP_FolderPath.Text = "Folder path :";
            this.lb_MP_FolderPath.Visible = false;
            // 
            // tb_MP_FileNames
            // 
            this.tb_MP_FileNames.Location = new System.Drawing.Point(227, 56);
            this.tb_MP_FileNames.Name = "tb_MP_FileNames";
            this.tb_MP_FileNames.Size = new System.Drawing.Size(151, 23);
            this.tb_MP_FileNames.TabIndex = 10;
            this.tb_MP_FileNames.Visible = false;
            // 
            // rb_MultipleFile
            // 
            this.rb_MultipleFile.AutoSize = true;
            this.rb_MultipleFile.Location = new System.Drawing.Point(17, 52);
            this.rb_MultipleFile.Name = "rb_MultipleFile";
            this.rb_MultipleFile.Size = new System.Drawing.Size(129, 19);
            this.rb_MultipleFile.TabIndex = 8;
            this.rb_MultipleFile.TabStop = true;
            this.rb_MultipleFile.Text = "Print (Multiple files)";
            this.rb_MultipleFile.UseVisualStyleBackColor = true;
            this.rb_MultipleFile.CheckedChanged += new System.EventHandler(this.rb_MultipleFile_CheckedChanged);
            // 
            // tb_FolderPath
            // 
            this.tb_FolderPath.Location = new System.Drawing.Point(227, 25);
            this.tb_FolderPath.Name = "tb_FolderPath";
            this.tb_FolderPath.Size = new System.Drawing.Size(220, 23);
            this.tb_FolderPath.TabIndex = 0;
            // 
            // tb_MP_FolderPath
            // 
            this.tb_MP_FolderPath.Location = new System.Drawing.Point(227, 25);
            this.tb_MP_FolderPath.Name = "tb_MP_FolderPath";
            this.tb_MP_FolderPath.Size = new System.Drawing.Size(220, 23);
            this.tb_MP_FolderPath.TabIndex = 6;
            this.tb_MP_FolderPath.Visible = false;
            // 
            // tb_FilePath
            // 
            this.tb_FilePath.Location = new System.Drawing.Point(227, 25);
            this.tb_FilePath.Name = "tb_FilePath";
            this.tb_FilePath.Size = new System.Drawing.Size(220, 23);
            this.tb_FilePath.TabIndex = 3;
            this.tb_FilePath.Visible = false;
            // 
            // btn_Options
            // 
            this.btn_Options.Location = new System.Drawing.Point(17, 84);
            this.btn_Options.Name = "btn_Options";
            this.btn_Options.Size = new System.Drawing.Size(106, 40);
            this.btn_Options.TabIndex = 2;
            this.btn_Options.Text = "Options";
            this.btn_Options.UseVisualStyleBackColor = true;
            this.btn_Options.Click += new System.EventHandler(this.btn_Options_Click);
            // 
            // rb_Print
            // 
            this.rb_Print.AutoSize = true;
            this.rb_Print.Location = new System.Drawing.Point(77, 26);
            this.rb_Print.Name = "rb_Print";
            this.rb_Print.Size = new System.Drawing.Size(50, 19);
            this.rb_Print.TabIndex = 1;
            this.rb_Print.Text = "Print";
            this.rb_Print.UseVisualStyleBackColor = true;
            this.rb_Print.CheckedChanged += new System.EventHandler(this.rb_Print_CheckedChanged);
            // 
            // rb_Scan
            // 
            this.rb_Scan.AutoSize = true;
            this.rb_Scan.Checked = true;
            this.rb_Scan.Location = new System.Drawing.Point(17, 26);
            this.rb_Scan.Name = "rb_Scan";
            this.rb_Scan.Size = new System.Drawing.Size(50, 19);
            this.rb_Scan.TabIndex = 0;
            this.rb_Scan.TabStop = true;
            this.rb_Scan.Text = "Scan";
            this.rb_Scan.UseVisualStyleBackColor = true;
            this.rb_Scan.CheckedChanged += new System.EventHandler(this.rb_Scan_CheckedChanged);
            // 
            // lb_FilePath
            // 
            this.lb_FilePath.AutoSize = true;
            this.lb_FilePath.Location = new System.Drawing.Point(148, 28);
            this.lb_FilePath.Name = "lb_FilePath";
            this.lb_FilePath.Size = new System.Drawing.Size(58, 15);
            this.lb_FilePath.TabIndex = 4;
            this.lb_FilePath.Text = "File path :";
            this.lb_FilePath.Visible = false;
            // 
            // lb_FolderPath
            // 
            this.lb_FolderPath.AutoSize = true;
            this.lb_FolderPath.Location = new System.Drawing.Point(148, 28);
            this.lb_FolderPath.Name = "lb_FolderPath";
            this.lb_FolderPath.Size = new System.Drawing.Size(73, 15);
            this.lb_FolderPath.TabIndex = 5;
            this.lb_FolderPath.Text = "Folder path :";
            // 
            // gb_PageCount
            // 
            this.gb_PageCount.Controls.Add(this.lb_PageCount);
            this.gb_PageCount.Controls.Add(this.nud_PageCount);
            this.gb_PageCount.Controls.Add(this.lb_PageCountDes);
            this.gb_PageCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_PageCount.Location = new System.Drawing.Point(145, 103);
            this.gb_PageCount.Name = "gb_PageCount";
            this.gb_PageCount.Size = new System.Drawing.Size(313, 101);
            this.gb_PageCount.TabIndex = 5;
            this.gb_PageCount.TabStop = false;
            this.gb_PageCount.Text = "Page Count";
            // 
            // lb_PageCount
            // 
            this.lb_PageCount.AutoSize = true;
            this.lb_PageCount.Location = new System.Drawing.Point(11, 69);
            this.lb_PageCount.Name = "lb_PageCount";
            this.lb_PageCount.Size = new System.Drawing.Size(105, 15);
            this.lb_PageCount.TabIndex = 1;
            this.lb_PageCount.Text = "* Page Count >= 1";
            // 
            // nud_PageCount
            // 
            this.nud_PageCount.Location = new System.Drawing.Point(176, 67);
            this.nud_PageCount.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nud_PageCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_PageCount.Name = "nud_PageCount";
            this.nud_PageCount.Size = new System.Drawing.Size(120, 23);
            this.nud_PageCount.TabIndex = 2;
            this.nud_PageCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lb_PageCountDes
            // 
            this.lb_PageCountDes.AutoSize = true;
            this.lb_PageCountDes.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_PageCountDes.Location = new System.Drawing.Point(7, 19);
            this.lb_PageCountDes.Name = "lb_PageCountDes";
            this.lb_PageCountDes.Size = new System.Drawing.Size(300, 45);
            this.lb_PageCountDes.TabIndex = 6;
            this.lb_PageCountDes.Text = "Select the number of pages for the scanned document. \r\nThe page count will be the" +
    " same for all devices, \r\nwhether physical or virtual.";
            // 
            // lb_FilePathDesc
            // 
            this.lb_FilePathDesc.AutoSize = true;
            this.lb_FilePathDesc.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_FilePathDesc.Location = new System.Drawing.Point(152, 52);
            this.lb_FilePathDesc.Name = "lb_FilePathDesc";
            this.lb_FilePathDesc.Size = new System.Drawing.Size(283, 45);
            this.lb_FilePathDesc.TabIndex = 7;
            this.lb_FilePathDesc.Text = "* Enter print file path include file name. (Delimiter: /)\r\n File name must includ" +
    "e file extension.\r\n Each path must have a unique value.";
            this.lb_FilePathDesc.Visible = false;
            // 
            // lb_FolderPathDesc
            // 
            this.lb_FolderPathDesc.AutoSize = true;
            this.lb_FolderPathDesc.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lb_FolderPathDesc.Location = new System.Drawing.Point(152, 52);
            this.lb_FolderPathDesc.Name = "lb_FolderPathDesc";
            this.lb_FolderPathDesc.Size = new System.Drawing.Size(299, 45);
            this.lb_FolderPathDesc.TabIndex = 6;
            this.lb_FolderPathDesc.Text = "* Enter scan folder path without file name. (Delimiter: /)\r\n Scan file name use d" +
    "efault name for monitoring.\r\n Each path must have a unique value.";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(496, 248);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 50);
            this.lockTimeoutControl.TabIndex = 4;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(23, 322);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(720, 177);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // CloudConnectorConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CloudConnectorConfigurationControl";
            this.Size = new System.Drawing.Size(757, 500);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gb_PageCount.ResumeLayout(false);
            this.gb_PageCount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PageCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private HP.ScalableTest.Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private HP.ScalableTest.Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.ComboBox cb_AppName;
        private System.Windows.Forms.MaskedTextBox tb_PWD;
        private System.Windows.Forms.TextBox tb_ID;
        private System.Windows.Forms.Label lb_PWD;
        private System.Windows.Forms.Label lb_ID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb_Print;
        private System.Windows.Forms.RadioButton rb_Scan;
        private System.Windows.Forms.Button btn_Options;
        private System.Windows.Forms.Label lb_PageCount;
        private System.Windows.Forms.NumericUpDown nud_PageCount;
        private System.Windows.Forms.TextBox tb_FolderPath;
        private System.Windows.Forms.Label lb_FilePath;
        private System.Windows.Forms.TextBox tb_FilePath;
        private System.Windows.Forms.Label lb_FolderPath;
        private System.Windows.Forms.Label lb_PageCountDes;
        private System.Windows.Forms.GroupBox gb_PageCount;
        private System.Windows.Forms.Label lb_FilePathDesc;
        private System.Windows.Forms.Label lb_FolderPathDesc;
        private System.Windows.Forms.TextBox tb_SharePointSites;
        private System.Windows.Forms.Label lb_SharePointSites;
        private System.Windows.Forms.Label lb_SharePointSites_Desc;
        private System.Windows.Forms.RadioButton rb_MultipleFile;
        private System.Windows.Forms.Label lb_MP_FolderPath;
        private System.Windows.Forms.TextBox tb_MP_FolderPath;
        private System.Windows.Forms.Label lb_MP_FileNames;
        private System.Windows.Forms.TextBox tb_MP_FileNames;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ListBox lbx_FileNames;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Label lb_SelectedFiles;
        private System.Windows.Forms.Label lb_MPDesc;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.ComboBox comboBox_Logout;
        private System.Windows.Forms.ComboBox comboBox_SIO;
        private System.Windows.Forms.Label label_SignOut;
        private System.Windows.Forms.Label label_SIO;
    }
}

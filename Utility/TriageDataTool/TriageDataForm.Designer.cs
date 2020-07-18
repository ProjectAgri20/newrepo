namespace HP.STF.TriageDataTool
{
    partial class TriageDataForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriageDataForm));
            this.scOne = new System.Windows.Forms.SplitContainer();
            this.gbTriageEvents = new System.Windows.Forms.GroupBox();
            this.dgvTriageEvents = new System.Windows.Forms.DataGridView();
            this.gbDeviceId = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbIpAddress = new System.Windows.Forms.TextBox();
            this.tbModelNumber = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbFirmwareDatecode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbFirmwareRevision = new System.Windows.Forms.TextBox();
            this.tbProductName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbDeviceName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbDeviceId = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbSessionId = new System.Windows.Forms.GroupBox();
            this.tbStfVersion = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbSessionId = new System.Windows.Forms.ComboBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbDbServer = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDbName = new System.Windows.Forms.TextBox();
            this.tbDbServer = new System.Windows.Forms.TextBox();
            this.cbDbEnvironment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scTwo = new System.Windows.Forms.SplitContainer();
            this.gbPerformanceEvents = new System.Windows.Forms.GroupBox();
            this.dgvPerformanceEvents = new System.Windows.Forms.DataGridView();
            this.btnExit = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpErrorInformation = new System.Windows.Forms.TabPage();
            this.rtbErrorInformation = new System.Windows.Forms.RichTextBox();
            this.cmsErrorInformation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyErrorInformationText = new System.Windows.Forms.ToolStripMenuItem();
            this.tpControlPanelImage = new System.Windows.Forms.TabPage();
            this.pbControlPanelImage = new System.Windows.Forms.PictureBox();
            this.cmsControlPanelImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyControlPanelImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tbAndroidLog = new System.Windows.Forms.TabPage();
            this.rtbAndroidLog = new System.Windows.Forms.RichTextBox();
            this.cmsAndroidLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyAndroidLogText = new System.Windows.Forms.ToolStripMenuItem();
            this.bsTriageEvents = new System.Windows.Forms.BindingSource(this.components);
            this.bsPerformanceEvents = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scOne)).BeginInit();
            this.scOne.Panel1.SuspendLayout();
            this.scOne.Panel2.SuspendLayout();
            this.scOne.SuspendLayout();
            this.gbTriageEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTriageEvents)).BeginInit();
            this.gbDeviceId.SuspendLayout();
            this.gbSessionId.SuspendLayout();
            this.gbDbServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTwo)).BeginInit();
            this.scTwo.Panel1.SuspendLayout();
            this.scTwo.Panel2.SuspendLayout();
            this.scTwo.SuspendLayout();
            this.gbPerformanceEvents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerformanceEvents)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpErrorInformation.SuspendLayout();
            this.cmsErrorInformation.SuspendLayout();
            this.tpControlPanelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbControlPanelImage)).BeginInit();
            this.cmsControlPanelImage.SuspendLayout();
            this.tbAndroidLog.SuspendLayout();
            this.cmsAndroidLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsTriageEvents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPerformanceEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // scOne
            // 
            this.scOne.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scOne.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scOne.Location = new System.Drawing.Point(0, 0);
            this.scOne.Name = "scOne";
            this.scOne.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scOne.Panel1
            // 
            this.scOne.Panel1.Controls.Add(this.gbTriageEvents);
            this.scOne.Panel1.Controls.Add(this.gbDeviceId);
            this.scOne.Panel1.Controls.Add(this.gbSessionId);
            this.scOne.Panel1.Controls.Add(this.gbDbServer);
            // 
            // scOne.Panel2
            // 
            this.scOne.Panel2.Controls.Add(this.scTwo);
            this.scOne.Size = new System.Drawing.Size(1167, 961);
            this.scOne.SplitterDistance = 382;
            this.scOne.TabIndex = 0;
            // 
            // gbTriageEvents
            // 
            this.gbTriageEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTriageEvents.Controls.Add(this.dgvTriageEvents);
            this.gbTriageEvents.Location = new System.Drawing.Point(10, 141);
            this.gbTriageEvents.Name = "gbTriageEvents";
            this.gbTriageEvents.Size = new System.Drawing.Size(1143, 234);
            this.gbTriageEvents.TabIndex = 3;
            this.gbTriageEvents.TabStop = false;
            this.gbTriageEvents.Text = "4. Select Triage Event";
            // 
            // dgvTriageEvents
            // 
            this.dgvTriageEvents.AllowUserToAddRows = false;
            this.dgvTriageEvents.AllowUserToDeleteRows = false;
            this.dgvTriageEvents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTriageEvents.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTriageEvents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTriageEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTriageEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTriageEvents.EnableHeadersVisualStyles = false;
            this.dgvTriageEvents.Location = new System.Drawing.Point(3, 16);
            this.dgvTriageEvents.Name = "dgvTriageEvents";
            this.dgvTriageEvents.ReadOnly = true;
            this.dgvTriageEvents.RowHeadersWidth = 10;
            this.dgvTriageEvents.Size = new System.Drawing.Size(1137, 215);
            this.dgvTriageEvents.TabIndex = 0;
            this.dgvTriageEvents.DataSourceChanged += new System.EventHandler(this.DgvTriageEvents_DataSourceChanged);
            this.dgvTriageEvents.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvTriageEvents_CellEnter);
            // 
            // gbDeviceId
            // 
            this.gbDeviceId.Controls.Add(this.label14);
            this.gbDeviceId.Controls.Add(this.tbIpAddress);
            this.gbDeviceId.Controls.Add(this.tbModelNumber);
            this.gbDeviceId.Controls.Add(this.label12);
            this.gbDeviceId.Controls.Add(this.tbFirmwareDatecode);
            this.gbDeviceId.Controls.Add(this.label11);
            this.gbDeviceId.Controls.Add(this.tbFirmwareRevision);
            this.gbDeviceId.Controls.Add(this.tbProductName);
            this.gbDeviceId.Controls.Add(this.label10);
            this.gbDeviceId.Controls.Add(this.tbDeviceName);
            this.gbDeviceId.Controls.Add(this.label9);
            this.gbDeviceId.Controls.Add(this.cbDeviceId);
            this.gbDeviceId.Controls.Add(this.label8);
            this.gbDeviceId.Controls.Add(this.label7);
            this.gbDeviceId.Location = new System.Drawing.Point(589, 10);
            this.gbDeviceId.Name = "gbDeviceId";
            this.gbDeviceId.Size = new System.Drawing.Size(564, 125);
            this.gbDeviceId.TabIndex = 2;
            this.gbDeviceId.TabStop = false;
            this.gbDeviceId.Text = "3. Select Device ID";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(330, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 13);
            this.label14.TabIndex = 13;
            this.label14.Text = "IP Address:";
            // 
            // tbIpAddress
            // 
            this.tbIpAddress.Location = new System.Drawing.Point(408, 19);
            this.tbIpAddress.Name = "tbIpAddress";
            this.tbIpAddress.ReadOnly = true;
            this.tbIpAddress.Size = new System.Drawing.Size(150, 20);
            this.tbIpAddress.TabIndex = 12;
            // 
            // tbModelNumber
            // 
            this.tbModelNumber.Location = new System.Drawing.Point(408, 73);
            this.tbModelNumber.Name = "tbModelNumber";
            this.tbModelNumber.ReadOnly = true;
            this.tbModelNumber.Size = new System.Drawing.Size(150, 20);
            this.tbModelNumber.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(310, 76);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Model Number:";
            // 
            // tbFirmwareDatecode
            // 
            this.tbFirmwareDatecode.Location = new System.Drawing.Point(408, 99);
            this.tbFirmwareDatecode.Name = "tbFirmwareDatecode";
            this.tbFirmwareDatecode.ReadOnly = true;
            this.tbFirmwareDatecode.Size = new System.Drawing.Size(150, 20);
            this.tbFirmwareDatecode.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(282, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Firmware Datecode:";
            // 
            // tbFirmwareRevision
            // 
            this.tbFirmwareRevision.Location = new System.Drawing.Point(126, 99);
            this.tbFirmwareRevision.Name = "tbFirmwareRevision";
            this.tbFirmwareRevision.ReadOnly = true;
            this.tbFirmwareRevision.Size = new System.Drawing.Size(150, 20);
            this.tbFirmwareRevision.TabIndex = 7;
            // 
            // tbProductName
            // 
            this.tbProductName.Location = new System.Drawing.Point(126, 73);
            this.tbProductName.Name = "tbProductName";
            this.tbProductName.ReadOnly = true;
            this.tbProductName.Size = new System.Drawing.Size(150, 20);
            this.tbProductName.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(29, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Product Name:";
            // 
            // tbDeviceName
            // 
            this.tbDeviceName.Location = new System.Drawing.Point(126, 46);
            this.tbDeviceName.Name = "tbDeviceName";
            this.tbDeviceName.ReadOnly = true;
            this.tbDeviceName.Size = new System.Drawing.Size(432, 20);
            this.tbDeviceName.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(33, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Device Name:";
            // 
            // cbDeviceId
            // 
            this.cbDeviceId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeviceId.FormattingEnabled = true;
            this.cbDeviceId.Location = new System.Drawing.Point(126, 19);
            this.cbDeviceId.Name = "cbDeviceId";
            this.cbDeviceId.Size = new System.Drawing.Size(150, 21);
            this.cbDeviceId.TabIndex = 2;
            this.cbDeviceId.SelectedIndexChanged += new System.EventHandler(this.CbDeviceId_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(52, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Device ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Firmware Revision:";
            // 
            // gbSessionId
            // 
            this.gbSessionId.Controls.Add(this.tbStfVersion);
            this.gbSessionId.Controls.Add(this.label13);
            this.gbSessionId.Controls.Add(this.cbSessionId);
            this.gbSessionId.Controls.Add(this.dtpEndDate);
            this.gbSessionId.Controls.Add(this.dtpStartDate);
            this.gbSessionId.Controls.Add(this.label6);
            this.gbSessionId.Controls.Add(this.label5);
            this.gbSessionId.Controls.Add(this.label4);
            this.gbSessionId.Location = new System.Drawing.Point(373, 10);
            this.gbSessionId.Name = "gbSessionId";
            this.gbSessionId.Size = new System.Drawing.Size(210, 125);
            this.gbSessionId.TabIndex = 1;
            this.gbSessionId.TabStop = false;
            this.gbSessionId.Text = "2. Select Session ID";
            // 
            // tbStfVersion
            // 
            this.tbStfVersion.Location = new System.Drawing.Point(84, 99);
            this.tbStfVersion.Name = "tbStfVersion";
            this.tbStfVersion.ReadOnly = true;
            this.tbStfVersion.Size = new System.Drawing.Size(120, 20);
            this.tbStfVersion.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(15, 102);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "STF Vers:";
            // 
            // cbSessionId
            // 
            this.cbSessionId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSessionId.FormattingEnabled = true;
            this.cbSessionId.Location = new System.Drawing.Point(83, 72);
            this.cbSessionId.Name = "cbSessionId";
            this.cbSessionId.Size = new System.Drawing.Size(121, 21);
            this.cbSessionId.TabIndex = 5;
            this.cbSessionId.SelectedIndexChanged += new System.EventHandler(this.CbSessionId_SelectedIndexChanged);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(84, 46);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(120, 20);
            this.dtpEndDate.TabIndex = 4;
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.DtpEndDate_ValueChanged);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(84, 20);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(120, 20);
            this.dtpStartDate.TabIndex = 3;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.DtpStartDate_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Start Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "End Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Session ID:";
            // 
            // gbDbServer
            // 
            this.gbDbServer.Controls.Add(this.label3);
            this.gbDbServer.Controls.Add(this.label2);
            this.gbDbServer.Controls.Add(this.tbDbName);
            this.gbDbServer.Controls.Add(this.tbDbServer);
            this.gbDbServer.Controls.Add(this.cbDbEnvironment);
            this.gbDbServer.Controls.Add(this.label1);
            this.gbDbServer.Location = new System.Drawing.Point(10, 10);
            this.gbDbServer.Name = "gbDbServer";
            this.gbDbServer.Size = new System.Drawing.Size(357, 125);
            this.gbDbServer.TabIndex = 0;
            this.gbDbServer.TabStop = false;
            this.gbDbServer.Text = "1. Select Database Server";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Database Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Database Server:";
            // 
            // tbDbName
            // 
            this.tbDbName.Location = new System.Drawing.Point(151, 73);
            this.tbDbName.Name = "tbDbName";
            this.tbDbName.ReadOnly = true;
            this.tbDbName.Size = new System.Drawing.Size(200, 20);
            this.tbDbName.TabIndex = 3;
            // 
            // tbDbServer
            // 
            this.tbDbServer.Location = new System.Drawing.Point(151, 46);
            this.tbDbServer.Name = "tbDbServer";
            this.tbDbServer.ReadOnly = true;
            this.tbDbServer.Size = new System.Drawing.Size(200, 20);
            this.tbDbServer.TabIndex = 2;
            // 
            // cbDbEnvironment
            // 
            this.cbDbEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDbEnvironment.FormattingEnabled = true;
            this.cbDbEnvironment.Location = new System.Drawing.Point(151, 19);
            this.cbDbEnvironment.Name = "cbDbEnvironment";
            this.cbDbEnvironment.Size = new System.Drawing.Size(200, 21);
            this.cbDbEnvironment.TabIndex = 1;
            this.cbDbEnvironment.SelectedIndexChanged += new System.EventHandler(this.CbDbEnvironment_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database Environment:";
            // 
            // scTwo
            // 
            this.scTwo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scTwo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTwo.Location = new System.Drawing.Point(0, 0);
            this.scTwo.Name = "scTwo";
            // 
            // scTwo.Panel1
            // 
            this.scTwo.Panel1.Controls.Add(this.gbPerformanceEvents);
            // 
            // scTwo.Panel2
            // 
            this.scTwo.Panel2.Controls.Add(this.btnExit);
            this.scTwo.Panel2.Controls.Add(this.tabControl1);
            this.scTwo.Size = new System.Drawing.Size(1167, 575);
            this.scTwo.SplitterDistance = 389;
            this.scTwo.TabIndex = 0;
            // 
            // gbPerformanceEvents
            // 
            this.gbPerformanceEvents.Controls.Add(this.dgvPerformanceEvents);
            this.gbPerformanceEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPerformanceEvents.Location = new System.Drawing.Point(0, 0);
            this.gbPerformanceEvents.Name = "gbPerformanceEvents";
            this.gbPerformanceEvents.Size = new System.Drawing.Size(385, 571);
            this.gbPerformanceEvents.TabIndex = 0;
            this.gbPerformanceEvents.TabStop = false;
            this.gbPerformanceEvents.Text = "Performance Events";
            // 
            // dgvPerformanceEvents
            // 
            this.dgvPerformanceEvents.AllowUserToAddRows = false;
            this.dgvPerformanceEvents.AllowUserToDeleteRows = false;
            this.dgvPerformanceEvents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPerformanceEvents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPerformanceEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPerformanceEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPerformanceEvents.EnableHeadersVisualStyles = false;
            this.dgvPerformanceEvents.Location = new System.Drawing.Point(3, 16);
            this.dgvPerformanceEvents.Name = "dgvPerformanceEvents";
            this.dgvPerformanceEvents.ReadOnly = true;
            this.dgvPerformanceEvents.RowHeadersWidth = 10;
            this.dgvPerformanceEvents.Size = new System.Drawing.Size(379, 552);
            this.dgvPerformanceEvents.TabIndex = 0;
            this.dgvPerformanceEvents.DataSourceChanged += new System.EventHandler(this.DgvPerformanceEvents_DataSourceChanged);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(685, 538);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpErrorInformation);
            this.tabControl1.Controls.Add(this.tpControlPanelImage);
            this.tabControl1.Controls.Add(this.tbAndroidLog);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(757, 529);
            this.tabControl1.TabIndex = 0;
            // 
            // tpErrorInformation
            // 
            this.tpErrorInformation.Controls.Add(this.rtbErrorInformation);
            this.tpErrorInformation.Location = new System.Drawing.Point(4, 22);
            this.tpErrorInformation.Name = "tpErrorInformation";
            this.tpErrorInformation.Padding = new System.Windows.Forms.Padding(3);
            this.tpErrorInformation.Size = new System.Drawing.Size(749, 503);
            this.tpErrorInformation.TabIndex = 0;
            this.tpErrorInformation.Text = "Error Information";
            this.tpErrorInformation.UseVisualStyleBackColor = true;
            // 
            // rtbErrorInformation
            // 
            this.rtbErrorInformation.ContextMenuStrip = this.cmsErrorInformation;
            this.rtbErrorInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbErrorInformation.Location = new System.Drawing.Point(3, 3);
            this.rtbErrorInformation.Name = "rtbErrorInformation";
            this.rtbErrorInformation.ReadOnly = true;
            this.rtbErrorInformation.Size = new System.Drawing.Size(743, 497);
            this.rtbErrorInformation.TabIndex = 0;
            this.rtbErrorInformation.Text = "";
            // 
            // cmsErrorInformation
            // 
            this.cmsErrorInformation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyErrorInformationText});
            this.cmsErrorInformation.Name = "cmsErrorInformation";
            this.cmsErrorInformation.Size = new System.Drawing.Size(103, 26);
            // 
            // copyErrorInformationText
            // 
            this.copyErrorInformationText.Name = "copyErrorInformationText";
            this.copyErrorInformationText.Size = new System.Drawing.Size(102, 22);
            this.copyErrorInformationText.Text = "Copy";
            this.copyErrorInformationText.Click += new System.EventHandler(this.CopyErrorInformationText_Click);
            // 
            // tpControlPanelImage
            // 
            this.tpControlPanelImage.Controls.Add(this.pbControlPanelImage);
            this.tpControlPanelImage.Location = new System.Drawing.Point(4, 22);
            this.tpControlPanelImage.Name = "tpControlPanelImage";
            this.tpControlPanelImage.Padding = new System.Windows.Forms.Padding(3);
            this.tpControlPanelImage.Size = new System.Drawing.Size(749, 503);
            this.tpControlPanelImage.TabIndex = 1;
            this.tpControlPanelImage.Text = "Control Panel Image";
            this.tpControlPanelImage.UseVisualStyleBackColor = true;
            // 
            // pbControlPanelImage
            // 
            this.pbControlPanelImage.ContextMenuStrip = this.cmsControlPanelImage;
            this.pbControlPanelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbControlPanelImage.Location = new System.Drawing.Point(3, 3);
            this.pbControlPanelImage.Name = "pbControlPanelImage";
            this.pbControlPanelImage.Size = new System.Drawing.Size(743, 497);
            this.pbControlPanelImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbControlPanelImage.TabIndex = 0;
            this.pbControlPanelImage.TabStop = false;
            // 
            // cmsControlPanelImage
            // 
            this.cmsControlPanelImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyControlPanelImage});
            this.cmsControlPanelImage.Name = "cmsControlPanelImage";
            this.cmsControlPanelImage.Size = new System.Drawing.Size(103, 26);
            // 
            // copyControlPanelImage
            // 
            this.copyControlPanelImage.Name = "copyControlPanelImage";
            this.copyControlPanelImage.Size = new System.Drawing.Size(102, 22);
            this.copyControlPanelImage.Text = "Copy";
            this.copyControlPanelImage.Click += new System.EventHandler(this.CopyControlPanelImage_Click);
            // 
            // tbAndroidLog
            // 
            this.tbAndroidLog.Controls.Add(this.rtbAndroidLog);
            this.tbAndroidLog.Location = new System.Drawing.Point(4, 22);
            this.tbAndroidLog.Name = "tbAndroidLog";
            this.tbAndroidLog.Size = new System.Drawing.Size(749, 503);
            this.tbAndroidLog.TabIndex = 2;
            this.tbAndroidLog.Text = "Android Log";
            this.tbAndroidLog.UseVisualStyleBackColor = true;
            // 
            // rtbAndroidLog
            // 
            this.rtbAndroidLog.ContextMenuStrip = this.cmsAndroidLog;
            this.rtbAndroidLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbAndroidLog.Location = new System.Drawing.Point(0, 0);
            this.rtbAndroidLog.Name = "rtbAndroidLog";
            this.rtbAndroidLog.ReadOnly = true;
            this.rtbAndroidLog.Size = new System.Drawing.Size(749, 503);
            this.rtbAndroidLog.TabIndex = 0;
            this.rtbAndroidLog.Text = "";
            // 
            // cmsAndroidLog
            // 
            this.cmsAndroidLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyAndroidLogText});
            this.cmsAndroidLog.Name = "cmsAndroidLog";
            this.cmsAndroidLog.Size = new System.Drawing.Size(103, 26);
            // 
            // copyAndroidLogText
            // 
            this.copyAndroidLogText.Name = "copyAndroidLogText";
            this.copyAndroidLogText.Size = new System.Drawing.Size(102, 22);
            this.copyAndroidLogText.Text = "Copy";
            this.copyAndroidLogText.Click += new System.EventHandler(this.CopyAndroidLogText_Click);
            // 
            // TriageDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 961);
            this.Controls.Add(this.scOne);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1183, 1000);
            this.Name = "TriageDataForm";
            this.Text = "Triage Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TriageDataForm_FormClosing);
            this.Load += new System.EventHandler(this.TriageDataForm_Load);
            this.scOne.Panel1.ResumeLayout(false);
            this.scOne.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scOne)).EndInit();
            this.scOne.ResumeLayout(false);
            this.gbTriageEvents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTriageEvents)).EndInit();
            this.gbDeviceId.ResumeLayout(false);
            this.gbDeviceId.PerformLayout();
            this.gbSessionId.ResumeLayout(false);
            this.gbSessionId.PerformLayout();
            this.gbDbServer.ResumeLayout(false);
            this.gbDbServer.PerformLayout();
            this.scTwo.Panel1.ResumeLayout(false);
            this.scTwo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTwo)).EndInit();
            this.scTwo.ResumeLayout(false);
            this.gbPerformanceEvents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPerformanceEvents)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpErrorInformation.ResumeLayout(false);
            this.cmsErrorInformation.ResumeLayout(false);
            this.tpControlPanelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbControlPanelImage)).EndInit();
            this.cmsControlPanelImage.ResumeLayout(false);
            this.tbAndroidLog.ResumeLayout(false);
            this.cmsAndroidLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsTriageEvents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPerformanceEvents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scOne;
        private System.Windows.Forms.GroupBox gbDbServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDbName;
        private System.Windows.Forms.TextBox tbDbServer;
        private System.Windows.Forms.ComboBox cbDbEnvironment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbSessionId;
        private System.Windows.Forms.ComboBox cbSessionId;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbDeviceId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbDeviceId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbFirmwareRevision;
        private System.Windows.Forms.TextBox tbProductName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbDeviceName;
        private System.Windows.Forms.TextBox tbModelNumber;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbFirmwareDatecode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gbTriageEvents;
        private System.Windows.Forms.DataGridView dgvTriageEvents;
        private System.Windows.Forms.SplitContainer scTwo;
        private System.Windows.Forms.GroupBox gbPerformanceEvents;
        private System.Windows.Forms.DataGridView dgvPerformanceEvents;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpErrorInformation;
        private System.Windows.Forms.RichTextBox rtbErrorInformation;
        private System.Windows.Forms.TabPage tpControlPanelImage;
        private System.Windows.Forms.PictureBox pbControlPanelImage;
        private System.Windows.Forms.TabPage tbAndroidLog;
        private System.Windows.Forms.RichTextBox rtbAndroidLog;
        private System.Windows.Forms.BindingSource bsTriageEvents;
        private System.Windows.Forms.BindingSource bsPerformanceEvents;
        private System.Windows.Forms.ContextMenuStrip cmsErrorInformation;
        private System.Windows.Forms.ToolStripMenuItem copyErrorInformationText;
        private System.Windows.Forms.ContextMenuStrip cmsControlPanelImage;
        private System.Windows.Forms.ToolStripMenuItem copyControlPanelImage;
        private System.Windows.Forms.ContextMenuStrip cmsAndroidLog;
        private System.Windows.Forms.ToolStripMenuItem copyAndroidLogText;
        private System.Windows.Forms.TextBox tbStfVersion;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbIpAddress;
        private System.Windows.Forms.Label label14;
    }
}


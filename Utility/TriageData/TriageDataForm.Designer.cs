using HP.ScalableTestTriageData.Data.DataLog;

namespace HP.ScalableTestTriageData.Data
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn2 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewImageColumn gridViewImageColumn1 = new Telerik.WinControls.UI.GridViewImageColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewImageColumn gridViewImageColumn2 = new Telerik.WinControls.UI.GridViewImageColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn3 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn4 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriageDataForm));
            this.rgvTriageData = new Telerik.WinControls.UI.RadGridView();
            this.cboSessionIds = new System.Windows.Forms.ComboBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetData = new System.Windows.Forms.Button();
            this.rgvPerformanceMarkers = new Telerik.WinControls.UI.RadGridView();
            this.lblDeviceId = new System.Windows.Forms.Label();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.lblModelInfo = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblProduct = new System.Windows.Forms.Label();
            this.tabControlInfo = new System.Windows.Forms.TabControl();
            this.tabPageError = new System.Windows.Forms.TabPage();
            this.rtbErrorMessage = new System.Windows.Forms.RichTextBox();
            this.tabPageImage = new System.Windows.Forms.TabPage();
            this.pbControlPanel = new System.Windows.Forms.PictureBox();
            this.tabPageJAError = new System.Windows.Forms.TabPage();
            this.errMessageAndroid = new System.Windows.Forms.RichTextBox();
            this.btnSessionTriage = new System.Windows.Forms.Button();
            this.pbThumbnail = new System.Windows.Forms.PictureBox();
            this.lblFirmwareRevision = new System.Windows.Forms.Label();
            this.lblFirmwareDatecode = new System.Windows.Forms.Label();
            this.lbActivityType = new System.Windows.Forms.Label();
            this.lblActivityName = new System.Windows.Forms.Label();
            this.cboIpAddress = new System.Windows.Forms.ComboBox();
            this.cboProduct = new System.Windows.Forms.ComboBox();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtActivityExecutionId = new System.Windows.Forms.TextBox();
            this.btnFindAEid = new System.Windows.Forms.Button();
            this.lblCountErrors = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.cmsErrorInfo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUIDump = new System.Windows.Forms.Button();
            this.activityExecutionPerformanceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.triageDataListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.rgvTriageData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvTriageData.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPerformanceMarkers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPerformanceMarkers.MasterTemplate)).BeginInit();
            this.tabControlInfo.SuspendLayout();
            this.tabPageError.SuspendLayout();
            this.tabPageImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbControlPanel)).BeginInit();
            this.tabPageJAError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbnail)).BeginInit();
            this.cmsErrorInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activityExecutionPerformanceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triageDataListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvTriageData
            // 
            this.rgvTriageData.BackColor = System.Drawing.SystemColors.Control;
            this.rgvTriageData.Cursor = System.Windows.Forms.Cursors.Default;
            this.rgvTriageData.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rgvTriageData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rgvTriageData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rgvTriageData.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.rgvTriageData.MasterTemplate.AllowDragToGroup = false;
            gridViewTextBoxColumn1.DataType = typeof(System.Guid);
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "TriageDataId";
            gridViewTextBoxColumn1.HeaderText = "TriageDataId";
            gridViewTextBoxColumn1.IsAutoGenerated = true;
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.Name = "TriageDataId";
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "SessionId";
            gridViewTextBoxColumn2.HeaderText = "SessionId";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.IsVisible = false;
            gridViewTextBoxColumn2.Name = "SessionId";
            gridViewTextBoxColumn3.DataType = typeof(System.Guid);
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "ActivityExecutionId";
            gridViewTextBoxColumn3.HeaderText = "ActivityExecutionId";
            gridViewTextBoxColumn3.IsAutoGenerated = true;
            gridViewTextBoxColumn3.IsVisible = false;
            gridViewTextBoxColumn3.Name = "ActivityExecutionId";
            gridViewDateTimeColumn1.AllowGroup = false;
            gridViewDateTimeColumn1.EnableExpressionEditor = false;
            gridViewDateTimeColumn1.FieldName = "TriageDateTime";
            gridViewDateTimeColumn1.HeaderText = "TriageDateTime";
            gridViewDateTimeColumn1.IsAutoGenerated = true;
            gridViewDateTimeColumn1.IsVisible = false;
            gridViewDateTimeColumn1.Name = "TriageDateTime";
            gridViewDateTimeColumn1.ReadOnly = true;
            gridViewDateTimeColumn1.Width = 114;
            gridViewDateTimeColumn2.EnableExpressionEditor = false;
            gridViewDateTimeColumn2.FieldName = "LocalTriageDateTime";
            gridViewDateTimeColumn2.HeaderText = "TriageDateTime";
            gridViewDateTimeColumn2.IsAutoGenerated = true;
            gridViewDateTimeColumn2.Name = "LocalTriageDateTime";
            gridViewDateTimeColumn2.ReadOnly = true;
            gridViewDateTimeColumn2.Width = 133;
            gridViewTextBoxColumn4.AllowGroup = false;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "Reason";
            gridViewTextBoxColumn4.HeaderText = "Exception Message";
            gridViewTextBoxColumn4.IsAutoGenerated = true;
            gridViewTextBoxColumn4.MaxLength = 20;
            gridViewTextBoxColumn4.Name = "Reason";
            gridViewTextBoxColumn4.ReadOnly = true;
            gridViewTextBoxColumn4.Width = 325;
            gridViewImageColumn1.DataType = typeof(byte[]);
            gridViewImageColumn1.EnableExpressionEditor = false;
            gridViewImageColumn1.FieldName = "ControlPanelImage";
            gridViewImageColumn1.HeaderText = "ControlPanelImage";
            gridViewImageColumn1.IsAutoGenerated = true;
            gridViewImageColumn1.IsVisible = false;
            gridViewImageColumn1.Name = "ControlPanelImage";
            gridViewTextBoxColumn5.AllowGroup = false;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "DeviceWarnings";
            gridViewTextBoxColumn5.HeaderText = "DeviceWarnings";
            gridViewTextBoxColumn5.IsAutoGenerated = true;
            gridViewTextBoxColumn5.Name = "DeviceWarnings";
            gridViewTextBoxColumn5.ReadOnly = true;
            gridViewTextBoxColumn5.Width = 200;
            gridViewImageColumn2.DataType = typeof(byte[]);
            gridViewImageColumn2.EnableExpressionEditor = false;
            gridViewImageColumn2.FieldName = "Thumbnail";
            gridViewImageColumn2.HeaderText = "Thumb Nail";
            gridViewImageColumn2.IsAutoGenerated = true;
            gridViewImageColumn2.Name = "Thumbnail";
            gridViewImageColumn2.Width = 160;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.FieldName = "ControlIds";
            gridViewTextBoxColumn6.HeaderText = "ControlIds";
            gridViewTextBoxColumn6.IsAutoGenerated = true;
            gridViewTextBoxColumn6.IsVisible = false;
            gridViewTextBoxColumn6.Name = "ControlIds";
            gridViewTextBoxColumn6.VisibleInColumnChooser = false;
            gridViewTextBoxColumn6.Width = 173;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.FieldName = "UIDumpData";
            gridViewTextBoxColumn7.HeaderText = "UIDumpData";
            gridViewTextBoxColumn7.IsAutoGenerated = true;
            gridViewTextBoxColumn7.Name = "UIDumpData";
            this.rgvTriageData.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewDateTimeColumn1,
            gridViewDateTimeColumn2,
            gridViewTextBoxColumn4,
            gridViewImageColumn1,
            gridViewTextBoxColumn5,
            gridViewImageColumn2,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7});
            this.rgvTriageData.MasterTemplate.DataSource = this.triageDataListBindingSource;
            this.rgvTriageData.MasterTemplate.ShowRowHeaderColumn = false;
            this.rgvTriageData.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvTriageData.Name = "rgvTriageData";
            this.rgvTriageData.ReadOnly = true;
            this.rgvTriageData.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.rgvTriageData.RootElement.ControlBounds = new System.Drawing.Rectangle(12, 12, 821, 278);
            this.rgvTriageData.Size = new System.Drawing.Size(821, 278);
            this.rgvTriageData.TabIndex = 0;
            this.rgvTriageData.Text = "radGridView1";
            this.rgvTriageData.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.rgvTriageData_CellClick);
            this.rgvTriageData.DataBindingComplete += new Telerik.WinControls.UI.GridViewBindingCompleteEventHandler(this.rgvTriageData_DataBindingComplete);
            this.rgvTriageData.Paint += new System.Windows.Forms.PaintEventHandler(this.rgvTriageData_Paint);
            // 
            // cboSessionIds
            // 
            this.cboSessionIds.AllowDrop = true;
            this.cboSessionIds.FormattingEnabled = true;
            this.cboSessionIds.Location = new System.Drawing.Point(925, 160);
            this.cboSessionIds.Name = "cboSessionIds";
            this.cboSessionIds.Size = new System.Drawing.Size(121, 21);
            this.cboSessionIds.TabIndex = 1;
            this.cboSessionIds.SelectedIndexChanged += new System.EventHandler(this.cboSessionIds_SelectedIndexChanged);
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(925, 193);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(153, 20);
            this.dtpStart.TabIndex = 2;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(925, 220);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(153, 20);
            this.dtpEnd.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(850, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Start Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(850, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "End Date:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(838, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Session ID\'s:";
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(848, 250);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(138, 40);
            this.btnGetData.TabIndex = 7;
            this.btnGetData.Text = "Triage Sessions";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // rgvPerformanceMarkers
            // 
            this.rgvPerformanceMarkers.BackColor = System.Drawing.SystemColors.Control;
            this.rgvPerformanceMarkers.Cursor = System.Windows.Forms.Cursors.Default;
            this.rgvPerformanceMarkers.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rgvPerformanceMarkers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rgvPerformanceMarkers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rgvPerformanceMarkers.Location = new System.Drawing.Point(12, 296);
            // 
            // 
            // 
            this.rgvPerformanceMarkers.MasterTemplate.AllowDeleteRow = false;
            this.rgvPerformanceMarkers.MasterTemplate.AllowEditRow = false;
            this.rgvPerformanceMarkers.MasterTemplate.AllowRowResize = false;
            gridViewTextBoxColumn8.DataType = typeof(System.Guid);
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.FieldName = "ActivityExecutionPerformanceId";
            gridViewTextBoxColumn8.HeaderText = "ActivityExecutionPerformanceId";
            gridViewTextBoxColumn8.IsAutoGenerated = true;
            gridViewTextBoxColumn8.IsVisible = false;
            gridViewTextBoxColumn8.Name = "ActivityExecutionPerformanceId";
            gridViewTextBoxColumn9.EnableExpressionEditor = false;
            gridViewTextBoxColumn9.FieldName = "SessionId";
            gridViewTextBoxColumn9.HeaderText = "SessionId";
            gridViewTextBoxColumn9.IsAutoGenerated = true;
            gridViewTextBoxColumn9.IsVisible = false;
            gridViewTextBoxColumn9.Name = "SessionId";
            gridViewTextBoxColumn10.DataType = typeof(System.Nullable<System.Guid>);
            gridViewTextBoxColumn10.EnableExpressionEditor = false;
            gridViewTextBoxColumn10.FieldName = "ActivityExecutionId";
            gridViewTextBoxColumn10.HeaderText = "ActivityExecutionId";
            gridViewTextBoxColumn10.IsAutoGenerated = true;
            gridViewTextBoxColumn10.IsVisible = false;
            gridViewTextBoxColumn10.Name = "ActivityExecutionId";
            gridViewDateTimeColumn3.DataType = typeof(System.Nullable<System.DateTime>);
            gridViewDateTimeColumn3.EnableExpressionEditor = false;
            gridViewDateTimeColumn3.FieldName = "EventDateTime";
            gridViewDateTimeColumn3.HeaderText = "EventDateTime";
            gridViewDateTimeColumn3.IsAutoGenerated = true;
            gridViewDateTimeColumn3.IsVisible = false;
            gridViewDateTimeColumn3.Name = "EventDateTime";
            gridViewDateTimeColumn3.Width = 133;
            gridViewDateTimeColumn4.EnableExpressionEditor = false;
            gridViewDateTimeColumn4.FieldName = "LocalEventDateTime";
            gridViewDateTimeColumn4.HeaderText = "EventDateTime";
            gridViewDateTimeColumn4.IsAutoGenerated = true;
            gridViewDateTimeColumn4.Name = "LocalEventDateTime";
            gridViewDateTimeColumn4.ReadOnly = true;
            gridViewDateTimeColumn4.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewDateTimeColumn4.Width = 133;
            gridViewDecimalColumn1.DataType = typeof(System.Nullable<int>);
            gridViewDecimalColumn1.EnableExpressionEditor = false;
            gridViewDecimalColumn1.FieldName = "EventIndex";
            gridViewDecimalColumn1.HeaderText = "EventIndex";
            gridViewDecimalColumn1.IsAutoGenerated = true;
            gridViewDecimalColumn1.Name = "EventIndex";
            gridViewDecimalColumn1.Width = 72;
            gridViewTextBoxColumn11.EnableExpressionEditor = false;
            gridViewTextBoxColumn11.FieldName = "EventLabel";
            gridViewTextBoxColumn11.HeaderText = "EventLabel";
            gridViewTextBoxColumn11.IsAutoGenerated = true;
            gridViewTextBoxColumn11.Name = "EventLabel";
            gridViewTextBoxColumn11.Width = 141;
            gridViewTextBoxColumn12.DataType = typeof(HP.ScalableTestTriageData.Data.DataLog.SessionSummary);
            gridViewTextBoxColumn12.EnableExpressionEditor = false;
            gridViewTextBoxColumn12.FieldName = "SessionSummary";
            gridViewTextBoxColumn12.HeaderText = "SessionSummary";
            gridViewTextBoxColumn12.IsAutoGenerated = true;
            gridViewTextBoxColumn12.IsVisible = false;
            gridViewTextBoxColumn12.Name = "SessionSummary";
            gridViewTextBoxColumn12.Width = 109;
            this.rgvPerformanceMarkers.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewDateTimeColumn3,
            gridViewDateTimeColumn4,
            gridViewDecimalColumn1,
            gridViewTextBoxColumn11,
            gridViewTextBoxColumn12});
            this.rgvPerformanceMarkers.MasterTemplate.DataSource = this.activityExecutionPerformanceBindingSource;
            this.rgvPerformanceMarkers.MasterTemplate.ShowRowHeaderColumn = false;
            sortDescriptor1.PropertyName = "LocalEventDateTime";
            this.rgvPerformanceMarkers.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.rgvPerformanceMarkers.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.rgvPerformanceMarkers.Name = "rgvPerformanceMarkers";
            this.rgvPerformanceMarkers.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.rgvPerformanceMarkers.RootElement.ControlBounds = new System.Drawing.Rectangle(12, 296, 378, 675);
            this.rgvPerformanceMarkers.Size = new System.Drawing.Size(378, 675);
            this.rgvPerformanceMarkers.TabIndex = 8;
            this.rgvPerformanceMarkers.Text = "radGridView1";
            // 
            // lblDeviceId
            // 
            this.lblDeviceId.AutoSize = true;
            this.lblDeviceId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeviceId.Location = new System.Drawing.Point(842, 12);
            this.lblDeviceId.Name = "lblDeviceId";
            this.lblDeviceId.Size = new System.Drawing.Size(83, 16);
            this.lblDeviceId.TabIndex = 9;
            this.lblDeviceId.Text = "RDL-00001";
            // 
            // lblIPAddress
            // 
            this.lblIPAddress.AutoSize = true;
            this.lblIPAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIPAddress.Location = new System.Drawing.Point(842, 33);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(100, 16);
            this.lblIPAddress.TabIndex = 10;
            this.lblIPAddress.Text = "15.86.231.122";
            // 
            // lblModelInfo
            // 
            this.lblModelInfo.AutoSize = true;
            this.lblModelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelInfo.Location = new System.Drawing.Point(845, 80);
            this.lblModelInfo.Name = "lblModelInfo";
            this.lblModelInfo.Size = new System.Drawing.Size(39, 16);
            this.lblModelInfo.TabIndex = 11;
            this.lblModelInfo.Text = "MFP";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(1141, 931);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(138, 40);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduct.Location = new System.Drawing.Point(842, 56);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(68, 16);
            this.lblProduct.TabIndex = 15;
            this.lblProduct.Text = "Cordoba";
            // 
            // tabControlInfo
            // 
            this.tabControlInfo.Controls.Add(this.tabPageError);
            this.tabControlInfo.Controls.Add(this.tabPageImage);
            this.tabControlInfo.Controls.Add(this.tabPageJAError);
            this.tabControlInfo.Location = new System.Drawing.Point(407, 296);
            this.tabControlInfo.Name = "tabControlInfo";
            this.tabControlInfo.SelectedIndex = 0;
            this.tabControlInfo.Size = new System.Drawing.Size(872, 629);
            this.tabControlInfo.TabIndex = 16;
            // 
            // tabPageError
            // 
            this.tabPageError.Controls.Add(this.rtbErrorMessage);
            this.tabPageError.Location = new System.Drawing.Point(4, 22);
            this.tabPageError.Name = "tabPageError";
            this.tabPageError.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageError.Size = new System.Drawing.Size(864, 603);
            this.tabPageError.TabIndex = 0;
            this.tabPageError.Text = "Error Info";
            this.tabPageError.UseVisualStyleBackColor = true;
            // 
            // rtbErrorMessage
            // 
            this.rtbErrorMessage.Location = new System.Drawing.Point(20, 16);
            this.rtbErrorMessage.Name = "rtbErrorMessage";
            this.rtbErrorMessage.Size = new System.Drawing.Size(823, 581);
            this.rtbErrorMessage.TabIndex = 0;
            this.rtbErrorMessage.Text = "";
            this.rtbErrorMessage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtbErrorMessage_MouseDown);
            // 
            // tabPageImage
            // 
            this.tabPageImage.Controls.Add(this.pbControlPanel);
            this.tabPageImage.Location = new System.Drawing.Point(4, 22);
            this.tabPageImage.Name = "tabPageImage";
            this.tabPageImage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImage.Size = new System.Drawing.Size(864, 603);
            this.tabPageImage.TabIndex = 1;
            this.tabPageImage.Text = "Image";
            this.tabPageImage.UseVisualStyleBackColor = true;
            // 
            // pbControlPanel
            // 
            this.pbControlPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbControlPanel.Location = new System.Drawing.Point(6, 3);
            this.pbControlPanel.Name = "pbControlPanel";
            this.pbControlPanel.Size = new System.Drawing.Size(849, 598);
            this.pbControlPanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbControlPanel.TabIndex = 14;
            this.pbControlPanel.TabStop = false;
            // 
            // tabPageJAError
            // 
            this.tabPageJAError.Controls.Add(this.errMessageAndroid);
            this.tabPageJAError.Location = new System.Drawing.Point(4, 22);
            this.tabPageJAError.Name = "tabPageJAError";
            this.tabPageJAError.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJAError.Size = new System.Drawing.Size(864, 603);
            this.tabPageJAError.TabIndex = 2;
            this.tabPageJAError.Text = "Android Log";
            this.tabPageJAError.UseVisualStyleBackColor = true;
            // 
            // errMessageAndroid
            // 
            this.errMessageAndroid.Location = new System.Drawing.Point(21, 11);
            this.errMessageAndroid.Name = "errMessageAndroid";
            this.errMessageAndroid.Size = new System.Drawing.Size(823, 581);
            this.errMessageAndroid.TabIndex = 1;
            this.errMessageAndroid.Text = "";
            this.errMessageAndroid.MouseHover += new System.EventHandler(this.errMessageAndroid_MouseHover);
            // 
            // btnSessionTriage
            // 
            this.btnSessionTriage.Image = global::HP.ScalableTestTriageData.Data.Properties.Resources.CheckboxOn;
            this.btnSessionTriage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSessionTriage.Location = new System.Drawing.Point(1064, 154);
            this.btnSessionTriage.Name = "btnSessionTriage";
            this.btnSessionTriage.Size = new System.Drawing.Size(97, 23);
            this.btnSessionTriage.TabIndex = 17;
            this.btnSessionTriage.Text = "Get Data";
            this.btnSessionTriage.UseVisualStyleBackColor = true;
            this.btnSessionTriage.Click += new System.EventHandler(this.btnSessionTriage_Click);
            // 
            // pbThumbnail
            // 
            this.pbThumbnail.Location = new System.Drawing.Point(1179, 154);
            this.pbThumbnail.Name = "pbThumbnail";
            this.pbThumbnail.Size = new System.Drawing.Size(100, 100);
            this.pbThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbThumbnail.TabIndex = 12;
            this.pbThumbnail.TabStop = false;
            this.pbThumbnail.DoubleClick += new System.EventHandler(this.pbThumbnail_DoubleClick);
            // 
            // lblFirmwareRevision
            // 
            this.lblFirmwareRevision.AutoSize = true;
            this.lblFirmwareRevision.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirmwareRevision.Location = new System.Drawing.Point(926, 105);
            this.lblFirmwareRevision.Name = "lblFirmwareRevision";
            this.lblFirmwareRevision.Size = new System.Drawing.Size(120, 16);
            this.lblFirmwareRevision.TabIndex = 19;
            this.lblFirmwareRevision.Text = "2456317_627832";
            // 
            // lblFirmwareDatecode
            // 
            this.lblFirmwareDatecode.AutoSize = true;
            this.lblFirmwareDatecode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirmwareDatecode.Location = new System.Drawing.Point(838, 105);
            this.lblFirmwareDatecode.Name = "lblFirmwareDatecode";
            this.lblFirmwareDatecode.Size = new System.Drawing.Size(72, 16);
            this.lblFirmwareDatecode.TabIndex = 20;
            this.lblFirmwareDatecode.Text = "20160808";
            // 
            // lbActivityType
            // 
            this.lbActivityType.AutoSize = true;
            this.lbActivityType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbActivityType.Location = new System.Drawing.Point(1142, 130);
            this.lbActivityType.Name = "lbActivityType";
            this.lbActivityType.Size = new System.Drawing.Size(95, 16);
            this.lbActivityType.TabIndex = 21;
            this.lbActivityType.Text = "ScanToHpcr";
            // 
            // lblActivityName
            // 
            this.lblActivityName.AutoSize = true;
            this.lblActivityName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivityName.Location = new System.Drawing.Point(838, 130);
            this.lblActivityName.Name = "lblActivityName";
            this.lblActivityName.Size = new System.Drawing.Size(230, 16);
            this.lblActivityName.TabIndex = 22;
            this.lblActivityName.Text = "ScanToFolder_AllDevices_OCR";
            // 
            // cboIpAddress
            // 
            this.cboIpAddress.FormattingEnabled = true;
            this.cboIpAddress.Location = new System.Drawing.Point(951, 28);
            this.cboIpAddress.Name = "cboIpAddress";
            this.cboIpAddress.Size = new System.Drawing.Size(138, 21);
            this.cboIpAddress.TabIndex = 24;
            this.cboIpAddress.SelectedIndexChanged += new System.EventHandler(this.cboIpAddress_SelectedIndexChanged);
            // 
            // cboProduct
            // 
            this.cboProduct.FormattingEnabled = true;
            this.cboProduct.Location = new System.Drawing.Point(951, 55);
            this.cboProduct.Name = "cboProduct";
            this.cboProduct.Size = new System.Drawing.Size(138, 21);
            this.cboProduct.TabIndex = 25;
            this.cboProduct.SelectedIndexChanged += new System.EventHandler(this.cboProduct_SelectedIndexChanged);
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Location = new System.Drawing.Point(1114, 40);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(75, 23);
            this.btnRetrieve.TabIndex = 26;
            this.btnRetrieve.Text = "Retrieve";
            this.btnRetrieve.UseVisualStyleBackColor = true;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1214, 40);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(61, 23);
            this.btnClear.TabIndex = 27;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtActivityExecutionId
            // 
            this.txtActivityExecutionId.Location = new System.Drawing.Point(998, 261);
            this.txtActivityExecutionId.Name = "txtActivityExecutionId";
            this.txtActivityExecutionId.Size = new System.Drawing.Size(191, 20);
            this.txtActivityExecutionId.TabIndex = 28;
            // 
            // btnFindAEid
            // 
            this.btnFindAEid.Location = new System.Drawing.Point(1195, 261);
            this.btnFindAEid.Name = "btnFindAEid";
            this.btnFindAEid.Size = new System.Drawing.Size(75, 23);
            this.btnFindAEid.TabIndex = 29;
            this.btnFindAEid.Text = "Find AE ID";
            this.btnFindAEid.UseVisualStyleBackColor = true;
            this.btnFindAEid.Click += new System.EventHandler(this.btnFindAEid_Click);
            // 
            // lblCountErrors
            // 
            this.lblCountErrors.AutoSize = true;
            this.lblCountErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCountErrors.ForeColor = System.Drawing.Color.Maroon;
            this.lblCountErrors.Location = new System.Drawing.Point(1120, 14);
            this.lblCountErrors.Name = "lblCountErrors";
            this.lblCountErrors.Size = new System.Drawing.Size(0, 13);
            this.lblCountErrors.TabIndex = 23;
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserId.Location = new System.Drawing.Point(944, 9);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(0, 16);
            this.lblUserId.TabIndex = 18;
            // 
            // cmsErrorInfo
            // 
            this.cmsErrorInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.cmsErrorInfo.Name = "cmsErrorInfo";
            this.cmsErrorInfo.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // btnUIDump
            // 
            this.btnUIDump.Location = new System.Drawing.Point(1114, 73);
            this.btnUIDump.Name = "btnUIDump";
            this.btnUIDump.Size = new System.Drawing.Size(161, 23);
            this.btnUIDump.TabIndex = 31;
            this.btnUIDump.Text = "Save the Android Ui Dump";
            this.btnUIDump.UseVisualStyleBackColor = true;
            this.btnUIDump.Click += new System.EventHandler(this.btnUIDump_Click);
            // 
            // activityExecutionPerformanceBindingSource
            // 
            this.activityExecutionPerformanceBindingSource.DataSource = typeof(HP.ScalableTestTriageData.Data.DataLog.ActivityExecutionPerformance);
            // 
            // triageDataListBindingSource
            // 
            this.triageDataListBindingSource.DataSource = typeof(HP.ScalableTestTriageData.Data.TriageDataList);
            // 
            // TriageDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1289, 983);
            this.Controls.Add(this.btnUIDump);
            this.Controls.Add(this.btnFindAEid);
            this.Controls.Add(this.txtActivityExecutionId);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnRetrieve);
            this.Controls.Add(this.cboProduct);
            this.Controls.Add(this.cboIpAddress);
            this.Controls.Add(this.lblCountErrors);
            this.Controls.Add(this.lblActivityName);
            this.Controls.Add(this.lbActivityType);
            this.Controls.Add(this.lblFirmwareDatecode);
            this.Controls.Add(this.lblFirmwareRevision);
            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.btnSessionTriage);
            this.Controls.Add(this.tabControlInfo);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.pbThumbnail);
            this.Controls.Add(this.lblModelInfo);
            this.Controls.Add(this.lblIPAddress);
            this.Controls.Add(this.lblDeviceId);
            this.Controls.Add(this.rgvPerformanceMarkers);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.cboSessionIds);
            this.Controls.Add(this.rgvTriageData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TriageDataForm";
            this.Text = "Triage Data";
            this.Load += new System.EventHandler(this.TriageDataForm_Load);
            this.Shown += new System.EventHandler(this.TriageDataForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.rgvTriageData.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvTriageData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPerformanceMarkers.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPerformanceMarkers)).EndInit();
            this.tabControlInfo.ResumeLayout(false);
            this.tabPageError.ResumeLayout(false);
            this.tabPageImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbControlPanel)).EndInit();
            this.tabPageJAError.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbThumbnail)).EndInit();
            this.cmsErrorInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.activityExecutionPerformanceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triageDataListBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgvTriageData;
        private System.Windows.Forms.BindingSource triageDataListBindingSource;
        private System.Windows.Forms.ComboBox cboSessionIds;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetData;
        private Telerik.WinControls.UI.RadGridView rgvPerformanceMarkers;
        private System.Windows.Forms.BindingSource activityExecutionPerformanceBindingSource;
        private System.Windows.Forms.Label lblDeviceId;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.Label lblModelInfo;
        private System.Windows.Forms.PictureBox pbThumbnail;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TabControl tabControlInfo;
        private System.Windows.Forms.TabPage tabPageError;
        private System.Windows.Forms.RichTextBox rtbErrorMessage;
        private System.Windows.Forms.TabPage tabPageImage;
        private System.Windows.Forms.PictureBox pbControlPanel;
        private System.Windows.Forms.Button btnSessionTriage;
        private System.Windows.Forms.Label lblFirmwareRevision;
        private System.Windows.Forms.Label lblFirmwareDatecode;
        private System.Windows.Forms.Label lbActivityType;
        private System.Windows.Forms.Label lblActivityName;
        private System.Windows.Forms.ComboBox cboIpAddress;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.Button btnRetrieve;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtActivityExecutionId;
        private System.Windows.Forms.Button btnFindAEid;
        private System.Windows.Forms.Label lblCountErrors;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.TabPage tabPageJAError;
        private System.Windows.Forms.RichTextBox errMessageAndroid;
        private System.Windows.Forms.ContextMenuStrip cmsErrorInfo;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Button btnUIDump;
    }
}


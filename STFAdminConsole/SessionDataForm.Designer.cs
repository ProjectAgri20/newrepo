using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.LabConsole
{
    partial class SessionDataForm
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
            SessionClient.Instance.ClearSessionRequestReceived -= controller_SessionResetComplete; //Clean up event subscription
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn2 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn3 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionDataForm));
            this.close_Button = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sessionData_GridView = new Telerik.WinControls.UI.RadGridView();
            this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retentionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDeviceMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.releaseSession_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.notesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.extendToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.configurationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteSessionInfoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.releaseSession_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.refresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.exportMemoryToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusLabel = new System.Windows.Forms.Label();
            this.memoryFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.sessionData_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionData_GridView.MasterTemplate)).BeginInit();
            this.mainContextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(1293, 515);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(100, 28);
            this.close_Button.TabIndex = 3;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "SessionId";
            this.dataGridViewTextBoxColumn1.HeaderText = "Session ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 85;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ScenarioName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Scenario Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 350;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "StartDate";
            this.dataGridViewTextBoxColumn3.HeaderText = "Start Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 130;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ActivityCount";
            this.dataGridViewTextBoxColumn4.HeaderText = "Activities";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "PrintJobCount";
            this.dataGridViewTextBoxColumn5.HeaderText = "Print Jobs";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // sessionData_GridView
            // 
            this.sessionData_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionData_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.sessionData_GridView.ContextMenuStrip = this.mainContextMenuStrip;
            this.sessionData_GridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.sessionData_GridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sessionData_GridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sessionData_GridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sessionData_GridView.Location = new System.Drawing.Point(0, 28);
            // 
            // 
            // 
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.AllowHide = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "SessionId";
            gridViewTextBoxColumn1.HeaderText = "Session ID";
            gridViewTextBoxColumn1.MaxWidth = 80;
            gridViewTextBoxColumn1.MinWidth = 80;
            gridViewTextBoxColumn1.Name = "sessionId_GridViewColumn";
            gridViewTextBoxColumn1.Width = 80;
            gridViewTextBoxColumn2.FieldName = "Cycle";
            gridViewTextBoxColumn2.HeaderText = "Cycle";
            gridViewTextBoxColumn2.MinWidth = 80;
            gridViewTextBoxColumn2.Name = "sessionCycle_GridViewColumn";
            gridViewTextBoxColumn2.Width = 80;
            gridViewTextBoxColumn3.AllowGroup = false;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "SessionName";
            gridViewTextBoxColumn3.HeaderText = "Session Name";
            gridViewTextBoxColumn3.MinWidth = 200;
            gridViewTextBoxColumn3.Name = "sessionName_GridViewColumn";
            gridViewTextBoxColumn3.Width = 200;
            gridViewTextBoxColumn4.FieldName = "Type";
            gridViewTextBoxColumn4.HeaderText = "Session Type";
            gridViewTextBoxColumn4.MinWidth = 100;
            gridViewTextBoxColumn4.Name = "sessionType_GridViewColumn";
            gridViewTextBoxColumn4.Width = 100;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "Tags";
            gridViewTextBoxColumn5.HeaderText = "Tags";
            gridViewTextBoxColumn5.MinWidth = 100;
            gridViewTextBoxColumn5.Name = "tags_GridViewColumn";
            gridViewTextBoxColumn5.Width = 100;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.FieldName = "Owner";
            gridViewTextBoxColumn6.HeaderText = "Owner";
            gridViewTextBoxColumn6.MaxWidth = 100;
            gridViewTextBoxColumn6.MinWidth = 100;
            gridViewTextBoxColumn6.Name = "owner_GridViewColumn";
            gridViewTextBoxColumn6.Width = 100;
            gridViewDateTimeColumn1.AllowGroup = false;
            gridViewDateTimeColumn1.EnableExpressionEditor = false;
            gridViewDateTimeColumn1.FieldName = "StartDateTime";
            gridViewDateTimeColumn1.FilteringMode = Telerik.WinControls.UI.GridViewTimeFilteringMode.Date;
            gridViewDateTimeColumn1.HeaderText = "Start Date";
            gridViewDateTimeColumn1.MaxWidth = 150;
            gridViewDateTimeColumn1.MinWidth = 150;
            gridViewDateTimeColumn1.Name = "startDate_GridViewColumn";
            gridViewDateTimeColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending;
            gridViewDateTimeColumn1.Width = 150;
            gridViewDateTimeColumn2.AllowGroup = false;
            gridViewDateTimeColumn2.EnableExpressionEditor = false;
            gridViewDateTimeColumn2.FieldName = "EndDateTime";
            gridViewDateTimeColumn2.FilteringMode = Telerik.WinControls.UI.GridViewTimeFilteringMode.Date;
            gridViewDateTimeColumn2.HeaderText = "End Date";
            gridViewDateTimeColumn2.MaxWidth = 150;
            gridViewDateTimeColumn2.MinWidth = 150;
            gridViewDateTimeColumn2.Name = "endDate_GridViewColumn";
            gridViewDateTimeColumn2.Width = 150;
            gridViewDateTimeColumn3.AllowGroup = false;
            gridViewDateTimeColumn3.EnableExpressionEditor = false;
            gridViewDateTimeColumn3.FieldName = "ExpirationDateTime";
            gridViewDateTimeColumn3.FilteringMode = Telerik.WinControls.UI.GridViewTimeFilteringMode.Date;
            gridViewDateTimeColumn3.HeaderText = "Expiration Date";
            gridViewDateTimeColumn3.MaxWidth = 150;
            gridViewDateTimeColumn3.MinWidth = 150;
            gridViewDateTimeColumn3.Name = "expirationDate_GridViewColumn";
            gridViewDateTimeColumn3.Width = 150;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.FieldName = "Status";
            gridViewTextBoxColumn7.HeaderText = "Run State";
            gridViewTextBoxColumn7.MaxWidth = 80;
            gridViewTextBoxColumn7.MinWidth = 80;
            gridViewTextBoxColumn7.Name = "runState_GridViewColumn";
            gridViewTextBoxColumn7.Width = 80;
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.FieldName = "ShutdownState";
            gridViewTextBoxColumn8.HeaderText = "Shutdown State";
            gridViewTextBoxColumn8.MaxWidth = 100;
            gridViewTextBoxColumn8.MinWidth = 100;
            gridViewTextBoxColumn8.Name = "shutdownState_GridViewColumn";
            gridViewTextBoxColumn8.Width = 100;
            gridViewDecimalColumn1.AllowGroup = false;
            gridViewDecimalColumn1.DecimalPlaces = 0;
            gridViewDecimalColumn1.EnableExpressionEditor = false;
            gridViewDecimalColumn1.FieldName = "ActivityCount";
            gridViewDecimalColumn1.FormatString = "{0:#,#}";
            gridViewDecimalColumn1.HeaderText = "Activities";
            gridViewDecimalColumn1.MaxWidth = 80;
            gridViewDecimalColumn1.MinWidth = 80;
            gridViewDecimalColumn1.Name = "activityCount_GridViewColumn";
            gridViewDecimalColumn1.Width = 80;
            gridViewTextBoxColumn9.EnableExpressionEditor = false;
            gridViewTextBoxColumn9.FieldName = "Dispatcher";
            gridViewTextBoxColumn9.HeaderText = "Dispatcher";
            gridViewTextBoxColumn9.MaxWidth = 100;
            gridViewTextBoxColumn9.MinWidth = 100;
            gridViewTextBoxColumn9.Name = "dispatcher_GridViewColumn";
            gridViewTextBoxColumn9.Width = 100;
            this.sessionData_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewDateTimeColumn1,
            gridViewDateTimeColumn2,
            gridViewDateTimeColumn3,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewDecimalColumn1,
            gridViewTextBoxColumn9});
            this.sessionData_GridView.MasterTemplate.EnableFiltering = true;
            sortDescriptor1.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor1.PropertyName = "startDate_GridViewColumn";
            this.sessionData_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.sessionData_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.sessionData_GridView.Name = "sessionData_GridView";
            this.sessionData_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.sessionData_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 28, 240, 150);
            this.sessionData_GridView.Size = new System.Drawing.Size(1405, 475);
            this.sessionData_GridView.TabIndex = 11;
            this.sessionData_GridView.Text = "radGridView1";
            this.sessionData_GridView.SelectionChanged += new System.EventHandler(this.sessionData_GridView_SelectionChanged);
            // 
            // mainContextMenuStrip
            // 
            this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notesToolStripMenuItem,
            this.retentionToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.exportDeviceMemoryToolStripMenuItem,
            this.exportToSQLToolStripMenuItem,
            this.toolStripSeparator1,
            this.releaseSession_ToolStripMenuItem});
            this.mainContextMenuStrip.Name = "mainContextMenuStrip";
            this.mainContextMenuStrip.Size = new System.Drawing.Size(194, 186);
            this.mainContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.mainContextMenuStrip_Opening);
            // 
            // notesToolStripMenuItem
            // 
            this.notesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("notesToolStripMenuItem.Image")));
            this.notesToolStripMenuItem.Name = "notesToolStripMenuItem";
            this.notesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.notesToolStripMenuItem.Text = "Notes";
            this.notesToolStripMenuItem.Click += new System.EventHandler(this.notesToolStripButton_Click);
            // 
            // retentionToolStripMenuItem
            // 
            this.retentionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("retentionToolStripMenuItem.Image")));
            this.retentionToolStripMenuItem.Name = "retentionToolStripMenuItem";
            this.retentionToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.retentionToolStripMenuItem.Text = "Retention";
            this.retentionToolStripMenuItem.Click += new System.EventHandler(this.extendToolStripButton_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("configurationToolStripMenuItem.Image")));
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripButton_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteSessionInfoToolStripButton_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exportToolStripMenuItem.Text = "Export Configuration";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripButton_Click);
            // 
            // exportDeviceMemoryToolStripMenuItem
            // 
            this.exportDeviceMemoryToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportDeviceMemoryToolStripMenuItem.Image")));
            this.exportDeviceMemoryToolStripMenuItem.Name = "exportDeviceMemoryToolStripMenuItem";
            this.exportDeviceMemoryToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exportDeviceMemoryToolStripMenuItem.Text = "Export Device Memory";
            this.exportDeviceMemoryToolStripMenuItem.Click += new System.EventHandler(this.exportMemoryToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(190, 6);
            // 
            // releaseSession_ToolStripMenuItem
            // 
            this.releaseSession_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("releaseSession_ToolStripMenuItem.Image")));
            this.releaseSession_ToolStripMenuItem.Name = "releaseSession_ToolStripMenuItem";
            this.releaseSession_ToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.releaseSession_ToolStripMenuItem.Text = "Release Session";
            this.releaseSession_ToolStripMenuItem.Click += new System.EventHandler(this.releaseSession_ToolStripButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notesToolStripButton,
            this.extendToolStripButton,
            this.configurationToolStripButton,
            this.deleteSessionInfoToolStripButton,
            this.releaseSession_ToolStripButton,
            this.toolStripSeparator2,
            this.refresh_ToolStripButton,
            this.toolStripSeparator3,
            this.exportToolStripButton,
            this.exportMemoryToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1405, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // notesToolStripButton
            // 
            this.notesToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("notesToolStripButton.Image")));
            this.notesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.notesToolStripButton.Name = "notesToolStripButton";
            this.notesToolStripButton.Size = new System.Drawing.Size(58, 22);
            this.notesToolStripButton.Text = "Notes";
            this.notesToolStripButton.ToolTipText = "View notes associated with selected Session";
            this.notesToolStripButton.Click += new System.EventHandler(this.notesToolStripButton_Click);
            // 
            // extendToolStripButton
            // 
            this.extendToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("extendToolStripButton.Image")));
            this.extendToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.extendToolStripButton.Name = "extendToolStripButton";
            this.extendToolStripButton.Size = new System.Drawing.Size(78, 22);
            this.extendToolStripButton.Text = "Retention";
            this.extendToolStripButton.ToolTipText = "Adjust the Session Information retention period";
            this.extendToolStripButton.Click += new System.EventHandler(this.extendToolStripButton_Click);
            // 
            // configurationToolStripButton
            // 
            this.configurationToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("configurationToolStripButton.Image")));
            this.configurationToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.configurationToolStripButton.Name = "configurationToolStripButton";
            this.configurationToolStripButton.Size = new System.Drawing.Size(101, 22);
            this.configurationToolStripButton.Text = "Configuration";
            this.configurationToolStripButton.ToolTipText = "View Session configuration details";
            this.configurationToolStripButton.Click += new System.EventHandler(this.configurationToolStripButton_Click);
            // 
            // deleteSessionInfoToolStripButton
            // 
            this.deleteSessionInfoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteSessionInfoToolStripButton.Image")));
            this.deleteSessionInfoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteSessionInfoToolStripButton.Name = "deleteSessionInfoToolStripButton";
            this.deleteSessionInfoToolStripButton.Size = new System.Drawing.Size(60, 22);
            this.deleteSessionInfoToolStripButton.Text = "Delete";
            this.deleteSessionInfoToolStripButton.ToolTipText = "This will delete the Session Information for the selected session";
            this.deleteSessionInfoToolStripButton.Click += new System.EventHandler(this.deleteSessionInfoToolStripButton_Click);
            // 
            // releaseSession_ToolStripButton
            // 
            this.releaseSession_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.releaseSession_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("releaseSession_ToolStripButton.Image")));
            this.releaseSession_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.releaseSession_ToolStripButton.Name = "releaseSession_ToolStripButton";
            this.releaseSession_ToolStripButton.Size = new System.Drawing.Size(108, 22);
            this.releaseSession_ToolStripButton.Text = "Release Session";
            this.releaseSession_ToolStripButton.ToolTipText = "Resets selected session by releasing machines and resources";
            this.releaseSession_ToolStripButton.Click += new System.EventHandler(this.releaseSession_ToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // refresh_ToolStripButton
            // 
            this.refresh_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.refresh_ToolStripButton.CheckOnClick = true;
            this.refresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refresh_ToolStripButton.Image")));
            this.refresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh_ToolStripButton.Name = "refresh_ToolStripButton";
            this.refresh_ToolStripButton.Size = new System.Drawing.Size(66, 22);
            this.refresh_ToolStripButton.Text = "Refresh";
            this.refresh_ToolStripButton.ToolTipText = "Refresh the table below";
            this.refresh_ToolStripButton.Click += new System.EventHandler(this.refresh_ToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // exportToolStripButton
            // 
            this.exportToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripButton.Image")));
            this.exportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportToolStripButton.Name = "exportToolStripButton";
            this.exportToolStripButton.Size = new System.Drawing.Size(137, 22);
            this.exportToolStripButton.Text = "Export Configuration";
            this.exportToolStripButton.ToolTipText = "Export the session configuration details";
            this.exportToolStripButton.Click += new System.EventHandler(this.exportToolStripButton_Click);
            // 
            // exportMemoryToolStripButton
            // 
            this.exportMemoryToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("exportMemoryToolStripButton.Image")));
            this.exportMemoryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportMemoryToolStripButton.Name = "exportMemoryToolStripButton";
            this.exportMemoryToolStripButton.Size = new System.Drawing.Size(146, 22);
            this.exportMemoryToolStripButton.Text = "Export Device Memory";
            this.exportMemoryToolStripButton.ToolTipText = "Export the memory details for each device";
            this.exportMemoryToolStripButton.Click += new System.EventHandler(this.exportMemoryToolStripButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(12, 530);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 15);
            this.statusLabel.TabIndex = 13;
            // 
            // memoryFolderBrowserDialog
            // 
            this.memoryFolderBrowserDialog.Description = "Select the folder to store the device memory files.";
            // 
            // SessionDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(1405, 555);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.sessionData_GridView);
            this.Controls.Add(this.close_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionDataForm";
            this.Text = "Manage Session Data";
            this.Load += new System.EventHandler(this.SessionDataForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sessionData_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionData_GridView)).EndInit();
            this.mainContextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private Telerik.WinControls.UI.RadGridView sessionData_GridView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton notesToolStripButton;
        private System.Windows.Forms.ToolStripButton extendToolStripButton;
        private System.Windows.Forms.ToolStripButton configurationToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteSessionInfoToolStripButton;
        private System.Windows.Forms.ToolStripButton releaseSession_ToolStripButton;
        private System.Windows.Forms.ContextMenuStrip mainContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem notesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retentionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem releaseSession_ToolStripMenuItem;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ToolStripButton refresh_ToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        //private System.Windows.Forms.CheckBox viewAll_CheckBox;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton exportToolStripButton;
        private System.Windows.Forms.ToolStripButton exportMemoryToolStripButton;
        private System.Windows.Forms.FolderBrowserDialog memoryFolderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem exportDeviceMemoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToSQLToolStripMenuItem;
    }
}
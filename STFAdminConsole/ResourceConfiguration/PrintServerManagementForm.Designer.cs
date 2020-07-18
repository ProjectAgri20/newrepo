namespace HP.ScalableTest.LabConsole
{
    partial class PrintServerManagementForm
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
            if (disposing && _controller != null)
            {
                _controller.Dispose();
                _controller = null;
            }

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
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintServerManagementForm));
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor2 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.close_Button = new System.Windows.Forms.Button();
            this.frameworkServerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.printQueueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.printServer_GridView = new Telerik.WinControls.UI.RadGridView();
            this.server_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editServer_ContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeServer_ContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.servers_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.servers_ToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.newServer_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editServer_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.removeServer_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.usage_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printQueue_GridView = new Telerik.WinControls.UI.RadGridView();
            this.deviceQueueMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.associateDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAssociationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queues_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.queues_ToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.newQueue_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.removeQueue_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editQueue_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.asset_ToolStripDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.associateAsset_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAssociation_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.frameworkServerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueueBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printServer_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printServer_GridView.MasterTemplate)).BeginInit();
            this.server_ContextMenuStrip.SuspendLayout();
            this.servers_ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView.MasterTemplate)).BeginInit();
            this.deviceQueueMenuStrip.SuspendLayout();
            this.queues_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.Location = new System.Drawing.Point(917, 400);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(100, 30);
            this.close_Button.TabIndex = 12;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            this.close_Button.Click += new System.EventHandler(this.close_Button_Click);
            // 
            // frameworkServerBindingSource
            // 
            this.frameworkServerBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.FrameworkServer);
            // 
            // printQueueBindingSource
            // 
            this.printQueueBindingSource.DataSource = typeof(HP.ScalableTest.Core.AssetInventory.RemotePrintQueue);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.printServer_GridView);
            this.splitContainer.Panel1.Controls.Add(this.servers_ToolStrip);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.printQueue_GridView);
            this.splitContainer.Panel2.Controls.Add(this.queues_ToolStrip);
            this.splitContainer.Size = new System.Drawing.Size(1029, 394);
            this.splitContainer.SplitterDistance = 582;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 15;
            // 
            // printServer_GridView
            // 
            this.printServer_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.printServer_GridView.ContextMenuStrip = this.server_ContextMenuStrip;
            this.printServer_GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printServer_GridView.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.printServer_GridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.printServer_GridView.Location = new System.Drawing.Point(0, 27);
            // 
            // 
            // 
            this.printServer_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "HostName";
            gridViewTextBoxColumn1.HeaderText = "Hostname";
            gridViewTextBoxColumn1.Name = "system_GridViewColumn";
            gridViewTextBoxColumn1.Width = 145;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "Architecture";
            gridViewTextBoxColumn2.HeaderText = "Arch";
            gridViewTextBoxColumn2.Name = "architecture_GridViewColumn";
            gridViewTextBoxColumn2.Width = 39;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "Processors";
            gridViewTextBoxColumn3.HeaderText = "CPU";
            gridViewTextBoxColumn3.Name = "processors_GridViewColumn";
            gridViewTextBoxColumn3.Width = 39;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "Cores";
            gridViewTextBoxColumn4.HeaderText = "Core";
            gridViewTextBoxColumn4.Name = "cores_GridViewColumn";
            gridViewTextBoxColumn4.Width = 44;
            gridViewTextBoxColumn5.FieldName = "Memory";
            gridViewTextBoxColumn5.HeaderText = "Mem";
            gridViewTextBoxColumn5.Name = "memory_GridViewColumn";
            gridViewTextBoxColumn5.Width = 58;
            gridViewTextBoxColumn6.FieldName = "OperatingSystem";
            gridViewTextBoxColumn6.HeaderText = "Operating System";
            gridViewTextBoxColumn6.Name = "operatingSystem_GridViewColumn";
            gridViewTextBoxColumn6.Width = 241;
            this.printServer_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6});
            this.printServer_GridView.MasterTemplate.EnableGrouping = false;
            sortDescriptor1.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor1.PropertyName = "modificationTime_GridViewColumn";
            this.printServer_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.printServer_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.printServer_GridView.MultiSelect = false;
            this.printServer_GridView.Name = "printServer_GridView";
            this.printServer_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.printServer_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 27, 240, 150);
            this.printServer_GridView.Size = new System.Drawing.Size(582, 367);
            this.printServer_GridView.TabIndex = 16;
            this.printServer_GridView.Text = "Print Servers";
            this.printServer_GridView.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.printServer_GridView_CellFormatting);
            this.printServer_GridView.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.printServer_GridView_CellClick);
            this.printServer_GridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.printServer_GridView_CellDoubleClick);
            // 
            // server_ContextMenuStrip
            // 
            this.server_ContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.server_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editServer_ContextMenuItem,
            this.removeServer_ContextMenuItem});
            this.server_ContextMenuStrip.Name = "server_ContextMenuStrip";
            this.server_ContextMenuStrip.Size = new System.Drawing.Size(185, 78);
            // 
            // editServer_ContextMenuItem
            // 
            this.editServer_ContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editServer_ContextMenuItem.Image")));
            this.editServer_ContextMenuItem.Name = "editServer_ContextMenuItem";
            this.editServer_ContextMenuItem.Size = new System.Drawing.Size(184, 26);
            this.editServer_ContextMenuItem.Text = "Edit";
            this.editServer_ContextMenuItem.Click += new System.EventHandler(this.editServer_ContextMenuItem_Click);
            // 
            // removeServer_ContextMenuItem
            // 
            this.removeServer_ContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeServer_ContextMenuItem.Image")));
            this.removeServer_ContextMenuItem.Name = "removeServer_ContextMenuItem";
            this.removeServer_ContextMenuItem.Size = new System.Drawing.Size(184, 26);
            this.removeServer_ContextMenuItem.Text = "Remove";
            this.removeServer_ContextMenuItem.Click += new System.EventHandler(this.removeServer_ContextMenuItem_Click);
            // 
            // servers_ToolStrip
            // 
            this.servers_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.servers_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.servers_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.servers_ToolStripLabel,
            this.newServer_ToolStripButton,
            this.editServer_ToolStripButton,
            this.removeServer_ToolStripButton,
            this.usage_ToolStripButton,
            this.refresh_ToolStripButton});
            this.servers_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.servers_ToolStrip.Name = "servers_ToolStrip";
            this.servers_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.servers_ToolStrip.Size = new System.Drawing.Size(582, 27);
            this.servers_ToolStrip.TabIndex = 14;
            this.servers_ToolStrip.Text = "toolStrip1";
            // 
            // servers_ToolStripLabel
            // 
            this.servers_ToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.servers_ToolStripLabel.Name = "servers_ToolStripLabel";
            this.servers_ToolStripLabel.Size = new System.Drawing.Size(50, 24);
            this.servers_ToolStripLabel.Text = "Servers";
            // 
            // newServer_ToolStripButton
            // 
            this.newServer_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newServer_ToolStripButton.Image")));
            this.newServer_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newServer_ToolStripButton.Name = "newServer_ToolStripButton";
            this.newServer_ToolStripButton.Size = new System.Drawing.Size(53, 24);
            this.newServer_ToolStripButton.Text = "Add";
            this.newServer_ToolStripButton.ToolTipText = "Add a new print server";
            this.newServer_ToolStripButton.Click += new System.EventHandler(this.newServer_ToolStripButton_Click);
            // 
            // editServer_ToolStripButton
            // 
            this.editServer_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("editServer_ToolStripButton.Image")));
            this.editServer_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editServer_ToolStripButton.Name = "editServer_ToolStripButton";
            this.editServer_ToolStripButton.Size = new System.Drawing.Size(51, 24);
            this.editServer_ToolStripButton.Text = "Edit";
            this.editServer_ToolStripButton.Click += new System.EventHandler(this.editServer_ToolStripButton_Click);
            // 
            // removeServer_ToolStripButton
            // 
            this.removeServer_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeServer_ToolStripButton.Image")));
            this.removeServer_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeServer_ToolStripButton.Name = "removeServer_ToolStripButton";
            this.removeServer_ToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.removeServer_ToolStripButton.Text = "Remove";
            this.removeServer_ToolStripButton.Click += new System.EventHandler(this.removeServer_ToolStripButton_Click);
            // 
            // usage_ToolStripButton
            // 
            this.usage_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.usage_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("usage_ToolStripButton.Image")));
            this.usage_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.usage_ToolStripButton.Name = "usage_ToolStripButton";
            this.usage_ToolStripButton.Size = new System.Drawing.Size(63, 24);
            this.usage_ToolStripButton.Text = "Usage";
            this.usage_ToolStripButton.Click += new System.EventHandler(this.usage_ToolStripButton_Click);
            // 
            // refresh_ToolStripButton
            // 
            this.refresh_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.refresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refresh_ToolStripButton.Image")));
            this.refresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh_ToolStripButton.Name = "refresh_ToolStripButton";
            this.refresh_ToolStripButton.Size = new System.Drawing.Size(70, 24);
            this.refresh_ToolStripButton.Text = "Refresh";
            this.refresh_ToolStripButton.Click += new System.EventHandler(this.refresh_ToolStripButton_Click);
            // 
            // printQueue_GridView
            // 
            this.printQueue_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.printQueue_GridView.ContextMenuStrip = this.deviceQueueMenuStrip;
            this.printQueue_GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printQueue_GridView.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.printQueue_GridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.printQueue_GridView.Location = new System.Drawing.Point(0, 27);
            this.printQueue_GridView.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            // 
            // 
            // 
            this.printQueue_GridView.MasterTemplate.AllowColumnReorder = false;
            this.printQueue_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewCheckBoxColumn1.FieldName = "Active";
            gridViewCheckBoxColumn1.HeaderText = "";
            gridViewCheckBoxColumn1.MinWidth = 26;
            gridViewCheckBoxColumn1.Name = "active_GridViewColumn";
            gridViewCheckBoxColumn1.Width = 54;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.FieldName = "PrinterId";
            gridViewTextBoxColumn7.HeaderText = "Asset Id";
            gridViewTextBoxColumn7.Name = "inventoryId_GridViewColumn";
            gridViewTextBoxColumn7.ReadOnly = true;
            gridViewTextBoxColumn7.Width = 163;
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.FieldName = "Name";
            gridViewTextBoxColumn8.HeaderText = "Name";
            gridViewTextBoxColumn8.Name = "name_GridViewColumn";
            gridViewTextBoxColumn8.ReadOnly = true;
            gridViewTextBoxColumn8.Width = 206;
            this.printQueue_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn1,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8});
            this.printQueue_GridView.MasterTemplate.EnableGrouping = false;
            sortDescriptor2.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor2.PropertyName = "modificationTime_GridViewColumn";
            this.printQueue_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor2});
            this.printQueue_GridView.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.printQueue_GridView.Name = "printQueue_GridView";
            this.printQueue_GridView.PrintStyle.CellPadding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.printQueue_GridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.printQueue_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 27, 240, 150);
            this.printQueue_GridView.RootElement.ShouldPaint = false;
            this.printQueue_GridView.Size = new System.Drawing.Size(442, 367);
            this.printQueue_GridView.TabIndex = 17;
            this.printQueue_GridView.Text = "Print Queues";
            this.printQueue_GridView.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.printQueue_GridView_CellFormatting);
            // 
            // deviceQueueMenuStrip
            // 
            this.deviceQueueMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.deviceQueueMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.associateDeviceToolStripMenuItem,
            this.removeAssociationToolStripMenuItem});
            this.deviceQueueMenuStrip.Name = "deviceQueueMenuStrip";
            this.deviceQueueMenuStrip.Size = new System.Drawing.Size(186, 56);
            // 
            // associateDeviceToolStripMenuItem
            // 
            this.associateDeviceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("associateDeviceToolStripMenuItem.Image")));
            this.associateDeviceToolStripMenuItem.Name = "associateDeviceToolStripMenuItem";
            this.associateDeviceToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.associateDeviceToolStripMenuItem.Text = "Associate Device";
            this.associateDeviceToolStripMenuItem.Click += new System.EventHandler(this.associateAsset_ToolStripMenuItem_Click);
            // 
            // removeAssociationToolStripMenuItem
            // 
            this.removeAssociationToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeAssociationToolStripMenuItem.Image")));
            this.removeAssociationToolStripMenuItem.Name = "removeAssociationToolStripMenuItem";
            this.removeAssociationToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.removeAssociationToolStripMenuItem.Text = "Remove Association";
            this.removeAssociationToolStripMenuItem.Click += new System.EventHandler(this.removeAssociation_ToolStripMenuItem_Click);
            // 
            // queues_ToolStrip
            // 
            this.queues_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.queues_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.queues_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queues_ToolStripLabel,
            this.newQueue_ToolStripButton,
            this.removeQueue_ToolStripButton,
            this.editQueue_ToolStripButton,
            this.asset_ToolStripDropDown});
            this.queues_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.queues_ToolStrip.Name = "queues_ToolStrip";
            this.queues_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.queues_ToolStrip.Size = new System.Drawing.Size(442, 27);
            this.queues_ToolStrip.TabIndex = 15;
            this.queues_ToolStrip.Text = "toolStrip2";
            // 
            // queues_ToolStripLabel
            // 
            this.queues_ToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.queues_ToolStripLabel.Name = "queues_ToolStripLabel";
            this.queues_ToolStripLabel.Size = new System.Drawing.Size(49, 24);
            this.queues_ToolStripLabel.Text = "Queues";
            // 
            // newQueue_ToolStripButton
            // 
            this.newQueue_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newQueue_ToolStripButton.Image")));
            this.newQueue_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newQueue_ToolStripButton.Name = "newQueue_ToolStripButton";
            this.newQueue_ToolStripButton.Size = new System.Drawing.Size(53, 24);
            this.newQueue_ToolStripButton.Text = "Add";
            this.newQueue_ToolStripButton.Click += new System.EventHandler(this.newQueue_ToolStripButton_Click);
            // 
            // removeQueue_ToolStripButton
            // 
            this.removeQueue_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeQueue_ToolStripButton.Image")));
            this.removeQueue_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeQueue_ToolStripButton.Name = "removeQueue_ToolStripButton";
            this.removeQueue_ToolStripButton.Size = new System.Drawing.Size(74, 24);
            this.removeQueue_ToolStripButton.Text = "Remove";
            this.removeQueue_ToolStripButton.Click += new System.EventHandler(this.removeQueue_ToolStripButton_Click);
            // 
            // editQueue_ToolStripButton
            // 
            this.editQueue_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("editQueue_ToolStripButton.Image")));
            this.editQueue_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editQueue_ToolStripButton.Name = "editQueue_ToolStripButton";
            this.editQueue_ToolStripButton.Size = new System.Drawing.Size(51, 24);
            this.editQueue_ToolStripButton.Text = "Edit";
            this.editQueue_ToolStripButton.Click += new System.EventHandler(this.editQueue_ToolStripButton_Click);
            // 
            // asset_ToolStripDropDown
            // 
            this.asset_ToolStripDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.associateAsset_ToolStripMenuItem,
            this.removeAssociation_ToolStripMenuItem});
            this.asset_ToolStripDropDown.Image = ((System.Drawing.Image)(resources.GetObject("asset_ToolStripDropDown.Image")));
            this.asset_ToolStripDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.asset_ToolStripDropDown.Name = "asset_ToolStripDropDown";
            this.asset_ToolStripDropDown.Size = new System.Drawing.Size(75, 24);
            this.asset_ToolStripDropDown.Text = "Device";
            // 
            // associateAsset_ToolStripMenuItem
            // 
            this.associateAsset_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("associateAsset_ToolStripMenuItem.Image")));
            this.associateAsset_ToolStripMenuItem.Name = "associateAsset_ToolStripMenuItem";
            this.associateAsset_ToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.associateAsset_ToolStripMenuItem.Text = "Associate Device";
            this.associateAsset_ToolStripMenuItem.Click += new System.EventHandler(this.associateAsset_ToolStripMenuItem_Click);
            // 
            // removeAssociation_ToolStripMenuItem
            // 
            this.removeAssociation_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeAssociation_ToolStripMenuItem.Image")));
            this.removeAssociation_ToolStripMenuItem.Name = "removeAssociation_ToolStripMenuItem";
            this.removeAssociation_ToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.removeAssociation_ToolStripMenuItem.Text = "Remove Association";
            this.removeAssociation_ToolStripMenuItem.Click += new System.EventHandler(this.removeAssociation_ToolStripMenuItem_Click);
            // 
            // PrintServerManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 442);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.close_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrintServerManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Server Management";
            this.Load += new System.EventHandler(this.PrintServerManagementForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.frameworkServerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueueBindingSource)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.printServer_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printServer_GridView)).EndInit();
            this.server_ContextMenuStrip.ResumeLayout(false);
            this.servers_ToolStrip.ResumeLayout(false);
            this.servers_ToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_GridView)).EndInit();
            this.deviceQueueMenuStrip.ResumeLayout(false);
            this.queues_ToolStrip.ResumeLayout(false);
            this.queues_ToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.BindingSource printQueueBindingSource;
        private System.Windows.Forms.BindingSource frameworkServerBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStrip servers_ToolStrip;
        private System.Windows.Forms.ToolStripLabel servers_ToolStripLabel;
        private System.Windows.Forms.ToolStripButton newServer_ToolStripButton;
        private System.Windows.Forms.ToolStrip queues_ToolStrip;
        private System.Windows.Forms.ToolStripLabel queues_ToolStripLabel;
        private System.Windows.Forms.ToolStripButton removeServer_ToolStripButton;
        private System.Windows.Forms.ToolStripButton refresh_ToolStripButton;
        private System.Windows.Forms.ToolStripButton usage_ToolStripButton;
        private System.Windows.Forms.ToolStripButton newQueue_ToolStripButton;
        private System.Windows.Forms.ToolStripButton removeQueue_ToolStripButton;
        private System.Windows.Forms.ToolStripButton editQueue_ToolStripButton;
        private Telerik.WinControls.UI.RadGridView printServer_GridView;
        private Telerik.WinControls.UI.RadGridView printQueue_GridView;
        private System.Windows.Forms.ToolStripDropDownButton asset_ToolStripDropDown;
        private System.Windows.Forms.ToolStripMenuItem associateAsset_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAssociation_ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip deviceQueueMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem associateDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAssociationToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton editServer_ToolStripButton;
        private System.Windows.Forms.ContextMenuStrip server_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editServer_ContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeServer_ContextMenuItem;
    }
}
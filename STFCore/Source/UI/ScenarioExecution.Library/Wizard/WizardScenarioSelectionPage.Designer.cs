namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    partial class WizardScenarioSelectionPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardScenarioSelectionPage));
            this.scenario_GroupBox = new System.Windows.Forms.GroupBox();
            this.reference_Label = new System.Windows.Forms.Label();
            this.sessionType_Label = new System.Windows.Forms.Label();
            this.sessionCycle_Label = new System.Windows.Forms.Label();
            this.reference_TextBox = new System.Windows.Forms.TextBox();
            this.sessionType_ComboBox = new System.Windows.Forms.ComboBox();
            this.sessionCycle_ComboBox = new System.Windows.Forms.ComboBox();
            this.runtime_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.runtime_Label = new System.Windows.Forms.Label();
            this.retention_ComboBox = new System.Windows.Forms.ComboBox();
            this.sessionName_ComboBox = new System.Windows.Forms.ComboBox();
            this.notes_TextBox = new System.Windows.Forms.TextBox();
            this.sessionName_Label = new System.Windows.Forms.Label();
            this.notes_Label = new System.Windows.Forms.Label();
            this.retention_Label = new System.Windows.Forms.Label();
            this.selectedScenario_TextBox = new System.Windows.Forms.TextBox();
            this.scenario_Label = new System.Windows.Forms.Label();
            this.scenarioSelection_Button = new System.Windows.Forms.Button();
            this.eventLog_CheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.connection_GroupBox = new System.Windows.Forms.GroupBox();
            this.environment_Label = new System.Windows.Forms.Label();
            this.dispatcher_Label = new System.Windows.Forms.Label();
            this.connection_Label = new System.Windows.Forms.Label();
            this.settings_TabControl = new System.Windows.Forms.TabControl();
            this.associatedProducts_TabPage = new System.Windows.Forms.TabPage();
            this.associatedProducts_DataGrid = new System.Windows.Forms.DataGridView();
            this.vendorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.scenarioProductBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.notifications_TabPage = new System.Windows.Forms.TabPage();
            this.RequireLabel = new System.Windows.Forms.Label();
            this.errorMessageString_Label = new System.Windows.Forms.Label();
            this.triggerList_TextBox = new System.Windows.Forms.TextBox();
            this.dartLog_CheckBox = new System.Windows.Forms.CheckBox();
            this.failureTime_comboBox = new System.Windows.Forms.ComboBox();
            this.failureTime_label = new System.Windows.Forms.Label();
            this.threshold_comboBox = new System.Windows.Forms.ComboBox();
            this.threshold_Label = new System.Windows.Forms.Label();
            this.email_Label = new System.Windows.Forms.Label();
            this.email_textBox = new System.Windows.Forms.TextBox();
            this.virtualMachineSelection_TabPage = new System.Windows.Forms.TabPage();
            this.refreshVMs_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.viewVMs_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.holdId_ComboBox = new System.Windows.Forms.ComboBox();
            this.holdId_RadioButton = new System.Windows.Forms.RadioButton();
            this.platform_RadioButton = new System.Windows.Forms.RadioButton();
            this.allVMs_RadioButton = new System.Windows.Forms.RadioButton();
            this.platform_ComboBox = new System.Windows.Forms.ComboBox();
            this.logSettings_TabPage = new System.Windows.Forms.TabPage();
            this.deviceOffline_CheckBox = new System.Windows.Forms.CheckBox();
            this.logLocation_TextBox = new System.Windows.Forms.TextBox();
            this.logLocation_Label = new System.Windows.Forms.Label();
            this.VendorsProductsVersionsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddPvp_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeletePvp_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip_wizScenario = new System.Windows.Forms.ToolTip(this.components);
            this.scenario_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runtime_NumericUpDown)).BeginInit();
            this.connection_GroupBox.SuspendLayout();
            this.settings_TabControl.SuspendLayout();
            this.associatedProducts_TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.associatedProducts_DataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioProductBindingSource)).BeginInit();
            this.notifications_TabPage.SuspendLayout();
            this.virtualMachineSelection_TabPage.SuspendLayout();
            this.logSettings_TabPage.SuspendLayout();
            this.VendorsProductsVersionsContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenario_GroupBox
            // 
            this.scenario_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenario_GroupBox.Controls.Add(this.reference_Label);
            this.scenario_GroupBox.Controls.Add(this.sessionType_Label);
            this.scenario_GroupBox.Controls.Add(this.sessionCycle_Label);
            this.scenario_GroupBox.Controls.Add(this.reference_TextBox);
            this.scenario_GroupBox.Controls.Add(this.sessionType_ComboBox);
            this.scenario_GroupBox.Controls.Add(this.sessionCycle_ComboBox);
            this.scenario_GroupBox.Controls.Add(this.runtime_NumericUpDown);
            this.scenario_GroupBox.Controls.Add(this.runtime_Label);
            this.scenario_GroupBox.Controls.Add(this.retention_ComboBox);
            this.scenario_GroupBox.Controls.Add(this.sessionName_ComboBox);
            this.scenario_GroupBox.Controls.Add(this.notes_TextBox);
            this.scenario_GroupBox.Controls.Add(this.sessionName_Label);
            this.scenario_GroupBox.Controls.Add(this.notes_Label);
            this.scenario_GroupBox.Controls.Add(this.retention_Label);
            this.scenario_GroupBox.Controls.Add(this.selectedScenario_TextBox);
            this.scenario_GroupBox.Controls.Add(this.scenario_Label);
            this.scenario_GroupBox.Controls.Add(this.scenarioSelection_Button);
            this.scenario_GroupBox.Location = new System.Drawing.Point(16, 33);
            this.scenario_GroupBox.Name = "scenario_GroupBox";
            this.scenario_GroupBox.Size = new System.Drawing.Size(655, 199);
            this.scenario_GroupBox.TabIndex = 4;
            this.scenario_GroupBox.TabStop = false;
            this.scenario_GroupBox.Text = "Scenario Settings";
            // 
            // reference_Label
            // 
            this.reference_Label.AutoSize = true;
            this.reference_Label.Location = new System.Drawing.Point(451, 82);
            this.reference_Label.Name = "reference_Label";
            this.reference_Label.Size = new System.Drawing.Size(59, 15);
            this.reference_Label.TabIndex = 40;
            this.reference_Label.Text = "Reference";
            this.reference_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionType_Label
            // 
            this.sessionType_Label.Location = new System.Drawing.Point(31, 82);
            this.sessionType_Label.Name = "sessionType_Label";
            this.sessionType_Label.Size = new System.Drawing.Size(88, 20);
            this.sessionType_Label.TabIndex = 39;
            this.sessionType_Label.Text = "Session Type";
            this.sessionType_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionCycle_Label
            // 
            this.sessionCycle_Label.AutoSize = true;
            this.sessionCycle_Label.Location = new System.Drawing.Point(269, 82);
            this.sessionCycle_Label.Name = "sessionCycle_Label";
            this.sessionCycle_Label.Size = new System.Drawing.Size(36, 15);
            this.sessionCycle_Label.TabIndex = 38;
            this.sessionCycle_Label.Text = "Cycle";
            this.sessionCycle_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // reference_TextBox
            // 
            this.reference_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reference_TextBox.Location = new System.Drawing.Point(516, 79);
            this.reference_TextBox.Name = "reference_TextBox";
            this.reference_TextBox.Size = new System.Drawing.Size(100, 23);
            this.reference_TextBox.TabIndex = 37;
            // 
            // sessionType_ComboBox
            // 
            this.sessionType_ComboBox.DisplayMember = "Name";
            this.sessionType_ComboBox.FormattingEnabled = true;
            this.sessionType_ComboBox.Location = new System.Drawing.Point(125, 79);
            this.sessionType_ComboBox.Name = "sessionType_ComboBox";
            this.sessionType_ComboBox.Size = new System.Drawing.Size(114, 23);
            this.sessionType_ComboBox.TabIndex = 36;
            this.sessionType_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // sessionCycle_ComboBox
            // 
            this.sessionCycle_ComboBox.DisplayMember = "Name";
            this.sessionCycle_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sessionCycle_ComboBox.FormattingEnabled = true;
            this.sessionCycle_ComboBox.Location = new System.Drawing.Point(311, 79);
            this.sessionCycle_ComboBox.Name = "sessionCycle_ComboBox";
            this.sessionCycle_ComboBox.Size = new System.Drawing.Size(114, 23);
            this.sessionCycle_ComboBox.TabIndex = 35;
            this.sessionCycle_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // runtime_NumericUpDown
            // 
            this.runtime_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runtime_NumericUpDown.Location = new System.Drawing.Point(551, 152);
            this.runtime_NumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.runtime_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.runtime_NumericUpDown.Name = "runtime_NumericUpDown";
            this.runtime_NumericUpDown.Size = new System.Drawing.Size(65, 23);
            this.runtime_NumericUpDown.TabIndex = 32;
            this.runtime_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // runtime_Label
            // 
            this.runtime_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.runtime_Label.Location = new System.Drawing.Point(345, 155);
            this.runtime_Label.Name = "runtime_Label";
            this.runtime_Label.Size = new System.Drawing.Size(184, 20);
            this.runtime_Label.TabIndex = 31;
            this.runtime_Label.Text = "Estimated Runtime (hours)";
            this.runtime_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // retention_ComboBox
            // 
            this.retention_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.retention_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.retention_ComboBox.FormattingEnabled = true;
            this.retention_ComboBox.ItemHeight = 15;
            this.retention_ComboBox.Location = new System.Drawing.Point(125, 151);
            this.retention_ComboBox.Name = "retention_ComboBox";
            this.retention_ComboBox.Size = new System.Drawing.Size(214, 23);
            this.retention_ComboBox.TabIndex = 29;
            // 
            // sessionName_ComboBox
            // 
            this.sessionName_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionName_ComboBox.DisplayMember = "Name";
            this.sessionName_ComboBox.FormattingEnabled = true;
            this.sessionName_ComboBox.Location = new System.Drawing.Point(125, 50);
            this.sessionName_ComboBox.Name = "sessionName_ComboBox";
            this.sessionName_ComboBox.Size = new System.Drawing.Size(491, 23);
            this.sessionName_ComboBox.TabIndex = 26;
            this.sessionName_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // notes_TextBox
            // 
            this.notes_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.notes_TextBox.Location = new System.Drawing.Point(125, 108);
            this.notes_TextBox.Multiline = true;
            this.notes_TextBox.Name = "notes_TextBox";
            this.notes_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.notes_TextBox.Size = new System.Drawing.Size(491, 38);
            this.notes_TextBox.TabIndex = 28;
            // 
            // sessionName_Label
            // 
            this.sessionName_Label.Location = new System.Drawing.Point(16, 53);
            this.sessionName_Label.Name = "sessionName_Label";
            this.sessionName_Label.Size = new System.Drawing.Size(102, 20);
            this.sessionName_Label.TabIndex = 25;
            this.sessionName_Label.Text = "Session Name";
            this.sessionName_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // notes_Label
            // 
            this.notes_Label.Location = new System.Drawing.Point(70, 111);
            this.notes_Label.Name = "notes_Label";
            this.notes_Label.Size = new System.Drawing.Size(48, 20);
            this.notes_Label.TabIndex = 27;
            this.notes_Label.Text = "Notes";
            this.notes_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // retention_Label
            // 
            this.retention_Label.Location = new System.Drawing.Point(17, 155);
            this.retention_Label.Name = "retention_Label";
            this.retention_Label.Size = new System.Drawing.Size(102, 20);
            this.retention_Label.TabIndex = 30;
            this.retention_Label.Text = "Log Retention";
            this.retention_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // selectedScenario_TextBox
            // 
            this.selectedScenario_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedScenario_TextBox.Location = new System.Drawing.Point(125, 21);
            this.selectedScenario_TextBox.Name = "selectedScenario_TextBox";
            this.selectedScenario_TextBox.ReadOnly = true;
            this.selectedScenario_TextBox.Size = new System.Drawing.Size(491, 23);
            this.selectedScenario_TextBox.TabIndex = 2;
            // 
            // scenario_Label
            // 
            this.scenario_Label.Location = new System.Drawing.Point(52, 28);
            this.scenario_Label.Name = "scenario_Label";
            this.scenario_Label.Size = new System.Drawing.Size(66, 20);
            this.scenario_Label.TabIndex = 0;
            this.scenario_Label.Text = "Scenario";
            this.scenario_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // scenarioSelection_Button
            // 
            this.scenarioSelection_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioSelection_Button.Location = new System.Drawing.Point(622, 21);
            this.scenarioSelection_Button.Name = "scenarioSelection_Button";
            this.scenarioSelection_Button.Size = new System.Drawing.Size(27, 23);
            this.scenarioSelection_Button.TabIndex = 2;
            this.scenarioSelection_Button.Text = "...";
            this.scenarioSelection_Button.UseVisualStyleBackColor = true;
            this.scenarioSelection_Button.Click += new System.EventHandler(this.scenarioSelection_Button_Click);
            // 
            // eventLog_CheckBox
            // 
            this.eventLog_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.eventLog_CheckBox.AutoSize = true;
            this.eventLog_CheckBox.Location = new System.Drawing.Point(16, 50);
            this.eventLog_CheckBox.Name = "eventLog_CheckBox";
            this.eventLog_CheckBox.Size = new System.Drawing.Size(243, 19);
            this.eventLog_CheckBox.TabIndex = 34;
            this.eventLog_CheckBox.Text = "Collect Event Logs from Virtual Machines";
            this.eventLog_CheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(519, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Welcome to the Session Configuration wizard!  To get started, select your scenari" +
    "o options below.";
            // 
            // connection_GroupBox
            // 
            this.connection_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connection_GroupBox.Controls.Add(this.environment_Label);
            this.connection_GroupBox.Controls.Add(this.dispatcher_Label);
            this.connection_GroupBox.Controls.Add(this.connection_Label);
            this.connection_GroupBox.Location = new System.Drawing.Point(15, 415);
            this.connection_GroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connection_GroupBox.Name = "connection_GroupBox";
            this.connection_GroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connection_GroupBox.Size = new System.Drawing.Size(656, 39);
            this.connection_GroupBox.TabIndex = 7;
            this.connection_GroupBox.TabStop = false;
            this.connection_GroupBox.Text = "Environment Settings";
            // 
            // environment_Label
            // 
            this.environment_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.environment_Label.Location = new System.Drawing.Point(413, 18);
            this.environment_Label.Name = "environment_Label";
            this.environment_Label.Size = new System.Drawing.Size(209, 19);
            this.environment_Label.TabIndex = 2;
            this.environment_Label.Text = "<Environment>";
            this.environment_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dispatcher_Label
            // 
            this.dispatcher_Label.Location = new System.Drawing.Point(96, 18);
            this.dispatcher_Label.Name = "dispatcher_Label";
            this.dispatcher_Label.Size = new System.Drawing.Size(104, 19);
            this.dispatcher_Label.TabIndex = 1;
            this.dispatcher_Label.Text = "[Not Connected]";
            // 
            // connection_Label
            // 
            this.connection_Label.AutoSize = true;
            this.connection_Label.Location = new System.Drawing.Point(15, 18);
            this.connection_Label.Name = "connection_Label";
            this.connection_Label.Size = new System.Drawing.Size(82, 15);
            this.connection_Label.TabIndex = 0;
            this.connection_Label.Text = "Connected to:";
            // 
            // settings_TabControl
            // 
            this.settings_TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settings_TabControl.Controls.Add(this.associatedProducts_TabPage);
            this.settings_TabControl.Controls.Add(this.notifications_TabPage);
            this.settings_TabControl.Controls.Add(this.virtualMachineSelection_TabPage);
            this.settings_TabControl.Controls.Add(this.logSettings_TabPage);
            this.settings_TabControl.Location = new System.Drawing.Point(16, 238);
            this.settings_TabControl.Name = "settings_TabControl";
            this.settings_TabControl.SelectedIndex = 0;
            this.settings_TabControl.Size = new System.Drawing.Size(655, 172);
            this.settings_TabControl.TabIndex = 3;
            // 
            // associatedProducts_TabPage
            // 
            this.associatedProducts_TabPage.BackColor = System.Drawing.SystemColors.Window;
            this.associatedProducts_TabPage.Controls.Add(this.associatedProducts_DataGrid);
            this.associatedProducts_TabPage.Location = new System.Drawing.Point(4, 24);
            this.associatedProducts_TabPage.Name = "associatedProducts_TabPage";
            this.associatedProducts_TabPage.Size = new System.Drawing.Size(647, 144);
            this.associatedProducts_TabPage.TabIndex = 2;
            this.associatedProducts_TabPage.Text = "Vendors, Products and Versions";
            // 
            // associatedProducts_DataGrid
            // 
            this.associatedProducts_DataGrid.AllowUserToAddRows = false;
            this.associatedProducts_DataGrid.AllowUserToDeleteRows = false;
            this.associatedProducts_DataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.associatedProducts_DataGrid.AutoGenerateColumns = false;
            this.associatedProducts_DataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.associatedProducts_DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.associatedProducts_DataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vendorDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.versionDataGridViewTextBoxColumn,
            this.activeDataGridViewCheckBoxColumn});
            this.associatedProducts_DataGrid.DataSource = this.scenarioProductBindingSource;
            this.associatedProducts_DataGrid.Location = new System.Drawing.Point(-2, -3);
            this.associatedProducts_DataGrid.Name = "associatedProducts_DataGrid";
            this.associatedProducts_DataGrid.Size = new System.Drawing.Size(650, 150);
            this.associatedProducts_DataGrid.TabIndex = 1;
            this.associatedProducts_DataGrid.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.AssociatedProductsDataGrid_CellValidated);
            this.associatedProducts_DataGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.AssociatedProductsDataGrid_CellValidating);
            this.associatedProducts_DataGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.AssociatedProductsDataGridView_EditingControlShowing);
            this.associatedProducts_DataGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AssociatedProductsDataGridView_MouseClick);
            // 
            // vendorDataGridViewTextBoxColumn
            // 
            this.vendorDataGridViewTextBoxColumn.DataPropertyName = "Vendor";
            this.vendorDataGridViewTextBoxColumn.HeaderText = "Vendor";
            this.vendorDataGridViewTextBoxColumn.Name = "vendorDataGridViewTextBoxColumn";
            this.vendorDataGridViewTextBoxColumn.Width = 69;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 64;
            // 
            // versionDataGridViewTextBoxColumn
            // 
            this.versionDataGridViewTextBoxColumn.DataPropertyName = "Version";
            this.versionDataGridViewTextBoxColumn.HeaderText = "Version";
            this.versionDataGridViewTextBoxColumn.Name = "versionDataGridViewTextBoxColumn";
            this.versionDataGridViewTextBoxColumn.Width = 70;
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "Active";
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            this.activeDataGridViewCheckBoxColumn.Width = 46;
            // 
            // scenarioProductBindingSource
            // 
            this.scenarioProductBindingSource.DataSource = typeof(HP.ScalableTest.UI.ScenarioConfiguration.Import.ScenarioProduct);
            // 
            // notifications_TabPage
            // 
            this.notifications_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.notifications_TabPage.Controls.Add(this.RequireLabel);
            this.notifications_TabPage.Controls.Add(this.errorMessageString_Label);
            this.notifications_TabPage.Controls.Add(this.triggerList_TextBox);
            this.notifications_TabPage.Controls.Add(this.dartLog_CheckBox);
            this.notifications_TabPage.Controls.Add(this.failureTime_comboBox);
            this.notifications_TabPage.Controls.Add(this.failureTime_label);
            this.notifications_TabPage.Controls.Add(this.threshold_comboBox);
            this.notifications_TabPage.Controls.Add(this.threshold_Label);
            this.notifications_TabPage.Controls.Add(this.email_Label);
            this.notifications_TabPage.Controls.Add(this.email_textBox);
            this.notifications_TabPage.Location = new System.Drawing.Point(4, 24);
            this.notifications_TabPage.Name = "notifications_TabPage";
            this.notifications_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.notifications_TabPage.Size = new System.Drawing.Size(647, 144);
            this.notifications_TabPage.TabIndex = 0;
            this.notifications_TabPage.Text = "Notification Settings";
            // 
            // RequireLabel
            // 
            this.RequireLabel.AutoSize = true;
            this.RequireLabel.Location = new System.Drawing.Point(431, 14);
            this.RequireLabel.Name = "RequireLabel";
            this.RequireLabel.Size = new System.Drawing.Size(138, 15);
            this.RequireLabel.TabIndex = 31;
            this.RequireLabel.Text = "Required To Collect Logs";
            // 
            // errorMessageString_Label
            // 
            this.errorMessageString_Label.AutoSize = true;
            this.errorMessageString_Label.Location = new System.Drawing.Point(285, 49);
            this.errorMessageString_Label.Name = "errorMessageString_Label";
            this.errorMessageString_Label.Size = new System.Drawing.Size(235, 15);
            this.errorMessageString_Label.TabIndex = 30;
            this.errorMessageString_Label.Text = "Error Messages (Press Enter between errors)";
            // 
            // triggerList_TextBox
            // 
            this.triggerList_TextBox.Location = new System.Drawing.Point(288, 67);
            this.triggerList_TextBox.Multiline = true;
            this.triggerList_TextBox.Name = "triggerList_TextBox";
            this.triggerList_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.triggerList_TextBox.Size = new System.Drawing.Size(408, 71);
            this.triggerList_TextBox.TabIndex = 29;
            this.triggerList_TextBox.Text = resources.GetString("triggerList_TextBox.Text");
            // 
            // dartLog_CheckBox
            // 
            this.dartLog_CheckBox.AutoSize = true;
            this.dartLog_CheckBox.Location = new System.Drawing.Point(51, 119);
            this.dartLog_CheckBox.Name = "dartLog_CheckBox";
            this.dartLog_CheckBox.Size = new System.Drawing.Size(231, 19);
            this.dartLog_CheckBox.TabIndex = 10;
            this.dartLog_CheckBox.Text = "Collect JTA/Dart Logs where applicable";
            this.dartLog_CheckBox.UseVisualStyleBackColor = true;
            // 
            // failureTime_comboBox
            // 
            this.failureTime_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.failureTime_comboBox.FormattingEnabled = true;
            this.failureTime_comboBox.Location = new System.Drawing.Point(142, 84);
            this.failureTime_comboBox.Name = "failureTime_comboBox";
            this.failureTime_comboBox.Size = new System.Drawing.Size(121, 23);
            this.failureTime_comboBox.TabIndex = 9;
            this.failureTime_comboBox.Visible = false;
            // 
            // failureTime_label
            // 
            this.failureTime_label.AutoSize = true;
            this.failureTime_label.Location = new System.Drawing.Point(53, 87);
            this.failureTime_label.Name = "failureTime_label";
            this.failureTime_label.Size = new System.Drawing.Size(70, 15);
            this.failureTime_label.TabIndex = 8;
            this.failureTime_label.Text = "Allowed Per";
            this.failureTime_label.Visible = false;
            // 
            // threshold_comboBox
            // 
            this.threshold_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.threshold_comboBox.FormattingEnabled = true;
            this.threshold_comboBox.Location = new System.Drawing.Point(142, 49);
            this.threshold_comboBox.Name = "threshold_comboBox";
            this.threshold_comboBox.Size = new System.Drawing.Size(121, 23);
            this.threshold_comboBox.TabIndex = 7;
            // 
            // threshold_Label
            // 
            this.threshold_Label.AutoSize = true;
            this.threshold_Label.Location = new System.Drawing.Point(10, 52);
            this.threshold_Label.Name = "threshold_Label";
            this.threshold_Label.Size = new System.Drawing.Size(113, 15);
            this.threshold_Label.TabIndex = 6;
            this.threshold_Label.Text = "Notification Failures";
            // 
            // email_Label
            // 
            this.email_Label.AutoSize = true;
            this.email_Label.Location = new System.Drawing.Point(12, 14);
            this.email_Label.Name = "email_Label";
            this.email_Label.Size = new System.Drawing.Size(94, 15);
            this.email_Label.TabIndex = 5;
            this.email_Label.Text = "Contact Email(s)";
            // 
            // email_textBox
            // 
            this.email_textBox.Location = new System.Drawing.Point(112, 11);
            this.email_textBox.Name = "email_textBox";
            this.email_textBox.Size = new System.Drawing.Size(309, 23);
            this.email_textBox.TabIndex = 4;
            // 
            // virtualMachineSelection_TabPage
            // 
            this.virtualMachineSelection_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.virtualMachineSelection_TabPage.Controls.Add(this.refreshVMs_LinkLabel);
            this.virtualMachineSelection_TabPage.Controls.Add(this.viewVMs_LinkLabel);
            this.virtualMachineSelection_TabPage.Controls.Add(this.holdId_ComboBox);
            this.virtualMachineSelection_TabPage.Controls.Add(this.holdId_RadioButton);
            this.virtualMachineSelection_TabPage.Controls.Add(this.platform_RadioButton);
            this.virtualMachineSelection_TabPage.Controls.Add(this.allVMs_RadioButton);
            this.virtualMachineSelection_TabPage.Controls.Add(this.platform_ComboBox);
            this.virtualMachineSelection_TabPage.Location = new System.Drawing.Point(4, 24);
            this.virtualMachineSelection_TabPage.Name = "virtualMachineSelection_TabPage";
            this.virtualMachineSelection_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.virtualMachineSelection_TabPage.Size = new System.Drawing.Size(647, 144);
            this.virtualMachineSelection_TabPage.TabIndex = 1;
            this.virtualMachineSelection_TabPage.Text = "Virtual Machine Selection";
            // 
            // refreshVMs_LinkLabel
            // 
            this.refreshVMs_LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshVMs_LinkLabel.AutoSize = true;
            this.refreshVMs_LinkLabel.Location = new System.Drawing.Point(595, 76);
            this.refreshVMs_LinkLabel.Name = "refreshVMs_LinkLabel";
            this.refreshVMs_LinkLabel.Size = new System.Drawing.Size(46, 15);
            this.refreshVMs_LinkLabel.TabIndex = 40;
            this.refreshVMs_LinkLabel.TabStop = true;
            this.refreshVMs_LinkLabel.Text = "Refresh";
            this.refreshVMs_LinkLabel.VisitedLinkColor = System.Drawing.Color.Blue;
            this.refreshVMs_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refreshVMs_LinkLabel_LinkClicked);
            // 
            // viewVMs_LinkLabel
            // 
            this.viewVMs_LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.viewVMs_LinkLabel.AutoSize = true;
            this.viewVMs_LinkLabel.Location = new System.Drawing.Point(485, 76);
            this.viewVMs_LinkLabel.Name = "viewVMs_LinkLabel";
            this.viewVMs_LinkLabel.Size = new System.Drawing.Size(88, 15);
            this.viewVMs_LinkLabel.TabIndex = 39;
            this.viewVMs_LinkLabel.TabStop = true;
            this.viewVMs_LinkLabel.Text = "View Selections";
            this.viewVMs_LinkLabel.VisitedLinkColor = System.Drawing.Color.Blue;
            this.viewVMs_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.viewVMs_LinkLabel_LinkClicked);
            // 
            // holdId_ComboBox
            // 
            this.holdId_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.holdId_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.holdId_ComboBox.Enabled = false;
            this.holdId_ComboBox.FormattingEnabled = true;
            this.holdId_ComboBox.Location = new System.Drawing.Point(150, 51);
            this.holdId_ComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.holdId_ComboBox.Name = "holdId_ComboBox";
            this.holdId_ComboBox.Size = new System.Drawing.Size(491, 23);
            this.holdId_ComboBox.TabIndex = 37;
            // 
            // holdId_RadioButton
            // 
            this.holdId_RadioButton.Location = new System.Drawing.Point(13, 52);
            this.holdId_RadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.holdId_RadioButton.Name = "holdId_RadioButton";
            this.holdId_RadioButton.Size = new System.Drawing.Size(132, 20);
            this.holdId_RadioButton.TabIndex = 36;
            this.holdId_RadioButton.Text = "Select by Hold ID";
            this.holdId_RadioButton.UseVisualStyleBackColor = true;
            this.holdId_RadioButton.CheckedChanged += new System.EventHandler(this.resourcePool_RadioButton_CheckedChanged);
            // 
            // platform_RadioButton
            // 
            this.platform_RadioButton.Location = new System.Drawing.Point(13, 27);
            this.platform_RadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.platform_RadioButton.Name = "platform_RadioButton";
            this.platform_RadioButton.Size = new System.Drawing.Size(132, 20);
            this.platform_RadioButton.TabIndex = 35;
            this.platform_RadioButton.Text = "Select by Platform";
            this.platform_RadioButton.UseVisualStyleBackColor = true;
            this.platform_RadioButton.CheckedChanged += new System.EventHandler(this.resourcePool_RadioButton_CheckedChanged);
            // 
            // allVMs_RadioButton
            // 
            this.allVMs_RadioButton.Checked = true;
            this.allVMs_RadioButton.Location = new System.Drawing.Point(13, 5);
            this.allVMs_RadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.allVMs_RadioButton.Name = "allVMs_RadioButton";
            this.allVMs_RadioButton.Size = new System.Drawing.Size(131, 18);
            this.allVMs_RadioButton.TabIndex = 34;
            this.allVMs_RadioButton.TabStop = true;
            this.allVMs_RadioButton.Text = "All Available VMs";
            this.allVMs_RadioButton.UseVisualStyleBackColor = true;
            this.allVMs_RadioButton.CheckedChanged += new System.EventHandler(this.resourcePool_RadioButton_CheckedChanged);
            // 
            // platform_ComboBox
            // 
            this.platform_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.platform_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platform_ComboBox.Enabled = false;
            this.platform_ComboBox.FormattingEnabled = true;
            this.platform_ComboBox.ItemHeight = 15;
            this.platform_ComboBox.Location = new System.Drawing.Point(150, 26);
            this.platform_ComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.platform_ComboBox.Name = "platform_ComboBox";
            this.platform_ComboBox.Size = new System.Drawing.Size(491, 23);
            this.platform_ComboBox.TabIndex = 38;
            // 
            // logSettings_TabPage
            // 
            this.logSettings_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.logSettings_TabPage.Controls.Add(this.deviceOffline_CheckBox);
            this.logSettings_TabPage.Controls.Add(this.logLocation_TextBox);
            this.logSettings_TabPage.Controls.Add(this.logLocation_Label);
            this.logSettings_TabPage.Controls.Add(this.eventLog_CheckBox);
            this.logSettings_TabPage.Location = new System.Drawing.Point(4, 24);
            this.logSettings_TabPage.Name = "logSettings_TabPage";
            this.logSettings_TabPage.Size = new System.Drawing.Size(647, 144);
            this.logSettings_TabPage.TabIndex = 3;
            this.logSettings_TabPage.Text = "Log Settings";
            // 
            // deviceOffline_CheckBox
            // 
            this.deviceOffline_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceOffline_CheckBox.AutoSize = true;
            this.deviceOffline_CheckBox.Location = new System.Drawing.Point(16, 75);
            this.deviceOffline_CheckBox.Name = "deviceOffline_CheckBox";
            this.deviceOffline_CheckBox.Size = new System.Drawing.Size(272, 19);
            this.deviceOffline_CheckBox.TabIndex = 37;
            this.deviceOffline_CheckBox.Text = "Take devices offline that become unresponsive";
            this.deviceOffline_CheckBox.UseVisualStyleBackColor = true;
            // 
            // logLocation_TextBox
            // 
            this.logLocation_TextBox.Location = new System.Drawing.Point(183, 11);
            this.logLocation_TextBox.Name = "logLocation_TextBox";
            this.logLocation_TextBox.Size = new System.Drawing.Size(254, 23);
            this.logLocation_TextBox.TabIndex = 36;
            this.toolTip_wizScenario.SetToolTip(this.logLocation_TextBox, "i.e. STFData01-bc.etl.boi.rd.hpicorp.net");
            // 
            // logLocation_Label
            // 
            this.logLocation_Label.AutoSize = true;
            this.logLocation_Label.Location = new System.Drawing.Point(12, 14);
            this.logLocation_Label.Name = "logLocation_Label";
            this.logLocation_Label.Size = new System.Drawing.Size(162, 15);
            this.logLocation_Label.TabIndex = 35;
            this.logLocation_Label.Text = "Monitor Service Log Location";
            // 
            // VendorsProductsVersionsContextMenuStrip
            // 
            this.VendorsProductsVersionsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddPvp_ToolStripMenuItem,
            this.DeletePvp_ToolStripMenuItem});
            this.VendorsProductsVersionsContextMenuStrip.Name = "VendorsProductsVersionsContextMenuStrip";
            this.VendorsProductsVersionsContextMenuStrip.Size = new System.Drawing.Size(108, 48);
            // 
            // AddPvp_ToolStripMenuItem
            // 
            this.AddPvp_ToolStripMenuItem.Name = "AddPvp_ToolStripMenuItem";
            this.AddPvp_ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.AddPvp_ToolStripMenuItem.Text = "Add";
            this.AddPvp_ToolStripMenuItem.Click += new System.EventHandler(this.AddPvp_ToolStripMenuItem_Click);
            // 
            // DeletePvp_ToolStripMenuItem
            // 
            this.DeletePvp_ToolStripMenuItem.Name = "DeletePvp_ToolStripMenuItem";
            this.DeletePvp_ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.DeletePvp_ToolStripMenuItem.Text = "Delete";
            this.DeletePvp_ToolStripMenuItem.Click += new System.EventHandler(this.DeletePvp_ToolStripMenuItem_Click);
            // 
            // WizardScenarioSelectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.settings_TabControl);
            this.Controls.Add(this.connection_GroupBox);
            this.Controls.Add(this.scenario_GroupBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WizardScenarioSelectionPage";
            this.Size = new System.Drawing.Size(686, 467);
            this.scenario_GroupBox.ResumeLayout(false);
            this.scenario_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runtime_NumericUpDown)).EndInit();
            this.connection_GroupBox.ResumeLayout(false);
            this.connection_GroupBox.PerformLayout();
            this.settings_TabControl.ResumeLayout(false);
            this.associatedProducts_TabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.associatedProducts_DataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioProductBindingSource)).EndInit();
            this.notifications_TabPage.ResumeLayout(false);
            this.notifications_TabPage.PerformLayout();
            this.virtualMachineSelection_TabPage.ResumeLayout(false);
            this.virtualMachineSelection_TabPage.PerformLayout();
            this.logSettings_TabPage.ResumeLayout(false);
            this.logSettings_TabPage.PerformLayout();
            this.VendorsProductsVersionsContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.GroupBox scenario_GroupBox;
        protected System.Windows.Forms.ComboBox retention_ComboBox;
        protected System.Windows.Forms.ComboBox sessionName_ComboBox;
        protected System.Windows.Forms.TextBox notes_TextBox;
        protected System.Windows.Forms.Label sessionName_Label;
        protected System.Windows.Forms.Label notes_Label;
        protected System.Windows.Forms.Label retention_Label;
        protected System.Windows.Forms.TextBox selectedScenario_TextBox;
        protected System.Windows.Forms.Label scenario_Label;
        protected System.Windows.Forms.Button scenarioSelection_Button;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label runtime_Label;
        protected System.Windows.Forms.NumericUpDown runtime_NumericUpDown;
        protected System.Windows.Forms.GroupBox connection_GroupBox;
        protected System.Windows.Forms.Label environment_Label;
        protected System.Windows.Forms.Label dispatcher_Label;
        protected System.Windows.Forms.Label connection_Label;
        private System.Windows.Forms.TabControl settings_TabControl;
        private System.Windows.Forms.TabPage notifications_TabPage;
        private System.Windows.Forms.TabPage virtualMachineSelection_TabPage;
        private System.Windows.Forms.ComboBox threshold_comboBox;
        private System.Windows.Forms.Label threshold_Label;
        private System.Windows.Forms.Label email_Label;
        private System.Windows.Forms.TextBox email_textBox;
        protected System.Windows.Forms.LinkLabel refreshVMs_LinkLabel;
        protected System.Windows.Forms.LinkLabel viewVMs_LinkLabel;
        protected System.Windows.Forms.ComboBox holdId_ComboBox;
        protected System.Windows.Forms.RadioButton holdId_RadioButton;
        protected System.Windows.Forms.RadioButton platform_RadioButton;
        protected System.Windows.Forms.RadioButton allVMs_RadioButton;
        protected System.Windows.Forms.ComboBox platform_ComboBox;
        private System.Windows.Forms.TabPage associatedProducts_TabPage;
        private System.Windows.Forms.ContextMenuStrip VendorsProductsVersionsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem AddPvp_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeletePvp_ToolStripMenuItem;
        private System.Windows.Forms.ComboBox failureTime_comboBox;
        private System.Windows.Forms.Label failureTime_label;
        private System.Windows.Forms.CheckBox eventLog_CheckBox;
        protected System.Windows.Forms.ComboBox sessionCycle_ComboBox;
        private System.Windows.Forms.TextBox reference_TextBox;
        protected System.Windows.Forms.ComboBox sessionType_ComboBox;
        private System.Windows.Forms.Label sessionCycle_Label;
        private System.Windows.Forms.Label reference_Label;
        private System.Windows.Forms.Label sessionType_Label;
        private System.Windows.Forms.CheckBox dartLog_CheckBox;
        private System.Windows.Forms.Label errorMessageString_Label;
        protected System.Windows.Forms.TextBox triggerList_TextBox;
        private System.Windows.Forms.TabPage logSettings_TabPage;
        private System.Windows.Forms.TextBox logLocation_TextBox;
        private System.Windows.Forms.Label logLocation_Label;
        private System.Windows.Forms.DataGridView associatedProducts_DataGrid;
        private System.Windows.Forms.Label RequireLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource scenarioProductBindingSource;
        private System.Windows.Forms.ToolTip toolTip_wizScenario;
        private System.Windows.Forms.CheckBox deviceOffline_CheckBox;
    }
}

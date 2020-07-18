namespace HP.ScalableTest.Development.UI
{
    partial class PluginFrameworkSimulatorForm
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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginFrameworkSimulatorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.pluginConfigurationTabPage = new System.Windows.Forms.TabPage();
            this.pluginConfigurationPanel = new System.Windows.Forms.Panel();
            this.pluginConfigurationToolStrip = new System.Windows.Forms.ToolStrip();
            this.initializeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.loadToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.validateToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.resetConfigurationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.unsavedChanges_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pluginExecutionTabPage = new System.Windows.Forms.TabPage();
            this.pluginExecutionSplitContainer = new System.Windows.Forms.SplitContainer();
            this.pluginExecutionPanel = new System.Windows.Forms.Panel();
            this.pluginExecutionResultsTextBox = new System.Windows.Forms.TextBox();
            this.pluginExecutionResultsToolStrip = new System.Windows.Forms.ToolStrip();
            this.clearResultToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pluginExecutionToolStrip = new System.Windows.Forms.ToolStrip();
            this.executeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.iterationsNumericUpDown = new HP.ScalableTest.Development.UI.PluginFrameworkSimulatorForm.ToolStripNumericUpDown();
            this.iterationCountToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.resetExecutionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.criticalSectionToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dataLoggerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.pluginConfigurationDataListView = new System.Windows.Forms.DataGridView();
            this.configurationDataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pluginConfigurationDataContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewMetadataContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.renameConfigurationDataContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportConfigurationDataContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteConfigurationDataContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginDataToolStrip = new System.Windows.Forms.ToolStrip();
            this.importConfigurationDataToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.exportConfigurationDataToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.clearConfigurationDataToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.executionBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.label_PluginType = new System.Windows.Forms.Label();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuStrip.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.pluginConfigurationTabPage.SuspendLayout();
            this.pluginConfigurationToolStrip.SuspendLayout();
            this.pluginExecutionTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pluginExecutionSplitContainer)).BeginInit();
            this.pluginExecutionSplitContainer.Panel1.SuspendLayout();
            this.pluginExecutionSplitContainer.Panel2.SuspendLayout();
            this.pluginExecutionSplitContainer.SuspendLayout();
            this.pluginExecutionResultsToolStrip.SuspendLayout();
            this.pluginExecutionToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pluginConfigurationDataListView)).BeginInit();
            this.pluginConfigurationDataContextMenuStrip.SuspendLayout();
            this.pluginDataToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1008, 24);
            this.mainMenuStrip.TabIndex = 1;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.mainTabControl.Controls.Add(this.pluginConfigurationTabPage);
            this.mainTabControl.Controls.Add(this.pluginExecutionTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.ItemSize = new System.Drawing.Size(150, 24);
            this.mainTabControl.Location = new System.Drawing.Point(0, 3);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(790, 699);
            this.mainTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.mainTabControl.TabIndex = 2;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
            this.mainTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.mainTabControl_Selecting);
            // 
            // pluginConfigurationTabPage
            // 
            this.pluginConfigurationTabPage.Controls.Add(this.pluginConfigurationPanel);
            this.pluginConfigurationTabPage.Controls.Add(this.pluginConfigurationToolStrip);
            this.pluginConfigurationTabPage.Location = new System.Drawing.Point(4, 28);
            this.pluginConfigurationTabPage.Name = "pluginConfigurationTabPage";
            this.pluginConfigurationTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.pluginConfigurationTabPage.Size = new System.Drawing.Size(782, 667);
            this.pluginConfigurationTabPage.TabIndex = 1;
            this.pluginConfigurationTabPage.Text = "Plugin Configuration";
            // 
            // pluginConfigurationPanel
            // 
            this.pluginConfigurationPanel.AutoScroll = true;
            this.pluginConfigurationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pluginConfigurationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginConfigurationPanel.Location = new System.Drawing.Point(3, 28);
            this.pluginConfigurationPanel.Name = "pluginConfigurationPanel";
            this.pluginConfigurationPanel.Padding = new System.Windows.Forms.Padding(3);
            this.pluginConfigurationPanel.Size = new System.Drawing.Size(776, 636);
            this.pluginConfigurationPanel.TabIndex = 1;
            // 
            // pluginConfigurationToolStrip
            // 
            this.pluginConfigurationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.pluginConfigurationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.initializeToolStripButton,
            this.loadToolStripButton,
            this.saveToolStripButton,
            this.validateToolStripButton,
            this.resetConfigurationToolStripButton,
            toolStripSeparator1,
            this.unsavedChanges_ToolStripButton});
            this.pluginConfigurationToolStrip.Location = new System.Drawing.Point(3, 3);
            this.pluginConfigurationToolStrip.Name = "pluginConfigurationToolStrip";
            this.pluginConfigurationToolStrip.Size = new System.Drawing.Size(776, 25);
            this.pluginConfigurationToolStrip.TabIndex = 0;
            this.pluginConfigurationToolStrip.Text = "toolStrip1";
            // 
            // initializeToolStripButton
            // 
            this.initializeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("initializeToolStripButton.Image")));
            this.initializeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.initializeToolStripButton.Name = "initializeToolStripButton";
            this.initializeToolStripButton.Size = new System.Drawing.Size(97, 22);
            this.initializeToolStripButton.Text = "Initialize New";
            this.initializeToolStripButton.ToolTipText = "Initialize the configuration control for a new activity.";
            this.initializeToolStripButton.Click += new System.EventHandler(this.initializeToolStripButton_Click);
            // 
            // loadToolStripButton
            // 
            this.loadToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("loadToolStripButton.Image")));
            this.loadToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadToolStripButton.Name = "loadToolStripButton";
            this.loadToolStripButton.Size = new System.Drawing.Size(96, 22);
            this.loadToolStripButton.Text = "Load Existing";
            this.loadToolStripButton.ToolTipText = "Initialize the configuration control by loading the selected configuration data.";
            this.loadToolStripButton.Click += new System.EventHandler(this.loadToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(128, 22);
            this.saveToolStripButton.Text = "Save Configuration";
            this.saveToolStripButton.ToolTipText = "Retrieve the configuration from the configuration control.";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // validateToolStripButton
            // 
            this.validateToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("validateToolStripButton.Image")));
            this.validateToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.validateToolStripButton.Name = "validateToolStripButton";
            this.validateToolStripButton.Size = new System.Drawing.Size(68, 22);
            this.validateToolStripButton.Text = "Validate";
            this.validateToolStripButton.ToolTipText = "Validate the configuration data and display a list of any errors.";
            this.validateToolStripButton.Click += new System.EventHandler(this.validateToolStripButton_Click);
            // 
            // resetConfigurationToolStripButton
            // 
            this.resetConfigurationToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("resetConfigurationToolStripButton.Image")));
            this.resetConfigurationToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetConfigurationToolStripButton.Name = "resetConfigurationToolStripButton";
            this.resetConfigurationToolStripButton.Size = new System.Drawing.Size(55, 22);
            this.resetConfigurationToolStripButton.Text = "Reset";
            this.resetConfigurationToolStripButton.ToolTipText = "Dispose of the configuration control and reinitialize with a new instance.";
            this.resetConfigurationToolStripButton.Click += new System.EventHandler(this.resetConfigurationToolStripButton_Click);
            // 
            // unsavedChanges_ToolStripButton
            // 
            this.unsavedChanges_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.unsavedChanges_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("unsavedChanges_ToolStripButton.Image")));
            this.unsavedChanges_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.unsavedChanges_ToolStripButton.Name = "unsavedChanges_ToolStripButton";
            this.unsavedChanges_ToolStripButton.Size = new System.Drawing.Size(135, 22);
            this.unsavedChanges_ToolStripButton.Text = "Clear Unsaved Changes";
            this.unsavedChanges_ToolStripButton.Click += new System.EventHandler(this.unsavedChanges_ToolStripButton_Click);
            // 
            // pluginExecutionTabPage
            // 
            this.pluginExecutionTabPage.Controls.Add(this.pluginExecutionSplitContainer);
            this.pluginExecutionTabPage.Controls.Add(this.pluginExecutionToolStrip);
            this.pluginExecutionTabPage.Location = new System.Drawing.Point(4, 28);
            this.pluginExecutionTabPage.Name = "pluginExecutionTabPage";
            this.pluginExecutionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.pluginExecutionTabPage.Size = new System.Drawing.Size(782, 667);
            this.pluginExecutionTabPage.TabIndex = 2;
            this.pluginExecutionTabPage.Text = "Plugin Execution";
            // 
            // pluginExecutionSplitContainer
            // 
            this.pluginExecutionSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginExecutionSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.pluginExecutionSplitContainer.Location = new System.Drawing.Point(3, 29);
            this.pluginExecutionSplitContainer.Name = "pluginExecutionSplitContainer";
            this.pluginExecutionSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pluginExecutionSplitContainer.Panel1
            // 
            this.pluginExecutionSplitContainer.Panel1.AutoScroll = true;
            this.pluginExecutionSplitContainer.Panel1.Controls.Add(this.pluginExecutionPanel);
            // 
            // pluginExecutionSplitContainer.Panel2
            // 
            this.pluginExecutionSplitContainer.Panel2.Controls.Add(this.pluginExecutionResultsTextBox);
            this.pluginExecutionSplitContainer.Panel2.Controls.Add(this.pluginExecutionResultsToolStrip);
            this.pluginExecutionSplitContainer.Size = new System.Drawing.Size(776, 635);
            this.pluginExecutionSplitContainer.SplitterDistance = 474;
            this.pluginExecutionSplitContainer.TabIndex = 0;
            // 
            // pluginExecutionPanel
            // 
            this.pluginExecutionPanel.AutoSize = true;
            this.pluginExecutionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pluginExecutionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginExecutionPanel.Location = new System.Drawing.Point(0, 0);
            this.pluginExecutionPanel.Name = "pluginExecutionPanel";
            this.pluginExecutionPanel.Size = new System.Drawing.Size(776, 474);
            this.pluginExecutionPanel.TabIndex = 2;
            // 
            // pluginExecutionResultsTextBox
            // 
            this.pluginExecutionResultsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginExecutionResultsTextBox.Location = new System.Drawing.Point(0, 25);
            this.pluginExecutionResultsTextBox.Multiline = true;
            this.pluginExecutionResultsTextBox.Name = "pluginExecutionResultsTextBox";
            this.pluginExecutionResultsTextBox.ReadOnly = true;
            this.pluginExecutionResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.pluginExecutionResultsTextBox.Size = new System.Drawing.Size(776, 132);
            this.pluginExecutionResultsTextBox.TabIndex = 1;
            // 
            // pluginExecutionResultsToolStrip
            // 
            this.pluginExecutionResultsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.pluginExecutionResultsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearResultToolStripButton});
            this.pluginExecutionResultsToolStrip.Location = new System.Drawing.Point(0, 0);
            this.pluginExecutionResultsToolStrip.Name = "pluginExecutionResultsToolStrip";
            this.pluginExecutionResultsToolStrip.Size = new System.Drawing.Size(776, 25);
            this.pluginExecutionResultsToolStrip.TabIndex = 0;
            this.pluginExecutionResultsToolStrip.Text = "toolStrip1";
            // 
            // clearResultToolStripButton
            // 
            this.clearResultToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("clearResultToolStripButton.Image")));
            this.clearResultToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearResultToolStripButton.Name = "clearResultToolStripButton";
            this.clearResultToolStripButton.Size = new System.Drawing.Size(54, 22);
            this.clearResultToolStripButton.Text = "Clear";
            this.clearResultToolStripButton.Click += new System.EventHandler(this.clearResultToolStripButton_Click);
            // 
            // pluginExecutionToolStrip
            // 
            this.pluginExecutionToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.pluginExecutionToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeToolStripButton,
            this.iterationsNumericUpDown,
            this.iterationCountToolStripLabel,
            this.toolStripSeparator3,
            this.resetExecutionToolStripButton,
            toolStripSeparator2,
            this.criticalSectionToolStripButton,
            this.dataLoggerToolStripButton});
            this.pluginExecutionToolStrip.Location = new System.Drawing.Point(3, 3);
            this.pluginExecutionToolStrip.Name = "pluginExecutionToolStrip";
            this.pluginExecutionToolStrip.Size = new System.Drawing.Size(776, 26);
            this.pluginExecutionToolStrip.TabIndex = 0;
            this.pluginExecutionToolStrip.Text = "toolStrip1";
            // 
            // executeToolStripButton
            // 
            this.executeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("executeToolStripButton.Image")));
            this.executeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.executeToolStripButton.Name = "executeToolStripButton";
            this.executeToolStripButton.Size = new System.Drawing.Size(67, 23);
            this.executeToolStripButton.Text = "Execute";
            this.executeToolStripButton.ToolTipText = "Execute the plugin using the selected configuration data.";
            this.executeToolStripButton.Click += new System.EventHandler(this.executeToolStripButton_Click);
            // 
            // iterationsNumericUpDown
            // 
            this.iterationsNumericUpDown.Name = "iterationsNumericUpDown";
            this.iterationsNumericUpDown.Size = new System.Drawing.Size(41, 23);
            this.iterationsNumericUpDown.Text = "1";
            // 
            // iterationCountToolStripLabel
            // 
            this.iterationCountToolStripLabel.Name = "iterationCountToolStripLabel";
            this.iterationCountToolStripLabel.Size = new System.Drawing.Size(64, 23);
            this.iterationCountToolStripLabel.Text = "iteration(s)";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 26);
            // 
            // resetExecutionToolStripButton
            // 
            this.resetExecutionToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("resetExecutionToolStripButton.Image")));
            this.resetExecutionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetExecutionToolStripButton.Name = "resetExecutionToolStripButton";
            this.resetExecutionToolStripButton.Size = new System.Drawing.Size(55, 23);
            this.resetExecutionToolStripButton.Text = "Reset";
            this.resetExecutionToolStripButton.ToolTipText = "Dispose of the execution engine and reinitialize with a new instance.";
            this.resetExecutionToolStripButton.Click += new System.EventHandler(this.resetExecutionToolStripButton_Click);
            // 
            // criticalSectionToolStripButton
            // 
            this.criticalSectionToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("criticalSectionToolStripButton.Image")));
            this.criticalSectionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.criticalSectionToolStripButton.Name = "criticalSectionToolStripButton";
            this.criticalSectionToolStripButton.Size = new System.Drawing.Size(183, 23);
            this.criticalSectionToolStripButton.Text = "Critical Section Configuration";
            this.criticalSectionToolStripButton.Click += new System.EventHandler(this.criticalSectionToolStripButton_Click);
            // 
            // dataLoggerToolStripButton
            // 
            this.dataLoggerToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("dataLoggerToolStripButton.Image")));
            this.dataLoggerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dataLoggerToolStripButton.Name = "dataLoggerToolStripButton";
            this.dataLoggerToolStripButton.Size = new System.Drawing.Size(132, 23);
            this.dataLoggerToolStripButton.Text = "Data Logger Output";
            this.dataLoggerToolStripButton.Click += new System.EventHandler(this.dataLoggerToolStripButton_Click);
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 24);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.pluginConfigurationDataListView);
            this.mainSplitContainer.Panel1.Controls.Add(this.pluginDataToolStrip);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.label_PluginType);
            this.mainSplitContainer.Panel2.Controls.Add(this.mainTabControl);
            this.mainSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.mainSplitContainer.Size = new System.Drawing.Size(1008, 706);
            this.mainSplitContainer.SplitterDistance = 210;
            this.mainSplitContainer.TabIndex = 3;
            // 
            // pluginConfigurationDataListView
            // 
            this.pluginConfigurationDataListView.AllowUserToAddRows = false;
            this.pluginConfigurationDataListView.AllowUserToResizeColumns = false;
            this.pluginConfigurationDataListView.AllowUserToResizeRows = false;
            this.pluginConfigurationDataListView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.pluginConfigurationDataListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pluginConfigurationDataListView.ColumnHeadersVisible = false;
            this.pluginConfigurationDataListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.configurationDataGridViewColumn});
            this.pluginConfigurationDataListView.ContextMenuStrip = this.pluginConfigurationDataContextMenuStrip;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.pluginConfigurationDataListView.DefaultCellStyle = dataGridViewCellStyle2;
            this.pluginConfigurationDataListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginConfigurationDataListView.Location = new System.Drawing.Point(0, 25);
            this.pluginConfigurationDataListView.Name = "pluginConfigurationDataListView";
            this.pluginConfigurationDataListView.ReadOnly = true;
            this.pluginConfigurationDataListView.RowHeadersVisible = false;
            this.pluginConfigurationDataListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.pluginConfigurationDataListView.Size = new System.Drawing.Size(206, 677);
            this.pluginConfigurationDataListView.TabIndex = 1;
            this.pluginConfigurationDataListView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.pluginConfigurationDataListView_CellContentDoubleClick);
            this.pluginConfigurationDataListView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.pluginConfigurationDataListView_CellMouseDown);
            // 
            // configurationDataGridViewColumn
            // 
            this.configurationDataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.configurationDataGridViewColumn.DataPropertyName = "DisplayText";
            this.configurationDataGridViewColumn.HeaderText = "Configuration Data";
            this.configurationDataGridViewColumn.Name = "configurationDataGridViewColumn";
            this.configurationDataGridViewColumn.ReadOnly = true;
            this.configurationDataGridViewColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pluginConfigurationDataContextMenuStrip
            // 
            this.pluginConfigurationDataContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMetadataContextMenuItem,
            this.toolStripSeparator,
            this.renameConfigurationDataContextMenuItem,
            this.exportConfigurationDataContextMenuItem,
            this.deleteConfigurationDataContextMenuItem});
            this.pluginConfigurationDataContextMenuStrip.Name = "pluginConfigurationDataContextMenuStrip";
            this.pluginConfigurationDataContextMenuStrip.Size = new System.Drawing.Size(153, 98);
            this.pluginConfigurationDataContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.pluginConfigurationDataContextMenuStrip_Opening);
            // 
            // viewMetadataContextMenuItem
            // 
            this.viewMetadataContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("viewMetadataContextMenuItem.Image")));
            this.viewMetadataContextMenuItem.Name = "viewMetadataContextMenuItem";
            this.viewMetadataContextMenuItem.Size = new System.Drawing.Size(152, 22);
            this.viewMetadataContextMenuItem.Text = "View Metadata";
            this.viewMetadataContextMenuItem.Click += new System.EventHandler(this.viewMetadataContextMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // renameConfigurationDataContextMenuItem
            // 
            this.renameConfigurationDataContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameConfigurationDataContextMenuItem.Image")));
            this.renameConfigurationDataContextMenuItem.Name = "renameConfigurationDataContextMenuItem";
            this.renameConfigurationDataContextMenuItem.Size = new System.Drawing.Size(152, 22);
            this.renameConfigurationDataContextMenuItem.Text = "Rename";
            this.renameConfigurationDataContextMenuItem.Click += new System.EventHandler(this.renameConfigurationDataContextMenuItem_Click);
            // 
            // exportConfigurationDataContextMenuItem
            // 
            this.exportConfigurationDataContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportConfigurationDataContextMenuItem.Image")));
            this.exportConfigurationDataContextMenuItem.Name = "exportConfigurationDataContextMenuItem";
            this.exportConfigurationDataContextMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportConfigurationDataContextMenuItem.Text = "Export";
            this.exportConfigurationDataContextMenuItem.Click += new System.EventHandler(this.exportConfigurationDataContextMenuItem_Click);
            // 
            // deleteConfigurationDataContextMenuItem
            // 
            this.deleteConfigurationDataContextMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteConfigurationDataContextMenuItem.Image")));
            this.deleteConfigurationDataContextMenuItem.Name = "deleteConfigurationDataContextMenuItem";
            this.deleteConfigurationDataContextMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteConfigurationDataContextMenuItem.Text = "Delete";
            this.deleteConfigurationDataContextMenuItem.Click += new System.EventHandler(this.deleteConfigurationDataContextMenuItem_Click);
            // 
            // pluginDataToolStrip
            // 
            this.pluginDataToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.pluginDataToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importConfigurationDataToolStripButton,
            this.exportConfigurationDataToolStripButton,
            this.clearConfigurationDataToolStripButton});
            this.pluginDataToolStrip.Location = new System.Drawing.Point(0, 0);
            this.pluginDataToolStrip.Name = "pluginDataToolStrip";
            this.pluginDataToolStrip.Size = new System.Drawing.Size(206, 25);
            this.pluginDataToolStrip.TabIndex = 0;
            this.pluginDataToolStrip.Text = "toolStrip2";
            // 
            // importConfigurationDataToolStripButton
            // 
            this.importConfigurationDataToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("importConfigurationDataToolStripButton.Image")));
            this.importConfigurationDataToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importConfigurationDataToolStripButton.Name = "importConfigurationDataToolStripButton";
            this.importConfigurationDataToolStripButton.Size = new System.Drawing.Size(63, 22);
            this.importConfigurationDataToolStripButton.Text = "Import";
            this.importConfigurationDataToolStripButton.ToolTipText = "Import configuration data from a file.";
            this.importConfigurationDataToolStripButton.Click += new System.EventHandler(this.importConfigurationDataToolStripButton_Click);
            // 
            // exportConfigurationDataToolStripButton
            // 
            this.exportConfigurationDataToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("exportConfigurationDataToolStripButton.Image")));
            this.exportConfigurationDataToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportConfigurationDataToolStripButton.Name = "exportConfigurationDataToolStripButton";
            this.exportConfigurationDataToolStripButton.Size = new System.Drawing.Size(60, 22);
            this.exportConfigurationDataToolStripButton.Text = "Export";
            this.exportConfigurationDataToolStripButton.ToolTipText = "Export the selected configuration data to a file.";
            this.exportConfigurationDataToolStripButton.Click += new System.EventHandler(this.exportConfigurationDataToolStripButton_Click);
            // 
            // clearConfigurationDataToolStripButton
            // 
            this.clearConfigurationDataToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("clearConfigurationDataToolStripButton.Image")));
            this.clearConfigurationDataToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearConfigurationDataToolStripButton.Name = "clearConfigurationDataToolStripButton";
            this.clearConfigurationDataToolStripButton.Size = new System.Drawing.Size(54, 22);
            this.clearConfigurationDataToolStripButton.Text = "Clear";
            this.clearConfigurationDataToolStripButton.ToolTipText = "Remove all loaded configuration data.";
            this.clearConfigurationDataToolStripButton.Click += new System.EventHandler(this.clearConfigurationDataToolStripButton_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(125, 20);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Plugin Data File|*.plugindata|All Files|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Plugin Data Files|*.plugindata|All Files|*.*";
            // 
            // executionBackgroundWorker
            // 
            this.executionBackgroundWorker.WorkerReportsProgress = true;
            this.executionBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.executionBackgroundWorker_DoWork);
            this.executionBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.executionBackgroundWorker_ProgressChanged);
            this.executionBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.executionBackgroundWorker_RunWorkerCompleted);
            // 
            // label_PluginType
            // 
            this.label_PluginType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PluginType.Location = new System.Drawing.Point(407, 4);
            this.label_PluginType.Name = "label_PluginType";
            this.label_PluginType.Size = new System.Drawing.Size(376, 21);
            this.label_PluginType.TabIndex = 3;
            this.label_PluginType.Text = "Plugin Type";
            this.label_PluginType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PluginFrameworkSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "PluginFrameworkSimulatorForm";
            this.Text = "Plugin Framework Simulator";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.pluginConfigurationTabPage.ResumeLayout(false);
            this.pluginConfigurationTabPage.PerformLayout();
            this.pluginConfigurationToolStrip.ResumeLayout(false);
            this.pluginConfigurationToolStrip.PerformLayout();
            this.pluginExecutionTabPage.ResumeLayout(false);
            this.pluginExecutionTabPage.PerformLayout();
            this.pluginExecutionSplitContainer.Panel1.ResumeLayout(false);
            this.pluginExecutionSplitContainer.Panel1.PerformLayout();
            this.pluginExecutionSplitContainer.Panel2.ResumeLayout(false);
            this.pluginExecutionSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pluginExecutionSplitContainer)).EndInit();
            this.pluginExecutionSplitContainer.ResumeLayout(false);
            this.pluginExecutionResultsToolStrip.ResumeLayout(false);
            this.pluginExecutionResultsToolStrip.PerformLayout();
            this.pluginExecutionToolStrip.ResumeLayout(false);
            this.pluginExecutionToolStrip.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel1.PerformLayout();
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pluginConfigurationDataListView)).EndInit();
            this.pluginConfigurationDataContextMenuStrip.ResumeLayout(false);
            this.pluginDataToolStrip.ResumeLayout(false);
            this.pluginDataToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage pluginConfigurationTabPage;
        private System.Windows.Forms.TabPage pluginExecutionTabPage;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.ToolStrip pluginConfigurationToolStrip;
        private System.Windows.Forms.ToolStrip pluginExecutionToolStrip;
        private System.Windows.Forms.Panel pluginConfigurationPanel;
        private System.Windows.Forms.SplitContainer pluginExecutionSplitContainer;
        private System.Windows.Forms.ToolStrip pluginDataToolStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton initializeToolStripButton;
        private System.Windows.Forms.ToolStripButton executeToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton loadToolStripButton;
        private System.Windows.Forms.ToolStripButton validateToolStripButton;
        private System.Windows.Forms.DataGridView pluginConfigurationDataListView;
        private System.Windows.Forms.DataGridViewTextBoxColumn configurationDataGridViewColumn;
        private System.Windows.Forms.ToolStripButton exportConfigurationDataToolStripButton;
        private System.Windows.Forms.ToolStripButton importConfigurationDataToolStripButton;
        private System.Windows.Forms.ToolStripButton clearConfigurationDataToolStripButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ContextMenuStrip pluginConfigurationDataContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem viewMetadataContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportConfigurationDataContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteConfigurationDataContextMenuItem;
        private System.Windows.Forms.ToolStripButton resetConfigurationToolStripButton;
        private System.Windows.Forms.ToolStripButton resetExecutionToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton unsavedChanges_ToolStripButton;
        private System.Windows.Forms.ToolStripButton criticalSectionToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem renameConfigurationDataContextMenuItem;
        private System.Windows.Forms.ToolStripButton dataLoggerToolStripButton;
        private System.Windows.Forms.TextBox pluginExecutionResultsTextBox;
        private System.Windows.Forms.ToolStrip pluginExecutionResultsToolStrip;
        private System.Windows.Forms.Panel pluginExecutionPanel;
        private System.ComponentModel.BackgroundWorker executionBackgroundWorker;
        private HP.ScalableTest.Development.UI.PluginFrameworkSimulatorForm.ToolStripNumericUpDown iterationsNumericUpDown;
        private System.Windows.Forms.ToolStripLabel iterationCountToolStripLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton clearResultToolStripButton;
        private System.Windows.Forms.Label label_PluginType;
    }
}
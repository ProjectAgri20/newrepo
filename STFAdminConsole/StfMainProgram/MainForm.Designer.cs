﻿namespace HP.ScalableTest.LabConsole
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
            if (disposing && _enterpriseTestUIController != null)
            {
                _enterpriseTestUIController.Dispose();
                _enterpriseTestUIController = null;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Epr"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "PerfMon"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ePrint"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "VCenter")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.file_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logIn_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.importScenario_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.exit_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetUsage_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machineGroupView_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.session_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageSessionData_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.sessionReports_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionWebUIReports_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configuration_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vmInventory_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulatorMgt_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.serverResourceCategory_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverInventory_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printServers_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ePrintServers_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.printDrivers_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDriverConfigurations_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorConfig_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perfMonCounters_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.products_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerProducts_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.citrixPublishedApps_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administration_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemSettings_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginReferences_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activityPluginSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProductPlugintoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.userManagement_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userGroup_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activieDirectoryGroup_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.vmPlatformAssoc_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machineGroup_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.softwareInstallers_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installerPackages_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.help_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.about_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.configurationTabPage = new System.Windows.Forms.TabPage();
            this.scenarioConfiguration_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.scenarioConfigurationTreeView = new HP.ScalableTest.UI.ScenarioConfiguration.EnterpriseScenarioTreeView();
            this.configTreeViewSearch_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.searchText_toolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.searchNext_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.searchPrev_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.configurationTreeView_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.newItem_DropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.newFolder_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newScenario_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.newFolderAtRoot_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newScenarioAtRoot_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delete_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.configurationTreeRefresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.resource_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.save_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.resource_ToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.analyze_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.executeScenario_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dataEditPanel = new System.Windows.Forms.Panel();
            this.dashboard_TabPage = new System.Windows.Forms.TabPage();
            this.execution_TabPage = new System.Windows.Forms.TabPage();
            this.connectedSessionsControl = new HP.ScalableTest.UI.SessionExecution.ConnectedSessionsControl();
            this.reports_TabPage = new System.Windows.Forms.TabPage();
            this.graphingUserControl = new HP.ScalableTest.UI.Charting.GraphingUserControl();
            this.main_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.userName_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.version_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.environment_StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenuStrip.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.configurationTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfiguration_SplitContainer)).BeginInit();
            this.scenarioConfiguration_SplitContainer.Panel1.SuspendLayout();
            this.scenarioConfiguration_SplitContainer.Panel2.SuspendLayout();
            this.scenarioConfiguration_SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfigurationTreeView)).BeginInit();
            this.configTreeViewSearch_ToolStrip.SuspendLayout();
            this.configurationTreeView_ToolStrip.SuspendLayout();
            this.resource_ToolStrip.SuspendLayout();
            this.execution_TabPage.SuspendLayout();
            this.reports_TabPage.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.BackColor = System.Drawing.SystemColors.MenuBar;
            this.mainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_MenuItem,
            this.view_MenuItem,
            this.session_MenuItem,
            this.configuration_MenuItem,
            this.administration_MenuItem,
            this.help_MenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.mainMenuStrip.Size = new System.Drawing.Size(1057, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // file_MenuItem
            // 
            this.file_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logIn_MenuItem,
            this.toolStripSeparator2,
            this.importScenario_MenuItem,
            this.toolStripSeparator11,
            this.exit_MenuItem});
            this.file_MenuItem.Name = "file_MenuItem";
            this.file_MenuItem.Size = new System.Drawing.Size(37, 20);
            this.file_MenuItem.Text = "File";
            // 
            // logIn_MenuItem
            // 
            this.logIn_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("logIn_MenuItem.Image")));
            this.logIn_MenuItem.Name = "logIn_MenuItem";
            this.logIn_MenuItem.Size = new System.Drawing.Size(208, 26);
            this.logIn_MenuItem.Text = "Connect to Environment";
            this.logIn_MenuItem.Click += new System.EventHandler(this.logIn_MenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // importScenario_MenuItem
            // 
            this.importScenario_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importScenario_MenuItem.Image")));
            this.importScenario_MenuItem.Name = "importScenario_MenuItem";
            this.importScenario_MenuItem.Size = new System.Drawing.Size(208, 26);
            this.importScenario_MenuItem.Text = "Import Scenario";
            this.importScenario_MenuItem.Click += new System.EventHandler(this.importScenarioMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(205, 6);
            // 
            // exit_MenuItem
            // 
            this.exit_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exit_MenuItem.Image")));
            this.exit_MenuItem.Name = "exit_MenuItem";
            this.exit_MenuItem.Size = new System.Drawing.Size(208, 26);
            this.exit_MenuItem.Text = "Exit";
            this.exit_MenuItem.Click += new System.EventHandler(this.exit_MenuItem_Click);
            // 
            // view_MenuItem
            // 
            this.view_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assetUsage_MenuItem,
            this.machineGroupView_MenuItem});
            this.view_MenuItem.Name = "view_MenuItem";
            this.view_MenuItem.Size = new System.Drawing.Size(44, 20);
            this.view_MenuItem.Text = "View";
            // 
            // assetUsage_MenuItem
            // 
            this.assetUsage_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("assetUsage_MenuItem.Image")));
            this.assetUsage_MenuItem.Name = "assetUsage_MenuItem";
            this.assetUsage_MenuItem.Size = new System.Drawing.Size(231, 26);
            this.assetUsage_MenuItem.Text = "Device Usage Summary";
            this.assetUsage_MenuItem.Click += new System.EventHandler(this.assetUsage_MenuItem_Click);
            // 
            // machineGroupView_MenuItem
            // 
            this.machineGroupView_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("machineGroupView_MenuItem.Image")));
            this.machineGroupView_MenuItem.Name = "machineGroupView_MenuItem";
            this.machineGroupView_MenuItem.Size = new System.Drawing.Size(231, 26);
            this.machineGroupView_MenuItem.Text = "Machine Group Assignments";
            // 
            // session_MenuItem
            // 
            this.session_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageSessionData_MenuItem,
            this.toolStripSeparator3,
            this.sessionReports_MenuItem,
            this.sessionWebUIReports_MenuItem});
            this.session_MenuItem.Name = "session_MenuItem";
            this.session_MenuItem.Size = new System.Drawing.Size(58, 20);
            this.session_MenuItem.Text = "Session";
            // 
            // manageSessionData_MenuItem
            // 
            this.manageSessionData_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("manageSessionData_MenuItem.Image")));
            this.manageSessionData_MenuItem.Name = "manageSessionData_MenuItem";
            this.manageSessionData_MenuItem.Size = new System.Drawing.Size(201, 26);
            this.manageSessionData_MenuItem.Text = "Manage Session Data";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(198, 6);
            // 
            // sessionReports_MenuItem
            // 
            this.sessionReports_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sessionReports_MenuItem.Image")));
            this.sessionReports_MenuItem.Name = "sessionReports_MenuItem";
            this.sessionReports_MenuItem.Size = new System.Drawing.Size(201, 26);
            this.sessionReports_MenuItem.Text = "Session Reports";
            // 
            // sessionWebUIReports_MenuItem
            // 
            this.sessionWebUIReports_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sessionWebUIReports_MenuItem.Image")));
            this.sessionWebUIReports_MenuItem.Name = "sessionWebUIReports_MenuItem";
            this.sessionWebUIReports_MenuItem.Size = new System.Drawing.Size(201, 26);
            this.sessionWebUIReports_MenuItem.Text = "Session Web UI Reports";
            // 
            // configuration_MenuItem
            // 
            this.configuration_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vmInventory_MenuItem,
            this.simulatorMgt_MenuItem,
            this.toolStripSeparator9,
            this.serverResourceCategory_MenuItem,
            this.serverInventory_MenuItem,
            this.printServers_MenuItem,
            this.ePrintServers_MenuItem,
            this.toolStripSeparator4,
            this.printDrivers_MenuItem,
            this.printDriverConfigurations_MenuItem,
            this.monitorConfig_MenuItem,
            this.perfMonCounters_MenuItem,
            this.toolStripSeparator1,
            this.products_MenuItem,
            this.printerProducts_MenuItem,
            this.citrixPublishedApps_MenuItem});
            this.configuration_MenuItem.Name = "configuration_MenuItem";
            this.configuration_MenuItem.Size = new System.Drawing.Size(93, 20);
            this.configuration_MenuItem.Text = "Configuration";
            this.configuration_MenuItem.Visible = false;
            // 
            // vmInventory_MenuItem
            // 
            this.vmInventory_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("vmInventory_MenuItem.Image")));
            this.vmInventory_MenuItem.Name = "vmInventory_MenuItem";
            this.vmInventory_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.vmInventory_MenuItem.Text = "Machine Inventory";
            // 
            // simulatorMgt_MenuItem
            // 
            this.simulatorMgt_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("simulatorMgt_MenuItem.Image")));
            this.simulatorMgt_MenuItem.Name = "simulatorMgt_MenuItem";
            this.simulatorMgt_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.simulatorMgt_MenuItem.Text = "Simulator Management";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(217, 6);
            // 
            // serverResourceCategory_MenuItem
            // 
            this.serverResourceCategory_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("serverResourceCategory_MenuItem.Image")));
            this.serverResourceCategory_MenuItem.Name = "serverResourceCategory_MenuItem";
            this.serverResourceCategory_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.serverResourceCategory_MenuItem.Text = "Server Resource Category";
            // 
            // serverInventory_MenuItem
            // 
            this.serverInventory_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("serverInventory_MenuItem.Image")));
            this.serverInventory_MenuItem.Name = "serverInventory_MenuItem";
            this.serverInventory_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.serverInventory_MenuItem.Text = "Infrastructure Servers";
            // 
            // printServers_MenuItem
            // 
            this.printServers_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printServers_MenuItem.Image")));
            this.printServers_MenuItem.Name = "printServers_MenuItem";
            this.printServers_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.printServers_MenuItem.Tag = "";
            this.printServers_MenuItem.Text = "Print Servers";
            // 
            // ePrintServers_MenuItem
            // 
            this.ePrintServers_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ePrintServers_MenuItem.Image")));
            this.ePrintServers_MenuItem.Name = "ePrintServers_MenuItem";
            this.ePrintServers_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.ePrintServers_MenuItem.Tag = "ePrint";
            this.ePrintServers_MenuItem.Text = "ePrint Servers";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(217, 6);
            // 
            // printDrivers_MenuItem
            // 
            this.printDrivers_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printDrivers_MenuItem.Image")));
            this.printDrivers_MenuItem.Name = "printDrivers_MenuItem";
            this.printDrivers_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.printDrivers_MenuItem.Text = "Print Drivers";
            // 
            // printDriverConfigurations_MenuItem
            // 
            this.printDriverConfigurations_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printDriverConfigurations_MenuItem.Image")));
            this.printDriverConfigurations_MenuItem.Name = "printDriverConfigurations_MenuItem";
            this.printDriverConfigurations_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.printDriverConfigurations_MenuItem.Text = "Print Driver Configuration";
            // 
            // monitorConfig_MenuItem
            // 
            this.monitorConfig_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("monitorConfig_MenuItem.Image")));
            this.monitorConfig_MenuItem.Name = "monitorConfig_MenuItem";
            this.monitorConfig_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.monitorConfig_MenuItem.Text = "STF Monitor Configuration";
            // 
            // perfMonCounters_MenuItem
            // 
            this.perfMonCounters_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("perfMonCounters_MenuItem.Image")));
            this.perfMonCounters_MenuItem.Name = "perfMonCounters_MenuItem";
            this.perfMonCounters_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.perfMonCounters_MenuItem.Text = "PerfMon Counters";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // products_MenuItem
            // 
            this.products_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("products_MenuItem.Image")));
            this.products_MenuItem.Name = "products_MenuItem";
            this.products_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.products_MenuItem.Text = "Products and Solutions";
            this.products_MenuItem.Visible = false;
            // 
            // printerProducts_MenuItem
            // 
            this.printerProducts_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printerProducts_MenuItem.Image")));
            this.printerProducts_MenuItem.Name = "printerProducts_MenuItem";
            this.printerProducts_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.printerProducts_MenuItem.Text = "Printer Products";
            // 
            // citrixPublishedApps_MenuItem
            // 
            this.citrixPublishedApps_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("citrixPublishedApps_MenuItem.Image")));
            this.citrixPublishedApps_MenuItem.Name = "citrixPublishedApps_MenuItem";
            this.citrixPublishedApps_MenuItem.Size = new System.Drawing.Size(220, 26);
            this.citrixPublishedApps_MenuItem.Text = "Citrix Published Apps";
            // 
            // administration_MenuItem
            // 
            this.administration_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemSettings_MenuItem,
            this.toolStripSeparator12,
            this.pluginReferences_MenuItem,
            this.activityPluginSettingsToolStripMenuItem,
            this.ProductPlugintoolStripMenuItem,
            this.toolStripSeparator5,
            this.userManagement_MenuItem,
            this.userGroup_MenuItem,
            this.activieDirectoryGroup_MenuItem,
            this.toolStripSeparator7,
            this.vmPlatformAssoc_MenuItem,
            this.machineGroup_MenuItem,
            this.toolStripSeparator8,
            this.softwareInstallers_MenuItem,
            this.installerPackages_MenuItem});
            this.administration_MenuItem.Name = "administration_MenuItem";
            this.administration_MenuItem.Size = new System.Drawing.Size(98, 20);
            this.administration_MenuItem.Text = "Administration";
            this.administration_MenuItem.Visible = false;
            // 
            // systemSettings_MenuItem
            // 
            this.systemSettings_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("systemSettings_MenuItem.Image")));
            this.systemSettings_MenuItem.Name = "systemSettings_MenuItem";
            this.systemSettings_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.systemSettings_MenuItem.Text = "Global System Settings";
            this.systemSettings_MenuItem.ToolTipText = "Add/Edit/Remove System Settings";
            this.systemSettings_MenuItem.Click += new System.EventHandler(this.systemSettings_MenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(229, 6);
            // 
            // pluginReferences_MenuItem
            // 
            this.pluginReferences_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pluginReferences_MenuItem.Image")));
            this.pluginReferences_MenuItem.Name = "pluginReferences_MenuItem";
            this.pluginReferences_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.pluginReferences_MenuItem.Text = "Activity Plugin References";
            this.pluginReferences_MenuItem.ToolTipText = "Add/Edit/Remove Virtual Resource Plugin References";
            // 
            // activityPluginSettingsToolStripMenuItem
            // 
            this.activityPluginSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("activityPluginSettingsToolStripMenuItem.Image")));
            this.activityPluginSettingsToolStripMenuItem.Name = "activityPluginSettingsToolStripMenuItem";
            this.activityPluginSettingsToolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.activityPluginSettingsToolStripMenuItem.Text = "Activity Plugin Settings";
            this.activityPluginSettingsToolStripMenuItem.Click += new System.EventHandler(this.pluginSettingsMenuItem_Click);
            // 
            // ProductPlugintoolStripMenuItem
            // 
            this.ProductPlugintoolStripMenuItem.Name = "ProductPlugintoolStripMenuItem";
            this.ProductPlugintoolStripMenuItem.Size = new System.Drawing.Size(232, 26);
            this.ProductPlugintoolStripMenuItem.Text = "Product Plugin Association";
            this.ProductPlugintoolStripMenuItem.Click += new System.EventHandler(this.ProductPlugintoolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(229, 6);
            // 
            // userManagement_MenuItem
            // 
            this.userManagement_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("userManagement_MenuItem.Image")));
            this.userManagement_MenuItem.Name = "userManagement_MenuItem";
            this.userManagement_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.userManagement_MenuItem.Text = "STF Users";
            // 
            // userGroup_MenuItem
            // 
            this.userGroup_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("userGroup_MenuItem.Image")));
            this.userGroup_MenuItem.Name = "userGroup_MenuItem";
            this.userGroup_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.userGroup_MenuItem.Text = "STF User Groups";
            // 
            // activieDirectoryGroup_MenuItem
            // 
            this.activieDirectoryGroup_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("activieDirectoryGroup_MenuItem.Image")));
            this.activieDirectoryGroup_MenuItem.Name = "activieDirectoryGroup_MenuItem";
            this.activieDirectoryGroup_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.activieDirectoryGroup_MenuItem.Text = "Active Directory Groups";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(229, 6);
            // 
            // vmPlatformAssoc_MenuItem
            // 
            this.vmPlatformAssoc_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("vmPlatformAssoc_MenuItem.Image")));
            this.vmPlatformAssoc_MenuItem.Name = "vmPlatformAssoc_MenuItem";
            this.vmPlatformAssoc_MenuItem.ShowShortcutKeys = false;
            this.vmPlatformAssoc_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.vmPlatformAssoc_MenuItem.Text = "Virtual Machine Platforms";
            // 
            // machineGroup_MenuItem
            // 
            this.machineGroup_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("machineGroup_MenuItem.Image")));
            this.machineGroup_MenuItem.Name = "machineGroup_MenuItem";
            this.machineGroup_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.machineGroup_MenuItem.Text = "Virtual Machine Assignments";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(229, 6);
            // 
            // softwareInstallers_MenuItem
            // 
            this.softwareInstallers_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("softwareInstallers_MenuItem.Image")));
            this.softwareInstallers_MenuItem.Name = "softwareInstallers_MenuItem";
            this.softwareInstallers_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.softwareInstallers_MenuItem.Text = "Software Installers";
            // 
            // installerPackages_MenuItem
            // 
            this.installerPackages_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("installerPackages_MenuItem.Image")));
            this.installerPackages_MenuItem.Name = "installerPackages_MenuItem";
            this.installerPackages_MenuItem.Size = new System.Drawing.Size(232, 26);
            this.installerPackages_MenuItem.Text = "Software Installer Packages";
            // 
            // help_MenuItem
            // 
            this.help_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.about_MenuItem});
            this.help_MenuItem.Name = "help_MenuItem";
            this.help_MenuItem.Size = new System.Drawing.Size(44, 20);
            this.help_MenuItem.Text = "Help";
            // 
            // about_MenuItem
            // 
            this.about_MenuItem.Image = ((System.Drawing.Image)(resources.GetObject("about_MenuItem.Image")));
            this.about_MenuItem.Name = "about_MenuItem";
            this.about_MenuItem.Size = new System.Drawing.Size(253, 26);
            this.about_MenuItem.Text = "About STE Management Console";
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.configurationTabPage);
            this.mainTabControl.Controls.Add(this.dashboard_TabPage);
            this.mainTabControl.Controls.Add(this.execution_TabPage);
            this.mainTabControl.Controls.Add(this.reports_TabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(0, 24);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1057, 675);
            this.mainTabControl.TabIndex = 9;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
            // 
            // configurationTabPage
            // 
            this.configurationTabPage.Controls.Add(this.scenarioConfiguration_SplitContainer);
            this.configurationTabPage.Location = new System.Drawing.Point(4, 24);
            this.configurationTabPage.Name = "configurationTabPage";
            this.configurationTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.configurationTabPage.Size = new System.Drawing.Size(1049, 647);
            this.configurationTabPage.TabIndex = 0;
            this.configurationTabPage.Text = "Scenario Configuration";
            // 
            // scenarioConfiguration_SplitContainer
            // 
            this.scenarioConfiguration_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scenarioConfiguration_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioConfiguration_SplitContainer.Location = new System.Drawing.Point(3, 3);
            this.scenarioConfiguration_SplitContainer.Name = "scenarioConfiguration_SplitContainer";
            // 
            // scenarioConfiguration_SplitContainer.Panel1
            // 
            this.scenarioConfiguration_SplitContainer.Panel1.Controls.Add(this.scenarioConfigurationTreeView);
            this.scenarioConfiguration_SplitContainer.Panel1.Controls.Add(this.configTreeViewSearch_ToolStrip);
            this.scenarioConfiguration_SplitContainer.Panel1.Controls.Add(this.configurationTreeView_ToolStrip);
            this.scenarioConfiguration_SplitContainer.Panel1MinSize = 75;
            // 
            // scenarioConfiguration_SplitContainer.Panel2
            // 
            this.scenarioConfiguration_SplitContainer.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.scenarioConfiguration_SplitContainer.Panel2.Controls.Add(this.resource_ToolStrip);
            this.scenarioConfiguration_SplitContainer.Panel2.Controls.Add(this.dataEditPanel);
            this.scenarioConfiguration_SplitContainer.Panel2MinSize = 698;
            this.scenarioConfiguration_SplitContainer.Size = new System.Drawing.Size(1043, 641);
            this.scenarioConfiguration_SplitContainer.SplitterDistance = 284;
            this.scenarioConfiguration_SplitContainer.TabIndex = 8;
            // 
            // scenarioConfigurationTreeView
            // 
            this.scenarioConfigurationTreeView.AllowDragDrop = true;
            this.scenarioConfigurationTreeView.AllowDrop = true;
            this.scenarioConfigurationTreeView.AllowRootScenarioCreation = false;
            this.scenarioConfigurationTreeView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.scenarioConfigurationTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioConfigurationTreeView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.scenarioConfigurationTreeView.Location = new System.Drawing.Point(0, 52);
            this.scenarioConfigurationTreeView.Name = "scenarioConfigurationTreeView";
            // 
            // 
            // 
            this.scenarioConfigurationTreeView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 52, 150, 250);
            this.scenarioConfigurationTreeView.Size = new System.Drawing.Size(282, 587);
            this.scenarioConfigurationTreeView.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.scenarioConfigurationTreeView.SpacingBetweenNodes = -1;
            this.scenarioConfigurationTreeView.TabIndex = 0;
            this.scenarioConfigurationTreeView.Text = "enterpriseScenarioTreeView1";
            this.scenarioConfigurationTreeView.ImportCompleted += new System.EventHandler(this.scenarioConfigurationTreeView_ImportCompleted);
            this.scenarioConfigurationTreeView.ConfigurationObjectSelectionChanging += new System.EventHandler<Telerik.WinControls.UI.RadTreeViewCancelEventArgs>(this.scenarioConfigurationTreeView_ConfigurationObjectSelectionChanging);
            this.scenarioConfigurationTreeView.ConfigurationObjectSelected += new System.EventHandler<HP.ScalableTest.UI.ScenarioConfiguration.ConfigurationTagEventArgs>(this.scenarioConfigurationTreeView_ConfigurationObjectSelected);
            // 
            // configTreeViewSearch_ToolStrip
            // 
            this.configTreeViewSearch_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.configTreeViewSearch_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.searchText_toolStripTextBox,
            this.searchNext_toolStripButton,
            this.searchPrev_toolStripButton});
            this.configTreeViewSearch_ToolStrip.Location = new System.Drawing.Point(0, 27);
            this.configTreeViewSearch_ToolStrip.Name = "configTreeViewSearch_ToolStrip";
            this.configTreeViewSearch_ToolStrip.Size = new System.Drawing.Size(282, 25);
            this.configTreeViewSearch_ToolStrip.TabIndex = 3;
            this.configTreeViewSearch_ToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel1.Text = "Search:";
            // 
            // searchText_toolStripTextBox
            // 
            this.searchText_toolStripTextBox.Name = "searchText_toolStripTextBox";
            this.searchText_toolStripTextBox.Size = new System.Drawing.Size(100, 25);
            // 
            // searchNext_toolStripButton
            // 
            this.searchNext_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchNext_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("searchNext_toolStripButton.Image")));
            this.searchNext_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchNext_toolStripButton.Name = "searchNext_toolStripButton";
            this.searchNext_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.searchNext_toolStripButton.Text = "toolStripButton1";
            this.searchNext_toolStripButton.ToolTipText = "Next";
            this.searchNext_toolStripButton.Click += new System.EventHandler(this.searchNext_toolStripButton_Click);
            // 
            // searchPrev_toolStripButton
            // 
            this.searchPrev_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchPrev_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("searchPrev_toolStripButton.Image")));
            this.searchPrev_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchPrev_toolStripButton.Name = "searchPrev_toolStripButton";
            this.searchPrev_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.searchPrev_toolStripButton.Text = "toolStripButton2";
            this.searchPrev_toolStripButton.ToolTipText = "Previous";
            this.searchPrev_toolStripButton.Click += new System.EventHandler(this.searchPrev_toolStripButton_Click);
            // 
            // configurationTreeView_ToolStrip
            // 
            this.configurationTreeView_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.configurationTreeView_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.configurationTreeView_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newItem_DropDownButton,
            this.delete_ToolStripButton,
            this.configurationTreeRefresh_ToolStripButton,
            this.toolStripSeparator10});
            this.configurationTreeView_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.configurationTreeView_ToolStrip.Name = "configurationTreeView_ToolStrip";
            this.configurationTreeView_ToolStrip.Size = new System.Drawing.Size(282, 27);
            this.configurationTreeView_ToolStrip.TabIndex = 2;
            this.configurationTreeView_ToolStrip.Text = "toolStrip1";
            // 
            // newItem_DropDownButton
            // 
            this.newItem_DropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFolder_ToolStripMenuItem,
            this.newScenario_ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.newFolderAtRoot_ToolStripMenuItem,
            this.newScenarioAtRoot_ToolStripMenuItem});
            this.newItem_DropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("newItem_DropDownButton.Image")));
            this.newItem_DropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newItem_DropDownButton.Name = "newItem_DropDownButton";
            this.newItem_DropDownButton.Size = new System.Drawing.Size(64, 24);
            this.newItem_DropDownButton.Text = "New";
            this.newItem_DropDownButton.ToolTipText = "Create new configuration objects";
            this.newItem_DropDownButton.DropDownOpening += new System.EventHandler(this.newItem_DropDownButton_DropDownOpening);
            // 
            // newFolder_ToolStripMenuItem
            // 
            this.newFolder_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newFolder_ToolStripMenuItem.Image")));
            this.newFolder_ToolStripMenuItem.Name = "newFolder_ToolStripMenuItem";
            this.newFolder_ToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this.newFolder_ToolStripMenuItem.Text = "Folder";
            this.newFolder_ToolStripMenuItem.Click += new System.EventHandler(this.newFolder_ToolStripMenuItem_Click);
            // 
            // newScenario_ToolStripMenuItem
            // 
            this.newScenario_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newScenario_ToolStripMenuItem.Image")));
            this.newScenario_ToolStripMenuItem.Name = "newScenario_ToolStripMenuItem";
            this.newScenario_ToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this.newScenario_ToolStripMenuItem.Text = "Scenario";
            this.newScenario_ToolStripMenuItem.Click += new System.EventHandler(this.newScenario_ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // newFolderAtRoot_ToolStripMenuItem
            // 
            this.newFolderAtRoot_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newFolderAtRoot_ToolStripMenuItem.Image")));
            this.newFolderAtRoot_ToolStripMenuItem.Name = "newFolderAtRoot_ToolStripMenuItem";
            this.newFolderAtRoot_ToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this.newFolderAtRoot_ToolStripMenuItem.Text = "Folder at Root";
            this.newFolderAtRoot_ToolStripMenuItem.Click += new System.EventHandler(this.newFolderAtRoot_ToolStripMenuItem_Click);
            // 
            // newScenarioAtRoot_ToolStripMenuItem
            // 
            this.newScenarioAtRoot_ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newScenarioAtRoot_ToolStripMenuItem.Image")));
            this.newScenarioAtRoot_ToolStripMenuItem.Name = "newScenarioAtRoot_ToolStripMenuItem";
            this.newScenarioAtRoot_ToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this.newScenarioAtRoot_ToolStripMenuItem.Text = "Scenario at Root";
            this.newScenarioAtRoot_ToolStripMenuItem.Click += new System.EventHandler(this.newScenarioAtRoot_ToolStripMenuItem_Click);
            // 
            // delete_ToolStripButton
            // 
            this.delete_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("delete_ToolStripButton.Image")));
            this.delete_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete_ToolStripButton.Name = "delete_ToolStripButton";
            this.delete_ToolStripButton.Size = new System.Drawing.Size(64, 24);
            this.delete_ToolStripButton.Text = "Delete";
            this.delete_ToolStripButton.ToolTipText = "Delete the selected object";
            this.delete_ToolStripButton.Click += new System.EventHandler(this.delete_ToolStripButton_Click);
            // 
            // configurationTreeRefresh_ToolStripButton
            // 
            this.configurationTreeRefresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("configurationTreeRefresh_ToolStripButton.Image")));
            this.configurationTreeRefresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.configurationTreeRefresh_ToolStripButton.Name = "configurationTreeRefresh_ToolStripButton";
            this.configurationTreeRefresh_ToolStripButton.Size = new System.Drawing.Size(70, 24);
            this.configurationTreeRefresh_ToolStripButton.Text = "Refresh";
            this.configurationTreeRefresh_ToolStripButton.ToolTipText = "Refresh the tree from the database";
            this.configurationTreeRefresh_ToolStripButton.Click += new System.EventHandler(this.configurationTreeRefresh_ToolStripButton_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 27);
            // 
            // resource_ToolStrip
            // 
            this.resource_ToolStrip.AllowMerge = false;
            this.resource_ToolStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.resource_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.resource_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.resource_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.save_ToolStripButton,
            this.resource_ToolStripLabel,
            this.analyze_ToolStripButton,
            this.executeScenario_ToolStripButton});
            this.resource_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.resource_ToolStrip.Name = "resource_ToolStrip";
            this.resource_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.resource_ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.resource_ToolStrip.Size = new System.Drawing.Size(753, 27);
            this.resource_ToolStrip.TabIndex = 1;
            this.resource_ToolStrip.Text = "toolStrip1";
            // 
            // save_ToolStripButton
            // 
            this.save_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("save_ToolStripButton.Image")));
            this.save_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.save_ToolStripButton.Name = "save_ToolStripButton";
            this.save_ToolStripButton.Size = new System.Drawing.Size(55, 24);
            this.save_ToolStripButton.Text = "Save";
            this.save_ToolStripButton.ToolTipText = "Save changes for this resource";
            this.save_ToolStripButton.Click += new System.EventHandler(this.save_ToolStripButton_Click);
            // 
            // resource_ToolStripLabel
            // 
            this.resource_ToolStripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.resource_ToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.resource_ToolStripLabel.Name = "resource_ToolStripLabel";
            this.resource_ToolStripLabel.Size = new System.Drawing.Size(0, 24);
            this.resource_ToolStripLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // analyze_ToolStripButton
            // 
            this.analyze_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("analyze_ToolStripButton.Image")));
            this.analyze_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.analyze_ToolStripButton.Name = "analyze_ToolStripButton";
            this.analyze_ToolStripButton.Size = new System.Drawing.Size(60, 24);
            this.analyze_ToolStripButton.Text = "Verify";
            this.analyze_ToolStripButton.ToolTipText = "Display the required system resources to execute this scenario";
            this.analyze_ToolStripButton.Click += new System.EventHandler(this.analyze_ToolStripButton_Click);
            // 
            // executeScenario_ToolStripButton
            // 
            this.executeScenario_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("executeScenario_ToolStripButton.Image")));
            this.executeScenario_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.executeScenario_ToolStripButton.Name = "executeScenario_ToolStripButton";
            this.executeScenario_ToolStripButton.Size = new System.Drawing.Size(55, 24);
            this.executeScenario_ToolStripButton.Text = "Start";
            this.executeScenario_ToolStripButton.ToolTipText = "Execute the currently selected scenario";
            this.executeScenario_ToolStripButton.Click += new System.EventHandler(this.executeScenario_ToolStripButton_Click);
            // 
            // dataEditPanel
            // 
            this.dataEditPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataEditPanel.BackColor = System.Drawing.SystemColors.Control;
            this.dataEditPanel.Location = new System.Drawing.Point(1, 27);
            this.dataEditPanel.Name = "dataEditPanel";
            this.dataEditPanel.Size = new System.Drawing.Size(751, 611);
            this.dataEditPanel.TabIndex = 0;
            // 
            // dashboard_TabPage
            // 
            this.dashboard_TabPage.Location = new System.Drawing.Point(4, 24);
            this.dashboard_TabPage.Name = "dashboard_TabPage";
            this.dashboard_TabPage.Size = new System.Drawing.Size(1049, 647);
            this.dashboard_TabPage.TabIndex = 6;
            this.dashboard_TabPage.Text = "Environment";
            // 
            // execution_TabPage
            // 
            this.execution_TabPage.Controls.Add(this.connectedSessionsControl);
            this.execution_TabPage.Location = new System.Drawing.Point(4, 24);
            this.execution_TabPage.Name = "execution_TabPage";
            this.execution_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.execution_TabPage.Size = new System.Drawing.Size(1049, 647);
            this.execution_TabPage.TabIndex = 1;
            this.execution_TabPage.Text = "Session Execution";
            // 
            // connectedSessionsControl
            // 
            this.connectedSessionsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectedSessionsControl.Location = new System.Drawing.Point(3, 3);
            this.connectedSessionsControl.Margin = new System.Windows.Forms.Padding(4);
            this.connectedSessionsControl.Name = "connectedSessionsControl";
            this.connectedSessionsControl.Size = new System.Drawing.Size(1043, 641);
            this.connectedSessionsControl.TabIndex = 0;
            // 
            // reports_TabPage
            // 
            this.reports_TabPage.Controls.Add(this.graphingUserControl);
            this.reports_TabPage.Location = new System.Drawing.Point(4, 24);
            this.reports_TabPage.Name = "reports_TabPage";
            this.reports_TabPage.Size = new System.Drawing.Size(1049, 647);
            this.reports_TabPage.TabIndex = 5;
            this.reports_TabPage.Text = "Test Results";
            // 
            // graphingUserControl
            // 
            this.graphingUserControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.graphingUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphingUserControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.graphingUserControl.Location = new System.Drawing.Point(0, 0);
            this.graphingUserControl.Margin = new System.Windows.Forms.Padding(5);
            this.graphingUserControl.Name = "graphingUserControl";
            this.graphingUserControl.Size = new System.Drawing.Size(1049, 647);
            this.graphingUserControl.TabIndex = 0;
            // 
            // main_StatusLabel
            // 
            this.main_StatusLabel.Name = "main_StatusLabel";
            this.main_StatusLabel.Size = new System.Drawing.Size(787, 19);
            this.main_StatusLabel.Spring = true;
            this.main_StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userName_StatusLabel
            // 
            this.userName_StatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.userName_StatusLabel.Name = "userName_StatusLabel";
            this.userName_StatusLabel.Size = new System.Drawing.Size(95, 19);
            this.userName_StatusLabel.Text = "[Not Logged In]";
            // 
            // version_StatusLabel
            // 
            this.version_StatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.version_StatusLabel.Name = "version_StatusLabel";
            this.version_StatusLabel.Size = new System.Drawing.Size(71, 19);
            this.version_StatusLabel.Text = "STF Version";
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.main_StatusLabel,
            this.userName_StatusLabel,
            this.environment_StatusLabel,
            this.version_StatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 699);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.mainStatusStrip.Size = new System.Drawing.Size(1057, 24);
            this.mainStatusStrip.TabIndex = 14;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // environment_StatusLabel
            // 
            this.environment_StatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.environment_StatusLabel.Name = "environment_StatusLabel";
            this.environment_StatusLabel.Size = new System.Drawing.Size(87, 19);
            this.environment_StatusLabel.Text = "[Environment]";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 723);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(1000, 750);
            this.Name = "MainForm";
            this.Text = "Solution Test Enterprise Management Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.configurationTabPage.ResumeLayout(false);
            this.scenarioConfiguration_SplitContainer.Panel1.ResumeLayout(false);
            this.scenarioConfiguration_SplitContainer.Panel1.PerformLayout();
            this.scenarioConfiguration_SplitContainer.Panel2.ResumeLayout(false);
            this.scenarioConfiguration_SplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfiguration_SplitContainer)).EndInit();
            this.scenarioConfiguration_SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfigurationTreeView)).EndInit();
            this.configTreeViewSearch_ToolStrip.ResumeLayout(false);
            this.configTreeViewSearch_ToolStrip.PerformLayout();
            this.configurationTreeView_ToolStrip.ResumeLayout(false);
            this.configurationTreeView_ToolStrip.PerformLayout();
            this.resource_ToolStrip.ResumeLayout(false);
            this.resource_ToolStrip.PerformLayout();
            this.execution_TabPage.ResumeLayout(false);
            this.reports_TabPage.ResumeLayout(false);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem file_MenuItem;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage execution_TabPage;
        private System.Windows.Forms.TabPage configurationTabPage;
        private System.Windows.Forms.SplitContainer scenarioConfiguration_SplitContainer;
        private System.Windows.Forms.Panel dataEditPanel;
        private System.Windows.Forms.ToolStripMenuItem administration_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManagement_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem exit_MenuItem;
        private System.Windows.Forms.ToolStripStatusLabel main_StatusLabel;
        private System.Windows.Forms.ToolStripMenuItem help_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem about_MenuItem;
        private System.Windows.Forms.TabPage reports_TabPage;
        private UI.Charting.GraphingUserControl graphingUserControl;
        private System.Windows.Forms.ToolStripMenuItem session_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageSessionData_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem configuration_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem printServers_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem printDrivers_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem products_MenuItem;
        private System.Windows.Forms.TabPage dashboard_TabPage;
        private System.Windows.Forms.ToolStripMenuItem serverInventory_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem printDriverConfigurations_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem view_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem assetUsage_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionReports_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem pluginReferences_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemSettings_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem monitorConfig_MenuItem;
        private UI.ScenarioConfiguration.EnterpriseScenarioTreeView scenarioConfigurationTreeView;
        private System.Windows.Forms.ToolStripStatusLabel userName_StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel version_StatusLabel;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        //private UI.SessionExecution.SessionExecutionControl sessionExecutionControl;
        private System.Windows.Forms.ToolStripMenuItem logIn_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem perfMonCounters_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem installerPackages_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem softwareInstallers_MenuItem;
        private System.Windows.Forms.ToolStrip resource_ToolStrip;
        private System.Windows.Forms.ToolStripButton save_ToolStripButton;
        private System.Windows.Forms.ToolStripLabel resource_ToolStripLabel;
        private System.Windows.Forms.ToolStrip configurationTreeView_ToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton newItem_DropDownButton;
        private System.Windows.Forms.ToolStripMenuItem newFolder_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newScenario_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newFolderAtRoot_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newScenarioAtRoot_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton configurationTreeRefresh_ToolStripButton;
        private System.Windows.Forms.ToolStripButton delete_ToolStripButton;
        private System.Windows.Forms.ToolStripButton executeScenario_ToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem vmPlatformAssoc_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem userGroup_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem machineGroup_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem ePrintServers_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem printerProducts_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem machineGroupView_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton analyze_ToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem vmInventory_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripStatusLabel environment_StatusLabel;
        private System.Windows.Forms.ToolStripMenuItem simulatorMgt_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem citrixPublishedApps_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem activieDirectoryGroup_MenuItem;
        private UI.SessionExecution.ConnectedSessionsControl connectedSessionsControl;
        private System.Windows.Forms.ToolStripMenuItem importScenario_MenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem activityPluginSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionWebUIReports_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverResourceCategory_MenuItem;
        private System.Windows.Forms.ToolStrip configTreeViewSearch_ToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox searchText_toolStripTextBox;
        private System.Windows.Forms.ToolStripButton searchNext_toolStripButton;
        private System.Windows.Forms.ToolStripButton searchPrev_toolStripButton;
        private System.Windows.Forms.ToolStripMenuItem ProductPlugintoolStripMenuItem;
    }
}



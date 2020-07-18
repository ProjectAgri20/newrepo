namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Main form for the Print Queue Installer
    /// </summary>
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.icons_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiThreadedInstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageDriverPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printQueuePropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installationLogDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registryChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadDriverPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installNewDriverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upgradeDriverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.driverConfigurationUtilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(960, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveAsToolStripMenuItem.Text = "Save Configuration As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(197, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiThreadedInstallToolStripMenuItem,
            this.manageDriverPathsToolStripMenuItem,
            this.printQueuePropertiesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installationLogDataToolStripMenuItem,
            this.registryChangesToolStripMenuItem,
            this.toolStripSeparator4,
            this.applicationLogToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(162, 6);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadDriverPackageToolStripMenuItem,
            this.installNewDriverToolStripMenuItem,
            this.upgradeDriverToolStripMenuItem,
            this.toolStripSeparator3,
            this.driverConfigurationUtilityToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.toolsToolStripMenuItem.Text = "Driver";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(179, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.aboutToolStripMenuItem.Text = "About Print Queue Installer";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 508);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(960, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(114, 17);
            this.toolStripStatusLabel.Text = "Print Queue Installer";
            this.toolStripStatusLabel.ToolTipText = "Installation Status";
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(12, 27);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(936, 478);
            this.mainPanel.TabIndex = 3;
            // 
            // icons_ImageList
            // 
            this.icons_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("icons_ImageList.ImageStream")));
            this.icons_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.icons_ImageList.Images.SetKeyName(0, "Save");
            this.icons_ImageList.Images.SetKeyName(1, "Folder");
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.folder_page_white;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openToolStripMenuItem.Text = "Open Configuration";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveToolStripMenuItem.Text = "Save Configuration";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // multiThreadedInstallToolStripMenuItem
            // 
            this.multiThreadedInstallToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.cog;
            this.multiThreadedInstallToolStripMenuItem.Name = "multiThreadedInstallToolStripMenuItem";
            this.multiThreadedInstallToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.multiThreadedInstallToolStripMenuItem.Text = "Threaded Install Setup";
            this.multiThreadedInstallToolStripMenuItem.Click += new System.EventHandler(this.multiThreadedInstallToolStripMenuItem_Click);
            // 
            // manageDriverPathsToolStripMenuItem
            // 
            this.manageDriverPathsToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.folder_table;
            this.manageDriverPathsToolStripMenuItem.Name = "manageDriverPathsToolStripMenuItem";
            this.manageDriverPathsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.manageDriverPathsToolStripMenuItem.Text = "Manage Driver Paths";
            this.manageDriverPathsToolStripMenuItem.Click += new System.EventHandler(this.manageDriverPathsToolStripMenuItem_Click);
            // 
            // printQueuePropertiesToolStripMenuItem
            // 
            this.printQueuePropertiesToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.PrinterPort;
            this.printQueuePropertiesToolStripMenuItem.Name = "printQueuePropertiesToolStripMenuItem";
            this.printQueuePropertiesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.printQueuePropertiesToolStripMenuItem.Text = "Print Queue Ports";
            this.printQueuePropertiesToolStripMenuItem.Click += new System.EventHandler(this.printQueuePropertiesToolStripMenuItem_Click);
            // 
            // installationLogDataToolStripMenuItem
            // 
            this.installationLogDataToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.page_white_dvd;
            this.installationLogDataToolStripMenuItem.Name = "installationLogDataToolStripMenuItem";
            this.installationLogDataToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.installationLogDataToolStripMenuItem.Text = "Installation Log";
            this.installationLogDataToolStripMenuItem.Click += new System.EventHandler(this.installationLogDataToolStripMenuItem_Click);
            // 
            // registryChangesToolStripMenuItem
            // 
            this.registryChangesToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.Registry;
            this.registryChangesToolStripMenuItem.Name = "registryChangesToolStripMenuItem";
            this.registryChangesToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.registryChangesToolStripMenuItem.Text = "Registry Changes";
            this.registryChangesToolStripMenuItem.Click += new System.EventHandler(this.registryChangesToolStripMenuItem_Click);
            // 
            // applicationLogToolStripMenuItem
            // 
            this.applicationLogToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.table_gear;
            this.applicationLogToolStripMenuItem.Name = "applicationLogToolStripMenuItem";
            this.applicationLogToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.applicationLogToolStripMenuItem.Text = "Application Log";
            this.applicationLogToolStripMenuItem.Click += new System.EventHandler(this.applicationLogToolStripMenuItem_Click);
            // 
            // downloadDriverPackageToolStripMenuItem
            // 
            this.downloadDriverPackageToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.Download;
            this.downloadDriverPackageToolStripMenuItem.Name = "downloadDriverPackageToolStripMenuItem";
            this.downloadDriverPackageToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.downloadDriverPackageToolStripMenuItem.Text = "Download Driver";
            this.downloadDriverPackageToolStripMenuItem.Click += new System.EventHandler(this.downloadDriverPackageToolStripMenuItem_Click);
            // 
            // installNewDriverToolStripMenuItem
            // 
            this.installNewDriverToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.drive_cd;
            this.installNewDriverToolStripMenuItem.Name = "installNewDriverToolStripMenuItem";
            this.installNewDriverToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.installNewDriverToolStripMenuItem.Text = "Install Driver";
            this.installNewDriverToolStripMenuItem.Click += new System.EventHandler(this.installNewDriverToolStripMenuItem_Click);
            // 
            // upgradeDriverToolStripMenuItem
            // 
            this.upgradeDriverToolStripMenuItem.Image = global::HP.ScalableTest.Print.Utility.Properties.Resources.cd_add;
            this.upgradeDriverToolStripMenuItem.Name = "upgradeDriverToolStripMenuItem";
            this.upgradeDriverToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.upgradeDriverToolStripMenuItem.Text = "Upgrade Driver";
            this.upgradeDriverToolStripMenuItem.Click += new System.EventHandler(this.upgradeDriverToolStripMenuItem_Click);
            // 
            // driverConfigurationUtilityToolStripMenuItem
            // 
            this.driverConfigurationUtilityToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("driverConfigurationUtilityToolStripMenuItem.Image")));
            this.driverConfigurationUtilityToolStripMenuItem.Name = "driverConfigurationUtilityToolStripMenuItem";
            this.driverConfigurationUtilityToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.driverConfigurationUtilityToolStripMenuItem.Text = "Driver Configuration";
            this.driverConfigurationUtilityToolStripMenuItem.Click += new System.EventHandler(this.driverConfigurationUtilityToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 530);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STF Print Queue Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadDriverPackageToolStripMenuItem;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ToolStripMenuItem driverConfigurationUtilityToolStripMenuItem;
        private System.Windows.Forms.ImageList icons_ImageList;
        private System.Windows.Forms.ToolStripMenuItem installNewDriverToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem multiThreadedInstallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageDriverPathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upgradeDriverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installationLogDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registryChangesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem applicationLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printQueuePropertiesToolStripMenuItem;


    }
}


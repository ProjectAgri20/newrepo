namespace HP.ScalableTest.Plugin.TwainDriverConfiguration
{
    partial class TwainDriverConfigurationConfigControl
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
            this.path_groupBox = new System.Windows.Forms.GroupBox();
            this.browseInstaller_Button = new System.Windows.Forms.Button();
            this.setupPath_TextBox = new System.Windows.Forms.TextBox();
            this.installerFile_Label = new System.Windows.Forms.Label();
            this.options_GroupBox = new System.Windows.Forms.GroupBox();
            this.scanConfiguration_RadioButton = new System.Windows.Forms.RadioButton();
            this.install_RadioButton = new System.Windows.Forms.RadioButton();
            this.deviceAddition_RadioButton = new System.Windows.Forms.RadioButton();
            this.modesOfOperation_GroupBox = new System.Windows.Forms.GroupBox();
            this.emailAsPDF_RadioButton = new System.Windows.Forms.RadioButton();
            this.shortcut_RadioButton = new System.Windows.Forms.RadioButton();
            this.everyDayScan_RadioButton = new System.Windows.Forms.RadioButton();
            this.emailAsJPEG_RadioButton = new System.Windows.Forms.RadioButton();
            this.saveAsJPEG_RadioButton = new System.Windows.Forms.RadioButton();
            this.saveAsPDF_RadioButton = new System.Windows.Forms.RadioButton();
            this.scanSettings_GroupBox = new System.Windows.Forms.GroupBox();
            this.colorMode_ComboBox = new System.Windows.Forms.ComboBox();
            this.source_ComboBox = new System.Windows.Forms.ComboBox();
            this.colorMode_Label = new System.Windows.Forms.Label();
            this.source_Label = new System.Windows.Forms.Label();
            this.pageSides_ComboBox = new System.Windows.Forms.ComboBox();
            this.pageSize_ComboBox = new System.Windows.Forms.ComboBox();
            this.itemType_ComboBox = new System.Windows.Forms.ComboBox();
            this.pageSize_Label = new System.Windows.Forms.Label();
            this.pageSides_Label = new System.Windows.Forms.Label();
            this.itemType_Label = new System.Windows.Forms.Label();
            this.destination_GroupBox = new System.Windows.Forms.GroupBox();
            this.sendTo_ComboBox = new System.Windows.Forms.ComboBox();
            this.sendTo_Label = new System.Windows.Forms.Label();
            this.fileType_ComboBox = new System.Windows.Forms.ComboBox();
            this.fileType_Label = new System.Windows.Forms.Label();
            this.createNewScanShortcut_GroupBox = new System.Windows.Forms.GroupBox();
            this.shortcutSettings_ComboBox = new System.Windows.Forms.ComboBox();
            this.shortcutSettings_label = new System.Windows.Forms.Label();
            this.configuration_GroupBox = new System.Windows.Forms.GroupBox();
            this.reservation_groupBox = new System.Windows.Forms.GroupBox();
            this.enableReservation_CheckBox = new System.Windows.Forms.CheckBox();
            this.pinRequired_CheckBox = new System.Windows.Forms.CheckBox();
            this.pinDescription_label = new System.Windows.Forms.Label();
            this.pin_Label = new System.Windows.Forms.Label();
            this.pin_TextBox = new System.Windows.Forms.TextBox();
            this.networkFolder_GroupBox = new System.Windows.Forms.GroupBox();
            this.saveToFolderPath_TextBox = new System.Windows.Forms.TextBox();
            this.saveToFolder_Label = new System.Windows.Forms.Label();
            this.email_GroupBox = new System.Windows.Forms.GroupBox();
            this.validation_Label = new System.Windows.Forms.Label();
            this.emailAddress_TextBox = new System.Windows.Forms.TextBox();
            this.emailAddress_Label = new System.Windows.Forms.Label();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.path_groupBox.SuspendLayout();
            this.options_GroupBox.SuspendLayout();
            this.modesOfOperation_GroupBox.SuspendLayout();
            this.scanSettings_GroupBox.SuspendLayout();
            this.destination_GroupBox.SuspendLayout();
            this.createNewScanShortcut_GroupBox.SuspendLayout();
            this.configuration_GroupBox.SuspendLayout();
            this.reservation_groupBox.SuspendLayout();
            this.networkFolder_GroupBox.SuspendLayout();
            this.email_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // path_groupBox
            // 
            this.path_groupBox.Controls.Add(this.browseInstaller_Button);
            this.path_groupBox.Controls.Add(this.setupPath_TextBox);
            this.path_groupBox.Controls.Add(this.installerFile_Label);
            this.path_groupBox.Location = new System.Drawing.Point(3, 61);
            this.path_groupBox.Name = "path_groupBox";
            this.path_groupBox.Size = new System.Drawing.Size(675, 65);
            this.path_groupBox.TabIndex = 14;
            this.path_groupBox.TabStop = false;
            this.path_groupBox.Text = "Path";
            // 
            // browseInstaller_Button
            // 
            this.browseInstaller_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseInstaller_Button.Location = new System.Drawing.Point(604, 17);
            this.browseInstaller_Button.Name = "browseInstaller_Button";
            this.browseInstaller_Button.Size = new System.Drawing.Size(40, 23);
            this.browseInstaller_Button.TabIndex = 2;
            this.browseInstaller_Button.Text = "...";
            this.browseInstaller_Button.UseVisualStyleBackColor = true;
            this.browseInstaller_Button.Click += new System.EventHandler(this.browseInstaller_Button_Click);
            // 
            // setupPath_TextBox
            // 
            this.setupPath_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setupPath_TextBox.BackColor = System.Drawing.Color.White;
            this.setupPath_TextBox.Location = new System.Drawing.Point(74, 20);
            this.setupPath_TextBox.Name = "setupPath_TextBox";
            this.setupPath_TextBox.Size = new System.Drawing.Size(521, 20);
            this.setupPath_TextBox.TabIndex = 1;
            // 
            // installerFile_Label
            // 
            this.installerFile_Label.AutoSize = true;
            this.installerFile_Label.Location = new System.Drawing.Point(6, 23);
            this.installerFile_Label.Name = "installerFile_Label";
            this.installerFile_Label.Size = new System.Drawing.Size(62, 13);
            this.installerFile_Label.TabIndex = 0;
            this.installerFile_Label.Text = "Installer File";
            // 
            // options_GroupBox
            // 
            this.options_GroupBox.Controls.Add(this.scanConfiguration_RadioButton);
            this.options_GroupBox.Controls.Add(this.install_RadioButton);
            this.options_GroupBox.Controls.Add(this.deviceAddition_RadioButton);
            this.options_GroupBox.Location = new System.Drawing.Point(3, 3);
            this.options_GroupBox.Name = "options_GroupBox";
            this.options_GroupBox.Size = new System.Drawing.Size(675, 52);
            this.options_GroupBox.TabIndex = 13;
            this.options_GroupBox.TabStop = false;
            this.options_GroupBox.Text = "Options";
            // 
            // scanConfiguration_RadioButton
            // 
            this.scanConfiguration_RadioButton.AutoSize = true;
            this.scanConfiguration_RadioButton.Location = new System.Drawing.Point(406, 19);
            this.scanConfiguration_RadioButton.Name = "scanConfiguration_RadioButton";
            this.scanConfiguration_RadioButton.Size = new System.Drawing.Size(115, 17);
            this.scanConfiguration_RadioButton.TabIndex = 2;
            this.scanConfiguration_RadioButton.Text = "Scan Configuration";
            this.scanConfiguration_RadioButton.UseVisualStyleBackColor = true;
            this.scanConfiguration_RadioButton.CheckedChanged += new System.EventHandler(this.operation_RadioButton_CheckedChanged);
            // 
            // install_RadioButton
            // 
            this.install_RadioButton.AutoSize = true;
            this.install_RadioButton.Location = new System.Drawing.Point(6, 19);
            this.install_RadioButton.Name = "install_RadioButton";
            this.install_RadioButton.Size = new System.Drawing.Size(52, 17);
            this.install_RadioButton.TabIndex = 0;
            this.install_RadioButton.Text = "Install";
            this.install_RadioButton.CheckedChanged += new System.EventHandler(this.operation_RadioButton_CheckedChanged);
            // 
            // deviceAddition_RadioButton
            // 
            this.deviceAddition_RadioButton.AutoSize = true;
            this.deviceAddition_RadioButton.Location = new System.Drawing.Point(206, 19);
            this.deviceAddition_RadioButton.Name = "deviceAddition_RadioButton";
            this.deviceAddition_RadioButton.Size = new System.Drawing.Size(100, 17);
            this.deviceAddition_RadioButton.TabIndex = 1;
            this.deviceAddition_RadioButton.Text = "Device Addition";
            this.deviceAddition_RadioButton.UseVisualStyleBackColor = true;
            this.deviceAddition_RadioButton.CheckedChanged += new System.EventHandler(this.operation_RadioButton_CheckedChanged);
            // 
            // modesOfOperation_GroupBox
            // 
            this.modesOfOperation_GroupBox.Controls.Add(this.emailAsPDF_RadioButton);
            this.modesOfOperation_GroupBox.Controls.Add(this.shortcut_RadioButton);
            this.modesOfOperation_GroupBox.Controls.Add(this.everyDayScan_RadioButton);
            this.modesOfOperation_GroupBox.Controls.Add(this.emailAsJPEG_RadioButton);
            this.modesOfOperation_GroupBox.Controls.Add(this.saveAsJPEG_RadioButton);
            this.modesOfOperation_GroupBox.Controls.Add(this.saveAsPDF_RadioButton);
            this.modesOfOperation_GroupBox.Location = new System.Drawing.Point(3, 15);
            this.modesOfOperation_GroupBox.Name = "modesOfOperation_GroupBox";
            this.modesOfOperation_GroupBox.Size = new System.Drawing.Size(648, 50);
            this.modesOfOperation_GroupBox.TabIndex = 60;
            this.modesOfOperation_GroupBox.TabStop = false;
            this.modesOfOperation_GroupBox.Text = "Modes of Operation";
            // 
            // emailAsPDF_RadioButton
            // 
            this.emailAsPDF_RadioButton.AutoSize = true;
            this.emailAsPDF_RadioButton.Location = new System.Drawing.Point(203, 20);
            this.emailAsPDF_RadioButton.Name = "emailAsPDF_RadioButton";
            this.emailAsPDF_RadioButton.Size = new System.Drawing.Size(88, 17);
            this.emailAsPDF_RadioButton.TabIndex = 5;
            this.emailAsPDF_RadioButton.Text = "Email as PDF";
            this.emailAsPDF_RadioButton.UseVisualStyleBackColor = true;
            this.emailAsPDF_RadioButton.CheckedChanged += new System.EventHandler(this.scanConfiguration_RadioButton_CheckedChanged);
            // 
            // shortcut_RadioButton
            // 
            this.shortcut_RadioButton.AutoSize = true;
            this.shortcut_RadioButton.Location = new System.Drawing.Point(502, 19);
            this.shortcut_RadioButton.Name = "shortcut_RadioButton";
            this.shortcut_RadioButton.Size = new System.Drawing.Size(152, 17);
            this.shortcut_RadioButton.TabIndex = 4;
            this.shortcut_RadioButton.Text = "Create New Scan Shortcut";
            this.shortcut_RadioButton.UseVisualStyleBackColor = true;
            this.shortcut_RadioButton.CheckedChanged += new System.EventHandler(this.scanConfiguration_RadioButton_CheckedChanged);
            // 
            // everyDayScan_RadioButton
            // 
            this.everyDayScan_RadioButton.AutoSize = true;
            this.everyDayScan_RadioButton.Location = new System.Drawing.Point(397, 19);
            this.everyDayScan_RadioButton.Name = "everyDayScan_RadioButton";
            this.everyDayScan_RadioButton.Size = new System.Drawing.Size(99, 17);
            this.everyDayScan_RadioButton.TabIndex = 3;
            this.everyDayScan_RadioButton.Text = "EveryDay Scan";
            this.everyDayScan_RadioButton.UseVisualStyleBackColor = true;
            this.everyDayScan_RadioButton.CheckedChanged += new System.EventHandler(this.scanConfiguration_RadioButton_CheckedChanged);
            // 
            // emailAsJPEG_RadioButton
            // 
            this.emailAsJPEG_RadioButton.AutoSize = true;
            this.emailAsJPEG_RadioButton.Location = new System.Drawing.Point(297, 19);
            this.emailAsJPEG_RadioButton.Name = "emailAsJPEG_RadioButton";
            this.emailAsJPEG_RadioButton.Size = new System.Drawing.Size(94, 17);
            this.emailAsJPEG_RadioButton.TabIndex = 2;
            this.emailAsJPEG_RadioButton.Text = "Email as JPEG";
            this.emailAsJPEG_RadioButton.UseVisualStyleBackColor = true;
            this.emailAsJPEG_RadioButton.CheckedChanged += new System.EventHandler(this.scanConfiguration_RadioButton_CheckedChanged);
            // 
            // saveAsJPEG_RadioButton
            // 
            this.saveAsJPEG_RadioButton.AutoSize = true;
            this.saveAsJPEG_RadioButton.Location = new System.Drawing.Point(103, 19);
            this.saveAsJPEG_RadioButton.Name = "saveAsJPEG_RadioButton";
            this.saveAsJPEG_RadioButton.Size = new System.Drawing.Size(94, 17);
            this.saveAsJPEG_RadioButton.TabIndex = 1;
            this.saveAsJPEG_RadioButton.Text = "Save as JPEG";
            this.saveAsJPEG_RadioButton.UseVisualStyleBackColor = true;
            this.saveAsJPEG_RadioButton.CheckedChanged += new System.EventHandler(this.scanConfiguration_RadioButton_CheckedChanged);
            // 
            // saveAsPDF_RadioButton
            // 
            this.saveAsPDF_RadioButton.AutoSize = true;
            this.saveAsPDF_RadioButton.Checked = true;
            this.saveAsPDF_RadioButton.Location = new System.Drawing.Point(9, 20);
            this.saveAsPDF_RadioButton.Name = "saveAsPDF_RadioButton";
            this.saveAsPDF_RadioButton.Size = new System.Drawing.Size(88, 17);
            this.saveAsPDF_RadioButton.TabIndex = 0;
            this.saveAsPDF_RadioButton.TabStop = true;
            this.saveAsPDF_RadioButton.Text = "Save as PDF";
            this.saveAsPDF_RadioButton.UseVisualStyleBackColor = true;
            this.saveAsPDF_RadioButton.CheckedChanged += new System.EventHandler(this.scanConfiguration_RadioButton_CheckedChanged);
            // 
            // scanSettings_GroupBox
            // 
            this.scanSettings_GroupBox.Controls.Add(this.colorMode_ComboBox);
            this.scanSettings_GroupBox.Controls.Add(this.source_ComboBox);
            this.scanSettings_GroupBox.Controls.Add(this.colorMode_Label);
            this.scanSettings_GroupBox.Controls.Add(this.source_Label);
            this.scanSettings_GroupBox.Controls.Add(this.pageSides_ComboBox);
            this.scanSettings_GroupBox.Controls.Add(this.pageSize_ComboBox);
            this.scanSettings_GroupBox.Controls.Add(this.itemType_ComboBox);
            this.scanSettings_GroupBox.Controls.Add(this.pageSize_Label);
            this.scanSettings_GroupBox.Controls.Add(this.pageSides_Label);
            this.scanSettings_GroupBox.Controls.Add(this.itemType_Label);
            this.scanSettings_GroupBox.Location = new System.Drawing.Point(3, 71);
            this.scanSettings_GroupBox.Name = "scanSettings_GroupBox";
            this.scanSettings_GroupBox.Size = new System.Drawing.Size(289, 166);
            this.scanSettings_GroupBox.TabIndex = 61;
            this.scanSettings_GroupBox.TabStop = false;
            this.scanSettings_GroupBox.Text = "Scan Settings";
            // 
            // colorMode_ComboBox
            // 
            this.colorMode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorMode_ComboBox.FormattingEnabled = true;
            this.colorMode_ComboBox.Location = new System.Drawing.Point(91, 129);
            this.colorMode_ComboBox.Name = "colorMode_ComboBox";
            this.colorMode_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.colorMode_ComboBox.TabIndex = 8;
            // 
            // source_ComboBox
            // 
            this.source_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.source_ComboBox.FormattingEnabled = true;
            this.source_ComboBox.Location = new System.Drawing.Point(91, 47);
            this.source_ComboBox.Name = "source_ComboBox";
            this.source_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.source_ComboBox.TabIndex = 10;
            this.source_ComboBox.SelectedIndexChanged += new System.EventHandler(this.source_ComboBox_SelectedIndexChanged);
            // 
            // colorMode_Label
            // 
            this.colorMode_Label.AutoSize = true;
            this.colorMode_Label.Location = new System.Drawing.Point(12, 132);
            this.colorMode_Label.Name = "colorMode_Label";
            this.colorMode_Label.Size = new System.Drawing.Size(61, 13);
            this.colorMode_Label.TabIndex = 1;
            this.colorMode_Label.Text = "Color Mode";
            // 
            // source_Label
            // 
            this.source_Label.AutoSize = true;
            this.source_Label.Location = new System.Drawing.Point(14, 50);
            this.source_Label.Name = "source_Label";
            this.source_Label.Size = new System.Drawing.Size(41, 13);
            this.source_Label.TabIndex = 9;
            this.source_Label.Text = "Source";
            // 
            // pageSides_ComboBox
            // 
            this.pageSides_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pageSides_ComboBox.FormattingEnabled = true;
            this.pageSides_ComboBox.Location = new System.Drawing.Point(91, 103);
            this.pageSides_ComboBox.Name = "pageSides_ComboBox";
            this.pageSides_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.pageSides_ComboBox.TabIndex = 5;
            // 
            // pageSize_ComboBox
            // 
            this.pageSize_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pageSize_ComboBox.FormattingEnabled = true;
            this.pageSize_ComboBox.Location = new System.Drawing.Point(91, 76);
            this.pageSize_ComboBox.Name = "pageSize_ComboBox";
            this.pageSize_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.pageSize_ComboBox.TabIndex = 6;
            // 
            // itemType_ComboBox
            // 
            this.itemType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.itemType_ComboBox.FormattingEnabled = true;
            this.itemType_ComboBox.Location = new System.Drawing.Point(91, 20);
            this.itemType_ComboBox.Name = "itemType_ComboBox";
            this.itemType_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.itemType_ComboBox.TabIndex = 4;
            this.itemType_ComboBox.SelectedIndexChanged += new System.EventHandler(this.itemType_ComboBox_SelectedIndexChanged);
            // 
            // pageSize_Label
            // 
            this.pageSize_Label.AutoSize = true;
            this.pageSize_Label.Location = new System.Drawing.Point(13, 79);
            this.pageSize_Label.Name = "pageSize_Label";
            this.pageSize_Label.Size = new System.Drawing.Size(55, 13);
            this.pageSize_Label.TabIndex = 2;
            this.pageSize_Label.Text = "Page Size";
            // 
            // pageSides_Label
            // 
            this.pageSides_Label.AutoSize = true;
            this.pageSides_Label.Location = new System.Drawing.Point(13, 106);
            this.pageSides_Label.Name = "pageSides_Label";
            this.pageSides_Label.Size = new System.Drawing.Size(61, 13);
            this.pageSides_Label.TabIndex = 1;
            this.pageSides_Label.Text = "Page Sides";
            // 
            // itemType_Label
            // 
            this.itemType_Label.AutoSize = true;
            this.itemType_Label.Location = new System.Drawing.Point(13, 23);
            this.itemType_Label.Name = "itemType_Label";
            this.itemType_Label.Size = new System.Drawing.Size(54, 13);
            this.itemType_Label.TabIndex = 0;
            this.itemType_Label.Text = "Item Type";
            // 
            // destination_GroupBox
            // 
            this.destination_GroupBox.Controls.Add(this.sendTo_ComboBox);
            this.destination_GroupBox.Controls.Add(this.sendTo_Label);
            this.destination_GroupBox.Controls.Add(this.fileType_ComboBox);
            this.destination_GroupBox.Controls.Add(this.fileType_Label);
            this.destination_GroupBox.Location = new System.Drawing.Point(300, 76);
            this.destination_GroupBox.Name = "destination_GroupBox";
            this.destination_GroupBox.Size = new System.Drawing.Size(292, 161);
            this.destination_GroupBox.TabIndex = 63;
            this.destination_GroupBox.TabStop = false;
            this.destination_GroupBox.Text = "Destination";
            this.destination_GroupBox.Visible = false;
            // 
            // sendTo_ComboBox
            // 
            this.sendTo_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sendTo_ComboBox.FormattingEnabled = true;
            this.sendTo_ComboBox.Location = new System.Drawing.Point(72, 50);
            this.sendTo_ComboBox.Name = "sendTo_ComboBox";
            this.sendTo_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.sendTo_ComboBox.TabIndex = 7;
            this.sendTo_ComboBox.SelectedIndexChanged += new System.EventHandler(this.sendTo_ComboBox_SelectedIndexChanged);
            // 
            // sendTo_Label
            // 
            this.sendTo_Label.AutoSize = true;
            this.sendTo_Label.Location = new System.Drawing.Point(12, 50);
            this.sendTo_Label.Name = "sendTo_Label";
            this.sendTo_Label.Size = new System.Drawing.Size(48, 13);
            this.sendTo_Label.TabIndex = 6;
            this.sendTo_Label.Text = "Send To";
            // 
            // fileType_ComboBox
            // 
            this.fileType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType_ComboBox.FormattingEnabled = true;
            this.fileType_ComboBox.Location = new System.Drawing.Point(72, 20);
            this.fileType_ComboBox.Name = "fileType_ComboBox";
            this.fileType_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.fileType_ComboBox.TabIndex = 5;
            // 
            // fileType_Label
            // 
            this.fileType_Label.AutoSize = true;
            this.fileType_Label.Location = new System.Drawing.Point(12, 23);
            this.fileType_Label.Name = "fileType_Label";
            this.fileType_Label.Size = new System.Drawing.Size(50, 13);
            this.fileType_Label.TabIndex = 1;
            this.fileType_Label.Text = "File Type";
            // 
            // createNewScanShortcut_GroupBox
            // 
            this.createNewScanShortcut_GroupBox.Controls.Add(this.shortcutSettings_ComboBox);
            this.createNewScanShortcut_GroupBox.Controls.Add(this.shortcutSettings_label);
            this.createNewScanShortcut_GroupBox.Location = new System.Drawing.Point(3, 71);
            this.createNewScanShortcut_GroupBox.Name = "createNewScanShortcut_GroupBox";
            this.createNewScanShortcut_GroupBox.Size = new System.Drawing.Size(292, 50);
            this.createNewScanShortcut_GroupBox.TabIndex = 64;
            this.createNewScanShortcut_GroupBox.TabStop = false;
            this.createNewScanShortcut_GroupBox.Text = "Create New Scan Shortcut";
            this.createNewScanShortcut_GroupBox.Visible = false;
            // 
            // shortcutSettings_ComboBox
            // 
            this.shortcutSettings_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shortcutSettings_ComboBox.FormattingEnabled = true;
            this.shortcutSettings_ComboBox.Location = new System.Drawing.Point(106, 19);
            this.shortcutSettings_ComboBox.Name = "shortcutSettings_ComboBox";
            this.shortcutSettings_ComboBox.Size = new System.Drawing.Size(170, 21);
            this.shortcutSettings_ComboBox.TabIndex = 8;
            // 
            // shortcutSettings_label
            // 
            this.shortcutSettings_label.AutoSize = true;
            this.shortcutSettings_label.Location = new System.Drawing.Point(12, 20);
            this.shortcutSettings_label.Name = "shortcutSettings_label";
            this.shortcutSettings_label.Size = new System.Drawing.Size(88, 13);
            this.shortcutSettings_label.TabIndex = 7;
            this.shortcutSettings_label.Text = "Shortcut Settings";
            // 
            // configuration_GroupBox
            // 
            this.configuration_GroupBox.Controls.Add(this.reservation_groupBox);
            this.configuration_GroupBox.Controls.Add(this.networkFolder_GroupBox);
            this.configuration_GroupBox.Controls.Add(this.email_GroupBox);
            this.configuration_GroupBox.Controls.Add(this.scanSettings_GroupBox);
            this.configuration_GroupBox.Controls.Add(this.destination_GroupBox);
            this.configuration_GroupBox.Controls.Add(this.createNewScanShortcut_GroupBox);
            this.configuration_GroupBox.Controls.Add(this.modesOfOperation_GroupBox);
            this.configuration_GroupBox.Location = new System.Drawing.Point(3, 244);
            this.configuration_GroupBox.Name = "configuration_GroupBox";
            this.configuration_GroupBox.Size = new System.Drawing.Size(675, 374);
            this.configuration_GroupBox.TabIndex = 63;
            this.configuration_GroupBox.TabStop = false;
            this.configuration_GroupBox.Visible = false;
            // 
            // reservation_groupBox
            // 
            this.reservation_groupBox.Controls.Add(this.enableReservation_CheckBox);
            this.reservation_groupBox.Controls.Add(this.pinRequired_CheckBox);
            this.reservation_groupBox.Controls.Add(this.pinDescription_label);
            this.reservation_groupBox.Controls.Add(this.pin_Label);
            this.reservation_groupBox.Controls.Add(this.pin_TextBox);
            this.reservation_groupBox.Location = new System.Drawing.Point(0, 238);
            this.reservation_groupBox.Name = "reservation_groupBox";
            this.reservation_groupBox.Size = new System.Drawing.Size(675, 37);
            this.reservation_groupBox.TabIndex = 73;
            this.reservation_groupBox.TabStop = false;
            this.reservation_groupBox.Text = "Reservation";
            // 
            // enableReservation_CheckBox
            // 
            this.enableReservation_CheckBox.AutoSize = true;
            this.enableReservation_CheckBox.Location = new System.Drawing.Point(18, 14);
            this.enableReservation_CheckBox.Name = "enableReservation_CheckBox";
            this.enableReservation_CheckBox.Size = new System.Drawing.Size(119, 17);
            this.enableReservation_CheckBox.TabIndex = 66;
            this.enableReservation_CheckBox.Text = "Enable Reservation";
            this.enableReservation_CheckBox.UseVisualStyleBackColor = true;
            this.enableReservation_CheckBox.CheckedChanged += new System.EventHandler(this.enableReservation_CheckBox_CheckedChanged);
            // 
            // pinRequired_CheckBox
            // 
            this.pinRequired_CheckBox.AutoSize = true;
            this.pinRequired_CheckBox.Enabled = false;
            this.pinRequired_CheckBox.Location = new System.Drawing.Point(219, 13);
            this.pinRequired_CheckBox.Name = "pinRequired_CheckBox";
            this.pinRequired_CheckBox.Size = new System.Drawing.Size(87, 17);
            this.pinRequired_CheckBox.TabIndex = 70;
            this.pinRequired_CheckBox.Text = "Pin Required";
            this.pinRequired_CheckBox.UseVisualStyleBackColor = true;
            this.pinRequired_CheckBox.CheckedChanged += new System.EventHandler(this.pinRequired_CheckBox_CheckedChanged);
            // 
            // pinDescription_label
            // 
            this.pinDescription_label.AutoSize = true;
            this.pinDescription_label.Location = new System.Drawing.Point(535, 14);
            this.pinDescription_label.Name = "pinDescription_label";
            this.pinDescription_label.Size = new System.Drawing.Size(132, 13);
            this.pinDescription_label.TabIndex = 71;
            this.pinDescription_label.Text = "numeric(0-9), 4 chars Max.";
            // 
            // pin_Label
            // 
            this.pin_Label.AutoSize = true;
            this.pin_Label.Location = new System.Drawing.Point(369, 15);
            this.pin_Label.Name = "pin_Label";
            this.pin_Label.Size = new System.Drawing.Size(25, 13);
            this.pin_Label.TabIndex = 68;
            this.pin_Label.Text = "PIN";
            // 
            // pin_TextBox
            // 
            this.pin_TextBox.Enabled = false;
            this.pin_TextBox.Location = new System.Drawing.Point(400, 11);
            this.pin_TextBox.MaxLength = 4;
            this.pin_TextBox.Name = "pin_TextBox";
            this.pin_TextBox.Size = new System.Drawing.Size(129, 20);
            this.pin_TextBox.TabIndex = 69;
            this.pin_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pin_TextBox_KeyPress);
            // 
            // networkFolder_GroupBox
            // 
            this.networkFolder_GroupBox.Controls.Add(this.saveToFolderPath_TextBox);
            this.networkFolder_GroupBox.Controls.Add(this.saveToFolder_Label);
            this.networkFolder_GroupBox.Location = new System.Drawing.Point(3, 275);
            this.networkFolder_GroupBox.Name = "networkFolder_GroupBox";
            this.networkFolder_GroupBox.Size = new System.Drawing.Size(666, 81);
            this.networkFolder_GroupBox.TabIndex = 65;
            this.networkFolder_GroupBox.TabStop = false;
            this.networkFolder_GroupBox.Text = "Folder";
            // 
            // saveToFolderPath_TextBox
            // 
            this.saveToFolderPath_TextBox.Location = new System.Drawing.Point(91, 20);
            this.saveToFolderPath_TextBox.Name = "saveToFolderPath_TextBox";
            this.saveToFolderPath_TextBox.Size = new System.Drawing.Size(527, 20);
            this.saveToFolderPath_TextBox.TabIndex = 1;
            // 
            // saveToFolder_Label
            // 
            this.saveToFolder_Label.AutoSize = true;
            this.saveToFolder_Label.Location = new System.Drawing.Point(13, 24);
            this.saveToFolder_Label.Name = "saveToFolder_Label";
            this.saveToFolder_Label.Size = new System.Drawing.Size(80, 13);
            this.saveToFolder_Label.TabIndex = 0;
            this.saveToFolder_Label.Text = "Save To Folder";
            // 
            // email_GroupBox
            // 
            this.email_GroupBox.Controls.Add(this.validation_Label);
            this.email_GroupBox.Controls.Add(this.emailAddress_TextBox);
            this.email_GroupBox.Controls.Add(this.emailAddress_Label);
            this.email_GroupBox.Location = new System.Drawing.Point(3, 275);
            this.email_GroupBox.Name = "email_GroupBox";
            this.email_GroupBox.Size = new System.Drawing.Size(666, 99);
            this.email_GroupBox.TabIndex = 65;
            this.email_GroupBox.TabStop = false;
            this.email_GroupBox.Text = "Email";
            this.email_GroupBox.Visible = false;
            // 
            // validation_Label
            // 
            this.validation_Label.AutoSize = true;
            this.validation_Label.Location = new System.Drawing.Point(85, 40);
            this.validation_Label.Name = "validation_Label";
            this.validation_Label.Size = new System.Drawing.Size(275, 13);
            this.validation_Label.TabIndex = 3;
            this.validation_Label.Text = "Please enter the email in this format jsmith@example.com";
            // 
            // emailAddress_TextBox
            // 
            this.emailAddress_TextBox.Location = new System.Drawing.Point(86, 17);
            this.emailAddress_TextBox.Name = "emailAddress_TextBox";
            this.emailAddress_TextBox.Size = new System.Drawing.Size(506, 20);
            this.emailAddress_TextBox.TabIndex = 1;
            // 
            // emailAddress_Label
            // 
            this.emailAddress_Label.AutoSize = true;
            this.emailAddress_Label.Location = new System.Drawing.Point(7, 20);
            this.emailAddress_Label.Name = "emailAddress_Label";
            this.emailAddress_Label.Size = new System.Drawing.Size(73, 13);
            this.emailAddress_Label.TabIndex = 0;
            this.emailAddress_Label.Text = "Email Address";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 132);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(675, 114);
            this.assetSelectionControl.TabIndex = 59;
            // 
            // TwainDriverConfigurationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.path_groupBox);
            this.Controls.Add(this.configuration_GroupBox);
            this.Controls.Add(this.options_GroupBox);
            this.Name = "TwainDriverConfigurationConfigControl";
            this.Size = new System.Drawing.Size(686, 621);
            this.path_groupBox.ResumeLayout(false);
            this.path_groupBox.PerformLayout();
            this.options_GroupBox.ResumeLayout(false);
            this.options_GroupBox.PerformLayout();
            this.modesOfOperation_GroupBox.ResumeLayout(false);
            this.modesOfOperation_GroupBox.PerformLayout();
            this.scanSettings_GroupBox.ResumeLayout(false);
            this.scanSettings_GroupBox.PerformLayout();
            this.destination_GroupBox.ResumeLayout(false);
            this.destination_GroupBox.PerformLayout();
            this.createNewScanShortcut_GroupBox.ResumeLayout(false);
            this.createNewScanShortcut_GroupBox.PerformLayout();
            this.configuration_GroupBox.ResumeLayout(false);
            this.reservation_groupBox.ResumeLayout(false);
            this.reservation_groupBox.PerformLayout();
            this.networkFolder_GroupBox.ResumeLayout(false);
            this.networkFolder_GroupBox.PerformLayout();
            this.email_GroupBox.ResumeLayout(false);
            this.email_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox path_groupBox;
        private System.Windows.Forms.Button browseInstaller_Button;
        private System.Windows.Forms.TextBox setupPath_TextBox;
        private System.Windows.Forms.Label installerFile_Label;
        private System.Windows.Forms.GroupBox options_GroupBox;
        private System.Windows.Forms.RadioButton install_RadioButton;
        private System.Windows.Forms.RadioButton deviceAddition_RadioButton;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox modesOfOperation_GroupBox;
        private System.Windows.Forms.RadioButton emailAsPDF_RadioButton;
        private System.Windows.Forms.RadioButton shortcut_RadioButton;
        private System.Windows.Forms.RadioButton everyDayScan_RadioButton;
        private System.Windows.Forms.RadioButton emailAsJPEG_RadioButton;
        private System.Windows.Forms.RadioButton saveAsJPEG_RadioButton;
        private System.Windows.Forms.RadioButton saveAsPDF_RadioButton;
        private System.Windows.Forms.GroupBox scanSettings_GroupBox;
        private System.Windows.Forms.ComboBox pageSize_ComboBox;
        private System.Windows.Forms.ComboBox pageSides_ComboBox;
        private System.Windows.Forms.ComboBox itemType_ComboBox;
        private System.Windows.Forms.Label pageSize_Label;
        private System.Windows.Forms.Label pageSides_Label;
        private System.Windows.Forms.Label itemType_Label;
        private System.Windows.Forms.ComboBox colorMode_ComboBox;
        private System.Windows.Forms.Label colorMode_Label;
        private System.Windows.Forms.GroupBox destination_GroupBox;
        private System.Windows.Forms.Label sendTo_Label;
        private System.Windows.Forms.ComboBox fileType_ComboBox;
        private System.Windows.Forms.Label fileType_Label;
        private System.Windows.Forms.ComboBox sendTo_ComboBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox createNewScanShortcut_GroupBox;
        private System.Windows.Forms.ComboBox shortcutSettings_ComboBox;
        private System.Windows.Forms.Label shortcutSettings_label;
        private System.Windows.Forms.ComboBox source_ComboBox;
        private System.Windows.Forms.Label source_Label;
        private System.Windows.Forms.GroupBox configuration_GroupBox;
        private System.Windows.Forms.RadioButton scanConfiguration_RadioButton;
        private System.Windows.Forms.GroupBox networkFolder_GroupBox;
        private System.Windows.Forms.TextBox saveToFolderPath_TextBox;
        private System.Windows.Forms.Label saveToFolder_Label;
        private System.Windows.Forms.CheckBox enableReservation_CheckBox;
        private System.Windows.Forms.Label pinDescription_label;
        private System.Windows.Forms.CheckBox pinRequired_CheckBox;
        private System.Windows.Forms.TextBox pin_TextBox;
        private System.Windows.Forms.Label pin_Label;
        private System.Windows.Forms.GroupBox email_GroupBox;
        private System.Windows.Forms.Label validation_Label;
        private System.Windows.Forms.TextBox emailAddress_TextBox;
        private System.Windows.Forms.Label emailAddress_Label;
        private System.Windows.Forms.GroupBox reservation_groupBox;
    }
}

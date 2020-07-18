namespace HP.ScalableTest.UI
{
    partial class ExceptionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionDialog));
            this.top_Panel = new System.Windows.Forms.Panel();
            this.exceptionMessage_TextBox = new System.Windows.Forms.TextBox();
            this.preamble_Label = new System.Windows.Forms.Label();
            this.header_Label = new System.Windows.Forms.Label();
            this.continue_Button = new System.Windows.Forms.Button();
            this.quit_Button = new System.Windows.Forms.Button();
            this.instructions_TextBox = new System.Windows.Forms.TextBox();
            this.exceptionDetails_Button = new System.Windows.Forms.Button();
            this.errorSymbol_PictureBox = new System.Windows.Forms.PictureBox();
            this.bottom_Panel = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.submit_TabPage = new System.Windows.Forms.TabPage();
            this.userNotes_TextBox = new System.Windows.Forms.TextBox();
            this.userEmail_TextBox = new System.Windows.Forms.TextBox();
            this.userEmail_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.error_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.submit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.submitted_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.environmentDetails_TabPage = new System.Windows.Forms.TabPage();
            this.version_TextBox = new System.Windows.Forms.TextBox();
            this.assembly_TextBox = new System.Windows.Forms.TextBox();
            this.address_TextBox = new System.Windows.Forms.TextBox();
            this.hostUser_TextBox = new System.Windows.Forms.TextBox();
            this.hostName_TextBox = new System.Windows.Forms.TextBox();
            this.version_Label = new System.Windows.Forms.Label();
            this.userName_TextBox = new System.Windows.Forms.TextBox();
            this.assembly_Label = new System.Windows.Forms.Label();
            this.address_Label = new System.Windows.Forms.Label();
            this.hostUser_Label = new System.Windows.Forms.Label();
            this.hostName_Label = new System.Windows.Forms.Label();
            this.userName_Label = new System.Windows.Forms.Label();
            this.exceptionDetails_TabPage = new System.Windows.Forms.TabPage();
            this.exception_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.save_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copy_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.exceptionDetail_TextBox = new System.Windows.Forms.TextBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.expandCollapse_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.timestamp_TextBox = new System.Windows.Forms.TextBox();
            this.timestamp_Label = new System.Windows.Forms.Label();
            this.top_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorSymbol_PictureBox)).BeginInit();
            this.bottom_Panel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.submit_TabPage.SuspendLayout();
            this.error_ToolStrip.SuspendLayout();
            this.environmentDetails_TabPage.SuspendLayout();
            this.exceptionDetails_TabPage.SuspendLayout();
            this.exception_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // top_Panel
            // 
            this.top_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.top_Panel.Controls.Add(this.exceptionMessage_TextBox);
            this.top_Panel.Controls.Add(this.preamble_Label);
            this.top_Panel.Controls.Add(this.header_Label);
            this.top_Panel.Controls.Add(this.continue_Button);
            this.top_Panel.Controls.Add(this.quit_Button);
            this.top_Panel.Controls.Add(this.instructions_TextBox);
            this.top_Panel.Controls.Add(this.exceptionDetails_Button);
            this.top_Panel.Controls.Add(this.errorSymbol_PictureBox);
            this.top_Panel.Location = new System.Drawing.Point(0, 0);
            this.top_Panel.MinimumSize = new System.Drawing.Size(530, 212);
            this.top_Panel.Name = "top_Panel";
            this.top_Panel.Size = new System.Drawing.Size(595, 228);
            this.top_Panel.TabIndex = 1;
            // 
            // exceptionMessage_TextBox
            // 
            this.exceptionMessage_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exceptionMessage_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.exceptionMessage_TextBox.Location = new System.Drawing.Point(69, 58);
            this.exceptionMessage_TextBox.Multiline = true;
            this.exceptionMessage_TextBox.Name = "exceptionMessage_TextBox";
            this.exceptionMessage_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.exceptionMessage_TextBox.Size = new System.Drawing.Size(511, 84);
            this.exceptionMessage_TextBox.TabIndex = 12;
            this.exceptionMessage_TextBox.Text = "Error message goes here.\r\nline 2\r\nline 3\r\n";
            // 
            // preamble_Label
            // 
            this.preamble_Label.AutoSize = true;
            this.preamble_Label.Location = new System.Drawing.Point(66, 39);
            this.preamble_Label.Name = "preamble_Label";
            this.preamble_Label.Size = new System.Drawing.Size(182, 15);
            this.preamble_Label.TabIndex = 16;
            this.preamble_Label.Text = "The following error has occurred:";
            // 
            // header_Label
            // 
            this.header_Label.AutoSize = true;
            this.header_Label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header_Label.Location = new System.Drawing.Point(65, 9);
            this.header_Label.Name = "header_Label";
            this.header_Label.Size = new System.Drawing.Size(335, 21);
            this.header_Label.TabIndex = 15;
            this.header_Label.Text = "A serious error has occurred in this application.";
            // 
            // continue_Button
            // 
            this.continue_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.continue_Button.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.continue_Button.Location = new System.Drawing.Point(374, 189);
            this.continue_Button.Name = "continue_Button";
            this.continue_Button.Size = new System.Drawing.Size(100, 28);
            this.continue_Button.TabIndex = 13;
            this.continue_Button.Text = "Continue";
            this.continue_Button.UseVisualStyleBackColor = true;
            // 
            // quit_Button
            // 
            this.quit_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.quit_Button.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.quit_Button.Location = new System.Drawing.Point(480, 189);
            this.quit_Button.Name = "quit_Button";
            this.quit_Button.Size = new System.Drawing.Size(100, 28);
            this.quit_Button.TabIndex = 14;
            this.quit_Button.Text = "Quit";
            this.quit_Button.UseVisualStyleBackColor = true;
            // 
            // instructions_TextBox
            // 
            this.instructions_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.instructions_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.instructions_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.instructions_TextBox.Location = new System.Drawing.Point(69, 143);
            this.instructions_TextBox.Multiline = true;
            this.instructions_TextBox.Name = "instructions_TextBox";
            this.instructions_TextBox.Size = new System.Drawing.Size(511, 45);
            this.instructions_TextBox.TabIndex = 11;
            this.instructions_TextBox.Text = "If you click Continue, the application will attempt to ignore the error. \r\nIf you" +
    " click Quit, the application will close immediately.";
            // 
            // exceptionDetails_Button
            // 
            this.exceptionDetails_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exceptionDetails_Button.Image = ((System.Drawing.Image)(resources.GetObject("exceptionDetails_Button.Image")));
            this.exceptionDetails_Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exceptionDetails_Button.Location = new System.Drawing.Point(11, 189);
            this.exceptionDetails_Button.Name = "exceptionDetails_Button";
            this.exceptionDetails_Button.Size = new System.Drawing.Size(100, 28);
            this.exceptionDetails_Button.TabIndex = 10;
            this.exceptionDetails_Button.Text = " Details";
            this.exceptionDetails_Button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.exceptionDetails_Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.exceptionDetails_Button.UseVisualStyleBackColor = true;
            this.exceptionDetails_Button.Click += new System.EventHandler(this.exceptionDetails_Button_Click);
            // 
            // errorSymbol_PictureBox
            // 
            this.errorSymbol_PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("errorSymbol_PictureBox.Image")));
            this.errorSymbol_PictureBox.Location = new System.Drawing.Point(11, 9);
            this.errorSymbol_PictureBox.Name = "errorSymbol_PictureBox";
            this.errorSymbol_PictureBox.Size = new System.Drawing.Size(48, 48);
            this.errorSymbol_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.errorSymbol_PictureBox.TabIndex = 9;
            this.errorSymbol_PictureBox.TabStop = false;
            // 
            // bottom_Panel
            // 
            this.bottom_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottom_Panel.Controls.Add(this.tabControl);
            this.bottom_Panel.Location = new System.Drawing.Point(0, 234);
            this.bottom_Panel.Name = "bottom_Panel";
            this.bottom_Panel.Size = new System.Drawing.Size(595, 290);
            this.bottom_Panel.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.submit_TabPage);
            this.tabControl.Controls.Add(this.environmentDetails_TabPage);
            this.tabControl.Controls.Add(this.exceptionDetails_TabPage);
            this.tabControl.Location = new System.Drawing.Point(11, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(573, 280);
            this.tabControl.TabIndex = 8;
            // 
            // submit_TabPage
            // 
            this.submit_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.submit_TabPage.Controls.Add(this.userNotes_TextBox);
            this.submit_TabPage.Controls.Add(this.userEmail_TextBox);
            this.submit_TabPage.Controls.Add(this.userEmail_Label);
            this.submit_TabPage.Controls.Add(this.description_Label);
            this.submit_TabPage.Controls.Add(this.error_ToolStrip);
            this.submit_TabPage.Location = new System.Drawing.Point(4, 24);
            this.submit_TabPage.Name = "submit_TabPage";
            this.submit_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.submit_TabPage.Size = new System.Drawing.Size(565, 252);
            this.submit_TabPage.TabIndex = 0;
            this.submit_TabPage.Text = "Submit Error";
            // 
            // userNotes_TextBox
            // 
            this.userNotes_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userNotes_TextBox.Location = new System.Drawing.Point(9, 63);
            this.userNotes_TextBox.Multiline = true;
            this.userNotes_TextBox.Name = "userNotes_TextBox";
            this.userNotes_TextBox.Size = new System.Drawing.Size(546, 98);
            this.userNotes_TextBox.TabIndex = 7;
            // 
            // userEmail_TextBox
            // 
            this.userEmail_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userEmail_TextBox.Location = new System.Drawing.Point(10, 195);
            this.userEmail_TextBox.Name = "userEmail_TextBox";
            this.userEmail_TextBox.Size = new System.Drawing.Size(546, 23);
            this.userEmail_TextBox.TabIndex = 8;
            // 
            // userEmail_Label
            // 
            this.userEmail_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.userEmail_Label.AutoSize = true;
            this.userEmail_Label.Location = new System.Drawing.Point(6, 172);
            this.userEmail_Label.Name = "userEmail_Label";
            this.userEmail_Label.Size = new System.Drawing.Size(407, 15);
            this.userEmail_Label.TabIndex = 9;
            this.userEmail_Label.Text = "(Optional) Enter your email address to receive a copy of the submitted error.";
            // 
            // description_Label
            // 
            this.description_Label.AutoSize = true;
            this.description_Label.Location = new System.Drawing.Point(6, 40);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(431, 15);
            this.description_Label.TabIndex = 6;
            this.description_Label.Text = "Please enter a brief description of what you were doing when this error occurred." +
    "";
            // 
            // error_ToolStrip
            // 
            this.error_ToolStrip.AutoSize = false;
            this.error_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.error_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.error_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submit_ToolStripButton,
            this.submitted_ToolStripButton});
            this.error_ToolStrip.Location = new System.Drawing.Point(3, 3);
            this.error_ToolStrip.Name = "error_ToolStrip";
            this.error_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.error_ToolStrip.Size = new System.Drawing.Size(559, 25);
            this.error_ToolStrip.TabIndex = 5;
            this.error_ToolStrip.Text = "toolStrip1";
            // 
            // submit_ToolStripButton
            // 
            this.submit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("submit_ToolStripButton.Image")));
            this.submit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.submit_ToolStripButton.Name = "submit_ToolStripButton";
            this.submit_ToolStripButton.Size = new System.Drawing.Size(97, 22);
            this.submit_ToolStripButton.Text = "Submit Error";
            this.submit_ToolStripButton.Click += new System.EventHandler(this.submit_ToolStripButton_Click);
            // 
            // submitted_ToolStripButton
            // 
            this.submitted_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("submitted_ToolStripButton.Image")));
            this.submitted_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.submitted_ToolStripButton.Name = "submitted_ToolStripButton";
            this.submitted_ToolStripButton.Size = new System.Drawing.Size(86, 22);
            this.submitted_ToolStripButton.Text = "Submitted";
            this.submitted_ToolStripButton.Visible = false;
            this.submitted_ToolStripButton.Click += new System.EventHandler(this.submitted_ToolStripButton_Click);
            // 
            // environmentDetails_TabPage
            // 
            this.environmentDetails_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.environmentDetails_TabPage.Controls.Add(this.timestamp_TextBox);
            this.environmentDetails_TabPage.Controls.Add(this.version_TextBox);
            this.environmentDetails_TabPage.Controls.Add(this.assembly_TextBox);
            this.environmentDetails_TabPage.Controls.Add(this.address_TextBox);
            this.environmentDetails_TabPage.Controls.Add(this.hostUser_TextBox);
            this.environmentDetails_TabPage.Controls.Add(this.hostName_TextBox);
            this.environmentDetails_TabPage.Controls.Add(this.timestamp_Label);
            this.environmentDetails_TabPage.Controls.Add(this.version_Label);
            this.environmentDetails_TabPage.Controls.Add(this.userName_TextBox);
            this.environmentDetails_TabPage.Controls.Add(this.assembly_Label);
            this.environmentDetails_TabPage.Controls.Add(this.address_Label);
            this.environmentDetails_TabPage.Controls.Add(this.hostUser_Label);
            this.environmentDetails_TabPage.Controls.Add(this.hostName_Label);
            this.environmentDetails_TabPage.Controls.Add(this.userName_Label);
            this.environmentDetails_TabPage.Location = new System.Drawing.Point(4, 24);
            this.environmentDetails_TabPage.Name = "environmentDetails_TabPage";
            this.environmentDetails_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.environmentDetails_TabPage.Size = new System.Drawing.Size(565, 252);
            this.environmentDetails_TabPage.TabIndex = 2;
            this.environmentDetails_TabPage.Text = "Environment Details";
            // 
            // version_TextBox
            // 
            this.version_TextBox.Location = new System.Drawing.Point(139, 180);
            this.version_TextBox.Name = "version_TextBox";
            this.version_TextBox.ReadOnly = true;
            this.version_TextBox.Size = new System.Drawing.Size(180, 23);
            this.version_TextBox.TabIndex = 1;
            // 
            // assembly_TextBox
            // 
            this.assembly_TextBox.Location = new System.Drawing.Point(139, 147);
            this.assembly_TextBox.Name = "assembly_TextBox";
            this.assembly_TextBox.ReadOnly = true;
            this.assembly_TextBox.Size = new System.Drawing.Size(180, 23);
            this.assembly_TextBox.TabIndex = 1;
            // 
            // address_TextBox
            // 
            this.address_TextBox.Location = new System.Drawing.Point(139, 114);
            this.address_TextBox.Name = "address_TextBox";
            this.address_TextBox.ReadOnly = true;
            this.address_TextBox.Size = new System.Drawing.Size(180, 23);
            this.address_TextBox.TabIndex = 1;
            // 
            // hostUser_TextBox
            // 
            this.hostUser_TextBox.Location = new System.Drawing.Point(139, 81);
            this.hostUser_TextBox.Name = "hostUser_TextBox";
            this.hostUser_TextBox.ReadOnly = true;
            this.hostUser_TextBox.Size = new System.Drawing.Size(180, 23);
            this.hostUser_TextBox.TabIndex = 1;
            // 
            // hostName_TextBox
            // 
            this.hostName_TextBox.Location = new System.Drawing.Point(139, 48);
            this.hostName_TextBox.Name = "hostName_TextBox";
            this.hostName_TextBox.ReadOnly = true;
            this.hostName_TextBox.Size = new System.Drawing.Size(180, 23);
            this.hostName_TextBox.TabIndex = 1;
            // 
            // version_Label
            // 
            this.version_Label.AutoSize = true;
            this.version_Label.Location = new System.Drawing.Point(52, 183);
            this.version_Label.Name = "version_Label";
            this.version_Label.Size = new System.Drawing.Size(57, 15);
            this.version_Label.TabIndex = 0;
            this.version_Label.Text = "VERSION:";
            this.version_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userName_TextBox
            // 
            this.userName_TextBox.Location = new System.Drawing.Point(139, 15);
            this.userName_TextBox.Name = "userName_TextBox";
            this.userName_TextBox.ReadOnly = true;
            this.userName_TextBox.Size = new System.Drawing.Size(180, 23);
            this.userName_TextBox.TabIndex = 1;
            // 
            // assembly_Label
            // 
            this.assembly_Label.AutoSize = true;
            this.assembly_Label.Location = new System.Drawing.Point(43, 150);
            this.assembly_Label.Name = "assembly_Label";
            this.assembly_Label.Size = new System.Drawing.Size(66, 15);
            this.assembly_Label.TabIndex = 0;
            this.assembly_Label.Text = "ASSEMBLY:";
            this.assembly_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // address_Label
            // 
            this.address_Label.AutoSize = true;
            this.address_Label.Location = new System.Drawing.Point(50, 117);
            this.address_Label.Name = "address_Label";
            this.address_Label.Size = new System.Drawing.Size(59, 15);
            this.address_Label.TabIndex = 0;
            this.address_Label.Text = "ADDRESS:";
            this.address_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // hostUser_Label
            // 
            this.hostUser_Label.AutoSize = true;
            this.hostUser_Label.Location = new System.Drawing.Point(41, 84);
            this.hostUser_Label.Name = "hostUser_Label";
            this.hostUser_Label.Size = new System.Drawing.Size(68, 15);
            this.hostUser_Label.TabIndex = 0;
            this.hostUser_Label.Text = "HOSTUSER:";
            this.hostUser_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // hostName_Label
            // 
            this.hostName_Label.AutoSize = true;
            this.hostName_Label.Location = new System.Drawing.Point(34, 51);
            this.hostName_Label.Name = "hostName_Label";
            this.hostName_Label.Size = new System.Drawing.Size(75, 15);
            this.hostName_Label.TabIndex = 0;
            this.hostName_Label.Text = "HOSTNAME:";
            this.hostName_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userName_Label
            // 
            this.userName_Label.AutoSize = true;
            this.userName_Label.Location = new System.Drawing.Point(38, 18);
            this.userName_Label.Name = "userName_Label";
            this.userName_Label.Size = new System.Drawing.Size(71, 15);
            this.userName_Label.TabIndex = 0;
            this.userName_Label.Text = "USERNAME:";
            this.userName_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // exceptionDetails_TabPage
            // 
            this.exceptionDetails_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.exceptionDetails_TabPage.Controls.Add(this.exception_ToolStrip);
            this.exceptionDetails_TabPage.Controls.Add(this.exceptionDetail_TextBox);
            this.exceptionDetails_TabPage.Location = new System.Drawing.Point(4, 24);
            this.exceptionDetails_TabPage.Name = "exceptionDetails_TabPage";
            this.exceptionDetails_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.exceptionDetails_TabPage.Size = new System.Drawing.Size(565, 252);
            this.exceptionDetails_TabPage.TabIndex = 1;
            this.exceptionDetails_TabPage.Text = "Exception Details";
            // 
            // exception_ToolStrip
            // 
            this.exception_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.exception_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.exception_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.save_ToolStripButton,
            this.copy_ToolStripButton});
            this.exception_ToolStrip.Location = new System.Drawing.Point(3, 3);
            this.exception_ToolStrip.Name = "exception_ToolStrip";
            this.exception_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.exception_ToolStrip.Size = new System.Drawing.Size(559, 27);
            this.exception_ToolStrip.TabIndex = 1;
            this.exception_ToolStrip.Text = "toolStrip2";
            // 
            // save_ToolStripButton
            // 
            this.save_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("save_ToolStripButton.Image")));
            this.save_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.save_ToolStripButton.Name = "save_ToolStripButton";
            this.save_ToolStripButton.Size = new System.Drawing.Size(90, 24);
            this.save_ToolStripButton.Text = "Save to File";
            this.save_ToolStripButton.Click += new System.EventHandler(this.save_ToolStripButton_Click);
            // 
            // copy_ToolStripButton
            // 
            this.copy_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copy_ToolStripButton.Image")));
            this.copy_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copy_ToolStripButton.Name = "copy_ToolStripButton";
            this.copy_ToolStripButton.Size = new System.Drawing.Size(128, 24);
            this.copy_ToolStripButton.Text = "Copy to Clipboard";
            this.copy_ToolStripButton.Click += new System.EventHandler(this.copy_ToolStripButton_Click);
            // 
            // exceptionDetail_TextBox
            // 
            this.exceptionDetail_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exceptionDetail_TextBox.Location = new System.Drawing.Point(3, 31);
            this.exceptionDetail_TextBox.Multiline = true;
            this.exceptionDetail_TextBox.Name = "exceptionDetail_TextBox";
            this.exceptionDetail_TextBox.ReadOnly = true;
            this.exceptionDetail_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.exceptionDetail_TextBox.Size = new System.Drawing.Size(559, 208);
            this.exceptionDetail_TextBox.TabIndex = 0;
            this.exceptionDetail_TextBox.WordWrap = false;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "log files (*.log)|*.log|All files (*.*)|*.*";
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Title = "Save Exception Detail";
            // 
            // expandCollapse_ImageList
            // 
            this.expandCollapse_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("expandCollapse_ImageList.ImageStream")));
            this.expandCollapse_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.expandCollapse_ImageList.Images.SetKeyName(0, "Expand");
            this.expandCollapse_ImageList.Images.SetKeyName(1, "Collapse");
            // 
            // timestamp_TextBox
            // 
            this.timestamp_TextBox.Location = new System.Drawing.Point(139, 213);
            this.timestamp_TextBox.Name = "timestamp_TextBox";
            this.timestamp_TextBox.ReadOnly = true;
            this.timestamp_TextBox.Size = new System.Drawing.Size(180, 23);
            this.timestamp_TextBox.TabIndex = 1;
            // 
            // timestamp_Label
            // 
            this.timestamp_Label.AutoSize = true;
            this.timestamp_Label.Location = new System.Drawing.Point(50, 216);
            this.timestamp_Label.Name = "timestamp_Label";
            this.timestamp_Label.Size = new System.Drawing.Size(75, 15);
            this.timestamp_Label.TabIndex = 0;
            this.timestamp_Label.Text = "TIMESTAMP:";
            this.timestamp_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 536);
            this.Controls.Add(this.bottom_Panel);
            this.Controls.Add(this.top_Panel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExceptionDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Error";
            this.Resize += new System.EventHandler(this.ExceptionDialog_Resize);
            this.top_Panel.ResumeLayout(false);
            this.top_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorSymbol_PictureBox)).EndInit();
            this.bottom_Panel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.submit_TabPage.ResumeLayout(false);
            this.submit_TabPage.PerformLayout();
            this.error_ToolStrip.ResumeLayout(false);
            this.error_ToolStrip.PerformLayout();
            this.environmentDetails_TabPage.ResumeLayout(false);
            this.environmentDetails_TabPage.PerformLayout();
            this.exceptionDetails_TabPage.ResumeLayout(false);
            this.exceptionDetails_TabPage.PerformLayout();
            this.exception_ToolStrip.ResumeLayout(false);
            this.exception_ToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel top_Panel;
        private System.Windows.Forms.Panel bottom_Panel;
        private System.Windows.Forms.Label preamble_Label;
        private System.Windows.Forms.Label header_Label;
        private System.Windows.Forms.Button continue_Button;
        private System.Windows.Forms.Button quit_Button;
        private System.Windows.Forms.TextBox instructions_TextBox;
        private System.Windows.Forms.TextBox exceptionMessage_TextBox;
        private System.Windows.Forms.Button exceptionDetails_Button;
        private System.Windows.Forms.PictureBox errorSymbol_PictureBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage submit_TabPage;
        private System.Windows.Forms.Label userEmail_Label;
        private System.Windows.Forms.TextBox userEmail_TextBox;
        private System.Windows.Forms.TextBox userNotes_TextBox;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.ToolStrip error_ToolStrip;
        private System.Windows.Forms.ToolStripButton submit_ToolStripButton;
        private System.Windows.Forms.ToolStripButton submitted_ToolStripButton;
        private System.Windows.Forms.TabPage environmentDetails_TabPage;
        private System.Windows.Forms.TextBox version_TextBox;
        private System.Windows.Forms.TextBox assembly_TextBox;
        private System.Windows.Forms.TextBox address_TextBox;
        private System.Windows.Forms.TextBox hostUser_TextBox;
        private System.Windows.Forms.TextBox hostName_TextBox;
        private System.Windows.Forms.Label version_Label;
        private System.Windows.Forms.TextBox userName_TextBox;
        private System.Windows.Forms.Label assembly_Label;
        private System.Windows.Forms.Label address_Label;
        private System.Windows.Forms.Label hostUser_Label;
        private System.Windows.Forms.Label hostName_Label;
        private System.Windows.Forms.Label userName_Label;
        private System.Windows.Forms.TabPage exceptionDetails_TabPage;
        private System.Windows.Forms.ToolStrip exception_ToolStrip;
        private System.Windows.Forms.ToolStripButton save_ToolStripButton;
        private System.Windows.Forms.ToolStripButton copy_ToolStripButton;
        private System.Windows.Forms.TextBox exceptionDetail_TextBox;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ImageList expandCollapse_ImageList;
        private System.Windows.Forms.TextBox timestamp_TextBox;
        private System.Windows.Forms.Label timestamp_Label;
    }
}
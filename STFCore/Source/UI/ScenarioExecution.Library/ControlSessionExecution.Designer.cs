namespace HP.ScalableTest.UI.SessionExecution
{
    partial class ControlSessionExecution
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlSessionExecution));
            this.execution_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.pause_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.resume_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.repeat_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.end_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dispatcherLog_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.notes_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sessionStatus_GroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sessionEndLabel = new System.Windows.Forms.Label();
            this.sessionStartLabel = new System.Windows.Forms.Label();
            this.sessionEndPrompt = new System.Windows.Forms.Label();
            this.sessionNamePrompt = new System.Windows.Forms.Label();
            this.sessionIdPrompt = new System.Windows.Forms.Label();
            this.sessionStatePrompt = new System.Windows.Forms.Label();
            this.sessionStateLabel = new System.Windows.Forms.Label();
            this.sessionNameLabel = new System.Windows.Forms.Label();
            this.sessionOwnerLabel = new System.Windows.Forms.Label();
            this.sessionOwnerPrompt = new System.Windows.Forms.Label();
            this.sessionIdLabel = new System.Windows.Forms.Label();
            this.sessionStartPrompt = new System.Windows.Forms.Label();
            this.sessionTotalActivities = new System.Windows.Forms.Label();
            this.sessionTotalActivitiesLabel = new System.Windows.Forms.Label();
            this.sessionFail = new System.Windows.Forms.Label();
            this.sessionFailLabel = new System.Windows.Forms.Label();
            this.sessionSkip = new System.Windows.Forms.Label();
            this.sessionSkipLabel = new System.Windows.Forms.Label();
            this.sessionPassLabel = new System.Windows.Forms.Label();
            this.sessionError = new System.Windows.Forms.Label();
            this.sessionErrorLabel = new System.Windows.Forms.Label();
            this.sessionRetry = new System.Windows.Forms.Label();
            this.sessionRetryLabel = new System.Windows.Forms.Label();
            this.sessionPass = new System.Windows.Forms.Label();
            this.execution_ToolStrip.SuspendLayout();
            this.sessionStatus_GroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // execution_ToolStrip
            // 
            this.execution_ToolStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.execution_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.execution_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.execution_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pause_ToolStripButton,
            this.resume_ToolStripButton,
            this.repeat_ToolStripButton,
            this.end_ToolStripButton,
            this.refresh_ToolStripButton,
            this.dispatcherLog_ToolStripButton,
            this.notes_ToolStripButton});
            this.execution_ToolStrip.Location = new System.Drawing.Point(3, 0);
            this.execution_ToolStrip.Name = "execution_ToolStrip";
            this.execution_ToolStrip.Size = new System.Drawing.Size(654, 27);
            this.execution_ToolStrip.TabIndex = 0;
            this.execution_ToolStrip.Text = "toolStrip1";
            // 
            // pause_ToolStripButton
            // 
            this.pause_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pause_ToolStripButton.Image")));
            this.pause_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pause_ToolStripButton.Name = "pause_ToolStripButton";
            this.pause_ToolStripButton.Size = new System.Drawing.Size(116, 24);
            this.pause_ToolStripButton.Text = "Pause Execution";
            this.pause_ToolStripButton.ToolTipText = "Pause execution of the currently running test session";
            this.pause_ToolStripButton.Click += new System.EventHandler(this.pause_ToolStripButton_Click);
            // 
            // resume_ToolStripButton
            // 
            this.resume_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("resume_ToolStripButton.Image")));
            this.resume_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resume_ToolStripButton.Name = "resume_ToolStripButton";
            this.resume_ToolStripButton.Size = new System.Drawing.Size(127, 24);
            this.resume_ToolStripButton.Text = "Resume Execution";
            this.resume_ToolStripButton.ToolTipText = "Resume execution of the currently running test session";
            this.resume_ToolStripButton.Visible = false;
            this.resume_ToolStripButton.Click += new System.EventHandler(this.resume_ToolStripButton_Click);
            // 
            // repeat_ToolStripButton
            // 
            this.repeat_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("repeat_ToolStripButton.Image")));
            this.repeat_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.repeat_ToolStripButton.Name = "repeat_ToolStripButton";
            this.repeat_ToolStripButton.Size = new System.Drawing.Size(109, 24);
            this.repeat_ToolStripButton.Text = "Repeat Session";
            this.repeat_ToolStripButton.ToolTipText = "Repeat the test session that just completed";
            this.repeat_ToolStripButton.Visible = false;
            this.repeat_ToolStripButton.Click += new System.EventHandler(this.repeat_ToolStripButton_Click);
            // 
            // end_ToolStripButton
            // 
            this.end_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("end_ToolStripButton.Image")));
            this.end_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.end_ToolStripButton.Name = "end_ToolStripButton";
            this.end_ToolStripButton.Size = new System.Drawing.Size(93, 24);
            this.end_ToolStripButton.Text = "End Session";
            this.end_ToolStripButton.ToolTipText = "End the currently running test session";
            this.end_ToolStripButton.Click += new System.EventHandler(this.end_ToolStripButton_Click);
            // 
            // refresh_ToolStripButton
            // 
            this.refresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refresh_ToolStripButton.Image")));
            this.refresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh_ToolStripButton.Name = "refresh_ToolStripButton";
            this.refresh_ToolStripButton.Size = new System.Drawing.Size(70, 24);
            this.refresh_ToolStripButton.Text = "Refresh";
            this.refresh_ToolStripButton.ToolTipText = "Refresh status information for the currently running test session";
            this.refresh_ToolStripButton.Click += new System.EventHandler(this.refresh_ToolStripButton_Click);
            // 
            // dispatcherLog_ToolStripButton
            // 
            this.dispatcherLog_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("dispatcherLog_ToolStripButton.Image")));
            this.dispatcherLog_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dispatcherLog_ToolStripButton.Name = "dispatcherLog_ToolStripButton";
            this.dispatcherLog_ToolStripButton.Size = new System.Drawing.Size(93, 24);
            this.dispatcherLog_ToolStripButton.Text = "Session Log";
            this.dispatcherLog_ToolStripButton.ToolTipText = "View log information for the currently running test session";
            this.dispatcherLog_ToolStripButton.Click += new System.EventHandler(this.dispatcherLog_ToolStripButton_Click);
            // 
            // notes_ToolStripButton
            // 
            this.notes_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("notes_ToolStripButton.Image")));
            this.notes_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.notes_ToolStripButton.Name = "notes_ToolStripButton";
            this.notes_ToolStripButton.Size = new System.Drawing.Size(62, 24);
            this.notes_ToolStripButton.Text = "Notes";
            this.notes_ToolStripButton.ToolTipText = "View/Edit notes for the currently running test session";
            this.notes_ToolStripButton.Click += new System.EventHandler(this.notes_ToolStripButton_Click);
            // 
            // sessionStatus_GroupBox
            // 
            this.sessionStatus_GroupBox.Controls.Add(this.tableLayoutPanel1);
            this.sessionStatus_GroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionStatus_GroupBox.Location = new System.Drawing.Point(3, 27);
            this.sessionStatus_GroupBox.Name = "sessionStatus_GroupBox";
            this.sessionStatus_GroupBox.Size = new System.Drawing.Size(654, 119);
            this.sessionStatus_GroupBox.TabIndex = 0;
            this.sessionStatus_GroupBox.TabStop = false;
            this.sessionStatus_GroupBox.Text = "Session Status";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.sessionEndLabel, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.sessionStartLabel, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.sessionEndPrompt, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.sessionNamePrompt, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.sessionIdPrompt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sessionStatePrompt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.sessionStateLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.sessionNameLabel, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.sessionOwnerLabel, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.sessionOwnerPrompt, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.sessionIdLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.sessionStartPrompt, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.sessionTotalActivities, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.sessionTotalActivitiesLabel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.sessionFail, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.sessionFailLabel, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.sessionSkip, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.sessionSkipLabel, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.sessionPassLabel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.sessionError, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.sessionErrorLabel, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.sessionRetry, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.sessionRetryLabel, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.sessionPass, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(648, 97);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // sessionEndLabel
            // 
            this.sessionEndLabel.AutoSize = true;
            this.sessionEndLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionEndLabel.Location = new System.Drawing.Point(359, 27);
            this.sessionEndLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionEndLabel.Name = "sessionEndLabel";
            this.sessionEndLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionEndLabel.TabIndex = 11;
            this.sessionEndLabel.Text = "---";
            // 
            // sessionStartLabel
            // 
            this.sessionStartLabel.AutoSize = true;
            this.sessionStartLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionStartLabel.Location = new System.Drawing.Point(359, 3);
            this.sessionStartLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionStartLabel.Name = "sessionStartLabel";
            this.sessionStartLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionStartLabel.TabIndex = 5;
            this.sessionStartLabel.Text = "---";
            // 
            // sessionEndPrompt
            // 
            this.sessionEndPrompt.AutoSize = true;
            this.sessionEndPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionEndPrompt.Location = new System.Drawing.Point(279, 27);
            this.sessionEndPrompt.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionEndPrompt.Name = "sessionEndPrompt";
            this.sessionEndPrompt.Size = new System.Drawing.Size(74, 15);
            this.sessionEndPrompt.TabIndex = 10;
            this.sessionEndPrompt.Text = "End (Est.):";
            this.sessionEndPrompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionNamePrompt
            // 
            this.sessionNamePrompt.AutoSize = true;
            this.sessionNamePrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionNamePrompt.Location = new System.Drawing.Point(131, 3);
            this.sessionNamePrompt.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionNamePrompt.Name = "sessionNamePrompt";
            this.sessionNamePrompt.Size = new System.Drawing.Size(114, 15);
            this.sessionNamePrompt.TabIndex = 2;
            this.sessionNamePrompt.Text = "Session Name:";
            this.sessionNamePrompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionIdPrompt
            // 
            this.sessionIdPrompt.AutoSize = true;
            this.sessionIdPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionIdPrompt.Location = new System.Drawing.Point(3, 3);
            this.sessionIdPrompt.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionIdPrompt.Name = "sessionIdPrompt";
            this.sessionIdPrompt.Size = new System.Drawing.Size(94, 15);
            this.sessionIdPrompt.TabIndex = 0;
            this.sessionIdPrompt.Text = "Session ID:";
            this.sessionIdPrompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionStatePrompt
            // 
            this.sessionStatePrompt.AutoSize = true;
            this.sessionStatePrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionStatePrompt.Location = new System.Drawing.Point(3, 27);
            this.sessionStatePrompt.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionStatePrompt.Name = "sessionStatePrompt";
            this.sessionStatePrompt.Size = new System.Drawing.Size(94, 15);
            this.sessionStatePrompt.TabIndex = 6;
            this.sessionStatePrompt.Text = "Session State:";
            this.sessionStatePrompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionStateLabel
            // 
            this.sessionStateLabel.AutoSize = true;
            this.sessionStateLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionStateLabel.Location = new System.Drawing.Point(103, 27);
            this.sessionStateLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionStateLabel.Name = "sessionStateLabel";
            this.sessionStateLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionStateLabel.TabIndex = 7;
            this.sessionStateLabel.Text = "---";
            this.sessionStateLabel.TextChanged += new System.EventHandler(this.sessionStateLabel_TextChanged);
            // 
            // sessionNameLabel
            // 
            this.sessionNameLabel.AutoEllipsis = true;
            this.sessionNameLabel.AutoSize = true;
            this.sessionNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionNameLabel.Location = new System.Drawing.Point(251, 3);
            this.sessionNameLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionNameLabel.Name = "sessionNameLabel";
            this.sessionNameLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionNameLabel.TabIndex = 3;
            this.sessionNameLabel.Text = "---";
            // 
            // sessionOwnerLabel
            // 
            this.sessionOwnerLabel.AutoEllipsis = true;
            this.sessionOwnerLabel.AutoSize = true;
            this.sessionOwnerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionOwnerLabel.Location = new System.Drawing.Point(251, 27);
            this.sessionOwnerLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionOwnerLabel.Name = "sessionOwnerLabel";
            this.sessionOwnerLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionOwnerLabel.TabIndex = 9;
            this.sessionOwnerLabel.Text = "---";
            // 
            // sessionOwnerPrompt
            // 
            this.sessionOwnerPrompt.AutoSize = true;
            this.sessionOwnerPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionOwnerPrompt.Location = new System.Drawing.Point(131, 27);
            this.sessionOwnerPrompt.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionOwnerPrompt.Name = "sessionOwnerPrompt";
            this.sessionOwnerPrompt.Size = new System.Drawing.Size(114, 15);
            this.sessionOwnerPrompt.TabIndex = 8;
            this.sessionOwnerPrompt.Text = "Session Owner:";
            this.sessionOwnerPrompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionIdLabel
            // 
            this.sessionIdLabel.AutoSize = true;
            this.sessionIdLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionIdLabel.Location = new System.Drawing.Point(103, 3);
            this.sessionIdLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionIdLabel.Name = "sessionIdLabel";
            this.sessionIdLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionIdLabel.TabIndex = 1;
            this.sessionIdLabel.Text = "---";
            // 
            // sessionStartPrompt
            // 
            this.sessionStartPrompt.AutoSize = true;
            this.sessionStartPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionStartPrompt.Location = new System.Drawing.Point(279, 3);
            this.sessionStartPrompt.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionStartPrompt.Name = "sessionStartPrompt";
            this.sessionStartPrompt.Size = new System.Drawing.Size(74, 15);
            this.sessionStartPrompt.TabIndex = 4;
            this.sessionStartPrompt.Text = "Started:";
            this.sessionStartPrompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionTotalActivities
            // 
            this.sessionTotalActivities.AutoSize = true;
            this.sessionTotalActivities.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionTotalActivities.Location = new System.Drawing.Point(3, 51);
            this.sessionTotalActivities.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionTotalActivities.Name = "sessionTotalActivities";
            this.sessionTotalActivities.Size = new System.Drawing.Size(94, 15);
            this.sessionTotalActivities.TabIndex = 12;
            this.sessionTotalActivities.Text = "# Activities:";
            this.sessionTotalActivities.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionTotalActivitiesLabel
            // 
            this.sessionTotalActivitiesLabel.AutoSize = true;
            this.sessionTotalActivitiesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionTotalActivitiesLabel.Location = new System.Drawing.Point(103, 51);
            this.sessionTotalActivitiesLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionTotalActivitiesLabel.Name = "sessionTotalActivitiesLabel";
            this.sessionTotalActivitiesLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionTotalActivitiesLabel.TabIndex = 13;
            this.sessionTotalActivitiesLabel.Text = "---";
            this.sessionTotalActivitiesLabel.Click += new System.EventHandler(this.activityCount_Click);
            // 
            // sessionFail
            // 
            this.sessionFail.AutoSize = true;
            this.sessionFail.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionFail.Location = new System.Drawing.Point(131, 51);
            this.sessionFail.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionFail.Name = "sessionFail";
            this.sessionFail.Size = new System.Drawing.Size(114, 15);
            this.sessionFail.TabIndex = 14;
            this.sessionFail.Text = "# Fail:";
            this.sessionFail.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionFailLabel
            // 
            this.sessionFailLabel.AutoSize = true;
            this.sessionFailLabel.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionFailLabel.Location = new System.Drawing.Point(251, 51);
            this.sessionFailLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionFailLabel.Name = "sessionFailLabel";
            this.sessionFailLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionFailLabel.TabIndex = 15;
            this.sessionFailLabel.Text = "---";
            this.sessionFailLabel.Click += new System.EventHandler(this.activityCount_Click);
            // 
            // sessionSkip
            // 
            this.sessionSkip.AutoSize = true;
            this.sessionSkip.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionSkip.Location = new System.Drawing.Point(279, 51);
            this.sessionSkip.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionSkip.Name = "sessionSkip";
            this.sessionSkip.Size = new System.Drawing.Size(74, 15);
            this.sessionSkip.TabIndex = 16;
            this.sessionSkip.Text = "# Skip:";
            this.sessionSkip.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionSkipLabel
            // 
            this.sessionSkipLabel.AutoSize = true;
            this.sessionSkipLabel.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionSkipLabel.Location = new System.Drawing.Point(359, 51);
            this.sessionSkipLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionSkipLabel.Name = "sessionSkipLabel";
            this.sessionSkipLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionSkipLabel.TabIndex = 17;
            this.sessionSkipLabel.Text = "---";
            this.sessionSkipLabel.Click += new System.EventHandler(this.activityCount_Click);
            // 
            // sessionPassLabel
            // 
            this.sessionPassLabel.AutoSize = true;
            this.sessionPassLabel.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionPassLabel.Location = new System.Drawing.Point(103, 75);
            this.sessionPassLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionPassLabel.Name = "sessionPassLabel";
            this.sessionPassLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionPassLabel.TabIndex = 19;
            this.sessionPassLabel.Text = "---";
            this.sessionPassLabel.Click += new System.EventHandler(this.activityCount_Click);
            // 
            // sessionError
            // 
            this.sessionError.AutoSize = true;
            this.sessionError.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionError.Location = new System.Drawing.Point(131, 75);
            this.sessionError.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionError.Name = "sessionError";
            this.sessionError.Size = new System.Drawing.Size(114, 15);
            this.sessionError.TabIndex = 20;
            this.sessionError.Text = "# Error:";
            this.sessionError.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionErrorLabel
            // 
            this.sessionErrorLabel.AutoSize = true;
            this.sessionErrorLabel.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionErrorLabel.Location = new System.Drawing.Point(251, 75);
            this.sessionErrorLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionErrorLabel.Name = "sessionErrorLabel";
            this.sessionErrorLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionErrorLabel.TabIndex = 21;
            this.sessionErrorLabel.Text = "---";
            this.sessionErrorLabel.Click += new System.EventHandler(this.activityCount_Click);
            // 
            // sessionRetry
            // 
            this.sessionRetry.AutoSize = true;
            this.sessionRetry.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionRetry.Location = new System.Drawing.Point(279, 75);
            this.sessionRetry.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionRetry.Name = "sessionRetry";
            this.sessionRetry.Size = new System.Drawing.Size(74, 15);
            this.sessionRetry.TabIndex = 22;
            this.sessionRetry.Text = "# Retry:";
            this.sessionRetry.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionRetryLabel
            // 
            this.sessionRetryLabel.AutoSize = true;
            this.sessionRetryLabel.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionRetryLabel.Location = new System.Drawing.Point(359, 75);
            this.sessionRetryLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionRetryLabel.Name = "sessionRetryLabel";
            this.sessionRetryLabel.Size = new System.Drawing.Size(22, 15);
            this.sessionRetryLabel.TabIndex = 23;
            this.sessionRetryLabel.Text = "---";
            this.sessionRetryLabel.Click += new System.EventHandler(this.activityCount_Click);
            // 
            // sessionPass
            // 
            this.sessionPass.AutoSize = true;
            this.sessionPass.Dock = System.Windows.Forms.DockStyle.Top;
            this.sessionPass.Location = new System.Drawing.Point(3, 75);
            this.sessionPass.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.sessionPass.Name = "sessionPass";
            this.sessionPass.Size = new System.Drawing.Size(94, 15);
            this.sessionPass.TabIndex = 18;
            this.sessionPass.Text = "# Pass:";
            this.sessionPass.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ControlSessionExecution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sessionStatus_GroupBox);
            this.Controls.Add(this.execution_ToolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ControlSessionExecution";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Size = new System.Drawing.Size(660, 146);
            this.execution_ToolStrip.ResumeLayout(false);
            this.execution_ToolStrip.PerformLayout();
            this.sessionStatus_GroupBox.ResumeLayout(false);
            this.sessionStatus_GroupBox.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip execution_ToolStrip;
        private System.Windows.Forms.ToolStripButton pause_ToolStripButton;
        private System.Windows.Forms.ToolStripButton resume_ToolStripButton;
        private System.Windows.Forms.ToolStripButton repeat_ToolStripButton;
        private System.Windows.Forms.ToolStripButton end_ToolStripButton;
        private System.Windows.Forms.GroupBox sessionStatus_GroupBox;
        private System.Windows.Forms.Label sessionStateLabel;
        private System.Windows.Forms.Label sessionStatePrompt;
        private System.Windows.Forms.Label sessionIdPrompt;
        private System.Windows.Forms.Label sessionOwnerPrompt;
        private System.Windows.Forms.Label sessionNamePrompt;
        private System.Windows.Forms.Label sessionOwnerLabel;
        private System.Windows.Forms.Label sessionNameLabel;
        private System.Windows.Forms.ToolStripButton refresh_ToolStripButton;
        private System.Windows.Forms.ToolStripButton dispatcherLog_ToolStripButton;
        private System.Windows.Forms.ToolStripButton notes_ToolStripButton;
        private System.Windows.Forms.Label sessionEndPrompt;
        private System.Windows.Forms.Label sessionStartPrompt;
        private System.Windows.Forms.Label sessionEndLabel;
        private System.Windows.Forms.Label sessionStartLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label sessionIdLabel;
        private System.Windows.Forms.Label sessionTotalActivities;
        private System.Windows.Forms.Label sessionTotalActivitiesLabel;
        private System.Windows.Forms.Label sessionFail;
        private System.Windows.Forms.Label sessionFailLabel;
        private System.Windows.Forms.Label sessionSkip;
        private System.Windows.Forms.Label sessionSkipLabel;
        private System.Windows.Forms.Label sessionPassLabel;
        private System.Windows.Forms.Label sessionError;
        private System.Windows.Forms.Label sessionErrorLabel;
        private System.Windows.Forms.Label sessionRetry;
        private System.Windows.Forms.Label sessionRetryLabel;
        private System.Windows.Forms.Label sessionPass;
    }
}

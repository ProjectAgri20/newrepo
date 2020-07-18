namespace HP.ScalableTest.UI.SessionExecution
{
    partial class SessionShutdownForm
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
            this.shutDown_Label = new System.Windows.Forms.Label();
            this.shutDown_Button = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.shutDown_CheckBox = new System.Windows.Forms.CheckBox();
            this.powerOff_RadioButton = new System.Windows.Forms.RadioButton();
            this.revert_RadioButton = new System.Windows.Forms.RadioButton();
            this.disableCrc_CheckBox = new System.Windows.Forms.CheckBox();
            this.shutdownSimulator_CheckBox = new System.Windows.Forms.CheckBox();
            this.purgeQueues_checkBox = new System.Windows.Forms.CheckBox();
            this.workerFinish_CheckBox = new System.Windows.Forms.CheckBox();
            this.EventLogs_CheckBox = new System.Windows.Forms.CheckBox();
            this.copyLogs_CheckBox = new System.Windows.Forms.CheckBox();
            this.panelCopyLogsSTF = new System.Windows.Forms.Panel();
            this.copyLogsLocation_Label = new System.Windows.Forms.Label();
            this.copyLogsInfo_Label = new System.Windows.Forms.Label();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.sessionId_Label = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelCopyLogsSTF.SuspendLayout();
            this.SuspendLayout();
            // 
            // shutDown_Label
            // 
            this.shutDown_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.shutDown_Label.AutoSize = true;
            this.shutDown_Label.Location = new System.Drawing.Point(12, 336);
            this.shutDown_Label.Name = "shutDown_Label";
            this.shutDown_Label.Size = new System.Drawing.Size(341, 15);
            this.shutDown_Label.TabIndex = 23;
            this.shutDown_Label.Text = "Click Shut Down to end the session and clean up test resources.";
            // 
            // shutDown_Button
            // 
            this.shutDown_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shutDown_Button.Location = new System.Drawing.Point(488, 331);
            this.shutDown_Button.Name = "shutDown_Button";
            this.shutDown_Button.Size = new System.Drawing.Size(93, 30);
            this.shutDown_Button.TabIndex = 22;
            this.shutDown_Button.Text = "Shut Down";
            this.shutDown_Button.UseVisualStyleBackColor = true;
            this.shutDown_Button.Click += new System.EventHandler(this.shutDown_Button_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AccessibleName = "";
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.shutDown_CheckBox);
            this.flowLayoutPanel1.Controls.Add(this.powerOff_RadioButton);
            this.flowLayoutPanel1.Controls.Add(this.revert_RadioButton);
            this.flowLayoutPanel1.Controls.Add(this.disableCrc_CheckBox);
            this.flowLayoutPanel1.Controls.Add(this.shutdownSimulator_CheckBox);
            this.flowLayoutPanel1.Controls.Add(this.purgeQueues_checkBox);
            this.flowLayoutPanel1.Controls.Add(this.workerFinish_CheckBox);
            this.flowLayoutPanel1.Controls.Add(this.EventLogs_CheckBox);
            this.flowLayoutPanel1.Controls.Add(this.copyLogs_CheckBox);
            this.flowLayoutPanel1.Controls.Add(this.panelCopyLogsSTF);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 42);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(661, 264);
            this.flowLayoutPanel1.TabIndex = 30;
            // 
            // shutDown_CheckBox
            // 
            this.shutDown_CheckBox.AutoSize = true;
            this.shutDown_CheckBox.Checked = true;
            this.shutDown_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shutDown_CheckBox.Location = new System.Drawing.Point(5, 2);
            this.shutDown_CheckBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.shutDown_CheckBox.Name = "shutDown_CheckBox";
            this.shutDown_CheckBox.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.shutDown_CheckBox.Size = new System.Drawing.Size(206, 24);
            this.shutDown_CheckBox.TabIndex = 11;
            this.shutDown_CheckBox.Text = "Shut down client Virtual Machines";
            this.shutDown_CheckBox.UseVisualStyleBackColor = true;
            this.shutDown_CheckBox.CheckedChanged += new System.EventHandler(this.shutDown_CheckBox_CheckedChanged);
            // 
            // powerOff_RadioButton
            // 
            this.powerOff_RadioButton.AutoSize = true;
            this.powerOff_RadioButton.Checked = true;
            this.powerOff_RadioButton.Location = new System.Drawing.Point(25, 30);
            this.powerOff_RadioButton.Margin = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.powerOff_RadioButton.Name = "powerOff_RadioButton";
            this.powerOff_RadioButton.Size = new System.Drawing.Size(153, 19);
            this.powerOff_RadioButton.TabIndex = 12;
            this.powerOff_RadioButton.TabStop = true;
            this.powerOff_RadioButton.Text = "Power off without revert";
            this.powerOff_RadioButton.UseVisualStyleBackColor = true;
            // 
            // revert_RadioButton
            // 
            this.revert_RadioButton.AutoSize = true;
            this.revert_RadioButton.Location = new System.Drawing.Point(25, 53);
            this.revert_RadioButton.Margin = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.revert_RadioButton.Name = "revert_RadioButton";
            this.revert_RadioButton.Size = new System.Drawing.Size(205, 19);
            this.revert_RadioButton.TabIndex = 13;
            this.revert_RadioButton.Text = "Power off and revert to clean state";
            this.revert_RadioButton.UseVisualStyleBackColor = true;
            // 
            // disableCrc_CheckBox
            // 
            this.disableCrc_CheckBox.AutoSize = true;
            this.disableCrc_CheckBox.Checked = true;
            this.disableCrc_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.disableCrc_CheckBox.Location = new System.Drawing.Point(5, 76);
            this.disableCrc_CheckBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.disableCrc_CheckBox.Name = "disableCrc_CheckBox";
            this.disableCrc_CheckBox.Size = new System.Drawing.Size(281, 19);
            this.disableCrc_CheckBox.TabIndex = 23;
            this.disableCrc_CheckBox.Text = "Disable paperless mode on all applicable devices";
            this.disableCrc_CheckBox.UseVisualStyleBackColor = true;
            // 
            // shutdownSimulator_CheckBox
            // 
            this.shutdownSimulator_CheckBox.AutoSize = true;
            this.shutdownSimulator_CheckBox.Checked = true;
            this.shutdownSimulator_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shutdownSimulator_CheckBox.Location = new System.Drawing.Point(5, 99);
            this.shutdownSimulator_CheckBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.shutdownSimulator_CheckBox.Name = "shutdownSimulator_CheckBox";
            this.shutdownSimulator_CheckBox.Size = new System.Drawing.Size(335, 19);
            this.shutdownSimulator_CheckBox.TabIndex = 22;
            this.shutdownSimulator_CheckBox.Text = "Shut down any Device (Jedi) Simulators used in the session";
            this.shutdownSimulator_CheckBox.UseVisualStyleBackColor = true;
            // 
            // purgeQueues_checkBox
            // 
            this.purgeQueues_checkBox.AutoSize = true;
            this.purgeQueues_checkBox.Checked = true;
            this.purgeQueues_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.purgeQueues_checkBox.Location = new System.Drawing.Point(5, 122);
            this.purgeQueues_checkBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.purgeQueues_checkBox.Name = "purgeQueues_checkBox";
            this.purgeQueues_checkBox.Size = new System.Drawing.Size(172, 19);
            this.purgeQueues_checkBox.TabIndex = 26;
            this.purgeQueues_checkBox.Tag = "purge";
            this.purgeQueues_checkBox.Text = "Purge Remote Print Queues";
            this.purgeQueues_checkBox.UseVisualStyleBackColor = true;
            this.purgeQueues_checkBox.CheckedChanged += new System.EventHandler(this.CheckPurge_CompleteActivity);
            // 
            // workerFinish_CheckBox
            // 
            this.workerFinish_CheckBox.AutoSize = true;
            this.workerFinish_CheckBox.Location = new System.Drawing.Point(5, 145);
            this.workerFinish_CheckBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.workerFinish_CheckBox.Name = "workerFinish_CheckBox";
            this.workerFinish_CheckBox.Size = new System.Drawing.Size(258, 19);
            this.workerFinish_CheckBox.TabIndex = 14;
            this.workerFinish_CheckBox.Tag = "worker";
            this.workerFinish_CheckBox.Text = "Allow resources to complete current activity";
            this.workerFinish_CheckBox.UseVisualStyleBackColor = true;
            this.workerFinish_CheckBox.CheckedChanged += new System.EventHandler(this.CheckPurge_CompleteActivity);
            // 
            // EventLogs_CheckBox
            // 
            this.EventLogs_CheckBox.AutoSize = true;
            this.EventLogs_CheckBox.Location = new System.Drawing.Point(5, 168);
            this.EventLogs_CheckBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.EventLogs_CheckBox.Name = "EventLogs_CheckBox";
            this.EventLogs_CheckBox.Size = new System.Drawing.Size(161, 19);
            this.EventLogs_CheckBox.TabIndex = 25;
            this.EventLogs_CheckBox.Text = "Collect Device Event Logs";
            this.EventLogs_CheckBox.UseVisualStyleBackColor = true;
            // 
            // copyLogs_CheckBox
            // 
            this.copyLogs_CheckBox.AutoSize = true;
            this.copyLogs_CheckBox.Location = new System.Drawing.Point(5, 191);
            this.copyLogs_CheckBox.Margin = new System.Windows.Forms.Padding(5, 2, 2, 2);
            this.copyLogs_CheckBox.Name = "copyLogs_CheckBox";
            this.copyLogs_CheckBox.Size = new System.Drawing.Size(204, 19);
            this.copyLogs_CheckBox.TabIndex = 15;
            this.copyLogs_CheckBox.Text = "Copy log files to dispatcher server";
            this.copyLogs_CheckBox.UseVisualStyleBackColor = true;
            this.copyLogs_CheckBox.CheckedChanged += new System.EventHandler(this.copyLogs_CheckBox_CheckedChanged);
            // 
            // panelCopyLogsSTF
            // 
            this.panelCopyLogsSTF.Controls.Add(this.copyLogsLocation_Label);
            this.panelCopyLogsSTF.Controls.Add(this.copyLogsInfo_Label);
            this.panelCopyLogsSTF.Location = new System.Drawing.Point(2, 212);
            this.panelCopyLogsSTF.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.panelCopyLogsSTF.Name = "panelCopyLogsSTF";
            this.panelCopyLogsSTF.Size = new System.Drawing.Size(655, 32);
            this.panelCopyLogsSTF.TabIndex = 31;
            this.panelCopyLogsSTF.Visible = false;
            // 
            // copyLogsLocation_Label
            // 
            this.copyLogsLocation_Label.AutoSize = true;
            this.copyLogsLocation_Label.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyLogsLocation_Label.Location = new System.Drawing.Point(222, 9);
            this.copyLogsLocation_Label.Margin = new System.Windows.Forms.Padding(25, 0, 2, 0);
            this.copyLogsLocation_Label.Name = "copyLogsLocation_Label";
            this.copyLogsLocation_Label.Size = new System.Drawing.Size(177, 13);
            this.copyLogsLocation_Label.TabIndex = 21;
            this.copyLogsLocation_Label.Text = "<Filled in from System Settings.>";
            this.copyLogsLocation_Label.Visible = false;
            // 
            // copyLogsInfo_Label
            // 
            this.copyLogsInfo_Label.AutoSize = true;
            this.copyLogsInfo_Label.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyLogsInfo_Label.Location = new System.Drawing.Point(35, 9);
            this.copyLogsInfo_Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.copyLogsInfo_Label.Name = "copyLogsInfo_Label";
            this.copyLogsInfo_Label.Size = new System.Drawing.Size(166, 13);
            this.copyLogsInfo_Label.TabIndex = 21;
            this.copyLogsInfo_Label.Text = "Session logs will be copied to: ";
            this.copyLogsInfo_Label.Visible = false;
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(597, 331);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 30);
            this.cancel_Button.TabIndex = 24;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 15);
            this.label1.TabIndex = 25;
            this.label1.Text = "Select options for ending session:";
            // 
            // sessionId_Label
            // 
            this.sessionId_Label.AutoSize = true;
            this.sessionId_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionId_Label.Location = new System.Drawing.Point(248, 9);
            this.sessionId_Label.Name = "sessionId_Label";
            this.sessionId_Label.Size = new System.Drawing.Size(67, 15);
            this.sessionId_Label.TabIndex = 26;
            this.sessionId_Label.Text = "1234ABCD";
            // 
            // SessionShutdownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(684, 371);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.sessionId_Label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.shutDown_Label);
            this.Controls.Add(this.shutDown_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionShutdownForm";
            this.Text = "Session Shutdown";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panelCopyLogsSTF.ResumeLayout(false);
            this.panelCopyLogsSTF.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label shutDown_Label;
        private System.Windows.Forms.Button shutDown_Button;
        private System.Windows.Forms.Label copyLogsLocation_Label;
        private System.Windows.Forms.Label copyLogsInfo_Label;
        private System.Windows.Forms.CheckBox shutDown_CheckBox;
        private System.Windows.Forms.RadioButton powerOff_RadioButton;
        private System.Windows.Forms.RadioButton revert_RadioButton;
        private System.Windows.Forms.CheckBox copyLogs_CheckBox;
        private System.Windows.Forms.CheckBox workerFinish_CheckBox;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sessionId_Label;
        private System.Windows.Forms.CheckBox shutdownSimulator_CheckBox;
        private System.Windows.Forms.CheckBox disableCrc_CheckBox;
        private System.Windows.Forms.CheckBox EventLogs_CheckBox;
        private System.Windows.Forms.CheckBox purgeQueues_checkBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelCopyLogsSTF;
    }
}
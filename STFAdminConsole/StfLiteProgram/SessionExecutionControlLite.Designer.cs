namespace HP.SolutionTest.WorkBench
{
    partial class SessionExecutionControlLite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionExecutionControlLite));
            this.execution_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.start_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pause_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.resume_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.repeat_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.end_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.refresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dispatcherLog_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.notes_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sessionStatus_GroupBox = new System.Windows.Forms.GroupBox();
            this.sessionOwner_Prompt = new System.Windows.Forms.Label();
            this.sessionName_Prompt = new System.Windows.Forms.Label();
            this.sessionOwner_Label = new System.Windows.Forms.Label();
            this.sessionName_Label = new System.Windows.Forms.Label();
            this.sessionId_Label = new System.Windows.Forms.Label();
            this.sessionId_Prompt = new System.Windows.Forms.Label();
            this.sessionState_Label = new System.Windows.Forms.Label();
            this.sessionStat_Prompt = new System.Windows.Forms.Label();
            this.execution_ToolStrip.SuspendLayout();
            this.sessionStatus_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // execution_ToolStrip
            // 
            this.execution_ToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.execution_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.execution_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.start_ToolStripButton,
            this.pause_ToolStripButton,
            this.resume_ToolStripButton,
            this.repeat_ToolStripButton,
            this.end_ToolStripButton,
            this.refresh_ToolStripButton,
            this.dispatcherLog_ToolStripButton,
            this.notes_ToolStripButton});
            this.execution_ToolStrip.Location = new System.Drawing.Point(3, 87);
            this.execution_ToolStrip.Name = "execution_ToolStrip";
            this.execution_ToolStrip.Size = new System.Drawing.Size(659, 27);
            this.execution_ToolStrip.TabIndex = 0;
            this.execution_ToolStrip.Text = "toolStrip1";
            // 
            // start_ToolStripButton
            // 
            this.start_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("start_ToolStripButton.Image")));
            this.start_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.start_ToolStripButton.Name = "start_ToolStripButton";
            this.start_ToolStripButton.Size = new System.Drawing.Size(91, 24);
            this.start_ToolStripButton.Text = "Start Test";
            this.start_ToolStripButton.Click += new System.EventHandler(this.start_ToolStripButton_Click);
            // 
            // pause_ToolStripButton
            // 
            this.pause_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pause_ToolStripButton.Image")));
            this.pause_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pause_ToolStripButton.Name = "pause_ToolStripButton";
            this.pause_ToolStripButton.Size = new System.Drawing.Size(98, 24);
            this.pause_ToolStripButton.Text = "Pause Test";
            this.pause_ToolStripButton.Click += new System.EventHandler(this.pause_ToolStripButton_Click);
            // 
            // resume_ToolStripButton
            // 
            this.resume_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("resume_ToolStripButton.Image")));
            this.resume_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resume_ToolStripButton.Name = "resume_ToolStripButton";
            this.resume_ToolStripButton.Size = new System.Drawing.Size(112, 24);
            this.resume_ToolStripButton.Text = "Resume Test";
            this.resume_ToolStripButton.Visible = false;
            this.resume_ToolStripButton.Click += new System.EventHandler(this.resume_ToolStripButton_Click);
            // 
            // repeat_ToolStripButton
            // 
            this.repeat_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("repeat_ToolStripButton.Image")));
            this.repeat_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.repeat_ToolStripButton.Name = "repeat_ToolStripButton";
            this.repeat_ToolStripButton.Size = new System.Drawing.Size(107, 24);
            this.repeat_ToolStripButton.Text = "Repeat Test";
            this.repeat_ToolStripButton.Visible = false;
            this.repeat_ToolStripButton.Click += new System.EventHandler(this.repeat_ToolStripButton_Click);
            // 
            // end_ToolStripButton
            // 
            this.end_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("end_ToolStripButton.Image")));
            this.end_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.end_ToolStripButton.Name = "end_ToolStripButton";
            this.end_ToolStripButton.Size = new System.Drawing.Size(85, 24);
            this.end_ToolStripButton.Text = "End Test";
            this.end_ToolStripButton.Click += new System.EventHandler(this.end_ToolStripButton_Click);
            // 
            // refresh_ToolStripButton
            // 
            this.refresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refresh_ToolStripButton.Image")));
            this.refresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh_ToolStripButton.Name = "refresh_ToolStripButton";
            this.refresh_ToolStripButton.Size = new System.Drawing.Size(78, 24);
            this.refresh_ToolStripButton.Text = "Refresh";
            this.refresh_ToolStripButton.Click += new System.EventHandler(this.refresh_ToolStripButton_Click);
            // 
            // dispatcherLog_ToolStripButton
            // 
            this.dispatcherLog_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("dispatcherLog_ToolStripButton.Image")));
            this.dispatcherLog_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dispatcherLog_ToolStripButton.Name = "dispatcherLog_ToolStripButton";
            this.dispatcherLog_ToolStripButton.Size = new System.Drawing.Size(56, 24);
            this.dispatcherLog_ToolStripButton.Text = "Log File";
            this.dispatcherLog_ToolStripButton.Click += new System.EventHandler(this.dispatcherLog_ToolStripButton_Click);
            // 
            // notes_ToolStripButton
            // 
            this.notes_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("notes_ToolStripButton.Image")));
            this.notes_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.notes_ToolStripButton.Name = "notes_ToolStripButton";
            this.notes_ToolStripButton.Size = new System.Drawing.Size(68, 24);
            this.notes_ToolStripButton.Text = "Notes";
            this.notes_ToolStripButton.Click += new System.EventHandler(this.notes_ToolStripButton_Click);
            // 
            // sessionStatus_GroupBox
            // 
            this.sessionStatus_GroupBox.Controls.Add(this.sessionOwner_Prompt);
            this.sessionStatus_GroupBox.Controls.Add(this.sessionName_Prompt);
            this.sessionStatus_GroupBox.Controls.Add(this.sessionOwner_Label);
            this.sessionStatus_GroupBox.Controls.Add(this.sessionName_Label);
            this.sessionStatus_GroupBox.Controls.Add(this.sessionId_Label);
            this.sessionStatus_GroupBox.Controls.Add(this.sessionId_Prompt);
            this.sessionStatus_GroupBox.Controls.Add(this.sessionState_Label);
            this.sessionStatus_GroupBox.Controls.Add(this.sessionStat_Prompt);
            this.sessionStatus_GroupBox.Controls.Add(this.execution_ToolStrip);
            this.sessionStatus_GroupBox.Location = new System.Drawing.Point(3, 3);
            this.sessionStatus_GroupBox.Name = "sessionStatus_GroupBox";
            this.sessionStatus_GroupBox.Size = new System.Drawing.Size(665, 117);
            this.sessionStatus_GroupBox.TabIndex = 2;
            this.sessionStatus_GroupBox.TabStop = false;
            this.sessionStatus_GroupBox.Text = "Test Session Status";
            // 
            // sessionOwner_Prompt
            // 
            this.sessionOwner_Prompt.Location = new System.Drawing.Point(255, 54);
            this.sessionOwner_Prompt.Name = "sessionOwner_Prompt";
            this.sessionOwner_Prompt.Size = new System.Drawing.Size(108, 20);
            this.sessionOwner_Prompt.TabIndex = 23;
            this.sessionOwner_Prompt.Text = "Session Owner:";
            this.sessionOwner_Prompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionName_Prompt
            // 
            this.sessionName_Prompt.Location = new System.Drawing.Point(258, 28);
            this.sessionName_Prompt.Name = "sessionName_Prompt";
            this.sessionName_Prompt.Size = new System.Drawing.Size(105, 20);
            this.sessionName_Prompt.TabIndex = 22;
            this.sessionName_Prompt.Text = "Session Name:";
            this.sessionName_Prompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionOwner_Label
            // 
            this.sessionOwner_Label.AutoSize = true;
            this.sessionOwner_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionOwner_Label.Location = new System.Drawing.Point(369, 54);
            this.sessionOwner_Label.Name = "sessionOwner_Label";
            this.sessionOwner_Label.Size = new System.Drawing.Size(27, 20);
            this.sessionOwner_Label.TabIndex = 25;
            this.sessionOwner_Label.Text = "---";
            // 
            // sessionName_Label
            // 
            this.sessionName_Label.AutoSize = true;
            this.sessionName_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionName_Label.Location = new System.Drawing.Point(369, 28);
            this.sessionName_Label.Name = "sessionName_Label";
            this.sessionName_Label.Size = new System.Drawing.Size(27, 20);
            this.sessionName_Label.TabIndex = 24;
            this.sessionName_Label.Text = "---";
            // 
            // sessionId_Label
            // 
            this.sessionId_Label.AutoSize = true;
            this.sessionId_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionId_Label.Location = new System.Drawing.Point(121, 28);
            this.sessionId_Label.Name = "sessionId_Label";
            this.sessionId_Label.Size = new System.Drawing.Size(27, 20);
            this.sessionId_Label.TabIndex = 21;
            this.sessionId_Label.Text = "---";
            // 
            // sessionId_Prompt
            // 
            this.sessionId_Prompt.Location = new System.Drawing.Point(35, 28);
            this.sessionId_Prompt.Name = "sessionId_Prompt";
            this.sessionId_Prompt.Size = new System.Drawing.Size(80, 20);
            this.sessionId_Prompt.TabIndex = 20;
            this.sessionId_Prompt.Text = "Session ID:";
            this.sessionId_Prompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionState_Label
            // 
            this.sessionState_Label.AutoSize = true;
            this.sessionState_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionState_Label.Location = new System.Drawing.Point(121, 54);
            this.sessionState_Label.Name = "sessionState_Label";
            this.sessionState_Label.Size = new System.Drawing.Size(27, 20);
            this.sessionState_Label.TabIndex = 19;
            this.sessionState_Label.Text = "---";
            // 
            // sessionStat_Prompt
            // 
            this.sessionStat_Prompt.Location = new System.Drawing.Point(16, 54);
            this.sessionStat_Prompt.Name = "sessionStat_Prompt";
            this.sessionStat_Prompt.Size = new System.Drawing.Size(99, 20);
            this.sessionStat_Prompt.TabIndex = 18;
            this.sessionStat_Prompt.Text = "Session State:";
            this.sessionStat_Prompt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SessionExecutionControlLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sessionStatus_GroupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionExecutionControlLite";
            this.Size = new System.Drawing.Size(672, 124);
            this.execution_ToolStrip.ResumeLayout(false);
            this.execution_ToolStrip.PerformLayout();
            this.sessionStatus_GroupBox.ResumeLayout(false);
            this.sessionStatus_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip execution_ToolStrip;
        private System.Windows.Forms.ToolStripButton start_ToolStripButton;
        private System.Windows.Forms.ToolStripButton pause_ToolStripButton;
        private System.Windows.Forms.ToolStripButton resume_ToolStripButton;
        private System.Windows.Forms.ToolStripButton repeat_ToolStripButton;
        private System.Windows.Forms.ToolStripButton end_ToolStripButton;
        private System.Windows.Forms.GroupBox sessionStatus_GroupBox;
        private System.Windows.Forms.Label sessionState_Label;
        private System.Windows.Forms.Label sessionStat_Prompt;
        private System.Windows.Forms.Label sessionId_Label;
        private System.Windows.Forms.Label sessionId_Prompt;
        private System.Windows.Forms.Label sessionOwner_Prompt;
        private System.Windows.Forms.Label sessionName_Prompt;
        private System.Windows.Forms.Label sessionOwner_Label;
        private System.Windows.Forms.Label sessionName_Label;
        private System.Windows.Forms.ToolStripButton refresh_ToolStripButton;
        private System.Windows.Forms.ToolStripButton dispatcherLog_ToolStripButton;
        private System.Windows.Forms.ToolStripButton notes_ToolStripButton;

    }
}

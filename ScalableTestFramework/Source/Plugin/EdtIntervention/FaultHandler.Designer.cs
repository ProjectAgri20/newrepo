namespace HP.ScalableTest.Plugin.EdtIntervention
{
    partial class FaultHandler
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbFaultType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSubType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbOperProgress = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbTimeToReady = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbRecovery = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbDisposition = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLink = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbComments = new System.Windows.Forms.TextBox();
            this.tbDescript = new System.Windows.Forms.TextBox();
            this.cbFaultCode = new System.Windows.Forms.ComboBox();
            this.ckbLinkEvent = new System.Windows.Forms.CheckBox();
            this.cbRootcauseId = new System.Windows.Forms.ComboBox();
            this.rootcauseId_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 100);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fault Type:";
            // 
            // cbFaultType
            // 
            this.cbFaultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFaultType.FormattingEnabled = true;
            this.cbFaultType.Items.AddRange(new object[] {
            "Error",
            "Jam"});
            this.cbFaultType.Location = new System.Drawing.Point(173, 95);
            this.cbFaultType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbFaultType.Name = "cbFaultType";
            this.cbFaultType.Size = new System.Drawing.Size(160, 24);
            this.cbFaultType.TabIndex = 1;
            this.cbFaultType.SelectedIndexChanged += new System.EventHandler(this.cbFaultType_SelectedIndexChanged);
            this.cbFaultType.Enter += new System.EventHandler(this.cbFaultType_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 138);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fault Sub-Type:";
            // 
            // cbSubType
            // 
            this.cbSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubType.FormattingEnabled = true;
            this.cbSubType.Location = new System.Drawing.Point(173, 133);
            this.cbSubType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSubType.Name = "cbSubType";
            this.cbSubType.Size = new System.Drawing.Size(160, 24);
            this.cbSubType.TabIndex = 3;
            this.cbSubType.SelectedIndexChanged += new System.EventHandler(this.cbSubType_SelectedIndexChanged);
            this.cbSubType.Enter += new System.EventHandler(this.cbSubType_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 176);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fault Code:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 214);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Operation in Progress:";
            // 
            // cbOperProgress
            // 
            this.cbOperProgress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperProgress.FormattingEnabled = true;
            this.cbOperProgress.Location = new System.Drawing.Point(173, 210);
            this.cbOperProgress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbOperProgress.Name = "cbOperProgress";
            this.cbOperProgress.Size = new System.Drawing.Size(160, 24);
            this.cbOperProgress.TabIndex = 7;
            this.cbOperProgress.SelectedIndexChanged += new System.EventHandler(this.cbOperProgress_SelectedIndexChanged);
            this.cbOperProgress.Enter += new System.EventHandler(this.cbOperProgress_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 252);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Time To Ready:";
            // 
            // tbTimeToReady
            // 
            this.tbTimeToReady.Location = new System.Drawing.Point(173, 249);
            this.tbTimeToReady.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbTimeToReady.Name = "tbTimeToReady";
            this.tbTimeToReady.Size = new System.Drawing.Size(132, 22);
            this.tbTimeToReady.TabIndex = 9;
            this.tbTimeToReady.TextChanged += new System.EventHandler(this.tbTimeToReady_TextChanged);
            this.tbTimeToReady.Enter += new System.EventHandler(this.tbTimeToReady_Enter);
            this.tbTimeToReady.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTimeToReady_KeyPress);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(343, 246);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 28);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.Enter += new System.EventHandler(this.btnStart_Enter);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(471, 246);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.Enter += new System.EventHandler(this.btnClear_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 290);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Recovery:";
            // 
            // cbRecovery
            // 
            this.cbRecovery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRecovery.FormattingEnabled = true;
            this.cbRecovery.Location = new System.Drawing.Point(173, 287);
            this.cbRecovery.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbRecovery.Name = "cbRecovery";
            this.cbRecovery.Size = new System.Drawing.Size(160, 24);
            this.cbRecovery.TabIndex = 13;
            this.cbRecovery.SelectedIndexChanged += new System.EventHandler(this.cbRecovery_SelectedIndexChanged);
            this.cbRecovery.Enter += new System.EventHandler(this.cbRecovery_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 329);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Job Disposition:";
            // 
            // cbDisposition
            // 
            this.cbDisposition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDisposition.FormattingEnabled = true;
            this.cbDisposition.Location = new System.Drawing.Point(173, 325);
            this.cbDisposition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDisposition.Name = "cbDisposition";
            this.cbDisposition.Size = new System.Drawing.Size(160, 24);
            this.cbDisposition.TabIndex = 15;
            this.cbDisposition.SelectedIndexChanged += new System.EventHandler(this.cbDisposition_SelectedIndexChanged);
            this.cbDisposition.Enter += new System.EventHandler(this.cbDisposition_Enter);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Enabled = false;
            this.btnSubmit.Location = new System.Drawing.Point(79, 493);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 28);
            this.btnSubmit.TabIndex = 16;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnSubmit.Enter += new System.EventHandler(this.btnSubmit_Enter);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(433, 493);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 28);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "Skip";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.Enter += new System.EventHandler(this.btnExit_Enter);
            // 
            // btnLink
            // 
            this.btnLink.Enabled = false;
            this.btnLink.Location = new System.Drawing.Point(256, 493);
            this.btnLink.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(100, 28);
            this.btnLink.TabIndex = 18;
            this.btnLink.Text = "New Fault";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Visible = false;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            this.btnLink.Enter += new System.EventHandler(this.btnLink_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 411);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 17);
            this.label8.TabIndex = 19;
            this.label8.Text = "Comments:";
            // 
            // tbComments
            // 
            this.tbComments.Location = new System.Drawing.Point(173, 408);
            this.tbComments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbComments.Name = "tbComments";
            this.tbComments.Size = new System.Drawing.Size(396, 22);
            this.tbComments.TabIndex = 20;
            this.tbComments.Enter += new System.EventHandler(this.tbComments_Enter);
            // 
            // tbDescript
            // 
            this.tbDescript.Enabled = false;
            this.tbDescript.Location = new System.Drawing.Point(21, 15);
            this.tbDescript.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDescript.Multiline = true;
            this.tbDescript.Name = "tbDescript";
            this.tbDescript.Size = new System.Drawing.Size(575, 59);
            this.tbDescript.TabIndex = 21;
            // 
            // cbFaultCode
            // 
            this.cbFaultCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cbFaultCode.FormattingEnabled = true;
            this.cbFaultCode.Location = new System.Drawing.Point(173, 172);
            this.cbFaultCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbFaultCode.Name = "cbFaultCode";
            this.cbFaultCode.Size = new System.Drawing.Size(160, 24);
            this.cbFaultCode.TabIndex = 22;
            this.cbFaultCode.SelectedIndexChanged += new System.EventHandler(this.cbFaultCode_SelectedIndexChanged);
            this.cbFaultCode.TextChanged += new System.EventHandler(this.cbFaultCode_TextChanged);
            this.cbFaultCode.Enter += new System.EventHandler(this.cbFaultCode_Enter);
            this.cbFaultCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbFaultCode_KeyPress);
            // 
            // ckbLinkEvent
            // 
            this.ckbLinkEvent.AutoSize = true;
            this.ckbLinkEvent.Location = new System.Drawing.Point(21, 451);
            this.ckbLinkEvent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckbLinkEvent.Name = "ckbLinkEvent";
            this.ckbLinkEvent.Size = new System.Drawing.Size(214, 21);
            this.ckbLinkEvent.TabIndex = 23;
            this.ckbLinkEvent.Text = "Event is caused by last event";
            this.ckbLinkEvent.UseVisualStyleBackColor = true;
            // 
            // cbRootcauseId
            // 
            this.cbRootcauseId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRootcauseId.FormattingEnabled = true;
            this.cbRootcauseId.Location = new System.Drawing.Point(174, 366);
            this.cbRootcauseId.Margin = new System.Windows.Forms.Padding(4);
            this.cbRootcauseId.Name = "cbRootcauseId";
            this.cbRootcauseId.Size = new System.Drawing.Size(160, 24);
            this.cbRootcauseId.TabIndex = 25;
            // 
            // rootcauseId_Label
            // 
            this.rootcauseId_Label.AutoSize = true;
            this.rootcauseId_Label.Location = new System.Drawing.Point(18, 370);
            this.rootcauseId_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rootcauseId_Label.Name = "rootcauseId_Label";
            this.rootcauseId_Label.Size = new System.Drawing.Size(101, 17);
            this.rootcauseId_Label.TabIndex = 24;
            this.rootcauseId_Label.Text = "Root Cause Id:";
            // 
            // FaultHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 538);
            this.Controls.Add(this.cbRootcauseId);
            this.Controls.Add(this.rootcauseId_Label);
            this.Controls.Add(this.ckbLinkEvent);
            this.Controls.Add(this.cbFaultCode);
            this.Controls.Add(this.tbDescript);
            this.Controls.Add(this.tbComments);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnLink);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cbDisposition);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbRecovery);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbTimeToReady);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbOperProgress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSubType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbFaultType);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FaultHandler";
            this.Text = "Fault Handler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFaultType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSubType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbOperProgress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbTimeToReady;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbRecovery;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbDisposition;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbComments;
        private System.Windows.Forms.TextBox tbDescript;
        private System.Windows.Forms.ComboBox cbFaultCode;
        private System.Windows.Forms.CheckBox ckbLinkEvent;
        private System.Windows.Forms.ComboBox cbRootcauseId;
        private System.Windows.Forms.Label rootcauseId_Label;
    }
}


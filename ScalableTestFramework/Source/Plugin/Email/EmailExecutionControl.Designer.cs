namespace HP.ScalableTest.Plugin.Email
{

    /// <summary>
    /// Control that is displayed when an Email activity is running.
    /// </summary>
    partial class EmailExecutionControl
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
            if (disposing && _emailController != null)
            {
                // Load any remaining inbox items before closing down
                _exchangeInboxControl.LoadInbox();
            }

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
            this.emailSplitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.emailToTextBox = new System.Windows.Forms.TextBox();
            this.emailSendButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.emailAttachmentsListBox = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.emailBodyTextBox = new System.Windows.Forms.TextBox();
            this.emailSubjectTextBox = new System.Windows.Forms.TextBox();
            this.emailCcTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.emailReceivePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.emailSplitContainer)).BeginInit();
            this.emailSplitContainer.Panel1.SuspendLayout();
            this.emailSplitContainer.Panel2.SuspendLayout();
            this.emailSplitContainer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // emailSplitContainer
            // 
            this.emailSplitContainer.BackColor = System.Drawing.Color.Black;
            this.emailSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.emailSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.emailSplitContainer.Name = "emailSplitContainer";
            // 
            // emailSplitContainer.Panel1
            // 
            this.emailSplitContainer.Panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.emailSplitContainer.Panel1.Controls.Add(this.groupBox2);
            // 
            // emailSplitContainer.Panel2
            // 
            this.emailSplitContainer.Panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.emailSplitContainer.Panel2.Controls.Add(this.groupBox3);
            this.emailSplitContainer.Size = new System.Drawing.Size(643, 326);
            this.emailSplitContainer.SplitterDistance = 301;
            this.emailSplitContainer.SplitterWidth = 2;
            this.emailSplitContainer.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.refreshButton);
            this.groupBox2.Controls.Add(this.emailToTextBox);
            this.groupBox2.Controls.Add(this.emailSendButton);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.emailAttachmentsListBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.emailBodyTextBox);
            this.groupBox2.Controls.Add(this.emailSubjectTextBox);
            this.groupBox2.Controls.Add(this.emailCcTextBox);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 318);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Send Email";
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshButton.BackColor = System.Drawing.SystemColors.Control;
            this.refreshButton.Location = new System.Drawing.Point(10, 289);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 8;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // emailToTextBox
            // 
            this.emailToTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailToTextBox.Location = new System.Drawing.Point(60, 24);
            this.emailToTextBox.Name = "emailToTextBox";
            this.emailToTextBox.Size = new System.Drawing.Size(227, 20);
            this.emailToTextBox.TabIndex = 2;
            // 
            // emailSendButton
            // 
            this.emailSendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.emailSendButton.BackColor = System.Drawing.SystemColors.Control;
            this.emailSendButton.Location = new System.Drawing.Point(212, 289);
            this.emailSendButton.Name = "emailSendButton";
            this.emailSendButton.Size = new System.Drawing.Size(75, 23);
            this.emailSendButton.TabIndex = 7;
            this.emailSendButton.Text = "Send";
            this.emailSendButton.UseVisualStyleBackColor = true;
            this.emailSendButton.Click += new System.EventHandler(this.emailSendButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "To...";
            // 
            // emailAttachmentsListBox
            // 
            this.emailAttachmentsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailAttachmentsListBox.FormattingEnabled = true;
            this.emailAttachmentsListBox.Location = new System.Drawing.Point(6, 240);
            this.emailAttachmentsListBox.Name = "emailAttachmentsListBox";
            this.emailAttachmentsListBox.Size = new System.Drawing.Size(281, 43);
            this.emailAttachmentsListBox.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Attachments...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Subject...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Cc...";
            // 
            // emailBodyTextBox
            // 
            this.emailBodyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailBodyTextBox.Location = new System.Drawing.Point(10, 102);
            this.emailBodyTextBox.Multiline = true;
            this.emailBodyTextBox.Name = "emailBodyTextBox";
            this.emailBodyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.emailBodyTextBox.Size = new System.Drawing.Size(277, 119);
            this.emailBodyTextBox.TabIndex = 5;
            // 
            // emailSubjectTextBox
            // 
            this.emailSubjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailSubjectTextBox.Location = new System.Drawing.Point(60, 76);
            this.emailSubjectTextBox.Name = "emailSubjectTextBox";
            this.emailSubjectTextBox.Size = new System.Drawing.Size(227, 20);
            this.emailSubjectTextBox.TabIndex = 4;
            // 
            // emailCcTextBox
            // 
            this.emailCcTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailCcTextBox.Location = new System.Drawing.Point(60, 50);
            this.emailCcTextBox.Name = "emailCcTextBox";
            this.emailCcTextBox.Size = new System.Drawing.Size(227, 20);
            this.emailCcTextBox.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.emailReceivePanel);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(333, 318);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Receive Email";
            // 
            // emailReceivePanel
            // 
            this.emailReceivePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailReceivePanel.Location = new System.Drawing.Point(6, 24);
            this.emailReceivePanel.Name = "emailReceivePanel";
            this.emailReceivePanel.Size = new System.Drawing.Size(321, 288);
            this.emailReceivePanel.TabIndex = 0;
            // 
            // ExchangeEmailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.emailSplitContainer);
            this.Name = "ExchangeEmailControl";
            this.Size = new System.Drawing.Size(643, 326);
            this.emailSplitContainer.Panel1.ResumeLayout(false);
            this.emailSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.emailSplitContainer)).EndInit();
            this.emailSplitContainer.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer emailSplitContainer;
        private System.Windows.Forms.TextBox emailBodyTextBox;
        private System.Windows.Forms.TextBox emailCcTextBox;
        private System.Windows.Forms.TextBox emailToTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox emailAttachmentsListBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox emailSubjectTextBox;
        private System.Windows.Forms.Button emailSendButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel emailReceivePanel;
        private System.Windows.Forms.Button refreshButton;
    }
}

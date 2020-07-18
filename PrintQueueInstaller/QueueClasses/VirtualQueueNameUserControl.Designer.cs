namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Class to support creating a virtual queue
    /// </summary>
    partial class VirtualQueueNameUserControl
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
            this.startingPort_Label = new System.Windows.Forms.Label();
            this.startValue_TextBox = new System.Windows.Forms.TextBox();
            this.snmp_CheckBox = new System.Windows.Forms.CheckBox();
            this.serverHostName_Label = new System.Windows.Forms.Label();
            this.incrementIP_CheckBox = new System.Windows.Forms.CheckBox();
            this.shared_CheckBox = new System.Windows.Forms.CheckBox();
            this.renderClient_CheckBox = new System.Windows.Forms.CheckBox();
            this.OK_Button = new System.Windows.Forms.Button();
            this.increment_Label = new System.Windows.Forms.Label();
            this.shared_Label = new System.Windows.Forms.Label();
            this.render_Label = new System.Windows.Forms.Label();
            this.snmp_Label = new System.Windows.Forms.Label();
            this.numberOfQueues_TextBox = new System.Windows.Forms.TextBox();
            this.numberOfQueues_Label = new System.Windows.Forms.Label();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.hostnameCode_TextBox = new System.Windows.Forms.TextBox();
            this.abbreviation_Label = new System.Windows.Forms.Label();
            this.virtualPrinterServerAddress_ComboBox = new System.Windows.Forms.ComboBox();
            this.endValue_TextBox = new System.Windows.Forms.TextBox();
            this.endingPort_Label = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startingPort_Label
            // 
            this.startingPort_Label.AutoSize = true;
            this.startingPort_Label.Location = new System.Drawing.Point(61, 150);
            this.startingPort_Label.Name = "startingPort_Label";
            this.startingPort_Label.Size = new System.Drawing.Size(115, 13);
            this.startingPort_Label.TabIndex = 7;
            this.startingPort_Label.Text = "IP Octet Starting Value";
            // 
            // startValue_TextBox
            // 
            this.startValue_TextBox.Location = new System.Drawing.Point(182, 147);
            this.startValue_TextBox.Name = "startValue_TextBox";
            this.startValue_TextBox.Size = new System.Drawing.Size(53, 20);
            this.startValue_TextBox.TabIndex = 4;
            this.startValue_TextBox.Text = "1";
            this.startValue_TextBox.TextChanged += new System.EventHandler(this.startValue_TextBox_TextChanged);
            this.startValue_TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumbersOnlyTextBox_KeyDown);
            this.startValue_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.startValue_TextBox_Validating);
            // 
            // snmp_CheckBox
            // 
            this.snmp_CheckBox.AutoSize = true;
            this.snmp_CheckBox.Location = new System.Drawing.Point(182, 259);
            this.snmp_CheckBox.Name = "snmp_CheckBox";
            this.snmp_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.snmp_CheckBox.TabIndex = 9;
            this.snmp_CheckBox.UseVisualStyleBackColor = true;
            // 
            // serverHostName_Label
            // 
            this.serverHostName_Label.AutoSize = true;
            this.serverHostName_Label.Location = new System.Drawing.Point(22, 72);
            this.serverHostName_Label.Name = "serverHostName_Label";
            this.serverHostName_Label.Size = new System.Drawing.Size(154, 13);
            this.serverHostName_Label.TabIndex = 5;
            this.serverHostName_Label.Text = "Virtual Printer Server Hostname";
            // 
            // incrementIP_CheckBox
            // 
            this.incrementIP_CheckBox.AutoSize = true;
            this.incrementIP_CheckBox.Checked = true;
            this.incrementIP_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.incrementIP_CheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.incrementIP_CheckBox.Location = new System.Drawing.Point(182, 199);
            this.incrementIP_CheckBox.Name = "incrementIP_CheckBox";
            this.incrementIP_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.incrementIP_CheckBox.TabIndex = 6;
            this.incrementIP_CheckBox.UseVisualStyleBackColor = true;
            // 
            // shared_CheckBox
            // 
            this.shared_CheckBox.AutoSize = true;
            this.shared_CheckBox.Checked = true;
            this.shared_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shared_CheckBox.Location = new System.Drawing.Point(182, 219);
            this.shared_CheckBox.Name = "shared_CheckBox";
            this.shared_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.shared_CheckBox.TabIndex = 7;
            this.shared_CheckBox.UseVisualStyleBackColor = true;
            // 
            // renderClient_CheckBox
            // 
            this.renderClient_CheckBox.AutoSize = true;
            this.renderClient_CheckBox.Location = new System.Drawing.Point(182, 238);
            this.renderClient_CheckBox.Name = "renderClient_CheckBox";
            this.renderClient_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.renderClient_CheckBox.TabIndex = 8;
            this.renderClient_CheckBox.UseVisualStyleBackColor = true;
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.Location = new System.Drawing.Point(191, 287);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(75, 23);
            this.OK_Button.TabIndex = 10;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // increment_Label
            // 
            this.increment_Label.AutoSize = true;
            this.increment_Label.Location = new System.Drawing.Point(50, 200);
            this.increment_Label.Name = "increment_Label";
            this.increment_Label.Size = new System.Drawing.Size(126, 13);
            this.increment_Label.TabIndex = 28;
            this.increment_Label.Text = "Increment IP Octet Value";
            // 
            // shared_Label
            // 
            this.shared_Label.AutoSize = true;
            this.shared_Label.Location = new System.Drawing.Point(33, 220);
            this.shared_Label.Name = "shared_Label";
            this.shared_Label.Size = new System.Drawing.Size(143, 13);
            this.shared_Label.TabIndex = 29;
            this.shared_Label.Text = "Configure Queues as Shared";
            // 
            // render_Label
            // 
            this.render_Label.AutoSize = true;
            this.render_Label.Location = new System.Drawing.Point(41, 239);
            this.render_Label.Name = "render_Label";
            this.render_Label.Size = new System.Drawing.Size(135, 13);
            this.render_Label.TabIndex = 30;
            this.render_Label.Text = "Render Print Jobs on Client";
            // 
            // snmp_Label
            // 
            this.snmp_Label.AutoSize = true;
            this.snmp_Label.Location = new System.Drawing.Point(63, 260);
            this.snmp_Label.Name = "snmp_Label";
            this.snmp_Label.Size = new System.Drawing.Size(113, 13);
            this.snmp_Label.TabIndex = 31;
            this.snmp_Label.Text = "SNMP Status Enabled";
            // 
            // numberOfQueues_TextBox
            // 
            this.numberOfQueues_TextBox.Location = new System.Drawing.Point(182, 121);
            this.numberOfQueues_TextBox.Name = "numberOfQueues_TextBox";
            this.numberOfQueues_TextBox.Size = new System.Drawing.Size(53, 20);
            this.numberOfQueues_TextBox.TabIndex = 3;
            this.numberOfQueues_TextBox.TextChanged += new System.EventHandler(this.numberOfQueues_TextBox_TextChanged);
            this.numberOfQueues_TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumbersOnlyTextBox_KeyDown);
            this.numberOfQueues_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.numberOfQueues_TextBox_Validating);
            // 
            // numberOfQueues_Label
            // 
            this.numberOfQueues_Label.AutoSize = true;
            this.numberOfQueues_Label.Location = new System.Drawing.Point(25, 124);
            this.numberOfQueues_Label.Name = "numberOfQueues_Label";
            this.numberOfQueues_Label.Size = new System.Drawing.Size(142, 13);
            this.numberOfQueues_Label.TabIndex = 33;
            this.numberOfQueues_Label.Text = "Number of Queues to Create";
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(272, 287);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 11;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // hostnameCode_TextBox
            // 
            this.hostnameCode_TextBox.Location = new System.Drawing.Point(182, 95);
            this.hostnameCode_TextBox.Name = "hostnameCode_TextBox";
            this.hostnameCode_TextBox.Size = new System.Drawing.Size(53, 20);
            this.hostnameCode_TextBox.TabIndex = 2;
            // 
            // abbreviation_Label
            // 
            this.abbreviation_Label.AutoSize = true;
            this.abbreviation_Label.Location = new System.Drawing.Point(59, 98);
            this.abbreviation_Label.Name = "abbreviation_Label";
            this.abbreviation_Label.Size = new System.Drawing.Size(117, 13);
            this.abbreviation_Label.TabIndex = 37;
            this.abbreviation_Label.Text = "Hostname Abbreviation";
            // 
            // virtualPrinterServerAddress_ComboBox
            // 
            this.virtualPrinterServerAddress_ComboBox.FormattingEnabled = true;
            this.virtualPrinterServerAddress_ComboBox.Location = new System.Drawing.Point(182, 69);
            this.virtualPrinterServerAddress_ComboBox.Name = "virtualPrinterServerAddress_ComboBox";
            this.virtualPrinterServerAddress_ComboBox.Size = new System.Drawing.Size(124, 21);
            this.virtualPrinterServerAddress_ComboBox.TabIndex = 1;
            this.virtualPrinterServerAddress_ComboBox.SelectedValueChanged += new System.EventHandler(this.virtualPrinterServerAddress_ComboBox_SelectedValueChanged);
            // 
            // endValue_TextBox
            // 
            this.endValue_TextBox.Location = new System.Drawing.Point(182, 173);
            this.endValue_TextBox.Name = "endValue_TextBox";
            this.endValue_TextBox.ReadOnly = true;
            this.endValue_TextBox.Size = new System.Drawing.Size(53, 20);
            this.endValue_TextBox.TabIndex = 5;
            // 
            // endingPort_Label
            // 
            this.endingPort_Label.AutoSize = true;
            this.endingPort_Label.Location = new System.Drawing.Point(64, 176);
            this.endingPort_Label.Name = "endingPort_Label";
            this.endingPort_Label.Size = new System.Drawing.Size(112, 13);
            this.endingPort_Label.TabIndex = 40;
            this.endingPort_Label.Text = "IP Octet Ending Value";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.endingPort_Label);
            this.groupBox1.Controls.Add(this.endValue_TextBox);
            this.groupBox1.Controls.Add(this.incrementIP_CheckBox);
            this.groupBox1.Controls.Add(this.increment_Label);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.serverHostName_Label);
            this.groupBox1.Controls.Add(this.virtualPrinterServerAddress_ComboBox);
            this.groupBox1.Controls.Add(this.cancel_Button);
            this.groupBox1.Controls.Add(this.OK_Button);
            this.groupBox1.Controls.Add(this.renderClient_CheckBox);
            this.groupBox1.Controls.Add(this.shared_CheckBox);
            this.groupBox1.Controls.Add(this.startValue_TextBox);
            this.groupBox1.Controls.Add(this.abbreviation_Label);
            this.groupBox1.Controls.Add(this.hostnameCode_TextBox);
            this.groupBox1.Controls.Add(this.startingPort_Label);
            this.groupBox1.Controls.Add(this.shared_Label);
            this.groupBox1.Controls.Add(this.numberOfQueues_Label);
            this.groupBox1.Controls.Add(this.render_Label);
            this.groupBox1.Controls.Add(this.numberOfQueues_TextBox);
            this.groupBox1.Controls.Add(this.snmp_CheckBox);
            this.groupBox1.Controls.Add(this.snmp_Label);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(352, 315);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Define Virtual Queues";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(12, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(335, 46);
            this.label10.TabIndex = 42;
            this.label10.Text = "Use this form to create ad-hoc print queues based on the data you provide.  It is" +
    " intended to provide a way to create multiple queues that point to a STF virtual" +
    " printer server. ";
            // 
            // VirtualQueueNameUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "VirtualQueueNameUserControl";
            this.Size = new System.Drawing.Size(359, 323);
            this.Load += new System.EventHandler(this.VirtualQueueDataControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label startingPort_Label;
        private System.Windows.Forms.TextBox startValue_TextBox;
        private System.Windows.Forms.CheckBox snmp_CheckBox;
        private System.Windows.Forms.Label serverHostName_Label;
        private System.Windows.Forms.CheckBox incrementIP_CheckBox;
        private System.Windows.Forms.CheckBox shared_CheckBox;
        private System.Windows.Forms.CheckBox renderClient_CheckBox;
        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Label increment_Label;
        private System.Windows.Forms.Label shared_Label;
        private System.Windows.Forms.Label render_Label;
        private System.Windows.Forms.Label snmp_Label;
        private System.Windows.Forms.TextBox numberOfQueues_TextBox;
        private System.Windows.Forms.Label numberOfQueues_Label;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox hostnameCode_TextBox;
        private System.Windows.Forms.Label abbreviation_Label;
        private System.Windows.Forms.ComboBox virtualPrinterServerAddress_ComboBox;
        private System.Windows.Forms.TextBox endValue_TextBox;
        private System.Windows.Forms.Label endingPort_Label;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
    }
}

namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    partial class SwitchDetailsControl
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
            this.switchIPAddress_Label = new System.Windows.Forms.Label();
            this.switchIPAddress_IpAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.portNumber_Label = new System.Windows.Forms.Label();
            this.switchPortNumber_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.switchDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.switch_FieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.switchPortNumber_NumericUpDown)).BeginInit();
            this.switchDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // switchIPAddress_Label
            // 
            this.switchIPAddress_Label.AutoSize = true;
            this.switchIPAddress_Label.Location = new System.Drawing.Point(17, 30);
            this.switchIPAddress_Label.Name = "switchIPAddress_Label";
            this.switchIPAddress_Label.Size = new System.Drawing.Size(61, 13);
            this.switchIPAddress_Label.TabIndex = 0;
            this.switchIPAddress_Label.Text = "IP Address:";
            // 
            // switchIPAddress_IpAddressControl
            // 
            this.switchIPAddress_IpAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.switchIPAddress_IpAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.switchIPAddress_IpAddressControl.Location = new System.Drawing.Point(84, 27);
            this.switchIPAddress_IpAddressControl.Margin = new System.Windows.Forms.Padding(3, 3, 50, 3);
            this.switchIPAddress_IpAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.switchIPAddress_IpAddressControl.Name = "switchIPAddress_IpAddressControl";
            this.switchIPAddress_IpAddressControl.Size = new System.Drawing.Size(136, 20);
            this.switchIPAddress_IpAddressControl.TabIndex = 1;
            this.switchIPAddress_IpAddressControl.Text = "...";
            this.toolTip.SetToolTip(this.switchIPAddress_IpAddressControl, "Enter switch IP address");
            this.switchIPAddress_IpAddressControl.TextChanged += new System.EventHandler(this.switchIPAddress_IpAddressControl_TextChanged);
            // 
            // portNumber_Label
            // 
            this.portNumber_Label.AutoSize = true;
            this.portNumber_Label.Location = new System.Drawing.Point(17, 60);
            this.portNumber_Label.Name = "portNumber_Label";
            this.portNumber_Label.Size = new System.Drawing.Size(46, 13);
            this.portNumber_Label.TabIndex = 2;
            this.portNumber_Label.Text = "Port No:";
            // 
            // switchPortNumber_NumericUpDown
            // 
            this.switchPortNumber_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.switchPortNumber_NumericUpDown.AutoSize = true;
            this.switchPortNumber_NumericUpDown.Location = new System.Drawing.Point(84, 58);
            this.switchPortNumber_NumericUpDown.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.switchPortNumber_NumericUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.switchPortNumber_NumericUpDown.MaximumSize = new System.Drawing.Size(35, 0);
            this.switchPortNumber_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.switchPortNumber_NumericUpDown.MinimumSize = new System.Drawing.Size(45, 0);
            this.switchPortNumber_NumericUpDown.Name = "switchPortNumber_NumericUpDown";
            this.switchPortNumber_NumericUpDown.ReadOnly = true;
            this.switchPortNumber_NumericUpDown.Size = new System.Drawing.Size(45, 20);
            this.switchPortNumber_NumericUpDown.TabIndex = 3;
            this.toolTip.SetToolTip(this.switchPortNumber_NumericUpDown, "Select switch port number");
            this.switchPortNumber_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.switchPortNumber_NumericUpDown.ValueChanged += new System.EventHandler(this.switchPortNumber_NumericUpDown_ValueChanged);
            // 
            // switchDetails_GroupBox
            // 
            this.switchDetails_GroupBox.Controls.Add(this.switchPortNumber_NumericUpDown);
            this.switchDetails_GroupBox.Controls.Add(this.portNumber_Label);
            this.switchDetails_GroupBox.Controls.Add(this.switchIPAddress_IpAddressControl);
            this.switchDetails_GroupBox.Controls.Add(this.switchIPAddress_Label);
            this.switchDetails_GroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.switchDetails_GroupBox.Location = new System.Drawing.Point(0, 0);
            this.switchDetails_GroupBox.Name = "switchDetails_GroupBox";
            this.switchDetails_GroupBox.Size = new System.Drawing.Size(240, 90);
            this.switchDetails_GroupBox.TabIndex = 4;
            this.switchDetails_GroupBox.TabStop = false;
            this.switchDetails_GroupBox.Text = "Switch Details";
            // 
            // SwitchDetailsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.switchDetails_GroupBox);
            this.MinimumSize = new System.Drawing.Size(240, 90);
            this.Name = "SwitchDetailsControl";
            this.Size = new System.Drawing.Size(240, 90);
            ((System.ComponentModel.ISupportInitialize)(this.switchPortNumber_NumericUpDown)).EndInit();
            this.switchDetails_GroupBox.ResumeLayout(false);
            this.switchDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label switchIPAddress_Label;
        private Framework.UI.IPAddressControl switchIPAddress_IpAddressControl;
        private System.Windows.Forms.Label portNumber_Label;
        private System.Windows.Forms.NumericUpDown switchPortNumber_NumericUpDown;
        private System.Windows.Forms.GroupBox switchDetails_GroupBox;
        private System.Windows.Forms.ToolTip toolTip;
        private Framework.UI.FieldValidator switch_FieldValidator;
    }
}

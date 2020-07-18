namespace HP.ScalableTest.Plugin.RoboAction
{
    partial class RoboActionConfigurationControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.duration_timeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.pin_label = new System.Windows.Forms.Label();
            this.pin_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pin_groupBox = new System.Windows.Forms.GroupBox();
            this.pi_label = new System.Windows.Forms.Label();
            this.pi_ipAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.port_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.port_label = new System.Windows.Forms.Label();
            this.pi_groupBox = new System.Windows.Forms.GroupBox();
            this.note_label = new System.Windows.Forms.Label();
            this.note_description_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pin_numericUpDown)).BeginInit();
            this.pin_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.port_numericUpDown)).BeginInit();
            this.pi_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Duration";
            // 
            // duration_timeSpanControl
            // 
            this.duration_timeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.duration_timeSpanControl.Location = new System.Drawing.Point(138, 47);
            this.duration_timeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.duration_timeSpanControl.Name = "duration_timeSpanControl";
            this.duration_timeSpanControl.Size = new System.Drawing.Size(120, 23);
            this.duration_timeSpanControl.TabIndex = 3;
            // 
            // pin_label
            // 
            this.pin_label.AutoSize = true;
            this.pin_label.Location = new System.Drawing.Point(8, 26);
            this.pin_label.Name = "pin_label";
            this.pin_label.Size = new System.Drawing.Size(71, 15);
            this.pin_label.TabIndex = 6;
            this.pin_label.Text = "Pin Number";
            // 
            // pin_numericUpDown
            // 
            this.pin_numericUpDown.Location = new System.Drawing.Point(6, 47);
            this.pin_numericUpDown.Maximum = new decimal(new int[] {
            27,
            0,
            0,
            0});
            this.pin_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pin_numericUpDown.Name = "pin_numericUpDown";
            this.pin_numericUpDown.Size = new System.Drawing.Size(122, 23);
            this.pin_numericUpDown.TabIndex = 7;
            this.pin_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pin_groupBox
            // 
            this.pin_groupBox.Controls.Add(this.pin_numericUpDown);
            this.pin_groupBox.Controls.Add(this.pin_label);
            this.pin_groupBox.Controls.Add(this.label1);
            this.pin_groupBox.Controls.Add(this.duration_timeSpanControl);
            this.pin_groupBox.Location = new System.Drawing.Point(21, 111);
            this.pin_groupBox.Name = "pin_groupBox";
            this.pin_groupBox.Size = new System.Drawing.Size(319, 81);
            this.pin_groupBox.TabIndex = 8;
            this.pin_groupBox.TabStop = false;
            this.pin_groupBox.Text = "Pin Info";
            // 
            // pi_label
            // 
            this.pi_label.AutoSize = true;
            this.pi_label.Location = new System.Drawing.Point(6, 19);
            this.pi_label.Name = "pi_label";
            this.pi_label.Size = new System.Drawing.Size(49, 15);
            this.pi_label.TabIndex = 9;
            this.pi_label.Text = "Address";
            // 
            // pi_ipAddressControl
            // 
            this.pi_ipAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.pi_ipAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.pi_ipAddressControl.Location = new System.Drawing.Point(6, 37);
            this.pi_ipAddressControl.MinimumSize = new System.Drawing.Size(87, 23);
            this.pi_ipAddressControl.Name = "pi_ipAddressControl";
            this.pi_ipAddressControl.Size = new System.Drawing.Size(120, 23);
            this.pi_ipAddressControl.TabIndex = 10;
            this.pi_ipAddressControl.Text = "...";
            // 
            // port_numericUpDown
            // 
            this.port_numericUpDown.Location = new System.Drawing.Point(136, 38);
            this.port_numericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.port_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.port_numericUpDown.Name = "port_numericUpDown";
            this.port_numericUpDown.Size = new System.Drawing.Size(72, 23);
            this.port_numericUpDown.TabIndex = 11;
            this.port_numericUpDown.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(133, 19);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(86, 15);
            this.port_label.TabIndex = 12;
            this.port_label.Text = "Port (Optional)";
            // 
            // pi_groupBox
            // 
            this.pi_groupBox.Controls.Add(this.note_description_label);
            this.pi_groupBox.Controls.Add(this.note_label);
            this.pi_groupBox.Controls.Add(this.pi_ipAddressControl);
            this.pi_groupBox.Controls.Add(this.port_label);
            this.pi_groupBox.Controls.Add(this.port_numericUpDown);
            this.pi_groupBox.Controls.Add(this.pi_label);
            this.pi_groupBox.Location = new System.Drawing.Point(21, 12);
            this.pi_groupBox.Name = "pi_groupBox";
            this.pi_groupBox.Size = new System.Drawing.Size(319, 93);
            this.pi_groupBox.TabIndex = 13;
            this.pi_groupBox.TabStop = false;
            this.pi_groupBox.Text = "Raspberry Pi Info";
            // 
            // note_label
            // 
            this.note_label.AutoSize = true;
            this.note_label.Location = new System.Drawing.Point(6, 75);
            this.note_label.Name = "note_label";
            this.note_label.Size = new System.Drawing.Size(36, 15);
            this.note_label.TabIndex = 13;
            this.note_label.Text = "Note:";
            // 
            // note_description_label
            // 
            this.note_description_label.AutoSize = true;
            this.note_description_label.Location = new System.Drawing.Point(42, 75);
            this.note_description_label.Name = "note_description_label";
            this.note_description_label.Size = new System.Drawing.Size(247, 15);
            this.note_description_label.TabIndex = 14;
            this.note_description_label.Text = "The WebApi is hosted on port 5000 by default";
            // 
            // RoboActionConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pi_groupBox);
            this.Controls.Add(this.pin_groupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RoboActionConfigurationControl";
            this.Size = new System.Drawing.Size(397, 195);
            ((System.ComponentModel.ISupportInitialize)(this.pin_numericUpDown)).EndInit();
            this.pin_groupBox.ResumeLayout(false);
            this.pin_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.port_numericUpDown)).EndInit();
            this.pi_groupBox.ResumeLayout(false);
            this.pi_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label label1;
        private Framework.UI.TimeSpanControl duration_timeSpanControl;
        private System.Windows.Forms.Label pin_label;
        private System.Windows.Forms.NumericUpDown pin_numericUpDown;
        private System.Windows.Forms.GroupBox pin_groupBox;
        private System.Windows.Forms.Label pi_label;
        private Framework.UI.IPAddressControl pi_ipAddressControl;
        private System.Windows.Forms.NumericUpDown port_numericUpDown;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.GroupBox pi_groupBox;
        private System.Windows.Forms.Label note_description_label;
        private System.Windows.Forms.Label note_label;
    }
}

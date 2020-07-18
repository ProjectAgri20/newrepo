namespace HP.ScalableTest.Plugin.PrintQueueManagement
{
    partial class ConfigureQueueForm
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
            this.Setting_groupBox = new System.Windows.Forms.GroupBox();
            this.duplexer_comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.duplex_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.copies_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.paperType_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.orientation_label = new System.Windows.Forms.Label();
            this.copies_label = new System.Windows.Forms.Label();
            this.paperSource_label = new System.Windows.Forms.Label();
            this.paperSize_label = new System.Windows.Forms.Label();
            this.paperSource_comboBox = new System.Windows.Forms.ComboBox();
            this.orientation_comboBox = new System.Windows.Forms.ComboBox();
            this.paperSize_comboBox = new System.Windows.Forms.ComboBox();
            this.Setting_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.copies_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Setting_groupBox
            // 
            this.Setting_groupBox.Controls.Add(this.duplexer_comboBox);
            this.Setting_groupBox.Controls.Add(this.label3);
            this.Setting_groupBox.Controls.Add(this.duplex_comboBox);
            this.Setting_groupBox.Controls.Add(this.label2);
            this.Setting_groupBox.Controls.Add(this.ok_button);
            this.Setting_groupBox.Controls.Add(this.copies_numericUpDown);
            this.Setting_groupBox.Controls.Add(this.paperType_comboBox);
            this.Setting_groupBox.Controls.Add(this.label1);
            this.Setting_groupBox.Controls.Add(this.orientation_label);
            this.Setting_groupBox.Controls.Add(this.copies_label);
            this.Setting_groupBox.Controls.Add(this.paperSource_label);
            this.Setting_groupBox.Controls.Add(this.paperSize_label);
            this.Setting_groupBox.Controls.Add(this.paperSource_comboBox);
            this.Setting_groupBox.Controls.Add(this.orientation_comboBox);
            this.Setting_groupBox.Controls.Add(this.paperSize_comboBox);
            this.Setting_groupBox.Location = new System.Drawing.Point(12, 12);
            this.Setting_groupBox.Name = "Setting_groupBox";
            this.Setting_groupBox.Size = new System.Drawing.Size(400, 270);
            this.Setting_groupBox.TabIndex = 9;
            this.Setting_groupBox.TabStop = false;
            this.Setting_groupBox.Text = "Settings";
            // 
            // duplexer_comboBox
            // 
            this.duplexer_comboBox.FormattingEnabled = true;
            this.duplexer_comboBox.Items.AddRange(new object[] {
            "Installed",
            "NotInstalled"});
            this.duplexer_comboBox.Location = new System.Drawing.Point(9, 191);
            this.duplexer_comboBox.Name = "duplexer_comboBox";
            this.duplexer_comboBox.Size = new System.Drawing.Size(156, 21);
            this.duplexer_comboBox.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Duplexer";
            // 
            // duplex_comboBox
            // 
            this.duplex_comboBox.FormattingEnabled = true;
            this.duplex_comboBox.Location = new System.Drawing.Point(194, 141);
            this.duplex_comboBox.Name = "duplex_comboBox";
            this.duplex_comboBox.Size = new System.Drawing.Size(156, 21);
            this.duplex_comboBox.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Duplex Settings";
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(143, 232);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 10;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // copies_numericUpDown
            // 
            this.copies_numericUpDown.Location = new System.Drawing.Point(194, 90);
            this.copies_numericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.copies_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.copies_numericUpDown.Name = "copies_numericUpDown";
            this.copies_numericUpDown.Size = new System.Drawing.Size(49, 20);
            this.copies_numericUpDown.TabIndex = 11;
            this.copies_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // paperType_comboBox
            // 
            this.paperType_comboBox.FormattingEnabled = true;
            this.paperType_comboBox.Location = new System.Drawing.Point(9, 141);
            this.paperType_comboBox.Name = "paperType_comboBox";
            this.paperType_comboBox.Size = new System.Drawing.Size(156, 21);
            this.paperType_comboBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Paper Type";
            // 
            // orientation_label
            // 
            this.orientation_label.AutoSize = true;
            this.orientation_label.Location = new System.Drawing.Point(191, 25);
            this.orientation_label.Name = "orientation_label";
            this.orientation_label.Size = new System.Drawing.Size(58, 13);
            this.orientation_label.TabIndex = 7;
            this.orientation_label.Text = "Orientation";
            // 
            // copies_label
            // 
            this.copies_label.AutoSize = true;
            this.copies_label.Location = new System.Drawing.Point(191, 74);
            this.copies_label.Name = "copies_label";
            this.copies_label.Size = new System.Drawing.Size(39, 13);
            this.copies_label.TabIndex = 6;
            this.copies_label.Text = "Copies";
            // 
            // paperSource_label
            // 
            this.paperSource_label.AutoSize = true;
            this.paperSource_label.Location = new System.Drawing.Point(6, 75);
            this.paperSource_label.Name = "paperSource_label";
            this.paperSource_label.Size = new System.Drawing.Size(72, 13);
            this.paperSource_label.TabIndex = 5;
            this.paperSource_label.Text = "Paper Source";
            // 
            // paperSize_label
            // 
            this.paperSize_label.AutoSize = true;
            this.paperSize_label.Location = new System.Drawing.Point(6, 25);
            this.paperSize_label.Name = "paperSize_label";
            this.paperSize_label.Size = new System.Drawing.Size(58, 13);
            this.paperSize_label.TabIndex = 4;
            this.paperSize_label.Text = "Paper Size";
            // 
            // paperSource_comboBox
            // 
            this.paperSource_comboBox.FormattingEnabled = true;
            this.paperSource_comboBox.Location = new System.Drawing.Point(9, 91);
            this.paperSource_comboBox.Name = "paperSource_comboBox";
            this.paperSource_comboBox.Size = new System.Drawing.Size(156, 21);
            this.paperSource_comboBox.TabIndex = 3;
            // 
            // orientation_comboBox
            // 
            this.orientation_comboBox.FormattingEnabled = true;
            this.orientation_comboBox.Location = new System.Drawing.Point(194, 41);
            this.orientation_comboBox.Name = "orientation_comboBox";
            this.orientation_comboBox.Size = new System.Drawing.Size(156, 21);
            this.orientation_comboBox.TabIndex = 2;
            // 
            // paperSize_comboBox
            // 
            this.paperSize_comboBox.FormattingEnabled = true;
            this.paperSize_comboBox.Location = new System.Drawing.Point(9, 41);
            this.paperSize_comboBox.Name = "paperSize_comboBox";
            this.paperSize_comboBox.Size = new System.Drawing.Size(156, 21);
            this.paperSize_comboBox.TabIndex = 0;
            // 
            // ConfigureQueueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 294);
            this.Controls.Add(this.Setting_groupBox);
            this.Name = "ConfigureQueueForm";
            this.Text = "Print Driver Preferences";
            this.Load += new System.EventHandler(this.ConfigureQueue_Load);
            this.Setting_groupBox.ResumeLayout(false);
            this.Setting_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.copies_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Setting_groupBox;
        private System.Windows.Forms.ComboBox paperType_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label orientation_label;
        private System.Windows.Forms.Label copies_label;
        private System.Windows.Forms.Label paperSource_label;
        private System.Windows.Forms.Label paperSize_label;
        private System.Windows.Forms.ComboBox paperSource_comboBox;
        private System.Windows.Forms.ComboBox orientation_comboBox;
        private System.Windows.Forms.ComboBox paperSize_comboBox;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.NumericUpDown copies_numericUpDown;
        private System.Windows.Forms.ComboBox duplex_comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox duplexer_comboBox;
        private System.Windows.Forms.Label label3;

    }
}

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    partial class SessionStartScheduleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionStartScheduleForm));
            this.delayedStartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.promptLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.setupLabel = new System.Windows.Forms.Label();
            this.setupBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.enableButton = new System.Windows.Forms.Button();
            this.disableButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.setupBufferNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // delayedStartDateTimePicker
            // 
            this.delayedStartDateTimePicker.CustomFormat = "dddMMM dd, yyyy hh:mm tt";
            this.delayedStartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.delayedStartDateTimePicker.Location = new System.Drawing.Point(197, 118);
            this.delayedStartDateTimePicker.Name = "delayedStartDateTimePicker";
            this.delayedStartDateTimePicker.ShowUpDown = true;
            this.delayedStartDateTimePicker.Size = new System.Drawing.Size(227, 27);
            this.delayedStartDateTimePicker.TabIndex = 0;
            // 
            // promptLabel
            // 
            this.promptLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptLabel.Location = new System.Drawing.Point(12, 9);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(418, 106);
            this.promptLabel.TabIndex = 1;
            this.promptLabel.Text = resources.GetString("promptLabel.Text");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Session Start Date/Time";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // setupLabel
            // 
            this.setupLabel.Location = new System.Drawing.Point(20, 153);
            this.setupLabel.Name = "setupLabel";
            this.setupLabel.Size = new System.Drawing.Size(171, 22);
            this.setupLabel.TabIndex = 3;
            this.setupLabel.Text = "System Setup Duration";
            this.setupLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // setupBufferNumericUpDown
            // 
            this.setupBufferNumericUpDown.Location = new System.Drawing.Point(197, 151);
            this.setupBufferNumericUpDown.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.setupBufferNumericUpDown.Name = "setupBufferNumericUpDown";
            this.setupBufferNumericUpDown.Size = new System.Drawing.Size(79, 27);
            this.setupBufferNumericUpDown.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(282, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 22);
            this.label2.TabIndex = 5;
            this.label2.Text = "(min)";
            // 
            // enableButton
            // 
            this.enableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.enableButton.Image = ((System.Drawing.Image)(resources.GetObject("enableButton.Image")));
            this.enableButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.enableButton.Location = new System.Drawing.Point(112, 198);
            this.enableButton.Name = "enableButton";
            this.enableButton.Size = new System.Drawing.Size(100, 32);
            this.enableButton.TabIndex = 6;
            this.enableButton.Text = "Enable";
            this.enableButton.UseVisualStyleBackColor = true;
            this.enableButton.Click += new System.EventHandler(this.enableButton_Click);
            // 
            // disableButton
            // 
            this.disableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.disableButton.Image = ((System.Drawing.Image)(resources.GetObject("disableButton.Image")));
            this.disableButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.disableButton.Location = new System.Drawing.Point(218, 198);
            this.disableButton.Name = "disableButton";
            this.disableButton.Size = new System.Drawing.Size(100, 32);
            this.disableButton.TabIndex = 7;
            this.disableButton.Text = "Disable";
            this.disableButton.UseVisualStyleBackColor = true;
            this.disableButton.Click += new System.EventHandler(this.disableButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelButton.Location = new System.Drawing.Point(324, 198);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // SessionStartScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 242);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.delayedStartDateTimePicker);
            this.Controls.Add(this.disableButton);
            this.Controls.Add(this.enableButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.setupBufferNumericUpDown);
            this.Controls.Add(this.setupLabel);
            this.Controls.Add(this.promptLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SessionStartScheduleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delay Session Start";
            ((System.ComponentModel.ISupportInitialize)(this.setupBufferNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker delayedStartDateTimePicker;
        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label setupLabel;
        private System.Windows.Forms.NumericUpDown setupBufferNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button enableButton;
        private System.Windows.Forms.Button disableButton;
        private System.Windows.Forms.Button cancelButton;
    }
}
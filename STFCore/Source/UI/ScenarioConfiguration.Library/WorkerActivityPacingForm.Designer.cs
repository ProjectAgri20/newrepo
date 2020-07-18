namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class WorkerActivityPacingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerActivityPacingForm));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.enableCheckBox = new System.Windows.Forms.CheckBox();
            this.pacingTimeDelayControl = new HP.ScalableTest.UI.TimeDelayControl();
            this.delayAfterAllRadioButton = new System.Windows.Forms.RadioButton();
            this.delayForEachRadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(279, 309);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 32);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(360, 309);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 32);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // enableCheckBox
            // 
            this.enableCheckBox.AutoSize = true;
            this.enableCheckBox.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.enableCheckBox.FlatAppearance.BorderSize = 10;
            this.enableCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.enableCheckBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Blue;
            this.enableCheckBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.enableCheckBox.Location = new System.Drawing.Point(12, 12);
            this.enableCheckBox.Name = "enableCheckBox";
            this.enableCheckBox.Size = new System.Drawing.Size(229, 22);
            this.enableCheckBox.TabIndex = 7;
            this.enableCheckBox.Tag = resources.GetString("enableCheckBox.Tag");
            this.enableCheckBox.Text = "Enable Activity Specific Pacing";
            this.enableCheckBox.UseVisualStyleBackColor = true;
            this.enableCheckBox.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.HelpRequestedEvent);
            // 
            // pacingTimeDelayControl
            // 
            this.pacingTimeDelayControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.pacingTimeDelayControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pacingTimeDelayControl.Location = new System.Drawing.Point(12, 53);
            this.pacingTimeDelayControl.Name = "pacingTimeDelayControl";
            this.pacingTimeDelayControl.Size = new System.Drawing.Size(266, 143);
            this.pacingTimeDelayControl.TabIndex = 0;
            this.pacingTimeDelayControl.Text = "Activity Pacing Value";
            // 
            // delayAfterAllRadioButton
            // 
            this.delayAfterAllRadioButton.AutoSize = true;
            this.delayAfterAllRadioButton.Location = new System.Drawing.Point(12, 239);
            this.delayAfterAllRadioButton.Name = "delayAfterAllRadioButton";
            this.delayAfterAllRadioButton.Size = new System.Drawing.Size(409, 22);
            this.delayAfterAllRadioButton.TabIndex = 8;
            this.delayAfterAllRadioButton.Tag = resources.GetString("delayAfterAllRadioButton.Tag");
            this.delayAfterAllRadioButton.Text = "Apply delay once after Activity executes \"Run Count\" times";
            this.delayAfterAllRadioButton.UseVisualStyleBackColor = true;
            this.delayAfterAllRadioButton.CheckedChanged += new System.EventHandler(this.delayRadioButton_CheckedChanged);
            this.delayAfterAllRadioButton.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.HelpRequestedEvent);
            // 
            // delayForEachRadioButton
            // 
            this.delayForEachRadioButton.AutoSize = true;
            this.delayForEachRadioButton.Checked = true;
            this.delayForEachRadioButton.Location = new System.Drawing.Point(12, 211);
            this.delayForEachRadioButton.Name = "delayForEachRadioButton";
            this.delayForEachRadioButton.Size = new System.Drawing.Size(346, 22);
            this.delayForEachRadioButton.TabIndex = 9;
            this.delayForEachRadioButton.TabStop = true;
            this.delayForEachRadioButton.Tag = "This option will ensure that the defined Pacing delay for this Activity will be a" +
    "pplied after every execution of the Activity.  ";
            this.delayForEachRadioButton.Text = "Apply delay after every time the Activity executes ";
            this.delayForEachRadioButton.UseVisualStyleBackColor = true;
            this.delayForEachRadioButton.CheckedChanged += new System.EventHandler(this.delayRadioButton_CheckedChanged);
            this.delayForEachRadioButton.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.HelpRequestedEvent);
            // 
            // WorkerActivityPacingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 353);
            this.Controls.Add(this.delayForEachRadioButton);
            this.Controls.Add(this.delayAfterAllRadioButton);
            this.Controls.Add(this.enableCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.pacingTimeDelayControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkerActivityPacingForm";
            this.Text = "Worker Activity Pacing";
            this.Load += new System.EventHandler(this.WorkerActivityPacingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeDelayControl pacingTimeDelayControl;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox enableCheckBox;
        private System.Windows.Forms.RadioButton delayAfterAllRadioButton;
        private System.Windows.Forms.RadioButton delayForEachRadioButton;
    }
}
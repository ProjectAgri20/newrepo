namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class ActivityExecutionHelpForm
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
            this.help_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // help_Label
            // 
            this.help_Label.BackColor = System.Drawing.SystemColors.Info;
            this.help_Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.help_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.help_Label.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.help_Label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.help_Label.Location = new System.Drawing.Point(0, 0);
            this.help_Label.Margin = new System.Windows.Forms.Padding(5);
            this.help_Label.Name = "help_Label";
            this.help_Label.Size = new System.Drawing.Size(344, 201);
            this.help_Label.TabIndex = 104;
            // 
            // ActivityExecutionHelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 201);
            this.Controls.Add(this.help_Label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ActivityExecutionHelpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ActivityExecutionHelpForm";
            this.Load += new System.EventHandler(this.ActivityExecutionHelpForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label help_Label;
    }
}
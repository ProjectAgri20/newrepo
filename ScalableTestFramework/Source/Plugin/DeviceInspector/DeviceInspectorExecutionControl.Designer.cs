namespace HP.ScalableTest.Plugin.DeviceInspector
{
    partial class DeviceInspectorExecutionControl
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
            status_Label = new System.Windows.Forms.Label();
            status_RichTextBox = new System.Windows.Forms.RichTextBox();
            SuspendLayout();
            // 
            // status_Label
            // 
            status_Label.AutoSize = true;
            status_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            status_Label.Location = new System.Drawing.Point(3, 9);
            status_Label.Name = "status_Label";
            status_Label.Size = new System.Drawing.Size(100, 15);
            status_Label.TabIndex = 0;
            status_Label.Text = "Execution Status";
            // 
            // status_RichTextBox
            // 
            status_RichTextBox.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right);
            status_RichTextBox.Location = new System.Drawing.Point(6, 27);
            status_RichTextBox.Name = "status_RichTextBox";
            status_RichTextBox.ReadOnly = true;
            status_RichTextBox.Size = new System.Drawing.Size(428, 304);
            status_RichTextBox.TabIndex = 1;
            status_RichTextBox.Text = "";
            status_RichTextBox.WordWrap = false;
            // 
            // DeviceConfigurationExecutionControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(status_RichTextBox);
            Controls.Add(status_Label);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Name = "DeviceConfigurationExecutionControl";
            Size = new System.Drawing.Size(437, 335);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label status_Label;
        private System.Windows.Forms.RichTextBox status_RichTextBox;
    }
}

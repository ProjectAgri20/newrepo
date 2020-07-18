using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DSSConfiguration.UIMaps;

namespace HP.ScalableTest.Plugin.DSSConfiguration
{
    partial class DssConfigurationExecutionControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            statusLabel = new Label();
            statusRichTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // status_Label
            // 
            statusLabel.AutoSize = true;
            statusLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            statusLabel.Location = new Point(3, 9);
            statusLabel.Name = "status_Label";
            statusLabel.Size = new Size(100, 15);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "Execution Status";
            // 
            // status_RichTextBox
            // 
            statusRichTextBox.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right);
            statusRichTextBox.Location = new Point(6, 27);
            statusRichTextBox.Name = "status_RichTextBox";
            statusRichTextBox.ReadOnly = true;
            statusRichTextBox.Size = new Size(428, 304);
            statusRichTextBox.TabIndex = 1;
            statusRichTextBox.Text = "";
            statusRichTextBox.WordWrap = false;
            // 
            // DSSConfigurationExecutionControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(statusRichTextBox);
            Controls.Add(statusLabel);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "DSSConfigurationExecutionControl";
            Size = new Size(437, 335);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label statusLabel;
        private RichTextBox statusRichTextBox;
    }
}

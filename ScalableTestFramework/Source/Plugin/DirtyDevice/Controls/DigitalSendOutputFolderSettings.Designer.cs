namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    partial class DigitalSendOutputFolderSettings
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
            this.OutputFolderLabel = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.UncFolderComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // OutputFolderLabel
            // 
            this.OutputFolderLabel.AutoSize = true;
            this.OutputFolderLabel.Location = new System.Drawing.Point(40, 4);
            this.OutputFolderLabel.Name = "OutputFolderLabel";
            this.OutputFolderLabel.Size = new System.Drawing.Size(74, 13);
            this.OutputFolderLabel.TabIndex = 2;
            this.OutputFolderLabel.Text = "Output Folder:";
            // 
            // UncFolderComboBox
            // 
            this.UncFolderComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UncFolderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UncFolderComboBox.FormattingEnabled = true;
            this.UncFolderComboBox.Location = new System.Drawing.Point(113, 1);
            this.UncFolderComboBox.Name = "UncFolderComboBox";
            this.UncFolderComboBox.Size = new System.Drawing.Size(282, 21);
            this.UncFolderComboBox.TabIndex = 12;
            // 
            // DigitalSendOutputFolderSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UncFolderComboBox);
            this.Controls.Add(this.OutputFolderLabel);
            this.Name = "DigitalSendOutputFolderSettings";
            this.Size = new System.Drawing.Size(398, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label OutputFolderLabel;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.ComboBox UncFolderComboBox;
    }
}

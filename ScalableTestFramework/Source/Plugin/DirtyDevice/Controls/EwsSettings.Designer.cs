namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    partial class EwsSettings
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
            this.QuickSetNameTextBox = new System.Windows.Forms.TextBox();
            this.QuicksetNameLabel = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.SuspendLayout();
            // 
            // QuickSetNameTextBox
            // 
            this.QuickSetNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuickSetNameTextBox.Location = new System.Drawing.Point(113, 3);
            this.QuickSetNameTextBox.Name = "QuickSetNameTextBox";
            this.QuickSetNameTextBox.Size = new System.Drawing.Size(282, 20);
            this.QuickSetNameTextBox.TabIndex = 1;
            this.QuickSetNameTextBox.Text = "Dirty Ews";
            // 
            // QuicksetNameLabel
            // 
            this.QuicksetNameLabel.AutoSize = true;
            this.QuicksetNameLabel.Location = new System.Drawing.Point(24, 6);
            this.QuicksetNameLabel.Name = "QuicksetNameLabel";
            this.QuicksetNameLabel.Size = new System.Drawing.Size(83, 13);
            this.QuicksetNameLabel.TabIndex = 0;
            this.QuicksetNameLabel.Text = "Quickset Name:";
            // 
            // EwsSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.QuickSetNameTextBox);
            this.Controls.Add(this.QuicksetNameLabel);
            this.Name = "EwsSettings";
            this.Size = new System.Drawing.Size(398, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox QuickSetNameTextBox;
        private System.Windows.Forms.Label QuicksetNameLabel;
        private Framework.UI.FieldValidator fieldValidator;
    }
}

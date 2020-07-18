namespace HP.ScalableTest.UI.SessionExecution
{
    partial class OfflineDevicesMessageBox
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
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.offlineDevicesChecklistBoxes = new System.Windows.Forms.CheckedListBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(8, 8);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(280, 61);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "During the last session some devices have been put offline. Check the Devices you" +
    " want to Bring back Online";
            // 
            // offlineDevicesChecklistBoxes
            // 
            this.offlineDevicesChecklistBoxes.FormattingEnabled = true;
            this.offlineDevicesChecklistBoxes.Location = new System.Drawing.Point(12, 73);
            this.offlineDevicesChecklistBoxes.Name = "offlineDevicesChecklistBoxes";
            this.offlineDevicesChecklistBoxes.Size = new System.Drawing.Size(276, 238);
            this.offlineDevicesChecklistBoxes.TabIndex = 1;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(11, 327);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(121, 40);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Ok
            // 
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Ok.Location = new System.Drawing.Point(178, 329);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(110, 39);
            this.Ok.TabIndex = 3;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // OfflineDevicesMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 378);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.offlineDevicesChecklistBoxes);
            this.Controls.Add(this.descriptionLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OfflineDevicesMessageBox";
            this.Text = "OfflineDevicesMessageBox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.CheckedListBox offlineDevicesChecklistBoxes;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Ok;
    }
}
namespace HP.ScalableTest.Plugin.MessageCenter
{
    partial class MessageCenterExecutionControl
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
            this.status_richTextBox = new System.Windows.Forms.RichTextBox();
            this.status_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.controlPanel_pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.controlPanel_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // status_richTextBox
            // 
            this.status_richTextBox.BackColor = System.Drawing.Color.LightGray;
            this.status_richTextBox.Location = new System.Drawing.Point(6, 381);
            this.status_richTextBox.Name = "status_richTextBox";
            this.status_richTextBox.ReadOnly = true;
            this.status_richTextBox.Size = new System.Drawing.Size(620, 96);
            this.status_richTextBox.TabIndex = 2;
            this.status_richTextBox.Text = "";
            // 
            // status_label
            // 
            this.status_label.AutoSize = true;
            this.status_label.Location = new System.Drawing.Point(3, 361);
            this.status_label.Name = "status_label";
            this.status_label.Size = new System.Drawing.Size(42, 15);
            this.status_label.TabIndex = 3;
            this.status_label.Text = "Status:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Control Panel Snapshot:";
            // 
            // controlPanel_pictureBox
            // 
            this.controlPanel_pictureBox.Location = new System.Drawing.Point(6, 32);
            this.controlPanel_pictureBox.Name = "controlPanel_pictureBox";
            this.controlPanel_pictureBox.Size = new System.Drawing.Size(620, 326);
            this.controlPanel_pictureBox.TabIndex = 0;
            this.controlPanel_pictureBox.TabStop = false;
            // 
            // MessageCenterExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.status_label);
            this.Controls.Add(this.status_richTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.controlPanel_pictureBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MessageCenterExecutionControl";
            this.Size = new System.Drawing.Size(640, 480);
            ((System.ComponentModel.ISupportInitialize)(this.controlPanel_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox status_richTextBox;
        private System.Windows.Forms.Label status_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox controlPanel_pictureBox;
    }
}

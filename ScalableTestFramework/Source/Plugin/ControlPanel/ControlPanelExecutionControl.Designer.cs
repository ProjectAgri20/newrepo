namespace HP.ScalableTest.Plugin.ControlPanel
{
    partial class ControlPanelExecutionControl
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
            this.pluginStatus_TextBox = new System.Windows.Forms.TextBox();
            this.statusTitle_Label = new System.Windows.Forms.Label();
            this.controlPanel_pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.controlPanel_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pluginStatus_TextBox
            // 
            this.pluginStatus_TextBox.Location = new System.Drawing.Point(0, 14);
            this.pluginStatus_TextBox.Multiline = true;
            this.pluginStatus_TextBox.Name = "pluginStatus_TextBox";
            this.pluginStatus_TextBox.ReadOnly = true;
            this.pluginStatus_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.pluginStatus_TextBox.Size = new System.Drawing.Size(600, 117);
            this.pluginStatus_TextBox.TabIndex = 8;
            this.pluginStatus_TextBox.WordWrap = false;
            // 
            // statusTitle_Label
            // 
            this.statusTitle_Label.AutoSize = true;
            this.statusTitle_Label.Location = new System.Drawing.Point(-2, -2);
            this.statusTitle_Label.Name = "statusTitle_Label";
            this.statusTitle_Label.Size = new System.Drawing.Size(87, 13);
            this.statusTitle_Label.TabIndex = 7;
            this.statusTitle_Label.Text = "Execution Status";
            // 
            // controlPanel_pictureBox
            // 
            this.controlPanel_pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel_pictureBox.Location = new System.Drawing.Point(0, 137);
            this.controlPanel_pictureBox.Name = "controlPanel_pictureBox";
            this.controlPanel_pictureBox.Size = new System.Drawing.Size(600, 250);
            this.controlPanel_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.controlPanel_pictureBox.TabIndex = 9;
            this.controlPanel_pictureBox.TabStop = false;
            // 
            // ControlPanelExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.controlPanel_pictureBox);
            this.Controls.Add(this.pluginStatus_TextBox);
            this.Controls.Add(this.statusTitle_Label);
            this.Name = "ControlPanelExecutionControl";
            this.Size = new System.Drawing.Size(606, 395);
            ((System.ComponentModel.ISupportInitialize)(this.controlPanel_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pluginStatus_TextBox;
        private System.Windows.Forms.Label statusTitle_Label;
        private System.Windows.Forms.PictureBox controlPanel_pictureBox;
    }
}

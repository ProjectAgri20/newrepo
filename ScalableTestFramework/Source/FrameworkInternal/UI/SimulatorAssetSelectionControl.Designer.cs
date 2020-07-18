namespace HP.ScalableTest.Framework.UI
{
    partial class SimulatorAssetSelectionControl
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
            this.simulatorConfiguration_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // simulatorConfiguration_LinkLabel
            // 
            this.simulatorConfiguration_LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simulatorConfiguration_LinkLabel.AutoSize = true;
            this.simulatorConfiguration_LinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.simulatorConfiguration_LinkLabel.Location = new System.Drawing.Point(554, 7);
            this.simulatorConfiguration_LinkLabel.Name = "simulatorConfiguration_LinkLabel";
            this.simulatorConfiguration_LinkLabel.Size = new System.Drawing.Size(135, 15);
            this.simulatorConfiguration_LinkLabel.TabIndex = 2;
            this.simulatorConfiguration_LinkLabel.TabStop = true;
            this.simulatorConfiguration_LinkLabel.Text = "Simulator Configuration";
            this.simulatorConfiguration_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.simulatorConfiguration_LinkLabel_LinkClicked);
            // 
            // SimulatorAssetSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.Controls.Add(this.simulatorConfiguration_LinkLabel);
            this.Name = "SimulatorAssetSelectionControl";
            this.Controls.SetChildIndex(this.simulatorConfiguration_LinkLabel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel simulatorConfiguration_LinkLabel;
    }
}

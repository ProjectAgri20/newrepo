namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class ScenarioConfigurationControlForm
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
            this.scenarioConfigurationControl_Panel = new System.Windows.Forms.Panel();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.resource_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.resource_ToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.resource_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenarioConfigurationControl_Panel
            // 
            this.scenarioConfigurationControl_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioConfigurationControl_Panel.Location = new System.Drawing.Point(12, 28);
            this.scenarioConfigurationControl_Panel.Name = "scenarioConfigurationControl_Panel";
            this.scenarioConfigurationControl_Panel.Size = new System.Drawing.Size(873, 586);
            this.scenarioConfigurationControl_Panel.TabIndex = 0;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(679, 620);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 28);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(785, 620);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 28);
            this.cancel_Button.TabIndex = 2;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // resource_ToolStrip
            // 
            this.resource_ToolStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.resource_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.resource_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resource_ToolStripLabel});
            this.resource_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.resource_ToolStrip.Name = "resource_ToolStrip";
            this.resource_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.resource_ToolStrip.Size = new System.Drawing.Size(897, 25);
            this.resource_ToolStrip.TabIndex = 6;
            this.resource_ToolStrip.Text = "toolStrip1";
            // 
            // resource_ToolStripLabel
            // 
            this.resource_ToolStripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.resource_ToolStripLabel.BackColor = System.Drawing.SystemColors.Control;
            this.resource_ToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.resource_ToolStripLabel.Name = "resource_ToolStripLabel";
            this.resource_ToolStripLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // ScenarioConfigurationControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 660);
            this.Controls.Add(this.resource_ToolStrip);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.scenarioConfigurationControl_Panel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(742, 698);
            this.Name = "ScenarioConfigurationControlForm";
            this.Text = "Test Object Configuration";
            this.Load += new System.EventHandler(this.ScenarioConfigurationControlForm_Load);
            this.resource_ToolStrip.ResumeLayout(false);
            this.resource_ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel scenarioConfigurationControl_Panel;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.ToolStrip resource_ToolStrip;
        private System.Windows.Forms.ToolStripLabel resource_ToolStripLabel;
    }
}
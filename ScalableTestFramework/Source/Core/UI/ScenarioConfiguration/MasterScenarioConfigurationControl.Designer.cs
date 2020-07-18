namespace HP.ScalableTest.Core.UI.ScenarioConfiguration
{
    partial class MasterScenarioConfigurationControl
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
            if (disposing)
            {
                _uiController.Dispose();
            }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterScenarioConfigurationControl));
            this.scenarioConfiguration_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.scenarioConfigurationTreeView = new HP.ScalableTest.Core.UI.ScenarioConfiguration.ScenarioConfigurationTreeView();
            this.configurationTreeView_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.configurationTreeRefresh_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.configurationEdit_Panel = new System.Windows.Forms.Panel();
            this.configurationEdit_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.save_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfiguration_SplitContainer)).BeginInit();
            this.scenarioConfiguration_SplitContainer.Panel1.SuspendLayout();
            this.scenarioConfiguration_SplitContainer.Panel2.SuspendLayout();
            this.scenarioConfiguration_SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfigurationTreeView)).BeginInit();
            this.configurationTreeView_ToolStrip.SuspendLayout();
            this.configurationEdit_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenarioConfiguration_SplitContainer
            // 
            this.scenarioConfiguration_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scenarioConfiguration_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioConfiguration_SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.scenarioConfiguration_SplitContainer.Name = "scenarioConfiguration_SplitContainer";
            // 
            // scenarioConfiguration_SplitContainer.Panel1
            // 
            this.scenarioConfiguration_SplitContainer.Panel1.Controls.Add(this.scenarioConfigurationTreeView);
            this.scenarioConfiguration_SplitContainer.Panel1.Controls.Add(this.configurationTreeView_ToolStrip);
            this.scenarioConfiguration_SplitContainer.Panel1MinSize = 75;
            // 
            // scenarioConfiguration_SplitContainer.Panel2
            // 
            this.scenarioConfiguration_SplitContainer.Panel2.Controls.Add(this.configurationEdit_Panel);
            this.scenarioConfiguration_SplitContainer.Panel2.Controls.Add(this.configurationEdit_ToolStrip);
            this.scenarioConfiguration_SplitContainer.Size = new System.Drawing.Size(800, 600);
            this.scenarioConfiguration_SplitContainer.SplitterDistance = 216;
            this.scenarioConfiguration_SplitContainer.TabIndex = 0;
            // 
            // scenarioConfigurationTreeView
            // 
            this.scenarioConfigurationTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioConfigurationTreeView.Location = new System.Drawing.Point(0, 25);
            this.scenarioConfigurationTreeView.Name = "scenarioConfigurationTreeView";
            this.scenarioConfigurationTreeView.Size = new System.Drawing.Size(214, 573);
            this.scenarioConfigurationTreeView.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.scenarioConfigurationTreeView.SpacingBetweenNodes = -1;
            this.scenarioConfigurationTreeView.TabIndex = 3;
            this.scenarioConfigurationTreeView.Text = "scenarioConfigurationTreeView1";
            // 
            // configurationTreeView_ToolStrip
            // 
            this.configurationTreeView_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.configurationTreeView_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationTreeRefresh_ToolStripButton});
            this.configurationTreeView_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.configurationTreeView_ToolStrip.Name = "configurationTreeView_ToolStrip";
            this.configurationTreeView_ToolStrip.Size = new System.Drawing.Size(214, 25);
            this.configurationTreeView_ToolStrip.TabIndex = 2;
            this.configurationTreeView_ToolStrip.Text = "toolStrip1";
            // 
            // configurationTreeRefresh_ToolStripButton
            // 
            this.configurationTreeRefresh_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("configurationTreeRefresh_ToolStripButton.Image")));
            this.configurationTreeRefresh_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.configurationTreeRefresh_ToolStripButton.Name = "configurationTreeRefresh_ToolStripButton";
            this.configurationTreeRefresh_ToolStripButton.Size = new System.Drawing.Size(66, 22);
            this.configurationTreeRefresh_ToolStripButton.Text = "Refresh";
            this.configurationTreeRefresh_ToolStripButton.ToolTipText = "Refresh the configuration tree view from the database.";
            this.configurationTreeRefresh_ToolStripButton.Click += new System.EventHandler(this.configurationTreeRefresh_ToolStripButton_Click);
            // 
            // configurationEdit_Panel
            // 
            this.configurationEdit_Panel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.configurationEdit_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configurationEdit_Panel.Location = new System.Drawing.Point(0, 25);
            this.configurationEdit_Panel.Name = "configurationEdit_Panel";
            this.configurationEdit_Panel.Size = new System.Drawing.Size(578, 573);
            this.configurationEdit_Panel.TabIndex = 1;
            // 
            // configurationEdit_ToolStrip
            // 
            this.configurationEdit_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.configurationEdit_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.save_ToolStripButton});
            this.configurationEdit_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.configurationEdit_ToolStrip.Name = "configurationEdit_ToolStrip";
            this.configurationEdit_ToolStrip.Size = new System.Drawing.Size(578, 25);
            this.configurationEdit_ToolStrip.TabIndex = 0;
            this.configurationEdit_ToolStrip.Text = "toolStrip1";
            // 
            // save_ToolStripButton
            // 
            this.save_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("save_ToolStripButton.Image")));
            this.save_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.save_ToolStripButton.Name = "save_ToolStripButton";
            this.save_ToolStripButton.Size = new System.Drawing.Size(51, 22);
            this.save_ToolStripButton.Text = "Save";
            this.save_ToolStripButton.ToolTipText = "Save changes to the configuration data.";
            this.save_ToolStripButton.Click += new System.EventHandler(this.save_ToolStripButton_Click);
            // 
            // MasterScenarioConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scenarioConfiguration_SplitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MasterScenarioConfigurationControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.scenarioConfiguration_SplitContainer.Panel1.ResumeLayout(false);
            this.scenarioConfiguration_SplitContainer.Panel1.PerformLayout();
            this.scenarioConfiguration_SplitContainer.Panel2.ResumeLayout(false);
            this.scenarioConfiguration_SplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfiguration_SplitContainer)).EndInit();
            this.scenarioConfiguration_SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scenarioConfigurationTreeView)).EndInit();
            this.configurationTreeView_ToolStrip.ResumeLayout(false);
            this.configurationTreeView_ToolStrip.PerformLayout();
            this.configurationEdit_ToolStrip.ResumeLayout(false);
            this.configurationEdit_ToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scenarioConfiguration_SplitContainer;
        private ScenarioConfigurationTreeView scenarioConfigurationTreeView;
        private System.Windows.Forms.ToolStrip configurationTreeView_ToolStrip;
        private System.Windows.Forms.ToolStrip configurationEdit_ToolStrip;
        private System.Windows.Forms.Panel configurationEdit_Panel;
        private System.Windows.Forms.ToolStripButton configurationTreeRefresh_ToolStripButton;
        private System.Windows.Forms.ToolStripButton save_ToolStripButton;
    }
}

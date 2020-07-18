namespace HP.ScalableTest.Plugin.Printing
{
    partial class PrintingConfigurationControl
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintingConfigurationControl));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.printQueueSelection_TabPage = new System.Windows.Forms.TabPage();
            this.printQueues_GridView = new Telerik.WinControls.UI.RadGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.enableThrottling_checkBox = new System.Windows.Forms.CheckBox();
            this.maxJobs_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.jobseparator_checkBox = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.localQueues_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remoteQueues_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.documentSelection_TabPage = new System.Windows.Forms.TabPage();
            this.document_GroupBox = new System.Windows.Forms.GroupBox();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.shuffle_CheckBox = new System.Windows.Forms.CheckBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabControl.SuspendLayout();
            this.printQueueSelection_TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView.MasterTemplate)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxJobs_NumericUpDown)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.documentSelection_TabPage.SuspendLayout();
            this.document_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.printQueueSelection_TabPage);
            this.tabControl.Controls.Add(this.documentSelection_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(667, 500);
            this.tabControl.TabIndex = 38;
            // 
            // printQueueSelection_TabPage
            // 
            this.printQueueSelection_TabPage.Controls.Add(this.printQueues_GridView);
            this.printQueueSelection_TabPage.Controls.Add(this.panel1);
            this.printQueueSelection_TabPage.Controls.Add(this.toolStrip1);
            this.printQueueSelection_TabPage.Location = new System.Drawing.Point(4, 24);
            this.printQueueSelection_TabPage.Name = "printQueueSelection_TabPage";
            this.printQueueSelection_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.printQueueSelection_TabPage.Size = new System.Drawing.Size(659, 472);
            this.printQueueSelection_TabPage.TabIndex = 1;
            this.printQueueSelection_TabPage.Text = "Print Queue Selection";
            // 
            // printQueues_GridView
            // 
            this.printQueues_GridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printQueues_GridView.Location = new System.Drawing.Point(3, 28);
            // 
            // 
            // 
            this.printQueues_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "QueueName";
            gridViewTextBoxColumn1.HeaderText = "Queue Name";
            gridViewTextBoxColumn1.Name = "queueName_GridViewColumn";
            gridViewTextBoxColumn1.Width = 326;
            gridViewTextBoxColumn2.FieldName = "PrintServer";
            gridViewTextBoxColumn2.HeaderText = "Print Server";
            gridViewTextBoxColumn2.MaxWidth = 150;
            gridViewTextBoxColumn2.Name = "printServer_GridViewColumn";
            gridViewTextBoxColumn2.Width = 150;
            gridViewTextBoxColumn3.FieldName = "QueueType";
            gridViewTextBoxColumn3.HeaderText = "Type";
            gridViewTextBoxColumn3.MaxWidth = 80;
            gridViewTextBoxColumn3.MinWidth = 80;
            gridViewTextBoxColumn3.Name = "queueType_GridViewColumn";
            gridViewTextBoxColumn3.Width = 80;
            gridViewTextBoxColumn4.FieldName = "Device";
            gridViewTextBoxColumn4.HeaderText = "Device";
            gridViewTextBoxColumn4.MaxWidth = 80;
            gridViewTextBoxColumn4.MinWidth = 80;
            gridViewTextBoxColumn4.Name = "device_GridViewColumn";
            gridViewTextBoxColumn4.Width = 80;
            this.printQueues_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.printQueues_GridView.Name = "printQueues_GridView";
            this.printQueues_GridView.Size = new System.Drawing.Size(653, 398);
            this.printQueues_GridView.TabIndex = 52;
            this.printQueues_GridView.Text = "radGridView1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.enableThrottling_checkBox);
            this.panel1.Controls.Add(this.maxJobs_NumericUpDown);
            this.panel1.Controls.Add(this.jobseparator_checkBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 426);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(653, 43);
            this.panel1.TabIndex = 1;
            // 
            // enableThrottling_checkBox
            // 
            this.enableThrottling_checkBox.AutoSize = true;
            this.enableThrottling_checkBox.Location = new System.Drawing.Point(167, 14);
            this.enableThrottling_checkBox.Name = "enableThrottling_checkBox";
            this.enableThrottling_checkBox.Size = new System.Drawing.Size(241, 19);
            this.enableThrottling_checkBox.TabIndex = 0;
            this.enableThrottling_checkBox.Text = "Limit number of active jobs per queue to";
            this.enableThrottling_checkBox.UseVisualStyleBackColor = true;
            this.enableThrottling_checkBox.CheckedChanged += new System.EventHandler(this.enableThrottling_checkBox_CheckedChanged);
            // 
            // maxJobs_NumericUpDown
            // 
            this.maxJobs_NumericUpDown.Location = new System.Drawing.Point(414, 13);
            this.maxJobs_NumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.maxJobs_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxJobs_NumericUpDown.Name = "maxJobs_NumericUpDown";
            this.maxJobs_NumericUpDown.Size = new System.Drawing.Size(120, 23);
            this.maxJobs_NumericUpDown.TabIndex = 58;
            this.maxJobs_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // jobseparator_checkBox
            // 
            this.jobseparator_checkBox.AutoSize = true;
            this.jobseparator_checkBox.Location = new System.Drawing.Point(9, 14);
            this.jobseparator_checkBox.Name = "jobseparator_checkBox";
            this.jobseparator_checkBox.Size = new System.Drawing.Size(123, 19);
            this.jobseparator_checkBox.TabIndex = 55;
            this.jobseparator_checkBox.Text = "Print job separator";
            this.jobseparator_checkBox.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localQueues_ToolStripButton,
            this.remoteQueues_ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(653, 25);
            this.toolStrip1.TabIndex = 53;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // localQueues_ToolStripButton
            // 
            this.localQueues_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("localQueues_ToolStripButton.Image")));
            this.localQueues_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.localQueues_ToolStripButton.Name = "localQueues_ToolStripButton";
            this.localQueues_ToolStripButton.Size = new System.Drawing.Size(98, 22);
            this.localQueues_ToolStripButton.Text = "Local Queues";
            this.localQueues_ToolStripButton.Click += new System.EventHandler(this.localQueues_ToolStripButton_Click);
            // 
            // remoteQueues_ToolStripButton
            // 
            this.remoteQueues_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remoteQueues_ToolStripButton.Image")));
            this.remoteQueues_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remoteQueues_ToolStripButton.Name = "remoteQueues_ToolStripButton";
            this.remoteQueues_ToolStripButton.Size = new System.Drawing.Size(111, 22);
            this.remoteQueues_ToolStripButton.Text = "Remote Queues";
            this.remoteQueues_ToolStripButton.Click += new System.EventHandler(this.remoteQueues_ToolStripButton_Click);
            // 
            // documentSelection_TabPage
            // 
            this.documentSelection_TabPage.Controls.Add(this.document_GroupBox);
            this.documentSelection_TabPage.Controls.Add(this.shuffle_CheckBox);
            this.documentSelection_TabPage.Location = new System.Drawing.Point(4, 24);
            this.documentSelection_TabPage.Name = "documentSelection_TabPage";
            this.documentSelection_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.documentSelection_TabPage.Size = new System.Drawing.Size(659, 472);
            this.documentSelection_TabPage.TabIndex = 0;
            this.documentSelection_TabPage.Text = "Document Selection";
            // 
            // document_GroupBox
            // 
            this.document_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.document_GroupBox.Controls.Add(this.documentSelectionControl);
            this.document_GroupBox.Location = new System.Drawing.Point(7, 48);
            this.document_GroupBox.Name = "document_GroupBox";
            this.document_GroupBox.Size = new System.Drawing.Size(646, 413);
            this.document_GroupBox.TabIndex = 53;
            this.document_GroupBox.TabStop = false;
            this.document_GroupBox.Text = "Document Selection";
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(3, 19);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.Size = new System.Drawing.Size(640, 391);
            this.documentSelectionControl.TabIndex = 0;
            // 
            // shuffle_CheckBox
            // 
            this.shuffle_CheckBox.Location = new System.Drawing.Point(10, 14);
            this.shuffle_CheckBox.Name = "shuffle_CheckBox";
            this.shuffle_CheckBox.Size = new System.Drawing.Size(200, 19);
            this.shuffle_CheckBox.TabIndex = 51;
            this.shuffle_CheckBox.Text = "Shuffle document printing order";
            // 
            // PrintingConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintingConfigurationControl";
            this.Size = new System.Drawing.Size(667, 500);
            this.tabControl.ResumeLayout(false);
            this.printQueueSelection_TabPage.ResumeLayout(false);
            this.printQueueSelection_TabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueues_GridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxJobs_NumericUpDown)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.documentSelection_TabPage.ResumeLayout(false);
            this.document_GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage documentSelection_TabPage;
        private System.Windows.Forms.TabPage printQueueSelection_TabPage;
        private System.Windows.Forms.CheckBox shuffle_CheckBox;
        private System.Windows.Forms.CheckBox enableThrottling_checkBox;
        private System.Windows.Forms.GroupBox document_GroupBox;
        private System.Windows.Forms.CheckBox jobseparator_checkBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.NumericUpDown maxJobs_NumericUpDown;
        private Framework.UI.DocumentSelectionControl documentSelectionControl;
        private Telerik.WinControls.UI.RadGridView printQueues_GridView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton localQueues_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remoteQueues_ToolStripButton;
        private System.Windows.Forms.Panel panel1;
    }
}

namespace HP.ScalableTest.LabConsole
{
    partial class PrintServerUsageDetails
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintServerUsageDetails));
            this.printQueueInUseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.close_Button = new System.Windows.Forms.Button();
            this.queues_DataGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.printQueueInUseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.queues_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.queues_DataGridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // printQueueInUseBindingSource
            // 
            this.printQueueInUseBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.PrintQueueInUse);
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.Location = new System.Drawing.Point(833, 430);
            this.close_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(100, 30);
            this.close_Button.TabIndex = 1;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            this.close_Button.Click += new System.EventHandler(this.close_Button_Click);
            // 
            // queues_DataGridView
            // 
            this.queues_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queues_DataGridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.queues_DataGridView.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.queues_DataGridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.queues_DataGridView.Location = new System.Drawing.Point(8, 8);
            // 
            // 
            // 
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "QueueName";
            gridViewTextBoxColumn1.HeaderText = "Queue";
            gridViewTextBoxColumn1.Name = "queueName_GridViewColumn";
            gridViewTextBoxColumn1.Width = 110;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "ScenarioName";
            gridViewTextBoxColumn2.HeaderText = "Scenario";
            gridViewTextBoxColumn2.Name = "scenario_GridViewColumn";
            gridViewTextBoxColumn2.Width = 40;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "ResourceType";
            gridViewTextBoxColumn3.HeaderText = "Resource Type";
            gridViewTextBoxColumn3.Name = "resourceType_GridViewColumn";
            gridViewTextBoxColumn3.Width = 40;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "VirtualResource";
            gridViewTextBoxColumn4.HeaderText = "Resource";
            gridViewTextBoxColumn4.Name = "resource_GridViewColumn";
            gridViewTextBoxColumn4.Width = 45;
            gridViewTextBoxColumn5.FieldName = "MetadataType";
            gridViewTextBoxColumn5.HeaderText = "Activity Type";
            gridViewTextBoxColumn5.Name = "metadataType_GridViewColumn";
            gridViewTextBoxColumn5.Width = 60;
            gridViewTextBoxColumn6.FieldName = "MetadataDescription";
            gridViewTextBoxColumn6.HeaderText = "Activity Description";
            gridViewTextBoxColumn6.Name = "metadataDescription_GridViewColumn";
            gridViewTextBoxColumn6.Width = 200;
            this.queues_DataGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6});
            this.queues_DataGridView.MasterTemplate.EnableGrouping = false;
            sortDescriptor1.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor1.PropertyName = "modificationTime_GridViewColumn";
            this.queues_DataGridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.queues_DataGridView.Name = "queues_DataGridView";
            this.queues_DataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.queues_DataGridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 27, 300, 187);
            this.queues_DataGridView.Size = new System.Drawing.Size(926, 415);
            this.queues_DataGridView.TabIndex = 17;
            this.queues_DataGridView.Text = "Print Servers";
            // 
            // PrintServerUsageDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 473);
            this.Controls.Add(this.queues_DataGridView);
            this.Controls.Add(this.close_Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PrintServerUsageDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Server Usage Details";
            this.Load += new System.EventHandler(this.PrintServerUsageDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.printQueueInUseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.queues_DataGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.queues_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.BindingSource printQueueInUseBindingSource;
        private Telerik.WinControls.UI.RadGridView queues_DataGridView;
    }
}
namespace HP.ScalableTest.UI.SessionExecution
{
    partial class ActivityDetailsGrid
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
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn2 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.GroupDescriptor groupDescriptor1 = new Telerik.WinControls.Data.GroupDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor2 = new Telerik.WinControls.Data.SortDescriptor();
            this.close_Button = new System.Windows.Forms.Button();
            this.activityDetails_RadGridView = new Telerik.WinControls.UI.RadGridView();
            this.activityExecution_BindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.activityDetails_RadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityDetails_RadGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityExecution_BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.Location = new System.Drawing.Point(597, 527);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 0;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            this.close_Button.Click += new System.EventHandler(this.close_Button_Click);
            // 
            // activityDetails_RadGridView
            // 
            this.activityDetails_RadGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activityDetails_RadGridView.BackColor = System.Drawing.SystemColors.Control;
            this.activityDetails_RadGridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.activityDetails_RadGridView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.activityDetails_RadGridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.activityDetails_RadGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.activityDetails_RadGridView.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.activityDetails_RadGridView.MasterTemplate.AllowAddNewRow = false;
            this.activityDetails_RadGridView.MasterTemplate.AllowDeleteRow = false;
            this.activityDetails_RadGridView.MasterTemplate.AllowEditRow = false;
            this.activityDetails_RadGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.activityDetails_RadGridView.MasterTemplate.ClipboardPasteMode = Telerik.WinControls.UI.GridViewClipboardPasteMode.Disable;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "ActivityName";
            gridViewTextBoxColumn1.HeaderText = "Activity Name";
            gridViewTextBoxColumn1.MinWidth = 100;
            gridViewTextBoxColumn1.Name = "ActivityName";
            gridViewTextBoxColumn1.Width = 100;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "ActivityType";
            gridViewTextBoxColumn2.HeaderText = "Activity Type";
            gridViewTextBoxColumn2.MinWidth = 100;
            gridViewTextBoxColumn2.Name = "ActivityType";
            gridViewTextBoxColumn2.Width = 100;
            gridViewDateTimeColumn1.EnableExpressionEditor = false;
            gridViewDateTimeColumn1.FieldName = "StartDateTime";
            gridViewDateTimeColumn1.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            gridViewDateTimeColumn1.HeaderText = "Start Date/Time";
            gridViewDateTimeColumn1.MinWidth = 100;
            gridViewDateTimeColumn1.Name = "StartDateTime";
            gridViewDateTimeColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending;
            gridViewDateTimeColumn1.Width = 100;
            gridViewDateTimeColumn2.EnableExpressionEditor = false;
            gridViewDateTimeColumn2.FieldName = "EndDateTime";
            gridViewDateTimeColumn2.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            gridViewDateTimeColumn2.HeaderText = "End Date/Time";
            gridViewDateTimeColumn2.MinWidth = 100;
            gridViewDateTimeColumn2.Name = "EndDateTime";
            gridViewDateTimeColumn2.Width = 100;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "Status";
            gridViewTextBoxColumn3.HeaderText = "Status";
            gridViewTextBoxColumn3.MinWidth = 50;
            gridViewTextBoxColumn3.Name = "Status";
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "ResultMessage";
            gridViewTextBoxColumn4.HeaderText = "Result Message";
            gridViewTextBoxColumn4.MinWidth = 100;
            gridViewTextBoxColumn4.Name = "ResultMessage";
            gridViewTextBoxColumn4.Width = 100;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "ResultCategory";
            gridViewTextBoxColumn5.HeaderText = "Result Category";
            gridViewTextBoxColumn5.MinWidth = 100;
            gridViewTextBoxColumn5.Name = "ResultCategory";
            gridViewTextBoxColumn5.Width = 100;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.FieldName = "UserName";
            gridViewTextBoxColumn6.HeaderText = "User Name";
            gridViewTextBoxColumn6.MinWidth = 100;
            gridViewTextBoxColumn6.Name = "UserName";
            gridViewTextBoxColumn6.Width = 100;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.FieldName = "HostName";
            gridViewTextBoxColumn7.HeaderText = "Host Name";
            gridViewTextBoxColumn7.MinWidth = 100;
            gridViewTextBoxColumn7.Name = "HostName";
            gridViewTextBoxColumn7.Width = 100;
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.FieldName = "ActivityExecutionId";
            gridViewTextBoxColumn8.HeaderText = "ActivityExecutionId";
            gridViewTextBoxColumn8.IsVisible = false;
            gridViewTextBoxColumn8.Name = "ActivityExecutionId";
            gridViewTextBoxColumn8.Width = 35;
            gridViewTextBoxColumn9.EnableExpressionEditor = false;
            gridViewTextBoxColumn9.FieldName = "ResourceMetadataId";
            gridViewTextBoxColumn9.HeaderText = "ResourceMetadataId";
            gridViewTextBoxColumn9.IsVisible = false;
            gridViewTextBoxColumn9.Name = "ResourceMetadataId";
            gridViewTextBoxColumn9.Width = 35;
            gridViewTextBoxColumn10.EnableExpressionEditor = false;
            gridViewTextBoxColumn10.FieldName = "SessionId";
            gridViewTextBoxColumn10.HeaderText = "SessionId";
            gridViewTextBoxColumn10.IsVisible = false;
            gridViewTextBoxColumn10.Name = "SessionId";
            gridViewTextBoxColumn10.Width = 35;
            gridViewTextBoxColumn11.EnableExpressionEditor = false;
            gridViewTextBoxColumn11.FieldName = "ResourceInstanceId";
            gridViewTextBoxColumn11.HeaderText = "ResourceInstanceId";
            gridViewTextBoxColumn11.IsVisible = false;
            gridViewTextBoxColumn11.Name = "ResourceInstanceId";
            gridViewTextBoxColumn11.Width = 35;
            gridViewTextBoxColumn12.EnableExpressionEditor = false;
            gridViewTextBoxColumn12.FieldName = "SessionInfo";
            gridViewTextBoxColumn12.HeaderText = "SessionInfo";
            gridViewTextBoxColumn12.IsVisible = false;
            gridViewTextBoxColumn12.Name = "SessionInfo";
            gridViewTextBoxColumn12.Width = 35;
            this.activityDetails_RadGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewDateTimeColumn1,
            gridViewDateTimeColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewTextBoxColumn11,
            gridViewTextBoxColumn12});
            this.activityDetails_RadGridView.MasterTemplate.DataSource = this.activityExecution_BindingSource;
            this.activityDetails_RadGridView.MasterTemplate.EnableAlternatingRowColor = true;
            sortDescriptor1.PropertyName = "Status";
            groupDescriptor1.GroupNames.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.activityDetails_RadGridView.MasterTemplate.GroupDescriptors.AddRange(new Telerik.WinControls.Data.GroupDescriptor[] {
            groupDescriptor1});
            this.activityDetails_RadGridView.MasterTemplate.ShowGroupedColumns = true;
            sortDescriptor2.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor2.PropertyName = "StartDateTime";
            this.activityDetails_RadGridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor2});
            this.activityDetails_RadGridView.Name = "activityDetails_RadGridView";
            this.activityDetails_RadGridView.ReadOnly = true;
            this.activityDetails_RadGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.activityDetails_RadGridView.Size = new System.Drawing.Size(660, 509);
            this.activityDetails_RadGridView.TabIndex = 1;
            this.activityDetails_RadGridView.Text = "radGridView1";
            this.activityDetails_RadGridView.ThemeName = "*/";
            this.activityDetails_RadGridView.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.activityDetails_RadGridView_ViewCellFormatting);
            this.activityDetails_RadGridView.GroupSummaryEvaluate += new Telerik.WinControls.UI.GroupSummaryEvaluateEventHandler(this.activityDetails_RadGridView_GroupSummaryEvaluate);
            // 
            // activityExecution_BindingSource
            // 
            this.activityExecution_BindingSource.AllowNew = false;
            // 
            // ActivityDetailsGrid
            // 
            this.AcceptButton = this.close_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.activityDetails_RadGridView);
            this.Controls.Add(this.close_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ActivityDetailsGrid";
            this.Text = "ActivityDetailsGrid";
            ((System.ComponentModel.ISupportInitialize)(this.activityDetails_RadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityDetails_RadGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityExecution_BindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        public System.Windows.Forms.BindingSource activityExecution_BindingSource;
        private Telerik.WinControls.UI.RadGridView activityDetails_RadGridView;
    }
}
namespace HP.ScalableTest.LabConsole
{
    partial class SessionReportsForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn2 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn3 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            this.close_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.generate_Button = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.session_GridView = new Telerik.WinControls.UI.RadGridView();
            this.template_TextBox = new System.Windows.Forms.TextBox();
            this.browse_Button = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.session_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.session_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(701, 382);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(100, 28);
            this.close_Button.TabIndex = 4;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            this.close_Button.Click += new System.EventHandler(this.close_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(376, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select the session(s) from which this report\'s data should be extracted.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Specify a report template:";
            // 
            // generate_Button
            // 
            this.generate_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generate_Button.Location = new System.Drawing.Point(595, 382);
            this.generate_Button.Name = "generate_Button";
            this.generate_Button.Size = new System.Drawing.Size(100, 28);
            this.generate_Button.TabIndex = 4;
            this.generate_Button.Text = "Create Report";
            this.generate_Button.UseVisualStyleBackColor = true;
            this.generate_Button.Click += new System.EventHandler(this.generate_Button_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "xlsx";
            this.saveFileDialog.Filter = "Excel Files(*.xls;*.xlsx;*.xlsm)|*.xls;*.xlsx;*.xlsm";
            this.saveFileDialog.Title = "Create Enterprise Test Report";
            // 
            // session_GridView
            // 
            this.session_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.session_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.session_GridView.Location = new System.Drawing.Point(15, 76);
            // 
            // session_GridView
            // 
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.AllowHide = false;
            gridViewTextBoxColumn1.FieldName = "SessionId";
            gridViewTextBoxColumn1.HeaderText = "Session ID";
            gridViewTextBoxColumn1.MaxWidth = 80;
            gridViewTextBoxColumn1.MinWidth = 80;
            gridViewTextBoxColumn1.Name = "sessionId_GridViewColumn";
            gridViewTextBoxColumn1.Width = 80;
            gridViewTextBoxColumn2.AllowGroup = false;
            gridViewTextBoxColumn2.FieldName = "ScenarioName";
            gridViewTextBoxColumn2.HeaderText = "Session Name";
            gridViewTextBoxColumn2.MinWidth = 200;
            gridViewTextBoxColumn2.Name = "sessionName_GridViewColumn";
            gridViewTextBoxColumn2.Width = 200;
            gridViewDateTimeColumn1.AllowGroup = false;
            gridViewDateTimeColumn1.FieldName = "StartDate";
            gridViewDateTimeColumn1.FilteringMode = Telerik.WinControls.UI.GridViewTimeFilteringMode.Date;
            gridViewDateTimeColumn1.HeaderText = "Start Date";
            gridViewDateTimeColumn1.MaxWidth = 150;
            gridViewDateTimeColumn1.MinWidth = 150;
            gridViewDateTimeColumn1.Name = "startDate_GridViewColumn";
            gridViewDateTimeColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending;
            gridViewDateTimeColumn1.Width = 150;
            gridViewTextBoxColumn3.FieldName = "Owner";
            gridViewTextBoxColumn3.HeaderText = "Owner";
            gridViewTextBoxColumn3.MaxWidth = 100;
            gridViewTextBoxColumn3.MinWidth = 100;
            gridViewTextBoxColumn3.Name = "owner_GridViewColumn";
            gridViewTextBoxColumn3.Width = 100;
            gridViewDecimalColumn1.AllowGroup = false;
            gridViewDecimalColumn1.DecimalPlaces = 0;
            gridViewDecimalColumn1.FieldName = "ActivityCount";
            gridViewDecimalColumn1.FormatString = "{0:#,#}";
            gridViewDecimalColumn1.HeaderText = "Activities";
            gridViewDecimalColumn1.MaxWidth = 80;
            gridViewDecimalColumn1.MinWidth = 80;
            gridViewDecimalColumn1.Name = "activityCount_GridViewColumn";
            gridViewDecimalColumn1.Width = 80;
            gridViewDecimalColumn2.AllowGroup = false;
            gridViewDecimalColumn2.DecimalPlaces = 0;
            gridViewDecimalColumn2.FieldName = "PrintJobCount";
            gridViewDecimalColumn2.FormatString = "{0:#,#}";
            gridViewDecimalColumn2.HeaderText = "Print Jobs";
            gridViewDecimalColumn2.MaxWidth = 80;
            gridViewDecimalColumn2.MinWidth = 80;
            gridViewDecimalColumn2.Name = "printJobCount_GridViewColumn";
            gridViewDecimalColumn2.Width = 80;
            gridViewDecimalColumn3.AllowGroup = false;
            gridViewDecimalColumn3.DecimalPlaces = 0;
            gridViewDecimalColumn3.FieldName = "ScanJobCount";
            gridViewDecimalColumn3.FormatString = "{0:#,#}";
            gridViewDecimalColumn3.HeaderText = "Scan Jobs";
            gridViewDecimalColumn3.MaxWidth = 80;
            gridViewDecimalColumn3.MinWidth = 80;
            gridViewDecimalColumn3.Name = "scanJob_GridViewColumn";
            gridViewDecimalColumn3.Width = 80;
            this.session_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewDateTimeColumn1,
            gridViewTextBoxColumn3,
            gridViewDecimalColumn1,
            gridViewDecimalColumn2,
            gridViewDecimalColumn3});
            this.session_GridView.MasterTemplate.EnableFiltering = true;
            this.session_GridView.MasterTemplate.MultiSelect = true;
            sortDescriptor1.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor1.PropertyName = "startDate_GridViewColumn";
            this.session_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.session_GridView.Name = "session_GridView";
            // 
            // 
            // 
            this.session_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(15, 76, 240, 150);
            this.session_GridView.Size = new System.Drawing.Size(786, 290);
            this.session_GridView.TabIndex = 9;
            this.session_GridView.Text = "radGridView1";
            // 
            // template_TextBox
            // 
            this.template_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.template_TextBox.Location = new System.Drawing.Point(199, 18);
            this.template_TextBox.Name = "template_TextBox";
            this.template_TextBox.Size = new System.Drawing.Size(521, 23);
            this.template_TextBox.TabIndex = 10;
            // 
            // browse_Button
            // 
            this.browse_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browse_Button.Location = new System.Drawing.Point(726, 17);
            this.browse_Button.Name = "browse_Button";
            this.browse_Button.Size = new System.Drawing.Size(75, 28);
            this.browse_Button.TabIndex = 11;
            this.browse_Button.Text = "Browse...";
            this.browse_Button.UseVisualStyleBackColor = true;
            this.browse_Button.Click += new System.EventHandler(this.browse_Button_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xlsx";
            this.openFileDialog.Filter = "Excel Files(*.xls;*.xlsx;*.xlsm)|*.xls;*.xlsx;*.xlsm";
            this.openFileDialog.Title = "Select Template";
            // 
            // SessionReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(813, 422);
            this.Controls.Add(this.browse_Button);
            this.Controls.Add(this.template_TextBox);
            this.Controls.Add(this.session_GridView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.generate_Button);
            this.Controls.Add(this.close_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionReportsForm";
            this.Text = "Session Reports";
            ((System.ComponentModel.ISupportInitialize)(this.session_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.session_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button generate_Button;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private Telerik.WinControls.UI.RadGridView session_GridView;
        private System.Windows.Forms.TextBox template_TextBox;
        private System.Windows.Forms.Button browse_Button;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
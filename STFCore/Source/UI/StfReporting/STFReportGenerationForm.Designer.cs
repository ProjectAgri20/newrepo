namespace HP.ScalableTest.UI.Reporting
{
    partial class STFReportGenerationForm
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
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn1 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewDateTimeColumn gridViewDateTimeColumn2 = new Telerik.WinControls.UI.GridViewDateTimeColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor2 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STFReportGenerationForm));
            this.generateReport_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.sessionSummary_radGridView = new Telerik.WinControls.UI.RadGridView();
            this.template_label = new System.Windows.Forms.Label();
            this.template_textBox = new System.Windows.Forms.TextBox();
            this.browse_button = new System.Windows.Forms.Button();
            this.selectSessions_label = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sessionSummary_radGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionSummary_radGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // generateReport_button
            // 
            this.generateReport_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generateReport_button.Location = new System.Drawing.Point(750, 632);
            this.generateReport_button.Name = "generateReport_button";
            this.generateReport_button.Size = new System.Drawing.Size(99, 23);
            this.generateReport_button.TabIndex = 1;
            this.generateReport_button.Text = "Generate Report";
            this.generateReport_button.UseVisualStyleBackColor = true;
            this.generateReport_button.Click += new System.EventHandler(this.generateReport_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Location = new System.Drawing.Point(855, 632);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(99, 23);
            this.cancel_button.TabIndex = 2;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // sessionSummary_radGridView
            // 
            this.sessionSummary_radGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionSummary_radGridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.sessionSummary_radGridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sessionSummary_radGridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sessionSummary_radGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sessionSummary_radGridView.Location = new System.Drawing.Point(15, 80);
            // 
            // 
            // 
            this.sessionSummary_radGridView.MasterTemplate.AllowAddNewRow = false;
            this.sessionSummary_radGridView.MasterTemplate.AllowDeleteRow = false;
            this.sessionSummary_radGridView.MasterTemplate.AllowEditRow = false;
            this.sessionSummary_radGridView.MasterTemplate.AutoGenerateColumns = false;
            this.sessionSummary_radGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "SessionId";
            gridViewTextBoxColumn1.HeaderText = "Session ID";
            gridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn1.MinWidth = 125;
            gridViewTextBoxColumn1.Name = "sessionId_column";
            gridViewTextBoxColumn1.Width = 125;
            gridViewTextBoxColumn2.FieldName = "SessionName";
            gridViewTextBoxColumn2.HeaderText = "Session Name";
            gridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn2.MinWidth = 250;
            gridViewTextBoxColumn2.Name = "sessionName_column";
            gridViewTextBoxColumn2.Width = 250;
            gridViewTextBoxColumn2.WrapText = true;
            gridViewTextBoxColumn3.FieldName = "Owner";
            gridViewTextBoxColumn3.HeaderText = "Owner";
            gridViewTextBoxColumn3.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn3.MinWidth = 125;
            gridViewTextBoxColumn3.Name = "owner_column";
            gridViewTextBoxColumn3.Width = 125;
            gridViewDateTimeColumn1.FieldName = "StartDateTime";
            gridViewDateTimeColumn1.HeaderText = "Start Date / Time";
            gridViewDateTimeColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewDateTimeColumn1.MinWidth = 200;
            gridViewDateTimeColumn1.Name = "startDateTime_column";
            gridViewDateTimeColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Descending;
            gridViewDateTimeColumn1.Width = 200;
            gridViewDateTimeColumn2.FieldName = "EndDateTime";
            gridViewDateTimeColumn2.HeaderText = "End Date / Time";
            gridViewDateTimeColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewDateTimeColumn2.MinWidth = 200;
            gridViewDateTimeColumn2.Name = "endDateTime_column";
            gridViewDateTimeColumn2.Width = 200;
            gridViewTextBoxColumn4.FieldName = "Status";
            gridViewTextBoxColumn4.HeaderText = "Status";
            gridViewTextBoxColumn4.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn4.MinWidth = 125;
            gridViewTextBoxColumn4.Name = "status_column";
            gridViewTextBoxColumn4.Width = 125;
            gridViewTextBoxColumn5.FieldName = "Type";
            gridViewTextBoxColumn5.HeaderText = "Type";
            gridViewTextBoxColumn5.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn5.MinWidth = 150;
            gridViewTextBoxColumn5.Name = "type_column";
            gridViewTextBoxColumn5.Width = 150;
            gridViewTextBoxColumn6.FieldName = "Cycle";
            gridViewTextBoxColumn6.HeaderText = "Cycle";
            gridViewTextBoxColumn6.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn6.MinWidth = 125;
            gridViewTextBoxColumn6.Name = "cycle_column";
            gridViewTextBoxColumn6.Width = 125;
            gridViewDecimalColumn1.DecimalPlaces = 0;
            gridViewDecimalColumn1.FieldName = "Activities";
            gridViewDecimalColumn1.FormatString = "{0:N0}";
            gridViewDecimalColumn1.HeaderText = "Activities";
            gridViewDecimalColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewDecimalColumn1.MinWidth = 125;
            gridViewDecimalColumn1.Name = "activities_column";
            gridViewDecimalColumn1.ShowUpDownButtons = false;
            gridViewDecimalColumn1.ThousandsSeparator = true;
            gridViewDecimalColumn1.Width = 125;
            gridViewTextBoxColumn7.FieldName = "Tags";
            gridViewTextBoxColumn7.HeaderText = "Tags";
            gridViewTextBoxColumn7.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn7.MinWidth = 125;
            gridViewTextBoxColumn7.Name = "tags_column";
            gridViewTextBoxColumn7.Width = 125;
            this.sessionSummary_radGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewDateTimeColumn1,
            gridViewDateTimeColumn2,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewDecimalColumn1,
            gridViewTextBoxColumn7});
            this.sessionSummary_radGridView.MasterTemplate.EnableAlternatingRowColor = true;
            this.sessionSummary_radGridView.MasterTemplate.EnableFiltering = true;
            this.sessionSummary_radGridView.MasterTemplate.EnableGrouping = false;
            this.sessionSummary_radGridView.MasterTemplate.EnablePaging = true;
            this.sessionSummary_radGridView.MasterTemplate.MultiSelect = true;
            this.sessionSummary_radGridView.MasterTemplate.PageSize = 50;
            this.sessionSummary_radGridView.MasterTemplate.ShowRowHeaderColumn = false;
            sortDescriptor1.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor1.PropertyName = "startDateTime_column";
            sortDescriptor2.Direction = System.ComponentModel.ListSortDirection.Descending;
            sortDescriptor2.PropertyName = "startDate_column";
            this.sessionSummary_radGridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1,
            sortDescriptor2});
            this.sessionSummary_radGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.sessionSummary_radGridView.Name = "sessionSummary_radGridView";
            this.sessionSummary_radGridView.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.sessionSummary_radGridView.ReadOnly = true;
            // 
            // 
            // 
            this.sessionSummary_radGridView.RootElement.ControlBounds = new System.Drawing.Rectangle(15, 80, 240, 150);
            this.sessionSummary_radGridView.Size = new System.Drawing.Size(939, 546);
            this.sessionSummary_radGridView.TabIndex = 8;
            this.sessionSummary_radGridView.Text = "radGridView1";
            this.sessionSummary_radGridView.ThemeName = "ControlDefault";
            // 
            // template_label
            // 
            this.template_label.AutoSize = true;
            this.template_label.Location = new System.Drawing.Point(14, 28);
            this.template_label.Name = "template_label";
            this.template_label.Size = new System.Drawing.Size(86, 13);
            this.template_label.TabIndex = 9;
            this.template_label.Text = "Report Template";
            // 
            // template_textBox
            // 
            this.template_textBox.Location = new System.Drawing.Point(116, 25);
            this.template_textBox.Name = "template_textBox";
            this.template_textBox.ReadOnly = true;
            this.template_textBox.Size = new System.Drawing.Size(733, 20);
            this.template_textBox.TabIndex = 10;
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(855, 23);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(99, 23);
            this.browse_button.TabIndex = 11;
            this.browse_button.Text = "Browse...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // selectSessions_label
            // 
            this.selectSessions_label.AutoSize = true;
            this.selectSessions_label.Location = new System.Drawing.Point(14, 59);
            this.selectSessions_label.Name = "selectSessions_label";
            this.selectSessions_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.selectSessions_label.Size = new System.Drawing.Size(241, 13);
            this.selectSessions_label.TabIndex = 12;
            this.selectSessions_label.Text = "Select the session(s) whose data will be extracted";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx|All Files|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            this.errorProvider.RightToLeft = true;
            // 
            // STFReportGenerationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(966, 667);
            this.Controls.Add(this.selectSessions_label);
            this.Controls.Add(this.browse_button);
            this.Controls.Add(this.sessionSummary_radGridView);
            this.Controls.Add(this.template_textBox);
            this.Controls.Add(this.template_label);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.generateReport_button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "STFReportGenerationForm";
            this.ShowIcon = false;
            this.Text = "STF Report Generation Form";
            ((System.ComponentModel.ISupportInitialize)(this.sessionSummary_radGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionSummary_radGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generateReport_button;
        private System.Windows.Forms.Button cancel_button;
        private Telerik.WinControls.UI.RadGridView sessionSummary_radGridView;
        private System.Windows.Forms.Label template_label;
        private System.Windows.Forms.TextBox template_textBox;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.Label selectSessions_label;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
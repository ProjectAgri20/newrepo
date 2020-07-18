namespace HP.ScalableTest.UI.Charting
{
    partial class SeriesFilterForm
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
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.close_Button = new System.Windows.Forms.Button();
            this.substringFilter_label = new System.Windows.Forms.Label();
            this.add_Button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.filter_textBox = new System.Windows.Forms.TextBox();
            this.series_radGridView = new Telerik.WinControls.UI.RadGridView();
            this.hidden_radGridView = new Telerik.WinControls.UI.RadGridView();
            this.remove_button = new System.Windows.Forms.Button();
            this.disableAll_button = new System.Windows.Forms.Button();
            this.enabledAll_button = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.series_radGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.series_radGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidden_radGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidden_radGridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(534, 636);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(112, 23);
            this.close_Button.TabIndex = 1;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // substringFilter_label
            // 
            this.substringFilter_label.AutoSize = true;
            this.substringFilter_label.Location = new System.Drawing.Point(9, 25);
            this.substringFilter_label.Name = "substringFilter_label";
            this.substringFilter_label.Size = new System.Drawing.Size(128, 13);
            this.substringFilter_label.TabIndex = 3;
            this.substringFilter_label.Text = "Hide All Series Containing";
            // 
            // add_Button
            // 
            this.add_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add_Button.Location = new System.Drawing.Point(537, 20);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(108, 23);
            this.add_Button.TabIndex = 7;
            this.add_Button.Text = "Add Filter";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Hidden Series";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 279);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Filter By Name";
            // 
            // filter_textBox
            // 
            this.filter_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filter_textBox.Location = new System.Drawing.Point(143, 22);
            this.filter_textBox.Name = "filter_textBox";
            this.filter_textBox.Size = new System.Drawing.Size(388, 20);
            this.filter_textBox.TabIndex = 14;
            this.filter_textBox.Click += new System.EventHandler(this.filter_textBox_Click);
            // 
            // series_radGridView
            // 
            this.series_radGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.series_radGridView.BackColor = System.Drawing.SystemColors.Control;
            this.series_radGridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.series_radGridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.series_radGridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.series_radGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.series_radGridView.Location = new System.Drawing.Point(12, 303);
            // 
            // 
            // 
            this.series_radGridView.MasterTemplate.AllowAddNewRow = false;
            this.series_radGridView.MasterTemplate.AllowColumnReorder = false;
            this.series_radGridView.MasterTemplate.AllowDeleteRow = false;
            this.series_radGridView.MasterTemplate.AllowDragToGroup = false;
            this.series_radGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewCheckBoxColumn1.AllowFiltering = false;
            gridViewCheckBoxColumn1.EnableExpressionEditor = false;
            gridViewCheckBoxColumn1.FieldName = "Enabled";
            gridViewCheckBoxColumn1.HeaderText = "Enabled";
            gridViewCheckBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewCheckBoxColumn1.MaxWidth = 100;
            gridViewCheckBoxColumn1.MinWidth = 20;
            gridViewCheckBoxColumn1.Name = "enabled_column";
            gridViewCheckBoxColumn1.Width = 93;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "Key";
            gridViewTextBoxColumn1.HeaderText = "Series";
            gridViewTextBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn1.Name = "series_column";
            gridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn1.Width = 520;
            this.series_radGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn1,
            gridViewTextBoxColumn1});
            this.series_radGridView.MasterTemplate.EnableFiltering = true;
            this.series_radGridView.MasterTemplate.EnableGrouping = false;
            this.series_radGridView.MasterTemplate.MultiSelect = true;
            sortDescriptor1.PropertyName = "series_column";
            this.series_radGridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.series_radGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.series_radGridView.Name = "series_radGridView";
            this.series_radGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.series_radGridView.ShowGroupPanel = false;
            this.series_radGridView.Size = new System.Drawing.Size(633, 327);
            this.series_radGridView.TabIndex = 15;
            this.series_radGridView.Text = "series_radGridView";
            // 
            // hidden_radGridView
            // 
            this.hidden_radGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hidden_radGridView.BackColor = System.Drawing.SystemColors.Control;
            this.hidden_radGridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.hidden_radGridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.hidden_radGridView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.hidden_radGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hidden_radGridView.Location = new System.Drawing.Point(12, 92);
            // 
            // 
            // 
            this.hidden_radGridView.MasterTemplate.AllowAddNewRow = false;
            this.hidden_radGridView.MasterTemplate.AllowDeleteRow = false;
            this.hidden_radGridView.MasterTemplate.AllowEditRow = false;
            this.hidden_radGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "Substring";
            gridViewTextBoxColumn2.HeaderText = "Word";
            gridViewTextBoxColumn2.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gridViewTextBoxColumn2.Name = "word_column";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.Width = 612;
            this.hidden_radGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn2});
            this.hidden_radGridView.MasterTemplate.EnableGrouping = false;
            this.hidden_radGridView.MasterTemplate.MultiSelect = true;
            this.hidden_radGridView.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.hidden_radGridView.Name = "hidden_radGridView";
            this.hidden_radGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.hidden_radGridView.ShowGroupPanel = false;
            this.hidden_radGridView.Size = new System.Drawing.Size(633, 149);
            this.hidden_radGridView.TabIndex = 16;
            this.hidden_radGridView.Text = "radGridView1";
            // 
            // remove_button
            // 
            this.remove_button.Location = new System.Drawing.Point(537, 63);
            this.remove_button.Name = "remove_button";
            this.remove_button.Size = new System.Drawing.Size(109, 23);
            this.remove_button.TabIndex = 17;
            this.remove_button.Text = "Remove Selected";
            this.remove_button.UseVisualStyleBackColor = true;
            this.remove_button.Click += new System.EventHandler(this.remove_button_Click);
            // 
            // disableAll_button
            // 
            this.disableAll_button.Location = new System.Drawing.Point(538, 274);
            this.disableAll_button.Name = "disableAll_button";
            this.disableAll_button.Size = new System.Drawing.Size(108, 23);
            this.disableAll_button.TabIndex = 18;
            this.disableAll_button.Text = "Disable All";
            this.disableAll_button.UseVisualStyleBackColor = true;
            this.disableAll_button.Click += new System.EventHandler(this.disableAll_button_Click);
            // 
            // enabledAll_button
            // 
            this.enabledAll_button.Location = new System.Drawing.Point(424, 274);
            this.enabledAll_button.Name = "enabledAll_button";
            this.enabledAll_button.Size = new System.Drawing.Size(108, 23);
            this.enabledAll_button.TabIndex = 19;
            this.enabledAll_button.Text = "Enable All";
            this.toolTip1.SetToolTip(this.enabledAll_button, "This will not enable data that has been filtered.");
            this.enabledAll_button.UseVisualStyleBackColor = true;
            this.enabledAll_button.Click += new System.EventHandler(this.enabledAll_button_Click);
            // 
            // SeriesFilterForm
            // 
            this.AcceptButton = this.add_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(658, 671);
            this.ControlBox = false;
            this.Controls.Add(this.enabledAll_button);
            this.Controls.Add(this.disableAll_button);
            this.Controls.Add(this.remove_button);
            this.Controls.Add(this.hidden_radGridView);
            this.Controls.Add(this.series_radGridView);
            this.Controls.Add(this.filter_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.substringFilter_label);
            this.Controls.Add(this.close_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SeriesFilterForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Series Filter";
            ((System.ComponentModel.ISupportInitialize)(this.series_radGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.series_radGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidden_radGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidden_radGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.Label substringFilter_label;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox filter_textBox;
        private Telerik.WinControls.UI.RadGridView series_radGridView;
        private Telerik.WinControls.UI.RadGridView hidden_radGridView;
        private System.Windows.Forms.Button remove_button;
        private System.Windows.Forms.Button disableAll_button;
        private System.Windows.Forms.Button enabledAll_button;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
namespace HP.ScalableTest.Print.Utility
{
    partial class RegistryInformationForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ok_Button = new System.Windows.Forms.Button();
            this.registryStartSize_Label = new System.Windows.Forms.Label();
            this.registryEndSize_Label = new System.Windows.Forms.Label();
            this.registryKey_DataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registryKeySnapshotDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.registryPath_DataGridView = new System.Windows.Forms.DataGridView();
            this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.registryPathSnapshotDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGrid_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.noData_Label = new System.Windows.Forms.Label();
            this.save_Button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.registryStartSizeValue_Label = new System.Windows.Forms.TextBox();
            this.registryEndSizeValue_Label = new System.Windows.Forms.TextBox();
            this.registrySizeChangeValue_Label = new System.Windows.Forms.TextBox();
            this.refresh_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.registryKey_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.registryKeySnapshotDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.registryPath_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.registryPathSnapshotDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_SplitContainer)).BeginInit();
            this.dataGrid_SplitContainer.Panel1.SuspendLayout();
            this.dataGrid_SplitContainer.Panel2.SuspendLayout();
            this.dataGrid_SplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(1133, 420);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 0;
            this.ok_Button.Text = "Ok";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // registryStartSize_Label
            // 
            this.registryStartSize_Label.AutoSize = true;
            this.registryStartSize_Label.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.registryStartSize_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registryStartSize_Label.Location = new System.Drawing.Point(2, 2);
            this.registryStartSize_Label.Margin = new System.Windows.Forms.Padding(0);
            this.registryStartSize_Label.Name = "registryStartSize_Label";
            this.registryStartSize_Label.Size = new System.Drawing.Size(32, 21);
            this.registryStartSize_Label.TabIndex = 1;
            this.registryStartSize_Label.Text = "Start:";
            this.registryStartSize_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // registryEndSize_Label
            // 
            this.registryEndSize_Label.AutoSize = true;
            this.registryEndSize_Label.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.registryEndSize_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registryEndSize_Label.Location = new System.Drawing.Point(114, 2);
            this.registryEndSize_Label.Margin = new System.Windows.Forms.Padding(0);
            this.registryEndSize_Label.Name = "registryEndSize_Label";
            this.registryEndSize_Label.Size = new System.Drawing.Size(29, 21);
            this.registryEndSize_Label.TabIndex = 2;
            this.registryEndSize_Label.Text = "End:";
            this.registryEndSize_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // registryKey_DataGridView
            // 
            this.registryKey_DataGridView.AllowUserToAddRows = false;
            this.registryKey_DataGridView.AllowUserToDeleteRows = false;
            this.registryKey_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.registryKey_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.registryKey_DataGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.471698F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.registryKey_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.registryKey_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.registryKey_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.registryKey_DataGridView.DataSource = this.registryKeySnapshotDataBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.471698F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.registryKey_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.registryKey_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registryKey_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.registryKey_DataGridView.Name = "registryKey_DataGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.471698F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.registryKey_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.registryKey_DataGridView.RowHeadersVisible = false;
            this.registryKey_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.registryKey_DataGridView.Size = new System.Drawing.Size(380, 347);
            this.registryKey_DataGridView.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn3.HeaderText = "Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 59;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Kind";
            this.dataGridViewTextBoxColumn4.HeaderText = "Kind";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 53;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn5.HeaderText = "Value";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // registryKeySnapshotDataBindingSource
            // 
            this.registryKeySnapshotDataBindingSource.DataSource = typeof(HP.ScalableTest.Print.Utility.RegistryKeySnapshotData);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(223, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 21);
            this.label1.TabIndex = 10;
            this.label1.Text = "Change:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // registryPath_DataGridView
            // 
            this.registryPath_DataGridView.AllowUserToAddRows = false;
            this.registryPath_DataGridView.AllowUserToDeleteRows = false;
            this.registryPath_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.registryPath_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.registryPath_DataGridView.AutoGenerateColumns = false;
            this.registryPath_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.registryPath_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pathDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn6});
            this.registryPath_DataGridView.DataSource = this.registryPathSnapshotDataBindingSource;
            this.registryPath_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registryPath_DataGridView.Location = new System.Drawing.Point(0, 0);
            this.registryPath_DataGridView.MultiSelect = false;
            this.registryPath_DataGridView.Name = "registryPath_DataGridView";
            this.registryPath_DataGridView.ReadOnly = true;
            this.registryPath_DataGridView.RowHeadersVisible = false;
            this.registryPath_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.registryPath_DataGridView.Size = new System.Drawing.Size(812, 347);
            this.registryPath_DataGridView.TabIndex = 11;
            this.registryPath_DataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.registryPath_DataGridView_CellClick);
            this.registryPath_DataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.registryPath_DataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.registryPath_DataGridView_CellEnter);
            // 
            // pathDataGridViewTextBoxColumn
            // 
            this.pathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            this.pathDataGridViewTextBoxColumn.HeaderText = "Registry Path";
            this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            this.pathDataGridViewTextBoxColumn.ReadOnly = true;
            this.pathDataGridViewTextBoxColumn.Width = 95;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "State";
            this.dataGridViewTextBoxColumn6.HeaderText = "State";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // registryPathSnapshotDataBindingSource
            // 
            this.registryPathSnapshotDataBindingSource.DataSource = typeof(HP.ScalableTest.Print.Utility.RegistryPathSnapshotData);
            // 
            // dataGrid_SplitContainer
            // 
            this.dataGrid_SplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid_SplitContainer.Location = new System.Drawing.Point(12, 67);
            this.dataGrid_SplitContainer.Name = "dataGrid_SplitContainer";
            // 
            // dataGrid_SplitContainer.Panel1
            // 
            this.dataGrid_SplitContainer.Panel1.Controls.Add(this.noData_Label);
            this.dataGrid_SplitContainer.Panel1.Controls.Add(this.registryPath_DataGridView);
            // 
            // dataGrid_SplitContainer.Panel2
            // 
            this.dataGrid_SplitContainer.Panel2.Controls.Add(this.registryKey_DataGridView);
            this.dataGrid_SplitContainer.Size = new System.Drawing.Size(1196, 347);
            this.dataGrid_SplitContainer.SplitterDistance = 812;
            this.dataGrid_SplitContainer.TabIndex = 12;
            // 
            // noData_Label
            // 
            this.noData_Label.AutoSize = true;
            this.noData_Label.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.noData_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noData_Label.ForeColor = System.Drawing.Color.MidnightBlue;
            this.noData_Label.Location = new System.Drawing.Point(100, 187);
            this.noData_Label.Name = "noData_Label";
            this.noData_Label.Size = new System.Drawing.Size(590, 26);
            this.noData_Label.TabIndex = 14;
            this.noData_Label.Text = "Too many queues on this system to display Registry details.";
            // 
            // save_Button
            // 
            this.save_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.save_Button.Location = new System.Drawing.Point(12, 420);
            this.save_Button.Name = "save_Button";
            this.save_Button.Size = new System.Drawing.Size(75, 23);
            this.save_Button.TabIndex = 13;
            this.save_Button.Text = "Save";
            this.save_Button.UseVisualStyleBackColor = true;
            this.save_Button.Click += new System.EventHandler(this.save_Button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Registry Size Change (bytes)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Registry Content Change";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "(Added or removed Registry keys)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // registryStartSizeValue_Label
            // 
            this.registryStartSizeValue_Label.BackColor = System.Drawing.Color.White;
            this.registryStartSizeValue_Label.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.registryStartSizeValue_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registryStartSizeValue_Label.Location = new System.Drawing.Point(39, 5);
            this.registryStartSizeValue_Label.Name = "registryStartSizeValue_Label";
            this.registryStartSizeValue_Label.Size = new System.Drawing.Size(70, 13);
            this.registryStartSizeValue_Label.TabIndex = 17;
            this.registryStartSizeValue_Label.TextChanged += new System.EventHandler(this.registryStartSizeValue_Label_TextChanged);
            // 
            // registryEndSizeValue_Label
            // 
            this.registryEndSizeValue_Label.BackColor = System.Drawing.Color.White;
            this.registryEndSizeValue_Label.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.registryEndSizeValue_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registryEndSizeValue_Label.Location = new System.Drawing.Point(148, 5);
            this.registryEndSizeValue_Label.Name = "registryEndSizeValue_Label";
            this.registryEndSizeValue_Label.Size = new System.Drawing.Size(70, 13);
            this.registryEndSizeValue_Label.TabIndex = 18;
            this.registryEndSizeValue_Label.TextChanged += new System.EventHandler(this.registryEndSizeValue_Label_TextChanged);
            // 
            // registrySizeChangeValue_Label
            // 
            this.registrySizeChangeValue_Label.BackColor = System.Drawing.Color.White;
            this.registrySizeChangeValue_Label.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.registrySizeChangeValue_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registrySizeChangeValue_Label.Location = new System.Drawing.Point(275, 5);
            this.registrySizeChangeValue_Label.Name = "registrySizeChangeValue_Label";
            this.registrySizeChangeValue_Label.Size = new System.Drawing.Size(74, 13);
            this.registrySizeChangeValue_Label.TabIndex = 19;
            // 
            // refresh_Button
            // 
            this.refresh_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refresh_Button.Location = new System.Drawing.Point(1052, 420);
            this.refresh_Button.Name = "refresh_Button";
            this.refresh_Button.Size = new System.Drawing.Size(75, 23);
            this.refresh_Button.TabIndex = 20;
            this.refresh_Button.Text = "Refresh";
            this.refresh_Button.UseVisualStyleBackColor = true;
            this.refresh_Button.Click += new System.EventHandler(this.refresh_Button_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.registryStartSize_Label, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.registryStartSizeValue_Label, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.registrySizeChangeValue_Label, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.registryEndSize_Label, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.registryEndSizeValue_Label, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 4, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(160, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(354, 25);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // RegistryInformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 455);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.refresh_Button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.save_Button);
            this.Controls.Add(this.dataGrid_SplitContainer);
            this.Controls.Add(this.ok_Button);
            this.Name = "RegistryInformationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registry Changes";
            this.Load += new System.EventHandler(this.RegistryInformation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.registryKey_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.registryKeySnapshotDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.registryPath_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.registryPathSnapshotDataBindingSource)).EndInit();
            this.dataGrid_SplitContainer.Panel1.ResumeLayout(false);
            this.dataGrid_SplitContainer.Panel1.PerformLayout();
            this.dataGrid_SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_SplitContainer)).EndInit();
            this.dataGrid_SplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label registryStartSize_Label;
        private System.Windows.Forms.Label registryEndSize_Label;
        private System.Windows.Forms.DataGridView registryKey_DataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView registryPath_DataGridView;
        private System.Windows.Forms.BindingSource registryKeySnapshotDataBindingSource;
        private System.Windows.Forms.BindingSource registryPathSnapshotDataBindingSource;
        private System.Windows.Forms.SplitContainer dataGrid_SplitContainer;
        private System.Windows.Forms.Button save_Button;
        private System.Windows.Forms.Label noData_Label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox registryStartSizeValue_Label;
        private System.Windows.Forms.TextBox registryEndSizeValue_Label;
        private System.Windows.Forms.TextBox registrySizeChangeValue_Label;
        private System.Windows.Forms.Button refresh_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
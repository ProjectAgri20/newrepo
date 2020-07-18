namespace HP.ScalableTest.LabConsole
{
    partial class InstallerPackagesForm
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
            if (disposing && _dataContext != null)
            {
                _dataContext.Dispose();
                _dataContext = null;
            }

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerPackagesForm));
            this.apply_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.resourceType_Label = new System.Windows.Forms.Label();
            this.packages_Label = new System.Windows.Forms.Label();
            this.installers_Label = new System.Windows.Forms.Label();
            this.packages_DataGridView = new System.Windows.Forms.DataGridView();
            this.packageName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resourceTypes_DataGridView = new System.Windows.Forms.DataGridView();
            this.resourceTypeSelected_Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.resourceTypeNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.metadataType_Label = new System.Windows.Forms.Label();
            this.metadataTypes_DataGridView = new System.Windows.Forms.DataGridView();
            this.metadataTypeSelected_Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.metadataTypeName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.metadataContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportSoftwarePackagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importButton = new System.Windows.Forms.Button();
            this.softwareInstallerSelectionControl = new HP.ScalableTest.LabConsole.SoftwareInstallerSelectionControl();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.packages_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourceTypes_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadataTypes_DataGridView)).BeginInit();
            this.metadataContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(795, 458);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 24);
            this.apply_Button.TabIndex = 20;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 208F));
            this.tableLayoutPanel.Controls.Add(this.resourceType_Label, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.packages_Label, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.installers_Label, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.packages_DataGridView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.resourceTypes_DataGridView, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.metadataType_Label, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.softwareInstallerSelectionControl, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.metadataTypes_DataGridView, 2, 3);
            this.tableLayoutPanel.Location = new System.Drawing.Point(4, 8);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(873, 444);
            this.tableLayoutPanel.TabIndex = 18;
            // 
            // resourceType_Label
            // 
            this.resourceType_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resourceType_Label.AutoSize = true;
            this.resourceType_Label.Location = new System.Drawing.Point(668, 7);
            this.resourceType_Label.Name = "resourceType_Label";
            this.resourceType_Label.Size = new System.Drawing.Size(140, 13);
            this.resourceType_Label.TabIndex = 15;
            this.resourceType_Label.Text = "Associated Resource Types";
            // 
            // packages_Label
            // 
            this.packages_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.packages_Label.AutoSize = true;
            this.packages_Label.Location = new System.Drawing.Point(3, 7);
            this.packages_Label.Name = "packages_Label";
            this.packages_Label.Size = new System.Drawing.Size(94, 13);
            this.packages_Label.TabIndex = 9;
            this.packages_Label.Text = "Installer Packages";
            // 
            // installers_Label
            // 
            this.installers_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.installers_Label.AutoSize = true;
            this.installers_Label.Location = new System.Drawing.Point(269, 7);
            this.installers_Label.Name = "installers_Label";
            this.installers_Label.Size = new System.Drawing.Size(93, 13);
            this.installers_Label.TabIndex = 10;
            this.installers_Label.Text = "Software Installers";
            // 
            // packages_DataGridView
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.packages_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.packages_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.packages_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.packages_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.packages_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.packageName_Column});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.packages_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.packages_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packages_DataGridView.Location = new System.Drawing.Point(3, 23);
            this.packages_DataGridView.MultiSelect = false;
            this.packages_DataGridView.Name = "packages_DataGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.packages_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.packages_DataGridView.RowHeadersWidth = 24;
            this.tableLayoutPanel.SetRowSpan(this.packages_DataGridView, 3);
            this.packages_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.packages_DataGridView.Size = new System.Drawing.Size(260, 418);
            this.packages_DataGridView.TabIndex = 12;
            this.packages_DataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.packages_DataGridView_CellEndEdit);
            this.packages_DataGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.packages_DataGridView_CellLeave);
            this.packages_DataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.packages_DataGridView_CellValidating);
            this.packages_DataGridView.SelectionChanged += new System.EventHandler(this.packages_DataGridView_SelectionChanged);
            this.packages_DataGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.packages_DataGridView_UserDeletingRow);
            this.packages_DataGridView.Leave += new System.EventHandler(this.packages_DataGridView_Leave);
            // 
            // packageName_Column
            // 
            this.packageName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.packageName_Column.DataPropertyName = "Description";
            this.packageName_Column.HeaderText = "Package Name";
            this.packageName_Column.Name = "packageName_Column";
            // 
            // resourceTypes_DataGridView
            // 
            this.resourceTypes_DataGridView.AllowUserToAddRows = false;
            this.resourceTypes_DataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.resourceTypes_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.resourceTypes_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.resourceTypes_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.resourceTypes_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resourceTypes_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resourceTypeSelected_Column,
            this.resourceTypeNameColumn});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.resourceTypes_DataGridView.DefaultCellStyle = dataGridViewCellStyle7;
            this.resourceTypes_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceTypes_DataGridView.Location = new System.Drawing.Point(668, 23);
            this.resourceTypes_DataGridView.Name = "resourceTypes_DataGridView";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.resourceTypes_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.resourceTypes_DataGridView.RowHeadersVisible = false;
            this.resourceTypes_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resourceTypes_DataGridView.Size = new System.Drawing.Size(202, 196);
            this.resourceTypes_DataGridView.TabIndex = 13;
            this.resourceTypes_DataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resourceTypes_DataGridView_CellContentClick);
            // 
            // resourceTypeSelected_Column
            // 
            this.resourceTypeSelected_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.resourceTypeSelected_Column.DataPropertyName = "Selected";
            this.resourceTypeSelected_Column.HeaderText = "";
            this.resourceTypeSelected_Column.Name = "resourceTypeSelected_Column";
            this.resourceTypeSelected_Column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.resourceTypeSelected_Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.resourceTypeSelected_Column.Width = 19;
            // 
            // resourceTypeNameColumn
            // 
            this.resourceTypeNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resourceTypeNameColumn.DataPropertyName = "Name";
            this.resourceTypeNameColumn.HeaderText = "Name";
            this.resourceTypeNameColumn.Name = "resourceTypeNameColumn";
            // 
            // metadataType_Label
            // 
            this.metadataType_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.metadataType_Label.AutoSize = true;
            this.metadataType_Label.Location = new System.Drawing.Point(668, 229);
            this.metadataType_Label.Name = "metadataType_Label";
            this.metadataType_Label.Size = new System.Drawing.Size(139, 13);
            this.metadataType_Label.TabIndex = 16;
            this.metadataType_Label.Text = "Associated Metadata Types";
            // 
            // metadataTypes_DataGridView
            // 
            this.metadataTypes_DataGridView.AllowUserToAddRows = false;
            this.metadataTypes_DataGridView.AllowUserToDeleteRows = false;
            this.metadataTypes_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.metadataTypes_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.metadataTypes_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metadataTypes_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.metadataTypes_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.metadataTypes_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.metadataTypeSelected_Column,
            this.metadataTypeName_Column});
            this.metadataTypes_DataGridView.ContextMenuStrip = this.metadataContextMenuStrip;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metadataTypes_DataGridView.DefaultCellStyle = dataGridViewCellStyle11;
            this.metadataTypes_DataGridView.Location = new System.Drawing.Point(668, 245);
            this.metadataTypes_DataGridView.Name = "metadataTypes_DataGridView";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metadataTypes_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.metadataTypes_DataGridView.RowHeadersVisible = false;
            this.metadataTypes_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metadataTypes_DataGridView.Size = new System.Drawing.Size(197, 189);
            this.metadataTypes_DataGridView.TabIndex = 17;
            this.metadataTypes_DataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.metadataTypes_DataGridView_CellContentClick);
            // 
            // metadataTypeSelected_Column
            // 
            this.metadataTypeSelected_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.metadataTypeSelected_Column.DataPropertyName = "Selected";
            this.metadataTypeSelected_Column.HeaderText = "";
            this.metadataTypeSelected_Column.Name = "metadataTypeSelected_Column";
            this.metadataTypeSelected_Column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.metadataTypeSelected_Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.metadataTypeSelected_Column.Width = 19;
            // 
            // metadataTypeName_Column
            // 
            this.metadataTypeName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.metadataTypeName_Column.DataPropertyName = "Name";
            this.metadataTypeName_Column.HeaderText = "Name";
            this.metadataTypeName_Column.Name = "metadataTypeName_Column";
            // 
            // metadataContextMenuStrip
            // 
            this.metadataContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.metadataContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSoftwarePackagesToolStripMenuItem});
            this.metadataContextMenuStrip.Name = "metadataContextMenuStrip";
            this.metadataContextMenuStrip.Size = new System.Drawing.Size(213, 30);
            // 
            // exportSoftwarePackagesToolStripMenuItem
            // 
            this.exportSoftwarePackagesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportSoftwarePackagesToolStripMenuItem.Image")));
            this.exportSoftwarePackagesToolStripMenuItem.Name = "exportSoftwarePackagesToolStripMenuItem";
            this.exportSoftwarePackagesToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.exportSoftwarePackagesToolStripMenuItem.Text = "Export Software Packages";
            this.exportSoftwarePackagesToolStripMenuItem.Click += new System.EventHandler(this.exportSoftwarePackagesToolStripMenuItem_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(633, 458);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 24);
            this.ok_Button.TabIndex = 17;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(714, 458);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 24);
            this.cancel_Button.TabIndex = 19;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Selected";
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "Selected";
            this.dataGridViewCheckBoxColumn2.HeaderText = "";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Installer Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "RebootMode";
            this.dataGridViewTextBoxColumn3.HeaderText = "Reboot";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "OrderNumber";
            this.dataGridViewTextBoxColumn4.HeaderText = "Order";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn5.HeaderText = "Package Name";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn3.DataPropertyName = "Selected";
            this.dataGridViewCheckBoxColumn3.HeaderText = "";
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            this.dataGridViewCheckBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn6.HeaderText = "Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.importButton.Location = new System.Drawing.Point(10, 458);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 24);
            this.importButton.TabIndex = 21;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // softwareInstallerSelectionControl
            // 
            this.softwareInstallerSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.softwareInstallerSelectionControl.Location = new System.Drawing.Point(270, 24);
            this.softwareInstallerSelectionControl.Margin = new System.Windows.Forms.Padding(4);
            this.softwareInstallerSelectionControl.Name = "softwareInstallerSelectionControl";
            this.tableLayoutPanel.SetRowSpan(this.softwareInstallerSelectionControl, 3);
            this.softwareInstallerSelectionControl.Size = new System.Drawing.Size(391, 416);
            this.softwareInstallerSelectionControl.TabIndex = 18;
            // 
            // InstallerPackagesForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(880, 493);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstallerPackagesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Software Installer Packages";
            this.Load += new System.EventHandler(this.InstallerPackagesForm_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.packages_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resourceTypes_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadataTypes_DataGridView)).EndInit();
            this.metadataContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label resourceType_Label;
        private System.Windows.Forms.Label packages_Label;
        private System.Windows.Forms.Label installers_Label;
        private System.Windows.Forms.DataGridView packages_DataGridView;
        private System.Windows.Forms.DataGridView resourceTypes_DataGridView;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label metadataType_Label;
        private System.Windows.Forms.DataGridView metadataTypes_DataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn metadataTypeSelected_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn metadataTypeName_Column;
        private System.Windows.Forms.DataGridViewCheckBoxColumn resourceTypeSelected_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn resourceTypeNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn packageName_Column;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private SoftwareInstallerSelectionControl softwareInstallerSelectionControl;
        private System.Windows.Forms.ContextMenuStrip metadataContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportSoftwarePackagesToolStripMenuItem;
        private System.Windows.Forms.Button importButton;
    }
}
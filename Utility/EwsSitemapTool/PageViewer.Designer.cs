namespace HP.ScalableTest.Tools
{
    partial class PageViewer
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
            this.page_Panel = new System.Windows.Forms.Panel();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deletePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.path_Label = new System.Windows.Forms.Label();
            this.path_TextBox = new System.Windows.Forms.TextBox();
            this.key_Label = new System.Windows.Forms.Label();
            this.page_Label = new System.Windows.Forms.Label();
            this.pageDetails_DataGridView = new System.Windows.Forms.DataGridView();
            this.siteMapsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.siteMapDataSet = new HP.ScalableTest.Tools.SiteMapDataSet();
            this.keys_ComboBox = new System.Windows.Forms.ComboBox();
            this.keyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Class = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.page_Panel.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageDetails_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteMapsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteMapDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // page_Panel
            // 
            this.page_Panel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.page_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.page_Panel.ContextMenuStrip = this.contextMenuStrip;
            this.page_Panel.Controls.Add(this.path_Label);
            this.page_Panel.Controls.Add(this.path_TextBox);
            this.page_Panel.Controls.Add(this.key_Label);
            this.page_Panel.Controls.Add(this.page_Label);
            this.page_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.page_Panel.Location = new System.Drawing.Point(0, 0);
            this.page_Panel.Name = "page_Panel";
            this.page_Panel.Size = new System.Drawing.Size(653, 26);
            this.page_Panel.TabIndex = 0;
            this.page_Panel.Click += new System.EventHandler(this.page_Panel_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deletePageToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(137, 26);
            // 
            // deletePageToolStripMenuItem
            // 
            this.deletePageToolStripMenuItem.Name = "deletePageToolStripMenuItem";
            this.deletePageToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.deletePageToolStripMenuItem.Text = "Delete Page";
            this.deletePageToolStripMenuItem.Click += new System.EventHandler(this.deletePageToolStripMenuItem_Click);
            // 
            // path_Label
            // 
            this.path_Label.AutoSize = true;
            this.path_Label.Location = new System.Drawing.Point(300, 6);
            this.path_Label.Name = "path_Label";
            this.path_Label.Size = new System.Drawing.Size(74, 13);
            this.path_Label.TabIndex = 4;
            this.path_Label.Text = "Ralative Path:";
            this.path_Label.Click += new System.EventHandler(this.page_Panel_Click);
            // 
            // path_TextBox
            // 
            this.path_TextBox.Location = new System.Drawing.Point(382, 2);
            this.path_TextBox.Name = "path_TextBox";
            this.path_TextBox.Size = new System.Drawing.Size(236, 20);
            this.path_TextBox.TabIndex = 3;
            // 
            // key_Label
            // 
            this.key_Label.AutoSize = true;
            this.key_Label.Location = new System.Drawing.Point(71, 5);
            this.key_Label.Name = "key_Label";
            this.key_Label.Size = new System.Drawing.Size(28, 13);
            this.key_Label.TabIndex = 2;
            this.key_Label.Text = "Key:";
            this.key_Label.Click += new System.EventHandler(this.page_Panel_Click);
            // 
            // page_Label
            // 
            this.page_Label.AutoSize = true;
            this.page_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.page_Label.Location = new System.Drawing.Point(4, 5);
            this.page_Label.Name = "page_Label";
            this.page_Label.Size = new System.Drawing.Size(40, 13);
            this.page_Label.TabIndex = 0;
            this.page_Label.Text = "Page:";
            this.page_Label.Click += new System.EventHandler(this.page_Panel_Click);
            // 
            // pageDetails_DataGridView
            // 
            this.pageDetails_DataGridView.AutoGenerateColumns = false;
            this.pageDetails_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pageDetails_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn,
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.XPath,
            this.Class,
            this.typeDataGridViewTextBoxColumn});
            this.pageDetails_DataGridView.DataSource = this.siteMapsBindingSource;
            this.pageDetails_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageDetails_DataGridView.Location = new System.Drawing.Point(0, 26);
            this.pageDetails_DataGridView.Name = "pageDetails_DataGridView";
            this.pageDetails_DataGridView.Size = new System.Drawing.Size(653, 298);
            this.pageDetails_DataGridView.TabIndex = 1;
            // 
            // siteMapsBindingSource
            // 
            this.siteMapsBindingSource.DataMember = "SiteMaps";
            this.siteMapsBindingSource.DataSource = this.siteMapDataSet;
            // 
            // siteMapDataSet
            // 
            this.siteMapDataSet.DataSetName = "SiteMapDataSet";
            this.siteMapDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // keys_ComboBox
            // 
            this.keys_ComboBox.FormattingEnabled = true;
            this.keys_ComboBox.Location = new System.Drawing.Point(105, 2);
            this.keys_ComboBox.Name = "keys_ComboBox";
            this.keys_ComboBox.Size = new System.Drawing.Size(190, 21);
            this.keys_ComboBox.TabIndex = 2;
            // 
            // keyDataGridViewTextBoxColumn
            // 
            this.keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            this.keyDataGridViewTextBoxColumn.HeaderText = "Key";
            this.keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            this.keyDataGridViewTextBoxColumn.Width = 200;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.Width = 150;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 150;
            // 
            // XPath
            // 
            this.XPath.DataPropertyName = "XPath";
            this.XPath.HeaderText = "XPath";
            this.XPath.Name = "XPath";
            this.XPath.Width = 250;
            // 
            // Class
            // 
            this.Class.DataPropertyName = "Class";
            this.Class.HeaderText = "Class";
            this.Class.Name = "Class";
            this.Class.Width = 150;
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            this.typeDataGridViewTextBoxColumn.Width = 150;
            // 
            // PageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.keys_ComboBox);
            this.Controls.Add(this.pageDetails_DataGridView);
            this.Controls.Add(this.page_Panel);
            this.Name = "PageViewer";
            this.Size = new System.Drawing.Size(653, 324);
            this.page_Panel.ResumeLayout(false);
            this.page_Panel.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pageDetails_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteMapsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteMapDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel page_Panel;
        private System.Windows.Forms.Label page_Label;
        private System.Windows.Forms.Label key_Label;
        private System.Windows.Forms.Label path_Label;
        private System.Windows.Forms.TextBox path_TextBox;
        private System.Windows.Forms.DataGridView pageDetails_DataGridView;
        private SiteMapDataSet siteMapDataSet;
        private System.Windows.Forms.BindingSource siteMapsBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deletePageToolStripMenuItem;
        private System.Windows.Forms.ComboBox keys_ComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn XPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn Class;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
    }
}

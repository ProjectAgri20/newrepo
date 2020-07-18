namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ImportDocumentControl
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

                if (_form != null)
                {
                    _form.Dispose();
                }
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportDocumentControl));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.resolveDataGridView = new System.Windows.Forms.DataGridView();
            this.resolvedColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.oldNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resolveToolStrip = new System.Windows.Forms.ToolStrip();
            this.resolveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.resolvedImageList = new System.Windows.Forms.ImageList(this.components);
            this.resolveContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resolveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resolveDataGridView)).BeginInit();
            this.resolveToolStrip.SuspendLayout();
            this.resolveContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.resolveDataGridView);
            this.mainPanel.Controls.Add(this.resolveToolStrip);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(754, 369);
            this.mainPanel.TabIndex = 1;
            // 
            // resolveDataGridView
            // 
            this.resolveDataGridView.AllowUserToAddRows = false;
            this.resolveDataGridView.AllowUserToDeleteRows = false;
            this.resolveDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.resolveDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.resolveDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.resolveDataGridView.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.resolveDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.resolveDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resolveDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resolvedColumn,
            this.oldNameColumn,
            this.newNameColumn});
            this.resolveDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resolveDataGridView.Location = new System.Drawing.Point(0, 27);
            this.resolveDataGridView.Name = "resolveDataGridView";
            this.resolveDataGridView.ReadOnly = true;
            this.resolveDataGridView.RowHeadersVisible = false;
            this.resolveDataGridView.RowTemplate.Height = 28;
            this.resolveDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resolveDataGridView.Size = new System.Drawing.Size(754, 342);
            this.resolveDataGridView.TabIndex = 2;
            this.resolveDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resolveDataGridView_CellDoubleClick);
            this.resolveDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.resolveDataGridView_CellFormatting);
            // 
            // resolvedColumn
            // 
            this.resolvedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.resolvedColumn.HeaderText = "";
            this.resolvedColumn.Name = "resolvedColumn";
            this.resolvedColumn.ReadOnly = true;
            this.resolvedColumn.Width = 5;
            // 
            // oldNameColumn
            // 
            this.oldNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.oldNameColumn.DataPropertyName = "Original";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.oldNameColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.oldNameColumn.HeaderText = "Original";
            this.oldNameColumn.Name = "oldNameColumn";
            this.oldNameColumn.ReadOnly = true;
            this.oldNameColumn.Width = 87;
            // 
            // newNameColumn
            // 
            this.newNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.newNameColumn.DataPropertyName = "Replacement";
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.newNameColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.newNameColumn.HeaderText = "Replacement";
            this.newNameColumn.Name = "newNameColumn";
            this.newNameColumn.ReadOnly = true;
            // 
            // resolveToolStrip
            // 
            this.resolveToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.resolveToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.resolveToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolveToolStripButton});
            this.resolveToolStrip.Location = new System.Drawing.Point(0, 0);
            this.resolveToolStrip.Name = "resolveToolStrip";
            this.resolveToolStrip.Size = new System.Drawing.Size(754, 27);
            this.resolveToolStrip.TabIndex = 1;
            this.resolveToolStrip.Text = "toolStrip1";
            // 
            // resolveToolStripButton
            // 
            this.resolveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("resolveToolStripButton.Image")));
            this.resolveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resolveToolStripButton.Name = "resolveToolStripButton";
            this.resolveToolStripButton.Size = new System.Drawing.Size(84, 24);
            this.resolveToolStripButton.Text = "Resolve";
            this.resolveToolStripButton.Click += new System.EventHandler(this.resolveToolStripButton_Click);
            // 
            // resolvedImageList
            // 
            this.resolvedImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("resolvedImageList.ImageStream")));
            this.resolvedImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.resolvedImageList.Images.SetKeyName(0, "Resolved");
            this.resolvedImageList.Images.SetKeyName(1, "NotResolved");
            // 
            // resolveContextMenuStrip
            // 
            this.resolveContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.resolveContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolveToolStripMenuItem});
            this.resolveContextMenuStrip.Name = "resolveContextMenuStrip";
            this.resolveContextMenuStrip.Size = new System.Drawing.Size(136, 30);
            // 
            // resolveToolStripMenuItem
            // 
            this.resolveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resolveToolStripMenuItem.Image")));
            this.resolveToolStripMenuItem.Name = "resolveToolStripMenuItem";
            this.resolveToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.resolveToolStripMenuItem.Text = "Resolve";
            this.resolveToolStripMenuItem.Click += new System.EventHandler(this.resolveToolStripMenuItem_Click);
            // 
            // ImportDocumentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ImportDocumentControl";
            this.Size = new System.Drawing.Size(754, 369);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resolveDataGridView)).EndInit();
            this.resolveToolStrip.ResumeLayout(false);
            this.resolveToolStrip.PerformLayout();
            this.resolveContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.DataGridView resolveDataGridView;
        private System.Windows.Forms.ToolStripButton resolveToolStripButton;
        private System.Windows.Forms.ImageList resolvedImageList;
        private System.Windows.Forms.DataGridViewImageColumn resolvedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn newNameColumn;
        private System.Windows.Forms.ContextMenuStrip resolveContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem resolveToolStripMenuItem;
        private System.Windows.Forms.ToolStrip resolveToolStrip;

    }
}

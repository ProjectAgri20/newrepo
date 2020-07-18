namespace HP.ScalableTest.UI
{
    partial class BadgeListControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BadgeListControl));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.button_Add = new System.Windows.Forms.ToolStripButton();
            this.button_Edit = new System.Windows.Forms.ToolStripButton();
            this.button_Delete = new System.Windows.Forms.ToolStripButton();
            this.column_UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_BadgeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_BadgeBoxId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_UserName,
            this.column_Index,
            this.column_BadgeId,
            this.Column_BadgeBoxId});
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.Location = new System.Drawing.Point(0, 32);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 24;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(552, 255);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_Add,
            this.button_Edit,
            this.button_Delete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(552, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip";
            // 
            // button_Add
            // 
            this.button_Add.Image = ((System.Drawing.Image)(resources.GetObject("button_Add.Image")));
            this.button_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(49, 22);
            this.button_Add.Text = "Add";
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Image = ((System.Drawing.Image)(resources.GetObject("button_Edit.Image")));
            this.button_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(47, 22);
            this.button_Edit.Text = "Edit";
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Image = ((System.Drawing.Image)(resources.GetObject("button_Delete.Image")));
            this.button_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(60, 22);
            this.button_Delete.Text = "Delete";
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // column_UserName
            // 
            this.column_UserName.DataPropertyName = "Username";
            this.column_UserName.HeaderText = "Username";
            this.column_UserName.Name = "column_UserName";
            this.column_UserName.ReadOnly = true;
            // 
            // column_Index
            // 
            this.column_Index.DataPropertyName = "Index";
            this.column_Index.HeaderText = "Index";
            this.column_Index.Name = "column_Index";
            this.column_Index.ReadOnly = true;
            this.column_Index.Width = 50;
            // 
            // column_BadgeId
            // 
            this.column_BadgeId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_BadgeId.DataPropertyName = "BadgeId";
            this.column_BadgeId.HeaderText = "Badge Id";
            this.column_BadgeId.Name = "column_BadgeId";
            this.column_BadgeId.ReadOnly = true;
            // 
            // Column_BadgeBoxId
            // 
            this.Column_BadgeBoxId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_BadgeBoxId.DataPropertyName = "BadgeBoxId";
            this.Column_BadgeBoxId.HeaderText = "Badge Box";
            this.Column_BadgeBoxId.Name = "Column_BadgeBoxId";
            this.Column_BadgeBoxId.ReadOnly = true;
            // 
            // BadgeListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.dataGridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BadgeListControl";
            this.Size = new System.Drawing.Size(552, 287);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton button_Add;
        private System.Windows.Forms.ToolStripButton button_Edit;
        private System.Windows.Forms.ToolStripButton button_Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_BadgeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_BadgeBoxId;
    }
}

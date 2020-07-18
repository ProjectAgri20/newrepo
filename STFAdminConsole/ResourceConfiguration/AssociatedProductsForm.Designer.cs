namespace HP.ScalableTest.LabConsole
{
    partial class AssociatedProductsForm
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
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.products_DataGridView = new System.Windows.Forms.DataGridView();
            this.add_Button = new System.Windows.Forms.Button();
            this.delete_Button = new System.Windows.Forms.Button();
            this.VendorDataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductDataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchCriteriaDataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.products_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(520, 312);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 0;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(601, 312);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 1;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(664, 33);
            this.label1.TabIndex = 3;
            this.label1.Text = "If vendor or product name is changed, a new database entry will be added, but old" +
    " vendor/name will not be deleted.";
            // 
            // products_DataGridView
            // 
            this.products_DataGridView.AllowUserToAddRows = false;
            this.products_DataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.products_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.products_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.products_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.products_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VendorDataGridViewColumn,
            this.ProductDataGridViewColumn,
            this.MatchCriteriaDataGridViewColumn});
            this.products_DataGridView.Location = new System.Drawing.Point(15, 45);
            this.products_DataGridView.MultiSelect = false;
            this.products_DataGridView.Name = "products_DataGridView";
            this.products_DataGridView.Size = new System.Drawing.Size(661, 261);
            this.products_DataGridView.TabIndex = 4;
            // 
            // add_Button
            // 
            this.add_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.add_Button.Location = new System.Drawing.Point(15, 312);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(75, 23);
            this.add_Button.TabIndex = 5;
            this.add_Button.Text = "Add";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // delete_Button
            // 
            this.delete_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delete_Button.Location = new System.Drawing.Point(96, 312);
            this.delete_Button.Name = "delete_Button";
            this.delete_Button.Size = new System.Drawing.Size(75, 23);
            this.delete_Button.TabIndex = 5;
            this.delete_Button.Text = "Delete";
            this.delete_Button.UseVisualStyleBackColor = true;
            this.delete_Button.Click += new System.EventHandler(this.delete_Button_Click);
            // 
            // VendorDataGridViewColumn
            // 
            this.VendorDataGridViewColumn.DataPropertyName = "Vendor";
            this.VendorDataGridViewColumn.HeaderText = "Vendor";
            this.VendorDataGridViewColumn.Name = "VendorDataGridViewColumn";
            // 
            // ProductDataGridViewColumn
            // 
            this.ProductDataGridViewColumn.DataPropertyName = "Name";
            this.ProductDataGridViewColumn.HeaderText = "Name";
            this.ProductDataGridViewColumn.Name = "ProductDataGridViewColumn";
            this.ProductDataGridViewColumn.Width = 200;
            // 
            // MatchCriteriaDataGridViewColumn
            // 
            this.MatchCriteriaDataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MatchCriteriaDataGridViewColumn.HeaderText = "Match Criteria";
            this.MatchCriteriaDataGridViewColumn.Name = "MatchCriteriaDataGridViewColumn";
            // 
            // AssociatedProductsForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(688, 347);
            this.Controls.Add(this.delete_Button);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.products_DataGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Name = "AssociatedProductsForm";
            this.Text = "Products and Solutions";
            this.Load += new System.EventHandler(this.AssociatedProductsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.products_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView products_DataGridView;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button delete_Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn VendorDataGridViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductDataGridViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchCriteriaDataGridViewColumn;
    }
}
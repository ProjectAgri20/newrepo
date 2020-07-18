namespace HP.ScalableTest.Plugin.Email
{
    partial class ExchangeInboxControl
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
            this.inboxGridView = new System.Windows.Forms.DataGridView();
            this.refreshLabel = new System.Windows.Forms.Label();
            this.from_DataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subject_DataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.inboxGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // inboxGridView
            // 
            this.inboxGridView.AllowUserToAddRows = false;
            this.inboxGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.inboxGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.inboxGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inboxGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inboxGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.from_DataGridViewColumn,
            this.subject_DataGridViewColumn});
            this.inboxGridView.Location = new System.Drawing.Point(3, 18);
            this.inboxGridView.Name = "inboxGridView";
            this.inboxGridView.ReadOnly = true;
            this.inboxGridView.RowHeadersVisible = false;
            this.inboxGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inboxGridView.RowTemplate.Height = 22;
            this.inboxGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.inboxGridView.Size = new System.Drawing.Size(375, 218);
            this.inboxGridView.TabIndex = 0;
            // 
            // refreshLabel
            // 
            this.refreshLabel.AutoSize = true;
            this.refreshLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshLabel.ForeColor = System.Drawing.Color.Blue;
            this.refreshLabel.Location = new System.Drawing.Point(3, 2);
            this.refreshLabel.Name = "refreshLabel";
            this.refreshLabel.Size = new System.Drawing.Size(0, 13);
            this.refreshLabel.TabIndex = 1;
            // 
            // from_DataGridViewColumn
            // 
            this.from_DataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.from_DataGridViewColumn.DataPropertyName = "FromAddress";
            this.from_DataGridViewColumn.HeaderText = "From";
            this.from_DataGridViewColumn.Name = "from_DataGridViewColumn";
            this.from_DataGridViewColumn.ReadOnly = true;
            this.from_DataGridViewColumn.Width = 55;
            // 
            // subject_DataGridViewColumn
            // 
            this.subject_DataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.subject_DataGridViewColumn.DataPropertyName = "Subject";
            this.subject_DataGridViewColumn.HeaderText = "Subject";
            this.subject_DataGridViewColumn.Name = "subject_DataGridViewColumn";
            this.subject_DataGridViewColumn.ReadOnly = true;
            // 
            // ExchangeInboxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.refreshLabel);
            this.Controls.Add(this.inboxGridView);
            this.Name = "ExchangeInboxControl";
            this.Size = new System.Drawing.Size(381, 239);
            this.Load += new System.EventHandler(this.ExchangeInboxControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView inboxGridView;
        private System.Windows.Forms.Label refreshLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn from_DataGridViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subject_DataGridViewColumn;
    }
}

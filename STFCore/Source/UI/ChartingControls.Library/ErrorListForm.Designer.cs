namespace HP.ScalableTest.UI.Charting
{
    partial class ErrorListForm
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
            this.error_DataGridView = new System.Windows.Forms.DataGridView();
            this.count_DataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.error_DataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.close_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.error_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // error_DataGridView
            // 
            this.error_DataGridView.AllowUserToAddRows = false;
            this.error_DataGridView.AllowUserToDeleteRows = false;
            this.error_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.error_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.error_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.count_DataGridViewColumn,
            this.error_DataGridViewColumn});
            this.error_DataGridView.Location = new System.Drawing.Point(12, 12);
            this.error_DataGridView.Name = "error_DataGridView";
            this.error_DataGridView.ReadOnly = true;
            this.error_DataGridView.RowHeadersVisible = false;
            this.error_DataGridView.Size = new System.Drawing.Size(620, 319);
            this.error_DataGridView.TabIndex = 0;
            // 
            // count_DataGridViewColumn
            // 
            this.count_DataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.count_DataGridViewColumn.DataPropertyName = "Count";
            this.count_DataGridViewColumn.HeaderText = "Count";
            this.count_DataGridViewColumn.Name = "count_DataGridViewColumn";
            this.count_DataGridViewColumn.ReadOnly = true;
            this.count_DataGridViewColumn.Width = 60;
            // 
            // error_DataGridViewColumn
            // 
            this.error_DataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.error_DataGridViewColumn.DataPropertyName = "Error";
            this.error_DataGridViewColumn.HeaderText = "Error";
            this.error_DataGridViewColumn.Name = "error_DataGridViewColumn";
            this.error_DataGridViewColumn.ReadOnly = true;
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(557, 337);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 1;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            this.close_Button.Click += new System.EventHandler(this.close_Button_Click);
            // 
            // ErrorListForm
            // 
            this.AcceptButton = this.close_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(644, 372);
            this.ControlBox = false;
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.error_DataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ErrorListForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error List";
            ((System.ComponentModel.ISupportInitialize)(this.error_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView error_DataGridView;
        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn count_DataGridViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn error_DataGridViewColumn;
    }
}
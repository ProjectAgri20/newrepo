namespace HP.ScalableTest.Plugin.TopCat
{
    partial class TopCatExecutionControl
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
            this.results_dataGridView = new System.Windows.Forms.DataGridView();
            this.testName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.results_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // results_dataGridView
            // 
            this.results_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.results_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.testName,
            this.testStatus});
            this.results_dataGridView.Location = new System.Drawing.Point(18, 13);
            this.results_dataGridView.Name = "results_dataGridView";
            this.results_dataGridView.RowHeadersVisible = false;
            this.results_dataGridView.Size = new System.Drawing.Size(458, 253);
            this.results_dataGridView.TabIndex = 0;
            // 
            // testName
            // 
            this.testName.HeaderText = "Test Name";
            this.testName.Name = "testName";
            this.testName.ReadOnly = true;
            this.testName.Width = 200;
            // 
            // testStatus
            // 
            this.testStatus.HeaderText = "Test Status";
            this.testStatus.Name = "testStatus";
            this.testStatus.ReadOnly = true;
            this.testStatus.Width = 200;
            // 
            // TopCatExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.results_dataGridView);
            this.Name = "TopCatExecutionControl";
            this.Size = new System.Drawing.Size(488, 279);
            ((System.ComponentModel.ISupportInitialize)(this.results_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView results_dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn testName;
        private System.Windows.Forms.DataGridViewTextBoxColumn testStatus;
    }
}

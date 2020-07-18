namespace HP.ScalableTest.LabConsole
{
    partial class FrameworkServerTypeForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameworkServerTypeForm));
            this.dataGridView_Types = new System.Windows.Forms.DataGridView();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel_Name = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox_Name = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel_Descr = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox_Descr = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton_Add = new System.Windows.Forms.ToolStripButton();
            this.column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Descr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Types)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_Types
            // 
            this.dataGridView_Types.AllowUserToAddRows = false;
            this.dataGridView_Types.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            this.dataGridView_Types.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Types.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Types.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Types.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_Name,
            this.column_Descr});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Types.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_Types.Location = new System.Drawing.Point(4, 28);
            this.dataGridView_Types.Name = "dataGridView_Types";
            this.dataGridView_Types.RowHeadersWidth = 24;
            this.dataGridView_Types.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Types.Size = new System.Drawing.Size(413, 265);
            this.dataGridView_Types.TabIndex = 0;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(342, 300);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(261, 300);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 2;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel_Name,
            this.toolStripTextBox_Name,
            this.toolStripLabel_Descr,
            this.toolStripTextBox_Descr,
            this.toolStripButton_Add});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(421, 25);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel_Name
            // 
            this.toolStripLabel_Name.Name = "toolStripLabel_Name";
            this.toolStripLabel_Name.Size = new System.Drawing.Size(39, 22);
            this.toolStripLabel_Name.Text = "Name";
            // 
            // toolStripTextBox_Name
            // 
            this.toolStripTextBox_Name.Name = "toolStripTextBox_Name";
            this.toolStripTextBox_Name.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel_Descr
            // 
            this.toolStripLabel_Descr.Name = "toolStripLabel_Descr";
            this.toolStripLabel_Descr.Size = new System.Drawing.Size(67, 22);
            this.toolStripLabel_Descr.Text = "Description";
            // 
            // toolStripTextBox_Descr
            // 
            this.toolStripTextBox_Descr.Name = "toolStripTextBox_Descr";
            this.toolStripTextBox_Descr.Size = new System.Drawing.Size(125, 25);
            // 
            // toolStripButton_Add
            // 
            this.toolStripButton_Add.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Add.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Add.Image")));
            this.toolStripButton_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Add.Name = "toolStripButton_Add";
            this.toolStripButton_Add.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_Add.Text = "Add New";
            this.toolStripButton_Add.Click += new System.EventHandler(this.toolStripButton_Add_Click);
            // 
            // column_Name
            // 
            this.column_Name.DataPropertyName = "Name";
            this.column_Name.HeaderText = "Name";
            this.column_Name.Name = "column_Name";
            this.column_Name.ReadOnly = true;
            // 
            // column_Descr
            // 
            this.column_Descr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_Descr.DataPropertyName = "Description";
            this.column_Descr.HeaderText = "Description";
            this.column_Descr.Name = "column_Descr";
            this.column_Descr.ReadOnly = true;
            // 
            // FrameworkServerTypeForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(421, 326);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.dataGridView_Types);
            this.Name = "FrameworkServerTypeForm";
            this.Text = "Server Types";
            this.ResizeEnd += new System.EventHandler(this.FrameworkServerTypeForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Types)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Types;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_Name;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox_Name;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_Descr;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox_Descr;
        private System.Windows.Forms.ToolStripButton toolStripButton_Add;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Descr;
    }
}
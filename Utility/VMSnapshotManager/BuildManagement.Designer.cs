namespace VMSnapshotManager
{
    partial class BuildManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.serverFilter_ToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.dispatchers_DataGridView = new System.Windows.Forms.DataGridView();
            this.machineSelectedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hostNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Database = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Environment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browse_button = new System.Windows.Forms.Button();
            this.repo_textBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.deploy_button = new System.Windows.Forms.Button();
            this.log_textBox = new System.Windows.Forms.TextBox();
            this.log_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dispatchers_DataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 420F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dispatchers_DataGridView, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(22, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(424, 456);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.serverFilter_ToolStripComboBox});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(420, 25);
            this.toolStrip1.TabIndex = 20;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel1.Text = "Server Type";
            // 
            // serverFilter_ToolStripComboBox
            // 
            this.serverFilter_ToolStripComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.serverFilter_ToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverFilter_ToolStripComboBox.DropDownWidth = 100;
            this.serverFilter_ToolStripComboBox.Name = "serverFilter_ToolStripComboBox";
            this.serverFilter_ToolStripComboBox.Size = new System.Drawing.Size(300, 25);
            this.serverFilter_ToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.serverFilter_ToolStripComboBox_SelectedIndexChanged);
            // 
            // dispatchers_DataGridView
            // 
            this.dispatchers_DataGridView.AllowUserToAddRows = false;
            this.dispatchers_DataGridView.AllowUserToDeleteRows = false;
            this.dispatchers_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dispatchers_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dispatchers_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dispatchers_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dispatchers_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dispatchers_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.machineSelectedColumn,
            this.hostNameColumn,
            this.VersionColumn,
            this.Database,
            this.Environment});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dispatchers_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.dispatchers_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dispatchers_DataGridView.Location = new System.Drawing.Point(7, 28);
            this.dispatchers_DataGridView.Name = "dispatchers_DataGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dispatchers_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dispatchers_DataGridView.RowHeadersVisible = false;
            this.dispatchers_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dispatchers_DataGridView.Size = new System.Drawing.Size(414, 478);
            this.dispatchers_DataGridView.TabIndex = 14;
            // 
            // machineSelectedColumn
            // 
            this.machineSelectedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.machineSelectedColumn.DataPropertyName = "Selected";
            this.machineSelectedColumn.HeaderText = "";
            this.machineSelectedColumn.Name = "machineSelectedColumn";
            this.machineSelectedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.machineSelectedColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.machineSelectedColumn.Width = 19;
            // 
            // hostNameColumn
            // 
            this.hostNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hostNameColumn.DataPropertyName = "HostName";
            this.hostNameColumn.HeaderText = "HostName";
            this.hostNameColumn.Name = "hostNameColumn";
            this.hostNameColumn.Width = 82;
            // 
            // VersionColumn
            // 
            this.VersionColumn.DataPropertyName = "ServiceVersion";
            this.VersionColumn.HeaderText = "Version";
            this.VersionColumn.Name = "VersionColumn";
            // 
            // Database
            // 
            this.Database.DataPropertyName = "DatabaseHostName";
            this.Database.HeaderText = "Database";
            this.Database.Name = "Database";
            // 
            // Environment
            // 
            this.Environment.DataPropertyName = "Environment";
            this.Environment.HeaderText = "Environment";
            this.Environment.Name = "Environment";
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(309, 28);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(39, 23);
            this.browse_button.TabIndex = 15;
            this.browse_button.Text = "...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // repo_textBox
            // 
            this.repo_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.repo_textBox.Location = new System.Drawing.Point(6, 31);
            this.repo_textBox.Name = "repo_textBox";
            this.repo_textBox.ReadOnly = true;
            this.repo_textBox.Size = new System.Drawing.Size(297, 20);
            this.repo_textBox.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.deploy_button);
            this.groupBox1.Controls.Add(this.repo_textBox);
            this.groupBox1.Controls.Add(this.browse_button);
            this.groupBox1.Location = new System.Drawing.Point(449, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 111);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Deployment";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Repo Path:";
            // 
            // deploy_button
            // 
            this.deploy_button.Location = new System.Drawing.Point(6, 57);
            this.deploy_button.Name = "deploy_button";
            this.deploy_button.Size = new System.Drawing.Size(60, 23);
            this.deploy_button.TabIndex = 17;
            this.deploy_button.Text = "Deploy";
            this.deploy_button.UseVisualStyleBackColor = true;
            this.deploy_button.Click += new System.EventHandler(this.deploy_button_Click);
            // 
            // log_textBox
            // 
            this.log_textBox.Location = new System.Drawing.Point(455, 167);
            this.log_textBox.Multiline = true;
            this.log_textBox.Name = "log_textBox";
            this.log_textBox.Size = new System.Drawing.Size(342, 319);
            this.log_textBox.TabIndex = 18;
            // 
            // log_label
            // 
            this.log_label.AutoSize = true;
            this.log_label.Location = new System.Drawing.Point(455, 151);
            this.log_label.Name = "log_label";
            this.log_label.Size = new System.Drawing.Size(28, 13);
            this.log_label.TabIndex = 19;
            this.log_label.Text = "Log:";
            // 
            // BuildManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 505);
            this.Controls.Add(this.log_label);
            this.Controls.Add(this.log_textBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BuildManagement";
            this.Text = "BuildManagement";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BuildManagement_FormClosing);
            this.Load += new System.EventHandler(this.BuildManagement_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dispatchers_DataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox serverFilter_ToolStripComboBox;
        private System.Windows.Forms.DataGridView dispatchers_DataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn machineSelectedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Database;
        private System.Windows.Forms.DataGridViewTextBoxColumn Environment;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox repo_textBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button deploy_button;
        private System.Windows.Forms.TextBox log_textBox;
        private System.Windows.Forms.Label log_label;
    }
}
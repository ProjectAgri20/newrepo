namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ImportPlatformControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.resourceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.assignPlatformGridView = new System.Windows.Forms.DataGridView();
            this.platformComboBox = new System.Windows.Forms.ComboBox();
            this.machinePlatformLabel = new System.Windows.Forms.Label();
            this.applyToAllButton = new System.Windows.Forms.Button();
            this.virtualResourceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.platformColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.assignPlatformGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.label2.Location = new System.Drawing.Point(341, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(415, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "Set the Machine Platform for each Resource Type to continue.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(28, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Resource Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // resourceTypeComboBox
            // 
            this.resourceTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceTypeComboBox.FormattingEnabled = true;
            this.resourceTypeComboBox.Location = new System.Drawing.Point(147, 6);
            this.resourceTypeComboBox.Name = "resourceTypeComboBox";
            this.resourceTypeComboBox.Size = new System.Drawing.Size(188, 28);
            this.resourceTypeComboBox.TabIndex = 6;
            this.resourceTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.resourceTypeComboBox_SelectedIndexChanged);
            // 
            // assignPlatformGridView
            // 
            this.assignPlatformGridView.AllowUserToAddRows = false;
            this.assignPlatformGridView.AllowUserToDeleteRows = false;
            this.assignPlatformGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.assignPlatformGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.assignPlatformGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assignPlatformGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assignPlatformGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.virtualResourceColumn,
            this.platformColumn});
            this.assignPlatformGridView.Location = new System.Drawing.Point(0, 74);
            this.assignPlatformGridView.Name = "assignPlatformGridView";
            this.assignPlatformGridView.RowHeadersVisible = false;
            this.assignPlatformGridView.RowTemplate.Height = 28;
            this.assignPlatformGridView.Size = new System.Drawing.Size(846, 365);
            this.assignPlatformGridView.TabIndex = 5;
            this.assignPlatformGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.assignPlatformGridView_CellClick);
            this.assignPlatformGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.assignPlatformGridView_DataError);
            // 
            // platformComboBox
            // 
            this.platformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platformComboBox.FormattingEnabled = true;
            this.platformComboBox.Location = new System.Drawing.Point(147, 40);
            this.platformComboBox.Name = "platformComboBox";
            this.platformComboBox.Size = new System.Drawing.Size(590, 28);
            this.platformComboBox.TabIndex = 9;
            // 
            // machinePlatformLabel
            // 
            this.machinePlatformLabel.Location = new System.Drawing.Point(12, 43);
            this.machinePlatformLabel.Name = "machinePlatformLabel";
            this.machinePlatformLabel.Size = new System.Drawing.Size(129, 23);
            this.machinePlatformLabel.TabIndex = 10;
            this.machinePlatformLabel.Text = "Machine Platform";
            this.machinePlatformLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // applyToAllButton
            // 
            this.applyToAllButton.Location = new System.Drawing.Point(743, 37);
            this.applyToAllButton.Name = "applyToAllButton";
            this.applyToAllButton.Size = new System.Drawing.Size(100, 32);
            this.applyToAllButton.TabIndex = 11;
            this.applyToAllButton.Text = "Apply to All";
            this.applyToAllButton.UseVisualStyleBackColor = true;
            this.applyToAllButton.Click += new System.EventHandler(this.applyToAllButton_Click);
            // 
            // virtualResourceColumn
            // 
            this.virtualResourceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.virtualResourceColumn.DataPropertyName = "Name";
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.virtualResourceColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.virtualResourceColumn.HeaderText = "Machine Name";
            this.virtualResourceColumn.Name = "virtualResourceColumn";
            this.virtualResourceColumn.Width = 127;
            // 
            // platformColumn
            // 
            this.platformColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.platformColumn.DataPropertyName = "Platform";
            this.platformColumn.HeaderText = "Machine Platform";
            this.platformColumn.Name = "platformColumn";
            // 
            // ImportPlatformControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.applyToAllButton);
            this.Controls.Add(this.machinePlatformLabel);
            this.Controls.Add(this.platformComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resourceTypeComboBox);
            this.Controls.Add(this.assignPlatformGridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ImportPlatformControl";
            this.Size = new System.Drawing.Size(846, 439);
            ((System.ComponentModel.ISupportInitialize)(this.assignPlatformGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox resourceTypeComboBox;
        private System.Windows.Forms.DataGridView assignPlatformGridView;
        private System.Windows.Forms.ComboBox platformComboBox;
        private System.Windows.Forms.Label machinePlatformLabel;
        private System.Windows.Forms.Button applyToAllButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn virtualResourceColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn platformColumn;
    }
}

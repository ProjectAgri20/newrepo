namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class AssignPlatformDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssignPlatformDialog));
            this.assignPlatformGridView = new System.Windows.Forms.DataGridView();
            this.virtualResourceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.platformColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.okButton = new System.Windows.Forms.Button();
            this.resourceTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.assignPlatformGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // assignPlatformGridView
            // 
            this.assignPlatformGridView.AllowUserToAddRows = false;
            this.assignPlatformGridView.AllowUserToDeleteRows = false;
            this.assignPlatformGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.assignPlatformGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.assignPlatformGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assignPlatformGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assignPlatformGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.virtualResourceColumn,
            this.platformColumn});
            this.assignPlatformGridView.Location = new System.Drawing.Point(14, 58);
            this.assignPlatformGridView.Name = "assignPlatformGridView";
            this.assignPlatformGridView.RowHeadersVisible = false;
            this.assignPlatformGridView.RowTemplate.Height = 28;
            this.assignPlatformGridView.Size = new System.Drawing.Size(814, 303);
            this.assignPlatformGridView.TabIndex = 0;
            // 
            // virtualResourceColumn
            // 
            this.virtualResourceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.virtualResourceColumn.DataPropertyName = "Name";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.virtualResourceColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.virtualResourceColumn.HeaderText = "Resource Name";
            this.virtualResourceColumn.Name = "virtualResourceColumn";
            this.virtualResourceColumn.Width = 134;
            // 
            // platformColumn
            // 
            this.platformColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.platformColumn.DataPropertyName = "Platform";
            this.platformColumn.HeaderText = "Platform";
            this.platformColumn.Name = "platformColumn";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(622, 367);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 28);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // resourceTypeComboBox
            // 
            this.resourceTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resourceTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceTypeComboBox.FormattingEnabled = true;
            this.resourceTypeComboBox.Location = new System.Drawing.Point(573, 12);
            this.resourceTypeComboBox.Name = "resourceTypeComboBox";
            this.resourceTypeComboBox.Size = new System.Drawing.Size(255, 26);
            this.resourceTypeComboBox.TabIndex = 2;
            this.resourceTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.resourceTypeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(454, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Resource Type";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(434, 46);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select each Resource Type at the right and then update the Platform for any resou" +
    "rces of that type listed below.";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(728, 367);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 28);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // AssignPlatformDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 407);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resourceTypeComboBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.assignPlatformGridView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AssignPlatformDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assign Platform to Resource";
            this.Load += new System.EventHandler(this.AssignPlatformDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.assignPlatformGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView assignPlatformGridView;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox resourceTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn virtualResourceColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn platformColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cancelButton;
    }
}
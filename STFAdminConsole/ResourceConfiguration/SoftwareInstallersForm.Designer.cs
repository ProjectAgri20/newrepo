namespace HP.ScalableTest.LabConsole
{
    partial class SoftwareInstallersForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// Only clean up the data context if it was created by this form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _dataContext != null && _localContext)
            {
                _dataContext.Dispose();
                _dataContext = null;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoftwareInstallersForm));
            this.installers_DataGridView = new System.Windows.Forms.DataGridView();
            this.installerName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reboot_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.installers_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.add_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.delete_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.edit_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.installers_DataGridView)).BeginInit();
            this.installers_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // installers_DataGridView
            // 
            this.installers_DataGridView.AllowUserToAddRows = false;
            this.installers_DataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.installers_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.installers_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.installers_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.installers_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.installers_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.installers_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.installerName_Column,
            this.reboot_Column});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.installers_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.installers_DataGridView.Location = new System.Drawing.Point(0, 34);
            this.installers_DataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.installers_DataGridView.MultiSelect = false;
            this.installers_DataGridView.Name = "installers_DataGridView";
            this.installers_DataGridView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.installers_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.installers_DataGridView.RowHeadersVisible = false;
            this.installers_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.installers_DataGridView.Size = new System.Drawing.Size(585, 527);
            this.installers_DataGridView.TabIndex = 24;
            // 
            // installerName_Column
            // 
            this.installerName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.installerName_Column.DataPropertyName = "Description";
            this.installerName_Column.HeaderText = "Installer Name";
            this.installerName_Column.Name = "installerName_Column";
            this.installerName_Column.ReadOnly = true;
            // 
            // reboot_Column
            // 
            this.reboot_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.reboot_Column.DataPropertyName = "RebootMode";
            this.reboot_Column.HeaderText = "Reboot";
            this.reboot_Column.Name = "reboot_Column";
            this.reboot_Column.ReadOnly = true;
            this.reboot_Column.Width = 83;
            // 
            // installers_ToolStrip
            // 
            this.installers_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.installers_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add_ToolStripButton,
            this.edit_ToolStripButton,
            this.delete_ToolStripButton});
            this.installers_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.installers_ToolStrip.Name = "installers_ToolStrip";
            this.installers_ToolStrip.Size = new System.Drawing.Size(585, 27);
            this.installers_ToolStrip.TabIndex = 25;
            this.installers_ToolStrip.Text = "Selected Installers";
            // 
            // add_ToolStripButton
            // 
            this.add_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("add_ToolStripButton.Image")));
            this.add_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_ToolStripButton.Name = "add_ToolStripButton";
            this.add_ToolStripButton.Size = new System.Drawing.Size(61, 24);
            this.add_ToolStripButton.Text = "Add";
            this.add_ToolStripButton.ToolTipText = "New Installer";
            this.add_ToolStripButton.Click += new System.EventHandler(this.add_ToolStripButton_Click);
            // 
            // delete_ToolStripButton
            // 
            this.delete_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("delete_ToolStripButton.Image")));
            this.delete_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete_ToolStripButton.Name = "delete_ToolStripButton";
            this.delete_ToolStripButton.Size = new System.Drawing.Size(87, 24);
            this.delete_ToolStripButton.Text = "Remove";
            this.delete_ToolStripButton.Click += new System.EventHandler(this.delete_ToolStripButton_Click);
            // 
            // edit_ToolStripButton
            // 
            this.edit_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("edit_ToolStripButton.Image")));
            this.edit_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edit_ToolStripButton.Name = "edit_ToolStripButton";
            this.edit_ToolStripButton.Size = new System.Drawing.Size(59, 24);
            this.edit_ToolStripButton.Text = "Edit";
            this.edit_ToolStripButton.Click += new System.EventHandler(this.edit_ToolStripButton_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(372, 569);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 28);
            this.ok_Button.TabIndex = 26;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(480, 569);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 28);
            this.cancel_Button.TabIndex = 27;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // SoftwareInstallersForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(585, 601);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.installers_ToolStrip);
            this.Controls.Add(this.installers_DataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SoftwareInstallersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Software Installers";
            ((System.ComponentModel.ISupportInitialize)(this.installers_DataGridView)).EndInit();
            this.installers_ToolStrip.ResumeLayout(false);
            this.installers_ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView installers_DataGridView;
        private System.Windows.Forms.ToolStrip installers_ToolStrip;
        private System.Windows.Forms.ToolStripButton delete_ToolStripButton;
        private System.Windows.Forms.ToolStripButton add_ToolStripButton;
        private System.Windows.Forms.ToolStripButton edit_ToolStripButton;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn installerName_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn reboot_Column;
    }
}
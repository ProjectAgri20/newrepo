namespace HP.ScalableTest.LabConsole
{
    partial class SoftwareInstallerSelectionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoftwareInstallerSelectionControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.selectedInstallers_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.moveDown_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.moveUp_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.add_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.remove_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.installers_DataGridView = new System.Windows.Forms.DataGridView();
            this.installerName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reboot_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectedInstallers_ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installers_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // selectedInstallers_ToolStrip
            // 
            this.selectedInstallers_ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.selectedInstallers_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.selectedInstallers_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveDown_ToolStripButton,
            this.moveUp_ToolStripButton,
            this.add_ToolStripButton,
            this.remove_ToolStripButton});
            this.selectedInstallers_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.selectedInstallers_ToolStrip.Name = "selectedInstallers_ToolStrip";
            this.selectedInstallers_ToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.selectedInstallers_ToolStrip.Size = new System.Drawing.Size(553, 27);
            this.selectedInstallers_ToolStrip.TabIndex = 22;
            this.selectedInstallers_ToolStrip.Text = "Selected Installers";
            // 
            // moveDown_ToolStripButton
            // 
            this.moveDown_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.moveDown_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveDown_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("moveDown_ToolStripButton.Image")));
            this.moveDown_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveDown_ToolStripButton.Name = "moveDown_ToolStripButton";
            this.moveDown_ToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.moveDown_ToolStripButton.Text = "Move Down";
            this.moveDown_ToolStripButton.Click += new System.EventHandler(this.moveDown_ToolStripButton_Click);
            // 
            // moveUp_ToolStripButton
            // 
            this.moveUp_ToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.moveUp_ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveUp_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("moveUp_ToolStripButton.Image")));
            this.moveUp_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveUp_ToolStripButton.Name = "moveUp_ToolStripButton";
            this.moveUp_ToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.moveUp_ToolStripButton.Text = "Move Up";
            this.moveUp_ToolStripButton.Click += new System.EventHandler(this.moveUp_ToolStripButton_Click);
            // 
            // add_ToolStripButton
            // 
            this.add_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("add_ToolStripButton.Image")));
            this.add_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_ToolStripButton.Name = "add_ToolStripButton";
            this.add_ToolStripButton.Size = new System.Drawing.Size(117, 24);
            this.add_ToolStripButton.Text = "Add Installer";
            this.add_ToolStripButton.Click += new System.EventHandler(this.add_ToolStripButton_Click);
            // 
            // remove_ToolStripButton
            // 
            this.remove_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("remove_ToolStripButton.Image")));
            this.remove_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.remove_ToolStripButton.Name = "remove_ToolStripButton";
            this.remove_ToolStripButton.Size = new System.Drawing.Size(143, 24);
            this.remove_ToolStripButton.Text = "Remove Installer";
            this.remove_ToolStripButton.Click += new System.EventHandler(this.remove_ToolStripButton_Click);
            // 
            // installers_DataGridView
            // 
            this.installers_DataGridView.AllowUserToAddRows = false;
            this.installers_DataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.installers_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            this.installers_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.installers_DataGridView.Location = new System.Drawing.Point(0, 27);
            this.installers_DataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.installers_DataGridView.MultiSelect = false;
            this.installers_DataGridView.Name = "installers_DataGridView";
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
            this.installers_DataGridView.Size = new System.Drawing.Size(553, 367);
            this.installers_DataGridView.TabIndex = 23;
            // 
            // installerName_Column
            // 
            this.installerName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.installerName_Column.DataPropertyName = "Name";
            this.installerName_Column.HeaderText = "Installer Name";
            this.installerName_Column.Name = "installerName_Column";
            // 
            // reboot_Column
            // 
            this.reboot_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.reboot_Column.DataPropertyName = "RebootMode";
            this.reboot_Column.HeaderText = "Reboot";
            this.reboot_Column.Name = "reboot_Column";
            this.reboot_Column.Width = 83;
            // 
            // SoftwareInstallerSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.installers_DataGridView);
            this.Controls.Add(this.selectedInstallers_ToolStrip);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SoftwareInstallerSelectionControl";
            this.Size = new System.Drawing.Size(553, 394);
            this.selectedInstallers_ToolStrip.ResumeLayout(false);
            this.selectedInstallers_ToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installers_DataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip selectedInstallers_ToolStrip;
        private System.Windows.Forms.DataGridView installers_DataGridView;
        private System.Windows.Forms.ToolStripButton moveDown_ToolStripButton;
        private System.Windows.Forms.ToolStripButton moveUp_ToolStripButton;
        private System.Windows.Forms.ToolStripButton remove_ToolStripButton;
        private System.Windows.Forms.ToolStripButton add_ToolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn installerName_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn reboot_Column;
    }
}

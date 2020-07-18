namespace HP.ScalableTest.UI
{
    partial class BadgeBoxListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BadgeBoxListForm));
            this.dataGridView_BadgeBox = new System.Windows.Forms.DataGridView();
            this.column_BadgeBoxId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column_PrinterId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip_BadgeBox = new System.Windows.Forms.ToolStrip();
            this.button_AddBadgeBox = new System.Windows.Forms.ToolStripButton();
            this.button_EditBadgeBox = new System.Windows.Forms.ToolStripButton();
            this.button_DeleteBadgeBox = new System.Windows.Forms.ToolStripButton();
            this.label_Badges = new System.Windows.Forms.Label();
            this.label_BadgeBoxes = new System.Windows.Forms.Label();
            this.button_Ok = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.badgeListControl = new HP.ScalableTest.UI.BadgeListControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_BadgeBox)).BeginInit();
            this.toolStrip_BadgeBox.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_BadgeBox
            // 
            this.dataGridView_BadgeBox.AllowUserToAddRows = false;
            this.dataGridView_BadgeBox.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridView_BadgeBox.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_BadgeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_BadgeBox.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_BadgeBox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_BadgeBox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column_BadgeBoxId,
            this.column_Address,
            this.column_PrinterId});
            this.dataGridView_BadgeBox.EnableHeadersVisualStyles = false;
            this.dataGridView_BadgeBox.Location = new System.Drawing.Point(3, 51);
            this.dataGridView_BadgeBox.MultiSelect = false;
            this.dataGridView_BadgeBox.Name = "dataGridView_BadgeBox";
            this.dataGridView_BadgeBox.ReadOnly = true;
            this.dataGridView_BadgeBox.RowHeadersWidth = 24;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(241)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridView_BadgeBox.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_BadgeBox.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_BadgeBox.Size = new System.Drawing.Size(537, 101);
            this.dataGridView_BadgeBox.TabIndex = 0;
            this.dataGridView_BadgeBox.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_BadgeBox_CellDoubleClick);
            // 
            // column_BadgeBoxId
            // 
            this.column_BadgeBoxId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.column_BadgeBoxId.DataPropertyName = "BadgeBoxId";
            this.column_BadgeBoxId.HeaderText = "Badge Box Id";
            this.column_BadgeBoxId.Name = "column_BadgeBoxId";
            this.column_BadgeBoxId.ReadOnly = true;
            // 
            // column_Address
            // 
            this.column_Address.DataPropertyName = "IPAddress";
            this.column_Address.HeaderText = "IP Address";
            this.column_Address.Name = "column_Address";
            this.column_Address.ReadOnly = true;
            // 
            // column_PrinterId
            // 
            this.column_PrinterId.DataPropertyName = "PrinterId";
            this.column_PrinterId.HeaderText = "Associated Printer";
            this.column_PrinterId.Name = "column_PrinterId";
            this.column_PrinterId.ReadOnly = true;
            this.column_PrinterId.Width = 120;
            // 
            // toolStrip_BadgeBox
            // 
            this.toolStrip_BadgeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip_BadgeBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_AddBadgeBox,
            this.button_EditBadgeBox,
            this.button_DeleteBadgeBox});
            this.toolStrip_BadgeBox.Location = new System.Drawing.Point(0, 20);
            this.toolStrip_BadgeBox.Name = "toolStrip_BadgeBox";
            this.toolStrip_BadgeBox.Size = new System.Drawing.Size(543, 28);
            this.toolStrip_BadgeBox.TabIndex = 1;
            this.toolStrip_BadgeBox.Text = "toolStrip";
            // 
            // button_AddBadgeBox
            // 
            this.button_AddBadgeBox.Image = ((System.Drawing.Image)(resources.GetObject("button_AddBadgeBox.Image")));
            this.button_AddBadgeBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_AddBadgeBox.Name = "button_AddBadgeBox";
            this.button_AddBadgeBox.Size = new System.Drawing.Size(49, 25);
            this.button_AddBadgeBox.Text = "Add";
            this.button_AddBadgeBox.Click += new System.EventHandler(this.button_AddBadgeBox_Click);
            // 
            // button_EditBadgeBox
            // 
            this.button_EditBadgeBox.Image = ((System.Drawing.Image)(resources.GetObject("button_EditBadgeBox.Image")));
            this.button_EditBadgeBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_EditBadgeBox.Name = "button_EditBadgeBox";
            this.button_EditBadgeBox.Size = new System.Drawing.Size(47, 25);
            this.button_EditBadgeBox.Text = "Edit";
            this.button_EditBadgeBox.Click += new System.EventHandler(this.button_EditBadgeBox_Click);
            // 
            // button_DeleteBadgeBox
            // 
            this.button_DeleteBadgeBox.Image = ((System.Drawing.Image)(resources.GetObject("button_DeleteBadgeBox.Image")));
            this.button_DeleteBadgeBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_DeleteBadgeBox.Name = "button_DeleteBadgeBox";
            this.button_DeleteBadgeBox.Size = new System.Drawing.Size(60, 25);
            this.button_DeleteBadgeBox.Text = "Delete";
            this.button_DeleteBadgeBox.Click += new System.EventHandler(this.button_DeleteBadgeBox_Click);
            // 
            // label_Badges
            // 
            this.label_Badges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_Badges.AutoSize = true;
            this.label_Badges.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Badges.Location = new System.Drawing.Point(3, 170);
            this.label_Badges.Name = "label_Badges";
            this.label_Badges.Size = new System.Drawing.Size(47, 15);
            this.label_Badges.TabIndex = 3;
            this.label_Badges.Text = "Badges";
            // 
            // label_BadgeBoxes
            // 
            this.label_BadgeBoxes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_BadgeBoxes.AutoSize = true;
            this.label_BadgeBoxes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_BadgeBoxes.Location = new System.Drawing.Point(3, 5);
            this.label_BadgeBoxes.Name = "label_BadgeBoxes";
            this.label_BadgeBoxes.Size = new System.Drawing.Size(79, 15);
            this.label_BadgeBoxes.TabIndex = 4;
            this.label_BadgeBoxes.Text = "Badge Boxes";
            // 
            // button_Ok
            // 
            this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(465, 405);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 5;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.toolStrip_BadgeBox, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.button_Ok, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.dataGridView_BadgeBox, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.badgeListControl, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.label_Badges, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.label_BadgeBoxes, 0, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(5, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(543, 431);
            this.tableLayoutPanel.TabIndex = 6;
            // 
            // badgeListControl
            // 
            this.badgeListControl.BadgeBox = null;
            this.badgeListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.badgeListControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.badgeListControl.Location = new System.Drawing.Point(3, 188);
            this.badgeListControl.Name = "badgeListControl";
            this.badgeListControl.Size = new System.Drawing.Size(537, 209);
            this.badgeListControl.TabIndex = 2;
            // 
            // BadgeBoxListForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 448);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BadgeBoxListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Badge Box Management";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_BadgeBox)).EndInit();
            this.toolStrip_BadgeBox.ResumeLayout(false);
            this.toolStrip_BadgeBox.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_BadgeBox;
        private System.Windows.Forms.ToolStrip toolStrip_BadgeBox;
        private BadgeListControl badgeListControl;
        private System.Windows.Forms.Label label_Badges;
        private System.Windows.Forms.Label label_BadgeBoxes;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ToolStripButton button_AddBadgeBox;
        private System.Windows.Forms.ToolStripButton button_EditBadgeBox;
        private System.Windows.Forms.ToolStripButton button_DeleteBadgeBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_BadgeBoxId;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn column_PrinterId;
    }
}
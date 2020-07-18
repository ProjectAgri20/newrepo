namespace HP.ScalableTest.UI
{
    partial class BadgeBoxEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BadgeBoxEditForm));
            this.label_Descr = new System.Windows.Forms.Label();
            this.label_Address = new System.Windows.Forms.Label();
            this.label_PrinterId = new System.Windows.Forms.Label();
            this.label_Badges = new System.Windows.Forms.Label();
            this.textBox_Descr = new System.Windows.Forms.TextBox();
            this.textBox_Address = new System.Windows.Forms.TextBox();
            this.badgeListControl = new HP.ScalableTest.UI.BadgeListControl();
            this.textBox_PrinterId = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.textBox_BadgeBoxId = new System.Windows.Forms.TextBox();
            this.label_BadgeBoxId = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Descr
            // 
            this.label_Descr.AutoSize = true;
            this.label_Descr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Descr.Location = new System.Drawing.Point(3, 28);
            this.label_Descr.Name = "label_Descr";
            this.label_Descr.Size = new System.Drawing.Size(114, 28);
            this.label_Descr.TabIndex = 0;
            this.label_Descr.Text = "Description";
            this.label_Descr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Address
            // 
            this.label_Address.AutoSize = true;
            this.label_Address.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Address.Location = new System.Drawing.Point(3, 56);
            this.label_Address.Name = "label_Address";
            this.label_Address.Size = new System.Drawing.Size(114, 28);
            this.label_Address.TabIndex = 1;
            this.label_Address.Text = "IP Address";
            this.label_Address.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_PrinterId
            // 
            this.label_PrinterId.AutoSize = true;
            this.label_PrinterId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_PrinterId.Location = new System.Drawing.Point(3, 84);
            this.label_PrinterId.Name = "label_PrinterId";
            this.label_PrinterId.Size = new System.Drawing.Size(114, 28);
            this.label_PrinterId.TabIndex = 2;
            this.label_PrinterId.Text = "Associated Printer";
            this.label_PrinterId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Badges
            // 
            this.label_Badges.AutoSize = true;
            this.label_Badges.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label_Badges.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Badges.Location = new System.Drawing.Point(3, 123);
            this.label_Badges.Name = "label_Badges";
            this.label_Badges.Size = new System.Drawing.Size(114, 17);
            this.label_Badges.TabIndex = 3;
            this.label_Badges.Text = "Badges";
            // 
            // textBox_Descr
            // 
            this.textBox_Descr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Descr.Location = new System.Drawing.Point(123, 31);
            this.textBox_Descr.Name = "textBox_Descr";
            this.textBox_Descr.Size = new System.Drawing.Size(436, 23);
            this.textBox_Descr.TabIndex = 4;
            this.textBox_Descr.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Descr_Validating);
            // 
            // textBox_Address
            // 
            this.textBox_Address.Location = new System.Drawing.Point(123, 59);
            this.textBox_Address.Name = "textBox_Address";
            this.textBox_Address.Size = new System.Drawing.Size(116, 23);
            this.textBox_Address.TabIndex = 5;
            this.textBox_Address.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Address_Validating);
            // 
            // badgeListControl
            // 
            this.badgeListControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.badgeListControl.BadgeBox = null;
            this.badgeListControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.badgeListControl.Location = new System.Drawing.Point(6, 155);
            this.badgeListControl.Name = "badgeListControl";
            this.badgeListControl.Size = new System.Drawing.Size(562, 287);
            this.badgeListControl.TabIndex = 6;
            // 
            // textBox_PrinterId
            // 
            this.textBox_PrinterId.Location = new System.Drawing.Point(123, 87);
            this.textBox_PrinterId.Name = "textBox_PrinterId";
            this.textBox_PrinterId.Size = new System.Drawing.Size(116, 23);
            this.textBox_PrinterId.TabIndex = 7;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.label_BadgeBoxId, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBox_BadgeBoxId, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBox_Descr, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.label_Badges, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.label_Descr, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.label_PrinterId, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.label_Address, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBox_PrinterId, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBox_Address, 1, 2);
            this.tableLayoutPanel.Location = new System.Drawing.Point(6, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(562, 140);
            this.tableLayoutPanel.TabIndex = 8;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(496, 458);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 9;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ok.Location = new System.Drawing.Point(415, 458);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 10;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // textBox_BadgeBoxId
            // 
            this.textBox_BadgeBoxId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_BadgeBoxId.Location = new System.Drawing.Point(123, 3);
            this.textBox_BadgeBoxId.Name = "textBox_BadgeBoxId";
            this.textBox_BadgeBoxId.Size = new System.Drawing.Size(436, 23);
            this.textBox_BadgeBoxId.TabIndex = 11;
            // 
            // label_BadgeBoxId
            // 
            this.label_BadgeBoxId.AutoSize = true;
            this.label_BadgeBoxId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_BadgeBoxId.Location = new System.Drawing.Point(3, 0);
            this.label_BadgeBoxId.Name = "label_BadgeBoxId";
            this.label_BadgeBoxId.Size = new System.Drawing.Size(114, 28);
            this.label_BadgeBoxId.TabIndex = 11;
            this.label_BadgeBoxId.Text = "Badge Box Id";
            this.label_BadgeBoxId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BadgeBoxEditForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(575, 486);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.badgeListControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BadgeBoxEditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Badge Box";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Descr;
        private System.Windows.Forms.Label label_Address;
        private System.Windows.Forms.Label label_PrinterId;
        private System.Windows.Forms.Label label_Badges;
        private System.Windows.Forms.TextBox textBox_Descr;
        private System.Windows.Forms.TextBox textBox_Address;
        private BadgeListControl badgeListControl;
        private System.Windows.Forms.TextBox textBox_PrinterId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Label label_BadgeBoxId;
        private System.Windows.Forms.TextBox textBox_BadgeBoxId;
    }
}
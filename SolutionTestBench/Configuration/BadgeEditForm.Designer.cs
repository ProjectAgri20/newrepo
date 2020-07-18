namespace HP.ScalableTest.UI
{
    partial class BadgeEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BadgeEditForm));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_Descr = new System.Windows.Forms.TextBox();
            this.textBox_Username = new System.Windows.Forms.TextBox();
            this.label_Username = new System.Windows.Forms.Label();
            this.label_Index = new System.Windows.Forms.Label();
            this.label_BadgeBox = new System.Windows.Forms.Label();
            this.label_Descr = new System.Windows.Forms.Label();
            this.comboBox_BadgeBox = new System.Windows.Forms.ComboBox();
            this.textBox_Index = new System.Windows.Forms.TextBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label_BadgeId = new System.Windows.Forms.Label();
            this.textBox_BadgeId = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.textBox_BadgeId, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.label_BadgeId, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBox_Descr, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.textBox_Username, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.label_Username, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.label_Index, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.label_BadgeBox, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.label_Descr, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBox_BadgeBox, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.textBox_Index, 1, 3);
            this.tableLayoutPanel.Location = new System.Drawing.Point(6, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(401, 140);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // textBox_Descr
            // 
            this.textBox_Descr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Descr.Location = new System.Drawing.Point(123, 31);
            this.textBox_Descr.Name = "textBox_Descr";
            this.textBox_Descr.Size = new System.Drawing.Size(275, 20);
            this.textBox_Descr.TabIndex = 7;
            this.textBox_Descr.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Descr_Validating);
            // 
            // textBox_Username
            // 
            this.textBox_Username.Location = new System.Drawing.Point(123, 59);
            this.textBox_Username.Name = "textBox_Username";
            this.textBox_Username.Size = new System.Drawing.Size(100, 20);
            this.textBox_Username.TabIndex = 8;
            this.textBox_Username.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Username_Validating);
            // 
            // label_Username
            // 
            this.label_Username.AutoSize = true;
            this.label_Username.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Username.Location = new System.Drawing.Point(3, 56);
            this.label_Username.Name = "label_Username";
            this.label_Username.Size = new System.Drawing.Size(114, 28);
            this.label_Username.TabIndex = 2;
            this.label_Username.Text = "Username";
            this.label_Username.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Index
            // 
            this.label_Index.AutoSize = true;
            this.label_Index.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Index.Location = new System.Drawing.Point(3, 84);
            this.label_Index.Name = "label_Index";
            this.label_Index.Size = new System.Drawing.Size(114, 28);
            this.label_Index.TabIndex = 3;
            this.label_Index.Text = "Index";
            this.label_Index.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_BadgeBox
            // 
            this.label_BadgeBox.AutoSize = true;
            this.label_BadgeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_BadgeBox.Location = new System.Drawing.Point(3, 112);
            this.label_BadgeBox.Name = "label_BadgeBox";
            this.label_BadgeBox.Size = new System.Drawing.Size(114, 28);
            this.label_BadgeBox.TabIndex = 4;
            this.label_BadgeBox.Text = "Associated Badge Box";
            this.label_BadgeBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Descr
            // 
            this.label_Descr.AutoSize = true;
            this.label_Descr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Descr.Location = new System.Drawing.Point(3, 28);
            this.label_Descr.Name = "label_Descr";
            this.label_Descr.Size = new System.Drawing.Size(114, 28);
            this.label_Descr.TabIndex = 1;
            this.label_Descr.Text = "Description";
            this.label_Descr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox_BadgeBox
            // 
            this.comboBox_BadgeBox.DisplayMember = "Value";
            this.comboBox_BadgeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_BadgeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_BadgeBox.Location = new System.Drawing.Point(123, 115);
            this.comboBox_BadgeBox.Name = "comboBox_BadgeBox";
            this.comboBox_BadgeBox.Size = new System.Drawing.Size(275, 21);
            this.comboBox_BadgeBox.TabIndex = 5;
            this.comboBox_BadgeBox.ValueMember = "Key";
            // 
            // textBox_Index
            // 
            this.textBox_Index.Location = new System.Drawing.Point(123, 87);
            this.textBox_Index.Name = "textBox_Index";
            this.textBox_Index.Size = new System.Drawing.Size(37, 20);
            this.textBox_Index.TabIndex = 6;
            this.textBox_Index.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Index_Validating);
            // 
            // button_Ok
            // 
            this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ok.Location = new System.Drawing.Point(257, 161);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 12;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(337, 161);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 11;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // label_BadgeId
            // 
            this.label_BadgeId.AutoSize = true;
            this.label_BadgeId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_BadgeId.Location = new System.Drawing.Point(3, 0);
            this.label_BadgeId.Name = "label_BadgeId";
            this.label_BadgeId.Size = new System.Drawing.Size(114, 28);
            this.label_BadgeId.TabIndex = 13;
            this.label_BadgeId.Text = "Badge Id";
            this.label_BadgeId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_BadgeId
            // 
            this.textBox_BadgeId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_BadgeId.Location = new System.Drawing.Point(123, 3);
            this.textBox_BadgeId.Name = "textBox_BadgeId";
            this.textBox_BadgeId.Size = new System.Drawing.Size(275, 20);
            this.textBox_BadgeId.TabIndex = 13;
            // 
            // BadgeEditForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(414, 186);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BadgeEditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Badge";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBox_Descr;
        private System.Windows.Forms.TextBox textBox_Username;
        private System.Windows.Forms.Label label_Username;
        private System.Windows.Forms.Label label_Index;
        private System.Windows.Forms.Label label_BadgeBox;
        private System.Windows.Forms.Label label_Descr;
        private System.Windows.Forms.ComboBox comboBox_BadgeBox;
        private System.Windows.Forms.TextBox textBox_Index;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_BadgeId;
        private System.Windows.Forms.Label label_BadgeId;
    }
}
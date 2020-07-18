namespace HP.ScalableTest
{
    partial class SelectPrinterDialog
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
            this.accept_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.textBox_Address1 = new System.Windows.Forms.TextBox();
            this.label_Address1 = new System.Windows.Forms.Label();
            this.manualCheckBox = new System.Windows.Forms.CheckBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label_Address2 = new System.Windows.Forms.Label();
            this.textBox_Address2 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // accept_Button
            // 
            this.accept_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.accept_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.accept_Button.Location = new System.Drawing.Point(167, 148);
            this.accept_Button.Name = "accept_Button";
            this.accept_Button.Size = new System.Drawing.Size(75, 28);
            this.accept_Button.TabIndex = 3;
            this.accept_Button.Text = "OK";
            this.accept_Button.UseVisualStyleBackColor = true;
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(248, 148);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 28);
            this.cancel_Button.TabIndex = 4;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // textBox_Address1
            // 
            this.textBox_Address1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Address1.Location = new System.Drawing.Point(129, 3);
            this.textBox_Address1.Name = "textBox_Address1";
            this.textBox_Address1.Size = new System.Drawing.Size(179, 23);
            this.textBox_Address1.TabIndex = 0;
            // 
            // label_Address1
            // 
            this.label_Address1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_Address1.AutoSize = true;
            this.label_Address1.Location = new System.Drawing.Point(3, 0);
            this.label_Address1.Name = "label_Address1";
            this.label_Address1.Size = new System.Drawing.Size(52, 30);
            this.label_Address1.TabIndex = 4;
            this.label_Address1.Text = "Address:";
            this.label_Address1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // manualCheckBox
            // 
            this.manualCheckBox.AutoSize = true;
            this.manualCheckBox.Location = new System.Drawing.Point(12, 118);
            this.manualCheckBox.Name = "manualCheckBox";
            this.manualCheckBox.Size = new System.Drawing.Size(209, 19);
            this.manualCheckBox.TabIndex = 2;
            this.manualCheckBox.Text = "Enter printer information manually";
            this.manualCheckBox.UseVisualStyleBackColor = true;
            this.manualCheckBox.CheckedChanged += new System.EventHandler(this.manualCheckBox_CheckedChanged);
            // 
            // label_Password
            // 
            this.label_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_Password.AutoSize = true;
            this.label_Password.Location = new System.Drawing.Point(3, 30);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(98, 30);
            this.label_Password.TabIndex = 4;
            this.label_Password.Text = "Device Password:";
            this.label_Password.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Password
            // 
            this.textBox_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Password.Location = new System.Drawing.Point(129, 33);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(179, 23);
            this.textBox_Password.TabIndex = 1;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.textBox_Address2, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.label_Address2, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.label_Address1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.label_Password, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBox_Address1, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBox_Password, 1, 1);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(311, 89);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // label_Address2
            // 
            this.label_Address2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_Address2.AutoSize = true;
            this.label_Address2.Location = new System.Drawing.Point(3, 60);
            this.label_Address2.Name = "label_Address2";
            this.label_Address2.Size = new System.Drawing.Size(110, 30);
            this.label_Address2.TabIndex = 5;
            this.label_Address2.Text = "Secondary Address:";
            this.label_Address2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_Address2
            // 
            this.textBox_Address2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Address2.Location = new System.Drawing.Point(129, 63);
            this.textBox_Address2.Name = "textBox_Address2";
            this.textBox_Address2.Size = new System.Drawing.Size(179, 23);
            this.textBox_Address2.TabIndex = 6;
            // 
            // SelectPrinterDialog
            // 
            this.AcceptButton = this.accept_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(335, 183);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.manualCheckBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.accept_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SelectPrinterDialog";
            this.Text = "Add Printer";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button accept_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox textBox_Address1;
        private System.Windows.Forms.Label label_Address1;
        private System.Windows.Forms.CheckBox manualCheckBox;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBox_Address2;
        private System.Windows.Forms.Label label_Address2;
    }
}
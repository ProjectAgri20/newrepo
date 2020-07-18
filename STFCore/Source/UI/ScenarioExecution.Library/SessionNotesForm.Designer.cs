namespace HP.ScalableTest.UI.SessionExecution
{
    partial class SessionNotesForm
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
                if (_enterpriseTestContext != null)
                {
                    _enterpriseTestContext.Dispose();
                    _enterpriseTestContext = null;
                }

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
            this.notes_TextBox = new System.Windows.Forms.TextBox();
            this.notes_Label = new System.Windows.Forms.Label();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.tags_CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.tags_Label = new System.Windows.Forms.Label();
            this.reference_Label = new System.Windows.Forms.Label();
            this.sessionType_Label = new System.Windows.Forms.Label();
            this.sessionCycle_Label = new System.Windows.Forms.Label();
            this.reference_TextBox = new System.Windows.Forms.TextBox();
            this.sessionType_ComboBox = new System.Windows.Forms.ComboBox();
            this.sessionCycle_ComboBox = new System.Windows.Forms.ComboBox();
            this.sessionIdLabel_Label = new System.Windows.Forms.Label();
            this.sessionId_Label = new System.Windows.Forms.Label();
            this.sessionName_ComboBox = new System.Windows.Forms.ComboBox();
            this.sessionName_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.product_GridView = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.version_TextBox = new System.Windows.Forms.TextBox();
            this.add_Button = new System.Windows.Forms.Button();
            this.delete_Button = new System.Windows.Forms.Button();
            this.save_Button = new System.Windows.Forms.Button();
            this.addNewProduct_ComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.product_GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // notes_TextBox
            // 
            this.notes_TextBox.AcceptsReturn = true;
            this.notes_TextBox.AcceptsTab = true;
            this.notes_TextBox.Location = new System.Drawing.Point(108, 213);
            this.notes_TextBox.Multiline = true;
            this.notes_TextBox.Name = "notes_TextBox";
            this.notes_TextBox.Size = new System.Drawing.Size(456, 161);
            this.notes_TextBox.TabIndex = 3;
            // 
            // notes_Label
            // 
            this.notes_Label.AutoSize = true;
            this.notes_Label.Location = new System.Drawing.Point(64, 213);
            this.notes_Label.Name = "notes_Label";
            this.notes_Label.Size = new System.Drawing.Size(38, 15);
            this.notes_Label.TabIndex = 5;
            this.notes_Label.Text = "Notes";
            this.notes_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(420, 535);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(87, 23);
            this.ok_Button.TabIndex = 4;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(513, 535);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(87, 23);
            this.cancel_Button.TabIndex = 5;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // tags_CheckedListBox
            // 
            this.tags_CheckedListBox.CheckOnClick = true;
            this.tags_CheckedListBox.FormattingEnabled = true;
            this.tags_CheckedListBox.Location = new System.Drawing.Point(108, 131);
            this.tags_CheckedListBox.Name = "tags_CheckedListBox";
            this.tags_CheckedListBox.Size = new System.Drawing.Size(160, 76);
            this.tags_CheckedListBox.TabIndex = 6;
            // 
            // tags_Label
            // 
            this.tags_Label.AutoSize = true;
            this.tags_Label.Location = new System.Drawing.Point(71, 131);
            this.tags_Label.Name = "tags_Label";
            this.tags_Label.Size = new System.Drawing.Size(31, 15);
            this.tags_Label.TabIndex = 7;
            this.tags_Label.Text = "Tags";
            this.tags_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // reference_Label
            // 
            this.reference_Label.AutoSize = true;
            this.reference_Label.Location = new System.Drawing.Point(43, 105);
            this.reference_Label.Name = "reference_Label";
            this.reference_Label.Size = new System.Drawing.Size(59, 15);
            this.reference_Label.TabIndex = 46;
            this.reference_Label.Text = "Reference";
            this.reference_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionType_Label
            // 
            this.sessionType_Label.Location = new System.Drawing.Point(14, 76);
            this.sessionType_Label.Name = "sessionType_Label";
            this.sessionType_Label.Size = new System.Drawing.Size(88, 20);
            this.sessionType_Label.TabIndex = 45;
            this.sessionType_Label.Text = "Session Type";
            this.sessionType_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionCycle_Label
            // 
            this.sessionCycle_Label.AutoSize = true;
            this.sessionCycle_Label.Location = new System.Drawing.Point(249, 76);
            this.sessionCycle_Label.Name = "sessionCycle_Label";
            this.sessionCycle_Label.Size = new System.Drawing.Size(36, 15);
            this.sessionCycle_Label.TabIndex = 44;
            this.sessionCycle_Label.Text = "Cycle";
            this.sessionCycle_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // reference_TextBox
            // 
            this.reference_TextBox.Location = new System.Drawing.Point(108, 102);
            this.reference_TextBox.Name = "reference_TextBox";
            this.reference_TextBox.Size = new System.Drawing.Size(297, 23);
            this.reference_TextBox.TabIndex = 43;
            // 
            // sessionType_ComboBox
            // 
            this.sessionType_ComboBox.DisplayMember = "Name";
            this.sessionType_ComboBox.FormattingEnabled = true;
            this.sessionType_ComboBox.Location = new System.Drawing.Point(108, 73);
            this.sessionType_ComboBox.Name = "sessionType_ComboBox";
            this.sessionType_ComboBox.Size = new System.Drawing.Size(114, 23);
            this.sessionType_ComboBox.TabIndex = 42;
            this.sessionType_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // sessionCycle_ComboBox
            // 
            this.sessionCycle_ComboBox.DisplayMember = "Name";
            this.sessionCycle_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sessionCycle_ComboBox.FormattingEnabled = true;
            this.sessionCycle_ComboBox.Location = new System.Drawing.Point(291, 73);
            this.sessionCycle_ComboBox.Name = "sessionCycle_ComboBox";
            this.sessionCycle_ComboBox.Size = new System.Drawing.Size(114, 23);
            this.sessionCycle_ComboBox.TabIndex = 41;
            this.sessionCycle_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // sessionIdLabel_Label
            // 
            this.sessionIdLabel_Label.AutoSize = true;
            this.sessionIdLabel_Label.Location = new System.Drawing.Point(43, 17);
            this.sessionIdLabel_Label.Name = "sessionIdLabel_Label";
            this.sessionIdLabel_Label.Size = new System.Drawing.Size(59, 15);
            this.sessionIdLabel_Label.TabIndex = 48;
            this.sessionIdLabel_Label.Text = "Session Id";
            this.sessionIdLabel_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sessionId_Label
            // 
            this.sessionId_Label.Location = new System.Drawing.Point(108, 17);
            this.sessionId_Label.Name = "sessionId_Label";
            this.sessionId_Label.Size = new System.Drawing.Size(297, 15);
            this.sessionId_Label.TabIndex = 49;
            this.sessionId_Label.Text = "AB12345";
            // 
            // sessionName_ComboBox
            // 
            this.sessionName_ComboBox.DisplayMember = "Name";
            this.sessionName_ComboBox.FormattingEnabled = true;
            this.sessionName_ComboBox.Location = new System.Drawing.Point(108, 43);
            this.sessionName_ComboBox.Name = "sessionName_ComboBox";
            this.sessionName_ComboBox.Size = new System.Drawing.Size(456, 23);
            this.sessionName_ComboBox.TabIndex = 50;
            this.sessionName_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // sessionName_Label
            // 
            this.sessionName_Label.AutoSize = true;
            this.sessionName_Label.Location = new System.Drawing.Point(21, 46);
            this.sessionName_Label.Name = "sessionName_Label";
            this.sessionName_Label.Size = new System.Drawing.Size(81, 15);
            this.sessionName_Label.TabIndex = 51;
            this.sessionName_Label.Text = "Session Name";
            this.sessionName_Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 377);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 52;
            this.label1.Text = "Session Products";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // product_GridView
            // 
            this.product_GridView.AllowUserToAddRows = false;
            this.product_GridView.AllowUserToDeleteRows = false;
            this.product_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.product_GridView.Location = new System.Drawing.Point(24, 395);
            this.product_GridView.MultiSelect = false;
            this.product_GridView.Name = "product_GridView";
            this.product_GridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.product_GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.product_GridView.Size = new System.Drawing.Size(240, 150);
            this.product_GridView.TabIndex = 53;
            this.product_GridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.product_GridView_CellClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 377);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 54;
            this.label2.Text = "Version";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // version_TextBox
            // 
            this.version_TextBox.Enabled = false;
            this.version_TextBox.Location = new System.Drawing.Point(270, 395);
            this.version_TextBox.Name = "version_TextBox";
            this.version_TextBox.Size = new System.Drawing.Size(297, 23);
            this.version_TextBox.TabIndex = 55;
            // 
            // add_Button
            // 
            this.add_Button.Location = new System.Drawing.Point(270, 477);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(114, 23);
            this.add_Button.TabIndex = 56;
            this.add_Button.Text = "Add New Product";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // delete_Button
            // 
            this.delete_Button.Enabled = false;
            this.delete_Button.Location = new System.Drawing.Point(270, 522);
            this.delete_Button.Name = "delete_Button";
            this.delete_Button.Size = new System.Drawing.Size(114, 23);
            this.delete_Button.TabIndex = 57;
            this.delete_Button.Text = "Delete Selected";
            this.delete_Button.UseVisualStyleBackColor = true;
            this.delete_Button.Click += new System.EventHandler(this.delete_Button_Click);
            // 
            // save_Button
            // 
            this.save_Button.Enabled = false;
            this.save_Button.Location = new System.Drawing.Point(270, 424);
            this.save_Button.Name = "save_Button";
            this.save_Button.Size = new System.Drawing.Size(114, 23);
            this.save_Button.TabIndex = 58;
            this.save_Button.Text = "Save Version";
            this.save_Button.UseVisualStyleBackColor = true;
            this.save_Button.Click += new System.EventHandler(this.save_Button_Click);
            // 
            // addNewProduct_ComboBox
            // 
            this.addNewProduct_ComboBox.DisplayMember = "Name";
            this.addNewProduct_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.addNewProduct_ComboBox.FormattingEnabled = true;
            this.addNewProduct_ComboBox.Location = new System.Drawing.Point(390, 477);
            this.addNewProduct_ComboBox.Name = "addNewProduct_ComboBox";
            this.addNewProduct_ComboBox.Size = new System.Drawing.Size(177, 23);
            this.addNewProduct_ComboBox.TabIndex = 59;
            this.addNewProduct_ComboBox.ValueMember = "EnterpriseScenarioId";
            // 
            // SessionNotesForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(612, 564);
            this.Controls.Add(this.addNewProduct_ComboBox);
            this.Controls.Add(this.save_Button);
            this.Controls.Add(this.delete_Button);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.version_TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.product_GridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sessionName_Label);
            this.Controls.Add(this.sessionName_ComboBox);
            this.Controls.Add(this.sessionId_Label);
            this.Controls.Add(this.sessionIdLabel_Label);
            this.Controls.Add(this.reference_Label);
            this.Controls.Add(this.sessionType_Label);
            this.Controls.Add(this.sessionCycle_Label);
            this.Controls.Add(this.reference_TextBox);
            this.Controls.Add(this.sessionType_ComboBox);
            this.Controls.Add(this.sessionCycle_ComboBox);
            this.Controls.Add(this.tags_Label);
            this.Controls.Add(this.tags_CheckedListBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.notes_Label);
            this.Controls.Add(this.notes_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionNotesForm";
            this.Text = "Session Notes";
            this.Load += new System.EventHandler(this.SessionNotesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.product_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox notes_TextBox;
        private System.Windows.Forms.Label notes_Label;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.CheckedListBox tags_CheckedListBox;
        private System.Windows.Forms.Label tags_Label;
        private System.Windows.Forms.Label reference_Label;
        private System.Windows.Forms.Label sessionType_Label;
        private System.Windows.Forms.Label sessionCycle_Label;
        private System.Windows.Forms.TextBox reference_TextBox;
        protected System.Windows.Forms.ComboBox sessionType_ComboBox;
        protected System.Windows.Forms.ComboBox sessionCycle_ComboBox;
        private System.Windows.Forms.Label sessionIdLabel_Label;
        private System.Windows.Forms.Label sessionId_Label;
        private System.Windows.Forms.ComboBox sessionName_ComboBox;
        private System.Windows.Forms.Label sessionName_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView product_GridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox version_TextBox;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button delete_Button;
        private System.Windows.Forms.Button save_Button;
        protected System.Windows.Forms.ComboBox addNewProduct_ComboBox;
    }
}
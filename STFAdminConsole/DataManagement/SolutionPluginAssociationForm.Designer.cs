namespace HP.ScalableTest.LabConsole.DataManagement
{
    partial class SolutionPluginAssociationForm
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
            this.components = new System.ComponentModel.Container();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_button = new System.Windows.Forms.Button();
            this.product_DatagridView = new System.Windows.Forms.DataGridView();
            this.vendorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.associatedProductBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.metadata_DatagridView = new System.Windows.Forms.DataGridView();
            this.metadataTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.addProduct_button = new System.Windows.Forms.Button();
            this.vendor_TextBox = new System.Windows.Forms.TextBox();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.metaData_ComboBox = new System.Windows.Forms.ComboBox();
            this.addAssociation_Button = new System.Windows.Forms.Button();
            this.deleteProduct_Button = new System.Windows.Forms.Button();
            this.removeAssociation_Button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.editProduct_Button = new System.Windows.Forms.Button();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.product_DatagridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.associatedProductBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadata_DatagridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadataTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(687, 405);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 22;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // ok_button
            // 
            this.ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_button.Location = new System.Drawing.Point(579, 405);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(100, 32);
            this.ok_button.TabIndex = 21;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // product_DatagridView
            // 
            this.product_DatagridView.AllowUserToAddRows = false;
            this.product_DatagridView.AllowUserToDeleteRows = false;
            this.product_DatagridView.AutoGenerateColumns = false;
            this.product_DatagridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.product_DatagridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vendorDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn});
            this.product_DatagridView.DataSource = this.associatedProductBindingSource;
            this.product_DatagridView.Location = new System.Drawing.Point(40, 25);
            this.product_DatagridView.Name = "product_DatagridView";
            this.product_DatagridView.ReadOnly = true;
            this.product_DatagridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.product_DatagridView.Size = new System.Drawing.Size(295, 107);
            this.product_DatagridView.TabIndex = 23;
            this.product_DatagridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.product_DatagridView_CellEnter);
            // 
            // vendorDataGridViewTextBoxColumn
            // 
            this.vendorDataGridViewTextBoxColumn.DataPropertyName = "Vendor";
            this.vendorDataGridViewTextBoxColumn.HeaderText = "Vendor";
            this.vendorDataGridViewTextBoxColumn.Name = "vendorDataGridViewTextBoxColumn";
            this.vendorDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // associatedProductBindingSource
            // 
            this.associatedProductBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.AssociatedProduct);
            // 
            // metadata_DatagridView
            // 
            this.metadata_DatagridView.AllowUserToAddRows = false;
            this.metadata_DatagridView.AllowUserToDeleteRows = false;
            this.metadata_DatagridView.AutoGenerateColumns = false;
            this.metadata_DatagridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.metadata_DatagridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn1});
            this.metadata_DatagridView.DataSource = this.metadataTypeBindingSource;
            this.metadata_DatagridView.Location = new System.Drawing.Point(383, 25);
            this.metadata_DatagridView.MultiSelect = false;
            this.metadata_DatagridView.Name = "metadata_DatagridView";
            this.metadata_DatagridView.ReadOnly = true;
            this.metadata_DatagridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metadata_DatagridView.Size = new System.Drawing.Size(405, 107);
            this.metadata_DatagridView.TabIndex = 24;
            this.metadata_DatagridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.metadata_DatagridView_CellEnter);
            // 
            // metadataTypeBindingSource
            // 
            this.metadataTypeBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.MetadataType);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Add/Edit Product";
            // 
            // addProduct_button
            // 
            this.addProduct_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addProduct_button.Location = new System.Drawing.Point(40, 265);
            this.addProduct_button.Margin = new System.Windows.Forms.Padding(4);
            this.addProduct_button.Name = "addProduct_button";
            this.addProduct_button.Size = new System.Drawing.Size(100, 32);
            this.addProduct_button.TabIndex = 26;
            this.addProduct_button.Text = "Add As New";
            this.addProduct_button.UseVisualStyleBackColor = true;
            this.addProduct_button.Click += new System.EventHandler(this.addProduct_button_Click);
            // 
            // vendor_TextBox
            // 
            this.vendor_TextBox.Location = new System.Drawing.Point(40, 192);
            this.vendor_TextBox.Name = "vendor_TextBox";
            this.vendor_TextBox.Size = new System.Drawing.Size(193, 20);
            this.vendor_TextBox.TabIndex = 27;
            // 
            // name_TextBox
            // 
            this.name_TextBox.Location = new System.Drawing.Point(40, 238);
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(193, 20);
            this.name_TextBox.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Vendor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Name";
            // 
            // metaData_ComboBox
            // 
            this.metaData_ComboBox.DataSource = this.metadataTypeBindingSource;
            this.metaData_ComboBox.DisplayMember = "Name";
            this.metaData_ComboBox.FormattingEnabled = true;
            this.metaData_ComboBox.Location = new System.Drawing.Point(383, 173);
            this.metaData_ComboBox.Name = "metaData_ComboBox";
            this.metaData_ComboBox.Size = new System.Drawing.Size(194, 21);
            this.metaData_ComboBox.TabIndex = 31;
            this.metaData_ComboBox.ValueMember = "Name";
            // 
            // addAssociation_Button
            // 
            this.addAssociation_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addAssociation_Button.Enabled = false;
            this.addAssociation_Button.Location = new System.Drawing.Point(383, 204);
            this.addAssociation_Button.Margin = new System.Windows.Forms.Padding(4);
            this.addAssociation_Button.Name = "addAssociation_Button";
            this.addAssociation_Button.Size = new System.Drawing.Size(100, 32);
            this.addAssociation_Button.TabIndex = 32;
            this.addAssociation_Button.Text = "Add Association";
            this.addAssociation_Button.UseVisualStyleBackColor = true;
            this.addAssociation_Button.Click += new System.EventHandler(this.addAssociation_Button_Click);
            // 
            // deleteProduct_Button
            // 
            this.deleteProduct_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteProduct_Button.Enabled = false;
            this.deleteProduct_Button.Location = new System.Drawing.Point(235, 265);
            this.deleteProduct_Button.Margin = new System.Windows.Forms.Padding(4);
            this.deleteProduct_Button.Name = "deleteProduct_Button";
            this.deleteProduct_Button.Size = new System.Drawing.Size(100, 32);
            this.deleteProduct_Button.TabIndex = 33;
            this.deleteProduct_Button.Text = "Delete";
            this.deleteProduct_Button.UseVisualStyleBackColor = true;
            this.deleteProduct_Button.Click += new System.EventHandler(this.deleteProduct_Button_Click);
            // 
            // removeAssociation_Button
            // 
            this.removeAssociation_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeAssociation_Button.Enabled = false;
            this.removeAssociation_Button.Location = new System.Drawing.Point(688, 139);
            this.removeAssociation_Button.Margin = new System.Windows.Forms.Padding(4);
            this.removeAssociation_Button.Name = "removeAssociation_Button";
            this.removeAssociation_Button.Size = new System.Drawing.Size(100, 58);
            this.removeAssociation_Button.TabIndex = 34;
            this.removeAssociation_Button.Text = "Remove Metadata Association";
            this.removeAssociation_Button.UseVisualStyleBackColor = true;
            this.removeAssociation_Button.Click += new System.EventHandler(this.removeAssociation_Button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(380, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "MetaDataType Name";
            // 
            // editProduct_Button
            // 
            this.editProduct_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editProduct_Button.Enabled = false;
            this.editProduct_Button.Location = new System.Drawing.Point(40, 305);
            this.editProduct_Button.Margin = new System.Windows.Forms.Padding(4);
            this.editProduct_Button.Name = "editProduct_Button";
            this.editProduct_Button.Size = new System.Drawing.Size(100, 32);
            this.editProduct_Button.TabIndex = 36;
            this.editProduct_Button.Text = "Edit Selected";
            this.editProduct_Button.UseVisualStyleBackColor = true;
            this.editProduct_Button.Click += new System.EventHandler(this.editProduct_Button_Click);
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            this.nameDataGridViewTextBoxColumn1.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn1.Width = 300;
            // 
            // SolutionPluginAssociationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.editProduct_Button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.removeAssociation_Button);
            this.Controls.Add(this.deleteProduct_Button);
            this.Controls.Add(this.addAssociation_Button);
            this.Controls.Add(this.metaData_ComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.vendor_TextBox);
            this.Controls.Add(this.addProduct_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.metadata_DatagridView);
            this.Controls.Add(this.product_DatagridView);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_button);
            this.Name = "SolutionPluginAssociationForm";
            this.Text = "SolutionPluginAssociationForm";
            this.Load += new System.EventHandler(this.SolutionPluginAssociationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.product_DatagridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.associatedProductBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadata_DatagridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadataTypeBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.DataGridView product_DatagridView;
        private System.Windows.Forms.BindingSource associatedProductBindingSource;
        private System.Windows.Forms.DataGridView metadata_DatagridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addProduct_button;
        private System.Windows.Forms.TextBox vendor_TextBox;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox metaData_ComboBox;
        private System.Windows.Forms.BindingSource metadataTypeBindingSource;
        private System.Windows.Forms.Button addAssociation_Button;
        private System.Windows.Forms.Button deleteProduct_Button;
        private System.Windows.Forms.Button removeAssociation_Button;
        private System.Windows.Forms.Label label4;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Button editProduct_Button;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
    }
}
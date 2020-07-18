namespace HP.ScalableTest.LabConsole
{
    partial class PrinterProductsForm
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
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterProductsForm));
            this.productFamily_ComboBox = new System.Windows.Forms.ComboBox();
            this.productFamily_label = new System.Windows.Forms.Label();
            this.productName_label = new System.Windows.Forms.Label();
            this.printerProduct_OK = new System.Windows.Forms.Button();
            this.printerProduct_Apply = new System.Windows.Forms.Button();
            this.printerProduct_Cancel = new System.Windows.Forms.Button();
            this.stringValueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.productName_dataGridView = new System.Windows.Forms.DataGridView();
            this.enterpriseScenarioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.systemSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.scenarioSessionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nameValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.stringValueBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productName_dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enterpriseScenarioBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemSettingBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioSessionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // productFamily_ComboBox
            // 
            this.productFamily_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productFamily_ComboBox.FormattingEnabled = true;
            this.productFamily_ComboBox.Location = new System.Drawing.Point(90, 8);
            this.productFamily_ComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.productFamily_ComboBox.Name = "productFamily_ComboBox";
            this.productFamily_ComboBox.Size = new System.Drawing.Size(188, 21);
            this.productFamily_ComboBox.TabIndex = 0;
            this.productFamily_ComboBox.SelectedIndexChanged += new System.EventHandler(this.productFamily_ComboBox_SelectedIndexChanged);
            // 
            // productFamily_label
            // 
            this.productFamily_label.AutoSize = true;
            this.productFamily_label.Location = new System.Drawing.Point(10, 11);
            this.productFamily_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.productFamily_label.Name = "productFamily_label";
            this.productFamily_label.Size = new System.Drawing.Size(76, 13);
            this.productFamily_label.TabIndex = 1;
            this.productFamily_label.Text = "Product Family";
            // 
            // productName_label
            // 
            this.productName_label.AutoSize = true;
            this.productName_label.Location = new System.Drawing.Point(12, 39);
            this.productName_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.productName_label.Name = "productName_label";
            this.productName_label.Size = new System.Drawing.Size(75, 13);
            this.productName_label.TabIndex = 3;
            this.productName_label.Text = "Product Name";
            // 
            // printerProduct_OK
            // 
            this.printerProduct_OK.Location = new System.Drawing.Point(45, 292);
            this.printerProduct_OK.Margin = new System.Windows.Forms.Padding(2);
            this.printerProduct_OK.Name = "printerProduct_OK";
            this.printerProduct_OK.Size = new System.Drawing.Size(75, 23);
            this.printerProduct_OK.TabIndex = 4;
            this.printerProduct_OK.Text = "OK";
            this.printerProduct_OK.UseVisualStyleBackColor = true;
            this.printerProduct_OK.Click += new System.EventHandler(this.printerProduct_OK_Click);
            // 
            // printerProduct_Apply
            // 
            this.printerProduct_Apply.Location = new System.Drawing.Point(203, 292);
            this.printerProduct_Apply.Margin = new System.Windows.Forms.Padding(2);
            this.printerProduct_Apply.Name = "printerProduct_Apply";
            this.printerProduct_Apply.Size = new System.Drawing.Size(75, 23);
            this.printerProduct_Apply.TabIndex = 5;
            this.printerProduct_Apply.Text = "Apply";
            this.printerProduct_Apply.UseVisualStyleBackColor = true;
            this.printerProduct_Apply.Click += new System.EventHandler(this.printerProduct_Apply_Click);
            // 
            // printerProduct_Cancel
            // 
            this.printerProduct_Cancel.Location = new System.Drawing.Point(124, 292);
            this.printerProduct_Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.printerProduct_Cancel.Name = "printerProduct_Cancel";
            this.printerProduct_Cancel.Size = new System.Drawing.Size(75, 23);
            this.printerProduct_Cancel.TabIndex = 6;
            this.printerProduct_Cancel.Text = "Cancel";
            this.printerProduct_Cancel.UseVisualStyleBackColor = true;
            this.printerProduct_Cancel.Click += new System.EventHandler(this.printerProduct_Cancel_Click);
            // 
            // stringValueBindingSource
            // 
            this.stringValueBindingSource.DataSource = typeof(HP.ScalableTest.Framework.StringValue);
            // 
            // productName_dataGridView
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.productName_dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.productName_dataGridView.AutoGenerateColumns = false;
            this.productName_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productName_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameValue});
            this.productName_dataGridView.DataSource = this.stringValueBindingSource;
            this.productName_dataGridView.Location = new System.Drawing.Point(15, 54);
            this.productName_dataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.productName_dataGridView.Name = "productName_dataGridView";
            this.productName_dataGridView.RowHeadersWidth = 35;
            this.productName_dataGridView.RowTemplate.Height = 22;
            this.productName_dataGridView.Size = new System.Drawing.Size(263, 224);
            this.productName_dataGridView.TabIndex = 7;
            this.productName_dataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.productName_dataGridView_CellValidating);
            // 
            // enterpriseScenarioBindingSource
            // 
            this.enterpriseScenarioBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.EnterpriseScenario);
            // 
            // systemSettingBindingSource
            // 
            this.systemSettingBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.SystemSetting);
            // 
            // scenarioSessionBindingSource
            // 
            this.scenarioSessionBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.ScenarioSession);
            // 
            // nameValue
            // 
            this.nameValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameValue.DataPropertyName = "Value";
            this.nameValue.HeaderText = "Name";
            this.nameValue.Name = "nameValue";
            // 
            // PrinterProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 326);
            this.Controls.Add(this.productName_dataGridView);
            this.Controls.Add(this.printerProduct_Cancel);
            this.Controls.Add(this.printerProduct_Apply);
            this.Controls.Add(this.printerProduct_OK);
            this.Controls.Add(this.productName_label);
            this.Controls.Add(this.productFamily_label);
            this.Controls.Add(this.productFamily_ComboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PrinterProductsForm";
            this.Text = "Printer Products by Family";
            this.Load += new System.EventHandler(this.PrinterProductsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.stringValueBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productName_dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enterpriseScenarioBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemSettingBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioSessionBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox productFamily_ComboBox;
        private System.Windows.Forms.Label productFamily_label;
        private System.Windows.Forms.Label productName_label;
        private System.Windows.Forms.Button printerProduct_OK;
        private System.Windows.Forms.Button printerProduct_Apply;
        private System.Windows.Forms.Button printerProduct_Cancel;
        private System.Windows.Forms.BindingSource stringValueBindingSource;
        private System.Windows.Forms.DataGridView productName_dataGridView;
        private System.Windows.Forms.BindingSource enterpriseScenarioBindingSource;
        private System.Windows.Forms.BindingSource systemSettingBindingSource;
        private System.Windows.Forms.BindingSource scenarioSessionBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameValue;
    }
}
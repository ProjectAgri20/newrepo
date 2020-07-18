namespace HP.ScalableTest.Tools
{
    partial class Settings
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
            this.sitemap_GroupBox = new System.Windows.Forms.GroupBox();
            this.browse_Button = new System.Windows.Forms.Button();
            this.location_TextBox = new System.Windows.Forms.TextBox();
            this.location_Label = new System.Windows.Forms.Label();
            this.keys_DataGridView = new System.Windows.Forms.DataGridView();
            this.key1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.key2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.key3DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keysBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.siteMapDataSet = new HP.ScalableTest.Tools.SiteMapDataSet();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.keys_Label = new System.Windows.Forms.Label();
            this.sitemap_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keys_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.keysBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteMapDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // sitemap_GroupBox
            // 
            this.sitemap_GroupBox.Controls.Add(this.browse_Button);
            this.sitemap_GroupBox.Controls.Add(this.location_TextBox);
            this.sitemap_GroupBox.Controls.Add(this.location_Label);
            this.sitemap_GroupBox.Location = new System.Drawing.Point(13, 13);
            this.sitemap_GroupBox.Name = "sitemap_GroupBox";
            this.sitemap_GroupBox.Size = new System.Drawing.Size(722, 60);
            this.sitemap_GroupBox.TabIndex = 0;
            this.sitemap_GroupBox.TabStop = false;
            this.sitemap_GroupBox.Text = "Sitemap Location";
            // 
            // browse_Button
            // 
            this.browse_Button.Location = new System.Drawing.Point(679, 17);
            this.browse_Button.Name = "browse_Button";
            this.browse_Button.Size = new System.Drawing.Size(30, 23);
            this.browse_Button.TabIndex = 2;
            this.browse_Button.Text = "...";
            this.browse_Button.UseVisualStyleBackColor = true;
            this.browse_Button.Click += new System.EventHandler(this.browse_Button_Click);
            // 
            // location_TextBox
            // 
            this.location_TextBox.Enabled = false;
            this.location_TextBox.Location = new System.Drawing.Point(65, 20);
            this.location_TextBox.Name = "location_TextBox";
            this.location_TextBox.Size = new System.Drawing.Size(608, 20);
            this.location_TextBox.TabIndex = 1;
            // 
            // location_Label
            // 
            this.location_Label.AutoSize = true;
            this.location_Label.Location = new System.Drawing.Point(7, 20);
            this.location_Label.Name = "location_Label";
            this.location_Label.Size = new System.Drawing.Size(51, 13);
            this.location_Label.TabIndex = 0;
            this.location_Label.Text = "Location:";
            // 
            // keys_DataGridView
            // 
            this.keys_DataGridView.AutoGenerateColumns = false;
            this.keys_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.keys_DataGridView.ColumnHeadersVisible = false;
            this.keys_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.key1DataGridViewTextBoxColumn,
            this.key2DataGridViewTextBoxColumn,
            this.key3DataGridViewTextBoxColumn});
            this.keys_DataGridView.DataSource = this.keysBindingSource;
            this.keys_DataGridView.Location = new System.Drawing.Point(13, 100);
            this.keys_DataGridView.Name = "keys_DataGridView";
            this.keys_DataGridView.Size = new System.Drawing.Size(722, 451);
            this.keys_DataGridView.TabIndex = 1;
            // 
            // key1DataGridViewTextBoxColumn
            // 
            this.key1DataGridViewTextBoxColumn.DataPropertyName = "Key1";
            this.key1DataGridViewTextBoxColumn.HeaderText = "Key1";
            this.key1DataGridViewTextBoxColumn.Name = "key1DataGridViewTextBoxColumn";
            this.key1DataGridViewTextBoxColumn.Width = 220;
            // 
            // key2DataGridViewTextBoxColumn
            // 
            this.key2DataGridViewTextBoxColumn.DataPropertyName = "Key2";
            this.key2DataGridViewTextBoxColumn.HeaderText = "Key2";
            this.key2DataGridViewTextBoxColumn.Name = "key2DataGridViewTextBoxColumn";
            this.key2DataGridViewTextBoxColumn.Width = 220;
            // 
            // key3DataGridViewTextBoxColumn
            // 
            this.key3DataGridViewTextBoxColumn.DataPropertyName = "Key3";
            this.key3DataGridViewTextBoxColumn.HeaderText = "Key3";
            this.key3DataGridViewTextBoxColumn.Name = "key3DataGridViewTextBoxColumn";
            this.key3DataGridViewTextBoxColumn.Width = 220;
            // 
            // keysBindingSource
            // 
            this.keysBindingSource.DataMember = "Keys";
            this.keysBindingSource.DataSource = this.siteMapDataSet;
            // 
            // siteMapDataSet
            // 
            this.siteMapDataSet.DataSetName = "SiteMapDataSet";
            this.siteMapDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ok_Button
            // 
            this.ok_Button.Location = new System.Drawing.Point(578, 563);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "&OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(660, 563);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "&Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // keys_Label
            // 
            this.keys_Label.AutoSize = true;
            this.keys_Label.Location = new System.Drawing.Point(13, 81);
            this.keys_Label.Name = "keys_Label";
            this.keys_Label.Size = new System.Drawing.Size(68, 13);
            this.keys_Label.TabIndex = 4;
            this.keys_Label.Text = "Master Keys:";
            // 
            // Settings
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(747, 598);
            this.Controls.Add(this.keys_Label);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.keys_DataGridView);
            this.Controls.Add(this.sitemap_GroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.sitemap_GroupBox.ResumeLayout(false);
            this.sitemap_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keys_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.keysBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteMapDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox sitemap_GroupBox;
        private System.Windows.Forms.Label location_Label;
        private System.Windows.Forms.TextBox location_TextBox;
        private System.Windows.Forms.Button browse_Button;
        private System.Windows.Forms.DataGridView keys_DataGridView;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label keys_Label;
        private System.Windows.Forms.BindingSource keysBindingSource;
        private SiteMapDataSet siteMapDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn key1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn key2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn key3DataGridViewTextBoxColumn;
    }
}
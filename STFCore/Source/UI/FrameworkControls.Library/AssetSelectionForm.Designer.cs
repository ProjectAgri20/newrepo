namespace HP.ScalableTest.UI.Framework
{
    partial class AssetSelectionForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.select_Button = new System.Windows.Forms.Button();
            this.references_Button = new System.Windows.Forms.Button();
            this.assets_GridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.assets_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assets_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(703, 349);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 11;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // select_Button
            // 
            this.select_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.select_Button.Location = new System.Drawing.Point(597, 349);
            this.select_Button.Name = "select_Button";
            this.select_Button.Size = new System.Drawing.Size(100, 32);
            this.select_Button.TabIndex = 10;
            this.select_Button.Text = "Select";
            this.select_Button.UseVisualStyleBackColor = true;
            this.select_Button.Click += new System.EventHandler(this.select_Button_Click);
            // 
            // references_Button
            // 
            this.references_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.references_Button.Location = new System.Drawing.Point(12, 349);
            this.references_Button.Name = "references_Button";
            this.references_Button.Size = new System.Drawing.Size(143, 32);
            this.references_Button.TabIndex = 13;
            this.references_Button.Text = "Device Usage Summary";
            this.references_Button.UseVisualStyleBackColor = true;
            this.references_Button.Click += new System.EventHandler(this.usage_Button_Click);
            // 
            // assets_GridView
            // 
            this.assets_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assets_GridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.assets_GridView.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.assets_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.FieldName = "AssetId";
            gridViewTextBoxColumn1.HeaderText = "Device ID";
            gridViewTextBoxColumn1.MinWidth = 100;
            gridViewTextBoxColumn1.Name = "assetId_GridViewColumn";
            gridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn1.Width = 141;
            gridViewTextBoxColumn2.FieldName = "AssetType";
            gridViewTextBoxColumn2.HeaderText = "Device Type";
            gridViewTextBoxColumn2.MinWidth = 100;
            gridViewTextBoxColumn2.Name = "assetType_GridViewColumn";
            gridViewTextBoxColumn2.Width = 141;
            gridViewTextBoxColumn3.FieldName = "Description";
            gridViewTextBoxColumn3.HeaderText = "Description";
            gridViewTextBoxColumn3.Name = "description_GridViewColumn";
            gridViewTextBoxColumn3.Width = 389;
            gridViewTextBoxColumn4.FieldName = "ReservationKey";
            gridViewTextBoxColumn4.HeaderText = "Reservation Key";
            gridViewTextBoxColumn4.MinWidth = 70;
            gridViewTextBoxColumn4.Name = "reservationKey_GridViewColumn";
            gridViewTextBoxColumn4.Width = 127;
            this.assets_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.assets_GridView.MasterTemplate.EnableFiltering = true;
            sortDescriptor1.PropertyName = "assetId_GridViewColumn";
            this.assets_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.assets_GridView.Name = "assets_GridView";
            // 
            // 
            // 
            this.assets_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 300, 187);
            this.assets_GridView.Size = new System.Drawing.Size(815, 334);
            this.assets_GridView.TabIndex = 14;
            this.assets_GridView.Text = "radGridView1";
            this.assets_GridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.assets_GridView_CellDoubleClick);
            // 
            // AssetSelectionForm
            // 
            this.AcceptButton = this.select_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(815, 393);
            this.Controls.Add(this.assets_GridView);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.select_Button);
            this.Controls.Add(this.references_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "AssetSelectionForm";
            this.Text = "Device Selection";
            this.Load += new System.EventHandler(this.AssetSelectionForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AssetSelectionForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.assets_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assets_GridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button select_Button;
        private System.Windows.Forms.Button references_Button;
        private Telerik.WinControls.UI.RadGridView assets_GridView;


    }
}
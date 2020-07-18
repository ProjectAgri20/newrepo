namespace HP.ScalableTest.Framework.UI
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.assets_GridView = new Telerik.WinControls.UI.RadGridView();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.assets_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assets_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
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
            this.assets_GridView.MasterTemplate.AllowAddNewRow = false;
            this.assets_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.FieldName = "AssetId";
            gridViewTextBoxColumn1.HeaderText = "Device ID";
            gridViewTextBoxColumn1.MinWidth = 100;
            gridViewTextBoxColumn1.Name = "assetId_GridViewColumn";
            gridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn1.Width = 121;
            gridViewTextBoxColumn2.FieldName = "AssetType";
            gridViewTextBoxColumn2.HeaderText = "Device Type";
            gridViewTextBoxColumn2.MinWidth = 100;
            gridViewTextBoxColumn2.Name = "assetType_GridViewColumn";
            gridViewTextBoxColumn2.Width = 121;
            gridViewTextBoxColumn3.FieldName = "Description";
            gridViewTextBoxColumn3.HeaderText = "Description";
            gridViewTextBoxColumn3.Name = "description_GridViewColumn";
            gridViewTextBoxColumn3.Width = 333;
            gridViewTextBoxColumn4.FieldName = "ReservationKey";
            gridViewTextBoxColumn4.HeaderText = "Reservation Key";
            gridViewTextBoxColumn4.MinWidth = 70;
            gridViewTextBoxColumn4.Name = "reservationKey_GridViewColumn";
            gridViewTextBoxColumn4.Width = 107;
            this.assets_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.assets_GridView.MasterTemplate.EnableFiltering = true;
            sortDescriptor1.PropertyName = "assetId_GridViewColumn";
            this.assets_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.assets_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.assets_GridView.Name = "assets_GridView";
            // 
            // 
            // 
            this.assets_GridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 240, 150);
            this.assets_GridView.Size = new System.Drawing.Size(700, 299);
            this.assets_GridView.TabIndex = 0;
            this.assets_GridView.Text = "radGridView1";
            this.assets_GridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.assets_GridView_CellDoubleClick);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(532, 305);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(613, 305);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 2;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // AssetSelectionForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(700, 340);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.assets_GridView);
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
        private Telerik.WinControls.UI.RadGridView assets_GridView;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
    }
}
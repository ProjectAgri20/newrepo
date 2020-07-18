namespace HP.ScalableTest.Framework.UI
{
    partial class DocumentPreviewForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.close_Button = new System.Windows.Forms.Button();
            this.documents_GridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.documents_GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documents_GridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(613, 305);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 1;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // documents_GridView
            // 
            this.documents_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documents_GridView.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.documents_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.AllowGroup = false;
            gridViewTextBoxColumn1.FieldName = "FileName";
            gridViewTextBoxColumn1.HeaderText = "File Name";
            gridViewTextBoxColumn1.MinWidth = 200;
            gridViewTextBoxColumn1.Name = "fileName_GridViewColumn";
            gridViewTextBoxColumn1.SortOrder = Telerik.WinControls.UI.RadSortOrder.Ascending;
            gridViewTextBoxColumn1.Width = 283;
            gridViewTextBoxColumn2.FieldName = "FileSize";
            gridViewTextBoxColumn2.HeaderText = "File Size";
            gridViewTextBoxColumn2.MaxWidth = 100;
            gridViewTextBoxColumn2.MinWidth = 100;
            gridViewTextBoxColumn2.Name = "fileSize_GridViewColumn";
            gridViewTextBoxColumn2.Width = 100;
            gridViewTextBoxColumn3.FieldName = "Pages";
            gridViewTextBoxColumn3.HeaderText = "Pages";
            gridViewTextBoxColumn3.MaxWidth = 100;
            gridViewTextBoxColumn3.MinWidth = 100;
            gridViewTextBoxColumn3.Name = "pages_GridViewColumn";
            gridViewTextBoxColumn3.Width = 100;
            gridViewTextBoxColumn4.FieldName = "ColorMode";
            gridViewTextBoxColumn4.HeaderText = "Color Mode";
            gridViewTextBoxColumn4.MaxWidth = 100;
            gridViewTextBoxColumn4.MinWidth = 100;
            gridViewTextBoxColumn4.Name = "colorMode_GridViewColumn";
            gridViewTextBoxColumn4.Width = 100;
            gridViewTextBoxColumn5.FieldName = "Orientation";
            gridViewTextBoxColumn5.HeaderText = "Orientation";
            gridViewTextBoxColumn5.MaxWidth = 100;
            gridViewTextBoxColumn5.MinWidth = 100;
            gridViewTextBoxColumn5.Name = "orientation_GridViewColumn";
            gridViewTextBoxColumn5.Width = 100;
            this.documents_GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            sortDescriptor1.PropertyName = "fileName_GridViewColumn";
            this.documents_GridView.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.documents_GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.documents_GridView.Name = "documents_GridView";
            this.documents_GridView.Size = new System.Drawing.Size(700, 299);
            this.documents_GridView.TabIndex = 0;
            this.documents_GridView.Text = "radGridView1";
            // 
            // DocumentPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(700, 340);
            this.Controls.Add(this.documents_GridView);
            this.Controls.Add(this.close_Button);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DocumentPreviewForm";
            this.Text = "Preview Document Query Results";
            ((System.ComponentModel.ISupportInitialize)(this.documents_GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documents_GridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button close_Button;
        private Telerik.WinControls.UI.RadGridView documents_GridView;
    }
}
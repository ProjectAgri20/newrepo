namespace HP.ScalableTest.Framework.UI
{
    partial class DocumentBrowseControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentBrowseControl));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.documentTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.documentImages = new System.Windows.Forms.ImageList(this.components);
            this.selectedDocuments_ListControl = new Telerik.WinControls.UI.RadListControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTreeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedDocuments_ListControl)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.documentTreeView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.selectedDocuments_ListControl, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(459, 296);
            this.tableLayoutPanel.TabIndex = 63;
            // 
            // documentTreeView
            // 
            this.documentTreeView.CheckBoxes = true;
            this.documentTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.documentTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentTreeView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.documentTreeView.ForeColor = System.Drawing.Color.Black;
            this.documentTreeView.ImageList = this.documentImages;
            this.documentTreeView.Location = new System.Drawing.Point(3, 18);
            this.documentTreeView.Name = "documentTreeView";
            this.documentTreeView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.documentTreeView.ShowLines = true;
            this.documentTreeView.Size = new System.Drawing.Size(223, 275);
            this.documentTreeView.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.documentTreeView.SpacingBetweenNodes = -1;
            this.documentTreeView.TabIndex = 1;
            this.documentTreeView.Text = "radTreeView1";
            this.documentTreeView.TriStateMode = true;
            // 
            // documentImages
            // 
            this.documentImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("documentImages.ImageStream")));
            this.documentImages.TransparentColor = System.Drawing.Color.Transparent;
            this.documentImages.Images.SetKeyName(0, "Folder");
            this.documentImages.Images.SetKeyName(1, "Unknown");
            this.documentImages.Images.SetKeyName(2, "Text");
            this.documentImages.Images.SetKeyName(3, "Word");
            this.documentImages.Images.SetKeyName(4, "Excel");
            this.documentImages.Images.SetKeyName(5, "PowerPoint");
            this.documentImages.Images.SetKeyName(6, "PDF");
            this.documentImages.Images.SetKeyName(7, "Image");
            // 
            // selectedDocuments_ListControl
            // 
            this.selectedDocuments_ListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedDocuments_ListControl.Location = new System.Drawing.Point(232, 18);
            this.selectedDocuments_ListControl.Name = "selectedDocuments_ListControl";
            this.selectedDocuments_ListControl.Size = new System.Drawing.Size(224, 275);
            this.selectedDocuments_ListControl.TabIndex = 3;
            this.selectedDocuments_ListControl.Text = "radListControl1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Available Documents";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(232, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selected Documents";
            // 
            // DocumentBrowseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DocumentBrowseControl";
            this.Size = new System.Drawing.Size(459, 299);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentTreeView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectedDocuments_ListControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private Telerik.WinControls.UI.RadTreeView documentTreeView;
        private Telerik.WinControls.UI.RadListControl selectedDocuments_ListControl;
        private System.Windows.Forms.ImageList documentImages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

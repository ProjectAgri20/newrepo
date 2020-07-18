namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class TestDocumentResolverForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestDocumentResolverForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.oldNameValueLabel = new System.Windows.Forms.Label();
            this.oldNameLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.allDocuments_TreeView = new Telerik.WinControls.UI.RadTreeView();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.documentImages = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allDocuments_TreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.oldNameValueLabel);
            this.groupBox1.Controls.Add(this.oldNameLabel);
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 83);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unresolved Document Name";
            // 
            // oldNameValueLabel
            // 
            this.oldNameValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.oldNameValueLabel.Location = new System.Drawing.Point(94, 35);
            this.oldNameValueLabel.Name = "oldNameValueLabel";
            this.oldNameValueLabel.Size = new System.Drawing.Size(370, 25);
            this.oldNameValueLabel.TabIndex = 5;
            // 
            // oldNameLabel
            // 
            this.oldNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.oldNameLabel.Location = new System.Drawing.Point(12, 35);
            this.oldNameLabel.Name = "oldNameLabel";
            this.oldNameLabel.Size = new System.Drawing.Size(76, 21);
            this.oldNameLabel.TabIndex = 3;
            this.oldNameLabel.Text = "Name";
            this.oldNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.allDocuments_TreeView);
            this.groupBox2.Location = new System.Drawing.Point(14, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(482, 310);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Available Replacement Documents";
            // 
            // allDocuments_TreeView
            // 
            this.allDocuments_TreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allDocuments_TreeView.ImageList = this.documentImages;
            this.allDocuments_TreeView.Location = new System.Drawing.Point(17, 23);
            this.allDocuments_TreeView.Name = "allDocuments_TreeView";
            this.allDocuments_TreeView.Size = new System.Drawing.Size(450, 270);
            this.allDocuments_TreeView.SpacingBetweenNodes = -1;
            this.allDocuments_TreeView.TabIndex = 0;
            this.allDocuments_TreeView.Text = "radTreeView1";
            this.allDocuments_TreeView.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.allDocuments_TreeView_SelectedNodeChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(300, 439);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(94, 32);
            this.okButton.TabIndex = 9;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(401, 439);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(94, 32);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // documentImages
            // 
            this.documentImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("documentImages.ImageStream")));
            this.documentImages.TransparentColor = System.Drawing.Color.Transparent;
            this.documentImages.Images.SetKeyName(0, "FOLDER");
            this.documentImages.Images.SetKeyName(1, "UNKNOWN");
            this.documentImages.Images.SetKeyName(2, ".PDF");
            this.documentImages.Images.SetKeyName(3, ".DOC");
            this.documentImages.Images.SetKeyName(4, ".DOCX");
            this.documentImages.Images.SetKeyName(5, ".RTF");
            this.documentImages.Images.SetKeyName(6, ".XLS");
            this.documentImages.Images.SetKeyName(7, ".XLSX");
            this.documentImages.Images.SetKeyName(8, ".PPT");
            this.documentImages.Images.SetKeyName(9, ".PPTX");
            this.documentImages.Images.SetKeyName(10, ".TXT");
            this.documentImages.Images.SetKeyName(11, ".JPG");
            this.documentImages.Images.SetKeyName(12, ".JPEG");
            this.documentImages.Images.SetKeyName(13, ".PNG");
            this.documentImages.Images.SetKeyName(14, ".GIF");
            this.documentImages.Images.SetKeyName(15, ".TIF");
            this.documentImages.Images.SetKeyName(16, ".BMP");
            // 
            // TestDocumentResolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 484);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestDocumentResolverForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Document Resolution";
            this.Load += new System.EventHandler(this.PrintQueueUsageResolverForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.allDocuments_TreeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label oldNameValueLabel;
        private System.Windows.Forms.Label oldNameLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private Telerik.WinControls.UI.RadTreeView allDocuments_TreeView;
        private System.Windows.Forms.ImageList documentImages;
    }
}
namespace HP.ScalableTest.UI.Framework
{
    partial class PrintDriverDownloadControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintDriverDownloadControl));
            this.printDrivers_TreeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // printDrivers_TreeView
            // 
            this.printDrivers_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printDrivers_TreeView.HideSelection = false;
            this.printDrivers_TreeView.ImageIndex = 0;
            this.printDrivers_TreeView.ImageList = this.imageList;
            this.printDrivers_TreeView.Location = new System.Drawing.Point(0, 0);
            this.printDrivers_TreeView.Margin = new System.Windows.Forms.Padding(4);
            this.printDrivers_TreeView.Name = "printDrivers_TreeView";
            this.printDrivers_TreeView.SelectedImageIndex = 0;
            this.printDrivers_TreeView.Size = new System.Drawing.Size(479, 450);
            this.printDrivers_TreeView.TabIndex = 0;
            this.printDrivers_TreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.printDrivers_TreeView_BeforeExpand);
            this.printDrivers_TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.printDrivers_TreeView_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "Printing.png");
            // 
            // PrintDriverDownloadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.printDrivers_TreeView);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PrintDriverDownloadControl";
            this.Size = new System.Drawing.Size(479, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView printDrivers_TreeView;
        private System.Windows.Forms.ImageList imageList;
    }
}

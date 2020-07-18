namespace HP.ScalableTest.Framework.UI
{
    partial class RemotePrintQueueSelectionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemotePrintQueueSelectionControl));
            this.printQueueTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.queueImages = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.printQueueTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // printQueueTreeView
            // 
            this.printQueueTreeView.CheckBoxes = true;
            this.printQueueTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printQueueTreeView.ImageList = this.queueImages;
            this.printQueueTreeView.Location = new System.Drawing.Point(0, 0);
            this.printQueueTreeView.Name = "printQueueTreeView";
            this.printQueueTreeView.Size = new System.Drawing.Size(150, 150);
            this.printQueueTreeView.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.printQueueTreeView.SpacingBetweenNodes = -1;
            this.printQueueTreeView.TabIndex = 0;
            this.printQueueTreeView.Text = "radTreeView1";
            this.printQueueTreeView.TriStateMode = true;
            this.printQueueTreeView.NodeCheckedChanged += new Telerik.WinControls.UI.TreeNodeCheckedEventHandler(this.printQueueTreeView_NodeCheckedChanged);
            // 
            // queueImages
            // 
            this.queueImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("queueImages.ImageStream")));
            this.queueImages.TransparentColor = System.Drawing.Color.Transparent;
            this.queueImages.Images.SetKeyName(0, "Server");
            this.queueImages.Images.SetKeyName(1, "Queue");
            // 
            // RemotePrintQueueSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.printQueueTreeView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RemotePrintQueueSelectionControl";
            ((System.ComponentModel.ISupportInitialize)(this.printQueueTreeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTreeView printQueueTreeView;
        private System.Windows.Forms.ImageList queueImages;
    }
}

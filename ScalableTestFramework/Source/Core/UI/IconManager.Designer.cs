namespace HP.ScalableTest.Core.UI
{
    partial class IconManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IconManager));
            this.commonImageList = new System.Windows.Forms.ImageList(this.components);
            this.virtualResourceImageList = new System.Windows.Forms.ImageList(this.components);
            this.pluginImageList = new System.Windows.Forms.ImageList(this.components);
            this.assetImageList = new System.Windows.Forms.ImageList(this.components);
            this.configurationImageList = new System.Windows.Forms.ImageList(this.components);
            this.executionImageList = new System.Windows.Forms.ImageList(this.components);
            this.enableDisableImageList = new System.Windows.Forms.ImageList(this.components);
            this.disabledImageOverlay = new HP.ScalableTest.Core.UI.ImageOverlay();
            // 
            // commonImageList
            // 
            this.commonImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("commonImageList.ImageStream")));
            this.commonImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.commonImageList.Images.SetKeyName(0, "Unknown");
            this.commonImageList.Images.SetKeyName(1, "Folder");
            this.commonImageList.Images.SetKeyName(2, "Scenario");
            this.commonImageList.Images.SetKeyName(3, "Computer");
            this.commonImageList.Images.SetKeyName(4, "Monitor");
            this.commonImageList.Images.SetKeyName(5, "Server");
            this.commonImageList.Images.SetKeyName(6, "Session");
            this.commonImageList.Images.SetKeyName(7, "SessionResources");
            this.commonImageList.Images.SetKeyName(8, "SessionAssets");
            this.commonImageList.Images.SetKeyName(9, "RemotePrinter");
            this.commonImageList.Images.SetKeyName(10, "PrintServer");
            // 
            // virtualResourceImageList
            // 
            this.virtualResourceImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("virtualResourceImageList.ImageStream")));
            this.virtualResourceImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.virtualResourceImageList.Images.SetKeyName(0, "OfficeWorker");
            this.virtualResourceImageList.Images.SetKeyName(1, "AdminWorker");
            this.virtualResourceImageList.Images.SetKeyName(2, "CitrixWorker");
            this.virtualResourceImageList.Images.SetKeyName(3, "SolutionTester");
            this.virtualResourceImageList.Images.SetKeyName(4, "LoadTester");
            this.virtualResourceImageList.Images.SetKeyName(5, "PerfMonCollector");
            this.virtualResourceImageList.Images.SetKeyName(6, "EventLogCollector");
            this.virtualResourceImageList.Images.SetKeyName(7, "MachineReservation");
            // 
            // pluginImageList
            // 
            this.pluginImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.pluginImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.pluginImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // assetImageList
            // 
            this.assetImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("assetImageList.ImageStream")));
            this.assetImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.assetImageList.Images.SetKeyName(0, "PrintDevice");
            this.assetImageList.Images.SetKeyName(1, "JediSimulator");
            this.assetImageList.Images.SetKeyName(2, "BadgeBox");
            this.assetImageList.Images.SetKeyName(3, "MobileDevice");
            // 
            // configurationImageList
            // 
            this.configurationImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.configurationImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.configurationImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // executionImageList
            // 
            this.executionImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.executionImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.executionImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // enableDisableImageList
            // 
            this.enableDisableImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("enableDisableImageList.ImageStream")));
            this.enableDisableImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.enableDisableImageList.Images.SetKeyName(0, "Enable");
            this.enableDisableImageList.Images.SetKeyName(1, "Disable");
            // 
            // disabledImageOverlay
            // 
            this.disabledImageOverlay.CropArea = new System.Drawing.Rectangle(4, 4, 8, 8);
            this.disabledImageOverlay.HorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.disabledImageOverlay.Image = ((System.Drawing.Image)(resources.GetObject("disabledImageOverlay.Image")));
            this.disabledImageOverlay.VerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Bottom;

        }

        #endregion

        private ImageOverlay disabledImageOverlay;
        private System.Windows.Forms.ImageList commonImageList;
        private System.Windows.Forms.ImageList virtualResourceImageList;
        private System.Windows.Forms.ImageList pluginImageList;
        private System.Windows.Forms.ImageList assetImageList;
        private System.Windows.Forms.ImageList configurationImageList;
        private System.Windows.Forms.ImageList executionImageList;
        private System.Windows.Forms.ImageList enableDisableImageList;
    }
}

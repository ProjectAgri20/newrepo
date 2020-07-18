using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.EnterpriseTest;

namespace HP.ScalableTest.Core.UI
{
    /// <summary>
    /// Manages icons used in UI components.
    /// </summary>
    [ToolboxItem(false)]
    public partial class IconManager : Component
    {
        private static readonly Lazy<IconManager> _instance = new Lazy<IconManager>(() =>
        {
            IconManager manager = new IconManager();
            manager.Initialize();
            return manager;
        });

        /// <summary>
        /// Gets the <see cref="IconManager" /> singleton instance.
        /// </summary>
        public static IconManager Instance => _instance.Value;

        /// <summary>
        /// Gets an <see cref="ImageList" /> with icons used in the STF configuration view.
        /// </summary>
        public ImageList ConfigurationIcons => configurationImageList;

        /// <summary>
        /// Gets an <see cref="ImageList" /> with icons used in the STF execution view.
        /// </summary>
        public ImageList ExecutionIcons => executionImageList;

        /// <summary>
        /// Gets an <see cref="ImageList" /> with icons representing virtual resources.
        /// </summary>
        public ImageList VirtualResourceIcons => virtualResourceImageList;

        /// <summary>
        /// Gets an <see cref="ImageList" /> with icons representing plugins.
        /// </summary>
        public ImageList PluginIcons => pluginImageList;

        /// <summary>
        /// Gets an <see cref="Image" /> of an icon representing Enable.
        /// </summary>
        public Image EnableIcon => enableDisableImageList.Images["Enable"];

        /// <summary>
        /// Gets an <see cref="Image" /> of an icon representing Disable.
        /// </summary>
        public Image DisableIcon => enableDisableImageList.Images["Disable"];

        private IconManager()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            // Fetch plugin icons from the database
            using (EnterpriseTestContext context = DbConnect.EnterpriseTestContext())
            {
                foreach (MetadataType metadataType in context.MetadataTypes.Where(n => n.Icon != null))
                {
                    Image icon = ImageUtil.ReadImage(metadataType.Icon);
                    pluginImageList.Images.Add(metadataType.Name, icon);
                }
            }

            // Add icons with "disabled" overlay for resources and plugins
            ImageComposer.Append(virtualResourceImageList, disabledImageOverlay, "Disabled");
            ImageComposer.Append(pluginImageList, disabledImageOverlay, "Disabled");

            // For each image list, add the appropriate images and overlays.
            configurationImageList.Images.Add(commonImageList);
            configurationImageList.Images.Add(virtualResourceImageList);
            configurationImageList.Images.Add(pluginImageList);

            executionImageList.Images.Add(commonImageList);
            executionImageList.Images.Add(virtualResourceImageList);
            executionImageList.Images.Add(pluginImageList);
            executionImageList.Images.Add(assetImageList);
        }
    }
}

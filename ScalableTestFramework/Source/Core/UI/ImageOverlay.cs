using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace HP.ScalableTest.Core.UI
{
    /// <summary>
    /// An icon that can be applied to another image as an overlay.
    /// </summary>
    public class ImageOverlay : Component
    {
        private Image _image;

        /// <summary>
        /// Gets or sets the overlay image.
        /// </summary>
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                if (CropArea == new Rectangle())
                {
                    CropArea = Rectangle.FromLTRB(0, 0, _image.Width, _image.Height);
                }
            }
        }

        /// <summary>
        /// Gets or sets the portion of the image to use as the overlay.
        /// </summary>
        public Rectangle CropArea { get; set; }

        /// <summary>
        /// Gets or sets the preferred horizontal alignment relative to the parent image.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the preferred vertical alignment relative to the parent image.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }
    }
}

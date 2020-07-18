using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace HP.ScalableTest.Core.UI
{
    /// <summary>
    /// Creates images dynamically by combining other images.
    /// </summary>
    public static class ImageComposer
    {
        /// <summary>
        /// Composes a new <see cref="Image" /> by applying the specified overlay to the base image.
        /// </summary>
        /// <param name="image">The base image.</param>
        /// <param name="overlay">The overlay image.</param>
        /// <returns>A new overlaid <see cref="Image" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="image" /> is null.
        /// <para>or</para>
        /// <paramref name="overlay" /> is null.
        /// </exception>
        public static Image Create(Image image, ImageOverlay overlay)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (overlay == null)
            {
                throw new ArgumentNullException(nameof(overlay));
            }

            using (Image overlayImage = overlay.Image.Crop(overlay.CropArea))
            {
                Point offset = GetOverlayOffset(image, overlay);
                return Compose(image, overlayImage, offset);
            }
        }

        /// <summary>
        /// Composes a set of new images by applying the specified overlay to each image and appending it to the list.
        /// </summary>
        /// <param name="imageList">The base images.</param>
        /// <param name="overlay">The overlay image.</param>
        /// <param name="keySuffix">A suffix to be appended to the key of each composed image.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="imageList" /> is null.
        /// <para>or</para>
        /// <paramref name="overlay" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="keySuffix" /> is null or empty.
        /// </exception>
        public static void Append(ImageList imageList, ImageOverlay overlay, string keySuffix)
        {
            if (imageList == null)
            {
                throw new ArgumentNullException(nameof(imageList));
            }

            if (overlay == null)
            {
                throw new ArgumentNullException(nameof(overlay));
            }

            if (string.IsNullOrEmpty(keySuffix))
            {
                throw new ArgumentException("Key suffix cannot be null or empty.", nameof(keySuffix));
            }

            using (Image overlayImage = overlay.Image.Crop(overlay.CropArea))
            {
                foreach (string key in imageList.Images.Keys)
                {
                    Image image = imageList.Images[key];
                    Point offset = GetOverlayOffset(image, overlay);
                    string newKey = key + keySuffix;
                    imageList.Images.Add(newKey, Compose(image, overlayImage, offset));
                }
            }
        }

        /// <summary>
        /// Composes a new <see cref="Image" /> by applying the specified overlay to the base image.
        /// </summary>
        /// <param name="image">The base image.</param>
        /// <param name="overlay">The overlay image.</param>
        /// <param name="overlayOffset">The coordinate where the overlay image should be positioned.</param>
        /// <returns>A new overlaid <see cref="Image" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="image" /> is null.
        /// <para>or</para>
        /// <paramref name="overlay" /> is null.
        /// </exception>
        public static Image Compose(Image image, Image overlay, Point overlayOffset)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (overlay == null)
            {
                throw new ArgumentNullException(nameof(overlay));
            }

            Bitmap bitmap = new Bitmap(image);
            try
            {
                using (Graphics canvas = Graphics.FromImage(bitmap))
                {
                    canvas.DrawImage(overlay, overlayOffset);
                    canvas.Save();
                    return bitmap;
                }
            }
            catch
            {
                bitmap?.Dispose();
                throw;
            }
        }

        private static Point GetOverlayOffset(Image original, ImageOverlay overlay)
        {
            return new Point
            {
                X = GetHorizontalOffset(original.Width, overlay.CropArea.Width, overlay.HorizontalAlignment),
                Y = GetVerticalOffset(original.Height, overlay.CropArea.Height, overlay.VerticalAlignment)
            };
        }

        private static int GetHorizontalOffset(int originalWidth, int overlayWidth, HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    return (originalWidth - overlayWidth) / 2;

                case HorizontalAlignment.Right:
                    return originalWidth - overlayWidth;

                case HorizontalAlignment.Left:
                default:
                    return 0;
            }
        }

        private static int GetVerticalOffset(int originalHeight, int overlayHeight, VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case VerticalAlignment.Center:
                    return (originalHeight - overlayHeight) / 2;

                case VerticalAlignment.Bottom:
                    return originalHeight - overlayHeight;

                case VerticalAlignment.Top:
                default:
                    return 0;
            }
        }
    }
}

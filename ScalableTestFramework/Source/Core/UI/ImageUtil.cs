using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace HP.ScalableTest.Core.UI
{
    /// <summary>
    /// Provides extension methods and utilities for working with images.
    /// </summary>
    public static class ImageUtil
    {
        /// <summary>
        /// Converts an <see cref="Image" /> to a byte array.
        /// </summary>
        /// <param name="image">The <see cref="Image" />.</param>
        /// <returns>A byte array containing the image data.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="image" /> is null.</exception>
        public static byte[] ToByteArray(this Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Reads an <see cref="Image" /> out of a byte array.
        /// </summary>
        /// <param name="imageByteArray">The byte array containing the image data.</param>
        /// <returns>An <see cref="Image" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="imageByteArray" /> is null.</exception>
        public static Image ReadImage(byte[] imageByteArray)
        {
            if (imageByteArray == null)
            {
                throw new ArgumentNullException(nameof(imageByteArray));
            }

            using (MemoryStream ms = new MemoryStream(imageByteArray))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Crops the specified <see cref="Image" /> to the specified bounding area.
        /// </summary>
        /// <param name="image">The <see cref="Image" />.</param>
        /// <param name="cropArea">The area of the image to crop to.</param>
        /// <returns>A cropped image.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="image" /> is null.</exception>
        public static Image Crop(this Image image, Rectangle cropArea)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (image.Size == cropArea.Size)
            {
                return image;
            }
            else
            {
                using (Bitmap bmpImage = new Bitmap(image))
                {
                    return bmpImage.Clone(cropArea, image.PixelFormat);
                }
            }
        }

        /// <summary>
        /// Adds all images from the specified <see cref="ImageList" /> to the specified <see cref="ImageList.ImageCollection" />.
        /// </summary>
        /// <param name="imageCollection">The <see cref="ImageList.ImageCollection" /> to add the images to.</param>
        /// <param name="imageList">The <see cref="ImageList" /> containing the images to add.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="imageCollection" /> is null.
        /// <para>or</para>
        /// <paramref name="imageList" /> is null.
        /// </exception>
        public static void Add(this ImageList.ImageCollection imageCollection, ImageList imageList)
        {
            if (imageCollection == null)
            {
                throw new ArgumentNullException(nameof(imageCollection));
            }

            if (imageList == null)
            {
                throw new ArgumentNullException(nameof(imageList));
            }

            foreach (string key in imageList.Images.Keys)
            {
                imageCollection.Add(key, imageList.Images[key]);
            }
        }
    }
}

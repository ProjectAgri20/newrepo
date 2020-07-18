using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.FileAnalysis.Analyzers
{
    /// <summary>
    /// Implementation of <see cref="FileAnalyzer" /> for Tagged Image File Format (TIFF) files.
    /// </summary>
    [ObjectFactory("TIF")]
    [ObjectFactory("TIFF")]
    internal sealed class TiffAnalyzer : FileAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TiffAnalyzer" /> class.
        /// </summary>
        /// <param name="file">The file to be analyzed by this <see cref="FileAnalyzer" />.</param>
        public TiffAnalyzer(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// Performs basic validation on the file being analyzed by this <see cref="FileAnalyzer" />.
        /// </summary>
        /// <returns>A <see cref="FileValidationResult" /> object representing the result of validation.</returns>
        public override FileValidationResult Validate()
        {
            try
            {
                LogDebug($"Loading TIF image: {File.Name}");
                using (Image tiff = Image.FromFile(File.FullName))
                {
                    return FileValidationResult.Pass;
                }
            }
            catch (OutOfMemoryException)
            {
                LogWarn("Unable to load image.");
                return FileValidationResult.Fail("The file is not a valid TIF document.");
            }
        }

        /// <summary>
        /// Gets information about a document, such as page count, title, and author.
        /// </summary>
        /// <returns>A <see cref="DocumentProperties" /> object.</returns>
        public override DocumentProperties GetProperties()
        {
            DocumentProperties properties = new DocumentProperties(File);

            try
            {
                LogDebug($"Loading TIF image: {File.Name}");
                using (Image tiff = Image.FromFile(File.FullName))
                {
                    properties.Pages = (short)tiff.GetFrameCount(FrameDimension.Page);
                    properties.Orientation = (tiff.Size.Height > tiff.Size.Width) ? Orientation.Portrait : Orientation.Landscape;
                }
            }
            catch (OutOfMemoryException)
            {
                // An OutOfMemoryException is thrown if loading the file is unsuccessful.
                // Log the error, then return whatever properties have been collected.
                LogWarn("Unable to load image.");
            }

            return properties;
        }
    }
}

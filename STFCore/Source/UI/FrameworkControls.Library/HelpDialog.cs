using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// HTML display dialog used to display HTML pages.  It provides a mechanism for 
    /// embedded images to display as well (see remarks).
    /// </summary>
    /// <remarks>
    /// This control also supports the use of images that are stored as embedded resources
    /// in a project.  In order for this to work, the HTML document to be displayed must use
    /// a unique ID as the value for the src="ABCD" attribute in the img element.  Then
    /// the developer creates a dictionary using this unique Id as the key and the embedded
    /// resource Bitmap as the value.  This dictionary is passed into the alternate constructor.
    /// The dialog will then iterate over the images and write them to a temp location, then
    /// update the HTML to point to those locations and then load the Browser control.  This
    /// will ensure the images are found in the HTML document.
    /// </remarks>
    public partial class HelpDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpDialog"/> class.
        /// </summary>
        /// <param name="html">The HTML.</param>
        public HelpDialog(string html)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);

            // Load the web browser with the base HTML, then wait for it to fully load
            webBrowser.DocumentText = html;
            do
            {
                Application.DoEvents();
            } while (webBrowser.ReadyState != WebBrowserReadyState.Complete);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpDialog" /> class.
        /// </summary>
        /// <param name="html">The HTML data.</param>
        /// <param name="images">The collection of image maps.</param>
        public HelpDialog(string html, Dictionary<string, Bitmap> images)
            : this(html)
        {
            if (images != null)
            {
                // Write out temp files for all the image entries provided with this page
                var locations = WriteImageFiles(images);

                // Update all the image references with the path to the temporary image file.
                var updatedHtml = UpdateImageReferences(locations);

                // Reload the web browser with the updated HTML data and wait for it to load
                webBrowser.DocumentText = updatedHtml.ToString();
                do
                {
                    Application.DoEvents();
                } while (webBrowser.ReadyState != WebBrowserReadyState.Complete);

                // Delete the temporary image files from disk
                DeleteImageFiles(locations);
            }            
        }

        static Dictionary<string, string> WriteImageFiles(Dictionary<string, Bitmap> images)
        {
            Dictionary<string, string> locations = new Dictionary<string, string>();

            // For each image file reference given, write the Bitmap data out and 
            // save the file location.
            foreach (var key in images.Keys)
            {
                locations.Add(key, Path.GetTempFileName());
                images[key].Save(locations[key]);
            }

            return locations;
        }

        static void DeleteImageFiles(Dictionary<string, string> locations)
        {
            // Delete each temporary image file created earlier
            foreach (var key in locations.Keys)
            {
                try
                {
                    File.Delete(locations[key]);
                }
                catch (IOException ex)
                {
                    TraceFactory.Logger.Error("Failed to delete image file: {0}".FormatWith(ex.Message));
                }
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private StringBuilder UpdateImageReferences(Dictionary<string, string> imageLocations)
        {
            string tag = "img";
            string attr = "src";

            StringBuilder updatedHtml = new StringBuilder(webBrowser.Document.Body.Parent.OuterHtml);

            // Find the element referenced by the incoming tag
            foreach (HtmlElement element in webBrowser.Document.Body.Parent.GetElementsByTagName(tag))
            {
                // Pull out the attribute value for this element, in this case "src" 
                string url = element.GetAttribute(attr).Replace("about:", string.Empty);
                string outerHtml = element.OuterHtml;

                // Break down the element and iterate over each part.  If a part is not the one
                // we are interested in ("src"), then just add it back into the replacement string.
                // If it is, then change its value to the location value of the temp image file.                
                StringBuilder replacement = new StringBuilder();
                string[] items = outerHtml.Split(new string[] { " " }, StringSplitOptions.None);
                foreach (var item in items)
                {
                    if (!item.StartsWith(attr, StringComparison.OrdinalIgnoreCase) || !imageLocations.ContainsKey(url))
                    {
                        replacement.Append(item + " ");
                    }
                    else
                    {
                        replacement.Append(attr + "=\"" + imageLocations[url] + "\" ");
                    }
                }

                // In the master HTML map, replace this element with the updated element that
                // now has a path to the temp location.
                updatedHtml.Replace(outerHtml, replacement.ToString());
            }

            return updatedHtml;
        }

        // Click for Ok_Button is explicitly handled since for modeless dialogs clicking on Close Button doesn't close the dialog.
        private void ok_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

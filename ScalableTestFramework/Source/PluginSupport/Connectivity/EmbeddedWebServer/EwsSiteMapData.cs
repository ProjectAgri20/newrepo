using System;
using System.IO;
using System.Xml;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{

    /// <summary>
    /// Class that manages the location of the EWS Site Maps and their usage.
    /// </summary>
    public class EwsSiteMapData
    {
        private EwsSettings _settings = null;
        private Uri _deviceUrl = null;
        private XmlDocument _doc = null;
        private XmlNode _currentPageNode = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsSiteMapData" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public EwsSiteMapData(EwsSettings settings, string siteMapPath)
        {
            _settings = settings;

            // load site maps into xml document
            LoadSiteMaps(siteMapPath);
        }

        /// <summary>
        /// Gets the device URL.
        /// </summary>
        /// <value>
        /// The device URL.
        /// </value>
        public Uri DeviceUrl
        {
            get { return _deviceUrl; }
        }

        /// <summary>
        /// Sets the current page.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="hypertext">protocol: http or https</param>
        public void SelectPage(string key, string hypertext)
        {
            XmlElement root = _doc.DocumentElement;

            // get page relative path from sitemaps
            _currentPageNode = root.SelectSingleNode("//Page[@key='{0}']".FormatWith(key));

            if (null != _currentPageNode)
            {
                _deviceUrl = new Uri("{0}://{1}/{2}".FormatWith
                        (
                            hypertext,
                            _settings.DeviceAddress,
                            _currentPageNode.Attributes["relative_path"].Value
                        ));
            }
            else
            {
                throw new Exception("Current selected page {0} doesn't exists in the sitemaps".FormatWith(key));
            }
        }

        /// <summary>
        /// Gets the locator.
        /// </summary>
        /// <param name="controlId">The control id.</param>
        /// <param name="sourceType">HTML element source type</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">You must call SetSiteMap() first</exception>
        public string GetLocator(string controlId, out FindType sourceType)
        {
            if (null == _currentPageNode)
            {
                throw new FileNotFoundException("You must set the Page first");
            }


            var node = _currentPageNode.SelectSingleNode("Elements/Element[@key='{0}']".FormatWith(controlId));

            if (node == null)
            {
                throw new XmlException("Unable to find Element Id for {0}".FormatWith(controlId));
            }

            // Priority goes in this order id, name, xpath and CSS class

            var attribute = node.Attributes["id"];

            if (attribute == null || string.IsNullOrWhiteSpace(attribute.Value.Trim()))
            {
                attribute = node.Attributes["name"];

                if (attribute == null || string.IsNullOrWhiteSpace(attribute.Value.Trim()))
                {
                    attribute = node.Attributes["xpath"];

                    if (attribute == null || string.IsNullOrWhiteSpace(attribute.Value.Trim()))
                    {
                        attribute = node.Attributes["class"];

                        if (attribute == null || string.IsNullOrWhiteSpace(attribute.Value.Trim()))
                        {
                            throw new XmlException("Check sitemaps for '{0}' locator, either attributes are missing or has empty values.".FormatWith(controlId));
                        }
                        else
                        {
                            sourceType = FindType.ByClassName;
                            return attribute.Value;
                        }
                    }
                    else
                    {
                        sourceType = FindType.ByXPath;
                        return attribute.Value;
                    }
                }
                else
                {
                    sourceType = FindType.ByName;
                    string name = attribute.Value;

                    if (_settings.AdapterType == EwsAdapterType.SeleniumServerRC)
                    {
                        name = "name=" + attribute.Value;
                    }

                    return name;
                }
            }
            else
            {
                sourceType = FindType.ById;
                return attribute.Value;
            }
        }

        /// <summary>
        /// Loads the site maps into the xml objects
        /// </summary>
        private void LoadSiteMaps(string basePath)
        {
            // walk thru all the xml files in the device sitemaps directory and load
            foreach (string siteMap in Directory.GetFiles(basePath, "*.xml", SearchOption.TopDirectoryOnly))
            {
                TraceFactory.Logger.Info("sitemap-filename : {0}".FormatWith(siteMap));
                if (null == _doc)
                {
                    _doc = new XmlDocument();
                    _doc.Load(siteMap);
                }
                else
                {
                    CombineSiteMap(siteMap);
                }
            }
        }

        /// <summary>
        /// Combines the current site map into the main site map document
        /// </summary>
        /// <param name="siteMap">Site map xml file</param>
        private void CombineSiteMap(string siteMap)
        {
            XmlDocument doc = new XmlDocument();

            // load the current site map and merge into the master site map object
            doc.Load(siteMap);

            // walk thru each page and add the page to the master site map list
            foreach (XmlNode page in doc.DocumentElement.SelectNodes("//Page"))
            {
                XmlNode newPage = _doc.ImportNode(page, true);
                _doc.DocumentElement.SelectSingleNode("//Page").AppendChild(newPage);
            }
        }
    }
}

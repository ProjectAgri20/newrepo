using System;
using System.Globalization;
using System.IO;
using System.Xml;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{
    /// <summary>
    /// Used to create an <see cref="EwsSettings"/> class from an XML configuration file or XML string
    /// </summary>
    public static class EwsSettingsFactory
    {
        /// <summary>
        /// Loads the XML string representing configuration data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="configurationId">The configuration id.</param>
        /// <returns></returns>
        /// <exception cref="System.Xml.XmlException">Configuration node not found</exception>
        /// <remarks>
        /// This allows the client to read XML based configuration data to create an
        /// EwsSettings object.  The schema should be as follows:
        /// <Configurations>
        ///   <Configuration>
        ///     <Browser>Explorer</Browser>
        ///     <Server>localhost</Server>
        ///     <Version>Version 1.1</Version>
        ///     <Port>4444</Port>
        ///     <Product>Mamba</Product>
        ///     <HostName>16.185.186.57</HostName>
        ///     <ProductType>VEP</ProductType>
        ///   </Configuration>
        /// </Configurations>
        /// </remarks>
        public static EwsSettings CreateFromXml(string xml, string configurationId = "")
        {
            EwsSettings settings = new EwsSettings();
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(xml);
                XmlNode root = null;

                // If the user provides a set of configurations defined by an id attribute
                // the proper one can be selected, otherwise, the first element will be
                // selected
                if (!string.IsNullOrEmpty(configurationId))
                {
                    root = doc.SelectSingleNode("//Configuration[@id='{0}']".FormatWith(configurationId));
                    if (root == null)
                    {
                        throw new XmlException("Configuration node not found");
                    }
                }
                else
                {
                    var nodes = doc.SelectNodes("//Configuration");
                    if (nodes != null && nodes.Count == 1)
                    {
                        root = nodes[0];
                    }
                    else
                    {
                        if (nodes == null)
                        {
                            throw new XmlException("Configuration node not found");
                        }
                        else if (nodes.Count > 1)
                        {
                            throw new XmlException("More than one configuration node available");
                        }
                        else
                        {
                            throw new XmlException("Configuration node note found");
                        }
                    }
                }

                var browserStr = root.SelectSingleNode("Browser").InnerText;
                settings.Browser = Enum<BrowserModel>.Parse(browserStr);
                settings.HttpRemoteControlHost = root.SelectSingleNode("HttpRemoteControlHost").InnerText;
                var port = root.SelectSingleNode("HttpRemoteControlPort").InnerText;
                if (!string.IsNullOrEmpty(port))
                {
                    settings.HttpRemoteControlPort = int.Parse(port, CultureInfo.CurrentCulture);
                }
                settings.SitemapsLocation = root.SelectSingleNode("Version").InnerText;
                settings.ProductName = root.SelectSingleNode("Product").InnerText;
                settings.DeviceAddress = root.SelectSingleNode("DeviceAddress").InnerText;
                settings.ProductType = Enum<PrinterFamilies>.Parse(root.SelectSingleNode("ProductType").InnerText);
            }
            catch (XmlException ex)
            {
                Logger.LogError("Failed to parse configuration", ex);
                throw;
            }
            catch (ArgumentException ex)
            {
                Logger.LogError("Failed to parse configuration", ex);
                throw;
            }

            return settings;
        }

        /// <summary>
        /// Loads the specified XML configuration filename.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <param name="configurationId">The configuration id.</param>
        /// <returns></returns>
        public static EwsSettings CreateFromFile(string fileName, string configurationId = "")
        {
            return CreateFromXml(File.ReadAllText(fileName), configurationId);
        }
    }
}
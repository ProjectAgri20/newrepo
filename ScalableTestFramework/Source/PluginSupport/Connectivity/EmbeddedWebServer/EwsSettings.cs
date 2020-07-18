using System;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using System.IO;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{
    /// <summary>
    ///
    /// </summary>
    public class EwsSettings
    {
        private string _siteMapsPath = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsSettings" /> class.
        /// </summary>
        public EwsSettings()
        {
            // Setting some defaults here, maybe they shouldn't be set...?
            HttpRemoteControlHost = "localhost";
            HttpRemoteControlPort = 4444;
            Browser = BrowserModel.Explorer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsSettings" /> class.
        /// </summary>
        /// <param name="productName">Name of the product.</param>
        /// <param name="productType">Type of the product.</param>
        /// <param name="sitemapsLocation">The sitemaps location.</param>
        /// <param name="deviceAddress">The device address.</param>
        /// <param name="browser">The browser.</param>
        /// <param name="adapterType">Type of Adapter</param>
        /// <param name="remoteControlServer">The remote control server.</param>
        /// <param name="remoteControlPort">The remote control port.</param>
        public EwsSettings
            (
                string productName,
                PrinterFamilies productType,
                string sitemapsLocation,
                string deviceAddress,
                BrowserModel browser,
                EwsAdapterType adapterType = EwsAdapterType.SeleniumServerRC,
                string remoteControlServer = "localhost",
                int remoteControlPort = 4444
            )
            : this()
        {
            ProductName = productName;
            ProductType = productType;
            SitemapsLocation = sitemapsLocation;
            DeviceAddress = deviceAddress;
            Browser = browser;
            AdapterType = adapterType;
            HttpRemoteControlHost = remoteControlServer;
            HttpRemoteControlPort = remoteControlPort;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EwsSettings" /> class.
        /// </summary>
        /// <param name="productName">Name of the product.</param>
        /// <param name="productType">Type of the product.</param>
        /// <param name="sitemapsLocation">The sitemaps Location.</param>
        /// <param name="deviceAddress">The device address.</param>
        /// <param name="browser">The browser.</param>
        /// <param name="adapterType">Type of Adapter</param>
        /// <param name="remoteControlServer">The remote control server.</param>
        /// <param name="remoteControlPort">The remote control port.</param>
        public EwsSettings
            (
                string productName,
                PrinterFamilies productType,
                string sitemapsLocation,
                string deviceAddress,
                BrowserModel browser,
                TimeSpan pageNavigationDelay,
                TimeSpan elementOperationDelay,
                EwsAdapterType adapterType = EwsAdapterType.SeleniumServerRC,
                string remoteControlServer = "localhost",
                int remoteControlPort = 4444
            )
            : this()
        {
            ProductName = productName;
            ProductType = productType;
            SitemapsLocation = sitemapsLocation;
            DeviceAddress = deviceAddress;
            Browser = browser;
            AdapterType = adapterType;
            HttpRemoteControlHost = remoteControlServer;
            HttpRemoteControlPort = remoteControlPort;
            PageNavigationDelay = pageNavigationDelay;
            ElementOperationDelay = elementOperationDelay;
        }

        /// <summary>
        /// Gets the device site maps path.
        /// </summary>
        /// <value>
        /// The device site maps path.
        /// </value>
        public string GetDeviceSiteMapPath(string basePath)
        {
            if (string.IsNullOrEmpty(_siteMapsPath))
            {
                // Look in global settings for the path to the SiteMap files.  This
                // can be local or on a central server.  In production this would
                // ideally be a central server.
                _siteMapsPath = Path.Combine(basePath, SitemapsLocation);
            }

            return _siteMapsPath;
        }

        /// <summary>
        /// Gets or sets the browser.
        /// </summary>
        /// <value>
        /// The browser.
        /// </value>
        public BrowserModel Browser { get; set; }

        /// <summary>
        /// Gets or sets the type of the product.
        /// </summary>
        /// <value>
        /// The type of the product.
        /// </value>
        public PrinterFamilies ProductType { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the device address.
        /// </summary>
        /// <value>
        /// The device address.
        /// </value>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string SitemapsLocation { get; set; }

        /// <summary>
        /// Enum for AdapterType
        /// </summary>
        /// <value>
        /// The AdapterType.
        /// </value>
        public EwsAdapterType AdapterType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP remote control server.
        /// </summary>
        /// <value>
        /// The HTTP control server.
        /// </value>
        public string HttpRemoteControlHost { get; set; }

        /// <summary>
        /// Gets or sets the HTTP remote control port.
        /// </summary>
        /// <value>
        /// The HTTP remote control port.
        /// </value>
        public int HttpRemoteControlPort { get; set; }

        /// <summary>
        /// Gets or sets the page navigation delay
        /// </summary>
        public TimeSpan PageNavigationDelay { get; set; }

        /// <summary>
        /// Gets or sets the element operation delay 
        /// </summary>
        public TimeSpan ElementOperationDelay { get; set; }
    }
}
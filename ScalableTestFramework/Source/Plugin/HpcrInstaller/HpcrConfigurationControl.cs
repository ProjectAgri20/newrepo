using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using HP.DeviceAutomation;

namespace HP.ScalableTest.Plugin.HpcrInstaller
{
    /// <summary>
    /// Provides a control that is used to configure the plug-in.
    /// </summary>
    /// <remarks>
    /// This class inherits from the <see cref="UserControl"/> class and implements the
    /// <see cref="IPluginConfigurationControl"/> interface.
    ///
    /// The <see cref="UserControl"/> class provides an empty control that can be used to contain
    /// other controls that are used to configure this plug-in.
    ///
    /// <seealso cref="IPluginConfigurationControl"/>
    /// <seealso cref="System.Windows.Forms.UserControl"/>
    /// </remarks>
    [ToolboxItem(false)]
    public partial class HpcrConfigurationControl : UserControl, IPluginConfigurationControl
    {
        /// <summary>
        /// Create the definition of the data that will be used by this activity.  It will be
        /// instantiated later when the plug-in is started up.
        /// </summary>
        private HpcrActivityData _data;

        /// <summary>
        ///
        /// </summary>
        public HpcrConfigurationControl()
        {
            InitializeComponent();
            hpcrAction_ComboBox.DataSource = EnumUtil.GetValues<HpcrAction>().ToList();
            fieldValidator.RequireSelection(hpcrAction_ComboBox, hpcrAction_Label);
            fieldValidator.RequireSelection(deviceGroup_ComboBox, deviceGroup_Label);
            fieldValidator.RequireSelection(hpcr_ServerComboBox, hpcrServer_Label);
            hpcrAction_ComboBox.SelectedIndex = 0;
        }

        #region IPluginConfigurationControl implementation

        /// <summary>
        /// This event indicates to the framework that the user has changed something in the
        /// configuration; it will be used to keep track of unsaved changes so the user can be
        /// notified. This event should be raised whenever the user makes a change that modifies
        /// the configuration that the control would return.
        ///
        /// Failure to use this event will not prevent metadata from saving successfully; however,
        /// the user will not be prompted if they attempt to navigate away from the activity
        /// without saving their changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// This method should return a new <see cref="PluginConfigurationData"/> object containing
        /// all the configuration from the control. (This is the same object used in Initialize.)
        /// Custom metadata is passed into the constructor, either as XML or an object that will be
        /// serialized. The metadata version can be set to any value other than null.
        ///
        /// Selection data for assets and documents is set using the Assets and Documents
        /// properties. Plug-ins that will not make use of Assets and/or Documents can ignore these
        /// properties.
        ///
        /// <seealso cref="PluginConfigurationData"/>
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data = new HpcrActivityData
            {
                HpcrAction = (HpcrAction)hpcrAction_ComboBox.SelectedItem,
                DeviceGroup = deviceGroup_ComboBox.SelectedItem.ToString(),
            };

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = hpcr_AssetSelectionControl.AssetSelectionData,
                Servers = new ServerSelectionData(hpcr_ServerComboBox.SelectedServer)
            };
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        ///
        /// <seealso cref="PluginEnvironment"/>
        /// </summary>
        /// <param name="environment">Information about the plug-in environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data.
            _data = new HpcrActivityData();
            hpcr_AssetSelectionControl.Initialize(AssetAttributes.ControlPanel);
            hpcr_ServerComboBox.Initialize("HPCR");
        }

        /// <summary>
        /// Provides plug-in configuration data for an existing activity, including plug-in
        /// specific metadata, a metadata version, and selections of assets and documents.
        ///
        /// The custom metadata can be retrieved from the configuration using one of the
        /// <c>GetMetadata</c> methods, which return either a deserialized object or the XML. This
        /// data is then used to populate the configuration control. Asset and Document information
        /// can be retrieved using. The metadata version is included for forwards compatibility.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plug-in environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of the
            // configuration information.
            _data = configuration.GetMetadata<HpcrActivityData>();
            hpcr_AssetSelectionControl.Initialize(configuration.Assets, AssetAttributes.ControlPanel);
            hpcr_ServerComboBox.Initialize(configuration.Servers.SelectedServers.FirstOrDefault(), "HPCR");
            hpcrAction_ComboBox.SelectedItem = _data.HpcrAction;
            deviceGroup_ComboBox.DataSource = GetDeviceGroups(hpcr_ServerComboBox.SelectedServer.Address);
            deviceGroup_ComboBox.SelectedItem = _data.DeviceGroup;
        }

        /// <summary>
        /// This method is used for validating the data entered on the control before saving.
        ///
        /// This method must return a <see cref="PluginValidationResult"/> indicating whether
        /// validation was successful, and if not, the error message(s) to present to the user.
        ///
        /// <seealso cref="PluginValidationResult"/>
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult"/> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            // This is where you can add any validation for your UI and then
            // return the appropriate validation result when saving the data.
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        #endregion IPluginConfigurationControl implementation

        /// <summary>
        /// This method should be called when the configuration of the plug-in changes. It will
        /// raise the 'ConfigurationChanged' event that will eventually inform the user that they
        /// need to save the configuration.
        /// </summary>
        /// <param name="e">Contains any event data that should be sent with the event.</param>
        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private List<string> GetDeviceGroups(string hpcrServerAddress)
        {
            Uri getUri = new Uri($"http://{hpcrServerAddress}/webapi/scripts/omisapiu.dll");
            string data = "<?xml version='1.0' encoding='UTF-8' ?><Transaction><ContainerGetProperties><Parameters><ObjectType>00030023</ObjectType></Parameters></ContainerGetProperties><Client><Locale>0409</Locale></Client></Transaction>";
            var response = SendRequest(getUri, data);

            if (string.IsNullOrEmpty(response))
                return new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            XmlNodeList ns = xmlDoc.DocumentElement.SelectNodes("PropertyList/prGroups/PropertyList/prName");

            List<string> prGroups = ns.Cast<XmlNode>().Select(x => x.InnerText).ToList();
            return prGroups;
        }

        private void hpcr_ServerComboBox_SelectionChanged(object sender, EventArgs e)
        {
            deviceGroup_ComboBox.DataSource = null;
            deviceGroup_ComboBox.DataSource = GetDeviceGroups(hpcr_ServerComboBox.SelectedServer.Address);
        }

        private static string SendRequest(Uri requestUri, string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "POST";
            request.ContentType = "application/xml; charset=utf-8";
            request.KeepAlive = false;

           

            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = buffer.Length;
                Stream sw = request.GetRequestStream();
                sw.Write(buffer, 0, buffer.Length);
                sw.Close();

                var response = HttpMessenger.ExecuteRequest(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return string.Empty;
                }

                return response.Body;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return string.Empty;
            }

           

        }
    }
}
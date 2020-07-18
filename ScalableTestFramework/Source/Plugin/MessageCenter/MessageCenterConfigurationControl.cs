﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.MessageCenter
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
    public partial class MessageCenterConfigurationControl : UserControl, IPluginConfigurationControl
    {
        /// <summary>
        /// Create the definition of the data that will be used by this activity.  It will be
        /// instantiated later when the plug-in is started up.
        /// </summary>
        private MessageCenterActivityData _data;

        /// <summary>
        /// 
        /// </summary>
        public MessageCenterConfigurationControl()
        {
            InitializeComponent();

            messageType_comboBox.DataSource = EnumUtil.GetValues<MessageType>().ToList();
            message_fieldValidator.RequireAssetSelection(message_assetSelectionControl);
            message_fieldValidator.RequireSelection(messageType_comboBox, messageType_label);
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
            _data.Presence = present_radioButton.Checked;
            _data.Message = (MessageType)messageType_comboBox.SelectedItem;
            _data.UseEws = ews_radioButton.Checked;
            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = message_assetSelectionControl.AssetSelectionData,

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
            _data = new MessageCenterActivityData();

            message_assetSelectionControl.Initialize(AssetAttributes.ControlPanel);
            
        }

        /// <summary>
        /// Provides plug-in configuration data for an existing activity, including plug-in
        /// specific metadata, a metadata version, and selections of assets and documents.
        /// 
        /// The custom metadata can be retrieved from the configuration using one of the
        /// <c>GetMetadata</c> methods, which return either a deserialized object or the XML. This
        /// data is then used to populate the configuration control. Asset and Document information
        /// can be retrieved using <see cref="ConfigurationServices"/>. The metadata version is
        /// included for forwards compatibility.
        /// 
        /// <seealso cref="ConfigurationServices"/>
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plug-in environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of the
            // configuration information.
            _data = configuration.GetMetadata<MessageCenterActivityData>();

            message_assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.ControlPanel);
            messageType_comboBox.SelectedIndex = messageType_comboBox.Items.IndexOf(_data.Message);
            present_radioButton.Checked = _data.Presence;
            absent_radioButton.Checked = !_data.Presence;
            ews_radioButton.Checked = _data.UseEws;
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
            return new PluginValidationResult(message_fieldValidator.ValidateAll());
        }

        #endregion

        /// <summary>
        /// This method should be called when the configuration of the plug-in changes. It will
        /// raise the 'ConfigurationChanged' event that will eventually inform the user that they
        /// need to save the configuration.
        /// </summary>
        /// <param name="e">Contains any event data that should be sent with the event.</param>
        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            EventHandler handler = ConfigurationChanged;
            handler?.Invoke(this, e);
        }
    }
}

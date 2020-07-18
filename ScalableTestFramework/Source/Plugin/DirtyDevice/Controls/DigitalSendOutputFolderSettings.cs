using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    internal partial class DigitalSendOutputFolderSettings : UserControl
    {
        private const string UncPathLookupFailedMessage = "Digital Send Folder lookup exception must be corrected before this plugin can be used.";
        private const string NeedUncPathsForPluginMessage = "Digital Send Destination(s) with UNC paths must be defined in settings before this plugin can be used.  Drive-letter paths won't work.";
        private string _blockingErrorMessage = "There is an unknown problem with the Digital Send Output Folder control or selected value.";
        private string _nonBlockingErrorMessage = "There is an unknown problem with the Digital Send Output Folder selected value.";
        public event EventHandler ConfigurationChanged;

        public DigitalSendOutputFolderSettings()
        {
            InitializeComponent();

            foreach (var control in new Control[] { UncFolderComboBox, OutputFolderLabel })
            {
                fieldValidator.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
            }

            fieldValidator.RequireValue(UncFolderComboBox, "UNC Folder", ValidationCondition.IfEnabled);

            var locations = new List<KeyValuePair<string, DigitalSendOutputLocation>>();
            try
            {
                foreach (string location in ConfigurationServices.EnvironmentConfiguration.GetOutputMonitorDestinations("OutputDirectory"))
                {
                    if (!location.Contains(":"))
                    {
                        var outputLocation = new DigitalSendOutputLocation(location);
                        locations.Add(new KeyValuePair<string, DigitalSendOutputLocation>(outputLocation.ToShortUncPath(), outputLocation));
                    }
                }
            }
            catch (Exception x)
            {
                _blockingErrorMessage = UncPathLookupFailedMessage + " " + x.ToString();
                // Put this validator on the label instead of the combobox to avoid overwriting the validator on the combobox.  User will not be able to fix this issue from this plugin.
                fieldValidator.RequireCustom(OutputFolderLabel, DigitalSendLocationBlockingError);
                fieldValidator.ValidateAll();
            }

            if (locations.Count == 0)
            {
                _blockingErrorMessage = NeedUncPathsForPluginMessage;
                // Put this validator on the label instead of the combobox to avoid overwriting the validator on the combobox.  User will not be able to fix this issue from this plugin.
                fieldValidator.RequireCustom(OutputFolderLabel, DigitalSendLocationBlockingError);
                fieldValidator.ValidateAll();
            }

            UncFolderComboBox.DataSource = locations;
            UncFolderComboBox.DisplayMember = "Key";
            UncFolderComboBox.ValueMember = "Value";
            UncFolderComboBox.SelectedValueChanged += (s, e) => OnConfigurationChanged(s, e);
        }

        public DigitalSendOutputFolderActivityData Value
        {
            get
            {
                var activityData = new DigitalSendOutputFolderActivityData()
                {
                    OutputFolder = (DigitalSendOutputLocation)UncFolderComboBox.SelectedValue,
                };
                return activityData;
            }
            set
            {
                var loc = value.OutputFolder;
                if (loc == null)
                {
                    UncFolderComboBox.ResetText();
                    UncFolderComboBox.SelectedIndex = -1;
                    return;
                }

                var selectedItem = UncFolderComboBox.Items
                    .OfType<KeyValuePair<string, DigitalSendOutputLocation>>()
                    .Select(kvp => kvp.Value)
                    .SingleOrDefault(item =>
                        item != null &&
                        item.ServerHostName == loc.ServerHostName &&
                        item.MonitorLocation == loc.MonitorLocation);

                if (selectedItem == null)
                {
                    var expectedValue = $"{nameof(loc.ServerHostName)}='{loc.ServerHostName}'; {nameof(loc.MonitorLocation)}='{loc.MonitorLocation}'";
                    _nonBlockingErrorMessage = $"Cannot select nonexistent value in {OutputFolderLabel.Text}. ({expectedValue})";
                    if (_blockingErrorMessage == null)
                    {
                        fieldValidator.RequireCustom(OutputFolderLabel, DigitalSendLocationNonBlockingError);
                    }
                    return;
                }

                UncFolderComboBox.SelectedValue = selectedItem;
            }
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            // If the user was warned about the previously selected output location and is choosing another one clear the error since they are only allowed to choose valid selections.
            _nonBlockingErrorMessage = null;

            ConfigurationChanged?.Invoke(this, e);
        }

        private ValidationResult DigitalSendLocationBlockingError()
        {
            return new ValidationResult(false, _blockingErrorMessage);
        }

        private ValidationResult DigitalSendLocationNonBlockingError()
        {
            if (_nonBlockingErrorMessage == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(false, _nonBlockingErrorMessage);
        }

        public IEnumerable<ValidationResult> ValidateConfiguration()
        {
            return fieldValidator.ValidateAll();
        }
    }
}

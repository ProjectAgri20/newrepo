using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    internal partial class DirtyDeviceSettings : UserControl
    {
        public event EventHandler ConfigurationChanged;
        public DirtyDeviceActionFlags[] OutputFolderRequired = new[] { DirtyDeviceActionFlags.DigitalSend, DirtyDeviceActionFlags.EWS };
        public DirtyDeviceActionFlags[] QuickSetRequired = new[] { DirtyDeviceActionFlags.EWS };

        public DirtyDeviceSettings()
        {
            InitializeComponent();
            pluginActions1.ConfigurationChanged += (s, e) => OnConfigurationChanged(s, e);
            digitalSendOutputFolderSettings1.ConfigurationChanged += (s, e) => OnConfigurationChanged(s, e);
            ewsSettings1.ConfigurationChanged += (s, e) => OnConfigurationChanged(s, e);
        }

        public DirtyDeviceActivityData Value
        {
            get
            {
                return new DirtyDeviceActivityData()
                {
                    DirtyDeviceActionFlags = pluginActions1.Value,
                    DigitalSend = digitalSendOutputFolderSettings1.Value,
                    Ews = ewsSettings1.Value,
                };
            }
            set
            {
                pluginActions1.Value = value.DirtyDeviceActionFlags;
                digitalSendOutputFolderSettings1.Value = value.DigitalSend;
                ewsSettings1.Value = value.Ews;
            }
        }

        public IEnumerable<ValidationResult> ValidateConfiguration()
        {
            return pluginActions1.ValidateConfiguration().Union(digitalSendOutputFolderSettings1.ValidateConfiguration()).Union(ewsSettings1.ValidateConfiguration());
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            digitalSendOutputFolderSettings1.Enabled = OutputFolderRequired.Any(flag => pluginActions1.Value.HasFlag(flag));
            ewsSettings1.Enabled = QuickSetRequired.Any(flag => pluginActions1.Value.HasFlag(flag));
            ConfigurationChanged?.Invoke(this, e);
        }
    }
}

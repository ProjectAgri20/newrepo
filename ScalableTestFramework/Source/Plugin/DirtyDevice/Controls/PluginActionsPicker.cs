using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    internal partial class PluginActionsPicker : UserControl
    {
        public event EventHandler ConfigurationChanged;
        private bool AllowControlToRaiseItemChecks = true;

        public PluginActionsPicker()
        {
            InitializeComponent();

            PluginActionsCheckedListBox.DataSource =
                DirtyDeviceActions.AllActions
                .Select(enumVal => new { Key = enumVal, Value = "Dirty " + EnumUtil.GetDescription(enumVal) })
                .ToList();
            PluginActionsCheckedListBox.DisplayMember = "Value";
            PluginActionsCheckedListBox.ValueMember = "Key";

            fieldValidator.RequireCustom(
                PluginActionsCheckedListBox,
                () =>
                {
                    bool success = PluginActionsCheckedListBox.CheckedIndices.Count != 0;
                    if (!success)
                    {
                        return new ValidationResult(success, "One or more actions must be selected.");
                    }
                    return new ValidationResult(success);
                });


            PluginActionsCheckedListBox.ItemCheck += PluginActionsCheckedListBox_ItemCheck;
        }

        public DirtyDeviceActionFlags Value
        {
            get
            {
                var flags = DirtyDeviceActionFlags.None;
                for (var itemIndex = 0; itemIndex < PluginActionsCheckedListBox.Items.Count; itemIndex++)
                {
                    if (!PluginActionsCheckedListBox.GetItemChecked(itemIndex))
                    {
                        continue;
                    }

                    flags |= (DirtyDeviceActionFlags)(int)Math.Pow(2, itemIndex);
                }
                return flags;
            }
            set
            {
                AllowControlToRaiseItemChecks = false;
                for (var itemIndex = 0; itemIndex < PluginActionsCheckedListBox.Items.Count; itemIndex++)
                {
                    var isSet = ((int)value & (int)Math.Pow(2, itemIndex)) != 0;
                    PluginActionsCheckedListBox.SetItemChecked(itemIndex, isSet);
                }
                OnConfigurationChanged();
                AllowControlToRaiseItemChecks = true;
            }
        }

        public IEnumerable<ValidationResult> ValidateConfiguration()
        {
            return fieldValidator.ValidateAll();
        }

        private void AllButton_Click(object sender, EventArgs e)
        {
            Value = DirtyDeviceActionFlags.All;
            OnConfigurationChanged();
        }

        private void NoneButton_Click(object sender, EventArgs e)
        {
            Value = DirtyDeviceActionFlags.None;
            OnConfigurationChanged();
        }

        private void OnConfigurationChanged()
        {
            ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PluginActionsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!AllowControlToRaiseItemChecks)
            {
                return;
            }

            // Hack to work around the lack of an "ItemChecked" event.
            // This ensures that the event is not fired until *after* the value changes.
            BeginInvoke(new MethodInvoker(() => OnConfigurationChanged()));
        }
    }
}

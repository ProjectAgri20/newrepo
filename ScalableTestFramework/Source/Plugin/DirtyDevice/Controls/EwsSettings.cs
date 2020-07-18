using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.DirtyDevice.Controls
{
    internal partial class EwsSettings : UserControl
    {
        public event EventHandler ConfigurationChanged;

        public EwsSettings()
        {
            InitializeComponent();

            QuickSetNameTextBox.TextChanged += (s, e) => OnConfigurationChanged(s, e);

            foreach (var control in new Control[] { QuickSetNameTextBox })
            {
                fieldValidator.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
            }

            fieldValidator.RequireValue(QuickSetNameTextBox, "Quickset Name", ValidationCondition.IfEnabled);
        }

        public QuickSetActivityData Value
        {
            get
            {
                var activityData = new QuickSetActivityData()
                {
                    QuickSetTitle = QuickSetNameTextBox.Text,
                };
                return activityData;
            }
            set
            {
                QuickSetNameTextBox.Text = value.QuickSetTitle;
            }
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        public IEnumerable<ValidationResult> ValidateConfiguration()
        {
            return fieldValidator.ValidateAll();
        }
    }
}

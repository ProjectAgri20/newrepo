using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// User control that allows users to perform print queue configuration.
    /// </summary>
    internal partial class PrintServerUserControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintServerUserControl"/> class.
        /// </summary>
        public PrintServerUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates and returns a <see cref="PrintServerTabData" /> instance containing the
        /// PrintServer tab data from this control.
        /// </summary>
        /// <returns>The PrintServer data.</returns>
        public PrintServerTabData GetConfigurationData()
        {
            PrintServerTabData printserverdata = new PrintServerTabData();
            printserverdata.QueueName = queueName_TextBox.Text;
            printserverdata.Configuration = new List<HpacConfiguration>();
            if (tracking_CheckBox.Checked)
            {
                printserverdata.Configuration.Add(HpacConfiguration.Tracking);
            }
            if (quota_CheckBox.Checked)
            {
                printserverdata.Configuration.Add(HpacConfiguration.Quota);
            }
            if (ipm_Checkbox.Checked)
            {
                printserverdata.Configuration.Add(HpacConfiguration.IPM);
            }

            return printserverdata;
        }

        /// <summary>
        /// Configures the controls per the PrintServer data either derived from initialization or the saved meta data.
        /// </summary>
        public void LoadConfiguration(PrintServerTabData printserverdata)
        {
            ClearCheckBoxes(this);
            foreach (var checkedItemText in printserverdata.Configuration)
            {
                switch (checkedItemText)
                {
                    case HpacConfiguration.Tracking:
                        tracking_CheckBox.Checked = true;
                        break;

                    case HpacConfiguration.Quota:
                        quota_CheckBox.Checked = true;
                        break;
                    case HpacConfiguration.IPM:
                        ipm_Checkbox.Checked = true;
                        break;
                }
                queueName_TextBox.Text = printserverdata.QueueName;
            }
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult AddValidaton()
        {
            fieldValidator.RequireValue(queueName_TextBox, queueName_Label);
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Removes the Validation for the controls.
        /// </summary>
        public void RemoveValidation()
        {
            fieldValidator.Remove(queueName_TextBox);
        }

        private void ClearCheckBoxes(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
                ClearCheckBoxes(ctrl);
            }
        }
    }
}

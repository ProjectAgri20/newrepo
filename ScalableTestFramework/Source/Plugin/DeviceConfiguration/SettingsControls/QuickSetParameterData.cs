using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    public class QuickSetParameterData
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string ValidValues { get; set; }

        public QuickSetParameterData(string name, string value, string valid)
        {
            ParameterName = name;
            ParameterValue = value;
            ValidValues = valid;
        }
        public QuickSetParameterData(string name)
        {
            ParameterName = name;
            ParameterValue = string.Empty;
            ValidValues = string.Empty;
        }
    }
}

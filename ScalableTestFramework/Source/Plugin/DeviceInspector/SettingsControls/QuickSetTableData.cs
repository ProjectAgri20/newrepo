using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsControls
{
    public class QuickSetTableData
    {
        public string QuickSetName { get; set; }
        public string QuickSetType { get; set; }

        public QuickSetTableData(string name, string type)
        {
            QuickSetName = name;
            QuickSetType = type;
        }
    }
}

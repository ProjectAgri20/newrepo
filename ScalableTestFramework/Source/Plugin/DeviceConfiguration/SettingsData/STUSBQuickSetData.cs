using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// Scan To USB Quick Set info. Contains scan settings and file settings
    /// </summary>
    [DataContract]
    public class STUSBQuickSetData : AQuickSet
    {
        public STUSBQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();

        }

        public STUSBQuickSetData(string name, string type, ScanSettings data, FileSettings fileSettingsData)
        {
            ScanSetData = data;
            QName = name;
            Name = name;
            QType = type;
            FileSetData = fileSettingsData;
        }

        /// <summary>
        /// Nothing to validate, returns true
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static bool Validate(List<QuickSetParameterData> arguments)
        {
            //bool valid = true;

            return true;
        }
    }
}

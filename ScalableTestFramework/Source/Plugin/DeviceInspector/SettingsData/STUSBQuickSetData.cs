using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
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

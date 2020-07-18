using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsControls;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    [DataContract]
    public class CopyQuickSetData : AQuickSet
    {
        [DataMember]
        public string Copies { get; set; }

        public CopyQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();
            Copies = "1";
        }

        public CopyQuickSetData(string copies, string name, string type, ScanSettings scanData, FileSettings fileSettingsData)
        {
            Copies = copies;
            ScanSetData = scanData;
            FileSetData = fileSettingsData;
            QName = name;
            Name = name;
            QType = type;
        }

        public static bool Validate(List<QuickSetParameterData> arguments)
        {
            bool valid = true;

            string copies = arguments[0].ParameterValue;

            int value;
            bool success = int.TryParse(copies, out value);

            if (!success || value <= 0)
            {
                valid = false;
            }

            return valid;
        }
    }
}
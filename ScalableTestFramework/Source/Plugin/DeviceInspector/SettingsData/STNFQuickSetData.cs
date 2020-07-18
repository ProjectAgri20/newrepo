using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsControls;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    [DataContract]
    public class STNFQuickSetData : AQuickSet
    {
        [DataMember]
        public string FolderPath { get; set; }

        public STNFQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();
            FolderPath = string.Empty;
        }

        public STNFQuickSetData(string path, string name, string type, ScanSettings data, FileSettings fileSettingsData)
        {
            ScanSetData = data;
            FileSetData = fileSettingsData;
            FolderPath = path;
            QName = name;
            Name = name;
            QType = type;
        }

        public static bool Validate(List<QuickSetParameterData> arguments)
        {
            bool valid = false;

            string path = arguments[0].ParameterValue;

            Regex regex = new Regex("\\\\([a-zA-Z]+[.]?)*(\\[a-zA-Z]+)*");
            if (!string.IsNullOrWhiteSpace(path) && regex.IsMatch(path))
            {
                valid = true;
            }

            return valid;
        }
    }
}
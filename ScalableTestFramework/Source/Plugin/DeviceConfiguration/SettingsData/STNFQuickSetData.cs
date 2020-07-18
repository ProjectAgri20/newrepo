using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// Scan To Network Folder Quick Set info. Contains scan settings and file settings
    /// </summary>
    [DataContract]
    public class STNFQuickSetData : AQuickSet
    {
        [DataMember]
        public List<string> FolderPaths { get; set; }

        public STNFQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();
            FolderPaths = new List<string>();
        }

        public STNFQuickSetData(List<string> path, string name, string type, ScanSettings data, FileSettings fileSettingsData)
        {
            ScanSetData = data;
            FileSetData = fileSettingsData;
            FolderPaths = path;
            QName = name;
            Name = name;
            QType = type;
        }

        public static bool Validate(List<QuickSetParameterData> arguments)
        {
            bool valid = true;

            //string path = arguments[0].ParameterValue;

            foreach (var item in arguments)
            {
                if (valid)
                {
                    string path = item.ParameterValue;

                    Regex regex = new Regex("\\\\([a-zA-Z]+[.]?)*(\\[a-zA-Z]+)*");
                    if (string.IsNullOrWhiteSpace(path) && regex.IsMatch(path))
                    {
                        valid = false;
                    }
                }
            }


            return valid;
        }
    }
}
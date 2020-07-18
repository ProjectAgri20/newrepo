using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// STSharePoint Quick Set info. Contains scan settings and file settings
    /// </summary>
    [DataContract]
    public class STSharePointQuickSetData : AQuickSet
    {
        [DataMember]
        public string FolderPath { get; set; }

        public STSharePointQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FolderPath = string.Empty;
        }

        public STSharePointQuickSetData(string path, string name, string type, ScanSettings data, FileSettings fileSettingsData)
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

            if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                valid = true;
            }

            return valid;
        }
    }
}
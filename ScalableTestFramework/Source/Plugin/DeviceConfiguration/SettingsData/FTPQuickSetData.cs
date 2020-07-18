using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// FTP Quick Set info. Contains scan settings and file settings
    /// </summary>
    [DataContract]
    public class FTPQuickSetData : AQuickSet
    {
        [DataMember]
        public string FTPServer { get; set; }

        [DataMember]
        public string PortNumber { get; set; }

        [DataMember]
        public string DirectoryPath { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FTPProtocol { get; set; }

        public FTPQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();
        }

        public FTPQuickSetData(List<string> server, string name, string type, ScanSettings data, FileSettings fileSettingsData)
        {
            ScanSetData = data;
            FileSetData = fileSettingsData;
            FTPServer = server[0];
            PortNumber = server[1];
            DirectoryPath = server[2];
            UserName = server[3];
            FTPProtocol = server[4];
            QName = name;
            QType = type;
            Name = name;
        }

        public static bool Validate(List<QuickSetParameterData> arguments)
        {
            bool valid = true;
            string ftpServer = arguments[0].ParameterValue;
            string port = arguments[1].ParameterValue;
            int value;
            bool success = int.TryParse(port, out value);

            if (!success || value <= 0)
            {
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(ftpServer) || !valid)
            {
                valid = false;
            }

            return valid;
        }
    }
}

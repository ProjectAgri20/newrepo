using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// Copy Quick Set info. Contains scan settings and file settings
    /// </summary>
    [DataContract]
    public class CopyQuickSetData : AQuickSet
    {
        /// <summary>
        /// Number of copies to set on the quickset.
        /// </summary>
        [DataMember]
        public string Copies { get; set; }

        public CopyQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();
            Copies = "1";
        }
        /// <summary>
        /// Initializes an instance of CopyQuickSetData.
        /// </summary>
        /// <param name="copies"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="scanData"></param>
        /// <param name="fileSettingsData"></param>
        public CopyQuickSetData(string copies, string name, string type, ScanSettings scanData, FileSettings fileSettingsData)
        {
            Copies = copies;
            ScanSetData = scanData;
            FileSetData = fileSettingsData;
            QName = name;
            Name = name;
            QType = type;
        }
        /// <summary>
        /// Validates that entry data is correct.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>bool success</returns>
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
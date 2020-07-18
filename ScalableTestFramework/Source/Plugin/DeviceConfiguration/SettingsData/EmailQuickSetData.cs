using System.Collections.Generic;
using System.Runtime.Serialization;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData
{
    /// <summary>
    /// Email Quick Set info. Contains scan settings and file settings
    /// </summary>
    [DataContract]
    public class EmailQuickSetData : AQuickSet
    {
        /// <summary>
        /// Default Email From User
        /// </summary>
        [DataMember]
        public string FromUser { get; set; }
        /// <summary>
        /// Default From Email
        /// </summary>
        [DataMember]
        public string DefaultFrom { get; set; }
        /// <summary>
        /// Default To Field for Quickset.
        /// </summary>
        [DataMember]
        public string To { get; set; }

        public EmailQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();
           
            FromUser = "";
            DefaultFrom = "";
            To = "";
        }
        /// <summary>
        /// Initializes Instance of EmailQuickSetData.
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="defaultFrom"></param>
        /// <param name="to"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="fileSettingsData"></param>
        public EmailQuickSetData(string fromUser, string defaultFrom, string to, string name, string type, ScanSettings data, FileSettings fileSettingsData)
        {
            ScanSetData = data;
            FileSetData = fileSettingsData;
            FromUser = fromUser;
            DefaultFrom = defaultFrom;
            To = to;
            QName = name;
            QType = type;
            Name = name;
        }
        /// <summary>
        /// Validates that entry data is correct.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>bool success</returns>
        public static bool Validate(List<QuickSetParameterData> arguments)
        {
            bool valid = true;

            string to = arguments[0].ParameterValue;
            string defaultFrom = arguments[1].ParameterValue;
            string fromUser = arguments[2].ParameterValue;

            if (to != "Default From:" && to != "User's Address")
            {
                valid = false;
            }

            if ((defaultFrom != "Blank" && defaultFrom != "User's Address") || !valid)
            {
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(fromUser) || !valid)
            {
                valid = false;
            }
            


            return valid;
        }







    }
}

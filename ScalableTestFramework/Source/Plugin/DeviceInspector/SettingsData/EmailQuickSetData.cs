using System.Collections.Generic;
using System.Runtime.Serialization;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsControls;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    [DataContract]
    public class EmailQuickSetData : AQuickSet
    {

        [DataMember]
        public string FromUser { get; set; }
        [DataMember]
        public string DefaultFrom { get; set; }
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

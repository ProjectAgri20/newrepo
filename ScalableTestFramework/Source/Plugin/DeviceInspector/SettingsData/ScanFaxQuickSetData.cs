using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    [DataContract]
    public class ScanFaxQuickSetData : AQuickSet
    {
        [DataMember]
        public string Number { get; set; }


        public ScanFaxQuickSetData()
        {
            ScanSetData = new ScanSettings();
            FileSetData = new FileSettings();
            Number = string.Empty;

        }

        public ScanFaxQuickSetData(string number, string name, string type, ScanSettings data, FileSettings fileSettingsData)
        {
            ScanSetData = data;
            FileSetData = fileSettingsData;
            Number = number;
            QName = name;
            Name = name;
            QType = type;
        }


        public static bool Validate(List<QuickSetParameterData> arguments)
        {
            bool valid = false;

            string phone = arguments[0].ParameterValue;


            Regex regex = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            if (regex.IsMatch(phone))
            {
                valid = true;
            }

            return valid;
        }


    }
}

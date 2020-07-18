using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsData
{
    public class PrintSettingsData : IComponentData
    {
        [DataMember]
        public DataPair<string> PrintFromUsb { get; set; }

        [DataMember]
        public DataPair<string> Copies { get; set; }

        [DataMember]
        public DataPair<string> OriginalSize { get; set; }

        [DataMember]
        public DataPair<string> PaperType { get; set; }

        [DataMember]
        public DataPair<string> PaperTray { get; set; }

        [DataMember]
        public DataPair<string> OutputBin { get; set; }

        [DataMember]
        public DataPair<string> OutputSides { get; set; }

        [DataMember]
        public DataPair<string> Resolution { get; set; }

        private StringBuilder _failedSettings = new StringBuilder();

        public PrintSettingsData()
        {
            PrintFromUsb = new DataPair<string> { Key = string.Empty };
            Copies = new DataPair<string> { Key = String.Empty };
            OriginalSize = new DataPair<string> { Key = String.Empty };

            PaperType = new DataPair<string> { Key = String.Empty };
            PaperTray = new DataPair<string> { Key = String.Empty };

            OutputSides = new DataPair<string> { Key = String.Empty };
            OutputBin = new DataPair<string> { Key = String.Empty };

            Resolution = new DataPair<string> { Key = String.Empty };
        }

        public bool GetFields(JediDevice device)
        {
            Type type = typeof(PrintSettingsData);
            bool result = true;

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                properties.Add(prop.Name, prop.GetValue(this));
            }

            foreach (var item in properties)
            {
                switch (item.Key)
                {
                    case "PrintFromUsb":
                        result &= SetUsbDefaultValues("PrintFromUSBMassStorageEnabled", "Print From USB", (DataPair<string>)item.Value, device);
                        break;

                    case "Copies":
                        result &= SetPrintDefaultValues("DefaultPrintCopies", "Default Print Copies", (DataPair<string>)item.Value, device);
                        break;

                    case "OriginalSize":
                        result &= SetPrintDefaultValues("MediaSizeID", "Original Size", (DataPair<string>)item.Value, device);
                        break;

                    case "PaperType":
                        result &= SetPrintDefaultValues("MediaType", "Paper Type", (DataPair<string>)item.Value, device);
                        break;

                    case "PaperTray":
                        result &= SetPrintDefaultValues("ManualFeed", "Paper Tray", (DataPair<string>)item.Value, device);
                        break;

                    case "OutputBin":
                        result &= SetPrintDefaultValues("DefaultOutputBin", "Output Bin", (DataPair<string>)item.Value, device);
                        break;

                    case "OutputSides":
                        result &= SetPrintDefaultValues("Plex", "Output Sides", (DataPair<string>)item.Value, device);
                        break;

                    case "Resolution":
                        result &= SetPrintDefaultValues("ResolutionSetting", "Resolution", (DataPair<string>)item.Value, device);
                        break;
                }
            }

            return result;
        }

        private bool SetPrintDefaultValues(string elementName, string activityName, DataPair<string> dataPair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:print:PrintService:DefaultJob";
            string endpoint = "print";

            Func<WebServiceTicket, bool> getProperty = n =>n.FindElement(elementName).Value.Equals(dataPair.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, dataPair, activityUrn, endpoint, activityName);
        }

        private bool SetUsbDefaultValues(string elementName, string activityName, DataPair<string> dataPair, JediDevice device)
        {
            string activityUrn = "urn:hp:imaging:con:service:openfromusb:OpenFromUsbService";
            string endpoint = "openfromusb";

            Func<WebServiceTicket, bool> getProperty = n => n.FindElement(elementName).Value.Equals(dataPair.Key, StringComparison.OrdinalIgnoreCase);

            return UpdateField(getProperty, device, dataPair, activityUrn, endpoint, activityName);
        }

        public bool UpdateField<T>(Func<WebServiceTicket, bool> getProperty, JediDevice device, DataPair<T> data, string urn, string endpoint, string activityName)
        {
            bool success;
            if (data.Value)
            {
                try
                {
                    WebServiceTicket tic = device.WebServices.GetDeviceTicket(endpoint, urn);
                    success = getProperty(tic);
                }
                catch
                {
                    Logger.LogError($"Failed to set field {activityName}");
                    success = false;
                }
            }
            else
            {
                success = true;
            }
            if (!success)
                _failedSettings.Append($"{activityName}, ");

            return success;
        }

        public DataPair<string> VerifyFields(JediDevice device)
        {
            _failedSettings = new StringBuilder();
            var result = GetFields(device);

            return new DataPair<string> { Key = _failedSettings.ToString(), Value = result };
        }
    }
}
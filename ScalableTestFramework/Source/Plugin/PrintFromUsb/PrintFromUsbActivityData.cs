using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.PrintFromUsb
{
    public class PrintFromUsbActivityData
    {
        /// <summary>
        /// The firmware file to flash
        /// </summary>

        [DataMember]
        public string UsbName { get; set; }


        /// <summary>
        /// Initializes a new instance of the FlashFirmwareActivityData class. 
        /// </summary>
        public PrintFromUsbActivityData()
        {
            UsbName = string.Empty;
        }
    }
}

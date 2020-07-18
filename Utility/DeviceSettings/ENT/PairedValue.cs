
namespace HP.RDL.STF.DeviceSettings
{
    public class PairedValue
    {
        /// <summary>
        /// Contains the setting name as well as its value as retrieved from the device.
        /// </summary>
        public string IdentName { get; set; }
        public string IdentValue { get; set; }

        public PairedValue()
        {
            IdentName = string.Empty;
            IdentValue = string.Empty;
        }
        public override string ToString()
        {
            return IdentName;
        }

    }
}

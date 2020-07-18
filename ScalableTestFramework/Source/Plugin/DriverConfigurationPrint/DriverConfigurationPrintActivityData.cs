using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.Print;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint
{
    /// <summary>
    /// Contains data needed to execute a printer preference activity through the Driver Configuration Print plugin.
    /// </summary>
    [DataContract]
    public class DriverConfigurationPrintActivityData : PrintingActivityData
    {
        /// <summary>
        /// Gets or Sets the Printer Preference Data. This Object is used for Printer Preference Data movement.
        /// </summary>
        [DataMember]
        public PrinterPreferenceData PrinterPreference { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverConfigurationPrintActivityData"/> class
        /// </summary>
        public DriverConfigurationPrintActivityData()
        {
            PrinterPreference = new PrinterPreferenceData();
        }
    }
}

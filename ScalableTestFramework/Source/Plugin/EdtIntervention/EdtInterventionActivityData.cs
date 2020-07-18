using System.ComponentModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.EdtIntervention
{
    /// <summary>
    /// Holds the Metadata  between Configuration and Execution Control
    /// </summary>
    [DataContract]
    public class EdtInterventionActivityData
    {
        /// <summary>
        /// The alert message to be displayed
        /// </summary>
        [DataMember]
        public string AlertMessage { get; set; }
      
        /// <summary>
        /// The test type Operation Reliability or PowerBoot
        /// </summary>
        [DataMember]
        public EdtTestType TestType { get; set; }

        /// <summary>
        /// The Test Method to be used
        /// </summary>
        [DataMember]
        public string TestMethod { get; set; }

        /// <summary>
        /// The wake up method as described in DDS
        /// </summary>
        [DataMember]
        public string WakeMethod { get; set; }

        
        /// <summary>
        /// Intialize the new instance of the UserInterventionActivityData class
        /// </summary>
        public EdtInterventionActivityData()
        {
            AlertMessage = string.Empty;
            TestType = EdtTestType.None;
            TestMethod = string.Empty;
            WakeMethod = string.Empty;
        }
    }

    /// <summary>
    /// EDT Test Types
    /// </summary>
    public enum EdtTestType
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,
        /// <summary>
        /// Power Boot test
        /// </summary>
        [Description("PowerBoot")]
        PowerBoot,
        /// <summary>
        /// Operation Reliability
        /// </summary>
        [Description("ExtDuration")]
        OperationReliability,
        [Description("FIM")]
        FIM

    }

    /// <summary>
    /// Boot Methods for Power Boot Tests
    /// </summary>
    public enum BootMethod
    {
        /// <summary>
        /// Describes method of shutting down by holding the power button
        /// </summary>
        [Description("Forced Shutdown")]
        Hard,
        /// <summary>
        /// Describes the method of shutting down by pulling power chord
        /// </summary>
        [Description("Loss of Power")]
        Dirty,
        /// <summary>
        /// Describes the method of shutting down by pressing power button once
        /// </summary>
        [Description("Power Button")]
        Soft
    }

    /// <summary>
    /// Sleep Wake Test Methods
    /// </summary>
    public enum SleepWakeMethod
    {
        /// <summary>
        /// Almost 1W method
        /// </summary>
        [Description("A1W Test")]
        Sleep,
        /// <summary>
        /// Waking the device by automated jobs or manual wake up events
        /// </summary>
        [Description("Automated/Manual Wakeup")]
        Wake
    }

    /// <summary>
    /// Wake up methods as described in DDS
    /// </summary>
    public enum WakeMethod
    {
        /// <summary>
        /// Swipe the card
        /// </summary>
        [Description("Card Swipe")]
        CardSwipe,
        /// <summary>
        /// Insert the USB stick
        /// </summary>
        [Description("USB Inserted")]
        USBInserted,
        /// <summary>
        /// Remove the inserted USB stick
        /// </summary>
        [Description("USB Removed")]
        USBRemoved,
        /// <summary>
        /// Open the flat bed cover
        /// </summary>
        [Description("Open Flatbed")]
        OpenFlatbed,
        /// <summary>
        /// Open or close the jam door
        /// </summary>
        [Description("Open/Close Jam Door")]
        OpenCloseJamDoor,
        /// <summary>
        /// open or close the cartridge door
        /// </summary>
        [Description("Open/Close Cartridge Door")]
        OpenCloseCartridgeDoor,
        /// <summary>
        /// open or close the media tray
        /// </summary>
        [Description("Open/Close Media Tray")]
        OpenCloseMediaTray,
        /// <summary>
        /// press any physical button on control panel
        /// </summary>
        [Description("Control Panel Button")]
        ControlPanelButton,
        /// <summary>
        /// press any key on the physical keyboard
        /// </summary>
        [Description("Physical Keyboard Press")]
        PhysicalKeyboardPress,
        /// <summary>
        /// touch the screen
        /// </summary>
        [Description("Touch Screen")]
        TouchScreen,
        /// <summary>
        /// Insert media in ADF
        /// </summary>
        [Description("Insert Media in ADF")]
        InsertMediaInADF,
        /// <summary>
        /// Insert media in tray 1
        /// </summary>
        [Description("Insert Media in Tray 1")]
        InsertMediaInTray1,
        /// <summary>
        /// Send a print job
        /// </summary>
        [Description("Print Job")]
        PrintJob,
        /// <summary>
        /// Trigger network traffic on to the device
        /// </summary>
        [Description("Network Traffic")]
        NetworkTarrif,
        /// <summary>
        /// Send a fax to the device
        /// </summary>
        [Description("Incoming Analog Fax")]
        IncomingAnalogFax,
        /// <summary>
        /// Trigger any NFC event
        /// </summary>
        [Description("NFC Event")]
        NFCEvent
    }


}

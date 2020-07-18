using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using HP.RDL.DDSContants;
using HP.ScalableTest.Utility;
// ReSharper disable InconsistentNaming

namespace HP.RDL.EDT.AutoTestHelper.DDS
{
    class FaultEventConstants
    {
        /// <summary>
        /// Returns a list of the defined string values for the fault event types.
        /// Error or Jam
        /// </summary>
        /// <returns>List of strings</returns>
        public static List<string> Events => EnumUtil.GetDescriptions<ErrorEventTypes>().ToList();
          

        /// <summary>
        /// Returns a list of the defined string values for the fault sub error type enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> ErrorTypes => EnumUtil.GetDescriptions<ErrorSubevents>().ToList();

        /// <summary>
        /// Returns a list of the defined string values for the fault sub jam type enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> JamTypes => EnumUtil.GetDescriptions<JamSubevents>().ToList();

        /// <summary>
        /// Returns a list of the defined string values for the fault operation in progress enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> Operations => EnumUtil.GetDescriptions<FaultOpInProgress>().ToList();

        /// <summary>
        /// Returns a list of the defined string values for the fault recovery error enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> ErrorRecoveries => EnumUtil.GetDescriptions<ErrorEventRecoveries>().ToList();

        /// <summary>
        /// Returns a list of the defined string values for the fault recovery jam enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> JamRecoveries => EnumUtil.GetDescriptions<JamRecoveries>().ToList();

        /// <summary>
        /// Returns a list of the defined string values for the job disposition type enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> JobDispositions => EnumUtil.GetDescriptions<FaultJobDispositions>().ToList();

        /// <summary>
        /// Returns a list of the defined string values for the fault hang type enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> HangTypes => EnumUtil.GetDescriptions<ErrorEventHangs>().ToList();

        /// <summary>
        /// Returns a list of the defined string values for the fault hang type enumerations.
        /// </summary>
        /// <returns>List of Strings</returns>
        public static List< string> RootCauses => EnumUtil.GetDescriptions<FaultRootCauses>().ToList();

        public static Guid GetEventTypeId(string value)
        {
            return new Guid(Enum<ErrorEventTypes>.Value(EnumUtil.GetByDescription<ErrorEventTypes>(value)));
        }

        public static Guid GetEventSubtypeId(ErrorEventTypes eventType, string value)
        {
            return eventType == ErrorEventTypes.Error ? new Guid(Enum<ErrorSubevents>.Value(EnumUtil.GetByDescription<ErrorSubevents>(value))) : new Guid(Enum<JamSubevents>.Value(EnumUtil.GetByDescription<JamSubevents>(value)));
        }

        public static Guid GetOperationInProgressId(string value)
        {
            return new Guid(Enum<FaultOpInProgress>.Value(EnumUtil.GetByDescription<FaultOpInProgress>(value)));
        }

        public static Guid GetRecoveryId(ErrorEventTypes eventType, string value)
        {
            if (eventType == ErrorEventTypes.Error)
            {
                return new Guid(Enum<ErrorEventRecoveries>.Value(EnumUtil.GetByDescription<ErrorEventRecoveries>(value)));
            }
            else
            {
                return new Guid(Enum<JamRecoveries>.Value(EnumUtil.GetByDescription<JamRecoveries>(value)));
            }
        }

        public static Guid GetJobDispositionId(string value)
        {
            return new Guid(Enum<FaultJobDispositions>.Value(EnumUtil.GetByDescription<FaultJobDispositions>(value)));
        }

        public static Guid GetRootCauseId(string value)
        {
            return new Guid(Enum<FaultRootCauses>.Value(EnumUtil.GetByDescription<FaultRootCauses>(value)));
        }
    }

    public enum ErrorEventTypes
    {
        [EnumValue("None")]
        [Description("None")]
        // ReSharper disable once InconsistentNaming
        aNone = 0,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFB6")]
        [Description("Error")]
        Error = 1,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFC2")]
        [Description("Jam")]
        Jam = 2
    };

    public enum ErrorSubevents
    {
      
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF93")]
        [Description("Hang")]
        Hang,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF92")]
        [Description("Crash")]
        Crash,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF94")]
        [Description("Orange Screen")]
        OrangeScreen,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFD9")]
        [Description("Auto Reboot")]
        AutoReboot,
        [EnumValue("CF9B9F90-DA7E-44E3-B5AE-E5AA1340EA51")]
        [Description("Communication Issue")]
        CommunicationIssue,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF95")]
        [Description("Incorrect Behavior")]
        IncorrectBehavior,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFE0")]
        [Description("Incorrect Output")]
        IncorrectOutput,
        [EnumValue("DBBC9823-6346-41DA-915C-2104A8893832")]
        [Description("Missing Job/Content")]
        MissingContent,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFE3")]
        [Description("Operator Error")]
        OperatorError,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFE1")]
        [Description("Sluggishness")]
        Sluggishness,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFE2")]
        [Description("Test Error")]
        TestError
    };

    public enum JamSubevents
    {
        [EnumValue("None")]
        [Description("None")]
        aNone = 0,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF99")]
        [Description("ADF Jam")]
        ADF = 1,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF9C")]
        [Description("Finisher Jam")]
        Finisher = 2,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF9B")]
        [Description("Fuser Jam")]
        Fuser = 3,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF9A")]
        [Description("Tray 1 Jam")]
        Tray1Jam = 4,
        [EnumValue("AFE865AA-BE2C-46B7-B3C3-D3E2164773E3")]
        [Description("Tray 1 Mispick")]
        Tray1Mispick = 5,
        [EnumValue("93596115-013E-491F-BBD6-46573FF8B80E")]
        [Description("Tray 2 Jam")]
        Tray2Jam = 6,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF9D")]
        [Description("Tray 2 Mispick")]
        Tray2Mispick = 7,
        [EnumValue("A8D91543-530F-4EBB-84DB-427F4D59A231")]
        [Description("Tray 3 Jam")]
        Tray3Jam = 8,
        [EnumValue("29009728-E3F6-481C-9FF9-46FD97688CB2")]
        [Description("Tray 3 Mispick")]
        Tray3Mispick = 9
    };

    public enum FaultOpInProgress
    {
      
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC011")]
        [Description("Authorization")]
        Authorization,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC01D")]
        [Description("Backup")]
        Backup,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC01A")]
        [Description("Booting")]
        Booting,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC013")]
        [Description("Button Press")]
        ButtonPress,
        [EnumValue("A1B0030E-1A21-4495-BDDE-5593EB6F9760")]
        [Description("Captain Mandrake")]
        CaptainMandrake,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC022")]
        [Description("Cleaning ADF")]
        CleaningADF,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC021")]
        [Description("Cleaning Paper Path")]
        CleanPaperPath,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC023")]
        [Description("Cleaning Print Heads")]
        CleanPrintHeads,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC015")]
        [Description("Closing Door")]
        ClosingDoor,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC00B")]
        [Description("Copying")]
        Copying,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFC0")]
        [Description("Digital Send Job Data Entry")]
        DSJDE,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC01C")]
        [Description("Firmware Upgrade")]
        FIM,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFBE")]
        [Description("Going to Sleep")]
        GoingToSleep,
        [EnumValue("ACDB39F2-959B-44E9-A976-02F695DCD55C")]
        [Description("Interface Testing")]
        IFaceTesting,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC016")]
        [Description("Loading ADF")]
        LoadingADF,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC017")]
        [Description("Loading Media")]
        LoadingMedia,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC01F")]
        [Description("Maintenance Event")]
        MaintenanceEvent,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC012")]
        [Description("Menu Navigation")]
        MenuNavigation,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC014")]
        [Description("Opening Door")]
        OpeningDoor,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC01B")]
        [Description("Powering Off")]
        PoweringOff,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFBD")]
        [Description("Printing")]
        Printing,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC00D")]
        [Description("Reading from USB")]
        ReadUSB,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC00F")]
        [Description("Receiving Fax")]
        ReceivingFax,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC018")]
        [Description("Removing Media")]
        RemovingMedia,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC01E")]
        [Description("Restore")]
        Restore,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC020")]
        [Description("Running Diagnostics")]
        RunDiagnostics,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFBC")]
        [Description("Scanning")]
        Scanning,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC00E")]
        [Description("Sending FAX")]
        SendingFAX,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC019")]
        [Description("Sleeping")]
        Sleeping,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC024")]
        [Description("Unknown")]
        Unknown,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFBF")]
        [Description("Waking From Sleep")]
        WakingFromSleep,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC010")]
        [Description("Writing to Network")]
        WritingToNetwork,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CC00C")]
        [Description("Writing to USB")]
        WritingToUSB
    };

    public enum ErrorEventRecoveries
    {
        [EnumValue("4F217F12-11E1-4038-9D20-0A5BD4948D65")]
        [Description("Service Event - Break/Fix Part Replaced")]
        PartReplaced,
        [EnumValue("F175F663-DB9C-4AB4-8284-1BB1E0AC561A")]
        [Description("Service Event - No Parts Adjust")]
        PartAdjust,
        [EnumValue("B7A7003E-71AD-4B29-B1C0-268716E9C05F")]
        [Description("Service Event - No Parts Non-PM Clean")]
        PartNonPmClean,
        [EnumValue("CA1EDF0D-5993-4AE0-90A5-3F1F2F510215")]
        [Description("Service Event - FW/SW upgrade/reload")]
        SoftwareReload,
        [EnumValue("A87CC1FF-4E04-47F7-B206-6C998427C005")]
        [Description("Service Event - No Parts Jam Event")]
        PartsJam,
        [EnumValue("AC776BAB-9064-477F-B268-DAC0F90BE233")]
        [Description("Service Event - No Parts PM Clean")]
        PartsPmClean,
        [EnumValue("88D0F540-7EA1-4D02-AFCC-DD4329D6AA02")]
        [Description("Service Event - PM Part Replaced")]
        PartsReplaced,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFF7")]
        [Description("Auto Recovered")]
        AutoRecovery,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF86")]
        [Description("Clean Reboot")]
        CleanReboot,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF87")]
        [Description("Dirty Reboot")]
        DirtyReboot,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF79")]
        [Description("Error")]
        Error,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFF9")]
        [Description("Reload Firmware")]
        FimLoad,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFC1")]
        [Description("Held for Triage")]
        HeldTriage,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF81")]
        [Description("Job Cancel")]
        JobCancel,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF78")]
        [Description("Job Reset")]
        JobReset,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFF8")]
        [Description("Manually Recovered")]
        ManuallyRecovered,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF88")]
        [Description("Power Cycle")]
        PowerCycle,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF80")]
        [Description("System Reset")]
        SystemReset
    };

    public enum JamRecoveries
    {
        [EnumValue("None")]
        [Description("None")]
        aNone = 0,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF96")]
        [Description("Cleared - Help accurate")]
        ClearedHelpAccurate = 1,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFFA")]
        [Description("Cleared - Help inaccurate")]
        ClearedHelpInAccurate = 2,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFFA")]
        [Description("Cleared - No Help")]
        ClearedNoHelp = 3,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFFC")]
        [Description("Error Event")]
        ErrorEvent = 4,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFFF")]
        [Description("Firmware Load Event")]
        FimLdEvt = 5,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFFD")]
        [Description("Jam Event")]
        JamEvent = 6,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFFB")]
        [Description("Not Cleared")]
        NotCleared = 7,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF98")]
        [Description("Power Cycle")]
        PowerCycle = 8,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFFE")]
        [Description("Sleep/Wake Event")]
        SWEvent = 9
    };

    public enum FaultJobDispositions
    {
        [EnumValue("None")]
        [Description("None")]
        aNone = 0,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFE6")]
        [Description("Error")]
        Error,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF91")]
        [Description("Job Lost")]
        JobLost,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF90")]
        [Description("Job Restarted")]
        JobRestarted,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFE4")]
        [Description("Recovered and Completed")]
        RecoveredCompleted,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBF89")]
        [Description("Successful")]
        Successful,
        [EnumValue("33E7CD86-0731-4CCB-BFCF-FC20B89CBFE5")]
        [Description("Warning")]
        Warning,
    };

    public enum ErrorEventHangs
    {
        [EnumValue("None")]
        [Description("None")]
        aNone = 0,
        [EnumValue("14F23FB9-9C2F-41FF-82B8-49A1647C6F64")]
        [Description("Email")]
        Email,
        [EnumValue("DD10C3E9-D1F8-462A-8CD4-6272D7E2C1F1")]
        [Description("Fax Receiving")]
        FaxReceiving,
        [EnumValue("1092E6A7-7FC7-404A-93D6-35AD24C04495")]
        [Description("Fax Sending")]
        FaxSending,
        [EnumValue("563D59F6-AE25-4106-BE53-FB94411F8946")]
        [Description("Network Folder")]
        NetworkFolder,
        [EnumValue("9453A0DD-97B7-4837-A5E9-1002AA17A7FB")]
        [Description("Processing Copy")]
        ProcessingCopy,
        [EnumValue("E5DEDE63-0F7C-4ACF-8FAD-42AD9487DE71")]
        [Description("Processing Job")]
        ProcessingJob,
        [EnumValue("6113599E-06C2-4D74-A870-4A630B10D451")]
        [Description("Processing...")]
        Processing,
        [EnumValue("DE4F1D5F-6B90-4503-AAEF-2E60F91F4483")]
        [Description("Ready")]
        Ready,
        [EnumValue("4D833A33-D1DE-4E51-988C-31796BA6CF9F")]
        [Description("Scanning")]
        Scanning
    };

    public enum FaultRootCauses
    {
       
        [EnumValue("8CAF7E13-3B69-46F3-B3DE-143051B04A7E")]
        [Description("Firmware-HL")]
        FirmwareHL,
        [EnumValue("9D54D18C-F414-4E67-84BD-713AF793ACA3")]
        [Description("Firmware-NL")]
        FirmwareNL,
        [EnumValue("837343C5-1056-4F75-BB58-196CCEBF12A6")]
        [Description("Firmware-SL")]
        FirmwareSL,
        [EnumValue("BA37A360-BD31-4923-A012-A99EDA59DA10")]
        [Description("Hardware")]
        Hardware,
        [EnumValue("5C5C9F0B-E4FE-44EA-A8B3-A0B8CDA2A3F5")]
        [Description("Software-NL")]
        SoftwareNL,
        [EnumValue("CA4266C2-DA75-4D44-9BAE-46F6DD21818F")]
        [Description("Software-SL")]
        SoftwareSL,
        [EnumValue("5FF3BEDF-9277-4520-98D6-E0695204F311")]
        [Description("Tester")]
        Tester,
        [EnumValue("06E07730-2DC8-4C5D-8F4F-0787639B7CC8")]
        [Description("TestScript")]
        TestScript

    }

    

}

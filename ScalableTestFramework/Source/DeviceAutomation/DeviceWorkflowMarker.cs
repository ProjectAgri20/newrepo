using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>Enumerations for marking when the printer list is ready for use </summary>
    public enum DeviceWorkflowMarker
    {
        /// <summary>
        /// Marks when the solution activity begins
        /// Usage: RecordEvent(DeviceWorkflowMarker.ActivityBegin);
        /// </summary>
        [Description("ActivityBegin")]
        ActivityBegin,

        /// <summary>
        /// Marks when the solution activity ends
        /// Usage: RecordEvent(DeviceWorkflowMarker.ActivityEnd);
        /// </summary>
        [Description("ActivityEnd")]
        ActivityEnd,

        /// <summary>
        /// Marks the time of Application/Solution Button Press.
        /// Usage: RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Hpcr - Scan to Me");
        /// </summary>
        [Description("AppButtonPress")]
        AppButtonPress,

        /// <summary>
        /// Marks the time the application/solution screen is shown.
        /// Usage: RecordEvent(DeviceWorkflowMarker.AppShown);
        /// </summary>
        [Description("AppShown")]
        AppShown,

        /// <summary>
        /// Marks the time Authentication begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
        /// </summary>
        [Description("AuthenticationBegin")]
        AuthenticationBegin,

        /// <summary>
        /// Marks the time Authentication ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
        /// </summary>
        [Description("AuthenticationEnd")]
        AuthenticationEnd,

        /// <summary>
        /// The authentication type utilized to sign into a solution.
        /// Usage: RecordInfo(DeviceWorkflowMarker.AuthType, "Windows");
        /// </summary>
        [Description("AuthType")]
        AuthType,

        /// <summary>
        /// Marks when a calibration begin is detected.
        /// </summary>
        [Description("CalibrationBegin")]
        CalibrationBegin,

        /// <summary>
        /// Marks when a the calibration end is detected.
        /// </summary>
        [Description("CalibrationEnd")]
        CalibrationEnd,

        /// <summary>
        /// Marks the time when a solution begins deleting selected jobs.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DeleteBegin);
        /// </summary>
        [Description("DeleteBegin")]
        DeleteBegin,

        /// <summary>
        /// Marks the time when a solution ends deleting selected jobs.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        /// </summary>
        [Description("DeleteEnd")]
        DeleteEnd,

        /// <summary>
        /// The device button press
        /// </summary>
        [Description("DeviceButtonPress")]
        DeviceButtonPress,

        /// <summary>
        /// Marks the time a device lock begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
        /// </summary>
        [Description("DeviceLockBegin")]
        DeviceLockBegin,

        /// <summary>
        /// Marks the time device lock ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
        /// </summary>
        [Description("DeviceLockEnd")]
        DeviceLockEnd,

        /// <summary>
        /// Marks the time a device sign out begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
        /// </summary>
        [Description("DeviceSignOutBegin")]
        DeviceSignOutBegin,

        /// <summary>
        /// Marks the time device sign out ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
        /// </summary>
        [Description("DeviceSignOutEnd")]
        DeviceSignOutEnd,

        /// <summary>
        /// Marks the time when the solution's document list is fully presented and ready for use.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DocumentListReady);
        /// </summary>
        [Description("DocumentListReady")]
        DocumentListReady,

        /// <summary>
        /// Marks the time when entering the credentials for Authentication begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
        /// </summary>
        [Description("EnterCredentialsBegin")]
        EnterCredentialsBegin,

        /// <summary>
        /// Marks the time when entering the credentials for Authentication ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);
        /// </summary>
        [Description("EnterCredentialsEnd")]
        EnterCredentialsEnd,

        /// <summary>
        /// Marks the time when image preview begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DocumentListReady);
        /// </summary>
        [Description("ImagePreviewBegin")]
        ImagePreviewBegin,

        /// <summary>
        /// Marks the time when image preview has completed.
        /// Usage: RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);
        /// </summary>
        [Description("ImagePreviewEnd")]
        ImagePreviewEnd,

        /// <summary>
        /// Marks the time when job build begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
        /// </summary>
        [Description("JobBuildBegin")]
        JobBuildBegin,

        /// <summary>
        /// Marks the time when job build has completed.
        /// Usage: RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
        /// </summary>
        [Description("JobBuildEnd")]
        JobBuildEnd,

        /// <summary>
        /// Marks when the system starts to navigate home
        /// Usage: RecordEvent(DeviceWorkflowMarker.NavigateHomeBegin);
        /// </summary>
        [Description("NavigateHomeBegin")]
        NavigateHomeBegin,

        /// <summary>
        /// Marks when the system is on the home screen
        /// Usage: RecordEvent(DeviceWorkflowMarker.NavigateHomeEnd);
        /// </summary>
        [Description("NavigateHomeEnd")]
        NavigateHomeEnd,

        /// <summary>
        /// Marks the time when a solution's method of printing all jobs begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
        /// </summary>
        [Description("PrintAllBegin")]
        PrintAllBegin,

        /// <summary>
        /// Marks the time when a solution's method of printing all jobs ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
        /// </summary>
        [Description("PrintAllEnd")]
        PrintAllEnd,

        /// <summary>
        /// Marks the time when a solution's method of deleting selected documents begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
        /// </summary>
        [Description("PrintDeleteBegin")]
        PrintDeleteBegin,

        /// <summary>
        /// Marks the time when a solution's method of deleting selected documents ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        /// </summary>
        [Description("PrintDeleteEnd")]
        PrintDeleteEnd,

        /// Marks the time when the solution's printer list is presented and ready for use.
        /// Used to mark when the bluetooth enabled printer is found.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrinterListReady);
        [Description("PrinterListReady")]
        PrinterListReady,
        /// <summary>
        /// Marks the time when the device first starts printing the job.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
        /// </summary>
        [Description("PrintJobBegin")]
        PrintJobBegin,

        /// <summary>
        /// Marks the time a print job ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
        /// </summary>
        [Description("PrintJobEnd")]
        PrintJobEnd,

        /// <summary>
        /// Marks the time a solution's (Pull Print) prints and keeps the selected jobs begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintKeepBegin);
        /// </summary>
        [Description("PrintKeepBegin")]
        PrintKeepBegin,

        /// <summary>
        /// Marks the time a solution's (Pull Print) prints and keeps the selected jobs has ended.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PrintKeepEnd;
        /// </summary>
        [Description("PrintKeepEnd")]
        PrintKeepEnd,

        /// <summary>
        /// Marks the time when the device or workflow event begins process the requested job
        /// Usage: RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
        /// </summary>
        [Description("ProcessingJobBegin")]
        ProcessingJobBegin,

        /// <summary>
        /// Marks the time that processing a job ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
        /// </summary>
        [Description("ProcessingJobEnd")]
        ProcessingJobEnd,

        /// <summary>
        /// Marks the time when the solution starts to pull a job from a pull print server.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
        /// </summary>
        [Description("PullingJobFromServerBegin")]
        PullingJobFromServerBegin,

        /// <summary>
        /// Marks the time when the solution ends pulling a job from a pull print server.
        /// Usage: RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);
        /// </summary>
        [Description("PullingJobFromServerEnd")]
        PullingJobFromServerEnd,

        /// <summary>
        /// Marks the time when the solution's QuickSet list is fully presented and ready for use.
        /// Usage: RecordEvent(DeviceWorkflowMarker.QuickSetReady);
        /// </summary>
        [Description("QuickSetListReady")]
        QuickSetListReady,

        /// <summary>
        /// Marks the time when a solution begins refreshing the document list.
        /// Usage: RecordEvent(DeviceWorkflowMarker.RefreshBegin);
        /// </summary>
        [Description("RefreshBegin")]
        RefreshBegin,

        /// <summary>
        /// Marks the time when a solution ends refreshing the document list.
        /// Usage: RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        /// </summary>
        [Description("RefreshEnd")]
        RefreshEnd,

        /// <summary>
        /// Marks the start of when a scan job begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
        /// </summary>
        [Description("ScanJobBegin")]
        ScanJobBegin,

        /// <summary>
        /// Marks the end of when the scan job ends
        /// Usage: RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
        /// </summary>
        [Description("ScanJobEnd")]
        ScanJobEnd,

        /// <summary>
        /// Marks the time when a solution begins selecting documents (1 to n) from the document list.
        /// Usage: RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
        /// </summary>
        [Description("SelectDocumentBegin")]
        SelectDocumentBegin,

        /// <summary>
        /// Marks the time when a solution ends selecting documents (1 to n) from the document list.
        /// Usage: RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        /// </summary>
        [Description("SelectDocumentEnd")]
        SelectDocumentEnd,

        /// <summary>
        /// Marks the time when the sending of a job begins.
        /// Usage: RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
        /// </summary>
        [Description("SendingJobBegin")]
        SendingJobBegin,

        /// <summary>
        /// Marks the time when the sending of a job ends.
        /// Usage: RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
        /// </summary>
        [Description("SendingJobEnd")]
        SendingJobEnd,

        /// <summary>
        /// The sign out method used by the solution. 
        /// Usage: RecordInfo(DeviceWorkflowMarker.SignOutType, "");
        /// </summary>
        [Description("SignOutType")]
        SignOutType,

        /// <summary>
        /// Marks the time when a firmware update begins
        /// Usage: RecordInfo(DeviceWorkflowMarker.FirmwareUpdateBegin, "");
        /// </summary>
        [Description("FirmwareUpdateBegin")]
        FirmwareUpdateBegin,

        /// <summary>
        /// Marks the time when a firmware update ends
        /// Usage: RecordInfo(DeviceWorkflowMarker.FirmwareUpdateEnd, "");
        /// </summary>
        [Description("FirmwareUpdateEnd")]
        FirmwareUpdateEnd,
        /// <summary>
        /// Marks the time when a device begins rebooting
        /// Usage: RecordInfo(DeviceWorkflowMarker.DeviceRebootBegin, "");
        /// </summary>
        [Description("DeviceRebootBegin")]
        DeviceRebootBegin,
        /// <summary>
        /// Marks the time when a device finishes rebooting (EWS is up)
        /// Usage: RecordInfo(DeviceWorkflowMarker.DeviceRebootEnd, "");
        /// </summary>
        [Description("DeviceRebootEnd")]
        DeviceRebootEnd,
        /// <summary>
        /// Marks the time when a web services comes up 
        /// Usage: RecordInfo(DeviceWorkflowMarker.WebServicesUp, "");
        /// </summary>
        [Description("WebServicesUp")]
        WebServicesUp,
        /// <summary>
        /// Marks the time when a device finishes rebooting (EWS is up)
        /// Usage: RecordInfo(DeviceWorkflowMarker.EmbeddedWebServiceUp, "");
        /// </summary>
        [Description("EmbeddedWebServerUp")]
        EmbeddedWebServerUp,

        /// <summary>
        /// Marks the time when a GFriend Exeuction is started
        /// Usage: RecordInfo(DeviceWorkflowMarker.GFriendExeuctionStart, "");
        /// </summary>
        [Description("GFriendExeuctionStart")]
        GFriendExeuctionStart,

        /// <summary>
        /// Marks the time when a GFriend Exeuction is ended
        /// Usage: RecordInfo(DeviceWorkflowMarker.GFriendExeuctionEnd, "");
        /// </summary>
        [Description("GFriendExeuctionEnd")]
        GFriendExeuctionEnd,
    }
}

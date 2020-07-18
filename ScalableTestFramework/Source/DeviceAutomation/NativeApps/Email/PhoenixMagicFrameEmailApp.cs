using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.PhoenixMagicFrame;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailApp" /> for a <see cref="PhoenixMagicFrameDevice" />.
    /// </summary>
    public sealed class PhoenixMagicFrameEmailApp : DeviceWorkflowLogSource, IEmailAppWithAddressSource, IEmailJobOptions
    {
        private readonly PhoenixMagicFrameDevice _device;
        private readonly PhoenixMagicFrameControlPanel _controlPanel;
        private readonly PhoenixMagicFrameJobExecutionManager _executionManager;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Gets or sets the address source.
        /// </summary>
        /// <value>The address source.</value>
        public string AddressSource { get; set; }

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>The from address.</value>
        public string FromAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixMagicFrameEmailApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public PhoenixMagicFrameEmailApp(PhoenixMagicFrameDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _executionManager = new PhoenixMagicFrameJobExecutionManager(device);
        }

        /// <summary>
        /// Launches the Email application on the device.
        /// </summary>
        public void Launch()
        {
            try
            {
                _controlPanel.Press("cScanHomeTouchButton");

                if (_controlPanel.WaitForDisplayedText("Scan Menu", TimeSpan.FromSeconds(1)))
                {
                    _controlPanel.Press("cEmailUiEmailDocument");
                }

                Pacekeeper.Pause();

                if (_controlPanel.WaitForDisplayedText("Scan to E-mail", TimeSpan.FromSeconds(1)))
                {
                    _controlPanel.Press("cSendAnEmail");
                }
            }
            catch (PhoenixInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {
                    _controlPanel.Press("cScanHomeTouchButton");

                    if (_controlPanel.WaitForDisplayedText("Scan Menu", TimeSpan.FromSeconds(1)))
                    {
                        _controlPanel.Press("cEmailUiEmailDocument");
                    }

                    Pacekeeper.Pause();

                    if (_controlPanel.WaitForDisplayedText("Scan to E-mail", TimeSpan.FromSeconds(1)))
                    {
                        _controlPanel.Press("cSendAnEmail");
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    authenticator.Authenticate();
                    _controlPanel.WaitForDisplayedText("unknown", TimeSpan.FromSeconds(30));
                }
                catch (PhoenixInvalidOperationException ex)
                {
                    throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                }
            }
            else  // AuthenticationMode.Eager
            {
                throw new NotImplementedException("Eager Authentication has not been implemented in PhoenixMagicFrameEmailApp.");
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            throw new NotImplementedException("LaunchFromQuickset has not been implemented in PhoenixMagicFrameEmailApp.");
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset if exists or else empty/null</param>
        /// </summary>  
        public void SelectQuickset(string quickSetName)
        {
            throw new NotImplementedException("Qucickset selection has not been implemented in Phoenix Email App.");
        }

        /// <summary>
        /// Enters the "To" email address.
        /// </summary>
        /// <param name="emailAddresses">The "To" email addresses.</param>
        public void EnterToAddress(IEnumerable<string> emailAddresses)
        {
            foreach (string address in emailAddresses)
            {
                if (_controlPanel.WaitForDisplayedText("From", TimeSpan.FromSeconds(1)))
                {
                    string toDisplayName = address.Split('@')[0];

                    if (AddressSource.Equals("Default"))
                    {
                        _controlPanel.Press(toDisplayName);
                    }
                    else if (AddressSource.Equals("Email Addressbook"))
                    {
                        _controlPanel.Press(FromAddress);
                        _controlPanel.WaitForDisplayedText($"From: {FromAddress}", TimeSpan.FromSeconds(2));
                        _controlPanel.Press("cToXStr");
                        _controlPanel.WaitForDisplayedText("To:", TimeSpan.FromSeconds(2));
                        _controlPanel.Press(toDisplayName);
                        _controlPanel.Press("cDoneTouchButton");

                    }
                    else if (AddressSource.Equals("LDAP Addressbook"))
                    {
                        _controlPanel.Press(FromAddress);
                        _controlPanel.Press("SEARCH");
                        _controlPanel.TypeOnVirtualKeyboard(toDisplayName);
                        _controlPanel.Press("cFindTouchButton");
                        _controlPanel.WaitForDisplayedText("Select a recipient.", TimeSpan.FromSeconds(5));
                        string domainName = string.Empty;

                        if (address.Contains('@'))
                        {
                            domainName = address.Split('@')[1];
                        }
                        string ldapAddress = string.Format("{0}\n %3Cfs%3E{1}@{2}%3C/fs%3E", toDisplayName.ToUpper(), toDisplayName.ToUpper(), domainName);
                        _controlPanel.Press(ldapAddress);
                    }
                }
            }
        }

        /// <summary>
        /// Enters the email subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        public void EnterSubject(string subject)
        {
            _controlPanel.WaitForDisplayedText("From:", TimeSpan.FromSeconds(2));
            _controlPanel.Press("cSubjectEmptyStr");
            _controlPanel.TypeOnVirtualKeyboard(subject);
            _controlPanel.Press("cOKTouchButton");
            _controlPanel.WaitForDisplayedText("From:", TimeSpan.FromSeconds(2));
            _controlPanel.Press("cNextTouchButton");
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            if (_controlPanel.WaitForDisplayedText("Scan to E-mail", TimeSpan.FromSeconds(1)))
            {
                _controlPanel.Press("cJobSettings");
                _controlPanel.Press("Scroll Down Arrow");
                var screenText = _controlPanel.GetDisplayedStrings().ToList();
                if (screenText.Contains("File Name Prefix"))
                {
                    _controlPanel.Press("cFilenamePrefix");
                    Pacekeeper.Pause();
                    var keyTexts = _controlPanel.GetDisplayedStrings().ToList();
                    int count = keyTexts.ElementAt(0).Count();
                    for (int i = 0; i < count; i++)
                    {
                        _controlPanel.Press("Del");
                    }
                    _controlPanel.TypeOnVirtualKeyboard(fileName);
                    _controlPanel.Press("cOKTouchButton");
                }
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool completedJob = false;
            _executionManager.WorkflowLogger = WorkflowLogger;
            completedJob = _executionManager.ExecuteScanJob("Scan Settings");
            return completedJob;
        }

        /// <summary>
        /// Gets the <see cref="IEmailJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IEmailJobOptions Options => this;

        #region IEmailJobOptions Members

        void IEmailJobOptions.SelectFileType(string fileType)
        {
        }

        void IEmailJobOptions.SetJobBuildState(bool enable)
        {
        }

        /// <summary>
        /// Enables print notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void IEmailJobOptions.EnablePrintNotification(NotifyCondition condition, bool thumbNail)
        {
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void IEmailJobOptions.EnableEmailNotification(NotifyCondition condition, string address, bool thumbNail)
        {
        }

        /// <summary>
        /// Selects the resolution for the selected file
        /// </summary>
        /// <param name="resolution">The resolution to select(case sensitive).</param>
        void IEmailJobOptions.SelectResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on PhoenixMagicFrame devices.");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        void IEmailJobOptions.SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor or Black  with setting {colorOrBlack} feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        void IEmailJobOptions.SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        void IEmailJobOptions.SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"SelectContentOrientation with setting {contentOrientation} feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Set image adjustment for sharpness, darkness, contrast and Background cleanup
        /// </summary>
        /// <param name="imageAdjustSharpness">Sets the value for Image adjustment sharpness </param>
        /// <param name="imageAdjustDarkness">Sets the value for Image adjustment darkness </param>
        /// <param name="imageAdjustContrast">Sets the value for Image adjustment contrast </param>
        /// <param name="imageAdjustBackgroundCleanup">Sets the value for Image adjustment background cleanup </param>
        /// <param name="automaticTone">if set to <c>true</c>enable sharpness only and disable all ;otherwise,enable all</param>
        void IEmailJobOptions.SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup, bool automaticTone)
        {

            throw new NotImplementedException($"SetImageadjustment feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or picture</param>
        void IEmailJobOptions.SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"SelectOptimizeTextOrPitcure with setting {optimizeTextOrPicture} feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Sets the Front and back erase edges for Top, Bottom, Left and Right
        /// </summary>
        /// <param name="eraseEdgeList">List cotaining the Key-value pair for the edge. Key, is the erase edge type enum and value is the string to be set for the element. Edges value must be a non empty, mumeric and a non zero string. Zeros will be ignored</param>
        /// <param name="applySameWidth">When set to <c>True</c> it applies same width for all edges for front; otherwise individual widths are applied</param>
        /// <param name="mirrorFrontSide">When set to <c>true</c>, back side will mirror the front side </param>
        /// <param name="useInches">If set to <c>true</c> inches will be used otherwise edges are set in milimeter </param>
        void IEmailJobOptions.SetEraseEdges(Dictionary<EraseEdgesType, decimal> eraseEdgeList, bool applySameWidth, bool mirrorFrontSide, bool useInches)
        {
            throw new NotImplementedException($"SetErase edges feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="selectedCropOption">crop optoin type to select(case sensitive)</param>
        void IEmailJobOptions.SelectCropOption(Cropping selectedCropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {selectedCropOption} feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        void IEmailJobOptions.SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        void IEmailJobOptions.SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException($"SelectOriginalSides with setting  feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        void IEmailJobOptions.CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException($"CreateMultipleFiles feature is not implemented on PhoenixMagicFrame devices");
        }

        /// <summary>
        /// Selects the signing or encrypt option for the scanned file
        /// </summary>
        /// <param name="sign">if set to <c>true</c> enable signing.</param>
        /// <param name="encrypt">if set to <c>true</c> enable encryption.</param>
        void IEmailJobOptions.SelectSigningAndEncrypt(bool sign, bool encrypt)
        {
            throw new NotImplementedException($"SelectSigningAndEncrypt feature is not implemented on PhoenixMagicFrame devices");
        }
        #endregion
    }
}

using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.SiriusUIv3;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailApp" /> for a <see cref="SiriusUIv3Device" />.
    /// </summary>
    public sealed class SiriusUIv3EmailApp : DeviceWorkflowLogSource, IEmailAppWithAddressSource, IEmailJobOptions
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;
        private readonly SiriusUIv3JobExecutionManager _executionManager;
        private readonly TimeSpan _waitTimeSpan = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _shortTimeSpan = TimeSpan.FromSeconds(5);
        private readonly TimeSpan _longTimeSpan = TimeSpan.FromSeconds(30);
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
        /// Initializes a new instance of the <see cref="SiriusUIv3EmailApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv3EmailApp(SiriusUIv3Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
            _executionManager = new SiriusUIv3JobExecutionManager(device);
        }

        /// <summary>
        /// Launches the Email application on the device.
        /// </summary>      
        public void Launch()
        {
            try
            {

                _controlPanel.ScrollPress("sfolderview_p","group.group.scan" );
                _controlPanel.WaitForScreenLabel("Home", _shortTimeSpan); //Scan To Page
                Pacekeeper.Pause();
                _controlPanel.ScrollPress("sfolderview_p","command.scan_email");
                Pacekeeper.Pause();
               

                _controlPanel.WaitForScreenLabel("Scan_Email_EmailPageGlass", _shortTimeSpan); //Email Page
                Pacekeeper.Pause();
            }
            catch (ElementNotFoundException ex)
            {
                string currentForm = _controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault();
                if (currentForm.Equals("Scan_Status_Error"))
                {
                    throw new DeviceWorkflowException("Email application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Email application from {currentForm}.", ex);
                }
            }
            catch (SiriusInvalidOperationException ex)
            {
                switch (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault())
                {
                    case "Scan_Email_EmailPageGlass":
                        // The application launched successfully. This happens sometimes.
                        break;

                    case "Scan_Status_Error":
                        {
                            throw new DeviceWorkflowException("Sign-in required to launch the Email application.", ex);
                        }

                    case "Scan_Email_NotSetup":
                        {
                            throw new DeviceWorkflowException("Scan To Email is not configured");
                        }
                    default:
                        {
                            throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                        }
                }
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {
                    _controlPanel.ScrollPress("sfolderview_p","group.group.scan" );

                    _controlPanel.WaitForScreenLabel("Home", _shortTimeSpan); //Scan To Page
                    Pacekeeper.Pause();
                    _controlPanel.ScrollPress("sfolderview_p","command.scan_email" );
                    Pacekeeper.Pause();

                    if(!_controlPanel.WaitForScreenLabel("Scan_Email_EmailPageGlass", _shortTimeSpan))
                    {
                        //if there are two sender's profile, we will have this intermediate screen which needs to be handled, selecting the first one as default
                        if (_controlPanel.WaitForScreenLabel("Scan_Email_SenderProfile", _shortTimeSpan))
                        {
                            _controlPanel.Press("model.ScanEmailProfilesModel.0");
                            Pacekeeper.Pause();
                        }

                        if (_controlPanel.WaitForScreenLabel("AnA_Login_With_Windows_Authentication", _shortTimeSpan) || _controlPanel.WaitForScreenLabel("AnA_Login_With_LDAP_Authentication", _shortTimeSpan)) //Scan To Page
                            Pacekeeper.Pause();
                    }

                  
                    if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault() != "Scan_Email_EmailPageGlass")
                    {
                        if (authenticator == null)
                        {
                            throw new DeviceWorkflowException("Credentials are not supplied");
                        }
                        authenticator.Authenticate();
                    }
                    _controlPanel.WaitForScreenLabel("Scan_Email_EmailPageGlass", _longTimeSpan); //Email Form
                    Pacekeeper.Pause();

                }
                catch (ElementNotFoundException ex)
                {
                    string currentForm = _controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault();
                    if (currentForm.Equals("Home"))
                    {
                        throw new DeviceWorkflowException($"Email application button was not found on device home screen.", ex);
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Cannot launch the Email application from {currentForm}.", ex);
                    }
                }
                catch (SiriusInvalidOperationException ex)
                {
                    switch (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault())
                    {
                        case "Scan_Email_EmailPageGlass":
                            // The application launched successfully. This happens sometimes.
                            break;

                        case "AnA_Login_With_Windows_Authentication;AnA_Login_With_LDAP_Authentication":
                            {
                                throw new DeviceWorkflowException($"Sign-in required to launch the Email application.", ex);
                            }

                        case "Scan_Email_NotSetup":
                        {
                            throw new DeviceWorkflowException("Scan To Email is not configured");
                        }

                        default:
                            {
                                throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                            }
                    }
                }
            }
            else // AuthenticationMode.Eager
            {
                throw new NotImplementedException("Eager Authentication has not been implemented in SiriusUIv3EmailApp.");
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <exception cref="System.NotImplementedException">LaunchFromQuickset has not been implemented in SiriusUIv3EmailApp.</exception>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            throw new NotImplementedException("LaunchFromQuickset has not been implemented in SiriusUIv3EmailApp.");
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset if exists or else empty/null</param>
        /// </summary>  
        public void SelectQuickset(string quickSetName)
        {
            throw new NotImplementedException("Qucickset selection has not been implemented in Sirius Email App.");
        }

        /// <summary>
        /// Enters the "To" email address.
        /// </summary>
        /// <param name="emailAddresses">The "To" email addresses.</param>
        public void EnterToAddress(IEnumerable<string> emailAddresses)
        {
            foreach (string address in emailAddresses)
            {
                string displayName = address.Split('@')[0];
                string emailControlName = $"{displayName} {address}".ToLowerInvariant();

                try
                {
                    if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault() == "Scan_Email_EmailPageGlass")
                    {
                        switch (AddressSource)
                        {
                            case "Default":
                                {
                                    _controlPanel.Press("item1");//To Address
                                    Pacekeeper.Pause();
                                    _controlPanel.WaitForScreenLabel("Scan_Email_AddEmail", _waitTimeSpan);
                                    _controlPanel.SetValue("lineedit", address);
                                    Pacekeeper.Pause();
                                    _controlPanel.Press("_done"); //Done
                                }
                                break;

                            case "Email Addressbook":
                                {
                                    _controlPanel.Press("button2");
                                    _controlPanel.WaitForScreenLabel("Scan_Email_AddressBook", _waitTimeSpan);
                                    _controlPanel.PressByValue(displayName, StringMatch.StartsWith); //Check box
                                    Pacekeeper.Pause();
                                    _controlPanel.Press("fb_done");//OK
                                }
                                break;

                            case "LDAP Addressbook":
                                {
                                    _controlPanel.Press("email_menu_value_to");//To Address
                                    _controlPanel.WaitForScreenLabel(" view_email_Addressbook_Flow_In_STE_list", TimeSpan.FromSeconds(5));
                                    _controlPanel.Press("Addressbook_Flow_In_STE_footer.3");//Search Address book
                                    _controlPanel.WaitForScreenLabel("view_ldap_search", _shortTimeSpan);
                                    _controlPanel.SetValue("ldap_search_enter_search_vkbd.4", displayName);
                                    Pacekeeper.Pause();
                                    _controlPanel.WaitForScreenLabel("view_ldap_search_result", _longTimeSpan);
                                    _controlPanel.PressByValue(emailControlName); //Check box
                                    Pacekeeper.Pause();
                                    _controlPanel.WaitForScreenLabel("view_email_Addressbook_Flow_In_STE_list", _waitTimeSpan);
                                    _controlPanel.Press("Addressbook_Flow_In_STE_footer.0"); //Done
                                    Pacekeeper.Pause();

                                }
                                break;
                        }
                    }
                }
                catch (ElementNotFoundException ex)
                {
                    throw new DeviceWorkflowException($"Display name is different from To Address {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Enters the email subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        public void EnterSubject(string subject)
        {
            if (_controlPanel.WaitForScreenLabel("Scan_Email_EmailPageGlass"))
            {
                _controlPanel.Press("item2");
                Pacekeeper.Pause(); 
                _controlPanel.SetValue("slineedit", subject);//Keyboard
                Pacekeeper.Pause();
                _controlPanel.Press("_done");
                Pacekeeper.Pause();
            }
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            try
            {
                if (_controlPanel.WaitForScreenLabel("Scan_Email_EmailPageGlass"))
                {
                    _controlPanel.Press("item3");
                    Pacekeeper.Pause();
                   
                    _controlPanel.SetValue("slineedit", fileName);//Keyboard
                    Pacekeeper.Pause();
                    
                    _controlPanel.Press("_done");
                    Pacekeeper.Pause();
                }
            }
            catch (ElementNotFoundException)
            {
                //in small screen the option to set filename is missing, so ignore this
                
                //throw new DeviceWorkflowException($"Display name is different from To Address {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            var jobCompleted = _executionManager.ExecuteScanJob(executionOptions, "Scan_Email_EmailPageGlass");
            return jobCompleted;
        }

        /// <summary>
        /// Gets the <see cref="IEmailJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IEmailJobOptions Options => this;

        #region IEmailJobOptions Members

        void IEmailJobOptions.SelectFileType(string fileType)
        {
            //File type is set from EWS page
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
            throw new NotImplementedException($"Unable to set job resolution to resolution:{resolution}. Resolution Selection is not implemented on SiriusUIv3 devices.");
        }

        /// <summary>
        /// Selects the Color/Black for the scanned file
        /// </summary>
        /// <param name="colorOrBlack">The Color/Black is selected.</param>
        void IEmailJobOptions.SelectColorOrBlack(ColorType colorOrBlack)
        {
            throw new NotImplementedException($"SelectColor or Black  with setting {colorOrBlack} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Original Size for the scanned file
        /// </summary>
        /// <param name="originalSize">The original Size to select (case sensitive).</param>
        void IEmailJobOptions.SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"SelectOriginalSize with setting {originalSize} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Content Orientation  for the scanned file
        /// </summary>
        /// <param name="contentOrientation">content orientation.</param>
        void IEmailJobOptions.SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"SelectContentOrientation with setting {contentOrientation} feature is not implemented on SiriusUIv3 devices");
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
            throw new NotImplementedException($"SetImageadjustment feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Optimize text or picture  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        void IEmailJobOptions.SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"SelectOptimizeTextOrPitcure with setting {optimizeTextOrPicture} feature is not implemented on SiriusUIv3 devices");
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
            throw new NotImplementedException($"SetErase edges feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Crop Option for the scanned file
        /// </summary>
        /// <param name="selectedCropOption">crop optoin type to select(case sensitive)</param>
        void IEmailJobOptions.SelectCropOption(Cropping selectedCropOption)
        {
            throw new NotImplementedException($"SelectCropOption with setting {selectedCropOption} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Blank Page Supress for scanned file
        /// </summary>
        /// <param name="optionType"> blank page supress to select(case sensitive)</param>
        void IEmailJobOptions.SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"SelectBlankPageSupress with setting {optionType} feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the Original Sides for the scanned file
        /// </summary>
        /// <param name="originalSides">The original Sides to select (case sensitive).</param>
        /// <param name="pageFlipUp">if set to <c>true</c>enable pageFlipUp ;otherwise,disable it</param>
        void IEmailJobOptions.SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException($"SelectOriginalSides with setting  feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the multiple files setting for the scanned file
        /// </summary>
        /// <param name="isChecked">isCreatemultipleFiles selected<value><c>true</c>if it is checked;otherwise,<c>false</c></value></param>
        /// <param name="pagesPerFile">pages per file value.</param>
        void IEmailJobOptions.CreateMultipleFiles(bool isChecked, string pagesPerFile)
        {
            throw new NotImplementedException($"CreateMultipleFiles feature is not implemented on SiriusUIv3 devices");
        }

        /// <summary>
        /// Selects the signing or encrypt option for the scanned file
        /// </summary>
        /// <param name="sign">if set to <c>true</c> enable signing.</param>
        /// <param name="encrypt">if set to <c>true</c> enable encryption.</param>
        void IEmailJobOptions.SelectSigningAndEncrypt(bool sign, bool encrypt)
        {
            throw new NotImplementedException($"SelectSigningAndEncrypt feature is not implemented on SiriusUIv3 devices");
        }

        #endregion
    }
}
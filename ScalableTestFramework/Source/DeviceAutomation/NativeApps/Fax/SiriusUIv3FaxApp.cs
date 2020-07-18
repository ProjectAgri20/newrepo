using System;
using System.Linq;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.SiriusUIv3;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Fax
{

    /// <summary>
    /// Implementation of <see cref="IFaxApp"/> for a <see cref="SiriusUIv3Device" />.
    /// </summary>

    public sealed class SiriusUIv3FaxApp : DeviceWorkflowLogSource, IFaxApp, IFaxJobOptions
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;
        private readonly SiriusUIv3JobExecutionManager _executionManager;


        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3FaxApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv3FaxApp(SiriusUIv3Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
            _executionManager = new SiriusUIv3JobExecutionManager(device);

        }


        /// <summary>
        /// Launches the Fax application on the device.
        /// </summary>
        public void Launch()
        {
            try
            {
                _controlPanel.PressKey(SiriusSoftKey.Home);
                Pacekeeper.Pause();
                _controlPanel.Press("group.group.fax");
                Pacekeeper.Pause();
                _controlPanel.Press("command.fax_now");
                _controlPanel.WaitForScreenLabel("Fax_FaxHome_ImportedPhoneBook", TimeSpan.FromSeconds(10));
                Pacekeeper.Pause();
            }

            catch (SiriusInvalidOperationException ex)
            {
                switch (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault())
                {
                    case "Fax_FaxHome_ImportedPhoneBook":
                        // The application launched successfully. This happens sometimes.
                        break;

                    case "Scan_Status_Error":
                        {
                            throw new DeviceWorkflowException("Sign-in required to launch the Fax application.", ex);
                        }

                    default:
                        {
                            throw new DeviceWorkflowException($"Could not launch fax application: {ex.Message}", ex);
                        }
                }
            }

        }


        /// <summary>
        /// Launches the Fax application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>

        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {

            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                try
                {

                    _controlPanel.PressKey(SiriusSoftKey.Home);
                    Pacekeeper.Pause();
                    _controlPanel.Press("group.group.fax");
                    Pacekeeper.Pause();
                    _controlPanel.Press("command.fax_now");
                    //Pacekeeper.Pause();
                    // _controlPanel.WaitForScreenLabel("Fax_FaxHome_ImportedPhoneBook", TimeSpan.FromSeconds(10));
                    if (_controlPanel.WaitForScreenLabel("AnA_Login_With_Windows_Authentication", TimeSpan.FromSeconds(10)) || _controlPanel.WaitForScreenLabel("AnA_Login_With_LDAP_Authentication", TimeSpan.FromSeconds(10))) //Scan To Page
                    {
                        Pacekeeper.Pause();


                        if (authenticator == null)
                        {
                            throw new DeviceWorkflowException("Credentials are not supplied");
                        }

                        throw new DeviceWorkflowException(" Authentication needs to be implemented");
                        //authenticator.Authenticate();

                    }
                    _controlPanel.WaitForScreenLabel("Fax_FaxHome_ImportedPhoneBook", TimeSpan.FromSeconds(30)); //Send Fax Form
                    Pacekeeper.Pause();

                }
                catch (SiriusInvalidOperationException ex)
                {
                    switch (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault())
                    {
                        case "Fax_FaxHome_ImportedPhoneBook":
                            // The application launched successfully. This happens sometimes.
                            break;

                        case "AnA_Login_With_Windows_Authentication;AnA_Login_With_LDAP_Authentication":
                            {
                                throw new DeviceWorkflowException($"Sign-in required to launch the Fax application.", ex);
                            }

                        default:
                            {
                                throw new DeviceWorkflowException($"Could not launch Fax application: {ex.Message}", ex);
                            }
                    }
                }
            }
            else // AuthenticationMode.Eager
            {
                throw new NotImplementedException("Eager Authentication has not been implemented in SiriusUIv3FaxApp.");
            }
        }



        /// <summary>
        /// Adds the specified recipient/s for the fax.
        /// </summary>
        /// <param name="recipients">The recipients. Contains PINs, if used.</param>
        /// <param name="useSpeedDial">Uses the #s as speed dials</param>
        public void AddRecipients(Dictionary<string, string> recipients, bool useSpeedDial)
        {

            try
            {
                if (_controlPanel.WaitForScreenLabel("Fax_FaxHome_ImportedPhoneBook", TimeSpan.FromSeconds(1)))
                {
                    char[] recp = recipients.First().Key.ToArray();

                    for (int i = 0; i < recp.Length; i++)
                    {
                        char value = recp[i];

                        switch (value)
                        {
                            case '1':
                                _controlPanel.Press("text.1");
                                break;

                            case '2':
                                _controlPanel.Press("text.2");
                                break;

                            case '3':
                                _controlPanel.Press("text.3");
                                break;
                            case '4':
                                _controlPanel.Press("text.4");
                                break;
                            case '5':
                                _controlPanel.Press("text.5");
                                break;
                            case '6':
                                _controlPanel.Press("text.6");
                                break;

                            case '7':
                                _controlPanel.Press("text.7");
                                break;

                            case '8':
                                _controlPanel.Press("text.8");
                                break;
                            case '9':
                                _controlPanel.Press("text.9");
                                break;
                            case '0':
                                _controlPanel.Press("text.0");
                                break;

                            default:
                                throw new DeviceWorkflowException("Error entering fax recipient");
                        }
                    }
                    Pacekeeper.Sync();
                }
                _controlPanel.Press("fb_start"); //Starts fax job.
                Pacekeeper.Sync();
            }
            catch (SiriusInvalidOperationException ex)
            {
                if (_controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault() == "Scan_Status_Error")
                {
                    throw new DeviceWorkflowException("Error entering fax recipient.", ex);
                }
            }

        }

        /// <summary>
        /// Retrieves the HTML string for the Fax report from Device Control Panel
        /// </summary>
        public string RetrieveFaxReport()
        {
            throw new NotImplementedException("Fax Report is not supported");
        }


        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>

        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            //var jobCompleted = _executionManager.ExecuteScanJob(executionOptions, "Fax_FaxHome_ImportedPhoneBook");
            var jobCompleted = _executionManager.ExecuteScanJob(executionOptions, "Fax_SendSuccessfully");
            if (jobCompleted)
            {
                if (_controlPanel.WaitForScreenLabel("Fax_SendSuccessfully", TimeSpan.FromSeconds(90)))
                {
                    _controlPanel.Press("mdlg_single_button");
                }
                else
                {
                    jobCompleted = false;
                    throw new DeviceWorkflowException("Device did not receive fax receive confirmation.");
                }
            }
            else
            {
                throw new DeviceWorkflowException("Fax Send job failure.");
            }

            return jobCompleted;
        }


        /// <summary>
        /// Gets the <see cref="IFaxJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IFaxJobOptions Options => this;

        #region IFaxJobOptions Members

        void IFaxJobOptions.SetJobBuildState(bool enable)
        {
            throw new NotImplementedException($"Set job buildstate has not been implemented for SiriusUIv3 fax app");
        }

        /// <summary>
        /// Enables print notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void IFaxJobOptions.EnablePrintNotification(NotifyCondition condition, bool thumbNail)
        {
            throw new NotImplementedException($"Enable print notification has not been implemented for SiriusUIv3 fax app");
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="condition">The condition under which to send notification.</param>
        /// <param name="address">The email address to receive the notification.</param>
        /// <param name="thumbNail">if set to<c>true>enable thumbnail;otherwise,disable it</c></param>
        void IFaxJobOptions.EnableEmailNotification(NotifyCondition condition, string address, bool thumbNail)
        {
            throw new NotImplementedException($"Enable email notification has not been implemented for SiriusUIv3 fax app");
        }

        /// <summary>
        /// Select Fax Resolution
        /// </summary>
        /// <param name="resolution">The resolution.</param>
        public void SelectFaxResolution(ResolutionType resolution)
        {
            throw new NotImplementedException($"Select fax resolution has not been implemented for SiriusUIv3 fax app.");
        }

        /// <summary>
        /// Select OriginalSize.
        /// </summary>
        /// <param name="originalSize">The originalSize.</param>
        public void SelectOriginalSize(OriginalSize originalSize)
        {
            throw new NotImplementedException($"Select original size has not been implemented for SiriusUIv3 fax app.");
        }

        /// <summary>
        /// SelectContentOrientation
        /// </summary>
        /// <param name="contentOrientation">The content Orientation.</param>
        public void SelectContentOrientation(ContentOrientation contentOrientation)
        {
            throw new NotImplementedException($"Select content orientation has not been implemented for SiriusUIv3 fax app.");
        }

        /// <summary>
        /// Enables email notification for this job.
        /// </summary>
        /// <param name="imageAdjustSharpness">The imageAdjustSharpness.</param>
        /// <param name="imageAdjustDarkness">The imageAdjustDarkness.</param>
        /// <param name="imageAdjustContrast">The imageAdjustContrast</param>
        /// <param name="imageAdjustBackgroundCleanup">The imageAdjustContrast</param>
        public void SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup)
        {
            throw new NotImplementedException($"Select image adjustment has not been implemented for SiriusUIv3 fax app.");
        }

        /// <summary>
        /// Select Optimize TextOrPitcure
        /// </summary>
        /// <param name="optimizeTextOrPicture">The optimizeTextOrPicture.</param>
        public void SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture)
        {
            throw new NotImplementedException($"Select optimize text or picture has not been implemented for SiriusUIv3 fax app.");
        }

        /// <summary>
        /// Select Blank Page Supress
        /// </summary>
        /// <param name="optionType">The optionType.</param>
        public void SelectBlankPageSupress(BlankPageSupress optionType)
        {
            throw new NotImplementedException($"Select blank page supress has not been implemented for SiriusUIv3 fax app.");
        }

        /// <summary>
        /// Selects Original Sides
        /// </summary>
        /// <param name="originalSides">The originalSides.</param>
        /// <param name="pageFlipUp">The email address to receive the notification.</param>
        public void SelectOriginalSides(OriginalSides originalSides, bool pageFlipUp)
        {
            throw new NotImplementedException($"Select original sides has not been implemented for Sirius UIv3 fax app.");
        }

        /// <summary>
        /// Adds the recipient.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        public void AddRecipient(string recipient)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

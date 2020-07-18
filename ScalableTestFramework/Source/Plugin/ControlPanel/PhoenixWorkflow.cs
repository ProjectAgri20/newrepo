using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.DeviceAutomation;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    internal class PhoenixWorkflow
    {
        private readonly PhoenixNovaDevice _phoenixNovaDevice;
        private readonly PhoenixMagicFrameDevice _phoenixMagicFrameDevice;
        private readonly NetworkCredential _credential;
        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(1));

        public PhoenixWorkflow(PhoenixNovaDevice device)
        {
            _phoenixNovaDevice = device;
        }

        public PhoenixWorkflow(PhoenixMagicFrameDevice device)
        {
            _phoenixMagicFrameDevice = device;
        }

        public PhoenixWorkflow(IDevice device, NetworkCredential credential)
        {
            _credential = credential;
            var novaDevice = device as PhoenixNovaDevice;
            if (novaDevice != null)
            {
                _phoenixNovaDevice = novaDevice;
            }
            else
            {
                _phoenixMagicFrameDevice = (PhoenixMagicFrameDevice)device;
            }
        }

        /// <summary>
        /// Checks the assigned password from EWS on control panel
        /// </summary>
        /// <param name="password">Password to verify</param>
        [Description("Verifies the password")]
        public void CheckPassword(string password)
        {
            if (_phoenixNovaDevice != null)
            {
                CheckPasswordNova(password);
            }
            else
            {
                CheckPasswordMagicFrame(password);
            }
        }

        private void CheckPasswordMagicFrame(string password)
        {
            NavigateToSetupMagicFrame();

            //Entering the password
            _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(password);

            //Pressing on Ok button after entering the password
            _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");

            //Getting the string dispalyed on the control panel and checking if the password is invalid
            var resultCollection = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings();
            if (resultCollection.Any(resultString => resultString.Contains("cInvalidPassword")))
            {
                throw new DeviceWorkflowException("Password is invalid");
            }
        }

        private void CheckPasswordNova(string password)
        {
            NavigateToSetupNova();
            //Entering the password
            _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(password);

            //Pressing on Ok button after entering the password
            _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");

            //Getting the string dispalyed on the control panel and checking if the password is invalid
            var resultCollection = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings();
            if (resultCollection.Any(resultString => resultString.Contains("cInvalidPassword")))
            {
                throw new DeviceWorkflowException("Password is invalid");
            }
        }

        /// <summary>
        /// Checks the scan output file type for the selected shared network folder
        /// </summary>
        /// <param name="networkFolderName">network folder name</param>
        /// <param name="scanOutputFileType">the file type</param>
        /// <param name="scanPaperSize">the paper size</param>
        [Description("Verifies the Scan Filetype and Paper size for SNF workflow")]
        public void CheckScanFileTypeAndPaperSize(string networkFolderName, string scanOutputFileType,
            string scanPaperSize)
        {
            if (_phoenixNovaDevice != null)
            {
                CheckScanFileTypeAndPaperSizeNova(networkFolderName, scanOutputFileType, scanPaperSize);
            }
            else
            {
                CheckScanFileTypeAndPaperSizeMagicFrame(networkFolderName, scanOutputFileType, scanPaperSize);
            }
        }

        private void CheckScanFileTypeAndPaperSizeMagicFrame(string networkFolderName, string scanOutputFileType,
            string scanPaperSize)
        {
            bool scanFileTypeResponse = false, scanPaperSizeResponse = false;

            //Check for Default Paper Size
            _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            _pacekeeper.Pause();

            //Press on scan button - cScanHomeTouchButton
            _phoenixMagicFrameDevice.ControlPanel.Press("cScanHomeTouchButton");
            _pacekeeper.Pause();

            //Press on Scan to Network folder - cScanToNetworkFolder
            _phoenixMagicFrameDevice.ControlPanel.Press("cScanToNetworkFolder");
            _pacekeeper.Pause();

            //Press on actual Scan to Network folder - cScanToNetworkFolder
            _phoenixMagicFrameDevice.ControlPanel.Press(networkFolderName);
            _pacekeeper.Pause();

            //Get on the control panel text
            var resultCollection = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings();
            _pacekeeper.Pause();
            foreach (string results in resultCollection)
            {
                if (results.Equals(scanOutputFileType))
                {
                    scanFileTypeResponse = true;
                }
                if (results.Equals(scanPaperSize))
                {
                    scanPaperSizeResponse = true;
                }
            }
            if ((scanFileTypeResponse == false) && (scanPaperSizeResponse == false))
            {
                throw new DeviceWorkflowException("Both Scan File Type and Paper Size are not Matching.");
            }
            if ((scanFileTypeResponse == false))
            {
                throw new DeviceWorkflowException("Scan File Type is not matching");
            }
            if ((scanPaperSizeResponse == false))
            {
                throw new DeviceWorkflowException("Scan Paper Size is not matching");
            }
        }

        private void CheckScanFileTypeAndPaperSizeNova(string networkFolderName, string scanOutputFileType,
            string scanPaperSize)
        {
            bool scanFileTypeResponse = false, scanPaperSizeResponse = false;

            //Check for Default Paper Size
            _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            _pacekeeper.Pause();

            //Press on scan button - cScanHomeTouchButton
            _phoenixNovaDevice.ControlPanel.Press("cScanHomeTouchButton");
            _pacekeeper.Pause();

            //Press on Scan to Network folder - cScanToNetworkFolder
            _phoenixNovaDevice.ControlPanel.Press("cScanToNetworkFolder");
            _pacekeeper.Pause();

            //Press on actual Scan to Network folder - cScanToNetworkFolder
            _phoenixNovaDevice.ControlPanel.Press(networkFolderName);
            _pacekeeper.Pause();

            //Get on the control panel text
            var resultCollection = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings();
            _pacekeeper.Pause();
            foreach (string results in resultCollection)
            {
                if (results.Equals(scanOutputFileType))
                {
                    scanFileTypeResponse = true;
                }
                if (results.Equals(scanPaperSize))
                {
                    scanPaperSizeResponse = true;
                }
            }
            if ((scanFileTypeResponse == false) && (scanPaperSizeResponse == false))
            {
                throw new DeviceWorkflowException("Both Scan File Type and Paper Size are not Matching.");
            }
            if ((scanFileTypeResponse == false))
            {
                throw new DeviceWorkflowException("Scan File Type is not matching");
            }
            if ((scanPaperSizeResponse == false))
            {
                throw new DeviceWorkflowException("Scan Paper Size is not matching");
            }
        }

        /// <summary>
        /// LDAP Authentication for Copy App
        /// </summary>
        [Description("Performs LDAP authentication for Copy")]
        public void LdapAuthenticationforCopyApp(string password)
        {
            try
            {
                _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cCopyHomeTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("UsernameTextbox");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(_credential.UserName);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("PasswordTextbox");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(password);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cSignInTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"LDAP Authentication failed with exception:{ex.Message}");
            }
        }

        private bool NavigateToSetupMagicFrame()
        {
            _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            _pacekeeper.Pause();

            var checkScreen = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings().First();
            _pacekeeper.Pause();

            if (checkScreen == "Ready")
            {
                _phoenixMagicFrameDevice.ControlPanel.Press("SetupButton");
                _pacekeeper.Pause();

                checkScreen = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings().First();
                _pacekeeper.Pause();

                if (checkScreen == "Setup Menu")
                {
                    _phoenixMagicFrameDevice.ControlPanel.Press("cSystemSetup");
                    _pacekeeper.Pause();
                    return true;
                }
            }
            return false;
        }

        private bool NavigateToSetupNova()
        {
            _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            _pacekeeper.Pause();

            _phoenixNovaDevice.ControlPanel.ScrollRight(1);
            _pacekeeper.Pause();
            _phoenixNovaDevice.ControlPanel.ScrollRight(1);
            _pacekeeper.Pause();
            var checkScreen = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings().First();
            _pacekeeper.Pause();

            if (checkScreen == "Ready")
            {
                _phoenixNovaDevice.ControlPanel.Press("cSetupHomeTouchButton");
                _pacekeeper.Pause();
                checkScreen = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings().First();
                _pacekeeper.Pause();
                if (checkScreen == "Setup Menu")
                {
                    _phoenixNovaDevice.ControlPanel.Press("cSystemSetup");
                    _pacekeeper.Pause();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Restarts the device
        /// </summary>
        [Description("Restarts the device")]
        public void RestartDevice()
        {
            Snmp snmp;
            snmp = _phoenixNovaDevice != null
                ? new Snmp(_phoenixNovaDevice.Address)
                : new Snmp(_phoenixMagicFrameDevice.Address);

            try
            {
                snmp.Set("1.3.6.1.2.1.43.5.1.1.3.1", 6);
            }
            catch (SnmpException ex)
            {
                throw new SnmpException("Not able to restart printer", ex);
            }
        }

        /// <summary>
        /// Navigate to Alarm Settings
        /// </summary>
        [Description("Sets the Alarm volume level to Off, Soft, Medium and Loud")]
        public void SetAlarmVolume(string volumeLevel = "Off")
        {
            if (_phoenixNovaDevice != null)
            {
                SetAlarmVolumeNova(volumeLevel);
            }
            else
            {
                SetAlarmVolumeMagicFrame(volumeLevel);
            }
        }

        private void SetAlarmVolumeMagicFrame(string volumeLevel)
        {
            try
            {
                _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();

                var checkScreen = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings().First();
                _pacekeeper.Pause();

                if (checkScreen == "Ready")
                {
                    _phoenixMagicFrameDevice.ControlPanel.Press("SetupButton");
                    _pacekeeper.Pause();
                    checkScreen = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings().First();
                    _pacekeeper.Pause();
                    if (checkScreen == "Setup Menu")
                    {
                        _pacekeeper.Pause();
                        _phoenixMagicFrameDevice.ControlPanel.Press("cSystemSetup");
                        _pacekeeper.Pause();

                        _phoenixMagicFrameDevice.ControlPanel.Press("Scroll Down Arrow");
                        _pacekeeper.Pause();

                        _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeSettingsStr");
                        _pacekeeper.Pause();

                        _phoenixMagicFrameDevice.ControlPanel.Press("cAlarmVolumeStr");
                        _pacekeeper.Pause();

                        if (string.Equals(volumeLevel, "Off"))
                        {
                            _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeOff");
                            _pacekeeper.Pause();
                        }
                        else if (string.Equals(volumeLevel, "Soft"))
                        {
                            _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeSoft");
                            _pacekeeper.Pause();
                        }
                        else if (string.Equals(volumeLevel, "Medium"))
                        {
                            _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeMedium");
                            _pacekeeper.Pause();
                        }
                        else if (string.Equals(volumeLevel, "Loud"))
                        {
                            _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeLoud");
                            _pacekeeper.Pause();
                        }
                    }
                }
                _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Volume not set");
            }
        }

        private void SetAlarmVolumeNova(string volumeLevel)
        {
            try
            {
                _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.ScrollRight(1);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.ScrollRight(1);
                _pacekeeper.Pause();

                var checkScreen = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings().First();
                _pacekeeper.Pause();

                if (checkScreen == "Ready")
                {
                    _phoenixNovaDevice.ControlPanel.Press("cSetupHomeTouchButton");
                    _pacekeeper.Pause();

                    checkScreen = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings().First();

                    if (checkScreen == "Setup Menu")
                    {
                        _phoenixNovaDevice.ControlPanel.Press("cSystemSetup");
                        _pacekeeper.Pause();

                        _phoenixNovaDevice.ControlPanel.ScrollDown(100);
                        _pacekeeper.Pause();

                        _phoenixNovaDevice.ControlPanel.Press("cVolumeSettingsStr");
                        _pacekeeper.Pause();

                        _phoenixNovaDevice.ControlPanel.Press("cAlarmVolumeStr");
                        _pacekeeper.Pause();

                        if (string.Equals(volumeLevel, "Off"))
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeOff");
                            _pacekeeper.Pause();
                        }
                        else if (string.Equals(volumeLevel, "Soft"))
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeSoft");
                            _pacekeeper.Pause();
                        }
                        else if (string.Equals(volumeLevel, "Medium"))
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeMedium");
                            _pacekeeper.Pause();
                        }
                        else if (string.Equals(volumeLevel, "Loud"))
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeLoud");
                            _pacekeeper.Pause();
                        }
                    }
                }
                _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Volume not set");
            }
        }
        /// <summary>
        /// Sets the ring volume level
        /// </summary>
        /// <param name="volumeLevel"></param>
        [Description("Sets the Ring volume level to Off, Soft, Medium and Loud")]
        public void SetRingVolume(string volumeLevel = "Off")
        {
            if (_phoenixNovaDevice != null)
            {
                SetRingVolumeNova(volumeLevel);
            }
            else
            {
                SetRingVolumeMagicFrame(volumeLevel);
            }
        }

        private void SetRingVolumeMagicFrame(string volumeLevel)
        {
            try
            {
                if (NavigateToSetupMagicFrame())
                {
                    _phoenixMagicFrameDevice.ControlPanel.Press("Scroll Down Arrow");
                    _pacekeeper.Pause();

                    _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeSettingsStr");
                    _pacekeeper.Pause();

                    _phoenixMagicFrameDevice.ControlPanel.Press("cRingVolumeStr");
                    _pacekeeper.Pause();

                    switch ((volumeLevel))
                    {
                        case "Off":
                            {
                                _phoenixNovaDevice.ControlPanel.Press("cVolumeOff");
                                _pacekeeper.Pause();
                            }
                            break;

                        case "Soft":
                            {
                                _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeSoft");
                                _pacekeeper.Pause();
                            }
                            break;

                        case "Medium":
                            {
                                _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeMedium");
                                _pacekeeper.Pause();
                            }
                            break;

                        case "Loud":
                            {
                                _phoenixMagicFrameDevice.ControlPanel.Press("cVolumeLoud");
                                _pacekeeper.Pause();
                            }
                            break;
                    }
                }
            }
            catch
            {
                throw new DeviceWorkflowException("Volume not set");
            }
        }

        /// <summary>
        /// Navigate to Ring Volume
        /// </summary>
        private void SetRingVolumeNova(string volumeLevel)
        {
            try
            {
                NavigateToSetupNova();

                _phoenixNovaDevice.ControlPanel.ScrollDown(100);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cVolumeSettingsStr");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cRingVolumeStr");
                _pacekeeper.Pause();

                switch ((volumeLevel))
                {
                    case "Off":
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeOff");
                            _pacekeeper.Pause();
                        }
                        break;

                    case "Soft":
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeSoft");
                            _pacekeeper.Pause();
                        }
                        break;

                    case "Medium":
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeMedium");
                            _pacekeeper.Pause();
                        }
                        break;

                    case "Loud":
                        {
                            _phoenixNovaDevice.ControlPanel.Press("cVolumeLoud");
                            _pacekeeper.Pause();
                        }
                        break;
                }
            }
            catch
            {
                throw new DeviceWorkflowException("Volume not set");
            }
        }

        /// <summary>
        /// ControlPanel SignOut
        /// </summary>
        [Description("Signs out the current logged in user")]
        public void SignOut()
        {
            if (_phoenixNovaDevice != null)
            {
                SignOutNova();
            }
            else
            {
                SignOutMagicFrame();
            }
        }

        private void SignOutMagicFrame()
        {
            _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            _pacekeeper.Pause();

            try
            {
                _phoenixMagicFrameDevice.ControlPanel.Press("SignOutButton");
                _pacekeeper.Pause();

                _phoenixMagicFrameDevice.ControlPanel.Press("cSignOutTouchButton");
                _pacekeeper.Pause();
            }
            catch
            {
                Framework.Logger.LogError("Sign in must be required");
                throw new DeviceWorkflowException("Sign in must be required");
            }
        }

        private void SignOutNova()
        {
            _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            _pacekeeper.Pause();

            try
            {
                _phoenixNovaDevice.ControlPanel.Press("SignOutButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cSignOutTouchButton");
                _pacekeeper.Pause();
            }
            catch
            {
                Framework.Logger.LogInfo("Sign in must be required");
                throw new DeviceWorkflowException("Sign in must be required");
            }
        }

        [Description("Validates default paper size and the type")]
        public void ValidateDefaultPaperSizeAndType(string defaultPaperSize, string defaultPaperType)
        {
            if (_phoenixNovaDevice != null)
            {
                ValidateDefaultPaperSizeAndTypeNova(defaultPaperSize, defaultPaperType);
            }
            else
            {
                ValidateDefaultPaperSizeAndTypeMagicFrame(defaultPaperSize, defaultPaperType);
            }
        }

        private void ValidateDefaultPaperSizeAndTypeMagicFrame(string defaultPaperSize, string defaultPaperType)
        {
            bool defaultPaperSizeResult = false, defaultPaperTypeResult = false;

            NavigateToSetupMagicFrame();

            if (
                _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings()
                    .Any(x => x.StartsWith("Enter Password", StringComparison.OrdinalIgnoreCase)))
            {
                _phoenixMagicFrameDevice.ControlPanel.TypeOnVirtualKeyboard(_phoenixMagicFrameDevice.AdminPassword);
                _phoenixMagicFrameDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                if (
                    _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings()
                        .Any(x => x.StartsWith("Password", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new DeviceWorkflowException("Default Password not accepted, Please reset password or remove to continue");
                }
            }

            //Click on Paper set up - cPaperSetup
            _phoenixMagicFrameDevice.ControlPanel.Press("cPaperSetup");
            _pacekeeper.Pause();

            //Click on Default paper size - cDefaultPaperSizeStr - cDefaultPaperSizeStr
            _phoenixMagicFrameDevice.ControlPanel.Press("cDefaultPaperSizeStr");
            _pacekeeper.Pause();

            if (_phoenixMagicFrameDevice.ControlPanel.GetSelectedListItem()
                .Equals(defaultPaperSize, StringComparison.OrdinalIgnoreCase))
            {
                defaultPaperSizeResult = true;
            }

            //Check for Default Paper Type
            NavigateToSetupMagicFrame();

            if (
                _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings()
                    .Any(x => x.StartsWith("Enter Password", StringComparison.OrdinalIgnoreCase)))
            {
                _phoenixMagicFrameDevice.ControlPanel.TypeOnVirtualKeyboard(_phoenixMagicFrameDevice.AdminPassword);
                _phoenixMagicFrameDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                if (
                    _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings()
                        .Any(x => x.StartsWith("Password", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new DeviceWorkflowException(
                        "Default Password not accepted, Please reset password or remove to continue");
                }
            }
            //Click on Paper set up - cPaperSetup
            _phoenixMagicFrameDevice.ControlPanel.Press("cPaperSetup");
            _pacekeeper.Pause();

            //Click on Default paper size - cDefaultPaperSizeStr - cDefaultPaperSizeStr
            _phoenixMagicFrameDevice.ControlPanel.Press("cDefaultPaperType");
            _pacekeeper.Pause();

            //Getting the string dispalyed on the control panel and checking if the password is invalid
            if (_phoenixMagicFrameDevice.ControlPanel.GetSelectedListItem()
                .Equals(defaultPaperType, StringComparison.OrdinalIgnoreCase))
            {
                defaultPaperTypeResult = true;
            }

            //return home
            _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);

            if ((defaultPaperSizeResult == false) && (defaultPaperTypeResult == false))
            {
                throw new DeviceWorkflowException("Both Default Paper Type and Size are not Matching.");
            }

            if ((defaultPaperSizeResult == false))
            {
                throw new DeviceWorkflowException("Default Paper Size is not matching");
            }

            if ((defaultPaperTypeResult == false))
            {
                throw new DeviceWorkflowException("Default Paper Type is not matching");
            }
        }

        private void ValidateDefaultPaperSizeAndTypeNova(string defaultPaperSize, string defaultPaperType)
        {
            bool defaultPaperSizeResult = false, defaultPaperTypeResult = false;

            NavigateToSetupNova();

            if (
                _phoenixNovaDevice.ControlPanel.GetDisplayedStrings()
                    .Any(x => x.StartsWith("Enter Password", StringComparison.OrdinalIgnoreCase)))
            {
                _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(_phoenixNovaDevice.AdminPassword);
                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                if (
                    _phoenixNovaDevice.ControlPanel.GetDisplayedStrings()
                        .Any(x => x.StartsWith("Password", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new DeviceWorkflowException(
                        "Default Password not accepted, Please reset password or remove to continue");
                }
            }

            //Click on Paper set up - cPaperSetup
            _phoenixNovaDevice.ControlPanel.Press("cPaperSetup");
            _pacekeeper.Pause();

            //Click on Default paper size - cDefaultPaperSizeStr - cDefaultPaperSizeStr
            _phoenixNovaDevice.ControlPanel.Press("cDefaultPaperSizeStr");
            _pacekeeper.Pause();

            if (_phoenixNovaDevice.ControlPanel.GetSelectedListItem()
                .Equals(defaultPaperSize, StringComparison.OrdinalIgnoreCase))
            {
                defaultPaperSizeResult = true;
            }

            NavigateToSetupNova();

            if (
                _phoenixNovaDevice.ControlPanel.GetDisplayedStrings()
                    .Any(x => x.StartsWith("Enter Password", StringComparison.OrdinalIgnoreCase)))
            {
                _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(_phoenixNovaDevice.AdminPassword);
                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                if (
                    _phoenixNovaDevice.ControlPanel.GetDisplayedStrings()
                        .Any(x => x.StartsWith("Password", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new DeviceWorkflowException(
                        "Default Password not accepted, Please reset password or remove to continue");
                }
            }
            //Click on Paper set up - cPaperSetup
            _phoenixNovaDevice.ControlPanel.Press("cPaperSetup");
            _pacekeeper.Pause();

            //Click on Default paper size - cDefaultPaperSizeStr - cDefaultPaperSizeStr
            _phoenixNovaDevice.ControlPanel.Press("cDefaultPaperType");
            _pacekeeper.Pause();

            //Getting the string dispalyed on the control panel and checking if the password is invalid
            if (_phoenixNovaDevice.ControlPanel.GetSelectedListItem()
                .Equals(defaultPaperType, StringComparison.OrdinalIgnoreCase))
            {
                defaultPaperTypeResult = true;
            }

            //return home
            _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);

            if ((defaultPaperSizeResult == false) && (defaultPaperTypeResult == false))
            {
                throw new DeviceWorkflowException("Both Default Paper Type and Size are not Matching.");
            }
            if ((defaultPaperSizeResult == false))
            {
                throw new DeviceWorkflowException("Default Paper Size is not matching");
            }
            if ((defaultPaperTypeResult == false))
            {
                throw new DeviceWorkflowException("Default Paper Type is not matching");
            }
        }


        private void ValidateInkThresholdMagicFrame(string inkLabel, int inkValue)
        {
            _phoenixMagicFrameDevice.ControlPanel.Press(inkLabel);
            _pacekeeper.Pause();

            var resultCollection = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings();

            _phoenixMagicFrameDevice.ControlPanel.Press("cOKTouchButton");
            _pacekeeper.Pause();

            if (!resultCollection.Contains(inkValue.ToString()))
            {
                throw new DeviceWorkflowException($"Low Cartridge threshold values not matching for {inkLabel.Substring(13)}");
            }
        }

        private void ValidateInkThresholdNova(string inkLabel, int inkValue)
        {
            _phoenixNovaDevice.ControlPanel.Press(inkLabel);
            _pacekeeper.Pause();

            var resultCollection = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings();

            _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
            _pacekeeper.Pause();

            if (!resultCollection.Contains(inkValue.ToString()))
            {
                throw new DeviceWorkflowException(
                    $"Low Cartridge threshold values not matching for {inkLabel.Substring(13)}");
            }
        }

        /// <summary>
        /// Validates the low catridge threshold values set through EWS
        /// </summary>
        /// <param name="cyan">cyan ink level</param>
        /// <param name="magenta">magenta ink level</param>
        /// <param name="yellow">yellow ink level</param>
        /// <param name="black">black ink level</param>
        [Description("Validates ink threshold levels for Cyan, Magenta, Yellow and Black")]
        public void ValidateLowCartridgeThreshold(int cyan = 10, int magenta = 10, int yellow = 10, int black = 10)
        {
            if (cyan == 0 || magenta == 0 || yellow == 0 || black == 0)
            {
                throw new DeviceWorkflowException("Invalid threshold value for ink values");

            }

            if (_phoenixNovaDevice != null)
            {
                ValidateLowCartridgeThresholdNova(cyan, magenta, yellow, black);
            }
            else
            {
                ValidateLowCartridgeThresholdMagicFrame(cyan, magenta, yellow, black);
            }
        }

        private void ValidateLowCartridgeThresholdMagicFrame(int cyan, int magenta, int yellow, int black)
        {
            NavigateToSetupMagicFrame();

            if (
                _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings()
                    .Any(x => x.StartsWith("Enter Password", StringComparison.OrdinalIgnoreCase)))
            {
                _phoenixMagicFrameDevice.ControlPanel.TypeOnVirtualKeyboard(_phoenixMagicFrameDevice.AdminPassword);
                _phoenixMagicFrameDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                if (
                    _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings()
                        .Any(x => x.StartsWith("Password", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new DeviceWorkflowException(
                        "Default Password not accepted, Please reset password or remove to continue");
                }
            }

            if (
                !_phoenixMagicFrameDevice.ControlPanel.GetVirtualButtons()
                    .Any(x => x.Name.StartsWith("cCPSupplySettings", StringComparison.OrdinalIgnoreCase)))
            {
                _phoenixMagicFrameDevice.ControlPanel.Press("Scroll Down Arrow");
                _pacekeeper.Pause();
            }
            _phoenixMagicFrameDevice.ControlPanel.Press("cCPSupplySettings");
            _pacekeeper.Pause();

            //let's validate black first

            //Press on Black Cartridge
            _phoenixMagicFrameDevice.ControlPanel.Press("cCPSPSBlackCartridge");
            _pacekeeper.Pause();
            //Press on Low Threshold
            _phoenixMagicFrameDevice.ControlPanel.Press("cLowThreshold");
            _pacekeeper.Pause();

            if (_phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings().Contains(black.ToString()))
            {
                _phoenixMagicFrameDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();
                _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Back);
                _pacekeeper.Pause();
            }
            else
            {
                throw new DeviceWorkflowException("Low Cartridge threshold value not matching for Black");
            }

            if (_phoenixMagicFrameDevice.ControlPanel.GetVirtualButtons()
                .Any(n => n.Name == "cCPSPSColorCartridges"))
            {
                _phoenixMagicFrameDevice.ControlPanel.Press("cCPSPSColorCartridges");
                _pacekeeper.Pause();
                //Press on Low Threshold
                _phoenixMagicFrameDevice.ControlPanel.Press("cLowThreshold");
                _pacekeeper.Pause();

                //lets check cyan

                ValidateInkThresholdMagicFrame("cCPSPSLowMenuCyan", cyan);

                ValidateInkThresholdMagicFrame("cCPSPSLowMenuMagenta", magenta);

                ValidateInkThresholdMagicFrame("cCPSPSLowMenuYellow", yellow);
            }
            else
            {
                throw new DeviceWorkflowException("The device is a mono printer, activity skipped");
            }
        }

        private void ValidateLowCartridgeThresholdNova(int cyan, int magenta, int yellow, int black)
        {
            NavigateToSetupNova();

            if (
                _phoenixNovaDevice.ControlPanel.GetDisplayedStrings()
                    .Any(x => x.StartsWith("Enter Password", StringComparison.OrdinalIgnoreCase)))
            {
                _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(_phoenixNovaDevice.AdminPassword);
                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                if (
                    _phoenixNovaDevice.ControlPanel.GetDisplayedStrings()
                        .Any(x => x.StartsWith("Password", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new DeviceWorkflowException(
                        "Default Password not accepted, Please reset password or remove to continue");
                }
            }

            _phoenixNovaDevice.ControlPanel.ScrollDown(40);
            _pacekeeper.Pause();

            _phoenixNovaDevice.ControlPanel.Press("cCPSupplySettings");
            _pacekeeper.Pause();

            //let's validate black first

            //Press on Black Cartridge
            _phoenixNovaDevice.ControlPanel.Press("cCPSPSBlackCartridge");
            _pacekeeper.Pause();
            //Press on Low Threshold
            _phoenixNovaDevice.ControlPanel.Press("cLowThreshold");
            _pacekeeper.Pause();

            if (_phoenixNovaDevice.ControlPanel.GetDisplayedStrings().Contains(black.ToString()))
            {
                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();
                _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Back);
                _pacekeeper.Pause();
            }
            else
            {
                throw new DeviceWorkflowException("Low Cartridge threshold value not matching for Black");
            }

            if (_phoenixNovaDevice.ControlPanel.GetVirtualButtons().Any(n => n.Name == "cCPSPSColorCartridges"))
            {
                _phoenixNovaDevice.ControlPanel.Press("cCPSPSColorCartridges");
                _pacekeeper.Pause();
                //Press on Low Threshold
                _phoenixNovaDevice.ControlPanel.Press("cLowThreshold");
                _pacekeeper.Pause();

                //lets check cyan

                ValidateInkThresholdNova("cCPSPSLowMenuCyan", cyan);

                ValidateInkThresholdNova("cCPSPSLowMenuMagenta", magenta);

                ValidateInkThresholdNova("cCPSPSLowMenuYellow", yellow);
            }
            else
            {
                throw new DeviceWorkflowException("The device is a mono printer, activity skipped");
            }
        }

        /// <summary>
        /// windowsAuthenticatinforCopyApp
        /// </summary>
        [Description("Performs Windows Authentication for Copy")]
        public void WindowsAuthenticationforCopyApp(string password = "admin")
        {
            try
            {
                _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cCopyHomeTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("UsernameTextbox");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(_credential.UserName);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("PasswordTextbox");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.TypeOnVirtualKeyboard(password);
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cOKTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.Press("cSignInTouchButton");
                _pacekeeper.Pause();

                _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Windows Authentication failed with exception:{ex.Message}");
            }
        }
    }
}
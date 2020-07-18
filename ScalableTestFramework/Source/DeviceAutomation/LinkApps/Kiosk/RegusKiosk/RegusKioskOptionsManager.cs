using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk
{
    /// <summary>
    /// Control RegusKiosk options
    /// </summary>
    public class RegusKioskOptionsManager
    {
        private SESLib _controller;
        private JetAdvantageLinkControlHelper _controlHelper;
        private string _packageName;        
        private JetAdvantageLinkUI _linkUI;

        /// <summary>
        /// Initialize <see cref="RegusKioskOptionsManager"/> class
        /// </summary>
        /// <param name="linkUI">target JetAdvantageLink UI to contorl</param> 
        /// <param name="packageName">package name to control</param>
        public RegusKioskOptionsManager(JetAdvantageLinkUI linkUI, string packageName)
        {
            _linkUI = linkUI;
            _controller = linkUI.Controller;
            _controlHelper = new JetAdvantageLinkControlHelper(linkUI, packageName);
            _packageName = packageName;
        }

        /// <summary>
        /// Set Color mode
        /// </summary>
        /// <param name="colorMode">output sides to set</param>
        public void SetColorMode(RegusKioskColorMode colorMode)
        {            
            if(!SetOption(colorMode.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set ColorMode :: {colorMode.GetDescription()}");
            }            
        }

        /// <summary>
        /// Set Original size
        /// </summary>
        /// <param name="originalSize">Original size to set</param>
        public void SetOriginalSize(RegusKioskOriginalSize originalSize)
        {
            if (!SetOption(originalSize.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set original size :: {originalSize.GetDescription()}");
            }
        }

        /// <summary>
        /// Set N-Up
        /// </summary>
        /// <param name="nUp">N-Up to set</param>
        public void SetNUp(RegusKioskNUp nUp)
        {
            if (!SetOption(nUp.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set N-Up :: {nUp.GetDescription()}");
            }
        }

        /// <summary>
        /// Set originalOrientation
        /// </summary>
        /// <param name="originalOrientation">originalOrientation to set</param>
        public void SetOriginalOrientation(RegusKioskOriginalOrientation originalOrientation)
        {
            if (!SetOption(originalOrientation.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set original orientation :: {originalOrientation.GetDescription()}");
            }
        }

        /// <summary>
        /// Set Duplex Original
        /// </summary>
        /// <param name="duplex">Duplex to set for original sides</param>
        public void SetDuplexOriginal(RegusKioskDuplexSided duplex)
        {
            if (!SetOption(duplex.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set Duplex Original :: {duplex.GetDescription()}");
            }
        }

        /// <summary>
        /// Set Image Rotation
        /// </summary>
        /// <param name="imageRotation">ImageRotaion to set</param>
        public void SetImageRotation(RegusKioskImageRotation imageRotation)
        {
            if (!SetOption(imageRotation.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set Image Rotation :: {imageRotation.GetDescription()}");
            }
        }

        /// <summary>
        /// Set Duplex Output
        /// </summary>
        /// <param name="duplex">Duplex to set for output sides</param>
        public void SetDuplexOutput(RegusKioskDuplexSided duplex)
        {
            int index = 1;

            if (!SetOption(duplex.GetDescription(), index))
            {
                throw new DeviceWorkflowException($"Can not set Duplex Output :: {duplex.GetDescription()}");
            }
        }

        /// <summary>
        /// Set Duplex Output
        /// </summary>
        /// <param name="reduceEnlarge">Reduce/Enlarge to set</param>
        public void SetReduceEnlarge(string reduceEnlarge)
        {
            string percent = reduceEnlarge.Split(' ')[0];
            if (!string.IsNullOrEmpty(percent))
            {
                if (!_linkUI.Controller.Click(new UiSelector().TextContains(percent)))
                {
                    throw new DeviceWorkflowException($"Can not set reduceEnlarge :: {reduceEnlarge} ({percent})");
                }                
            }           
        }

        /// <summary>
        /// Set file format
        /// </summary>
        /// <param name="fileFormat">file format to set</param>
        public void SetFileFormat(RegusKioskFileFormat fileFormat)
        {
            if (!SetOption(fileFormat.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set file format :: {fileFormat.GetDescription()}");
            }
        }
        
        /// <summary>
        /// Set file name
        /// </summary>
        /// <param name="fileName">file name to set</param>
        public void SetFileName(string fileName)
        {
            if(!string.IsNullOrEmpty(fileName))
            {
                if (!_controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/scan_fileName_input")))
                {
                    throw new DeviceWorkflowException($"Can not find control for scan to usb job :: file name");
                }

                if (!_controller.SetText(new UiSelector().ResourceId($"{_packageName}:id/scan_fileName_input"), fileName))
                {
                    throw new DeviceWorkflowException($"Can not set file name :: {fileName}");
                }

                Thread.Sleep(2000);
                _linkUI.Controller.PressKey(KeyCode.KEYCODE_BACK);
            }
        }

        /// <summary>
        /// Set field for to address
        /// </summary>
        /// <param name="toAddress">set the field for to address</param>
        public void SetToAddress(string toAddress)
        {
            if (!string.IsNullOrEmpty(toAddress))
            {
                if (!_controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/scan_toAddr_input")))
                {
                    throw new DeviceWorkflowException($"Can not find control for scan to email job :: to address field");
                }

                if (!_controller.SetText(new UiSelector().ResourceId($"{_packageName}:id/scan_toAddr_input"), toAddress))
                {
                    throw new DeviceWorkflowException($"Can not set to address :: {toAddress}");
                }
            }
        }

        /// <summary>
        /// Set resolution
        /// </summary>
        /// <param name="resolution">resolution to set</param>
        public void SetResolution(RegusKioskResolution resolution)
        {
            if (!SetOption(resolution.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set file format :: {resolution.GetDescription()}");
            }
        }

        /// <summary>
        /// Set duplex
        /// </summary>
        /// <param name="duplex">duplex to set</param>
        public void SetDuplexPrint(RegusKioskDuplexPrint duplex)
        {
            if (!SetOption(duplex.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set duplex :: {duplex.GetDescription()}");
            }
        }

        /// <summary>
        /// Set paper source
        /// </summary>
        /// <param name="papersource">paper source to set</param>
        public void SetPaperSource(RegusKioskOriginalSize papersource)
        {
            if(!_controller.Click(new UiSelector().ResourceId($"{_packageName}:id/paper_src_btn")))
            {
                throw new DeviceWorkflowException($"Failed to click Paper Source list :: {papersource.GetDescription()}");
            }

            if (!SetOption(papersource.GetDescription()))
            {
                throw new DeviceWorkflowException($"Can not set paper source :: {papersource.GetDescription()}");
            }
        }

        /// <summary>
        /// Set Auto Fit
        /// </summary>
        /// <param name="autofit">autofit to set</param>
        public void SetAutoFit(bool autofit)
        {
            if (!autofit)
            {
                if(!_controller.Click(new UiSelector().ResourceId($"{_packageName}:id/auto_fit_btn_switch")))
                {
                    throw new DeviceWorkflowException($"Can not set Auto fit :: {autofit}");
                }
            }
        }

        /// <summary>
        /// Set Copies for Copy job
        /// </summary>
        /// <param name="copies"> Copies to set</param>
        public void SetCopyCopies(int copies)
        {
            if(copies != 1)
            {
                if (!_controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/iet_set_number_copy")))
                {
                    throw new DeviceWorkflowException("Can not find control for copy job :: Number of copies");
                }

                for (int count = 1; count < copies; count++)
                {
                    if (!_controller.Click(new UiSelector().ResourceId($"{_packageName}:id/im_number_plus")))
                    {
                        throw new DeviceWorkflowException($"Can not set copies for copy job :: Number of copies ({copies})");
                    }
                }

                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Set Copies for Print job
        /// </summary>
        /// <param name="copies"> Copies to set</param>
        public void SetPrintCopies(int copies)
        {
            if (copies != 1)
            {
                if (!_controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/print_set_number_copy")))
                {
                    throw new DeviceWorkflowException("Can not find control for print job :: Number of copies");
                }
                
                if (!_controller.SetText(new UiSelector().ResourceId($"{_packageName}:id/print_set_number_copy"),copies.ToString()))
                {
                    throw new DeviceWorkflowException($"Can not set copies for print job :: Number of copies ({copies})");
                }

                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Set Options without index
        /// </summary>
        /// <param name="optionText">Text to set option</param>
        private bool SetOption(string optionText)
        {
            bool result = false;

            if (_controller.IsEnabled(new UiSelector().Text(optionText)))
            {
                result = _controller.Click(new UiSelector().Text(optionText));
            }
            else
            {
                throw new DeviceWorkflowException($"Selected object is not existed or enabled: {optionText}");
            }

            return result;
        }

        /// <summary>
        /// Set Options without index
        /// </summary>
        /// <param name="optionText">Text to set option</param>
        /// <param name="index">Index to set option</param>
        private bool SetOption(string optionText, int index)
        {
            bool result = false;

            if (_controller.IsEnabled(new UiSelector().Text(optionText), index))
            {
                result = _controller.Click(new UiSelector().Text(optionText), index);
            }
            else
            {
                throw new DeviceWorkflowException($"Selected object is not existed or enabled {optionText}");
            }

            return result;
        }
    }
}

using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System.Threading;
using System;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink
{
    /// <summary>
    /// Helpers that set the options for JetAdvantageLink Scan Options
    /// </summary>
    public class JetAdvantageLinkScanOptionManager
    {
        private SESLib _controller;
        private JetAdvantageLinkControlHelper _controlHelper;
        private string _packageName;

        /// <summary>
        /// Initialize <see cref="JetAdvantageLinkScanOptionManager"/> class
        /// </summary>
        /// <param name="linkUI">target JetAdvantageLink UI to contorl</param>
        /// <param name="packageName">package name to control</param>
        public JetAdvantageLinkScanOptionManager(JetAdvantageLinkUI linkUI, string packageName)
        {
            _controller = linkUI.Controller;
            _controlHelper = new JetAdvantageLinkControlHelper(linkUI, packageName);
            _packageName = packageName;
        }

        /// <summary>
        /// Call to SetOptionsScreen on the JetAdvantageLinkControlHelper
        /// It use for set to the "Optoins" screen
        /// </summary>
        public void SetOptionsScreen()
        {
            _controlHelper.SetOptionsScreen();
        }

        /// <summary>
        /// Set Original Sides for scan source
        /// </summary>
        /// <param name="originalSides">original sides to set</param>
        public void SetOriginalSides(LinkScanOriginalSides originalSides)
        {
            if(!originalSides.Equals(GetOriginalSides()))
            {
                bool result = true;
                result &= _controller.Click(new UiSelector().Text("Original Sides"));
                switch(originalSides)
                {
                    case LinkScanOriginalSides.Onesided:
                        if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("1-sided"), 200, 300))
                        {
                            Thread.Sleep(1000);
                            result &= _controller.Click(new UiSelector().Text("1-sided"));
                        }
                        break;
                    case LinkScanOriginalSides.Twosided:
                        if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("2-sided"), 200, 300))
                        {
                            Thread.Sleep(1000);
                            result &= _controller.Click(new UiSelector().Text("2-sided"));
                        }
                        break;
                    case LinkScanOriginalSides.Pagesflipup:
                        if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("2-sided"), 200, 300))
                        {
                            Thread.Sleep(1000);
                            result &= _controller.Click(new UiSelector().Text("2-sided"));
                        }
                        result &= _controller.Click(new UiSelector().ResourceId($"{_packageName}:id/chk_check"));
                        break;
                }

                if (!result)
                {
                    throw new DeviceWorkflowException($"Can not set option :: Original Sides to {originalSides.GetDescription()}");
                }
            }
        }

        /// <summary>
        /// Get current option for original sides
        /// </summary>
        /// <returns>current value of <see cref="LinkScanOriginalSides"/></returns>
        /// <exception cref="DeviceWorkflowException">Throws if string is not matched</exception>
        public LinkScanOriginalSides GetOriginalSides()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("Original Sides");
                return EnumUtil.GetByDescription<LinkScanOriginalSides>(currentOption);
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    throw new DeviceWorkflowException($"Can not set option :: GetOriginalSides is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);                    
                }
                throw new DeviceWorkflowException($"Can not set option :: GetOriginalSides is failed :: {ex.ToString()}", ex);
            }
        }

        /// <summary>
        /// Set File type and Resolution for scan output
        /// </summary>
        /// <param name="fileType">file type to set</param>
        /// <param name="resolution">resolution to set</param>
        public void SetFileTypeAndResolution (LinkScanFileType fileType, LinkScanResolution resolution)
        {
            Tuple<LinkScanFileType, LinkScanResolution> currentOption = GetFileTypeAndResolution();
            if (!fileType.Equals(currentOption.Item1) || !resolution.Equals(currentOption.Item2))
            {
                bool result = true;
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("File Type and Resolution"), 200, 300))
                {
                    result &= _controller.Click(new UiSelector().Text("File Type and Resolution"));
                }

                if (!fileType.Equals(currentOption.Item1))
                {
                    if(result &= _controlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{_packageName}:id/btn_file_type")))
                    {
                        result &= _controller.Click(new UiSelector().ResourceId($"{_packageName}:id/btn_file_type"));
                    }
                    if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{_packageName}:id/lv_option_sub_list")))
                    {
                        result &= _controlHelper.ClickOnListWithScroll(new UiSelector().ResourceId($"{_packageName}:id/lv_option_sub_list"), new UiSelector().Text(fileType.GetDescription()));
                    }
                }

                if (!resolution.Equals(currentOption.Item2))
                {
                    if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{_packageName}:id/btn_resolution")))
                    {
                        result &= _controller.Click(new UiSelector().ResourceId($"{_packageName}:id/btn_resolution"));
                    }
                    if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text(resolution.GetDescription())))
                    {
                        result &= _controller.Click(new UiSelector().Text(resolution.GetDescription()));
                    }
                }
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{_packageName}:id/bt_done")))
                {
                    result &= _controller.Click(new UiSelector().ResourceId($"{_packageName}:id/bt_done"));
                }

                if (!result)
                {
                    throw new DeviceWorkflowException($"Can not set option :: File Type and Resolution to {fileType.GetDescription()} and {resolution.GetDescription()}");
                }
            }

        }

        /// <summary>
        /// Get current File type and Resolution option
        /// </summary>
        /// <returns>current value of <see cref="LinkScanFileType"/> and <see cref="LinkScanResolution"/></returns>
        public Tuple<LinkScanFileType, LinkScanResolution> GetFileTypeAndResolution()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("File Type and Resolution");
                string fileType = currentOption.Split(',')[0].Trim();
                string resolution = currentOption.Split(',')[1].Trim();

                return new Tuple<LinkScanFileType, LinkScanResolution>((EnumUtil.GetByDescription<LinkScanFileType>(fileType)), (EnumUtil.GetByDescription<LinkScanResolution>(resolution)));
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    throw new DeviceWorkflowException($"Can not set option :: GetFileTypeAndResolution is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);                    
                }
                throw new DeviceWorkflowException($"Can not set option :: GetFileTypeAndResolution is failed ::{ex.ToString()}", ex);
            }
        }

        /// <summary>
        /// Set Color/Black Option for scan output
        /// </summary>
        /// <param name="colorOption">Color/Black Option for set</param>
        public void SetColorBlack (LinkScanColorBlack colorOption)
        {
            if (!colorOption.Equals(GetColorBlack()))
            {
                bool result = true;
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Color/Black"), 200, 300))
                {
                    result &= _controller.Click(new UiSelector().Text("Color/Black"));
                }
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text(colorOption.GetDescription()), 200, 300))
                {
                    Thread.Sleep(1000);
                    result &= _controller.Click(new UiSelector().Text(colorOption.GetDescription()));
                }
                if (!result)
                {
                    throw new DeviceWorkflowException($"Can not set option :: Color/Black to {colorOption.GetDescription()}");
                }
            }
        }

        /// <summary>
        /// Get current Color/Black option
        /// </summary>
        /// <returns>current value of <see cref="LinkScanColorBlack"/></returns>
        public LinkScanColorBlack GetColorBlack()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("Color/Black");
                return EnumUtil.GetByDescription<LinkScanColorBlack>(currentOption);
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    throw new DeviceWorkflowException($"Can not set option :: GetColorBlack is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);                    
                }
                throw new DeviceWorkflowException($"Can not set option :: GetColorBlack is failed :: {ex.ToString()}", ex);
            }
        }
        /// <summary>
        /// Set Original Size of scan input
        /// </summary>
        /// <param name="originalSize">Original size to set</param>
        public void SetOriginalSize(LinkScanOriginalSize originalSize)
        {
            if (!originalSize.Equals(GetOriginalSize()))
            {
                bool result = true;
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Original Size"), 200, 300))
                {
                    result &= _controller.Click(new UiSelector().Text("Original Size"));
                }

                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{_packageName}:id/lv_option_sub_list"), 200, 300))
                {
                    Thread.Sleep(1000);
                    result &= _controlHelper.ClickOnListWithScroll(new UiSelector().ResourceId($"{_packageName}:id/lv_option_sub_list"), new UiSelector().Text(originalSize.GetDescription()));
                }

                if (!result)
                {
                    throw new DeviceWorkflowException($"Can not set option :: Original Size to {originalSize.GetDescription()}");
                }
            }
        }

        /// <summary>
        /// Get current Original Size
        /// </summary>
        /// <returns>current value of <see cref="LinkScanOriginalSize"/></returns>
        public LinkScanOriginalSize GetOriginalSize()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("Original Size");
                return EnumUtil.GetByDescription<LinkScanOriginalSize>(currentOption);
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    throw new DeviceWorkflowException($"Can not set option :: GetOriginalSize is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);
                    
                }
                throw new DeviceWorkflowException($"Can not set option :: GetOriginalSize is failed :: {ex.ToString()}", ex);
            }
        }

        /// <summary>
        /// Set Content Orientation for scan input
        /// </summary>
        /// <param name="orientation">Orientation to set</param>
        public void SetOrientation(LinkScanContentOrientation orientation)
        {
            if (!orientation.Equals(GetOrientation()))
            {
                bool result = true;
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Content Orientation"), 200, 300))
                {
                    result &= _controller.Click(new UiSelector().Text("Content Orientation"));
                }
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text(orientation.GetDescription()), 200, 300))
                {
                    Thread.Sleep(1000);
                    result &= _controller.Click(new UiSelector().Text(orientation.GetDescription()));
                }

                if (!result)
                {
                    throw new DeviceWorkflowException($"Can not set option :: Orientation to {orientation.GetDescription()}");
                }
            }
        }

        /// <summary>
        /// Get current Content Orientation option
        /// </summary>
        /// <returns>current value of <see cref="LinkScanContentOrientation"/></returns>
        public LinkScanContentOrientation GetOrientation()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("Content Orientation");
                if (string.IsNullOrEmpty(currentOption))
                {
                    return LinkScanContentOrientation.Portrait;
                }

                return EnumUtil.GetByDescription<LinkScanContentOrientation>(currentOption);
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    throw new DeviceWorkflowException($"Can not set option :: GetOrientation is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);
                }
                throw new DeviceWorkflowException($"Can not set option :: GetOrientation is failed :: {ex.ToString()}", ex);
            }
        }

        /// <summary>
        /// Set Filename of scan output
        /// </summary>
        /// <param name="fileName">File name to set</param>
        /// <param name="resourceId">Ui selector resource id to insput file name</param>
        public void SetFileName(string fileName, string resourceId = null)
        {
            if (!_controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/bt_hide_options")))                
            {
                throw new DeviceWorkflowException("App screen is not on the option setting");
            }
            if ("Hide Options".Equals(_controller.GetText(new UiSelector().ResourceId($"{_packageName}:id/bt_hide_options"))))
            {
                _controller.Click(new UiSelector().ResourceId($"{_packageName}:id/bt_hide_options"));
            }
            if(System.String.IsNullOrEmpty(resourceId))
            {
                if (!_controller.SetText(new UiSelector().ResourceId($"{_packageName}:id/editTextDescription"), fileName))
                {
                    throw new DeviceWorkflowException($"File name setting failed to {fileName}");
                }
            }
            else
            {
                if (!_controller.SetText(new UiSelector().ResourceId($"{_packageName}:id/{resourceId}"), fileName))
                {
                    throw new DeviceWorkflowException($"File name setting failed to {fileName}");
                }
            }

        }
    }
}

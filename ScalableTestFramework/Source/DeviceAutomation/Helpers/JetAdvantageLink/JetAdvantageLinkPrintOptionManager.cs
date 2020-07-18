using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink
{
    /// <summary>
    /// Helpers that set the options for JetAdvantageLink Print Options
    /// </summary>
    /// <exception cref="DeviceWorkflowException">This exception is thrown when set option is faild</exception>
    public class JetAdvantageLinkPrintOptionManager
    {
        private SESLib _controller;
        private JetAdvantageLinkControlHelper _controlHelper;
        private string _packageName;

        /// <summary>
        /// Initialize <see cref="JetAdvantageLinkPrintOptionManager"/> class
        /// </summary>
        /// <param name="linkUI">target JetAdvantageLink UI to contorl</param> 
        /// <param name="packageName">package name to control</param>
        public JetAdvantageLinkPrintOptionManager(JetAdvantageLinkUI linkUI, string packageName)
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
        /// Set Output sides for print output
        /// </summary>
        /// <param name="outputSides">output sides to set</param>
        public void SetOutputSides(LinkPrintOutputSides outputSides)
        {
            if (!outputSides.Equals(GetOutputSides()))
            {
                bool result = true;
                if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Output Sides")))
                {
                    result &= _controller.Click(new UiSelector().Text("Output Sides"));
                }
                switch (outputSides)
                {
                    case LinkPrintOutputSides.Onesided:
                        if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("1-sided")))
                        {
                            result &= _controller.Click(new UiSelector().Text("1-sided"));
                        }
                        break;
                    case LinkPrintOutputSides.Twosided:
                        if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("2-sided")))
                        {
                            result &= _controller.Click(new UiSelector().Text("2-sided"));
                        }
                        break;
                    case LinkPrintOutputSides.Pagesflipup:
                        if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("2-sided")))
                        {
                            result &= _controller.Click(new UiSelector().Text("2-sided"));
                        }
                        if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{_packageName}:id/chk_check")))
                        {
                            result &= _controller.Click(new UiSelector().ResourceId($"{_packageName}:id/chk_check"));
                        }
                        break;
                }

                if (!result)
                {
                    throw new DeviceWorkflowException($"Can not set option :: Output Sides to {outputSides.GetDescription()}");
                }
            }
        }

        /// <summary>
        /// Get current output sides option
        /// </summary>
        /// <returns>current value of <see cref="LinkPrintOutputSides"/></returns>
        public LinkPrintOutputSides GetOutputSides()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("Output Sides");
                return EnumUtil.GetByDescription<LinkPrintOutputSides>(currentOption);
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    throw new DeviceWorkflowException($"Can not set option :: GetOutputSides is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);                    
                }
                throw new DeviceWorkflowException($"Can not set option :: GetOutputSides is failed :: {ex.ToString()}", ex);
            }
        }

        /// <summary>
        /// Set Color/Black option for output
        /// </summary>
        /// <param name="colorBlack">Color/Black option to set</param>
        public void SetColorBlack(LinkPrintColorBlack colorBlack)
        {
            try
            {
                if (!colorBlack.Equals(GetColorBlack()))
                {
                    bool result = true;
                    if ( result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Color/Black")))
                    {
                        result &= _controller.Click(new UiSelector().Text("Color/Black"));
                    }
                    if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text(colorBlack.GetDescription())))
                    {
                        result &= _controller.Click(new UiSelector().Text(colorBlack.GetDescription()));
                    }

                    if (!result)
                    {
                        throw new DeviceWorkflowException($"Can not set option :: Color/Black to {colorBlack.GetDescription()}");
                    }
                }
            }
            catch (OptionNotFoundException)
            {
                // ignore for Mono devices

            }
        }

        /// <summary>
        /// Get current Color/Black option
        /// </summary>
        /// <returns>current value of <see cref="LinkPrintColorBlack"/></returns>
        /// <exception cref="OptionNotFoundException">throws when can not find Color/Black options in the Link UI</exception>
        public LinkPrintColorBlack GetColorBlack()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("Color/Black");
                return EnumUtil.GetByDescription<LinkPrintColorBlack>(currentOption);
            }
            catch (ArgumentNullException)
            {
                throw new OptionNotFoundException("Failed to retrive option :: GetColorBlack");
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Can not set option :: GetColorblack is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);
                    throw e;
                }
                throw new DeviceWorkflowException($"Can not Get option :: GetColorBlack is failed :: {ex.ToString()}", ex);
            }

        }

        /// <summary>
        /// Set Color/Black option for output
        /// </summary>
        /// <param name="staple">Staple option to set</param>
        public void SetStaple(LinkPrintStaple staple)
        {
            try
            {
                if (!staple.Equals(GetStaple()))
                {
                    bool result = true;
                    if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Staple")))
                    {
                        result &= _controller.Click(new UiSelector().Text("Staple"));
                    }
                    if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text(staple.GetDescription())))
                    {
                        result &= _controller.Click(new UiSelector().Text(staple.GetDescription()));
                    }

                    if (!result)
                    {
                        throw new DeviceWorkflowException($"Can not set option :: Staple to {staple.GetDescription()}");
                    }
                }
            }
            catch (OptionNotFoundException)
            {
                // ignore for not supported devices (without finisher)
            }
        }

        /// <summary>
        /// Get current Staple option
        /// </summary>
        /// <returns>current value of <see cref="LinkPrintStaple"/></returns>
        /// /// <exception cref="OptionNotFoundException">throws when can not find Staple options in the Link UI</exception>
        public LinkPrintStaple GetStaple()
        {
            try
            {
                string currentOption = _controlHelper.GetCurrentOption("Staple");
                return EnumUtil.GetByDescription<LinkPrintStaple>(currentOption);
            }
            catch (ArgumentNullException)
            {
                throw new OptionNotFoundException("Failed to retrive option :: GetStaple");
            }
            catch (Exception ex)
            {
                if (_controlHelper.CheckRetainPopup())
                {
                    throw new DeviceWorkflowException($"Can not set option :: GetStaple is failed, Error message is {_controlHelper.GetPopupMessage()}", ex);                    
                }
                throw new DeviceWorkflowException($"Can not set option :: GetStaple is failed :: {ex.ToString()}", ex);
            }
        }

        /// <summary>
        /// Set Paper Size and Paper Tray option
        /// </summary>
        /// <param name="paperSize">Paper size to set</param>
        /// <param name="paperTray">Paper tray to set</param>
        public void SetPaperSelection(LinkPrintPaperSize paperSize, LinkPrintPaperTray paperTray)
        {   
            bool result = true;

            if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Paper Selection")))
            {
                result &= _controller.Click(new UiSelector().Text("Paper Selection"));
            }
                        
            if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Paper Size").ResourceId($"{_packageName}:id/tv_title")))
            {
                result &= _controller.Click(new UiSelector().Text("Paper Size").ResourceId($"{_packageName}:id/tv_title"));
            }
            Thread.Sleep(1000);
            result &= _controlHelper.ClickOnListWithScroll(new UiSelector().ResourceId($"{_packageName}:id/lv_option_sub_list"), new UiSelector().Text(paperSize.GetDescription()));
            
                        
            if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text("Paper Tray").ResourceId($"{_packageName}:id/tv_title")))
            {
                result &= _controller.Click(new UiSelector().Text("Paper Tray").ResourceId($"{_packageName}:id/tv_title"));
            }
            if (result &= _controlHelper.WaitingObjectAppear(new UiSelector().Text(paperTray.GetDescription()).ResourceId($"{_packageName}:id/tv_title")))
            {
                result &= _controller.Click(new UiSelector().Text(paperTray.GetDescription()).ResourceId($"{_packageName}:id/tv_title"));
            }            

            if (!result)
            {
                throw new DeviceWorkflowException($"Can not set option :: Paper Size and Paper Tray to {paperSize.GetDescription()} and {paperTray.GetDescription()}");
            }            
        }
        
        /// <summary>
        /// Set number of coipes for print
        /// </summary>
        /// <param name="copies">number of copies</param>
        public void SetNumberOfCopies(int copies)
        {
            if(!_controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/et_number_of_copies")))
            {
                throw new DeviceWorkflowException("Can not find contorl :: Number of copies");
            }
            bool result = true;
            result = _controller.SetText(new UiSelector().ResourceId($"{_packageName}:id/et_number_of_copies"), copies.ToString());

            if(!result)
            {
                throw new DeviceWorkflowException($"Can not set option :: Number of copies to {copies}");
            }
        }

    }

} 
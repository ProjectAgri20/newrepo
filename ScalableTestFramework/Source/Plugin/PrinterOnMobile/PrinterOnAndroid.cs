using HP.ScalableTest.PluginSupport.MobilePrintApp;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace HP.ScalableTest.Plugin.PrinterOnMobile
{
    /// <summary>
    /// Represent Android App of PrinterOn
    /// </summary>
    public class PrinterOnAndroid : IMobilePrintApp, IDisposable
    {
        private const string PACKAGE_NAME = "com.printeron.droid.phone";
        private const string MAIN_ACTIVITY = "com.printeron.droid.phone/com.printeron.droid.phone.activity.MainActivity";
        private const int DEFAULT_TIMEOUT = 5;

        private string _deviceIdentifier;
        private SESLib _mobile = null;

        private string _name;
        private string _email;
        
        public PrinterOnAndroid(string deviceIdentifier)
        {
            _deviceIdentifier = deviceIdentifier;
            _mobile = SESLib.Create(_deviceIdentifier);
            _mobile.Connect();
            _mobile.SetTimeout(DEFAULT_TIMEOUT);

        }

        public string GetAppName()
        {
            return "PrinterOnAndroid";
        }

        public void CloseApp()
        {
            int count = 0;
            _mobile.SetTimeout(1);
            while(!_mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/main_entry_function_panel")))
            {
                _mobile.PressKey(4);
                count++;
                if(count > 5)
                {
                    _mobile.ExecuteADBCommand($"shell am force-stop {PACKAGE_NAME}");
                    _mobile.SetTimeout(DEFAULT_TIMEOUT);
                    return;
                }
            }
            _mobile.SetTimeout(DEFAULT_TIMEOUT);
            _mobile.PressKey(4);
        }

        public void Dispose()
        {
            if(_mobile != null)
            {
                _mobile.Disconnect();
            }
        }

        /// <summary>
        /// Launch PrinterOn App.
        /// </summary>
        /// <remarks>This method will kill current activated PrinterOn App due to stability issue.</remarks>
        /// <returns></returns>
        public bool LaunchApp()
        {
            _mobile.ExecuteADBCommand($"shell am force-stop {PACKAGE_NAME}");
            _mobile.StartActivity(MAIN_ACTIVITY);
            return _mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/main_entry_function_panel"));
        }

        /// <summary>
        /// Click Print Button
        /// </summary>
        public void Print()
        {
            if(!_mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/print_action_print_button")))
            {
                throw new MobileWorkflowException("Can not click Print button");
            }

            if(!string.IsNullOrEmpty(_name))
            {
                _mobile.SetText(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_job_accounting_network_login_edit"), _name);
            }

            if(!string.IsNullOrEmpty(_email))
            {
                _mobile.SetText(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_job_accounting_email_address_edit"), _email);
            }

            if(!_mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_setting_button_ok")))
            {
                throw new MobileWorkflowException("Can not file OK button at Print option popup");
            }
        }

        /// <summary>
        /// Select target printer
        /// </summary>
        /// <param name="printerIdentifier">Partial string of printer's name</param>
        public void SelectPrinter(string printerIdentifier)
        {
            _mobile.SetTimeout(0);
            bool inMain = _mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/module_entry_printers"));
            bool inPreview = _mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_preview_printer_selector"));
            if (!inMain && !inPreview)
            {
                _mobile.SetTimeout(DEFAULT_TIMEOUT);
                throw new MobileWorkflowException("Can not find Printer selection pane.");
            }

            if(inMain)
            {
                _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/module_entry_printers"));
            }
            else if(inPreview)
            {
                _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_preview_printer_selector"));
            }
            _mobile.SetTimeout(DEFAULT_TIMEOUT);
            if(!_mobile.Click(new UiSelector().TextContains(printerIdentifier)))
            {
                throw new MobileWorkflowException($"Can not find printer with given Printer Identifier {printerIdentifier}");
            }
            
            

        }

        /// <summary>
        /// Select target to print
        /// </summary>
        /// <param name="target">Only DocumentType.File is supported as of now</param>
        public void SelectTargetToPrint(ObjectToPrint target)
        {
            if(target.Type.Equals(DocumentType.File))
            {
                SelectDocumentToPrint(target.Infomation["FilePath"].ToString());
            }
            else
            {
                throw new NotSupportedException("Selected target is not supported");
            }
        }

        /// <summary>
        /// Select file on mobile app
        /// </summary>
        /// <param name="filePath">Internal stroage path</param>
        private void SelectDocumentToPrint(string filePath)
        {
            bool inMain = _mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/module_entry_document"));
            if(!inMain)
            {
                throw new MobileWorkflowException("App screen is not in the main.");
            }
            _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/module_entry_document"));
            _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/list_item_document_source_title"), 4);
            
            foreach(string s in filePath.Split('/'))
            {
                if(!ScrollAndClick(new UiSelector().TextContains(s), new UiSelector().ResourceId("android:id/list")))
                {
                    throw new MobileWorkflowException("Can not find specified file");
                }
            }

            if (!_mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_preview_view_pager")))
            {
                throw new MobileWorkflowException("Can not find preview pane");
            }
        }


        /// <summary>
        /// Wait until progress bar is gone.
        /// </summary>
        /// <param name="waitTime">Waiting time</param>
        public void WaitUntilPrintDone(TimeSpan waitTime)
        {
            Thread.Sleep(1000);
            WaitUntilDisappear(new UiSelector().ResourceId("android:id/progress"), TimeSpan.FromSeconds(30));
            if(!_mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/module_entry_print_job_history")))
            {
                if (!_mobile.Click("//*[@resource-id='com.printeron.droid.phone:id/module_entry_print_job_history']"))
                {
                    throw new MobileWorkflowException("Can not find job history button");
                }
                
            }
            
            if(_mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/list_item_print_job_info_status_progressbar")))
            {
                WaitUntilDisappear(new UiSelector().ResourceId("com.printeron.droid.phone:id/list_item_print_job_info_status_progressbar"), waitTime);
            }
        }

        /// <summary>
        /// Returns lastest job statis
        /// </summary>
        /// <returns>true if success</returns>
        public bool CheckPrintStatusOnMobile()
        {
            if(!_mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/list_item_print_job_info_status_imageview")))
            {
                if (!_mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/module_entry_print_job_history")))
                {
                    throw new MobileWorkflowException("Can not find job history button");
                }
            }

            _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/list_item_print_job_info_status_imageview"));

            _mobile.SetTimeout(2);
            bool ret = _mobile.DoesScreenContains(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_job_detail_release_code_group"));
            _mobile.SetTimeout(DEFAULT_TIMEOUT);
            return ret;
        }

        /// <summary>
        /// Set job options
        /// </summary>
        /// <param name="option"><see cref="PrinterOnJobOptions"/> should be given</param>
        public void SetOptions(MobilePrintJobOptions option)
        {
            PrinterOnJobOptions ponOption = (PrinterOnJobOptions)option;
            _name = ponOption.GetOption("name");
            _email = ponOption.GetOption("email");

            // If all default, skip option setting
            if(ponOption.Copies == -1 && ponOption.Page == null && ponOption.Orientation == null && 
                ponOption.Color == null && ponOption.Duplex == null && ponOption.PaperSize == null)
            {
                return;
            }

            // Click Option Setting
            if(!_mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/action_generic_settings")))
            {
                throw new MobileWorkflowException("Can not find option setting button");
            }

            // Set Copies
            if(ponOption.Copies != -1)
            {
                _mobile.SetText(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_copies_edit"), ponOption.Copies.ToString());
            }

            // Set Pages
            if(ponOption.Page != null)
            {
                if(ponOption.Page.ToLower().Equals("all"))
                {
                    _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_page_range_all_radio"));
                }
                else
                {
                    _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_page_range_selection_radio"));
                    _mobile.SetText(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_page_range_selection_edit"), ponOption.Page);
                }
            }

            // Set Orientation
            if(ponOption.Orientation != null)
            {
                _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_page_orientation_spinner"));
                int idx = 0;
                if(ponOption.Orientation.Equals(Option_Orientation.Portrait))
                {
                    idx = 0;
                }
                else if(ponOption.Orientation.Equals(Option_Orientation.Landscape))
                {
                    idx = 1;
                }

                _mobile.Click(new UiSelector().ClassName("android.widget.CheckedTextView"), idx);
            }

            // Set Duplex
            if (ponOption.Duplex != null)
            {
                _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_duplex_spinner"));
                int idx = 0;
                if (ponOption.Duplex.Equals(Option_Duplex.None))
                {
                    idx = 0;
                }
                else if (ponOption.Duplex.Equals(Option_Duplex.LongEdge))
                {
                    idx = 1;
                }
                else if (ponOption.Duplex.Equals(Option_Duplex.ShortEdge))
                {
                    idx = 2;
                }

                _mobile.Click(new UiSelector().ClassName("android.widget.CheckedTextView"), idx);
            }

            // Set Color
            if (ponOption.Color != null)
            {
                _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_color_spinner"));
                int idx = 0;
                if (ponOption.Color.Equals(Option_Color.BlackWhite))
                {
                    idx = 0;
                }
                else if (ponOption.Color.Equals(Option_Color.Color))
                {
                    idx = 1;
                }
                _mobile.Click(new UiSelector().ClassName("android.widget.CheckedTextView"), idx);
            }

            // Set Paper Size
            if (ponOption.PaperSize != null)
            {
                _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_option_media_size_spinner"));
                string textToClick = GetEnumDescription(ponOption.PaperSize);
                ScrollAndClick(new UiSelector().Text(textToClick), new UiSelector().ClassName("android.widget.ListView"));
            }

            // Click Ok Button
            _mobile.Click(new UiSelector().ResourceId("com.printeron.droid.phone:id/fragment_print_setting_button_ok"));
        
        }


        private bool ScrollAndClick(string uiSelector, string listItem)
        {
            _mobile.SetTimeout(0);
            _mobile.Swipe(listItem, SESLib.To.Down, 2);
            int count = 10;

            while(count > 0)
            {
                if(_mobile.DoesScreenContains(uiSelector))
                {
                    _mobile.SetTimeout(DEFAULT_TIMEOUT);
                    return _mobile.Click(uiSelector);
                }
                _mobile.Swipe(listItem, SESLib.To.Up, 20);
                count--;
            }
            _mobile.SetTimeout(DEFAULT_TIMEOUT);
            return false;
        }


        private void WaitUntilDisappear(string uiSelector, TimeSpan waitTime)
        {
            _mobile.SetTimeout(0);
            int sleepTime = 0;
            while(_mobile.DoesScreenContains(uiSelector))
            {
                Thread.Sleep(500);
                sleepTime += 500;
                if(sleepTime > waitTime.TotalMilliseconds)
                {
                    _mobile.SetTimeout(DEFAULT_TIMEOUT);
                    throw new MobileWorkflowException($"Item is not disappeared : {uiSelector}");
                }
            }
            _mobile.SetTimeout(DEFAULT_TIMEOUT);

        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        
    }
}

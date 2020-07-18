using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    internal class JediOmniCopyJobOptionsManager : JediOmniJobOptionsManager, ICopyJobOptions
    {
        private readonly TimeSpan _activeScreenTimeout = TimeSpan.FromSeconds(3);

        public JediOmniCopyJobOptionsManager(JediOmniDevice device)
            : base(device)
        {
        }

        /// <summary>
        /// Sets the color to print
        /// </summary>
        /// <param name="color">Color of the print</param>
        public void SetColor(string color)
        {
            OpenOptionsPanel();

            //For Color/Black Option
            if (_device.ControlPanel.CheckState("#hpid-copy-color-mode-button", OmniElementState.Exists))
            {
                //Select color/Black
                _device.ControlPanel.ScrollPressWait("#hpid-option-color-black", "#hpid-option-color-black-screen", TimeSpan.FromSeconds(2));
                Pacekeeper.Pause();

                //Color Option
                if (string.Equals(color, "Color", StringComparison.CurrentCultureIgnoreCase))
                {
                    _device.ControlPanel.ScrollPress("#hpid-color-black-selection-color");
                    Pacekeeper.Pause();
                }
                //For monochrome
                else if (string.Equals(color, "monochrome", StringComparison.CurrentCultureIgnoreCase))
                {
                    _device.ControlPanel.ScrollPress("#hpid-color-black-selection-grayscale");
                    Pacekeeper.Pause();
                }
                //For Automatically detect
                else
                {
                    _device.ControlPanel.ScrollPress("#hpid-color-black-selection-autodetect");
                    Pacekeeper.Pause();
                }
            }
        }

        /// <summary>
        /// Sets the number of copies to print
        /// </summary>
        /// <param name="copies">No of copies to be set</param>
        public void SetNumCopies(int copies)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            if (_deviceScreen.IsSmallSize)
            {
                //On small screens, the Options cover up the "Copy" button, so close it.
                CloseOptionsPanel();
            }

            if (! _device.ControlPanel.WaitForAvailable("#hpid-copy-start-copies", timeout))
            {
                throw new DeviceWorkflowException($"# of Copies textbox did not become available within {timeout.TotalSeconds} seconds.");
            }

            //Click on Copy start.
            _device.ControlPanel.Press("#hpid-copy-start-copies");
            Pacekeeper.Pause();

            if (! _device.ControlPanel.WaitForAvailable("#hpid-keypad-key-close", timeout))
            {
                throw new DeviceWorkflowException($"Keyboard did not close within {timeout.TotalSeconds} seconds.");
            }

            // Enter no. of copies
            _device.ControlPanel.TypeOnNumericKeypad(copies.ToString());
            //Close the numeric keypad
            _device.ControlPanel.Press("#hpid-keypad-key-close");
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Sets the Orientation - Portrait or Landscape
        /// </summary>
        /// <param name="orientation">Orientation type set Potrait or Landscape </param>
        public void SetOrientation(ContentOrientation orientation)
        {

            if (orientation != ContentOrientation.None)
            {
                OpenOptionsPanel();
                _device.ControlPanel.ScrollPressWait("#hpid-option-content-orientation", "#hpid-option-content-orientation-screen", _activeScreenTimeout);

                //Selecting Orientation value
                _device.ControlPanel.ScrollPress(string.Equals(orientation.ToString(), "Portrait", StringComparison.CurrentCultureIgnoreCase) ? "#hpid-content-orientation-selection-portrait" : "#hpid-content-orientation-selection-landscape");
                Pacekeeper.Pause();
            }

        }

        /// <summary>
        /// Set the Stamps for Top and Bottom 
        /// </summary>
        /// <param name="stampList">List containing Key-value Pair for Stamps to be set;Key is the StampType Enum and Value contains string to be set for the Stamptype</param>
        public void SetStamps(Dictionary<StampType, string> stampList)
        {
            OpenOptionsPanel();

            _device.ControlPanel.ScrollPressWait("#hpid-option-stamps", "#hpid-option-stamps-select-screen", _activeScreenTimeout);

            foreach (var item in stampList)
            {
                string stamptype = ($"{item.Key.ToString().ToLower()}stamp");

                if (_device.ControlPanel.CheckState($"#hpid-stamp-{ stamptype}", OmniElementState.Exists))
                {
                    _device.ControlPanel.ScrollPressWait($"#hpid-stamp-{ stamptype}", "#hpid-option-stamp-details-screen", _activeScreenTimeout);


                    if (_device.ControlPanel.CheckState("#hpid-stamp-content-textbox", OmniElementState.Exists))
                    {
                        _device.ControlPanel.ScrollPress("#hpid-stamp-content-textbox");
                        Pacekeeper.Pause();

                        _device.ControlPanel.TypeOnVirtualKeyboard(item.Value.ToString());

                        _device.ControlPanel.WaitForState("#hpid-keyboard-key-done", OmniElementState.Useable,
                           TimeSpan.FromSeconds(10));
                        _device.ControlPanel.Press("#hpid-keyboard-key-done");
                    }

                    if (_device.ControlPanel.WaitForState(".hp-button-done:last", OmniElementState.Useable, _activeScreenTimeout))
                    {
                        _device.ControlPanel.Press(".hp-button-done:last");
                    }
                }

            }

            if (_device.ControlPanel.WaitForState(".hp-button-done", OmniElementState.Useable, _activeScreenTimeout))
            {
                _device.ControlPanel.Press(".hp-button-done");
            }

            Pacekeeper.Pause();

        }

        /// <summary>
        /// Set text for Watermark
        /// </summary>
        /// <param name="watermarkText">Text to be set for watermark</param>
        public void SetWaterMark(string watermarkText)
        {
            OpenOptionsPanel();

            _device.ControlPanel.ScrollPressWait("#hpid-option-watermark", "#hpid-option-watermark-screen", _activeScreenTimeout);
            _device.ControlPanel.ScrollPress("#hpid-watermark-type-button");
            if (_device.ControlPanel.WaitForState("#hpid-watermark-type-list", OmniElementState.Useable))
            {
                _device.ControlPanel.ScrollPress("#hpid-watermark-type-list-textwatermark");

            }
            //Added a wait for the watermark textbox to be useable to enter values. Issue seen in a a few devices which are comparitively slow 
            if (_device.ControlPanel.WaitForState("#hpid-watermark-text-textbox", OmniElementState.Useable, _activeScreenTimeout))
            {
                _device.ControlPanel.ScrollPress("#hpid-watermark-text-textbox");
            }

            Pacekeeper.Pause();

            _device.ControlPanel.TypeOnVirtualKeyboard(watermarkText);

            _device.ControlPanel.WaitForState("#hpid-keyboard-key-done", OmniElementState.Useable,
               TimeSpan.FromSeconds(10));
            _device.ControlPanel.Press("#hpid-keyboard-key-done");


            if (_device.ControlPanel.WaitForState(".hp-button-done", OmniElementState.Useable, _activeScreenTimeout))
            {
                _device.ControlPanel.Press(".hp-button-done");
            }
            Pacekeeper.Pause();

        }

        /// <summary>
        /// Set Original size of paper
        /// </summary>
        /// <param name="sizeType">Set the paper size for Original Size</param>
        public void SetOriginalSize(OriginalSize sizeType)
        {
            if (!sizeType.Equals(OriginalSize.None))
            {
                OpenOptionsPanel();
                _device.ControlPanel.ScrollPressWait("#hpid-option-original-size", "#hpid-option-original-size-screen", _activeScreenTimeout);
                if (_device.ControlPanel.CheckState("#hpid-original-size-selection", OmniElementState.Exists))
                {
                    _device.ControlPanel.ScrollPress($"#hpid-original-size-selection-{sizeType.ToString().ToLower()}");
                }

                Pacekeeper.Pause();
            }
        }

        /// <summary>
        /// Closes the options panel for copy.
        /// </summary>
        private void CloseOptionsPanel()
        {
            _device.ControlPanel.Press(".hp-header-levelup-button");
        }
    }
}

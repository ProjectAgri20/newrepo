using System;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediOmni
{
    /// <summary>
    /// Assists in the management of Quicksets via the device control panel.
    /// </summary>
    public class JediOmniQuicksetManager
    {
        private readonly JediOmniControlPanel _controlPanel;

        /// <summary>
        /// Creates a new instance of JediOmniQuicksetManager.
        /// </summary>
        /// <param name="device"></param>
        public JediOmniQuicksetManager(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Creates a quickset with the specified name
        /// </summary>
        public void CreateQuickset(string quicksetName)
        {
            //press the save quickset button to create the quickset
            _controlPanel.ScrollPressWait(".hp-save-quickset-button", "#hpid-save-quickset-list-item");

            //enter the quickset name
            _controlPanel.PressWait("#hpid-save-quickset-title-textbox", "#hpid-keyboard", TimeSpan.FromSeconds(2));
            _controlPanel.TypeOnVirtualKeyboard(quicksetName);
            //_controlPanel.PressWait("#hpid-save-quickset-description-textbox", "#hpid-keyboard", TimeSpan.FromSeconds(2));
            //_controlPanel.TypeOnVirtualKeyboard("quickset has been created with the name " + quicksetName);

            if (_controlPanel.WaitForState("#hpid-keyboard-key-done", OmniElementState.Exists))
            {
                _controlPanel.WaitForAvailable("#hpid-keyboard-key-done");
                _controlPanel.Press("#hpid-keyboard-key-done");
            }

            //save the quickset created
            _controlPanel.WaitForState("#hpid-quicksets-save-button", OmniElementState.Useable, TimeSpan.FromSeconds(2));
            _controlPanel.Press("#hpid-quicksets-save-button");
        }
    }
}

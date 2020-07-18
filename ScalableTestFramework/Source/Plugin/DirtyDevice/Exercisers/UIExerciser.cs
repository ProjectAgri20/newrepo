using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class UIExerciser
    {
        private readonly DirtyDeviceManager _owner;
        private readonly JediDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private JediOmniPreparationManager _preparationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIExerciser" /> class.
        /// </summary>
        /// <param name="device">The <see cref="JediDevice" /> object.</param>
        internal UIExerciser(DirtyDeviceManager owner, JediOmniDevice device, JediOmniPreparationManager preparationManager)
        {
            _owner = owner;
            _device = device;
            _preparationManager = preparationManager;
            _controlPanel = device.ControlPanel;
        }

        internal void Exercise(DirtyDeviceActivityData activityData)
        {
            const string AppButtonFilter = "-homescreen-button";

            var controls = _controlPanel.GetIds("div", OmniIdCollectionType.Children).Where(id => id.EndsWith(AppButtonFilter)).ToArray();

            _owner.OnUpdateStatus(this, $"Found {controls.Length} app buttons...");

            for (int controlIndex = 0; controlIndex < controls.Length; controlIndex++)
            {
                string controlSelector = "#" + controls[controlIndex];

                string innerText = _controlPanel.GetValue(controlSelector, "innerText", OmniPropertyType.Property).Trim();

                _controlPanel.ScrollToItem(controlSelector);

                if (_controlPanel.WaitForState(controlSelector, OmniElementState.Useable, TimeSpan.FromSeconds(10)))
                {
                    _owner.OnUpdateStatus(this, $"  Pressing {innerText} ({controlIndex + 1:##}/{controls.Length:##})");
                    _controlPanel.Press(controlSelector);
                }
                else
                {
                    _owner.OnUpdateStatus(this, $"  Skipping {innerText}.  App is not available currently. ({controlIndex + 1:##}/{controls.Length:##})");
                    continue;
                }
                System.Threading.Thread.Sleep(5000);

                try
                {
                    _preparationManager.Reset();
                }
                catch (Exception x)
                {
                    _owner.OnUpdateStatus(this, x.ToString());
                    _owner.OnUpdateStatus(this, $"  Pressing 'Home' button did not exit {innerText}.  Consider filing a bug report against the app.");
                    _owner.OnUpdateStatus(this, $"  Attempting to return to home screen by waiting for timeout.");
                    UnauthenticateByTimeout();
                }
            }
        }

        /// <summary>
        /// 2017 April 19 MaxT: This code was copy/pasted from Authentication plugin per Kelly Youngman as the least offensive quick fix.  After the Authentication classes are moved to PluginSupport,
        /// this code can be removed and PluginSupport will be referenced.
        /// </summary>
        protected void UnauthenticateByTimeout()
        {
            var maxTime = DateTime.UtcNow.AddMinutes(10);

            if (Wait.ForTrue(() => _controlPanel.CheckState(".hp-homescreen-folder-view", OmniElementState.Useable), maxTime.Subtract(DateTime.UtcNow)))
            {
                // Verify
                if (DateTime.UtcNow >= maxTime
                     || !Wait.ForTrue(() => IsUserSignedOut(), maxTime.Subtract(DateTime.UtcNow))
                   )
                {
                    throw new DeviceInvalidOperationException("Did not sign out within inactivity timeout");
                }
            }
            else
            {
                throw new DeviceInvalidOperationException("Did not return to home screen within inactivity timeout");
            }
        }

        /// <summary>
        /// 2017 April 19 MaxT: This code was copy/pasted from Authentication plugin per Kelly Youngman as the least offensive quick fix.  After the Authentication classes are moved to PluginSupport,
        /// this code can be removed and PluginSupport will be referenced.
        /// </summary>
        private bool IsUserSignedOut()
        {
            string buttonText = _controlPanel.GetValue(JediOmniLaunchHelper.SignInOrSignoutButton, "innerText", OmniPropertyType.Property);
            return buttonText.StartsWith("Sign In");
        }
    }
}

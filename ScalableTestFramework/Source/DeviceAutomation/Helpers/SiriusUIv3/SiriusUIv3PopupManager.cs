using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Sirius;


namespace HP.ScalableTest.DeviceAutomation.Helpers.SiriusUIv3
{
    /// <summary>
    /// Maintains a list of known popup screen Ids (WidgetIds) so that the popup screen can be handled.
    /// </summary>
    public class SiriusUIv3PopupManager
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;
        private readonly List<PopupHandlerMapItem> _popupMap = new List<PopupHandlerMapItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3PopupManager" /> class.
        /// </summary>
        /// <param name="deviceControlPanel">The device.</param>
        public SiriusUIv3PopupManager(SiriusUIv3ControlPanel deviceControlPanel)
        {
            if (deviceControlPanel == null)
            {
                throw new ArgumentNullException(nameof(deviceControlPanel));
            }

            _controlPanel = deviceControlPanel;
            _popupMap.Add(new PopupHandlerMapItem("text", "Device Connection Problem", "fb_no"));
        }

        /// <summary>
        /// Checks device against list of known popup screens.  If one is found, it is handled and message text returned.
        /// If no known popup screens are detected, attempts to clear the screen by pressing an "OK" button if one exists.
        /// </summary>
        /// <returns>Message text if popup was handled.  Empty string if no known popup was handled.</returns>
        public string HandleAny()
        {
            return HandleAny(_controlPanel.GetScreenInfo());
        }

        /// <summary>
        /// Checks device against list of known popup screens.  If one is found, it is handled and message text returned.
        /// </summary>
        /// <param name="screenInfo"></param>
        /// <returns>Message text if popup was handled.  Empty string if no known popup was handled.</returns>
        public string HandleAny(ScreenInfo screenInfo)
        {
            string result = HandleKnownPopups(screenInfo);

            if (string.IsNullOrEmpty(result))
            {
                HandleDefault(screenInfo);
            }

            return result;
        }

        /// <summary>
        /// Checks device against list of known popup screens.  If one is found, it is handled and message text returned.
        /// </summary>
        /// <returns>Handled message text.  Empty string if no popup found.</returns>
        private string HandleKnownPopups(ScreenInfo screenInfo)
        {
            foreach (PopupHandlerMapItem item in _popupMap)
            {
                Widget popup = screenInfo.Widgets.FirstOrDefault(w => w.WidgetType == WidgetType.StaticText && w.Id == item.ScreenId);

                if (popup != null)
                {
                    // Close the popup by pressing the button to move past the screen.
                    _controlPanel.Press(item.ButtonId);
                    return item.HandledText;
                }
            }

            // Found no known popups
            return string.Empty;
        }

        /// <summary>
        /// Presses the OK button on a popup if it is available so that execution can continue.
        /// </summary>
        private void HandleDefault(ScreenInfo screenInfo)
        {
            // If a warning or error screen is displayed and only an OK button is available, press it
            var buttons = screenInfo.Widgets.Where(n => n.HasAction(WidgetAction.Select)).ToList();
            if (buttons.Count == 1 && buttons.First().HasValue("OK"))
            {
                _controlPanel.Press(buttons.First());
            }
        }
    }

    /// <summary>
    /// A class that maps known popup screens that hold up test executiion to the method of handling said screens.
    /// Also maps the return text once the popup screen has been handled.  This is for consistency in reporting the
    /// instance of the popup as and error.
    /// </summary>
    internal class PopupHandlerMapItem
    {
        public PopupHandlerMapItem(string screenId, string handledText, string buttonId)
        {
            ScreenId = screenId;
            HandledText = handledText;
            ButtonId = buttonId;
        }

        /// <summary>
        /// The WidgetId of the popup screen
        /// </summary>
        public string ScreenId { get; set; }

        /// <summary>
        /// The message text to be returned once the popup has been handled.
        /// </summary>
        public string HandledText { get; set; }

        /// <summary>
        /// The WidgetId of the button to press to close the popup.
        /// </summary>
        public string ButtonId { get; set; }
    }
}

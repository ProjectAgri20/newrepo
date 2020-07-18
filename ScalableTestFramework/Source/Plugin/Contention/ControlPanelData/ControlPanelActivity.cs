using System;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Custum attribute class for decorating classes related to Control Panel activities
    /// </summary>
    public class ControlPanelActivity : Attribute
    {
        /// <summary>
        /// Gets a value for the name of the Control Panel activity
        /// </summary>
        public string ActivityName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPanelActivity"/> attribute class
        /// </summary>
        /// <param name="controlPanelActivityName">Name of the Control Panel activity</param>
        public ControlPanelActivity(string controlPanelActivityName)
        {
            ActivityName = controlPanelActivityName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    [Flags]
    public enum DirtyDeviceActionFlags
    {
        None = 0,

        [Description("User Interface")]
        UserInterface = 1,

        [Description("Web Services")]
        WebServices = 2,

        [Description("Embedded Web Server (EWS)")]
        EWS = 4,

        [Description("SNMP")]
        SNMP = 8,

        [Description("Print")]
        Print = 16,

        [Description("Digital Send")]
        DigitalSend = 32,

        All = DigitalSend | (DigitalSend - 1),
    }

    public static class DirtyDeviceActions
    {
        private static DirtyDeviceActionFlags[] _allActions;

        public static IEnumerable<DirtyDeviceActionFlags> AllActions
        {
            get
            {
                if (_allActions == null)
                {
                    _allActions = EnumUtil.GetValues<DirtyDeviceActionFlags>()
                                  .Except(new[] { DirtyDeviceActionFlags.None, DirtyDeviceActionFlags.All })
                                  .ToArray();
                }
                return _allActions;
            }
        }

        public static IEnumerable<DirtyDeviceActionFlags> SelectedActions(DirtyDeviceActionFlags selection)
        {
            return AllActions.Where(n => selection.HasFlag(n));
        }
    }
}

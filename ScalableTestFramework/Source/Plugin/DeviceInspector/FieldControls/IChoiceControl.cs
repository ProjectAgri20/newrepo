using System;

namespace HP.ScalableTest.Plugin.DeviceInspector.FieldControls
{
    public interface IChoiceControl
    {
        //bool WillSet();

        event EventHandler FieldChecked;

        //Set get values as well?
    }
}

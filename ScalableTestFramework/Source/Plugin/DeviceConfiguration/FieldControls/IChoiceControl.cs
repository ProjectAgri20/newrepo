using System;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    public interface IChoiceControl
    {
        //bool WillSet();

        event EventHandler FieldChecked;

        //Set get values as well?
    }
}

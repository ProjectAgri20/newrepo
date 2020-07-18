using System;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;

namespace HP.ScalableTest.Plugin.DeviceInspector
{
    public interface IComponentData
    {
        bool GetFields(JediDevice device);

        DataPair<string> VerifyFields(JediDevice device);

        bool UpdateField<T>(Func<WebServiceTicket, bool> getProperty, JediDevice device, DataPair<T> data, string urn, string endpoint, string activityName);
    }
}

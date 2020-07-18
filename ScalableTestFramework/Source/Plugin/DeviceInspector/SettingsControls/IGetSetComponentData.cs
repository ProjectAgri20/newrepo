using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsControls
{
    interface IGetSetComponentData
    {
        IComponentData GetData();

        void SetControl(IEnumerable<IComponentData> list);

        void SetData();
    }
}

using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    interface IGetSetComponentData
    {
        IComponentData GetData();

        void SetControl(IEnumerable<IComponentData> list);

        void SetData();
    }
}

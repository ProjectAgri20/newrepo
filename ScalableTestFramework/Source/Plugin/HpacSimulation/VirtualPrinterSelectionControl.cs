using System.ComponentModel;
using System.Linq;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.HpacSimulation
{
    [ToolboxItem(false)]
    internal sealed class VirtualPrinterSelectionControl : AssetSelectionControl
    {
        protected override AssetInfoCollection ApplySelectionFilter(AssetInfoCollection selectableAssets)
        {
            return new AssetInfoCollection(selectableAssets.OfType<VirtualPrinterInfo>().ToList<AssetInfo>());
        }
    }
}

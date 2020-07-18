using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    [ObjectFactory(ElementType.Assets)]
    [ObjectFactory(ElementType.Workers)]
    public partial class BlankElementInfoControl : ElementInfoControlBase
    {
        public BlankElementInfoControl()
        {
            InitializeComponent();
        }
    }
}

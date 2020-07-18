using System.Windows.Forms;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.UI.SessionExecution
{
    public partial class ElementInfoControlBase : UserControl, IElementInfoControl
    {
        public ElementInfoControlBase()
        {
            InitializeComponent();
        }
        public virtual string GetTitle()
        {
            return string.Empty;
        }

        public virtual void Initialize(SessionMapElement element, SessionInfo sessionInfo)
        {
            // Do nothing by default
        }

        public virtual void RefreshData()
        {
            // Do nothing by default
        }
    }
}

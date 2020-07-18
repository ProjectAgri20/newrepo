using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.PluginSupport.Print
{
    public partial class RemoteQueueSelectionForm : Form
    {
        public PrintQueueInfoCollection SelectedQueues
        {
            get { return remotePrintQueueSelectionControl.SelectedPrintQueues; }
        }

        private RemoteQueueSelectionForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
        }

        public RemoteQueueSelectionForm(IEnumerable<RemotePrintQueueInfo> queues)
            : this()
        {
            remotePrintQueueSelectionControl.Initialize(queues);
        }
    }
}

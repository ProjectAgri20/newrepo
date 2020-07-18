using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    [ObjectFactory(ResourceUsageType.PrintQueue)]
    public partial class PrintQueueUsageResolverForm : UsageResolverForm
    {
        private KeyValuePair<Guid, string> _selectedQueue = new KeyValuePair<Guid, string>(Guid.Empty, string.Empty);

        private PrintQueueUsageResolverForm()
        { }

        public PrintQueueUsageResolverForm(UsageResolverData data)
            : base(data)
        {
            InitializeComponent();

            if (Data.Agent.ResolutionCompleted)
            {
                var id = new Guid(Data.Agent.ImportData.ResourceId);
                printQueueSelectionControl.Initialize(id, useCheckBox: false);
                _selectedQueue = new KeyValuePair<Guid, string>(id, Data.Replacement);
            }
            else
            {
                var id = Guid.Empty;
                printQueueSelectionControl.Initialize(id, useCheckBox: false);
            }

            printQueueSelectionControl.OnSelectedNodeChanged += printQueueSelectionControl_OnSelectedNodeChanged;
        }

        private void printQueueSelectionControl_OnSelectedNodeChanged(object sender, SelectedNodeEventArgs e)
        {
            if (e.ImageKey == "PrintQueue")
            {
                _selectedQueue = new KeyValuePair<Guid, string>((Guid)e.Tag, e.Name);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (_selectedQueue.Key == Guid.Empty)
            {
                MessageBox.Show("You must select a queue", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Data.Replacement = _selectedQueue.Value;
            Data.Agent.ImportData.ResourceId = _selectedQueue.Key.ToString();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PrintQueueUsageResolverForm_Load(object sender, EventArgs e)
        {
            oldNameValueLabel.Text = Data.Original;
        }
    }
}

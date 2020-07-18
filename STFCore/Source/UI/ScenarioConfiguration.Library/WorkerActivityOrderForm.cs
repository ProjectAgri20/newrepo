using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    internal partial class WorkerActivityOrderForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerActivityOrderForm"/> class.
        /// </summary>
        public WorkerActivityOrderForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
        }

        ///// <summary>
        ///// Initializes this instance with the specified activities.
        ///// </summary>
        ///// <param name="activities">The activities.</param>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        //public void Initialize(IEnumerable<VirtualResourceMetadata> activities)
        //{
        //    foreach (VirtualResourceMetadata activity in activities.OrderBy(n => n.ExecutionOrder))
        //    {
        //        RadListDataItem item = null;
        //        try
        //        {
        //            item = new RadListDataItem()
        //            {
        //                Value = activity,
        //                Text = "{0} [{1}]".FormatWith(activity.Name, activity.MetadataType),
        //                Image = STFIcons.Instance.ResourceMetadataIcons.Images[activity.MetadataType]
        //            };
        //            activities_ListControl.Items.Add(item);
        //        }
        //        catch
        //        {
        //            if (item != null)
        //            {
        //                item.Dispose();
        //            }
        //            throw;
        //        }
        //    }
        //}

        public void Initialize(List<WorkerActivityConfiguration> configurationByPhase)
        {
            foreach (var configuration in configurationByPhase.OrderBy(n => n.ExecutionPlan.Order))
            {
                RadListDataItem item = null;
                try
                {
                    item = new RadListDataItem()
                    {
                        Value = configuration,
                        Text = "{0} [{1}]".FormatWith(configuration.Metadata.Name, configuration.Metadata.MetadataType),
                        Image = IconManager.Instance.PluginIcons.Images[configuration.Metadata.MetadataType]
                    };
                    activities_ListControl.Items.Add(item);
                }
                catch
                {
                    if (item != null)
                    {
                        item.Dispose();
                    }
                    throw;
                }
            }
        }

        private void moveUp_Button_Click(object sender, EventArgs e)
        {
            MoveItem(activities_ListControl.SelectedIndex, -1);
        }

        private void moveDown_Button_Click(object sender, EventArgs e)
        {
            MoveItem(activities_ListControl.SelectedIndex, 1);
        }

        private void MoveItem(int index, int direction)
        {
            int newIndex = index + direction;

            // Make sure we have a selection and that we're not moving out of bounds
            if (index != -1 && newIndex >= 0 && newIndex < activities_ListControl.Items.Count)
            {
                RadListDataItem item = activities_ListControl.Items[index];
                activities_ListControl.Items.RemoveAt(index);
                activities_ListControl.Items.Insert(newIndex, item);

                activities_ListControl.SelectedIndex = newIndex;
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < activities_ListControl.Items.Count(); i++)
            {
                var configuration = activities_ListControl.Items[i].Value as WorkerActivityConfiguration;
                configuration.Order = i + 1;
            }
            //foreach (RadListDataItem item in activities_ListControl.Items)
            //{
            //    (item.Value as VirtualResourceMetadata).ExecutionOrder = item.RowIndex + 1;
            //}
            this.DialogResult = DialogResult.OK;
        }
    }
}

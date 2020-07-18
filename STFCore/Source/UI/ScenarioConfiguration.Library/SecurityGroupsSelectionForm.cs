using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    public partial class SecurityGroupsSelectionForm : Form
    {
        //public List<ActiveDirectoryGroup> SelectedGroups
        //{
        //    get
        //    {
        //        return (List<ActiveDirectoryGroup>)group_sideBySideListBox.DestinationItems;
        //    }
        //    set
        //    {
        //        // Make a copy of the list.  This is because we do not want to modify the original.
        //        group_sideBySideListBox.DestinationItems = value.ToList();
        //    }
        //}

        public SecurityGroupsSelectionForm()
        {
            InitializeComponent();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void group_sideBySideListBox_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    string serviceHost = GlobalSettings.WcfHosts[WcfService.DataGateway];
            //    using (DataGatewayClient client = DataGatewayClient.Create(serviceHost))
            //    {
            //        //group_sideBySideListBox.SourceItems = client.Channel.GetAvailableActiveDirectoryGroups();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Unable to query dispatcher for available security groups.\n\n" + ex.Message, 
            //        "Dispatcher service not running", MessageBoxButtons.OK);
            //    cancel_Button_Click(this, EventArgs.Empty);
            //}

            //if (group_sideBySideListBox.DestinationItems == null)
            //{
            //    group_sideBySideListBox.DestinationItems = new List<ActiveDirectoryGroup>();
            //}
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Help create the Dialog Box to pick the devices to bring back online 
    /// when the user hit the repeat button. 
    /// </summary>
    public partial class OfflineDevicesMessageBox : Form
    {
        /// <summary>
        /// Constructor Initializing the form.
        /// </summary>
        public OfflineDevicesMessageBox()
        {
            InitializeComponent();
        }

        //Remove the entire system menu from the form
        private const int WS_SYSMENU = 0x80000;
        
        /// <summary>
        /// Override CreateParams and remove system menu form the form.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }
        /// <summary>
        /// Shows a Dialog form to help the user choose the devices to bring back online. 
        /// </summary>
        /// <param name="offlineDevice"></param>
        /// <returns></returns>
        public DialogResult ShowDialog(HashSet<string> offlineDevice)
        {
            if (offlineDevice.Count != 0)
            {
                foreach (string assetId in offlineDevice)
                {
                    offlineDevicesChecklistBoxes.Items.Add(assetId, CheckState.Unchecked);
                }
            }
            
            return this.ShowDialog();
        }

        /// <summary>
        /// Get the list of devices that the user want to bring back online when the session gets repeated. 
        /// </summary>
        /// <returns>List of the devices that needs to be brought back online.</returns>
        public HashSet<string> GetSelectedOnlineDevicesChecklistBoxes()
        {
            //List of the devices that the user wants to bring back online. 
            HashSet<string> listOnlineAssetId = new HashSet<string>();

            foreach(var checkedItem in offlineDevicesChecklistBoxes.CheckedItems)
            {
                listOnlineAssetId.Add(checkedItem.ToString());
            }

            return listOnlineAssetId;
        }

        /// <summary>
        /// Handle what happen when the Cancel button is being clicked. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handle what happen when the Ok button is being clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.Framework
{
    /// <summary>
    /// Dialog for collecting database and dispatcher configuration data.
    /// </summary>
    public partial class MainFormConnectDialog : Form
    {
        private List<FrameworkServer> _dispatchers = null;
        private ErrorProvider _errorProvider = new ErrorProvider();

        public MainFormConnectDialog()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(dispatchers_GridView, GridViewStyle.ReadOnly);
            dispatchers_GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            InitializeErrorProvider();            
        }

        /// <summary>
        /// The selected Dispatcher
        /// </summary>
        public FrameworkServer SelectedDispatcher 
        { 
            get 
            {
                GridViewRowInfo row = dispatchers_GridView.SelectedRows.FirstOrDefault();
                if (row != null)
                {
                    return (FrameworkServer)row.DataBoundItem;
                }

                // Nothing selected
                return null;
            } 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadDispatchers();
            SetDefaultDispatcher();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                // Save selected registry entries
                UserAppDataRegistry.SetValue("DispatcherName", SelectedDispatcher.HostName);

                base.OnFormClosing(e);
            }
        }

        private void InitializeErrorProvider()
        {
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.ContainerControl = this;
            _errorProvider.SetIconAlignment(connect_Button, ErrorIconAlignment.MiddleLeft);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Lots of things could go wrong when connecting to the database, and we want to catch all of them.")]
        private void LoadDispatchers()
        {
            try
            {
                // Load the list of dispatchers
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    _dispatchers = GetDispatchers(context).ToList();
                    dispatchers_GridView.DataSource = _dispatchers;
                }
            }
            catch
            {
                MessageBox.Show("Could not connect to database '{0}'.".FormatWith(GlobalSettings.Database), "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static IQueryable<FrameworkServer> GetDispatchers(AssetInventoryContext entities)
        {
            string serverType = ServerType.Dispatcher.ToString();
            string systemEnvironment = GlobalSettings.Environment.ToString();

            return
                from x in entities.FrameworkServers
                where x.ServerTypes.Any(e => e.Name.Equals(serverType, StringComparison.OrdinalIgnoreCase))
                    && x.Environment == systemEnvironment
                orderby x.HostName ascending
                select x;
        }

        private bool SetDefaultDispatcher()
        {
            // load last used dispatcher from the registry
            string lastDispatcherUsed = UserAppDataRegistry.GetValue("DispatcherName") as string;

            FrameworkServer dispatcher = null;
            if (!string.IsNullOrEmpty(lastDispatcherUsed))
            {
                // First see if the last dispatcher used is in the list of available dispatchers
                // if it is, then go ahead and use it.
                dispatcher = _dispatchers.Where(x => x.HostName.Equals(lastDispatcherUsed, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (dispatcher != null)
                {
                    SelectDispatcher(dispatcher.HostName);
                    return true;
                }                
            }

            // If the last dispatcher used was not found in the list of available dispatchers, then see 
            // if this machine's hostname is found in the list.  If so, then use it and return.
            dispatcher = _dispatchers.Where(x => x.HostName.StartsWith(Environment.MachineName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (dispatcher != null)
            {
                SelectDispatcher(dispatcher.HostName);
                return true;
            }                

            return false;
        }

        private void SelectDispatcher(string dispatcherName)
        {
            GridViewRowInfo row = dispatchers_GridView.Rows.Where(r => (string)r.Cells[0].Value == dispatcherName).FirstOrDefault();
            if (row != null)
            {
                dispatchers_GridView.TableElement.ScrollToRow(row);
                row.IsCurrent = true;
            }
        }

        /// <summary>
        /// At this point, the user has had to fill in any missing config info.  Make sure all controls have a selected value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connect_Button_Click(object sender, EventArgs e)
        {
            if (DispatchersGridView_HasSelection())
            {
                // Close the form
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Validation for Grid.  Checks for selected item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool DispatchersGridView_HasSelection()
        {
            bool itemSelected = (dispatchers_GridView.SelectedRows.Count > 0);
            _errorProvider.SetError(connect_Button, itemSelected ? string.Empty : "A dispatcher must be selected.");

            return itemSelected;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Printing;
using System.Printing.IndexedProperties;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// UI for bulk updating Print Queue Port Address and Port Name.
    /// </summary>
    public partial class PortUpdateForm : Form
    {
        Dictionary<QueuePortData, StandardTcpIPPort> _installedPorts = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortUpdateForm"/> class.
        /// </summary>
        public PortUpdateForm()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;
            RefreshGrid();
        }

        private List<QueuePortData> Selected
        {
            get
            {
                List<QueuePortData> selected = new List<QueuePortData>();
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[0].Value != null && (bool)row.Cells[0].Value == true)
                    {
                        selected.Add((QueuePortData)row.DataBoundItem);
                    }
                }
                return selected;
            }
        }

        private void update_Button_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;                
                List<QueuePortData> selected = Selected;

                TraceFactory.Logger.Debug("Begin Updating Port Addresses...");
                if (ValidateInput(selected))
                {
                    StandardTcpIPPort port = null;
                    StandardTcpIPPort newPort = null;
                    foreach (QueuePortData selectedItem in selected)
                    {
                        port = _installedPorts[selectedItem];

                        /* What we need to do here is change the existing port's name and address.
                         * Since the OS identifies the port by it's name, we have to create a new port,
                         * copy over all of it's properties, point the print queue to the new port,
                         * then delete the old one.
                         * One caveat to note:  The port version needs to ALWAYS be 1 when creating a new port.
                         * Existing ports will show different values for Version.  For example:
                         * Server 2003/XP Version = 1.
                         * Server 2008/Win7 Version = 2.
                         * So, if this code is executing on a Win7 box, it's existing port will show a Version of 2.
                         * However, when creating the new port, the Version needs to be set to 1.  After it is created,
                         * it will display 2. I couldn't find any info to explain why this is.
                         * When using CreateRawPortData, the Version defaults to 1.
                         * All other properties are copied to the newly created port. -kyoungman
                        */
                        newPort = new StandardTcpIPPort(selectedItem.NewPortAddress, selectedItem.PortNumber);
                        newPort.Protocol = port.Protocol;
                        newPort.Queue = selectedItem.QueueName;
                        newPort.SnmpCommunity = port.SnmpCommunity;
                        newPort.SnmpEnabled = port.SnmpEnabled;
                        newPort.SnmpDevIndex = port.SnmpDevIndex;

                        newPort.CreatePort();
                        ChangeQueuePort(port.PortName, newPort.PortName); //Point the printer to the new port
                        port.DeletePort();
                    }

                    RefreshGrid();
                }
            }
            catch (InvalidOperationException opEx)
            {
                TraceFactory.Logger.Error(opEx);
                DisplayError();
            }
            catch (ManagementException manEx)
            {
                TraceFactory.Logger.Error(manEx.JoinAllErrorMessages(), manEx);
                DisplayError();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static void ChangeQueuePort(string portName, string newName)
        {
            ConnectionOptions options = new ConnectionOptions();
            options.EnablePrivileges = true;

            ManagementScope mgmtScope = new ManagementScope("root\\CIMV2", options);
            mgmtScope.Connect();

            ObjectQuery objQuery = new ObjectQuery("SELECT * FROM Win32_Printer WHERE PortName = '{0}'".FormatWith(portName));

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(mgmtScope, objQuery))
            {
                ManagementObjectCollection match = searcher.Get();
                if (match.Count != 1)
                {
                    throw new InvalidOperationException("Unexpected search result.  Returned {0} matches for PortName:{1}".FormatWith(match.Count, portName));
                }

                var enumerator = match.GetEnumerator();
                enumerator.MoveNext();
                ManagementObject printerObject = (ManagementObject)enumerator.Current;

                TraceFactory.Logger.Debug("Old Port: {0}, New Port: {1}".FormatWith(printerObject["PortName"], newName));
                printerObject["PortName"] = newName;

                PutOptions po = new PutOptions();
                po.Type = PutType.UpdateOrCreate;
                printerObject.Put(po);
            }
        }

        private static void DisplayError()
        {
            MessageBox.Show("Error Updating Port Addresses.\nCheck the Log file for additional information.", "Update Port Addresses", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RefreshGrid()
        {
            dataGridView.DataSource = null;

            //Loads Queues and ports
            dataGridView.DataSource = GetQueueData();
        }

        /// <summary>
        /// Retrieves the list of installed ports on this machine and pairs the port data with it's corresponding queue information.
        /// </summary>
        /// <returns></returns>
        private List<QueuePortData> GetQueueData()
        {
            Collection<StandardTcpIPPort> ports = StandardTcpIPPort.InstalledPorts;
            List<QueuePortData> result = new List<QueuePortData>();
            _installedPorts = new Dictionary<QueuePortData, StandardTcpIPPort>();

            StandardTcpIPPort port = null;
            foreach (PrintQueue queue in PrintQueueController.GetPrintQueues())
            {
                QueuePortData data = new QueuePortData(queue.Name, queue.QueuePort.Name);
                port = ports.Where(p => p.PortName == data.PortName).FirstOrDefault();
                if (port != null) //Only return the queues that have a port.
                {
                    data.PortAddress = port.Address;
                    data.PortNumber = unchecked((int)port.PortNumber);
                    result.Add(data);
                    _installedPorts.Add(data, port);
                }
            }

            return result;
        }

        private bool ValidateInput(List<QueuePortData> selected)
        {
            if (selected.Count == 0)
            {
                MessageBox.Show("Please select one or more Print Queues to update.", "Update Port Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            bool inputValid = true;
            foreach (QueuePortData rowData in selected)
            {
                if (string.IsNullOrEmpty(rowData.NewPortAddress))
                {
                    inputValid = false;
                    break;
                }
            }

            if (! inputValid)
            {
                MessageBox.Show("Please enter a new port address for all selected ports.", "Update Port Address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

    }
}

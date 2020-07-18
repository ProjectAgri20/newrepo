using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI;
using HP.ScalableTest.UI.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.Virtualization;
using VirtualMachine = HP.ScalableTest.Data.EnterpriseTest.VirtualMachine;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.Utility
{
    public partial class VirtualMachineManagamentForm : Form
    {
        private SortableBindingList<VirtualMachine> _machines = new SortableBindingList<VirtualMachine>();
        private EnterpriseTestEntities _entities = null;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private ObservableCollection<Executables> executableListCollection = new ObservableCollection<Executables>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualMachineManagamentForm"/> class.
        /// </summary>
        public VirtualMachineManagamentForm()
        {
            InitializeComponent();
            if (!STFLoginManager.Login())
            {
                Environment.Exit(1);
            }
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            _entities = new EnterpriseTestContext();

            //bind the virtual machine list to the combobox, the display will always the name
            // and the value will be the platformId
            platformFilter_ToolStripComboBox.ComboBox.DisplayMember = "Name";
            platformFilter_ToolStripComboBox.ComboBox.ValueMember = "PlatformId";
            executables_dataGridView.AutoGenerateColumns = false;
        }

        #region Form Load and Unload

        /// <summary>
        /// Handles the Load event of the VirtualMachinePlatformConfigForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void VirtualMachineGroupConfigForm_Load(object sender, EventArgs e)
        {
            machines_DataGridView.AutoGenerateColumns = false;
            
            
            ////var mStream = new MemoryStream(Convert.FromBase64String(byteArry));

            //byte[] bArray = ConvertHexStringToByteArray(byteArry);
            //MemoryStream ms = new MemoryStream(bArray);
            ////using (FileStream file = new FileStream(@"D:\Temp\cp_screenshot.txt", FileMode.Open, FileAccess.Read))
            ////{
            ////    BinaryReader bReader = new BinaryReader(file);
            ////    bReader.
            ////    file.CopyTo(ms);
            ////}

            //pictureBox1.Image = Image.FromStream(ms);
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //platformFilter_ToolStripComboBox.Items.Add("All Platforms");
            foreach (var item in _entities.VirtualMachinePlatforms.Where(x => x.Active))
            {
                platformFilter_ToolStripComboBox.Items.Add(item);
            }

            platformFilter_ToolStripComboBox.SelectedIndex = 0;

            // Spin off a thread to load each piece of data, then have then sync up through a
            // semaphore and come back together in the loadPlatformWorker_RunWorkerCompleted() call
            // where a bit more initialization will occur
            BackgroundWorker loadMachinesWorker = new BackgroundWorker();
            loadMachinesWorker.DoWork += new DoWorkEventHandler(LoadMachines_DoWork);
            loadMachinesWorker.RunWorkerAsync();
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }

        #endregion Form Load and Unload

        #region Load Platform and Virtual Machine Grids

        /// <summary>
        /// Handles the DoWork event of the loadVirtualMachinesWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void LoadMachines_DoWork(object sender, DoWorkEventArgs e)
        {
            machines_DataGridView.Invoke(new MethodInvoker(PopulateMachinesGrid));
            _resetEvent.Set();
        }

        /// <summary>
        /// Populates the virtual machine grid.
        /// </summary>
        private void PopulateMachinesGrid()
        {
            _machines.Clear();
            foreach (var machine in Data.EnterpriseTest.VirtualMachine.SelectAll().OrderBy(f => f.Name))
            {
                _machines.Add(machine);
            }

            machines_DataGridView.DataSource = null;
            machines_DataGridView.DataSource = _machines;
        }

        private void PopulateMachinesGrid(string platformName)
        {
            VirtualMachinePlatform platform =
                _entities.VirtualMachinePlatforms.FirstOrDefault(x => x.PlatformId.Equals(platformName));
            if (platform != null)
            {
                IEnumerable<VirtualMachine> machines = VirtualMachine.Select(platform: platformName,
                    includePlatforms: true);
                var query =
                    (
                        from m in machines
                        from p in m.VirtualMachinePlatforms
                        where platform.PlatformId.Equals(p.PlatformId)
                        select m
                        ).Distinct();

                _machines.Clear();
                foreach (var item in query)
                {
                    _machines.Add(item);
                }

                machines_DataGridView.DataSource = null;
                machines_DataGridView.DataSource = _machines;
            }
        }

        #endregion Load Platform and Virtual Machine Grids

        #region Virtual Machine DataGrid Handlers

        private void machines_DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (machines_DataGridView.IsCurrentCellDirty)
            {
                machines_DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        #endregion Virtual Machine DataGrid Handlers

        #region Button and Menu Handlers

        /// <summary>
        /// Handles the Click event of the Create snapshot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void create_Button_Click(object sender, EventArgs e)
        {
            operations_groupBox.Enabled = false;
            // Close();
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells[0].Value);
                if (!selected)
                    continue;

                VirtualMachine vm = row.DataBoundItem as VirtualMachine;

                using (var virtualCenter = GetVSphereController())
                {
                    var machine = virtualCenter.GetVirtualMachines().First(x => x.HostName == vm.Name);
                    if (machine.PowerState != VirtualMachinePowerState.PoweredOn)
                    {
                        // The VM is not powered on, so go ahead and power it on.
                        //if(machine.CreateSnapshot(true))
                        virtualCenter.CreateVirtualMachineSnapshot(machine);
                        {
                            TraceFactory.Logger.Info("Machine {0}: Snapshot created".FormatWith(vm.Name));
                        }
                    }
                }
            }

            operations_groupBox.Enabled = true;
        }

        /// <summary>
        /// Handles the Click event of the ok_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void remove_Button_Click(object sender, EventArgs e)
        {
            operations_groupBox.Enabled = false;
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells[0].Value);
                if (!selected)
                    continue;

                VirtualMachine vm = row.DataBoundItem as VirtualMachine;

                using (var virtualCenter = GetVSphereController())
                {
                    var machine = virtualCenter.GetVirtualMachines().First(x => x.HostName == vm.Name);
                    if (machine.PowerState == VirtualMachinePowerState.PoweredOn) continue;

                    // The VM is not powered on, so go ahead and power it on.
                    //if (!machine.RevertToSnapshot(true)) continue;
                    virtualCenter.RevertToSnapshot(machine);


                    //if (!machine.DeleteAllSnapshot(true)) continue;
                    virtualCenter.DeleteAllVirtualMachineSnapshot(machine);
                    TraceFactory.Logger.Info("Machine {0}: Snapshot deleted".FormatWith(vm.Name));
                    virtualCenter.PowerOn(machine);
                }
            }

            operations_groupBox.Enabled = true;
        }

        private void shutdown_button_Click(object sender, EventArgs e)
        {
            operations_groupBox.Enabled = false;
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells[0].Value);
                if (!selected)
                    continue;

                VirtualMachine vm = row.DataBoundItem as VirtualMachine;
                using (var virtualCenter = GetVSphereController())
                {
                    var machine = virtualCenter.GetVirtualMachines().First(x => x.HostName == vm.Name);
                    if (machine.PowerState == VirtualMachinePowerState.PoweredOn)
                    {
                        virtualCenter.Shutdown(machine);
                    }
                }
            }
            operations_groupBox.Enabled = true;
        }

        private void poweron_button_Click(object sender, EventArgs e)
        {
            operations_groupBox.Enabled = false;
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells[0].Value);
                if (!selected)
                    continue;

                VirtualMachine vm = row.DataBoundItem as VirtualMachine;

                using (var virtualCenter = GetVSphereController())
                {
                    var machine = virtualCenter.GetVirtualMachines().First(x => x.HostName == vm.Name);
                    if (machine.PowerState != VirtualMachinePowerState.PoweredOn)
                    {
                        virtualCenter.PowerOn(machine);
                    }
                }
            }
            operations_groupBox.Enabled = true;
        }

        private void validate_button_Click(object sender, EventArgs e)
        {
            var validateExecutable = new ObservableCollection<Executables>
                {
                    new Executables {Argument = "/c timeout 3", ExecutableFileName = "cmd.exe"}
                };
            operations_groupBox.Enabled = false;
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells[0].Value);
                if (!selected)
                    continue;

                VirtualMachine vm = row.DataBoundItem as VirtualMachine;
                UpdateRowIndex(row.Index, ExecuteOnVirtualMachine(vm, validateExecutable));
            }
           
           

            operations_groupBox.Enabled = true;
        }

        /// <summary>
        /// Handles the Click event of the selectAllToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Click event of the uncheckAllToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion Button and Menu Handlers

        private void platformFilter_ToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get the current chosen platform
            string name = platformFilter_ToolStripComboBox.ComboBox.Text;

            if (name.Equals("All Platforms"))
            {
                PopulateMachinesGrid();
            }
            else
            {
                //send the platformId to populate the grid
                name = ((VirtualMachinePlatform)platformFilter_ToolStripComboBox.ComboBox.SelectedItem).PlatformId;
                PopulateMachinesGrid(name);
            }
        }

        private void browse_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.*";
                dialog.Filter = "All files (*.*) | *.*";
                dialog.Multiselect = false;
                dialog.Title = "Add executable File";

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    file_textBox.Text = dialog.FileName;
                }
            }
        }

        private void execute_button_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells[0].Value);
                if (!selected)
                    continue;


                VirtualMachine vm = row.DataBoundItem as VirtualMachine;
                UpdateRowIndex(row.Index, ExecuteOnVirtualMachine(vm, executableListCollection));
            }
            //int parallelOperationCount = (int)(batchsize_numericUpDown.Value / machines_DataGridView.SelectedRows.Count);
            //Parallel.ForEach(Partitioner.Create(0, machines_DataGridView.SelectedRows.Count, parallelOperationCount), range =>
            //   {
            //       for (int i = range.Item1; i < range.Item2; i++)
            //       {
            //           VirtualMachine vm = machines_DataGridView.SelectedRows[i].DataBoundItem as VirtualMachine;
            //           UpdateRowIndex(i, ExecuteOnVirtualMachine(vm, executableListCollection));
            //       }
            //   });
        }

        private bool ExecuteOnVirtualMachine(VirtualMachine vm, ObservableCollection<Executables> executables)
        {
            using (var virtualCenter = GetVSphereController())
            {
                var machine = virtualCenter.GetVirtualMachines().First(x => x.HostName == vm.Name);
                if (machine.PowerState != VirtualMachinePowerState.PoweredOn)
                {
                    // The VM is not powered on, so go ahead and power it on.
                    //if (machine.PowerOn(true))
                    virtualCenter.PowerOn(machine);
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(90));
                    }
                }
                try
                {
                    for (int i = 0; i < executables.Count; i++)
                    {
                        string executable = executables.ElementAt(i).ExecutableFileName;
                        string argument =
                            executables.ElementAt(i).Argument.Replace("{MachineName}",
                                vm.Name);
                       // string decryptedPassword = BasicEncryption.Decrypt(UserManager.CurrentUserCredential.Password, UserManager.EncryptionKey);
                        var pid = virtualCenter.RunGuestProcess(machine, executable, argument, UserManager.CurrentUser.ToNetworkCredential(), true);

                        if (pid <= 0)
                        {
                            UpdateStatus("{0} failed to execute on {1}".FormatWith(executable, vm.Name));
                            return false;
                        }

                        UpdateStatus("Executed: {0} on {1}".FormatWith(executable, vm.Name));
                    }
                }
                catch (Exception exception)
                {
                    TraceFactory.Logger.Error("{0}:{1} with {2}".FormatWith(machine.HostName, "Failed",
                        exception.Message));
                    vm.PowerState = "Powered Off";
                    if (
                        exception.Message ==
                        "The guest authentication being used does not have sufficient permissions to perform the operation.")
                    {
                        vm.UsageState = VMError.GuestPrivilege.ToString();
                    }
                    else if (exception.Message == "The guest operations agent could not be contacted")
                    {
                        vm.UsageState = VMError.ServiceConnection.ToString();
                    }
                    else if (exception.Message.Contains("The specified guest user does not match the user currently logged in interactively"))
                    {
                        vm.UsageState = VMError.GuestLogin.ToString();
                    }
                    UpdateStatus("{0} failed with exception: {1}".FormatWith(vm.Name, exception.Message));
                    virtualCenter.Shutdown(machine);
                }
            }
            return true;
        }

        private void Queue_button_Click(object sender, EventArgs e)
        {
            executables_dataGridView.DataSource = null;
            executableListCollection.Add(new Executables()
            {
                Argument = arguments_textBox.Text,
                ExecutableFileName = file_textBox.Text
            });
            executables_dataGridView.DataSource = executableListCollection;
        }

        private void UpdateStatus(string text)
        {
            // If an invoke is required, this will call right back to itself through the
            // Invoke which will put it in the UI thread and then the else will come into
            // play and the control will be updated.
            if (log_textBox.InvokeRequired)
            {
                log_textBox.Invoke(new MethodInvoker(() => UpdateStatus(text)));
            }
            else
            {
                log_textBox.Text += text + Environment.NewLine;
            }
        }

        private void UpdateRowIndex(int currentRow, bool status)
        {
            if (machines_DataGridView.InvokeRequired)
            {
                machines_DataGridView.Invoke(new MethodInvoker(() => UpdateRowIndex(currentRow, status)));
            }
            else
            {
                if (status)
                {
                    machines_DataGridView.Rows[currentRow].Cells[1].Style.ForeColor = Color.Green;
                }
                else
                {
                    machines_DataGridView.Rows[currentRow].Cells[1].Style.ForeColor = Color.Red;
                    machines_DataGridView.Rows[currentRow].Selected = false;
                }
            }
        }

        private static VSphereVMController GetVSphereController()
        {
            string serverUri = GlobalSettings.Items[Setting.VMWareServerUri];
            return new VSphereVMController
            (
                new Uri(serverUri), UserManager.CurrentUser.ToNetworkCredential()
            );
        }

        public enum
            VMError
        {
            [Description("None")]
            None,

            [Description("Guest does not have privilege")]
            GuestPrivilege,

            [Description("Unable to connect to VM tools")]
            ServiceConnection,

            [Description("Guest user does not match")]
            GuestLogin
        }

        public class Executables
        {
            public string ExecutableFileName { get; set; }

            public string Argument { get; set; }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            executables_dataGridView.DataSource = null;
            executableListCollection.Clear();
            executables_dataGridView.DataSource = null;
        }

        private void button_reboot_Click(object sender, EventArgs e)
        {
            operations_groupBox.Enabled = false;
            foreach (DataGridViewRow row in machines_DataGridView.Rows)
            {
                var selected = Convert.ToBoolean(row.Cells[0].Value);
                if (!selected)
                    continue;

                VirtualMachine vm = row.DataBoundItem as VirtualMachine;

                using (var virtualCenter = GetVSphereController())
                {
                    var machine = virtualCenter.GetVirtualMachines().First(x => x.HostName == vm.Name);
                    if (machine.PowerState == VirtualMachinePowerState.PoweredOn)
                    {
                        virtualCenter.Reboot(machine);
                    }
                }
            }
            operations_groupBox.Enabled = true;
        }

        private void buildDeploy_button_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //using (BuildManagement build = new BuildManagement())
            //{
            //    build.ShowDialog();
            //}
            //this.Show();
        }
    }
}
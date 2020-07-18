using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.Xml;
using Telerik.WinControls.UI;

namespace HP.ScalableTest
{
    /// <summary>
    /// List form showing STB Print Device entries
    /// </summary>
    public partial class PrinterListForm : Form
    {
        private SortableBindingList<Printer> _printers = null;
        private BindingSource _bindingSource;
        private string _directory = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterListForm"/> class.
        /// </summary>
        public PrinterListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(radGridViewPrinters, GridViewStyle.ReadOnly);
            ShowIcon = true;

            radGridViewPrinters.AutoGenerateColumns = false;

            _printers = new SortableBindingList<Printer>();
        }

        private void AssetListForm_Load(object sender, System.EventArgs e)
        {
            RefreshItems();
            radGridViewPrinters.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridViewPrinters.BestFitColumns();
        }

        private void RefreshItems()
        {
            int index = (radGridViewPrinters.CurrentRow != null ? radGridViewPrinters.CurrentRow.Index : 0);
            using (new BusyCursor())
            {
                var printers = GetPrinters();

                _printers.Clear();
                foreach (var item in printers)
                {
                    _printers.Add(item);
                }

                _bindingSource = new BindingSource();
                _bindingSource.DataSource = _printers;
                radGridViewPrinters.DataSource = _bindingSource;

                GridViewRowInfo foundRow = null;
                while (radGridViewPrinters.RowCount > 0)
                {
                    foundRow = radGridViewPrinters.Rows.FirstOrDefault(x => x.Index == index);
                    if (foundRow != null)
                    {
                        radGridViewPrinters.CurrentRow = foundRow;
                        break;
                    }
                    index--;
                }
                
            }
        }

        private static Collection<Printer> GetPrinters()
        {
            Collection<Printer> result = new Collection<Printer>();

            // retain only the pools that match AssetInventoryPools SystemSetting
            string[] assetPools = GetAssetPoolsFromSettings();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                if (assetPools.Count() == 0 || assetPools.Any(x => x.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)))
                {
                    assetPools = context.AssetPools.Select(x => x.Name).ToArray();
                }

                if (assetPools.Any())
                {
                    foreach (Printer printer in context.Assets.Include(n => n.Reservations).OfType<Printer>().Where(n => assetPools.Contains(n.Pool.Name)))
                    {
                        result.Add(printer);
                    }
                }
            }
            return result;
        }

        private static string[] GetAssetPoolsFromSettings()
        {
            return (GlobalSettings.Items != null && GlobalSettings.Items.ContainsKey(Setting.AssetInventoryPools) ? GlobalSettings.Items[Setting.AssetInventoryPools] : string.Empty)
                .Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        private DialogResult EditPrinter(string printerId)
        {
            DialogResult result;
            using (var bc = new BusyCursor())
            {
                using (PrinterEditForm form = new PrinterEditForm(printerId))
                {
                    result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        _bindingSource.ResetBindings(false);
                    }
                }
            }
            return result;
        }

        private void edit_Button_Click(object sender, EventArgs e)
        {
            if (radGridViewPrinters.SelectedRows.Count == 1)
            {
                // get underlying object from grid and database
                Printer selectedPrinter = radGridViewPrinters.SelectedRows[0].DataBoundItem as Printer;

                // use database object if found, otherwise use grid object
                if (EditPrinter(selectedPrinter.AssetId) == DialogResult.OK)
                {
                    RefreshItems();
                }
            }
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            // First try to query for the device and determine if you can get some
            // information from it automatically.

            IDevice device = null;
            string address = string.Empty;
            string password = string.Empty;
            string address2 = string.Empty;

            using (var dialog = new SelectPrinterDialog())
            {
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    if (! dialog.ManualEntry)
                    {
                        address = dialog.Address1;
                        password = dialog.DevicePassword;
                        address2 = dialog.Address2;

                        if (string.IsNullOrWhiteSpace(address))
                        {
                            return;
                        }

                        try
                        {
                            device = ConnectToDevice(address, address2, password);
                        }
                        catch (Exception ex)
                        {
                            var result = MessageBox.Show
                            (
                                "Unable to connect to device.  {0} {1}{2}Enter properties manually?"
                                    .FormatWith(ex.Message, Environment.NewLine, Environment.NewLine),
                                "Connection Error",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error
                            );

                            if (result == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }
                }
            }

            Printer printer = new Printer()
            {
                AssetType = "Printer",
                Address1 = address,
                PortNumber = 9100,
                SnmpEnabled = true,
                Description = string.Empty,
                Address2 = address2,
                PrinterType = string.Empty
            };

            if (device != null)
            {
                IDeviceInfo deviceInfo = device.GetDeviceInfo();
                printer.ModelNumber = deviceInfo.ModelNumber;
                printer.Model = deviceInfo.ModelName;
                printer.SerialNumber = deviceInfo.SerialNumber;
                printer.Password = string.IsNullOrEmpty(password) ? string.Empty : password;

                if (printer.Model.StartsWith("HP"))
                {
                    printer.Product = "HP";
                }
            }

            AddPrinter(printer);
        }

        private IDevice ConnectToDevice(string address, string address2, string password)
        {
            IDevice device = null;

            if (! string.IsNullOrWhiteSpace(address))
            {
                using (new BusyCursor())
                {
                    Framework.Assets.DeviceInfo deviceInfo = new Framework.Assets.DeviceInfo(SequentialGuid.NewGuid().ToShortString(), Framework.Assets.AssetAttributes.None, string.Empty, address, address2, password, string.Empty);
                    device = DeviceConstructor.Create(deviceInfo);
                }
            }

            return device;
        }

        private void AddPrinter(Printer printer)
        {
            DialogResult result;
            using (var bc = new BusyCursor())
            {
                using (PrinterEditForm form = new PrinterEditForm(printer))
                {
                    result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        RefreshItems();
                    }
                }
            }
        }

        private void radGridViewPrinters_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = e.Row;
            if (row.IsCurrent && row.IsSelected)
            {
                edit_Button_Click(sender, EventArgs.Empty);
            }
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                Printer printer = row.DataBoundItem as Printer;
                DialogResult dialogResult = MessageBox.Show
                    (
                        "Removing Printer {0}.  Do you want to continue?".FormatWith(printer.AssetId),
                        "Delete Printer",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                if (dialogResult == DialogResult.Yes)
                {
                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        Asset asset = context.Assets.FirstOrDefault(n => n.AssetId == printer.AssetId);
                        context.Assets.Remove(asset);
                        context.SaveChanges();
                    }
                    RefreshItems();
                }
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return radGridViewPrinters.SelectedRows.FirstOrDefault();
        }

        private void manageReservationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageReservations();
        }

        private void reservationToolStripButton_Click(object sender, EventArgs e)
        {
            ManageReservations();
        }

        private void ManageReservations()
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var printer = row.DataBoundItem as Printer;

                using (var form = new AssetReservationListForm<Printer>(printer))
                {
                    form.ShowDialog();
                    RefreshItems();
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportSaveFileDialog(_directory, "Export Printer Definition Data", ImportExportType.Printer))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (new BusyCursor())
                    {
                        Refresh();
                        try
                        {
                            var file = dialog.Base.FileName;
                            _directory = Path.GetDirectoryName(file);

                            var printerContracts = new AssetContractCollection<PrinterContract>();
                            var printers = radGridViewPrinters
                                                .SelectedRows
                                                .Cast<GridViewRowInfo>()
                                                .Select(x => x.DataBoundItem)
                                                .Cast<Printer>();

                            printerContracts.Load(printers);
                            printerContracts.Export(file);

                            MessageBox.Show("Data successfully exported", "STB Data Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void importToolStripButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportOpenFileDialog(_directory, "Open STB Device Export File", ImportExportType.Printer))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var file = dialog.Base.FileName;
                    _directory = Path.GetDirectoryName(file);

                    using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                    {
                        var contracts = LegacySerializer.DeserializeDataContract<AssetContractCollection<PrinterContract>>(File.ReadAllText(file));
                        foreach (var contract in contracts)
                        {
                            var printer = ContractFactory.Create(contract, context);
                            AddPrinter(printer);
                        }
                    }
                }
            }
        }

        private void printer_ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var enable = radGridViewPrinters.SelectedRows.Count <= 1;
            reservationsToolStripMenuItem.Enabled = enable;
            editToolStripMenuItem.Enabled = enable;
        }

        private void refresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            RefreshItems();
        }
    }
}

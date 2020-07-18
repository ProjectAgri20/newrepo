using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Print;
using Telerik.WinControls.UI;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// A form that displays print drivers.
    /// </summary>
    public partial class PrintDriverConfigForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverConfigForm"/> class.
        /// </summary>
        public PrintDriverConfigForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(printDriver_RadGridView, GridViewStyle.ReadOnly);
        }

        /// <summary>
        /// Handles the Load event of the PrintDriverConfigForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintDriverConfigForm_Load(object sender, EventArgs e)
        {
            LoadPrintDrivers();
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return printDriver_RadGridView.SelectedRows.FirstOrDefault();
        }

        /// <summary>
        /// Loads the print drivers.
        /// </summary>
        private void LoadPrintDrivers()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                SortableBindingList<PrintDriverPackage> printDriverPackages = new SortableBindingList<PrintDriverPackage>();
                foreach (PrintDriverPackage driverPackage in context.PrintDriverPackages.Include("PrintDrivers").OrderBy(n => n.Name))
                {
                    printDriverPackages.Add(driverPackage);
                }
                printDriver_RadGridView.DataSource = printDriverPackages;

                printDriver_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                printDriver_RadGridView.BestFitColumns();
            }
        }

        private static string ListPrintDrivers(List<PrintDriver> drivers)
        {
            StringBuilder driverString = new StringBuilder();

            foreach (PrintDriver driver in drivers)
            {
                driverString.Append("{0}\n".FormatWith(driver.Name));
            }

            return driverString.ToString();
        }

        private void add_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (PrintDriverImportWizardForm iwf = new PrintDriverImportWizardForm())
            {
                if (iwf.ShowDialog(this) != DialogResult.Cancel)
                {
                    LoadPrintDrivers();
                }
            }
        }

        private void remove_ToolStripButton_Click(object sender, EventArgs e)
        {
            List<PrintDriver> drivers  = new List<PrintDriver>();
            List<PrintDriverPackage> packages = new List<PrintDriverPackage>();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                PrintDriverPackage printDriverPackage;

                foreach (var row in printDriver_RadGridView.SelectedRows)
                {
                    string selectedCell = row.Cells["Name"].Value.ToString();
                    printDriverPackage = context.PrintDriverPackages.Include("PrintDrivers").FirstOrDefault(n => n.Name == selectedCell);
                    packages.Add(printDriverPackage);
                    drivers.AddRange(context.PrintDrivers.Include("PrintDriverPackage").Where(n => n.PrintDriverPackage.PrintDriverPackageId == printDriverPackage.PrintDriverPackageId));
                }

                DialogResult result = MessageBox.Show("Are you sure you want to remove the following drivers?\n{0}".FormatWith(ListPrintDrivers(drivers)), "Remove Selected Drivers", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (PrintDriver driver in drivers)
                    {
                        context.PrintDrivers.Remove(driver);
                    }

                    foreach (PrintDriverPackage package in packages)
                    {
                        //if the printdriver package was only 64bit then it would wipe out the entire print driver repository as the empty string would result in base repository being passed for deletion
                        if (!string.IsNullOrEmpty(package.InfX86))
                        {
                            RemoveDriverPackageFile(package.InfX86);
                        }
                        if (!string.IsNullOrEmpty(package.InfX64))
                        {
                            RemoveDriverPackageFile(package.InfX64);
                        }
                        context.PrintDriverPackages.Remove(package);
                    }

                    context.SaveChanges();
                    MessageBox.Show("Drivers removed");
                    LoadPrintDrivers();                
                }
            }
        }

        private void RemoveDriverPackageFile(string path)
        {
            StringBuilder settings = new StringBuilder();
            settings.Append(GlobalSettings.Items[Setting.PrintDriverServer]);
            settings.Append('\\');
            settings.Append(path);

            string directoryPath = GetParentDirectoryPath(settings.ToString());

            //if the directory doesn't exist then don't try to delete it, this would happen if both 64bit and 32 bit files resided in same path
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            // Added a try catch because if the directory does not exist, we still want to be able to remove the driver from the database.
            // Otherwise it would have to be done manually.
            try
            {
                DeleteParentDirectoryIfEmpty(directoryPath);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex.Message);
            }
            
        }

        private static string GetParentDirectoryPath(string path)
        {
            object[] obj = path.ToString().Split('\\');

            StringBuilder directory = new StringBuilder();

            for (int i = 1; i < obj.Length - 1; i++)
            {
                directory.Append('\\');
                directory.Append(obj[i]);
            }

            return directory.ToString();
        }

        private static bool DirectoryIsRoot(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);

            if (info.Parent == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DeleteParentDirectoryIfEmpty(string path)
        {
            string parentDirectory = GetParentDirectoryPath(path);

            if (DirectoryIsRoot(parentDirectory))
            {
                return;
            }

            if (Directory.GetDirectories(parentDirectory).Length == 0)
            {
                Directory.Delete(parentDirectory);
                DeleteParentDirectoryIfEmpty(parentDirectory);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Gives the user the option of browsing to the directory containing the discrete driver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDiscrete_Button_Click(object sender, EventArgs e)
        {
            using (PrintDriverImportWizardForm iwf = new PrintDriverImportWizardForm())
            {
                iwf.ShowDialog(this);
            }

            return;            
        }

        /// <summary>
        /// add the driver to the repository
        /// </summary>
        /// <param name="driverDirectory"></param>
        private void AddDiscretePrinterDriver(string driverDirectory)
        {
            //only searching the top level directory for INF files, the discrete driver has lot of INF files in various directory, which is unnecessary to go through.
            if(Directory.GetFiles(driverDirectory, "*.inf", SearchOption.TopDirectoryOnly).Count() == 0)
            {
                MessageBox.Show("The selected directory does not contain any print drivers, please select the directory which has the print driver setup information file (*.inf).", "Print Driver not found", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

            //add the x86 and x64 drivers
            foreach (string fileName in Directory.GetFiles(driverDirectory, "*.inf", SearchOption.TopDirectoryOnly))
            {
                using (DriverInfReader reader = new DriverInfReader(fileName))
                {
                    DriverInfParser parser = new DriverInfParser(reader);
                    DriverInf inf = parser.BuildInf();
                    if (inf.DriverClass == "Printer")
                    {
                        string driverName = parser.GetDiscreteDriverName();
                        UploadDiscreteDriver(inf, driverName);
                    }
                }
            }
        }

        private void UploadDiscreteDriver(DriverInf infFile, string driverName)
        {
            string discreteDriverRepo = GlobalSettings.Items[Setting.PrintDriverServer];

            PrintDriverPackage newDiscretePrintDriverPackage = new PrintDriverPackage();
            newDiscretePrintDriverPackage.Name = new DirectoryInfo(Path.GetDirectoryName(infFile.Location)).Name;

            //if this is a fax driver, ignore it
            if (driverName.Contains("fax", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                //if the driver package already exist just update the x86 or x64 driver inf file
                var discreteDriver = context.PrintDriverPackages.Include("PrintDrivers").FirstOrDefault(n => n.Name == newDiscretePrintDriverPackage.Name);

                if (discreteDriver != null)
                {
                    newDiscretePrintDriverPackage = discreteDriver;

                    if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTAMD64))
                    {
                        //passing only the path relative to the repository
                        newDiscretePrintDriverPackage.InfX64 = Path.Combine(newDiscretePrintDriverPackage.Name, Path.GetFileName(infFile.Location));
                    }

                    if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTx86))
                    {
                        //passing only the path relative to the repository
                        newDiscretePrintDriverPackage.InfX86 = Path.Combine(newDiscretePrintDriverPackage.Name, Path.GetFileName(infFile.Location));
                    }

                }
                else
                {
                    //this is a new upload, add the necessary details
                    newDiscretePrintDriverPackage.PrintDriverPackageId = SequentialGuid.NewGuid();

                    newDiscretePrintDriverPackage.InfX64 = string.Empty;
                    newDiscretePrintDriverPackage.InfX86 = string.Empty;

                    if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTAMD64))
                    {
                        //passing only the path relative to the repository
                        newDiscretePrintDriverPackage.InfX64 = Path.Combine(newDiscretePrintDriverPackage.Name, Path.GetFileName(infFile.Location));
                    }


                    if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTx86))
                    {
                        //passing only the path relative to the repository
                        newDiscretePrintDriverPackage.InfX86 = Path.Combine(newDiscretePrintDriverPackage.Name, Path.GetFileName(infFile.Location));
                    }

                    //the folder does not exist, add them up to the repository
                    if (Directory.Exists(Path.Combine(discreteDriverRepo, newDiscretePrintDriverPackage.Name)))
                    {
                        TraceFactory.Logger.Debug("Driver Package already exists");
                    }
                    else
                    {
                        //this will take sometime depending upon size of the driver package
                        FileSystem.CopyDirectory(Path.GetDirectoryName(infFile.Location), Path.Combine(discreteDriverRepo, newDiscretePrintDriverPackage.Name));
                    }


                    context.PrintDriverPackages.Add(newDiscretePrintDriverPackage);
                }

                PrintDriver newdiscreteDriver = null;

                newdiscreteDriver = context.PrintDrivers.FirstOrDefault(n => n.Name.Equals(driverName));
                if (newdiscreteDriver == null || !newdiscreteDriver.PrintDriverPackage.Name.EqualsIgnoreCase(newDiscretePrintDriverPackage.Name))
                {
                    newdiscreteDriver = new PrintDriver();
                    newdiscreteDriver.Name = driverName;
                    newdiscreteDriver.PrintDriverId = SequentialGuid.NewGuid();
                    newdiscreteDriver.PrintDriverPackage = newDiscretePrintDriverPackage;
                    newdiscreteDriver.PrintProcessor = infFile.Drivers.FirstOrDefault().PrintProcessor;

                    context.PrintDrivers.Add(newdiscreteDriver);

                }
                context.SaveChanges();
            }
        }
    }
}

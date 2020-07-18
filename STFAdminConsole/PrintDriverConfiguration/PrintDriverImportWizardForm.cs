using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.WindowsAutomation;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.LabConsole
{
    public partial class PrintDriverImportWizardForm : Form
    {
        private DriverImportWelcomeControl _welcomeControl = null;
        private DriverImportCompletionControl _completionControl = null;
        
        public PrintDriverImportWizardForm()
        {
            InitializeComponent();

            _welcomeControl = new DriverImportWelcomeControl();
            _welcomeControl.AllowSelectionOptions = AllowOptions();
            _welcomeControl.Dock = DockStyle.Fill;
            welcomePanel.Controls.Add(_welcomeControl);

            _completionControl = new DriverImportCompletionControl();
            _completionControl.Dock = DockStyle.Fill;
            completionPanel.Controls.Add(_completionControl);
            
            wizardControl.Cancel += wizardControl_Cancel;
            //wizardControl.Previous += wizardControl_Previous;
            wizardControl.Next += wizardControl_Next;
            wizardControl.Finish += wizardControl_Finish;
            //wizardControl.SelectedPageChanged += wizardControl_SelectedPageChanged;

            wizardControl.SelectedPageChanged += WizardControl_SelectedPageChanged;
            wizardControl.SelectedPageChanging += WizardControl_SelectedPageChanging;
        }

        private bool AllowOptions()
        {
            bool enterpriseEnabled = false;

            try
            {
                enterpriseEnabled = GlobalSettings.Items[Setting.EnterpriseEnabled].Equals("True", StringComparison.InvariantCultureIgnoreCase);
            }
            catch (SettingNotFoundException)
            {
            }

            return GlobalSettings.IsDistributedSystem || enterpriseEnabled;
        }

        private void WizardControl_SelectedPageChanging(object sender, SelectedPageChangingEventArgs e)
        {
            Cursor = Cursors.WaitCursor;    
        }

        private void WizardControl_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void wizardControl_Cancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void wizardControl_Next(object sender, WizardCancelEventArgs e)
        {
            WizardPage currentPage = wizardControl.Pages.Where(x => x.IsSelected).FirstOrDefault();

            if (currentPage == wizardWelcomePage)
            {
                if (! _welcomeControl.ValidateChildren())
                {
                    e.Cancel = true;
                    return;
                }

                try
                {
                    Cursor = Cursors.WaitCursor;
                    _completionControl.DriverOrigination = _welcomeControl.PrintDriverLocation;
                    _completionControl.Initialize(_welcomeControl.PrintDriverPaths);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void wizardControl_Finish(object sender, EventArgs e)
        {
            bool result = true;

            Cursor.Current = Cursors.WaitCursor;
            switch (_welcomeControl.PrintDriverLocation)
            {
                case PrintDriverLocation.InstalledDriverFolder:
                    result = ImportFromFolder();
                    break;
                case PrintDriverLocation.DriverPackageFile:
                    result = ImportPackageFile();
                    break;
                case PrintDriverLocation.CentralRepository:
                    ImportFromCentral();
                    break;
            }
            Cursor.Current = Cursors.Default;

            if (result)
            {
                this.DialogResult = DialogResult.OK;
            }            
        }

        /// <summary>
        /// Import the drivers from the central repository location to the STF repository.
        /// </summary>
        private void ImportFromCentral()
        {
            Collection<string> paths = _welcomeControl.PrintDriverPaths;

            string sharePath = GlobalSettings.Items[Setting.UniversalPrintDriverBaseLocation];

            Cursor.Current = Cursors.WaitCursor;

            Collection<PrintDeviceDriverCollection> drivers = new Collection<PrintDeviceDriverCollection>();
            foreach (string path in paths)
            {
                string sourcePath = Path.Combine(sharePath, path);
                drivers.Add(PrintDriversManager.LoadDrivers(sourcePath, sharePath));
            }

            bool success = AddDrivers(drivers);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Adds the drivers to the STF repository.
        /// </summary>
        /// <param name="driverSet">The drivers.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns></returns>
        private bool AddDrivers(Collection<PrintDeviceDriverCollection> driverSet, bool overwrite = false)
        {
            try
            {
                // Attempt to add the drivers to the environment.  If it works, return true;
                // This also adds any necessary database entries for each driver.
                PrintDriversManager.AddToFrameworkRepository(driverSet, overwrite);
                return true;
            }
            catch (IOException ex)
            {
                TraceFactory.Logger.Error(ex.Message);
                // If we were not trying to overwrite, ask the user if they want to
                if (!overwrite)
                {
                    DialogResult result = MessageBox.Show("One or more of the selected print drivers have already been added to the test environment.\n"
                                                        + "Do you wish to overwrite these drivers?",
                                                        "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // User wants to overwrite - call the method again
                        return AddDrivers(driverSet, overwrite: true);
                    }
                }

                // If we were trying to overwrite and it failed, or the user opted not to overwrite,
                // return a failure condition.
                return false;
            }
            catch (OperationCanceledException)
            {
                // The user canceled the copy operation.
                return false;
            }
        }

        /// <summary>
        /// Import the driver from a pre-installed directory location.
        /// </summary>
        private bool ImportFromFolder()
        {
            string source = _welcomeControl.PrintDriverPaths.FirstOrDefault();
            string destination = _completionControl.DestinationPaths.FirstOrDefault();

            var path = Path.Combine(PrintDriversManager.DriverRepositoryLocation, destination);
            if (!CheckDestination(path)) return false;

            PrintDriverPackage printDriverPackage = null;
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                //add the drivers
                using (DriverInfReader reader = new DriverInfReader(source))
                {
                    DriverInfParser parser = new DriverInfParser(reader);
                    DriverInf inf = parser.BuildInf();
                    string driverName = parser.GetDiscreteDriverName();

                    //if this is a fax driver, ignore it
                    if (!driverName.Contains("fax", StringComparison.OrdinalIgnoreCase))
                    {
                        printDriverPackage = UpdateRepositoryDatabase(inf, driverName, destination, context);
                        UploadFolder(inf, printDriverPackage);
                    }
                }

                context.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Upload the driver to the repository
        /// </summary>
        /// <param name="infFile"></param>
        private void UploadFolder(DriverInf infFile, PrintDriverPackage printDriverPackage)
        {
            string discreteDriverRepo = PrintDriversManager.DriverRepositoryLocation;
            var path = Path.Combine(discreteDriverRepo, printDriverPackage.Name);

            try
            {
                //this will take some time depending upon size of the driver package
                FileSystem.CopyDirectory(Path.GetDirectoryName(infFile.Location), path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving driver {0} to Driver Repository. {1}".FormatWith(printDriverPackage.Name, ex.Message), "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// <summary>
        /// Import a driver package file (Self-extracting .exe or .zip)
        /// </summary>
        private bool ImportPackageFile()
        {
            string source = _welcomeControl.PrintDriverPaths.FirstOrDefault();
            string destinationDirectory = _completionControl.DestinationPaths.FirstOrDefault();
            string destination = Path.Combine(PrintDriversManager.DriverRepositoryLocation, destinationDirectory);

            try
            {
                if (!CheckDestination(destination)) return false;

                // Unzips the package to the destination server
                ZipFile.ExtractToDirectory(source, destination);

                // Read .inf files in the new destination folder to create database entries
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    //add the x86 and x64 drivers
                    foreach (string fileName in Directory.GetFiles(destination, "*.inf", SearchOption.TopDirectoryOnly))
                    {
                        using (DriverInfReader reader = new DriverInfReader(fileName))
                        {
                            DriverInfParser parser = new DriverInfParser(reader);
                            DriverInf inf = parser.BuildInf();
                            if (inf.DriverClass == "Printer")
                            {
                                //if this is a fax driver, ignore it
                                string driverName = parser.GetDiscreteDriverName();
                                if (!driverName.Contains("fax", StringComparison.OrdinalIgnoreCase))
                                {
                                    UpdateRepositoryDatabase(inf, driverName, destinationDirectory, context);
                                }

                            }
                        }
                    }
                    context.SaveChanges();
                }

                return true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TraceFactory.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message, "Import Package File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (InvalidDataException ex)
            {
                TraceFactory.Logger.Error(ex.Message, ex);
                StringBuilder message = new StringBuilder("Unable to extract:");
                message.AppendLine();
                message.AppendLine(source);
                message.AppendLine("Try manually extracting the driver package, then import using 'Driver INF Folder'.");
                MessageBox.Show(message.ToString(), "Import Package File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return false;
        }

        private bool CheckDestination(string destination)
        {
            bool result = true;

            try
            {
                if (Directory.Exists(destination) && Directory.EnumerateFileSystemEntries(destination).Any())
                {
                    MessageBox.Show("Your selected repository location already contains a driver.  Please select another location.", "Driver Data Already Exists?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error communicating with Driver Repository. Unable to save driver", "Driver Repository Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error(ex.Message);
                result = false;
            }

            return result;
        }

        private PrintDriverPackage UpdateRepositoryDatabase(DriverInf infFile, string driverName, string packageVersion, AssetInventoryContext context)
        {
            string driverRepo = PrintDriversManager.DriverRepositoryLocation;

            PrintDriverPackage printDriverPackage = new PrintDriverPackage();
            printDriverPackage.Name = packageVersion;   //new DirectoryInfo(Path.GetDirectoryName(infFile.Location)).Name;

            //if the driver package already exists just update the x86 or x64 driver inf file
            PrintDriverPackage existingDriverPackage = context.PrintDriverPackages.Include("PrintDrivers").FirstOrDefault(n => n.Name == printDriverPackage.Name);

            if (existingDriverPackage != null)
            {
                printDriverPackage = existingDriverPackage;

                if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTAMD64))
                {
                    //passing only the path relative to the repository
                    printDriverPackage.InfX64 = Path.Combine(printDriverPackage.Name, Path.GetFileName(infFile.Location));
                }

                if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTx86))
                {
                    //passing only the path relative to the repository
                    printDriverPackage.InfX86 = Path.Combine(printDriverPackage.Name, Path.GetFileName(infFile.Location));
                }

            }
            else
            {
                //this is a new upload, add the necessary details
                printDriverPackage.PrintDriverPackageId = SequentialGuid.NewGuid();

                printDriverPackage.InfX64 = string.Empty;
                printDriverPackage.InfX86 = string.Empty;

                if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTAMD64))
                {
                    //passing only the path relative to the repository
                    printDriverPackage.InfX64 = Path.Combine(printDriverPackage.Name, Path.GetFileName(infFile.Location));
                }


                if (infFile.Drivers.Any(n => n.Architecture == DriverArchitecture.NTx86))
                {
                    //passing only the path relative to the repository
                    printDriverPackage.InfX86 = Path.Combine(printDriverPackage.Name, Path.GetFileName(infFile.Location));
                }

                context.PrintDriverPackages.Add(printDriverPackage);
            }

            PrintDriver newPrintDriver = context.PrintDrivers.FirstOrDefault(n => n.Name.Equals(driverName));

            if (newPrintDriver == null || !newPrintDriver.PrintDriverPackage.Name.EqualsIgnoreCase(printDriverPackage.Name))
            {
                newPrintDriver = new PrintDriver();
                newPrintDriver.Name = driverName;
                newPrintDriver.PrintDriverId = SequentialGuid.NewGuid();
                newPrintDriver.PrintDriverPackage = printDriverPackage;
                newPrintDriver.PrintProcessor = infFile.Drivers.FirstOrDefault().PrintProcessor;

                context.PrintDrivers.Add(newPrintDriver);
            }

            return printDriverPackage;
        }


    } //PrintDriverImportWizardForm

    public enum PrintDriverLocation
    {
        InstalledDriverFolder,
        DriverPackageFile,
        CentralRepository
    }
}

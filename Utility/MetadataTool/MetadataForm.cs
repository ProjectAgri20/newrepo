using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using MetadataTool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.Utility.BtfMetadataHelper
{
    public partial class MetadataForm : Form
    {
        private EnterpriseScenario _scenario;
        private string _currentDatabase;
        private readonly string _tmsServer;
     

        public MetadataForm()
        {
            InitializeComponent();
            status_comboBox.DataSource = Enum.GetNames(typeof(Status));
            classification_comboBox.DataSource = Enum.GetNames(typeof(TestClassification));
            NameValueCollection btfCollection = ConfigurationManager.GetSection("BTF") as NameValueCollection;

            if (btfCollection !=null)
                _tmsServer = btfCollection["Tms"];
        }

        protected override void OnLoad(EventArgs e)
        {
            if (STFLoginManager.Login())
            {
                _currentDatabase = STFLoginManager.SystemDatabase;
                //Set whether STF or STB based on the worker type in the database.
                string officeWorkerType = VirtualResourceType.OfficeWorker.ToString();
                using (EnterpriseTestContext dataContext = new EnterpriseTestContext(_currentDatabase))
                {
                    GlobalSettings.IsDistributedSystem = dataContext.VirtualResources.Any(r => r.ResourceType == officeWorkerType);
                }
                loggedIn_textBox.Text = $@"Logged in as: {UserManager.CurrentUserName} to {STFLoginManager.SystemDatabase}";
            }
            else
            {
                Environment.Exit(1);
            }
            base.OnLoad(e);
        }

        private void scenarioSelection_Button_Click(object sender, EventArgs e)
        {
            using (ScenarioSelectionForm selectionForm = new ScenarioSelectionForm())
            {
                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    using (EnterpriseTestContext context = new EnterpriseTestContext())
                    {
                        LoadScenario(selectionForm.SelectedScenarioId, context);
                    }
                }
            }
        }

        private void LoadScenario(Guid scenarioId, EnterpriseTestContext context)
        {
            EnterpriseScenario scenario = EnterpriseScenario.Select(context, scenarioId);

            if (scenario != null)
            {
                _scenario = scenario.Clone();
                selectedScenario_TextBox.Text = _scenario.Name;
            }
        }

        private void generate_button_Click(object sender, EventArgs e)
        {
            if (_scenario == null)
            {
                return;
            }

            TestMetadata metadata = new TestMetadata
            {
                file_url = $"{_scenario.EnterpriseScenarioId}",
                name = _scenario.Name,
                is_manual = 0,
                last_mod_by = UserManager.CurrentUserName,
                last_mod_date = DateTime.Now,
                primary_owner_entity_type = "STF",
                primary_owner_entity_name = owner_comboBox.Text,
                purpose = purpose_textBox.Text,
                status = (Status)status_comboBox.SelectedIndex,
                test_classification = (TestClassification)classification_comboBox.SelectedIndex,
                test_framework = TestFrameWork.Stf,
                test_tier = 1,
                timeout = TimeSpan.FromHours(_scenario.EstimatedRuntime).TotalMinutes.ToString(CultureInfo.InvariantCulture),
                title = _scenario.Name,
                version = "1.0",
                test_dependencies = new List<string>(),
                resources = new List<Dictionary<string, object>>()
            };
            Dictionary<string, object> resourceDictionary = new Dictionary<string, object>
            {
                {"type", 18},
                {"id", "STF"},
                {"StfWebApiServer", $"http://{_currentDatabase}:9000/api/Session"}
            };
            metadata.resources.Add(resourceDictionary);


            TestDiscovery testDiscovery = new TestDiscovery
            {
                data = new List<TestMetadata> { metadata },
                datatype = "tests",
                repo_type = "sqlserver",
                repo_branch = "EnterpriseTest",
                repo_url = _currentDatabase
            };

            var metadataFilePath = Path.Combine(Path.GetTempPath(), "TestsDiscovered.json");

            var jsonString = JsonConvert.SerializeObject(testDiscovery);
            File.WriteAllText(metadataFilePath, jsonString);

            var metadataTarFilePath = Path.Combine(Path.GetTempPath(), "TestsDiscovered.tar");

            using (var targetStream = new TarOutputStream(File.Create(metadataTarFilePath)))
            {
                CreateTarManually(targetStream, metadataFilePath);
            }
            //now pack it using BZ2 compression
            var metadataBzFilePath = metadataTarFilePath + ".bz2";

            using (Stream tarInputStream = File.OpenRead(metadataTarFilePath))
            {
                using (Stream tarOutputStream = File.Create(metadataBzFilePath))
                {
                    BZip2.Compress(tarInputStream, tarOutputStream, true, 9);
                }
            }

            HttpClientHandler handler = new HttpClientHandler();
            using (HttpClient httpClient = new HttpClient(handler))
            {
                using (var multiPartFormData = new MultipartFormDataContent())
                {
                    using (var fileContent = new StreamContent(File.OpenRead(metadataBzFilePath)))
                    {
                        fileContent.Headers.Add("Content-Type", "application/x-bzip2");
                        fileContent.Headers.Add("Content-Disposition",
                            "form-data; name=\"file\"; filename=\"" + (metadataBzFilePath) + "\"");
                        multiPartFormData.Add(fileContent, "file", metadataBzFilePath);

                        var message = httpClient.PutAsync(
                            $"https://{_tmsServer}/tms/v1/testcases/uploadtestcases/",
                            multiPartFormData);

                        if (message.Result.IsSuccessStatusCode)
                        {
                            MessageBox.Show(@"Test Case submitted.",@"BTF Metadata Helper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void CreateTarManually(TarOutputStream tarOutputStream, string sourceFile)
        {
            // You might replace these 3 lines with your own stream code

            using (Stream inputStream = File.OpenRead(sourceFile))
            {
                string tarName = Path.GetFileName(sourceFile);//.Substring(3); // strip off "C:\"

                long fileSize = inputStream.Length;

                // Create a tar entry named as appropriate. You can set the name to anything,
                // but avoid names starting with drive or UNC.
                TarEntry entry = TarEntry.CreateTarEntry(tarName);

                // Must set size, otherwise TarOutputStream will fail when output exceeds.
                entry.Size = fileSize;

                // Add the entry to the tar stream, before writing the data.
                tarOutputStream.PutNextEntry(entry);

                // this is copied from TarArchive.WriteEntryCore
                byte[] localBuffer = new byte[32 * 1024];
                while (true)
                {
                    int numRead = inputStream.Read(localBuffer, 0, localBuffer.Length);
                    if (numRead <= 0)
                        break;

                    tarOutputStream.Write(localBuffer, 0, numRead);
                }
            }
            tarOutputStream.CloseEntry();
        }

       
    }
}
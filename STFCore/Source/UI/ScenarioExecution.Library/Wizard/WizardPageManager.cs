using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    /// <summary>
    /// <see cref="WizardScenarioSelectionPage"/> and <see cref="WizardScenarioBatchPage"/> utilize some of the same methods for populating and validating session startup data.
    /// This class is a central location to provide that functionality to both wizard pages.
    /// </summary>
    internal static class WizardPageManager
    {
        /// <summary>
        /// Matches the DefaultLogRetention setting in SystemSettings with the SessionLogRetention enum.
        /// Example:
        /// SystemSetting value: 25
        /// Will return 7 days because 25 is greater than 7 and less than 30.
        /// </summary>
        /// <returns>The highest SessionLogRetention value that matches the setting.</returns>
        public static SessionLogRetention GetDefaultLogRetention()
        {
            int setting = int.Parse(GlobalSettings.Items[Setting.DefaultLogRetention], CultureInfo.InvariantCulture);

            if (setting < (int)SessionLogRetention.None)
            {
                // Exceeds lower bound of SessionLogRetention enum, default to first item
                return SessionLogRetention.None;
            }

            int result = (int)SessionLogRetention.None;
            foreach (SessionLogRetention enumValue in Enum.GetValues(typeof(SessionLogRetention)))
            {
                int convertedVal = (int)enumValue;
                if (setting >= convertedVal)
                {
                    result = convertedVal;
                }
                else //setting < convertedVal
                {
                    break;
                }
            }

            return (SessionLogRetention)result;
        }

        /// <summary>
        /// This is a temporary method to satisfy PR 46688:
        /// Temporarily modify STB to use the Reference column in ScalableTestDatalog.SessionSummary to identify STB sessions
        /// The intention is that after STF Services are modified to handle different logging locations, this can be removed.
        /// Update 6/7/2018: It is helpful to the testers to know where the test is running.  This information would be better
        /// suited to a log table.  Even though the above PR has been completed, it still seems some log data is missing.
        /// </summary>
        /// <returns></returns>
        public static string GetReferenceData(TextBox reference_TextBox)
        {
            if (!GlobalSettings.IsDistributedSystem)
            {
                StringBuilder result = new StringBuilder("STB:");
                result.Append(GlobalSettings.Database);
                if (!string.IsNullOrEmpty(reference_TextBox.Text))
                {
                    result.Append("|");
                    result.Append(reference_TextBox.Text);
                }
                return result.ToString();
            }

            return reference_TextBox.Text;
        }

        /// <summary>
        /// Gets the AssociatedProducts for the given scenario.
        /// </summary>
        /// <param name="context">The EnterpriseTest data context.</param>
        /// <param name="scenario">The scenario.</param>
        /// <returns>The AssociatedProducts for the given scenario.</returns>
        public static IEnumerable<ScenarioProduct> GetAssociatedProducts(EnterpriseTestContext context, EnterpriseScenario scenario)
        {
            List<string> metadataTypes = new List<string>();
            foreach (VirtualResource vr in scenario.VirtualResources)
            {
                foreach (var vrms in vr.VirtualResourceMetadataSet)
                {
                    metadataTypes.Add(vrms.MetadataType);
                }
            }

            List<AssociatedProduct> associatedProducts = context.AssociatedProducts.Where(n => n.MetadataTypes.Any(m => metadataTypes.Contains(m.Name))).ToList();
            IEnumerable<Guid> productIds = associatedProducts.Select(x => x.AssociatedProductId);
            List<AssociatedProductVersion> productVersions = AssociatedProductVersion.SelectVersions(context, productIds, scenario.EnterpriseScenarioId).ToList();

            return from productInfo in associatedProducts
                   join versionInfo in productVersions
                   on productInfo.AssociatedProductId equals versionInfo.AssociatedProductId
                   select new ScenarioProduct
                              {
                                  ProductId = productInfo.AssociatedProductId,
                                  Version = versionInfo.Version,
                                  ScenarioId = versionInfo.EnterpriseScenarioId,
                                  Name = productInfo.Name,
                                  Vendor = productInfo.Vendor,
                                  Active = versionInfo.Active
                              };
        }

        /// <summary>
        /// Validates scenario data for the given scenario and checks to ensure enough free VMs (by platform type).
        /// </summary>
        /// <param name="scenario">The scenario to check.</param>
        /// <param name="platform"></param>
        /// <returns>true if the check passes, false otherwise.</returns>
        public static bool PerformScenarioIntegrityCheck(EnterpriseScenario scenario, FrameworkClientPlatform platform)
        {
            return PerformScenarioIntegrityCheck(scenario, platform, null);
        }

        /// <summary>
        /// Validates scenario data for the given scenario and checks to ensure enough free VMs (by platform type).
        /// </summary>
        /// <param name="scenario">The scenario to check.</param>
        /// <param name="holdId">The Hold Id.</param>
        /// <returns>true if the check passes, false otherwise.</returns>
        public static bool PerformScenarioIntegrityCheck(EnterpriseScenario scenario, string holdId)
        {
            return PerformScenarioIntegrityCheck(scenario, null, holdId);
        }

        /// <summary>
        /// Validates scenario data for the given scenario and checks to ensure enough free VMs (by platform type).
        /// </summary>
        /// <param name="scenario">The scenario to check.</param>
        /// <returns>true if the check passes, false otherwise.</returns>
        public static bool PerformScenarioIntegrityCheck(EnterpriseScenario scenario)
        {
            return PerformScenarioIntegrityCheck(scenario, null, null);
        }

        /// <summary>
        /// Validates scenario data for the given scenario and checks to ensure enough free VMs (by platform type).
        /// </summary>
        /// <param name="scenario">The scenario to check.</param>
        /// <param name="platform">The Virtual Machine Platform requested, if any.</param>
        /// <param name="holdId">The Hold Id, if any.</param>
        /// <returns>true if the check passes, false otherwise.</returns>
        private static bool PerformScenarioIntegrityCheck(EnterpriseScenario scenario, FrameworkClientPlatform platform, string holdId)
        {
            List<string> issues = scenario.ValidateData().ToList();
            issues.AddRange(ValidateUsages(scenario));

            // STF-only
            if (GlobalSettings.IsDistributedSystem)
            {
                if (!CheckPlatformsAvailable(scenario, platform, holdId))
                {
                    return false;
                }
            }

            if (issues.Any())
            {
                StringBuilder message = new StringBuilder("The following issues were found with the selected scenario:\n");
                message.Append(string.Join("\n", issues));
                message.Append("\n\nDo you want to continue?");
                DialogResult result = MessageBox.Show(message.ToString(), "Scenario Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return (result == DialogResult.Yes);
            }

            return true;
        }

        /// <summary>
        /// Checks the available VM Platforms in the database to make sure that 
        /// the number of VMs needed for the test does not exceed the available.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="platform"></param>
        /// <param name="holdId"></param>
        /// <returns>true if enough VM Platforms are available, false otherwise.</returns>
        private static bool CheckPlatformsAvailable(EnterpriseScenario scenario, FrameworkClientPlatform platform, string holdId)
        {
            ScenarioPlatformUsageSet usages = new ScenarioPlatformUsageSet(scenario);
            if (platform != null)
            {
                usages.Load(UserManager.CurrentUser, platform);
            }
            else if (string.IsNullOrEmpty(holdId) == false)
            {
                usages.Load(UserManager.CurrentUser, holdId);
            }
            else
            {
                usages.Load(UserManager.CurrentUser);
            }

            if (!usages.PlatformsAvailable)
            {
                using (InsufficientPlatformsErrorForm form = new InsufficientPlatformsErrorForm(usages))
                {
                    form.ShowDialog();
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<string> ValidateUsages(EnterpriseScenario scenario)
        {
            List<VirtualResourceMetadata> metadatas = scenario.VirtualResources.SelectMany(v => v.VirtualResourceMetadataSet).ToList();

            AssetInventoryContext assetContext = DbConnect.AssetInventoryContext();
            DocumentLibraryContext docContext = DbConnect.DocumentLibraryContext();

            try
            {
                StringBuilder message = new StringBuilder();
                foreach (VirtualResourceMetadata metadata in metadatas)
                {
                    message.Clear();
                    if (! ValidatePrintQueueUsage(metadata.PrintQueueUsage, assetContext))
                    {
                        message.Append(metadata.Name);
                        message.Append(" has no Print Queue definition");
                    }
                    if (! ValidateDocumentUsage(metadata.DocumentUsage, docContext))
                    {
                        message.Append(message.Length == 0 ? $"{metadata.Name} has no " : " or ");
                        message.Append("documents selected");
                    }
                    if (message.Length > 0)
                    {
                        message.Append(".");
                        yield return message.ToString();
                    }
                }
            }
            finally
            {
                assetContext.Dispose();
                docContext.Dispose();
            }
        }

        private static bool ValidatePrintQueueUsage(VirtualResourceMetadataPrintQueueUsage queueUsage, AssetInventoryContext context)
        {
            bool result = true;
            if (queueUsage != null)
            {
                PrintQueueSelectionData docSelection = Serializer.Deserialize<PrintQueueSelectionData>(XElement.Parse(queueUsage.PrintQueueSelectionData));
                foreach (RemotePrintQueueDefinition def in docSelection.SelectedPrintQueues.OfType<RemotePrintQueueDefinition>())
                {
                    result &= context.RemotePrintQueues.Any(q => q.RemotePrintQueueId == def.PrintQueueId);
                }
            }
            return result;
        }

        private static bool ValidateDocumentUsage(VirtualResourceMetadataDocumentUsage docUsage, DocumentLibraryContext context)
        {
            bool result = true;
            if (docUsage != null)
            {
                DocumentSelectionData docSelection = Serializer.Deserialize<DocumentSelectionData>(XElement.Parse(docUsage.DocumentSelectionData));
                foreach (Guid docId in docSelection.SelectedDocuments)
                {
                    result &= context.TestDocuments.Any(d => d.TestDocumentId == docId);
                }
            }
            return result;
        }

    }
}

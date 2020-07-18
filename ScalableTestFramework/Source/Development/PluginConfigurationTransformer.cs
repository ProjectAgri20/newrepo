using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Utility class that transforms selection criteria from <see cref="PluginConfigurationData" />
    /// into execution info for <see cref="PluginExecutionData" />.
    /// </summary>
    public static class PluginConfigurationTransformer
    {
        /// <summary>
        /// Transforms <see cref="AssetSelectionData" /> from a <see cref="PluginConfigurationData" /> object into a
        /// corresponding <see cref="AssetInfoCollection" /> using data from the specified <see cref="IAssetInventory" />.
        /// </summary>
        /// <param name="configurationData">The <see cref="PluginConfigurationData" />.</param>
        /// <param name="assetInventory">The <see cref="IAssetInventory" />.</param>
        /// <returns>An <see cref="AssetInfoCollection" /> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurationData" /> is null.
        /// <para>or</para>
        /// <paramref name="assetInventory" /> is null.
        /// </exception>
        public static AssetInfoCollection GetExecutionAssets(PluginConfigurationData configurationData, IAssetInventory assetInventory)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            if (assetInventory == null)
            {
                throw new ArgumentNullException(nameof(assetInventory));
            }

            AssetSelectionData assetSelectionData = configurationData.Assets ?? new AssetSelectionData();
            return assetInventory.GetAssets(assetSelectionData.SelectedAssets);
        }

        /// <summary>
        /// Transforms <see cref="DocumentSelectionData" /> from a <see cref="PluginConfigurationData" /> object into a
        /// corresponding <see cref="DocumentCollection" /> using data from the specified <see cref="IDocumentLibrary" />.
        /// </summary>
        /// <param name="configurationData">The <see cref="PluginConfigurationData" />.</param>
        /// <param name="documentLibrary">The <see cref="IDocumentLibrary" />.</param>
        /// <returns>An <see cref="DocumentCollection" /> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurationData" /> is null.
        /// <para>or</para>
        /// <paramref name="documentLibrary" /> is null.
        /// </exception>
        public static DocumentCollection GetExecutionDocuments(PluginConfigurationData configurationData, IDocumentLibrary documentLibrary)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            if (documentLibrary == null)
            {
                throw new ArgumentNullException(nameof(documentLibrary));
            }

            DocumentSelectionData documentSelectionData = configurationData.Documents ?? new DocumentSelectionData();
            DocumentCollection documents = null;
            switch (documentSelectionData.SelectionMode)
            {
                case DocumentSelectionMode.SpecificDocuments:
                    var selected = documentLibrary.GetDocuments().Where(n => documentSelectionData.SelectedDocuments.Contains(n.DocumentId));
                    documents = new DocumentCollection(selected.ToList());
                    break;

                case DocumentSelectionMode.DocumentSet:
                    var documentSet = documentLibrary.GetDocumentSets().First(n => n.Name == documentSelectionData.DocumentSetName);
                    documents = documentSet.Documents;
                    break;

                case DocumentSelectionMode.DocumentQuery:
                    documents = documentLibrary.GetDocuments(documentSelectionData.DocumentQuery);
                    break;
            }

            return documents;
        }

        /// <summary>
        /// Transforms <see cref="ServerSelectionData" /> from a <see cref="PluginConfigurationData" /> object into the corresponding
        /// <see cref="ServerInfoCollection" /> using data from the specified <see cref="IAssetInventory" />.
        /// </summary>
        /// <param name="configurationData">The <see cref="PluginConfigurationData" />.</param>
        /// <param name="assetInventory">The <see cref="IAssetInventory" />.</param>
        /// <returns>A <see cref="ServerInfoCollection" /> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurationData" /> is null.
        /// <para>or</para>
        /// <paramref name="assetInventory" /> is null.
        /// </exception>
        public static ServerInfoCollection GetExecutionServers(PluginConfigurationData configurationData, IAssetInventory assetInventory)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            if (assetInventory == null)
            {
                throw new ArgumentNullException(nameof(assetInventory));
            }

            ServerSelectionData serverSelectionData = configurationData.Servers ?? new ServerSelectionData();
            var selectedServers = assetInventory.GetServers().Where(n => serverSelectionData.SelectedServers.Contains(n.ServerId));
            return new ServerInfoCollection(selectedServers.ToList());
        }

        /// <summary>
        /// Transforms <see cref="PrintQueueSelectionData" /> from a <see cref="PluginConfigurationData" /> object into the corresponding
        /// <see cref="PrintQueueInfoCollection" /> using data from the specified <see cref="IAssetInventory" />.
        /// </summary>
        /// <param name="configurationData">The <see cref="PluginConfigurationData" />.</param>
        /// <param name="assetInventory">The <see cref="IAssetInventory" />.</param>
        /// <returns>A <see cref="PrintQueueInfoCollection" /> object.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurationData" /> is null.
        /// <para>or</para>
        /// <paramref name="assetInventory" /> is null.
        /// </exception>
        public static PrintQueueInfoCollection GetExecutionPrintQueues(PluginConfigurationData configurationData, IAssetInventory assetInventory)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            if (assetInventory == null)
            {
                throw new ArgumentNullException(nameof(assetInventory));
            }

            PrintQueueSelectionData printQueueSelectionData = configurationData.PrintQueues ?? new PrintQueueSelectionData();
            List<PrintQueueInfo> printQueues = new List<PrintQueueInfo>();

            foreach (LocalPrintQueueDefinition localQueue in printQueueSelectionData.SelectedPrintQueues.OfType<LocalPrintQueueDefinition>())
            {
                printQueues.Add(new LocalPrintQueueInfo(localQueue.QueueName, localQueue.AssociatedAssetId));
            }

            List<RemotePrintQueueInfo> allRemoteQueues = assetInventory.GetRemotePrintQueues().ToList();
            foreach (RemotePrintQueueDefinition remoteQueue in printQueueSelectionData.SelectedPrintQueues.OfType<RemotePrintQueueDefinition>())
            {
                printQueues.Add(allRemoteQueues.First(n => n.PrintQueueId == remoteQueue.PrintQueueId));
            }

            return new PrintQueueInfoCollection(printQueues);
        }
    }
}

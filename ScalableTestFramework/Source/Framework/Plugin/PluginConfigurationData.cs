using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Configuration data for an <see cref="IPluginConfigurationControl" />.
    /// </summary>
    public sealed class PluginConfigurationData : PluginData
    {
        /// <summary>
        /// Gets or sets the <see cref="AssetSelectionData" /> for this plugin configuration.
        /// </summary>
        public AssetSelectionData Assets { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DocumentSelectionData" /> for this plugin configuration.
        /// </summary>
        public DocumentSelectionData Documents { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ServerSelectionData" /> for this plugin configuration.
        /// </summary>
        public ServerSelectionData Servers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PrintQueueSelectionData" /> for this plugin configuration.
        /// </summary>
        public PrintQueueSelectionData PrintQueues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigurationData" /> class.
        /// </summary>
        /// <param name="metadata">An <see cref="XElement" /> containing plugin-specific XML metadata.</param>
        /// <param name="metadataVersion">The plugin-defined schema version of the XML metadata.</param>
        /// <exception cref="ArgumentNullException"><paramref name="metadata" /> is null.</exception>
        public PluginConfigurationData(XElement metadata, string metadataVersion)
            : base(metadata, metadataVersion)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigurationData" /> class.
        /// </summary>
        /// <param name="metadata">An object containing plugin-specific metadata.</param>
        /// <param name="metadataVersion">The plugin-defined schema version of the XML metadata.</param>
        /// <exception cref="ArgumentNullException"><paramref name="metadata" /> is null.</exception>
        /// <exception cref="InvalidDataContractException"><paramref name="metadata" /> is not a valid data contract.</exception>
        public PluginConfigurationData(object metadata, string metadataVersion)
            : base(metadata, metadataVersion)
        {
        }

        /// <summary>
        /// Writes the specified <see cref="PluginConfigurationData" /> to a file.
        /// </summary>
        /// <param name="configurationData">The <see cref="PluginConfigurationData" />.</param>
        /// <param name="fileName">The name of the file.</param>
        public static void WriteToFile(PluginConfigurationData configurationData, string fileName)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            List<string> lines = new List<string>
            {
                configurationData.GetMetadata().ToString(SaveOptions.DisableFormatting),
                configurationData.MetadataVersion,
                Serializer.Serialize(configurationData.Assets).ToString(SaveOptions.DisableFormatting),
                Serializer.Serialize(configurationData.Documents).ToString(SaveOptions.DisableFormatting),
                Serializer.Serialize(configurationData.Servers).ToString(SaveOptions.DisableFormatting),
                Serializer.Serialize(configurationData.PrintQueues).ToString(SaveOptions.DisableFormatting)
            };
            File.WriteAllLines(fileName, lines);
        }

        /// <summary>
        /// Loads the specified files into a <see cref="PluginConfigurationData" /> object.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>A <see cref="PluginConfigurationData" /> loaded from the file.</returns>
        public static PluginConfigurationData LoadFromFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            XElement metadata = XElement.Parse(lines[0]);
            string metadataVersion = lines[1];
            PluginConfigurationData data = new PluginConfigurationData(metadata, metadataVersion);

            if (lines.Length > 2)
            {
                data.Assets = Serializer.Deserialize<AssetSelectionData>(XElement.Parse(lines[2]));
            }

            if (lines.Length > 3)
            {
                data.Documents = Serializer.Deserialize<DocumentSelectionData>(XElement.Parse(lines[3]));
            }

            if (lines.Length > 4)
            {
                data.Servers = Serializer.Deserialize<ServerSelectionData>(XElement.Parse(lines[4]));
            }

            if (lines.Length > 5)
            {
                data.PrintQueues = Serializer.Deserialize<PrintQueueSelectionData>(XElement.Parse(lines[5]));
            }

            return data;
        }
    }
}

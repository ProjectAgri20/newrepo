using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Base class for data that both <see cref="PluginConfigurationData" /> and <see cref="PluginExecutionData" /> share.
    /// </summary>
    public abstract class PluginData
    {
        private readonly XElement _metadata;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _metadataVersion;

        /// <summary>
        /// Gets the schema version of the XML metadata.
        /// </summary>
        public string MetadataVersion => _metadataVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginData" /> class.
        /// </summary>
        /// <param name="metadata">An <see cref="XElement" /> containing plugin-specific XML metadata.</param>
        /// <param name="metadataVersion">The plugin-defined schema version of the XML metadata.</param>
        /// <exception cref="ArgumentNullException"><paramref name="metadata" /> is null.</exception>
        internal PluginData(XElement metadata, string metadataVersion)
        {
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            _metadataVersion = metadataVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginData" /> class.
        /// </summary>
        /// <param name="metadata">An object containing plugin-specific metadata.</param>
        /// <param name="metadataVersion">The plugin-defined schema version of the XML metadata.</param>
        /// <exception cref="ArgumentNullException"><paramref name="metadata" /> is null.</exception>
        /// <exception cref="InvalidDataContractException"><paramref name="metadata" /> is not a valid data contract.</exception>
        internal PluginData(object metadata, string metadataVersion)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            _metadata = Serializer.Serialize(metadata);
            _metadataVersion = metadataVersion;
        }

        /// <summary>
        /// Gets the plugin-specific XML metadata from this instance.
        /// </summary>
        /// <returns>The XML metadata.</returns>
        public XElement GetMetadata()
        {
            return _metadata;
        }

        /// <summary>
        /// Gets the object containing plugin-specific metadata from this instance.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <returns>An object containing plugin-specific metadata.</returns>
        /// <exception cref="InvalidCastException">The XML did not deserialize into an object of type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidDataContractException"><typeparamref name="T" /> is not a valid data contract.</exception>
        public T GetMetadata<T>()
        {
            return Serializer.Deserialize<T>(_metadata);
        }

        /// <summary>
        /// Gets the plugin-specific XML metadata from this instance, converting from old schemas if necessary.
        /// </summary>
        /// <param name="converters">A collection of <see cref="IPluginMetadataConverter" /> objects used to convert old versions of metadata.</param>
        /// <returns>The XML metadata.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="converters" /> is null.</exception>
        public XElement GetMetadata(IEnumerable<IPluginMetadataConverter> converters)
        {
            return ConvertMetadata(_metadata, _metadataVersion, converters);
        }

        /// <summary>
        /// Gets the object containing plugin-specific metadata from this instance, converting from old schemas if necessary.
        /// </summary>
        /// <param name="converters">A collection of <see cref="IPluginMetadataConverter" /> objects used to convert old versions of metadata.</param>
        /// <returns>An object containing plugin-specific metadata.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="converters" /> is null.</exception>
        /// <exception cref="InvalidCastException">The XML did not deserialize into an object of type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidDataContractException"><typeparamref name="T" /> is not a valid data contract.</exception>
        public T GetMetadata<T>(IEnumerable<IPluginMetadataConverter> converters)
        {
            XElement converted = ConvertMetadata(_metadata, _metadataVersion, converters);
            return Serializer.Deserialize<T>(converted);
        }

        private static XElement ConvertMetadata(XElement startMetadata, string startVersion, IEnumerable<IPluginMetadataConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException(nameof(converters));
            }

            XElement currentMetadata = startMetadata;
            string currentVersion = startVersion;
            List<string> encounteredVersions = new List<string> { startVersion };

            // Find a converter that matches the specified version
            IPluginMetadataConverter converter = converters.FirstOrDefault(n => string.Equals(currentVersion, n.OldVersion, StringComparison.OrdinalIgnoreCase));
            while (converter != null)
            {
                // Check the new version to make sure we don't convert to something we've already seen
                if (encounteredVersions.Contains(converter.NewVersion))
                {
                    return currentMetadata;
                }

                currentMetadata = converter.Convert(currentMetadata);
                currentVersion = converter.NewVersion;
                encounteredVersions.Add(converter.NewVersion);

                // Check to see if there is another converter to run
                converter = converters.FirstOrDefault(n => string.Equals(currentVersion, n.OldVersion));
            }

            // No more converters to run.  Return the end result.
            return currentMetadata;
        }
    }
}

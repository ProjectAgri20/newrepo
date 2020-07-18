using System.Xml.Linq;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Interface for a class that converts XML metadata to a new schema.
    /// </summary>
    public interface IPluginMetadataConverter
    {
        /// <summary>
        /// Gets the metadata schema version that this <see cref="IPluginMetadataConverter" /> is designed to process.
        /// </summary>
        string OldVersion { get; }

        /// <summary>
        /// Gets the metadata schema version that this <see cref="IPluginMetadataConverter" /> will produce.
        /// </summary>
        string NewVersion { get; }

        /// <summary>
        /// Converts the specified XML metadata from the old schema to the new schema.
        /// </summary>
        /// <param name="xml">The XML metadata in the schema version specified by <see cref="OldVersion" />.</param>
        /// <returns>XML metadata in the schema version specified by <see cref="NewVersion" />.</returns>
        XElement Convert(XElement xml);
    }
}

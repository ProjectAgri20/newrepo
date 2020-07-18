using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HP.ScalableTest;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Helper class to define data saving
    /// </summary>
    [Serializable]
    public class SaveData
    {
        /// <summary>
        /// Gets or sets the install data.
        /// </summary>
        /// <value>
        /// The install data.
        /// </value>
        public Collection<QueueInstallationData> InstallData { get; set; }

        /// <summary>
        /// Gets or sets the loaded packages.
        /// </summary>
        /// <value>
        /// The loaded packages.
        /// </value>
        public SerializableDictionary<string, PrintDeviceDriver> LoadedPackages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pre configure].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pre configure]; otherwise, <c>false</c>.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        public bool PreConfigure { get; set; }

        /// <summary>
        /// Gets or sets the pre configure text.
        /// </summary>
        /// <value>
        /// The pre configure text.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        public string PreConfigureText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveData"/> class.
        /// </summary>
        public SaveData()
        {
        }

        /// <summary>
        /// Saves to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Save(string path)
        {
            File.WriteAllText(path, LegacySerializer.SerializeXml(this).ToString());
        }
    }
}

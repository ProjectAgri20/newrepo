using System.Collections.Generic;
using System.IO;
using System.Linq;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest
{
    /// <summary>
    /// This is a container class for <see cref="DriverDetails"/> instances.  It 
    /// provides some general <seealso cref="T:System.Collections.Generic.IList`1"/> behaviors
    /// to help manage the list of drivers.
    /// </summary>
    public class PrintDeviceDriverCollection : List<DriverDetails>
    {
        /// <summary>
        /// Gets or sets the version for this print driver set.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets the index of the <see cref="HP.ScalableTest.Print.PrintDeviceDriverCollection"/> based on the provided criteria
        /// </summary>
        /// <param name="name">The name of the driver.</param>
        /// <param name="architecture">The applicable system architecture.</param>
        /// <param name="version">The version of the driver.</param>
        /// <param name="infFile">The INF file path for.</param>
        /// <returns></returns>
        public int IndexOf(string name, DriverArchitecture architecture, DriverVersion version, string infFile)
        {
            var enumeratedProperties = this.Select((item, i) => new { Items = item, Index = i });

            var entry =
                (
                    from p in enumeratedProperties
                    where p.Items.Architecture == architecture
                        && p.Items.Name.EqualsIgnoreCase(name)
                        && p.Items.Version.Equals(version)
                        && string.IsNullOrEmpty(p.Items.InfPath) == false
                        && Path.GetFileName(p.Items.InfPath).EqualsIgnoreCase(Path.GetFileName(infFile))
                    select p
                ).FirstOrDefault();

            return entry != null ? entry.Index : -1;
        }
    }
}

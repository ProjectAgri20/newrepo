using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using HP.ScalableTest.Print.Drivers;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// This class represents a print device driver used by a system to print to a designated print device.
    /// </summary>
    [DataContract]
    public class PrintDeviceDriver : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceDriver"/> class.
        /// </summary>
        public PrintDeviceDriver()
            : this(string.Empty, Guid.Empty, string.Empty)
        {
            PrintProcessor = "WinPrint";
            Name = "Undefined";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceDriver"/> class.
        /// </summary>
        /// <param name="name">The name of the driver.</param>
        /// <param name="id">The id.</param>
        /// <param name="printProcessor">Name of the print processor used by this print driver.</param>
        public PrintDeviceDriver(string name, Guid id, string printProcessor)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            PrintProcessor = printProcessor;
        }

        public PrintDeviceDriver(DriverDetails other)
        {
            Name = other.Name;
            Architecture = other.Architecture;
            Version = other.Version;
            InfPath = other.InfPath;
            Driver = other.DriverFile;
            ConfigurationFile = other.ConfigurationFile;
            HelpFile = other.HelpFile;
            DataFile = other.DataFile;
            PrintProcessor = other.PrintProcessor;
            Monitor = other.Monitor;
            Provider = other.Provider;
            DriverDate = other.DriverDate;
        }

        public DriverDetails CreateDetail()
        {
            return new DriverDetails
            {
                Name = this.Name,
                Architecture = this.Architecture,
                Version = this.Version,
                InfPath = this.InfPath,
                DriverFile = this.Driver,
                ConfigurationFile = this.ConfigurationFile,
                HelpFile = this.HelpFile,
                DataFile = this.DataFile,
                PrintProcessor = this.PrintProcessor,
                Monitor = this.Monitor,
                Provider = this.Provider,
                DriverDate = this.DriverDate
            };
        }

        /// <summary>
        /// Gets or sets the architecture such as x64 or x86 associated with this driver.
        /// </summary>
        [DataMember]
        public DriverArchitecture Architecture { get; set; }

        /// <summary>
        /// Gets or sets the name of this printer driver
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets the type of the driver for this instance.
        /// </summary>
        /// <remarks>
        /// Currently this takes the form of UPD, GPD or Discrete.
        /// </remarks>
        [IgnoreDataMember]
        public string DriverType
        {
            get
            {
                if (Name.StartsWith("HP Universal", StringComparison.CurrentCulture))
                {
                    return "UPD";
                }
                else if (Name.Contains("Global Print"))
                {
                    return "GPD";
                }
                else
                {
                    return "Discrete";
                }
            }
        }

        /// <summary>
        /// Gets the printer description language information per the INF file.
        /// </summary>
        [XmlIgnore]
        [IgnoreDataMember]
        public string VerifyPdl
        {
            get
            {
                Match match = Regex.Match(Name, @" (PCL\s*[0-9]+[0-9A-Za-z]*|PS)");
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the driver version per the INF file.
        /// </summary>
        [IgnoreDataMember]
        public string Release
        {
            get
            {
                Match match = Regex.Match(Name, @" \((v[0-9.]+)\)$");
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the driver INF file name.
        /// </summary>
        [DataMember]
        public string Driver { get; set; }

        /// <summary>
        /// Gets or sets the driver config file name.
        /// </summary>
        [DataMember]
        public string ConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets the driver help file name.
        /// </summary>
        [DataMember]
        public string HelpFile { get; set; }

        /// <summary>
        /// Gets or sets the driver version.
        /// </summary>
        [DataMember]
        public DriverVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the provider for the driver based on the INF file name.
        /// </summary>
        [DataMember]
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the version date for the driver.
        /// </summary>
        [DataMember]
        public string DriverDate { get; set; }

        /// <summary>
        /// Gets or sets the INF file path and name for the driver.
        /// </summary>
        [DataMember]
        public string InfPath { get; set; }

        /// <summary>
        /// Gets the directory location for the driver.
        /// </summary>
        [IgnoreDataMember]
        public string Location
        {
            get
            {
                return string.IsNullOrEmpty(InfPath) ? string.Empty : Path.GetDirectoryName(InfPath);
            }
        }

        /// <summary>
        /// Gets or sets the language monitor for the driver.
        /// </summary>
        [DataMember]
        public string Monitor { get; set; }

        /// <summary>
        /// Gets or sets the driver data file from the INF file.
        /// </summary>
        [DataMember]
        public string DataFile { get; set; }

        /// <summary>
        /// Gets or sets the print processor value for this driver.
        /// </summary>
        [DataMember]
        public string PrintProcessor { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }

            PrintDeviceDriver properties = obj as PrintDeviceDriver;

            return
                properties.Name.Equals(Name, StringComparison.OrdinalIgnoreCase)
                    && properties.Architecture == Architecture
                    && properties.Version.Equals(Version)
                    && properties.InfPath.Equals(InfPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var property in this.GetType().GetProperties())
            {
                builder.Append("[{0}: {1}] ".FormatWith(property.Name, property.GetValue(this, null)));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            string data = this.Name + this.Architecture.ToString() + Path.GetFileName(this.InfPath);
            return data.GetHashCode();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            PrintDeviceDriver item = obj as PrintDeviceDriver;

            string left = this.Name + this.Architecture.ToString() + this.InfPath + this.Version.ToString();
            string right = item.Name + item.Architecture.ToString() + item.InfPath + item.Version.ToString();

            return string.Compare(left, right, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="driver1">The left side properties.</param>
        /// <param name="driver2">The right side properties.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(PrintDeviceDriver driver1, PrintDeviceDriver driver2)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(driver1, driver2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)driver1 == null) || ((object)driver2 == null))
            {
                return false;
            }

            return driver1.Equals(driver2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="driver1">The left side properties.</param>
        /// <param name="driver2">The right side properties.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(PrintDeviceDriver driver1, PrintDeviceDriver driver2)
        {
            return !(driver1 == driver2);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="properties1">The left side properties.</param>
        /// <param name="properties2">The right side properties.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(PrintDeviceDriver properties1, PrintDeviceDriver properties2)
        {
            if (properties1 == null)
            {
                throw new ArgumentNullException("properties1");
            }

            return (properties1.CompareTo(properties2) < 0);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="driver1">The left side properties.</param>
        /// <param name="driver2">The right side properties.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(PrintDeviceDriver driver1, PrintDeviceDriver driver2)
        {
            if (driver1 == null)
            {
                throw new ArgumentNullException(nameof(driver1));
            }

            return (driver1.CompareTo(driver2) > 0);
        }
    }
}
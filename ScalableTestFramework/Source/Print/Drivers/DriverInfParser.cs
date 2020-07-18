using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Print.Drivers
{
    /// <summary>
    /// Parses data from print driver INF files.
    /// </summary>
    public sealed class DriverInfParser
    {
        private readonly DriverInfReader _reader;
        private Dictionary<string, string> _variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverInfParser" /> class.
        /// </summary>
        /// <param name="reader">The <see cref="DriverInfReader" /> to get data from.</param>
        public DriverInfParser(DriverInfReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Builds a <see cref="DriverInf" /> object from the specified INF file.
        /// </summary>
        /// <returns>A <see cref="DriverInf" /> object.</returns>
        public DriverInf BuildInf()
        {
            DriverInf inf = new DriverInf(_reader.FileLocation);
            inf.DriverClass = GetClass();

            if (inf.DriverClass == "Printer")
            {
                foreach (DriverDetails driver in GetAvailableDrivers())
                {
                    inf.Drivers.Add(driver);
                }
            }

            return inf;
        }

        /// <summary>
        /// Gets the class type of the INF file.
        /// </summary>
        /// <returns>A string representation of the class type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        public string GetClass()
        {
            string infClass = _reader.GetSection("Version").FirstOrDefault(n => Regex.IsMatch(n, @"Class\s*="));
            return infClass?.Split('=').Skip(1).FirstOrDefault()?.Trim();
        }

        /// <summary>
        /// Gets a list of all available <see cref="DriverDetails" /> from this INF.
        /// </summary>
        /// <returns>A collection of <see cref="DriverDetails" /> from the INF section.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        public IEnumerable<DriverDetails> GetAvailableDrivers()
        {
            List<DriverDetails> availableDrivers = new List<DriverDetails>();

            // First, figure out what versions of the driver are available.
            foreach (string line in _reader.GetSection("Manufacturer"))
            {
                string cleanLine = ExpandVariables(line);

                // We only care what's on the right side of the equal sign.
                string versionLine = cleanLine.Split('=')[1];

                // The syntax of the right-side of the equal sign is "model-section-name[,TargetOSVersion][,TargetOSVersion]..."
                string[] versions = versionLine.Split(',');

                // The first index is our model-section-name.
                string modelName = versions[0].Trim();

                // Get all drivers in the model section.
                List<DriverDetails> topDrivers = GetDrivers(modelName).ToList();
                topDrivers.ForEach(n => n.Architecture = DriverArchitecture.NTx86);
                availableDrivers.AddRange(topDrivers);

                // Parse each of the subsections.
                foreach (string version in versions.Skip(1).Select(n => n.Trim()))
                {
                    List<DriverDetails> drivers = GetDrivers($"{modelName}.{version}").ToList();
                    DriverArchitecture architecture = GetArchitecture(version);
                    drivers.ForEach(n => n.Architecture = architecture);
                    availableDrivers.AddRange(drivers);
                }
            }

            // INF files could contain x86 and x64 entries, however with print drivers, the INF file can only support
            // one or the other.  When investigating some UPD drivers, I discovered that on x64 INF files, an entry
            // exists for x86 (and internally, they redirect the INF to install the x64-version of the driver).
            // For this reason, if we see any drivers that support x64, remove the x86 entries.
            if (availableDrivers.Any(n => n.Architecture == DriverArchitecture.NTAMD64) && availableDrivers.Any(n => n.Architecture == DriverArchitecture.NTx86))
            {
                DriverArchitecture current = Environment.Is64BitOperatingSystem ? DriverArchitecture.NTAMD64 : DriverArchitecture.NTx86;
                availableDrivers.RemoveAll(n => n.Architecture != current);
            }

            return availableDrivers;
        }

        private static DriverArchitecture GetArchitecture(string version)
        {
            // The format of this is: NT[Architecture][.[OSMajorVersion][.[OSMinorVersion][.[ProductType][.SuiteMask]]]]
            // Architecture: x86, AMD64
            // OSMajorVersion: 5 = XP, 6 = Vista/Win7
            // OSMinorVersion: 1 (5.1 = XP, 6.1 = Win7)
            // ProductType:    Not relevant.
            // SuiteMask:      Not relevant.

            // Check to see if the first is the architecture.
            if (Enum.TryParse(version.Split('.')[0], true, out DriverArchitecture architecture))
            {
                // We've got our architecture!
                return architecture;
            }
            else
            {
                return DriverArchitecture.NTx86;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="DriverDetails" /> from the specified INF section.
        /// </summary>
        /// <param name="sectionName">The INF section name.</param>
        /// <returns>A collection of <see cref="DriverDetails" /> from the INF section.</returns>
        public IEnumerable<DriverDetails> GetDrivers(string sectionName)
        {
            List<DriverDetails> drivers = new List<DriverDetails>();

            foreach (string line in _reader.GetSection(sectionName))
            {
                // Each line should be formatted as "driverName=installRule,hardwareID".
                // We only care about getting the driver name.
                string[] parts = line.Split('=');
                string driverName = ExpandVariables(parts[0]).Trim().Trim('"');
                string driverSectionName = parts[1].Split(',')[0].Trim();
                if (!drivers.Any(n => n.Name == driverName))
                {
                    DriverDetails driver = new DriverDetails();
                    driver.Name = driverName;
                    driver.InfPath = _reader.FileLocation;
                    GetDriverProperties(driver, driverSectionName);
                    GetDriverProperties(driver, "Version");
                    drivers.Add(driver);
                }
            }
            return drivers;
        }

        private void GetDriverProperties(DriverDetails driver, string driverSectionName)
        {
            foreach (string dirtyLine in _reader.GetSection(driverSectionName))
            {
                string line = ExpandVariables(dirtyLine);
                string[] parts = line.Split('=');
                string propertyName = parts[0].Trim();
                string propertyValue = parts[1].Trim();
                switch (propertyName)
                {
                    case "PrintProcessor":
                        // The format of the line will be something like...
                        // PrintProcessor="HPCPP15,hpcpp15.dll".  We need to extract
                        // the "HPCPP15" part.
                        driver.PrintProcessor = propertyValue.Trim('"').Split(',')[0];
                        break;

                    case "DriverFile":
                        driver.DriverFile = propertyValue;
                        break;

                    case "ConfigFile":
                        driver.ConfigurationFile = propertyValue;
                        break;

                    case "HelpFile":
                        driver.HelpFile = propertyValue;
                        break;

                    case "LanguageMonitor":
                        driver.Monitor = propertyValue.Trim('"').Split(',')[0];
                        break;

                    case "DataFile":
                        driver.DataFile = propertyValue;
                        break;

                    case "DriverVer":
                        string[] versionParts = propertyValue.Split(',');
                        driver.DriverDate = versionParts[0];
                        driver.Version = new DriverVersion(versionParts[1]);
                        break;

                    case "Provider":
                        driver.Provider = propertyValue.Trim().Trim('"');
                        break;

                    case "DataSection":
                        // This is for Xerox inf files, they have another section
                        // that provides this information.
                        GetDriverProperties(driver, propertyValue);
                        break;

                    default:
                        // Default is intentionally left blank.
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the name of the discrete driver.
        /// </summary>
        /// <returns>The discrete driver name.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        public string GetDiscreteDriverName()
        {
            string versionLine = _reader.GetSection("Manufacturer").First().Split('=')[1];
            string[] versions = versionLine.Split(',');
            string modelName = versions[0].Trim();

            for (int i = 1; i < versions.Length; i++)
            {
                string version = versions[i].Trim();
                var sectionList = _reader.GetSection($"{modelName}.{version}");
                if (sectionList.Any())
                {
                    string printDriverName = sectionList.First().Split('=')[0];
                    printDriverName = ExpandVariables(printDriverName);
                    return printDriverName.Replace("\"", null).TrimEnd();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the specified string variable.
        /// </summary>
        /// <param name="variableName">The string variable name.</param>
        /// <returns>The string variable value.</returns>
        public string GetVariable(string variableName)
        {
            if (_variables == null)
            {
                _variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string line in _reader.GetSection("Strings"))
                {
                    string[] parts = line.Split('=');
                    string leftSide = parts[0].Trim();
                    string rightSide = parts[1].Trim();

                    _variables.Add(leftSide, rightSide);
                }
            }

            return _variables[variableName];
        }

        /// <summary>
        /// Expands all string variables in the specified line.
        /// </summary>
        /// <param name="line">The line with variables to expand.</param>
        /// <returns>The line with all variables expanded.</returns>
        public string ExpandVariables(string line)
        {
            string returnLine = line;
            Match match;
            do
            {
                match = Regex.Match(returnLine, "%([^%]+)%");
                if (match.Success)
                {
                    string variableName = match.Groups[1].Value;
                    string variableValue = GetVariable(variableName);
                    returnLine = returnLine.Replace($"%{variableName}%", variableValue);
                }
            } while (match.Success);

            return returnLine;
        }
    }
}

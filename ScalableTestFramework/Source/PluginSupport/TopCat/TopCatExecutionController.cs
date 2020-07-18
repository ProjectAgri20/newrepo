using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;
using Microsoft.Win32;

namespace HP.ScalableTest.PluginSupport.TopCat
{
    /// <summary>
    /// Executes TopCat scripts and retrieves the results.
    /// </summary>
    public class TopCatExecutionController
    {
        private const string _scriptRoot = @"C:\VirtualResource\TopCatScripts";
        private const string _topcatDirectory = @"C:\VirtualResource\TopCat";
        private static readonly object _lock = new object();

        private TopCatScript _script;
        private string _destinationScriptDirectoryName;
        private string _topcatPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopCatExecutionController" /> class.
        /// </summary>
        /// <param name="script">The <see cref="TopCatScript" /> object containing details of the script to execute.</param>
        public TopCatExecutionController(TopCatScript script)
        {
            _script = script;
            _destinationScriptDirectoryName = Path.Combine(_scriptRoot, Directory.GetParent(_script.FileName).Name);
            _topcatPath = Path.Combine(_topcatDirectory, Environment.Is64BitOperatingSystem ? "x64\\topcat.exe" : "x86\\topcat.exe");
        }

        /// <summary>
        /// Installs the prerequisites for TopCat execution from the specified directory.
        /// </summary>
        /// <param name="topCatSetupDirectory">The top cat setup directory.</param>
        public void InstallPrerequisites(string topCatSetupDirectory)
        {
            Directory.CreateDirectory(_scriptRoot);

            string srcDirName = Path.GetDirectoryName(_script.FileName);

            //now copy the content of the directory to the destination, this will ensure all dependencies are also copied
            lock (_lock)
            {
                if (!Directory.Exists(_destinationScriptDirectoryName))
                {
                    FileSystem.CopyDirectory(srcDirName, _destinationScriptDirectoryName);
                }

                if (!Directory.Exists(_topcatDirectory))
                {
                    FileSystem.CopyDirectory(topCatSetupDirectory, _topcatDirectory);
                }
            }
        }

        /// <summary>
        /// Executes the TopCat test.
        /// </summary>
        public void ExecuteTopCatTest()
        {
            if (!File.Exists(_topcatPath))
            {
                Logger.LogInfo("TopCat binary not found. Returning without execution");
                return;
            }

            if (_script.SelectedTests.Count == 0)
            {
                ExecuteTopCatTestCase(Path.Combine(_destinationScriptDirectoryName, Path.GetFileName(_script.FileName)));
            }
            else
            {
                ExtractandExecuteTestCases();
            }
        }

        private void ExecuteTopCatTestCase(string scriptFileName)
        {
            StringBuilder arguments = new StringBuilder();
            arguments.Append("\"");
            arguments.Append(scriptFileName);
            arguments.AppendFormat("\" /property:{0},{1}", "TestPath", _destinationScriptDirectoryName);
            arguments.AppendFormat(" /property:{0},{1}", "TopCatPath", Path.GetDirectoryName(_topcatPath));
            foreach (var property in _script.Properties)
            {
                if (property.Key.Equals("TopCatPath"))
                {
                    continue;
                }

                arguments.AppendFormat(" /property:{0},\"{1}\"", property.Key, property.Value);
            }

            ProcessUtil.Execute(_topcatPath, arguments.ToString(), TimeSpan.FromMinutes(10));
        }

        private void ExtractandExecuteTestCases()
        {
            XDocument xDoc = XDocument.Load(_script.FileName);
            XDocument clonedDoc = new XDocument(xDoc);
            XNamespace xNS = clonedDoc.Root.GetNamespaceOfPrefix("tcx");
            var tests = clonedDoc.Descendants(xNS + "Tests").First();
            tests.RemoveNodes();
            foreach (var testCaseName in _script.SelectedTests)
            {
                tests.Add(from p in xDoc.Descendants(xNS + "TestCase") where p.FirstAttribute.Value.Equals(testCaseName) select p);
            }

            string destinationScriptFileName = Path.Combine(_destinationScriptDirectoryName, Path.GetFileName(_script.FileName));
            clonedDoc.Save(destinationScriptFileName);

            ExecuteTopCatTestCase(destinationScriptFileName);
        }

        /// <summary>
        /// Gets the result file path.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The result file path.</returns>
        public string GetResultFilePath(string userName)
        {
            string localScriptFile = Path.Combine(_destinationScriptDirectoryName, Path.GetFileName(_script.FileName));

            XDocument xDoc = XDocument.Load(localScriptFile);

            XNamespace ns = xDoc.Root.GetNamespaceOfPrefix("vmps");

            string tempResult = xDoc.Descendants(ns + "ResultsFile").First().Value;

            string propertyName = tempResult.Substring(0, tempResult.IndexOf("\\", StringComparison.OrdinalIgnoreCase));

            //check if the path entered is hardcoded value
            if (Directory.Exists(propertyName))
            {
                propertyName = Path.GetTempPath();
                tempResult = Path.Combine(propertyName, tempResult.Substring(tempResult.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1));
                return tempResult;
            }
            else
            {
                var matchingProperties = _script.Properties.Where(x => x.Key.Equals(propertyName)).ToList();
                if (!matchingProperties.Any())
                {
                    string tempAdminPath = Path.GetTempPath();

                    tempResult = Path.Combine(tempAdminPath, tempResult.Substring(tempResult.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1));
                    if (!File.Exists(tempResult))
                    {
                        tempAdminPath = GetProfileTempPath(userName);
                        tempResult = Path.Combine(tempAdminPath, tempResult.Substring(tempResult.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1));
                    }

                    return tempResult;
                }
                else
                {
                    if (Directory.Exists(matchingProperties.First().Value))
                    {
                        tempResult = Path.Combine(matchingProperties.First().Value, tempResult.Substring(tempResult.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1));
                    }
                    else
                    {
                        tempResult = string.Empty;
                    }

                    return tempResult;
                }
            }
        }

        private static string GetProfileTempPath(string userName)
        {
            // Note that this is a bit risky as it doesn't take into consideration roaming profiles, but
            // for the needs we have it should be ok.
            string value = string.Empty;

            // Look through the profile list for the existence of the requested user
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\"))
            {
                foreach (string profileKey in key.GetSubKeyNames())
                {
                    var userValue = key.OpenSubKey(profileKey).GetValue("ProfileImagePath").ToString();

                    // If you find the profile for the requested user, then go get its Environment information
                    if (userValue.EndsWith(userName, StringComparison.OrdinalIgnoreCase))
                    {
                        using (RegistryKey tempPathKey = Registry.Users.OpenSubKey($@"{profileKey}\Environment\"))
                        {
                            // If you find the environment information, then build the temp file path from the 
                            // user value and the TEMP value.  Otherwise return an empty string.
                            if (tempPathKey != null)
                            {
                                var option = RegistryValueOptions.DoNotExpandEnvironmentNames;
                                var tempPath = tempPathKey.GetValue("TEMP", string.Empty, option).ToString().Replace("%USERPROFILE%", userValue);
                                value = Directory.Exists(tempPath) ? tempPath : string.Empty;
                                break;
                            }
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Cleans up and removes the local TopCat directory.
        /// </summary>
        public void Cleanup()
        {
            lock (_lock)
            {
                if (Directory.Exists(_destinationScriptDirectoryName))
                {
                    Directory.Delete(_destinationScriptDirectoryName, true);
                }

                if (Directory.Exists(_topcatDirectory))
                {
                    Directory.Delete(_topcatDirectory, true);
                }
            }
        }
    }
}

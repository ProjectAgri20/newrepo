using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CreateInstaller
{
    /// <summary>
    /// Class to encapulate the building of an Inno Installer file.
    /// </summary>
    internal class Installer : IDisposable, IProcessOutput
    {
        private bool _processing = true;
        private Process _process = null;
        private readonly string _root;
        private readonly string _installerType;
        private readonly string _version;
        private readonly string _configuration;

        public event EventHandler<InstallEventArgs> OnMessageUpdate;

        /// <summary>
        /// Creates a new instance of the Installer class.
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="installerType"></param>
        /// <param name="version"></param>
        /// <param name="configuration"></param>
        public Installer(string rootPath, string installerType, string version, string configuration)
        {
            _root = rootPath;
            _installerType = installerType;
            _version = version;
            _configuration = configuration;
        }

        /// <summary>
        /// Returns whether this instance is processing a build.
        /// </summary>
        public bool Processing
        {
            get { return _processing; }
        }

        /// <summary>
        /// Returns a description of the Installer Type setting.
        /// </summary>
        public string Label { get { return $"STB {_installerType} Installer Creation"; } }

        /// <summary>
        /// Returns a configuration description suited for a file name.
        /// </summary>
        public string Configuration { get { return $"STB{_installerType}-{_configuration}-{_version}"; } }

        /// <summary>
        /// Cancels the installer build process.
        /// </summary>
        public void Cancel()
        {
            if (_process != null && !_process.HasExited)
            {
                _process.OutputDataReceived -= Create_OutputDataReceived;
                _process.ErrorDataReceived -= Create_ErrorDataReceived;
                _process.Kill();
                _processing = false;
            }
        }

        /// <summary>
        /// Executes the installer build process.
        /// </summary>
        public void Execute()
        {
            //var root = @"C:\STB Deployment";
            //var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var root = _root;
            string configDirectory = Path.Combine(root, string.Format(@"Configuration\{0}", _configuration));
            if (!Directory.Exists(configDirectory))
            {
                OnMessageUpdate(this, new InstallEventArgs(string.Format("Configuration directory missing: {0}", configDirectory)));
                return;
            }

            string source = Path.Combine(root, string.Format(@"Setup\InstallerScripts\{0}SetupMain.iss", _installerType));
            if (!File.Exists(source))
            {
                OnMessageUpdate(this, new InstallEventArgs("Master installer script is missing. It should be in Setup\\InstallerScripts"));
                return;
            }

            string destination = Path.Combine(root, string.Format(@"Setup\InstallerScripts\{0}SetupMain-{1}-{2}.iss", _installerType, _configuration, _version));
            File.Copy(source, destination, true);

            // Load up all the plugins that should be included in this distribution
            Collection<string> plugins = new Collection<string>();
            var lines = File.ReadAllLines(Path.Combine(root, string.Format(@"Configuration\{0}\PluginList.txt", _configuration)));
            for (int i = 0; i < lines.Count(); i++)
            {
                if (!string.IsNullOrEmpty(lines[i]) && !lines[i].StartsWith("//") && !lines[i].StartsWith("*"))
                {
                    var items = lines[i].Split(',');
                    plugins.Add(items[0]);
                }
            }

            // Read every DLL file in the plugins directory.  If it doesn't start
            // with "plugin." then just add it.  Otherwise check the list to see
            // if it should be included.
            StringBuilder builder = new StringBuilder();
            string path = Path.Combine(root, @"Binaries\Plugin");

            if (!Directory.Exists(path))
            {
                OnMessageUpdate(this, new InstallEventArgs(string.Format("Binaries\\Plugin directory missing.")));
                return;
            }

            foreach (var file in Directory.GetFiles(path, "*.dll"))
            {
                var fileName = Path.GetFileName(file);
                if (!fileName.StartsWith("plugin.", StringComparison.OrdinalIgnoreCase))
                {
                    builder.AppendLine(Resource.NonPluginDLL.Replace("{NAME}", fileName));
                }
                else
                {
                    var plugin = Regex.Match(fileName, @"plugin\.(\S+)\.dll", RegexOptions.IgnoreCase).Groups[1].Value;
                    if (plugins.Contains(plugin))
                    {
                        builder.AppendLine(Resource.PluginDLL.Replace("{NAME}", plugin));
                    }
                }
            }

            string fileContents = File.ReadAllText(destination);
            File.WriteAllText(destination, fileContents.Replace(";INSERTPLUGINSHERE", builder.ToString()));

            try
            {
                string outputPath = Path.Combine(root, "Installers");
                string packageVersion = string.Format("{0}-{1}", _configuration, _version);
                string outputFile = string.Format("STB{0}-{1}", _installerType, packageVersion);
                string createArgs = string.Format(Resource.CreateServerArgs, outputPath, outputFile, packageVersion, _configuration, destination);

                string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                if (!Environment.Is64BitOperatingSystem)
                {
                    programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }

                _process = new Process();
                _process.StartInfo.FileName = Path.Combine(programFiles, @"Inno Setup 5\iscc.exe");
                _process.StartInfo.Arguments = createArgs;
                _process.StartInfo.UseShellExecute = false;
                _process.StartInfo.CreateNoWindow = true;
                _process.StartInfo.RedirectStandardOutput = true;
                _process.StartInfo.RedirectStandardError = true;
                _process.OutputDataReceived += Create_OutputDataReceived;
                _process.ErrorDataReceived += Create_ErrorDataReceived;
                _process.Start();
                _process.BeginErrorReadLine();
                _process.BeginOutputReadLine();
                _process.WaitForExit();
            }
            catch (Exception ex)
            {
                OnMessageUpdate(this, new InstallEventArgs(ex.ToString()));
            }

            _processing = false;
        }

        private void Create_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            OnMessageUpdate(sender, new InstallEventArgs(e.Data));
        }

        private void Create_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OnMessageUpdate(sender, new InstallEventArgs(e.Data));
        }

        /// <summary>
        /// Release resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            if (_process != null)
            {
                _process.Dispose();
            }
        }

        /// <summary>
        /// Gets the label for this package.  If there are existing installers, returns the label from the first one it finds.
        /// If no installers exist, Generates a default label based on today.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static string GetPackageLabel(string workingPath)
        {
            string fileName = Directory.GetFiles(Path.Combine(workingPath, "Installers")).FirstOrDefault();
            if (!string.IsNullOrEmpty(fileName))
            {
                //Found an existing build. Parse and use it's label.
                string[] parts = fileName.Split('-');
                if (parts.Length >= 2)
                {
                    return parts[2].Substring(0, parts[2].Length - 4);
                }
            }

            return GeneratePackageLabel(workingPath);

        }

        /// <summary>
        /// Generates a default label based on today.
        /// </summary>
        /// <param name="workingPath"></param>
        /// <returns></returns>
        private static string GeneratePackageLabel(string workingPath)
        {
            string binFile = Path.Combine(workingPath, @"Binaries\STB User Console\SolutionTestBench.exe");
            Version version = AssemblyName.GetAssemblyName(binFile).Version;
            string date = DateTime.Now.ToString("yyyyMMdd");

            return string.Format(@"{0}_{1}", version.ToString(), date);
        }

        /// <summary>
        /// Writes log data to a file.
        /// </summary>
        /// <param name="workingPath"></param>
        /// <param name="fileName"></param>
        /// <param name="logText"></param>
        public static void WriteLogToFile(string workingPath, string fileName, string logText)
        {
            //Assembly exe = Assembly.GetExecutingAssembly();

            StringBuilder filePath = new StringBuilder(workingPath);
            filePath.Append("\\");
            filePath.Append(fileName);
            filePath.Append(".log");

            File.WriteAllText(filePath.ToString(), logText);
        }
    }

}

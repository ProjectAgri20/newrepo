using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using HP.ScalableTest;

namespace HP.ScalableTest.Utility.VisualStudio
{
    public class DependencyGraphBuilder : IDisposable
    {
        private Dictionary<string, Collection<string>> _dependencies = new Dictionary<string, Collection<string>>();
        private Collection<string> _processedList = new Collection<string>();
        private Dictionary<string, string> _assemblyNamesByFile = new Dictionary<string, string>();
        private Dictionary<string, int> _colorWheel = new Dictionary<string, int>();
        private string _localAppDirectory = string.Empty;

        public event EventHandler<StringEventArgs> OnProcessingProject;
        public event EventHandler<StringEventArgs> OnUpdateStatus;
        public event EventHandler<StringEventArgs> OnGraphCreated;
        public event EventHandler<BoolEventArgs> OnUpdateProcessing;

        public List<string> ProjectList { get; private set; }
        public List<string> ShownProjectList { get; private set; }
        public List<string> AssemblyList
        {
            get
            {
                List<string> assemblies = new List<string>();

                foreach (string project in _dependencies.Keys)
                {
                    IEnumerable<string> dependOnProject = _dependencies.Keys.Where(n => _dependencies[n].Contains(project));
                    if (dependOnProject.Count() > 0)
                    {
                        assemblies.Add(project);
                    }
                }

                return assemblies;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyGraphBuilder"/> class.
        /// </summary>
        public DependencyGraphBuilder()
        {
            _localAppDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CSProjectDependency");
            ProjectList = new List<string>();
            ShownProjectList = new List<string>();
        }

        /// <summary>
        /// Loads the depedency graph starting at the specified root.
        /// </summary>
        /// <param name="root">The root.</param>
        public void Load(string root)
        {
            _dependencies.Clear();
            _processedList.Clear();

            FireOnUpdateStatus("Reading project files...");

            // Recursively work down through the directory hierarchy
            ReadDirectory(root);

            // Build the GraphViz command file and generate graph
            BuildGraph(_dependencies);
        }

        public void Filter(string baseProject, bool recursive)
        {
            Dictionary<string, Collection<string>> filteredDependencies = new Dictionary<string,Collection<string>>();
            ShownProjectList.Clear();

            // Add to the dictionary all projects that are upstream of our base project
            Queue<string> includedProjects = new Queue<string>();            
            includedProjects.Enqueue(baseProject);
            while (includedProjects.Count > 0)
            {
                // Add the node we're looking at to the included dependency dictionary
                string currentProject = includedProjects.Dequeue();
                if (!filteredDependencies.ContainsKey(currentProject))
                {
                    filteredDependencies.Add(currentProject, null);
                    ShownProjectList.Add(currentProject);
                }

                // Add all nodes that reference our current node to the queue
                if (recursive || baseProject == currentProject)
                {
                    foreach (string key in _dependencies.Keys)
                    {
                        if (_dependencies[key].Contains(currentProject) && !filteredDependencies.ContainsKey(key))
                        {
                            includedProjects.Enqueue(key);
                        }
                    }
                }
            }

            // Add to the dictionary all project dependencies that exist between included projects
            foreach (string key in filteredDependencies.Keys.ToList())
            {
                IEnumerable<string> allDependencies = _dependencies[key];
                IEnumerable<string> wantedDependencies = filteredDependencies.Keys;
                IEnumerable<string> includedDependencies = allDependencies.Intersect(wantedDependencies);
                filteredDependencies[key] = new Collection<string>(includedDependencies.ToList());
            }

            BuildGraph(filteredDependencies);
        }

        private void BuildGraph(Dictionary<string, Collection<string>> dependencies)
        {
            // Build the GraphViz command file
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph g {");
            dot.AppendLine("rankdir = LR;");
            dot.AppendLine("shape = box;");
            dot.AppendLine("bgcolor = white;");
            //dot.AppendLine("nodesep = .25;");
            dot.AppendLine("node [color=black,fontname=Arial,fontsize=12,fontcolor=black,shape=box,fill=white];");
            dot.AppendLine("edge [color=Blue, style=solid]");
            dot.AppendLine("rankdir = LR");

            int colorCount = 0;
            foreach (string key in dependencies.Keys)
            {
                string assembly = key.Replace(".", string.Empty);
                foreach (string dependency in dependencies[key])
                {
                    string dependent = dependency.Replace(".", string.Empty);
                    if (!_colorWheel.ContainsKey(dependency))
                    {
                        _colorWheel.Add(dependency, colorCount++);
                    }

                    string colorName = ColorWheel.Select(_colorWheel[dependency]);
                    dot.Append(string.Format("{0} -> {1} [color={2}];", assembly, dependent, colorName));
                    dot.Append(Environment.NewLine);
                }
            }
            dot.Append("}");
            dot.Append(Environment.NewLine);

            // Save the GraphViz command file to the local user's app directory
            if (!Directory.Exists(_localAppDirectory))
            {
                Directory.CreateDirectory(_localAppDirectory);
            }

            string dotFile = Path.Combine(_localAppDirectory, Path.GetRandomFileName());
            File.WriteAllText(dotFile, dot.ToString());
         
            string dotPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "GraphViz");
            string dotExe = Path.Combine(dotPath, "dot.exe");

            FireOnUpdateStatus("Creating dependency graph");
            FireOnUpdateProcessing(true);

            using (ProcessEx processEx = new ProcessEx(dotExe, string.Format("-Tbmp -O {0}", dotFile), new TimeSpan(0, 0, 30)))
            {
                processEx.StartInfo.UseShellExecute = false;
                processEx.WaitForCmdExit = true;
                processEx.Execute();

                string graphFile = dotFile + ".bmp";
                if (OnGraphCreated != null && File.Exists(graphFile))
                {
                    OnGraphCreated(this, new StringEventArgs(graphFile));
                }
            }

            FireOnUpdateStatus("Complete");
            FireOnUpdateProcessing(false);
        }

        /// <summary>
        /// Reads the directory and processes project files.
        /// </summary>
        /// <param name="projectFilePath">The project file path.</param>
        private void ReadDirectory(string projectFilePath)
        {
            if (!Directory.Exists(projectFilePath))
            {
                FireOnProcessingProject("    -> " + projectFilePath + " DOES NOT EXIST");
                return;
            }

            // Clean up the project file path in case it contains relative elements
            string projectFileDir = new DirectoryInfo(projectFilePath).FullName;

            foreach (string projectFile in Directory.GetFiles(projectFileDir, "*.csproj"))
            {
                ProcessProjectFile(projectFile);
                ProjectList.Add(projectFile);
            }

            foreach (string directory in Directory.GetDirectories(projectFileDir))
            {
                if (!directory.Contains(".svn"))
                {
                    //FireOnProcessingProject("Scanning  " + directory);
                    ReadDirectory(directory);
                }
            }
        }

        /// <summary>
        /// Processes the project file.
        /// </summary>
        /// <param name="projectFileName">Name of the project file.</param>
        private void ProcessProjectFile(string projectFileName)
        {
            if (!File.Exists(projectFileName))
            {
                FireOnProcessingProject("Resource not found: " + projectFileName);
                return;
            }

            // Skip those already processed
            if (_processedList.Contains(projectFileName))
            {
                //OnProcessingProject("    -> Processed " + projectFileName);
                return;
            }

            string projectFilePath = Path.GetDirectoryName(projectFileName);

            _processedList.Add(projectFileName);

            FireOnProcessingProject("Processing " + projectFileName);

            XmlDocument projectXml = new XmlDocument();
            projectXml.Load(projectFileName);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(projectXml.NameTable);
            namespaceManager.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
            string assemblyName = projectXml.DocumentElement.SelectSingleNode("//ns:AssemblyName", namespaceManager).InnerText;

            // Add this assembly name to a dictionary keyed off the file name
            if (!_assemblyNamesByFile.ContainsKey(projectFileName))
            {
                _assemblyNamesByFile.Add(projectFileName, assemblyName);
            }

            if (!_dependencies.ContainsKey(assemblyName))
            {
                _dependencies.Add(assemblyName, new Collection<string>());
            }

            foreach (XmlNode node in projectXml.SelectNodes("//ns:ProjectReference", namespaceManager))
            {
                // In order to ensure the names align up we have to forward look at the project file
                // and extract the assembly name out of it.
                string referenceFile = node.Attributes["Include"].Value;
                string referencePath = Path.Combine(projectFilePath, referenceFile);
                var referencePathInfo = new DirectoryInfo(referencePath);

                string referenceAssemblyName = string.Empty;
                if (_assemblyNamesByFile.ContainsKey(referencePathInfo.FullName))
                {
                    referenceAssemblyName = _assemblyNamesByFile[referencePathInfo.FullName];
                }
                else if (File.Exists(referencePathInfo.FullName))
                {
                    XmlDocument referenceXml = new XmlDocument();
                    referenceXml.Load(referencePathInfo.FullName);
                    referenceAssemblyName = referenceXml.SelectSingleNode("//ns:AssemblyName", namespaceManager).InnerText;
                }

                if (!string.IsNullOrEmpty(referenceAssemblyName) && !_dependencies[assemblyName].Contains(referenceAssemblyName))
                {
                    _dependencies[assemblyName].Add(referenceAssemblyName);

                    // Now move onto this reference and process its contents
                    ProcessProjectFile(referencePathInfo.FullName);
                }
            }
        }

        /// <summary>
        /// Cleans up this instance.
        /// </summary>
        public void Cleanup()
        {
            try
            {
                foreach (string file in Directory.GetFiles(_localAppDirectory))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                FireOnUpdateStatus("Cleanup: " + ex.Message);
            }
        }

        /// <summary>
        /// Fires the on update processing event.
        /// </summary>
        /// <param name="busy">if set to <c>true</c> it indicates that this class is processing.</param>
        private void FireOnUpdateProcessing(bool busy)
        {
            if (OnUpdateProcessing != null)
            {
                OnUpdateProcessing(this, new BoolEventArgs(busy));
            }
        }

        /// <summary>
        /// Fires the on update status event.
        /// </summary>
        /// <param name="status">The status.</param>
        private void FireOnUpdateStatus(string status)
        {
            if (OnUpdateStatus != null)
            {
                OnUpdateStatus(this, new StringEventArgs(status));
            }
        }

        /// <summary>
        /// Fires the on processing project event.
        /// </summary>
        /// <param name="text">The text.</param>
        private void FireOnProcessingProject(string text)
        {
            if (OnProcessingProject != null)
            {
                OnProcessingProject(this, new StringEventArgs(text));
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Cleanup();
            }
        }

        #endregion
    }
}

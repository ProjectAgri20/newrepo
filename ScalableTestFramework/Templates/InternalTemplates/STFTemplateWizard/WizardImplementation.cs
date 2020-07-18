using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace STFTemplateWizard
{
    /// <summary>
    /// Implements the logic for processing new items.
    /// </summary>
    public class WizardImplementation : IWizard
    {
        private PlatformTargetDialog _platformTargetDlg;
        private string _platformTarget;
        private string _safeActivityName;
        private string _safeNamespace;
        private string _safeClassPrefix;

        /// <summary>
        /// Runs custom wizard logic before opening an item in the template.
        /// </summary>
        /// <param name="projectItem">The project item that will be opened.</param>
        public void BeforeOpeningFile( ProjectItem projectItem ) { }

        /// <summary>
        /// Runs custom wizard logic when a project has finished generating.
        /// </summary>
        /// <param name="project">The project that finished generating.</param>
        public void ProjectFinishedGenerating( Project project )
        {
            if (!project.Name.StartsWith("Plugin."))
            {
                project.Name = "Plugin." + project.Name;
            }

            // Ensure that the path to CommonAssemblyInfo.cs is relative, not absolute.
            XDocument projectXml = XDocument.Load(project.FileName);
            XElement commonAssyElement = projectXml.Descendants(projectXml.Root.GetDefaultNamespace() + "Compile").FirstOrDefault(n => ((string)n.Attribute("Include").Value).Contains("CommonAssemblyInfo.cs"));
            commonAssyElement.SetAttributeValue("Include", @"..\..\CommonAssemblyInfo.cs");
            projectXml.Save(project.FileName);
        }

        /// <summary>
        /// Runs custom wizard logic when a project item has finished generating.
        /// </summary>
        /// <param name="projectItem">The project item that finished generating.</param>
        public void ProjectItemFinishedGenerating( ProjectItem projectItem ) { }

        /// <summary>
        /// Runs custom wizard logic when the wizard has completed all tasks.
        /// </summary>
        public void RunFinished() { }

        /// <summary>
        /// Runs custom wizard logic at the beginning of a template wizard run.
        /// </summary>
        /// <param name="automationObject">The automation object being used by the template wizard.</param>
        /// <param name="replacementsDictionary">The list of standard parameters to be replaced.</param>
        /// <param name="runKind">A WizardRunKind indicating the type of wizard run.</param>
        /// <param name="customParams">The custom parameters with which to perform parameter replacement in the project.</param>
        public void RunStarted( object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams )
        {
            // Get the platform target.
            try
            {
                using (_platformTargetDlg = new PlatformTargetDialog())
                {
                    if (_platformTargetDlg.ShowDialog() != DialogResult.OK)
                    {
                        throw new WizardCancelledException();
                    }

                    _platformTarget = _platformTargetDlg.PlatformTarget;
                    replacementsDictionary.Add("$platformtarget$", _platformTarget);
                }
            }
            catch (Exception)
            {
                // Clean up the template folder that was written to disk.
                string destinationDirectory = replacementsDictionary["$destinationdirectory$"];
                if (Directory.Exists(destinationDirectory))
                {
                    Directory.Delete(destinationDirectory, true);
                }

                throw;
            }

            // Get the name of the activity.
            _safeActivityName = replacementsDictionary["$safeprojectname$"];
            if (_safeActivityName.StartsWith("Plugin.", StringComparison.OrdinalIgnoreCase))
            {
                _safeActivityName = _safeActivityName.Substring("Plugin.".Length);
            }

            _safeNamespace = Regex.Replace(_safeActivityName, @"\s", ".", RegexOptions.CultureInvariant);
            replacementsDictionary.Add("$safenamespace$", _safeNamespace);

            _safeClassPrefix = Regex.Replace(_safeActivityName, @"[.\s]", "", RegexOptions.CultureInvariant);
            replacementsDictionary.Add("$safeclassprefix$", _safeClassPrefix);
        }

        /// <summary>
        /// Indicates whether the specified project item should be added to the project.
        /// </summary>
        /// <param name="filePath">The path to the project item.</param>
        /// <returns>true if the project item should be added to the project; otherwise, false.</returns>
        public bool ShouldAddProjectItem( string filePath ) => true;
    }
}

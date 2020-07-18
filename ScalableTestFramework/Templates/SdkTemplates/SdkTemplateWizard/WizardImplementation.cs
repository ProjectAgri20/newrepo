using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace SdkTemplateWizard
{
    /// <summary>
    /// Implements the logic for processing new items.
    /// </summary>
    public class WizardImplementation : IWizard
    {
        /// <summary>
        /// Runs custom wizard logic before opening an item in the template.
        /// </summary>
        /// <param name="projectItem">The project item that will be opened.</param>
        public void BeforeOpeningFile( ProjectItem projectItem ) { }

        /// <summary>
        /// Runs custom wizard logic when a project has finished generating.
        /// </summary>
        /// <param name="project">The project that finished generating.</param>
        public void ProjectFinishedGenerating( Project project ) { }

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
            // Create additional replacement strings.
            string safeNamespace = Regex.Replace(replacementsDictionary["$safeprojectname$"], @"\s", "_", RegexOptions.CultureInvariant);
            replacementsDictionary.Add("$safenamespace$", safeNamespace);

            string safeClassPrefix = Regex.Replace(replacementsDictionary["$safeprojectname$"], @"[.\s]", "", RegexOptions.CultureInvariant);
            replacementsDictionary.Add("$safeclassprefix$", safeClassPrefix);

            // Display the template parameters dialog box.
            try
            {
                using (TemplateParmsDialog parmsDialog = new TemplateParmsDialog())
                {
                    parmsDialog.defaultNamespaceTextBox.Text = replacementsDictionary["$safenamespace$"];

                    if (parmsDialog.ShowDialog() != DialogResult.OK)
                    {
                        throw new WizardCancelledException("User cancelled the parameter dialog.");
                    }

                    // Save the parameters specified by the user.
                    replacementsDictionary["$safenamespace$"] = Regex.Replace(parmsDialog.defaultNamespaceTextBox.Text, @"\s", "_", RegexOptions.CultureInvariant);
                    replacementsDictionary.Add("$hplibrarylocation$", parmsDialog.frameworkLibraryLocationTextBox.Text);
                }
            }
            catch (Exception)
            {
                // Clean up the template folder that was created on disk.
                string destinationDirectory = replacementsDictionary["$destinationdirectory$"];
                if (Directory.Exists(destinationDirectory))
                {
                    Directory.Delete(destinationDirectory, true);
                }

                throw;
            }
        }

        /// <summary>
        /// Indicates whether the specified project item should be added to the project.
        /// </summary>
        /// <param name="filePath">The path to the project item.</param>
        /// <returns>true if the project item should be added to the project; otherwise, false.</returns>
        public bool ShouldAddProjectItem( string filePath ) => true;
    }
}

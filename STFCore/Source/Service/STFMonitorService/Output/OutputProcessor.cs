using HP.ScalableTest.FileAnalysis;
using HP.ScalableTest.Utility;
using System;
using System.Collections.ObjectModel;
using System.IO;
using HP.ScalableTest.Framework;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Monitor;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Provides methods for validating and retaining digital send output files.
    /// </summary>
    public class OutputProcessor
    {
        private FileAnalyzer _analyzer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputProcessor"/> class.
        /// </summary>
        /// <param name="filePath">The file to be processed by this <see cref="OutputProcessor"/> instance.</param>
        public OutputProcessor(string filePath)
        {
            _analyzer = FileAnalyzerFactory.Create(filePath);
        }

        /// <summary>
        /// Returns the properties of a document such as page count, title, etc.
        /// </summary>
        /// <returns></returns>
        public DocumentProperties GetProperties() => _analyzer.GetProperties();

        /// <summary>
        /// Validates the file, using the <see cref="OutputMonitorConfig"/> provided.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <returns></returns>
        public ValidationResult Validate(OutputMonitorConfig options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            // Perform basic validation on the file
            FileValidationResult fileValidation = _analyzer.Validate();
            ValidationResult result = new ValidationResult(fileValidation.Success, fileValidation.Message);
            if (!result.Succeeded)
            {
                return result;
            }

            // If requested, ensure that there is an associated metadata file
            if (options.LookForMetadataFile)
            {
                string metadataFile = Path.ChangeExtension(_analyzer.File.FullName, options.MetadataFileExtension);
                if (!File.Exists(metadataFile))
                {
                    return new ValidationResult(false, "Metadata file not found: {0}".FormatWith(Path.GetFileName(metadataFile)));
                }
            }

            // If we reach this point, the file validated successfully
            return new ValidationResult(true);
        }

        /// <summary>
        /// Applies the retention policy from the specified <see cref="OutputMonitorConfig"/> to the file.
        /// </summary>
        /// <param name="options">The validation/retention options.</param>
        /// <param name="isFileValid">if set to <c>true</c> the file should be considered valid.</param>
        public void ApplyRetention(OutputMonitorConfig options, bool isFileValid = true)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            // If the retention option is DoNothing, quit now
            if (options.Retention == RetentionOption.DoNothing)
            {
                return;
            }

            // Determine the "real" name for the file
            string file = _analyzer.File.FullName;
            string filePrefix = options.RetentionFileName ?? Path.GetFileNameWithoutExtension(file);

            // Determine whether we should retain or delete the output file(s)
            bool retain = options.Retention == RetentionOption.AlwaysRetain ||
                (options.Retention == RetentionOption.RetainIfCorrupt && isFileValid == false);

            // Apply the retention option for the file
            string retentionDirectory = string.Empty;
            if (retain)
            {
                string session = ScanFilePrefix.Parse(filePrefix).SessionId;
                retentionDirectory = options.RetentionLocation.Replace("{SESSION}", session);
                System.IO.Directory.CreateDirectory(retentionDirectory);
                CopyFile(file, retentionDirectory, filePrefix, Path.GetExtension(file));
            }
            File.Delete(file);

            // If we were expecting a metadata file, apply the retention options there as well
            if (options.LookForMetadataFile)
            {
                string metadataFile = Path.ChangeExtension(file, options.MetadataFileExtension);
                if (Retry.UntilTrue(() => File.Exists(metadataFile), 5, TimeSpan.FromSeconds(1)))
                {
                    if (retain)
                    {
                        CopyFile(metadataFile, retentionDirectory, filePrefix, options.MetadataFileExtension);
                    }
                    File.Delete(metadataFile);
                }
            }
        }

        private static void CopyFile(string sourceFile, string directory, string name, string extension)
        {
            string destination = Path.Combine(directory, Path.ChangeExtension(name, extension));

            // Ensure that the path is unique so we don't overwrite duplicate files
            int i = 0;
            while (File.Exists(destination))
            {
                string newName = "{0}({1})".FormatWith(name, ++i);
                destination = Path.Combine(directory, Path.ChangeExtension(newName, extension));
            }
            Retry.WhileThrowing(
                () => File.Copy(sourceFile, destination, true),
                10,
                TimeSpan.FromSeconds(2),
                new Collection<Type>() { typeof(IOException) });
        }
    }
}

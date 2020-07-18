using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;


namespace CreateInstaller
{
    internal static class Packager
    {
        /// <summary>
        /// Creates a text file with the extension .md5 that contains the MD5 Checksum value of the specified zip file.
        /// </summary>
        /// <param name="zipFilePath"></param>
        public static void CreateMd5(string zipFilePath)
        {
            byte [] md5Result = null;

            using (MD5 md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(zipFilePath))
                {
                    md5Result = md5.ComputeHash(stream);
                }
            }

            // Remove dashes inserted by BitConverter
            string stringResult = BitConverter.ToString(md5Result).Replace("-", "");

            StringBuilder resultFilePath = new StringBuilder(zipFilePath.Substring(0, zipFilePath.LastIndexOf('.')));
            resultFilePath.Append(".md5");

            File.WriteAllText(resultFilePath.ToString(), stringResult);
        }

        /// <summary>
        /// Zips and creates MD5 files for each
        /// </summary>
        /// <returns>The filePath of the package zip file.</returns>
        public static string CreatePackage(List<string> filePaths, string packageName, string outputFolderPath)
        {
            string packageFilePath = Path.Combine(outputFolderPath, packageName + ".zip");

            filePaths.Add(CreateManifestFile(filePaths, outputFolderPath));

            string examplesFolderPath = string.Empty;
            foreach (string path in filePaths)
            {
                if (path.EndsWith("Examples"))
                {
                    examplesFolderPath = path;
                }
            }

            if (! string.IsNullOrEmpty(examplesFolderPath))
            {
                // Remove the folder path from the list so the zip operation doesn't throw.
                filePaths.Remove(examplesFolderPath);
            }

            Zip(filePaths, packageFilePath, examplesFolderPath);

            return packageFilePath;
        }

        /// <summary>
        /// Gathers all necessary files into a final compressed file for distribution.
        /// </summary>
        private static void Zip(List<string> filePathsToZip, string outputFilePath, string folderPathToZip = null)
        {
            // Clean up existing zip file if one is found.
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            // Create a new .zip archive using the Folder path as a starting point
            if (! string.IsNullOrEmpty(folderPathToZip))
            {
                ZipFile.CreateFromDirectory(folderPathToZip, outputFilePath);
            }

            // Open the .zip archive and add the files to it
            using (ZipArchive zipFile = ZipFile.Open(outputFilePath, ZipArchiveMode.Update))
            {
                foreach (string filePath in filePathsToZip)
                {
                    zipFile.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                }
            }
        }

        /// <summary>
        /// Creates the manifest file.
        /// A text-based list of files included in the zip
        /// </summary>
        private static string CreateManifestFile(List<string> filePaths, string outputFolderPath)
        {
            string manifestFilePath = Path.Combine(outputFolderPath, "Manifest.txt");

            // Clean up existing manifest file if one is found.
            if (File.Exists(manifestFilePath))
            {
                File.Delete(manifestFilePath);
            }

            // Make sure the directory path exists
            Directory.CreateDirectory(outputFolderPath);
            File.WriteAllLines(manifestFilePath, filePaths);

            using (StreamWriter file = new StreamWriter(manifestFilePath, false))
            {
                foreach (string filePath in filePaths)
                {
                    file.WriteLine(Path.GetFileName(filePath));
                }
            }

            return manifestFilePath;
        }

    }
}

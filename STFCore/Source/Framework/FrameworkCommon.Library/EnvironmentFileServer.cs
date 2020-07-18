using System.IO;
using System.Linq;
using System.Security.Cryptography;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Manages adding and removing resource files from the Enterprise Test environment.
    /// </summary>
    public static class EnvironmentFileServer
    {
        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <returns></returns>
        public static string ReadFile(string sourcePath)
        {
            string text = string.Empty;

            string shareLocation = Path.GetPathRoot(sourcePath);

            // The file exists on a server.  Connect to that server first.
            NetworkConnection.AddConnection(shareLocation, GlobalSettings.Items.DomainAdminCredential);
            try
            {
                // We ignore files that do not exist.
                if (File.Exists(sourcePath))
                {
                    text = File.ReadAllText(sourcePath);
                }
            }
            finally
            {
                NetworkConnection.RemoveConnection(shareLocation, forceRemoval: true);
            }

            return text;
        }

        /// <summary>
        /// Copies the file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        public static void CopyFile(string sourcePath, string destinationPath)
        {
            string shareLocation = Path.GetPathRoot(destinationPath);

            NetworkConnection.AddConnection(shareLocation, GlobalSettings.Items.DomainAdminCredential);
            try
            {
                // Copy the file if it does not already exist or is not already identical to source
                if (!File.Exists(destinationPath) || !FilesAreEqual(sourcePath, destinationPath))
                {
                    File.Copy(sourcePath, destinationPath, true);
                }
            }
            finally
            {
                NetworkConnection.RemoveConnection(shareLocation, forceRemoval: true);
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        public static void DeleteFile(string sourcePath)
        {
            string shareLocation = Path.GetPathRoot(sourcePath);

            // The file exists on a server.  Connect to that server first.
            NetworkConnection.AddConnection(shareLocation, GlobalSettings.Items.DomainAdminCredential);
            try
            {
                // We ignore files that do not exist.
                if (File.Exists(sourcePath))
                {
                    // Remove Readonly attribute
                    new FileInfo(sourcePath).Attributes &= ~FileAttributes.ReadOnly;

                    File.Delete(sourcePath);
                }
            }
            finally
            {
                NetworkConnection.RemoveConnection(shareLocation, forceRemoval: true);
            }
        }

        /// <summary>
        /// Determines whether file exists
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static bool FileExists(string filePath)
        {
            bool result = false;
            string shareLocation = Path.GetPathRoot(filePath);

            // The file exists on a server.  Connect to that server first.
            NetworkConnection.AddConnection(shareLocation, GlobalSettings.Items.DomainAdminCredential);
            try
            {
                result = File.Exists(filePath);
            }
            finally
            {
                NetworkConnection.RemoveConnection(shareLocation, forceRemoval: true);
            }

            return result;
        }

        /// <summary>
        /// Determines whether given files have same name and content
        /// </summary>
        /// <param name="filePath1">The file path1.</param>
        /// <param name="filePath2">The file path2.</param>
        /// <returns></returns>
        private static bool FilesAreEqual(string filePath1, string filePath2)
        {
            var result = false;
            byte[] m1 = null;
            byte[] m2 = null;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath1))
                {
                    m1 = md5.ComputeHash(stream);
                }

                using (var stream = File.OpenRead(filePath2))
                {
                    m2 = md5.ComputeHash(stream);
                }
            }

            result = m1.SequenceEqual(m2);
            return result;
        }

        /// <summary>
        /// Finds a file name that is similar to the requested file name, but does not conflict with any existing files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string FindUnusedFileName(string path)
        {
            string basePath = Path.GetDirectoryName(path);
            string baseFileName = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);

            if (!File.Exists(path))
            {
                return path;
            }

            // This means that the file path was seen.
            for (int i = 1; i < 100; i++)
            {
                string newPath = Path.Combine(basePath, baseFileName + i + extension);
                if (!File.Exists(newPath))
                {
                    return newPath;
                }
            }

            throw new IOException("Unable to find a unique file name for {0} after 100 tries.".FormatWith(path));
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using HP.ScalableTest.Utility;
using Microsoft.Win32.SafeHandles;

namespace HP.ScalableTest.WindowsAutomation
{
    /// <summary>
    /// Provides methods for working with files on a file system.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Recursively copies a directory and its contents to a new location.
        /// </summary>
        /// <param name="source">The source directory.</param>
        /// <param name="destination">The destination directory.</param>
        public static void CopyDirectory(string source, string destination)
        {
            // Create the destination path if it does not exist
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (string directory in Directory.GetDirectories(source))
            {
                CopyDirectory(directory, Path.Combine(destination, Path.GetFileName(directory)));
            }

            foreach (string sourceFile in Directory.GetFiles(source))
            {
                string destinationFile = Path.Combine(destination, Path.GetFileName(sourceFile));
                try
                {
                    Retry.WhileThrowing<IOException>(() => File.Copy(sourceFile, destinationFile, true), 10, TimeSpan.FromSeconds(1));
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    // Ignore the file that could not be accessed.
                }
            }
        }

        /// <summary>
        /// Removes all files and folders within a directory, leaving the directory empty.
        /// Files that cannot be deleted are silently ignored.
        /// </summary>
        /// <param name="directory">The directory to empty.</param>
        /// <exception cref="ArgumentNullException"><paramref name="directory" /> is null.</exception>
        public static void EmptyDirectory(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }

            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (IOException)
                {
                    // Do nothing
                }
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public static void DeleteDirectory(string directory)
        {
            DeleteDirectory(new DirectoryInfo(directory));
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="directory" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void DeleteDirectory(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            ProcessUtil.Execute("cmd.exe", $"/C rmdir /S /Q {directory.FullName}", TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <param name="credential">The credential to use when deleting the directory.</param>
        public static void DeleteDirectory(string directory, NetworkCredential credential)
        {
            DeleteDirectory(new DirectoryInfo(directory), credential);
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <param name="credential">The credential to use when deleting the directory.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="directory" /> is null.
        /// <para>or</para>
        /// <paramref name="credential" /> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void DeleteDirectory(DirectoryInfo directory, NetworkCredential credential)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            ProcessUtil.Execute("cmd.exe", $"/C rmdir /S /Q {directory.FullName}", credential, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Gets the size of the specified file using a safe handle that will not lock the file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The size of the file, in bytes.  (If the file was inaccessible, this method will return 0.)</returns>
        public static int GetFileSize(string fileName)
        {
            uint size = 0;
            using (SafeFileHandle handle = NativeMethods.CreateFile(fileName, 0, FileShare.Read, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero))
            {
                if (!handle.IsInvalid)
                {
                    size = NativeMethods.GetFileSize(handle, IntPtr.Zero);
                }
            }

            return (int)size;
        }

        #region DLL Imports

        private static class NativeMethods
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            internal static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

            [DllImport("kernel32")]
            internal static extern uint GetFileSize(SafeFileHandle hFile, IntPtr lpFileSizeHigh);
        }

        #endregion
    }
}

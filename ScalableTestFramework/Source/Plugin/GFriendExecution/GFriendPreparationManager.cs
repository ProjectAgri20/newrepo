using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    /// <summary>
    /// Prepare GFriend Execution (Script and Libraries)
    /// </summary>
    public static class GFriendPreparationManager
    {
        /// <summary>
        /// Prepare GFried native libraries for exeuciton and configuration control
        /// Developer Note : 
        /// GFriend dynamically load native libraries (GFK.*.dll files) when execution.
        /// For exeucting GFriend script path of library files should be given.
        /// This method unzip the embeded archive which contains GFriend native library to given path
        /// </summary>
        /// <param name="libraryPath">Path which library files are saved</param>
        public static void PrepareLibrary(string libraryPath)
        {
            using(MemoryStream zipStream = new MemoryStream(Properties.Resources.libs))
            {
                using (ZipArchive libsZip = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    if(!Directory.Exists(libraryPath))
                    {
                        Directory.CreateDirectory(libraryPath);
                    }

                    foreach(ZipArchiveEntry entry in libsZip.Entries)
                    {
                        string destinationPath = Path.Combine(libraryPath, entry.FullName);
                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            Directory.CreateDirectory(destinationPath);
                        }
                        else
                        {
                            destinationPath = destinationPath.Replace('/', Path.DirectorySeparatorChar);
                            try
                            {
                                using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                                {
                                    entry.Open().CopyTo(fileStream);
                                    fileStream.Flush();
                                    fileStream.Close();
                                }
                            }
                            catch (Exception)
                            {
                                // Consume exception here - if file is used by other process, assume all file is up-to-date
                            }


                        }
                    }
                }
            }
         }

        /// <summary>
        /// Prepare GFried script and dependencies (GF Library and GF Variables)
        /// This method used for saving files to the disk from <see cref="GFriendExecutionActivityData"/>
        /// </summary>
        /// <param name="files">GFriend file list</param>
        /// <param name="scriptPath">Path which script files are saved</param>
        /// <param name="executionData">Plugin execution data</param>
        public static void PrepareFiles(List<GFriendFile> files, string scriptPath)
        {
            if(!Directory.Exists(scriptPath))
            {
                Directory.CreateDirectory(scriptPath);
            }
            foreach(GFriendFile file in files)
            {
                if (file.FileType.Equals("GF Script"))
                {
                    file.FileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Environment.UserName}{Path.GetExtension(file.FileName)}";
                }

                string filePath = Path.Combine(scriptPath, file.FileName);
                
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.Write(file.FileContents);
                    writer.Close();
                }
            }
        }
    }
}

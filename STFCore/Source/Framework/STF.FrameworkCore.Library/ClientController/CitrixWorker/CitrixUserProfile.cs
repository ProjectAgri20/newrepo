using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using HP.ScalableTest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.WindowsAutomation;
using Microsoft.Win32;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Singleton class used to clean up after a Citrix user's session ends on the server
    /// </summary>
    public static class CitrixUserProfile
    {
        private const string _profilePath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";

        /// <summary>
        /// Cleans up the specified user profile path.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userProfilePath">The user profile path.</param>
        public static void Cleanup(string userId, string userProfilePath)
        {
            ThreadPool.QueueUserWorkItem(t => CleanupUserProfile(userId, userProfilePath));
        }

        internal static void CleanupUserProfile(string userId, string userPath)
        {
            try
            {
                TraceFactory.Logger.Debug("Path: {0}".FormatWith(userPath));
                if (userPath.Contains("TEMP"))
                {
                    // If the path contains TEMP, then it's a temporary profile and it will be 
                    // deleted automatically on its own.  Still go ahead and remove the registry
                    // entry for this user's profile.
                    TraceFactory.Logger.Debug("Temp profile {0}".FormatWith(userPath));
                    DeleteUserRegistry(userId);
                }
                else
                {
                    // If the real user directory exists, then delete the directory and then
                    // remove any entry in the registry for this user as well.  This will 
                    // clean up all profile related information.
                    if (Directory.Exists(userPath))
                    {
                        TraceFactory.Logger.Debug("Deleting {0}".FormatWith(userPath));
                        NetworkCredential credential = new NetworkCredential(GlobalSettings.Items[Setting.DomainAdminUserName], GlobalSettings.Items[Setting.DomainAdminPassword]);
                        FileSystem.DeleteDirectory(userPath, credential);
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Path doesn't exist");
                    }

                    DeleteUserRegistry(userId);
                }
            }
            catch (IOException ex)
            {
                TraceFactory.Logger.Error("IO error", ex);
            }
            catch (ObjectDisposedException ex)
            {
                TraceFactory.Logger.Error("Object disposed error", ex);
            }
            catch (SecurityException ex)
            {
                TraceFactory.Logger.Error("Security error", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                TraceFactory.Logger.Error("Autorization error", ex);
            }
            catch (ArgumentException ex)
            {
                TraceFactory.Logger.Error("Arg error", ex);
            }
        }

        internal static List<string> RegistryProfileList
        {
            get
            {
                // Return all the registry entries that have the profile path ending with .bak
                // These indicate that the user will have a temporary profile directory created
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(_profilePath))
                {
                    return key.GetSubKeyNames().ToList();
                }
            }
        }

        internal static void DeleteUserRegistry(string userName)
        {
            bool found = false;
            string profilePath = string.Empty;

            TraceFactory.Logger.Debug("Deleting registry entry for {0}".FormatWith(userName));
            foreach (string profile in RegistryProfileList)
            {
                profilePath = Path.Combine(_profilePath, profile);
                using (RegistryKey profileKey = Registry.LocalMachine.OpenSubKey(profilePath))
                {
                    if (profileKey != null)
                    {
                        object profileImagePathObj = profileKey.GetValue("ProfileImagePath");

                        if (profileImagePathObj != null)
                        {
                            string profileImagePath = (string)profileImagePathObj;

                            if (profileImagePath.Trim().EndsWith(userName, StringComparison.OrdinalIgnoreCase))
                            {
                                TraceFactory.Logger.Debug("Found {0}".FormatWith(profileImagePath));
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (found)
            {
                TraceFactory.Logger.Debug("Deleting {0}".FormatWith(profilePath));
                Registry.LocalMachine.DeleteSubKeyTree(profilePath);
                TraceFactory.Logger.Debug("Done");
            }
        }

        //static void DeleteUserDirectory(string directory)
        //{
        //    SecureString securePassword = null;
        //    try
        //    {
        //        using (Process p = new Process())
        //        {
        //            p.StartInfo.FileName = "cmd.exe";
        //            p.StartInfo.Arguments = "/C rmdir /S /Q {0}".FormatWith(directory);
        //            p.StartInfo.LoadUserProfile = true;
        //            p.StartInfo.CreateNoWindow = false;
        //            p.StartInfo.UseShellExecute = false;
        //            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        //            p.StartInfo.RedirectStandardOutput = false;

        //            p.StartInfo.UserName = GlobalSettings.Items[Setting.DomainAdminUserName];
        //            p.StartInfo.Domain = Environment.UserDomainName;
        //            p.StartInfo.Password = GlobalSettings.Items[Setting.DomainAdminPassword].ToSecureString();

        //            TraceFactory.Logger.Debug("{0} {1}".FormatWith(p.StartInfo.FileName, p.StartInfo.Arguments));

        //            p.Start();

        //            if (!p.WaitForExit(30000))
        //            {
        //                TraceFactory.Logger.Debug("Unable to delete directory, timeout");
        //            }
        //            else
        //            {
        //                TraceFactory.Logger.Debug("Directory deleted");
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (securePassword != null)
        //        {
        //            securePassword.Dispose();
        //        }
        //    }
        //}

    }
}

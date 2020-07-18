using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.WindowsAutomation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Singleton class used to clean up after a Citrix user's session ends on the server
    /// </summary>
    public static class VirtualResourceUserProfile
    {
        private const string RegistryProfilePath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";

        /// <summary>
        /// Cleans up the specified user profile path.
        /// </summary>
        /// <param name="credential">The user id.</param>
        public static void Cleanup(OfficeWorkerCredential credential)
        {
            //if the credential is same as the current user then ignore
            if (credential.UserName == Environment.UserName)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(t => CleanupUserProfile(credential));
        }

        internal static void CleanupUserProfile(OfficeWorkerCredential credential)
        {
            try
            {
                //remove the credential from the administrator group
                var userPath = GetProfilePath(credential.UserName);
                TraceFactory.Logger.Debug("Path: {0}".FormatWith(userPath));

                RemoveUserFromGroup(credential);

                if (string.IsNullOrEmpty(userPath))
                {
                    return;
                }

                DeleteUserRegistry(credential.UserName);

                if (userPath.Contains("TEMP"))
                {
                    // If the path contains TEMP, then it's a temporary profile and it will be
                    // deleted automatically on its own.  Still go ahead and remove the registry
                    // entry for this user's profile.
                    TraceFactory.Logger.Debug("Temp profile {0}".FormatWith(userPath));
                }
                else
                {
                    // If the real user directory exists, then delete the directory and then
                    // remove any entry in the registry for this user as well.  This will
                    // clean up all profile related information.
                    if (Directory.Exists(userPath))
                    {
                        TraceFactory.Logger.Debug("Deleting {0}".FormatWith(userPath));
                        NetworkCredential domainAdminNetworkCredential = new NetworkCredential(GlobalSettings.Items[Setting.DomainAdminUserName], GlobalSettings.Items[Setting.DomainAdminPassword], GlobalSettings.Items[Setting.Domain]);
                        FileSystem.DeleteDirectory(userPath, domainAdminNetworkCredential);

                        //sometimes the files are deleted but the folder structure remains, doing the same thing again works
                        if (Directory.Exists(userPath))
                            FileSystem.DeleteDirectory(userPath, domainAdminNetworkCredential);
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Path doesn't exist");
                    }
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
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryProfilePath))
                {
                    return key.GetSubKeyNames().ToList();
                }
            }
        }

        /// <summary>
        /// Gets the directory path of the user profile
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Directory Path</returns>
        internal static string GetProfilePath(string userName)
        {
            string profileImagePath = string.Empty;
            TraceFactory.Logger.Debug("Getting Profile Path for {0}".FormatWith(userName));
            foreach (string profile in RegistryProfileList)
            {
                var profilePath = Path.Combine(RegistryProfilePath, profile);
                using (RegistryKey profileKey = Registry.LocalMachine.OpenSubKey(profilePath))
                {
                    string profileImagePathObj = profileKey?.GetValue("ProfileImagePath").ToString();

                    if (string.IsNullOrEmpty(profileImagePathObj))
                        continue;

                    if (profileImagePathObj.Trim().EndsWith(userName, StringComparison.OrdinalIgnoreCase))
                    {
                        TraceFactory.Logger.Debug("Found {0}".FormatWith(profileImagePath));
                        profileImagePath = profileImagePathObj;
                        break;
                    }
                }
            }

            return profileImagePath;
        }

        /// <summary>
        /// Removes the Registry entry for the user in HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList
        /// </summary>
        /// <param name="userName"></param>
        internal static void DeleteUserRegistry(string userName)
        {
            bool found = false;
            string profilePath = string.Empty;

            TraceFactory.Logger.Debug("Deleting registry entry for {0}".FormatWith(userName));
            foreach (string profile in RegistryProfileList)
            {
                profilePath = Path.Combine(RegistryProfilePath, profile);
                using (RegistryKey profileKey = Registry.LocalMachine.OpenSubKey(profilePath))
                {
                    string profileImagePath = profileKey?.GetValue("ProfileImagePath").ToString();

                    if (string.IsNullOrEmpty(profileImagePath))
                        continue;

                    if (profileImagePath.Trim().EndsWith(userName, StringComparison.OrdinalIgnoreCase))
                    {
                        TraceFactory.Logger.Debug("Found {0}".FormatWith(profileImagePath));
                        found = true;
                        break;
                    }
                }
            }

            if (found)
            {
                TraceFactory.Logger.Debug("Deleting {0}".FormatWith(profilePath));
                Registry.LocalMachine.DeleteSubKeyTree(profilePath);
                Registry.LocalMachine.Close();

                TraceFactory.Logger.Debug("Done");
            }
        }

        /// <summary>
        /// Removes the user from Administrator group of the local machine
        /// </summary>
        /// <param name="credential"></param>
        internal static void RemoveUserFromGroup(OfficeWorkerCredential credential)
        {
            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, credential.Domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, credential.UserName);

            PrincipalContext userContext = new PrincipalContext(ContextType.Machine, Environment.MachineName);
            GroupPrincipal administrators = GroupPrincipal.FindByIdentity(userContext, "Administrators");

            ActiveDirectoryController.RemoveUserFromGroup(user, administrators);
        }
    }
}
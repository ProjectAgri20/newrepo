using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.Security.Principal;
using HP.ScalableTest;

namespace HP.SolutionTest.Install
{
    /// <summary>
    /// Shared drive class
    /// </summary>
    public class SharedDrive
    {
        private readonly string _name = string.Empty;

        /// <summary>
        /// Enum ShareResult
        /// </summary>
        public enum ShareResult : uint
        {
            /// <summary>
            /// 
            /// </summary>
            Success             = 0, 	
            /// <summary>
            /// 
            /// </summary>
            AccessDenied        = 2, 	
            /// <summary>
            /// 
            /// </summary>
            UnknownFailure      = 8, 	
            /// <summary>
            /// 
            /// </summary>
            InvalidName         = 9, 	
            /// <summary>
            /// 
            /// </summary>
            InvalidLevel        = 10, 	
            /// <summary>
            /// 
            /// </summary>
            InvalidParameter    = 21, 	
            /// <summary>
            /// 
            /// </summary>
            DuplicateShare      = 22, 	
            /// <summary>
            /// 
            /// </summary>
            RedirectedPath      = 23, 	
            /// <summary>
            /// 
            /// </summary>
            UnknownDevice       = 24, 	
            /// <summary>
            /// 
            /// </summary>
            NetNameNotFound     = 25 	
        }

        /// <summary>
        /// Enum ShareType
        /// </summary>
        public enum ShareType : uint
        {
            /// <summary>
            /// 
            /// </summary>
            DiskDrive       = 0x0, 	       
            /// <summary>
            /// 
            /// </summary>
            PrintQueue      = 0x1, 	       
            /// <summary>
            /// 
            /// </summary>
            Device          = 0x2, 	       
            /// <summary>
            /// 
            /// </summary>
            IPC             = 0x3, 	       
            /// <summary>
            /// 
            /// </summary>
            DiskDriveAdmin  = 0x80000000, 	
            /// <summary>
            /// 
            /// </summary>
            PrintQueueAdmin = 0x80000001, 	
            /// <summary>
            /// 
            /// </summary>
            DeviceAdmin     = 0x80000002, 	
            /// <summary>
            /// 
            /// </summary>
            IpcAdmin        = 0x80000003 
        }

        private readonly ManagementObject _manager;
        private ManagementBaseObject _params = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedDrive"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public SharedDrive(string name)
        {
            _name = name;
            _manager = new ManagementClass("Win32_Share");

            _params = _manager.GetMethodParameters("Create");
            _params["Type"] = ShareType.DiskDrive;
            _params["Password"] = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedDrive"/> class.
        /// </summary>
        /// <param name="managementObject">The management object.</param>
        private SharedDrive(ManagementObject managementObject)
        {
            _manager = managementObject;
        }

        /// <summary>
        /// Gets the access mask.
        /// </summary>
        /// <value>The access mask.</value>
        public uint AccessMask => Convert.ToUInt32(_manager["AccessMask"]);

        /// <summary>
        /// Gets a value indicating whether [allow maximum].
        /// </summary>
        /// <value><c>true</c> if [allow maximum]; otherwise, <c>false</c>.</value>
        public bool AllowMaximum => Convert.ToBoolean(_manager["AllowMaximum"]);

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption => Convert.ToString(_manager["Caption"]);

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description => Convert.ToString(_manager["Description"]);

        /// <summary>
        /// Gets the install date.
        /// </summary>
        /// <value>The install date.</value>
        public DateTime InstallDate => Convert.ToDateTime(_manager["InstallDate"]);

        /// <summary>
        /// Gets the maximum allowed.
        /// </summary>
        /// <value>The maximum allowed.</value>
        public uint MaximumAllowed => Convert.ToUInt32(_manager["MaximumAllowed"]);

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => Convert.ToString(_manager["Name"]);

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path => Convert.ToString(_manager["Path"]);

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status => Convert.ToString(_manager["Status"]);

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public ShareType Type => (ShareType)Convert.ToUInt32(_manager["Type"]);

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns>ShareResult.</returns>
        public ShareResult Delete()
        {
            object resultData = _manager.InvokeMethod("Delete", new object[] { });
            uint result = Convert.ToUInt32(resultData);
            return (ShareResult)result;
        }

        /// <summary>
        /// Creates the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="description">The description.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="username">The username.</param>
        /// <param name="maximumAllowed">The maximum allowed.</param>
        /// <param name="rights">The rights.</param>
        /// <returns>ShareResult.</returns>
        public ShareResult Create
            (
                string filePath,
                string description,
                string domain,
                string username,
                uint maximumAllowed,
                params FileSystemRights[] rights
            )
        {
            var account = CreateAccount(domain, username);
            return CreateShare(filePath, description, maximumAllowed, false, account, rights);
        }
        /// <summary>
        /// Creates the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="description">The description.</param>
        /// <param name="username">The username.</param>
        /// <param name="maximumAllowed">The maximum allowed.</param>
        /// <param name="rights">The rights.</param>
        /// <returns>ShareResult.</returns>
        public ShareResult Create
            (
                string filePath,
                string description,
                string username,
                uint maximumAllowed,
                params FileSystemRights[] rights
            )
        {
            return CreateShare(filePath, description, maximumAllowed, false, username, rights);
        }

        /// <summary>
        /// Creates the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="description">The description.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="username">The username.</param>
        /// <param name="rights">The rights.</param>
        /// <returns>ShareResult.</returns>
        public ShareResult Create
            (
                string filePath,
                string description,
                string domain,
                string username,
                params FileSystemRights[] rights
            )
        {
            var account = CreateAccount(domain, username);
            return CreateShare(filePath, description, 0, true, account, rights);
        }

        /// <summary>
        /// Creates the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="description">The description.</param>
        /// <param name="username">The username.</param>
        /// <param name="rights">The rights.</param>
        /// <returns>ShareResult.</returns>
        public ShareResult Create
            (
                string filePath,
                string description,
                string username,
                params FileSystemRights[] rights
            )
        {            
            return CreateShare(filePath, description, 0, true, username, rights);
        }

        private ShareResult CreateShare
            (
                string filePath,
                string description,
                uint maximumAllowed,
                bool allowMaximum,
                string account,
                params FileSystemRights[] rights
            )
        {
            ShareResult result = ShareResult.Success;

            try
            {
                _params["Name"] = _name;
                _params["Description"] = description;
                _params["Path"] = filePath;
                _params["Access"] = null; // CreateSecurityDescriptor(account, rights);

                if (allowMaximum)
                {
                    _params["MaximumAllowed"] = null;
                }
                else
                {
                    _params["MaximumAllowed"] = maximumAllowed;
                }

                ManagementBaseObject output = _manager.InvokeMethod("Create", _params, null);
                uint returnValue = (uint)(output.Properties["ReturnValue"].Value);
                result = (ShareResult)returnValue;

                TraceFactory.Logger.Debug("Result: {0}".FormatWith(result));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                throw;
            }

            return result;
        }

        private string CreateAccount(string domain, string username)
        {
            return string.Format(CultureInfo.CurrentCulture,@"{0}\{1}", domain, username);
        }

        /// <summary>
        /// Sets the user rights.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="username">The username.</param>
        /// <param name="rights">The rights.</param>
        public void SetUserRights(string domain, string username, params FileSystemRights[] rights)
        {
            SetUserRights(CreateAccount(domain, username), rights);
        }

        /// <summary>
        /// Sets the user rights.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="rights">The rights.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void SetUserRights(string username, params FileSystemRights[] rights)
        {
            var share = GetNamedShare(_name);
            if (share == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture,"A share named '{0}' does not exist", _name));
            }

            var securityDescriptor = CreateSecurityDescriptor(username, rights);

            uint? maximumAllowed = share.MaximumAllowed;
            if (share.AllowMaximum)
            {
                maximumAllowed = null;
            }

            ManagementObject shareInfo = new ManagementObject(_manager.Path + ".Name='" + _name + "'");
            var parameters = new object[] { maximumAllowed, null, securityDescriptor };
            shareInfo.InvokeMethod("SetShareInfo", parameters);
        }

        /// <summary>
        /// Creates the security descriptor.
        /// </summary>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="rights">The rights.</param>
        /// <returns>ManagementObject.</returns>
        private ManagementObject CreateSecurityDescriptor(string accountName, FileSystemRights[] rights)
        {
            var account = new NTAccount(accountName);
            SecurityIdentifier sid = (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));
            byte[] sidData = new byte[sid.BinaryLength];
            sid.GetBinaryForm(sidData, 0);

            var accountItems = accountName.Split('\\');
            var trustee = new ManagementClass("Win32_Trustee");
            if (accountItems.Count() == 1)
            {
                trustee["Name"] = accountName;
            }
            else 
            {
                trustee["Domain"] = accountItems[0];
                trustee["Name"] = accountItems[1];
            }
            trustee["SID"] = sidData;

            var ace = new ManagementClass("Win32_Ace");
            ace["AccessMask"] = rights.Cast<uint>().Aggregate((a, b) => a | b);
            ace["AceFlags"] = AceFlags.ObjectInherit | AceFlags.ContainerInherit;
            ace["AceType"] = AceType.AccessAllowed;
            ace["Trustee"] = trustee;

            var security = new ManagementClass("Win32_SecurityDescriptor");
            security["ControlFlags"] = ControlFlags.DiscretionaryAclPresent;
            security["DACL"] = new object[] { ace };

            return security;
        }

        /// <summary>
        /// Gets all shares.
        /// </summary>
        /// <returns>IList&lt;SharedDrive&gt;.</returns>
        public static IList<SharedDrive> GetAllShares()
        {
            IList<SharedDrive> result = new List<SharedDrive>();
            ManagementClass management = new ManagementClass("Win32_Share");
            ManagementObjectCollection managementItems = management.GetInstances();

            foreach (ManagementObject item in managementItems)
            {
                SharedDrive share = new SharedDrive(item);
                result.Add(share);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified name is shared.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified name is shared; otherwise, <c>false</c>.</returns>
        public static bool IsShared(string name)
        {
            return GetAllShares().Any(x => x.Name.Equals(name));
        }

        /// <summary>
        /// Gets the named share.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SharedDrive.</returns>
        public static SharedDrive GetNamedShare(string name)
        {
            return GetAllShares().FirstOrDefault(x => x.Name.Equals(name));
        }

        // Adds an ACL entry on the specified directory for the specified account. 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="account"></param>
        /// <param name="rights"></param>
        /// <param name="controlType"></param>
        public static void AddDirectorySecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(fileName);

            // Get a DirectorySecurity object that represents the  
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);
        }

        // Removes an ACL entry on the specified directory for the specified account. 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="account"></param>
        /// <param name="rights"></param>
        /// <param name="controlType"></param>
        public static void RemoveDirectorySecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(fileName);

            // Get a DirectorySecurity object that represents the  
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);
        }
    }
}

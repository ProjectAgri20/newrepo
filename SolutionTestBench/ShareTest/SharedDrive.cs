using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ShareTest
{
    internal class SharedDrive
    {
        private bool _allowMaximum = false;
        private readonly string _name = string.Empty;

        /// <summary>
        /// Enum ShareResult
        /// </summary>
        public enum ShareResult : uint
        {
            Success             = 0, 	
            AccessDenied        = 2, 	
            UnknownFailure      = 8, 	
            InvalidName         = 9, 	
            InvalidLevel        = 10, 	
            InvalidParameter    = 21, 	
            DuplicateShare      = 22, 	
            RedirectedPath      = 23, 	
            UnknownDevice       = 24, 	
            NetNameNotFound     = 25 	
        }

        /// <summary>
        /// Enum ShareType
        /// </summary>
        public enum ShareType : uint
        {
            DiskDrive       = 0x0, 	       
            PrintQueue      = 0x1, 	       
            Device          = 0x2, 	       
            IPC             = 0x3, 	       
            DiskDriveAdmin  = 0x80000000, 	
            PrintQueueAdmin = 0x80000001, 	
            DeviceAdmin     = 0x80000002, 	
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
            _allowMaximum = true;

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
        public uint AccessMask
        {
            get { return Convert.ToUInt32(_manager["AccessMask"]); }
        }

        /// <summary>
        /// Gets a value indicating whether [allow maximum].
        /// </summary>
        /// <value><c>true</c> if [allow maximum]; otherwise, <c>false</c>.</value>
        public bool AllowMaximum
        {
            get { return Convert.ToBoolean(_manager["AllowMaximum"]); }
        }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return Convert.ToString(_manager["Caption"]); }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return Convert.ToString(_manager["Description"]); }
        }

        /// <summary>
        /// Gets the install date.
        /// </summary>
        /// <value>The install date.</value>
        public DateTime InstallDate
        {
            get { return Convert.ToDateTime(_manager["InstallDate"]); }
        }

        /// <summary>
        /// Gets the maximum allowed.
        /// </summary>
        /// <value>The maximum allowed.</value>
        public uint MaximumAllowed
        {
            get { return Convert.ToUInt32(_manager["MaximumAllowed"]); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return Convert.ToString(_manager["Name"]); }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return Convert.ToString(_manager["Path"]); }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return Convert.ToString(_manager["Status"]); }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public ShareType Type
        {
            get { return (ShareType)Convert.ToUInt32(_manager["Type"]); }
        }

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

                Console.WriteLine("Result: <{0}>, <{1}>", returnValue, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return result;
        }

        private string CreateAccount(string domain, string username)
        {
            return string.Format(@"{0}\{1}", domain, username);
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
                throw new ArgumentNullException(string.Format("A share named '{0}' does not exist", _name));
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
            return GetAllShares().Where(x => x.Name.Equals(name)).FirstOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.Framework
{
    public class STFLoginManager
    {
        /// <summary>
        /// The System database to connect to (Prod, Dev, etc.)
        /// </summary>
        public static string SystemDatabase { get; set; }

        public static string Environment 
        { 
            get
            {
                string result = "Unknown";
                try
                {
                    result = GlobalSettings.Items[Setting.Environment];
                }
                catch { }
                return result;
            }
        }

        /// <summary>
        /// Set up all configuration data needed to connect to an STF Environment.
        /// Sets up which database to connect to as well as user logon credentials.
        /// If default settings are unavailable, the user is prompted for the information.
        /// </summary>
        /// <returns></returns>
        public static bool Login()
        {
            if (ConnectToDatabase())
            {
                if (GlobalSettings.IsDistributedSystem)
                {
                    return SetLoginConfiguration();
                }
                else
                {
                    return SetLoginConfiguration(System.Environment.UserName, null, null);
                }
            }

            return false;
        }

        /// <summary>
        /// Sets global settings for which database
        /// </summary>
        /// <returns></returns>
        public static bool ConnectToDatabase()
        {
            if (STFDispatcherManager.Dispatcher == null || STFDispatcherManager.DisconnectFromDispatcher(true))
            {
                var systems = GetAvailableSystems();

                if (systems.Count == 1)
                {
                    StfSystem system = systems.First();
                    InitializeDataConnection(system.Name, system.Address);
                    return true;
                }
                else
                {
                    string lastUsedSystem = UserAppDataRegistry.GetValue("STFSystem") as string;
                    using (InputDialog inputDialog = new InputDialog("Select an Environment to connect to:", "Connect to Environment", lastUsedSystem))
                    {
                        inputDialog.StartPosition = FormStartPosition.CenterScreen;
                        inputDialog.InitializeComboBox(systems.Select(x=>x.Name).ToList());

                        if (inputDialog.ShowDialog() == DialogResult.OK)
                        {
                            var selectedSystem = systems.First(x => x.Name.Equals(inputDialog.Value));
                            InitializeDataConnection(selectedSystem.Name, selectedSystem.Address);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static void InitializeDataConnection(string name, string value)
        {
            //Make sure we unsubscribe from any previous connections. 
            SessionClient.Instance.Stop();

            //Save the selection to the registry.
            UserAppDataRegistry.SetValue("STFSystem", name);

            LoadGlobalSettings(value);
        }

        private static void LoadGlobalSettings(string database)
        {
            // Load or reload settings from the selected database
            GlobalSettings.Clear();
            GlobalSettings.Load(database);
            SystemDatabase = database;

            STFDispatcherManager.DisconnectFromDispatcher(false);
        }

        /// <summary>
        /// Sets Login configuration for the user that is connecting.
        /// </summary>
        /// <returns></returns>
        private static bool SetLoginConfiguration()
        {
            using (MainFormLogOnDialog dialog = new MainFormLogOnDialog())
            {
                // Check to see if we should prepopulate the user name
                if (UserManager.UserLoggedIn)
                {
                    dialog.UserName = UserManager.CurrentUserName;
                }
                else if (IsUserRegistered(System.Environment.UserName))
                {
                    dialog.UserName = System.Environment.UserName;
                }

                while (true)
                {
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        // User clicked "Log In" - store entered credentials
                        if (SetLoginConfiguration(dialog.UserName, dialog.Password, dialog.Domain))
                        {
                            return true;
                        }
                    }
                    else if (result == DialogResult.Ignore)
                    {
                        // User chose "Login as Guest".  Use defaults - current user and guest permissions
                        UserManager.CurrentUser = new UserCredential(System.Environment.UserName, string.Empty, System.Environment.UserDomainName, UserRole.Guest);
                        return true;
                    }
                    else
                    {
                        // User canceled
                        return false;
                    }
                }
            }
        }

        private static bool SetLoginConfiguration(string userName, string password, string domain)
        {
            // Determine what the user's role is
            UserRole? role = GetUserRole(userName, domain);
            if (role.HasValue)
            {
                UserManager.CurrentUser = new UserCredential(userName, password, domain, role.Value);
                return true;
            }
            else
            {
                var notAuthResult = MessageBox.Show("User '{0}' is not authorized for STF operations.  \nContinue logging on with 'Guest' privileges?".FormatWith(UserManager.CurrentUserName), "Validate User", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (notAuthResult == DialogResult.OK)
                {
                    UserManager.CurrentUser = new UserCredential(userName, password, domain, UserRole.Guest);
                    return true;
                }
            }
            return false;
        }

        private static bool IsUserRegistered(string userName)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                return context.Users.Any(n => n.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }
        }

        private static UserRole? GetUserRole(string userName, string domain)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                if (domain != null)
                {
                    return User.Select(context, userName, domain)?.Role;
                }
                else
                {
                    return context.Users.FirstOrDefault(n => n.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))?.Role;
                }
            }
        }

        public static bool MultipleEnvironmentsAvailable()
        {
            return (GetAvailableSystems().Count > 1);
        }

        private static List<StfSystem> GetAvailableSystems()
        {
            NameValueCollection systems = ConfigurationManager.GetSection("Systems") as NameValueCollection;
            var result = systems.AllKeys.Select(x => new StfSystem { Name = x, Address = systems[x] }).ToList();
            return result;
        }

        private class StfSystem
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }
}

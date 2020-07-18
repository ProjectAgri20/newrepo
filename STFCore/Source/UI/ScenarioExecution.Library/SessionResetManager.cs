using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Manages the resetting of sessions.  Handles the display of the LogonDialog to get user credentials,
    /// then logs the username of the person who kicked off the reset operation.
    /// </summary>
    public class SessionResetManager
    {
        private bool _isAuthenticated = false;

        /// <summary>
        /// UserName of the Authorized user.
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// Domain of the Authorized user.
        /// </summary>
        public string Domain { get; private set; }


        /// <summary>
        /// Returns "Domain\UserName" string.
        /// </summary>
        public string DomainAndUser
        {
            get
            {
                StringBuilder result = new StringBuilder(Domain);
                result.Append("\\");
                result.Append(UserName);
                return result.ToString();
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="SessionResetManager"/> class.
        /// </summary>
        public SessionResetManager()
        {
            // For STB it is assumed the user shutting down is the same user that started the test.
            if (!GlobalSettings.IsDistributedSystem)
            {
                NetworkCredential user = UserManager.CurrentUser.ToNetworkCredential();

                UserName = user.UserName;
                Domain = user.Domain;
                _isAuthenticated = true;

                //UserName = "Jawa";
                //Domain = "ETL";
                //_isAuthenticated = true;
            }
        }

        /// <summary>
        /// Checks to see if the user is the owner of the passed-in session, or an admin.
        /// Will prompt for user credentials if not already authenticated for the domain and STF database
        /// </summary>
        /// <param name="sessionId">The session to check</param>
        /// <returns>Whether the user is authorized to reset the session.</returns>
        public bool IsAuthorized(string sessionId)
        {
            bool result = false;

            // Check if already authenticated
            if (!_isAuthenticated || string.IsNullOrEmpty(UserName))
            {
                if (!Authenticate())
                {
                    // simply return Authentication not successful, will already have displayed a message box
                    return result;
                }
            }

            // Check authorization for authenticated user
            if (_isAuthenticated)
            {
                User user = GetUser(UserName, Domain);
                if (user != null)
                {
                    result = IsOwnerOrManager(user, sessionId);
                }

                if (!result)
                {
                    string message = $"User '{DomainAndUser}' is not authorized for this operation.";
                    TraceFactory.Logger.Debug(message);
                    MessageBox.Show(message, "Authorize User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            return result;
        }

        /// <summary>
        /// Prompts for user credentials to authenticate against domain and STF database.
        /// Displays message box if user is not authenticated
        /// </summary>
        /// <returns><c>true</c> if user successfully authenticates, <c>false</c> if canceled or not authenticated.</returns>
        private bool Authenticate()
        {
            bool result = false;
            _isAuthenticated = false;
            using (MainFormLogOnDialog dialog = new MainFormLogOnDialog(true))
            {
                if (UserManager.UserLoggedIn)
                {
                    dialog.UserName = UserManager.CurrentUserName;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    User user = GetUser(dialog.UserName, dialog.Domain);
                    if (user != null)
                    {
                        UserName = user.UserName;
                        Domain = user.Domain;
                        _isAuthenticated = true;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("User '{0}' is not authorized.".FormatWith(dialog.UserName), "Validate User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            return result;
        }

        private User GetUser(string userName, string domain)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                return context.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && u.Domain.ToLower().StartsWith(domain.ToLower()));
            }
        }

        /// <summary>
        /// Inserts a ActivityExecution record in the STF Data Log database.
        /// Records the username and domain, host machine, Date/Time the reset occurred for the session.
        /// </summary>
        /// <param name="sessionId">The session Id</param>
        public void LogSessionReset(string sessionId)
        {
            if (string.IsNullOrEmpty(UserName))
            {
                TraceFactory.Logger.Debug("UserName not set.");
            }

            using (var context = DbConnect.DataLogContext())
            {
                SessionSummary session = context.DbSessions.FirstOrDefault(s => s.SessionId == sessionId);
                if (session != null)
                {
                    session.ShutdownUser = UserName;
                    session.ShutdownDateTime = DateTime.UtcNow;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Checks the specified user with the specified session.  If the user is the owner, OR the user is at least a manager, returns true.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sessionId"></param>
        /// <returns>true if the user is the owner of the session, OR the user is an admin.</returns>
        private bool IsOwnerOrManager(User user, string sessionId)
        {
            bool result = user != null;

            if (result)
            {
                using (DataLogContext context = DbConnect.DataLogContext())
                {
                    SessionInfo sessionSummary = context.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
                    if (sessionSummary != null)
                    {
                        result = (user.InRole(UserRole.Manager) || sessionSummary.Owner.Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase));
                    }
                    else
                    {
                        //Unable to get Owner information.  Return result based on role only.
                        result = user.InRole(UserRole.Manager);
                    }
                }
            }

            return result;
        }
    }
}

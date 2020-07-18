using HP.ScalableTest;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;

namespace HP.SolutionTest.WorkBench
{
    /// <summary>
    /// Configuration details used by the <see cref="SessionConfigurationWizard"/>.
    /// </summary>
    internal class WizardConfiguration
    {
        /// <summary>
        /// Gets the ticket.
        /// </summary>
        public SessionTicket Ticket { get; private set; }

        /// <summary>
        /// Gets the assets.
        /// </summary>
        public AssetDetailCollection SessionAssets { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardConfiguration"/> class.
        /// </summary>
        public WizardConfiguration()
        {
            // Add the user credentials to the ticket which will be used to interact with VCenter
            // for VM operations.  The assumption is that the user has a VCenter login with the same
            // username as their HP LR0 domain username.
            var credential = new UserCredential(UserManager.LoggedInUser.UserName, UserManager.LoggedInUser.Domain)
            {
                Role = UserManager.LoggedInUser.Role,
                Password = UserManager.LoggedInUser.Password
            };

            Ticket = new SessionTicket()
            {
                SessionOwner = credential
            };

            SessionAssets = new AssetDetailCollection();
        }
    }
}

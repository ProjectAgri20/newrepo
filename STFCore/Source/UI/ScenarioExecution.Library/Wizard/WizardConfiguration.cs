using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    /// <summary>
    /// Configuration details used by the <see cref="SessionConfigurationWizard"/>.
    /// </summary>
    public class WizardConfiguration
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
            Ticket = new SessionTicket()
            {
                SessionOwner = UserManager.CurrentUser
            };

            SessionAssets = new AssetDetailCollection();
        }
    }
}

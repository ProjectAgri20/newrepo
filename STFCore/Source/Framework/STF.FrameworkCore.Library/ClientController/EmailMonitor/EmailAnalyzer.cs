using System.Collections.Generic;
using HP.ScalableTest.Email;

namespace HP.ScalableTest.Framework.Automation.EmailMonitor
{
    internal class EmailAnalyzer : IEmailAnalyzer
    {
        public bool AnalyzeMessage(EmailMessage email)
        {
            return EmailTracker.IsTagged(email);
        }

        /// <summary>
        /// Process the specified email message.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <param name="controller"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ProcessMessage(EmailMessage emailMessage, IEmailController controller, string user)
        {
            TraceFactory.Logger.Debug("Cleaning up email message From:{0}   To:{1}".FormatWith(emailMessage.FromAddress.Address, user));
            //TODO: Add Logger support
            controller.Delete(emailMessage); //Clean up the email
            return true;
        }
    }
}

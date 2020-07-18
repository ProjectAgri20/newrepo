using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HP.ScalableTest.Email;

namespace HP.ScalableTest.Framework.Automation.EmailMonitor
{
    internal interface IEmailAnalyzer
    {
        bool AnalyzeMessage(EmailMessage message);
        bool ProcessMessage(EmailMessage message, IEmailController controller, string user);
    }
}

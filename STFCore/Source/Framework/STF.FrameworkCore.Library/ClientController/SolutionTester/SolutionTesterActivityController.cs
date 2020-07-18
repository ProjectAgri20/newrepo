using System;
using System.ServiceModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation
{
    [ObjectFactory(VirtualResourceType.SolutionTester)]
    public class SolutionTesterActivityController : OfficeWorkerActivityController
    {
        public SolutionTesterActivityController(string clientControllerHostName)
            : base(clientControllerHostName)
        {
        }

        protected override ServiceHost CreateServiceHost(int port)
        {
            TraceFactory.Logger.Debug("Creating endpoint");
            return VirtualResourceManagementConnection.CreateServiceHost("localhost", Guid.NewGuid().ToShortString());
        }
    }
}

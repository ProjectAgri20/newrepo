using HP.ScalableTest.Framework.Service;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    public class EPrintJobMonitorInstaller : FrameworkServiceInstaller
    {
                /// <summary>
        /// Initializes a new instance of the <see cref="EPrintJobMonitorInstaller"/> class.
        /// </summary>
        public EPrintJobMonitorInstaller()
            : base("EPrintJobMonitorService")
        {
            this.DisplayName = "ePrint Job Monitor Service";
            this.Description = "STF service for monitoring the status of jobs in the ePrint database.";
        }
    }
}

using System;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Automation.ActivityExecution;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.LoadTester
{
    public class LoadTestThread : IDisposable
    {
        private readonly Task _task = null;
        private readonly EngineBase _engine = null;

        public LoadTestThread(LoadTesterMetadataDetail metadataDetail, TimeSpan rampUpDelay)
        {
            _engine = ObjectFactory.Create<EngineBase>(metadataDetail.Plan.Mode, metadataDetail);
            _task = new Task(() => RunHandler(_engine, rampUpDelay));
        }

        public Task Task
        {
            get { return _task; }
        }

        public EngineBase Engine
        {
            get { return _engine; }
        }

        private void RunHandler(EngineBase engine, TimeSpan rampUpDelay)
        {
            LoadTesterActivityController.SetThreadName();

            TraceFactory.Logger.Debug("Pausing for {0} secs".FormatWith(rampUpDelay.TotalSeconds));
            ApplicationFlowControl.Instance.Wait(rampUpDelay);
            TraceFactory.Logger.Debug("Pausing for {0} secs - COMPLETE".FormatWith(rampUpDelay.TotalSeconds));

            engine.Run();

            TraceFactory.Logger.Debug("Engine run complete");
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_engine != null)
                {
                    _engine.Dispose();
                }

                if (_task != null)
                {
                    _task.Dispose();
                }
            }
        }

        #endregion IDisposable Members
    }
}

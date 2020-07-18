using System;
using System.ComponentModel;

namespace HP.SolutionTest.Install
{
    internal abstract class InstallerBase
    {
        private readonly ProgressEventArgs _progressArgs = null;

        protected BackgroundWorker Worker { get; private set; }

        public event EventHandler OnInstallationComplete;
        public event EventHandler OnCancellation;
        public event EventHandler<StatusEventArgs> OnStatusUpdate;
        public event EventHandler<StatusEventArgs> OnError;
        public event EventHandler<ProgressEventArgs> OnProgressUpdate;

        public InstallerBase()
        {
            Worker = new BackgroundWorker();
            Worker.WorkerSupportsCancellation = true;

            _progressArgs = new ProgressEventArgs();
        }

        /// <summary>
        /// Cancel the installation.
        /// </summary>
        public virtual void Cancel()
        {
            if (Worker.IsBusy)
            {
                Worker.CancelAsync();
            }
        }

        protected void SendComplete()
        {
            SystemTrace.Instance.Debug("Installer is complete");

            if (OnInstallationComplete != null)
            {
                OnInstallationComplete(this, new EventArgs());
            }
        }

        protected void SendError(Exception ex)
        {
            SendError(ex.Message, ex);
        }

        protected void SendError(string message)
        {
            SendError(message, null);
        }

        protected void SendError(string message, Exception ex)
        {
            string errorMessage = $"Error: {message}";
            SystemTrace.Instance.Error(errorMessage, ex);

            OnError?.Invoke(this, new StatusEventArgs(errorMessage));
        }

        protected void SendProgressStart(int total)
        {
            _progressArgs.State = ProgressState.Start;
            _progressArgs.Total = total;
            _progressArgs.Current = 0;

            OnProgressUpdate?.Invoke(this, _progressArgs);
        }

        protected void SendProgressUpdate(int current)
        {
            _progressArgs.State = ProgressState.Running;
            _progressArgs.Current = current;

            OnProgressUpdate?.Invoke(this, _progressArgs);
        }

        protected void SendProgressEnd()
        {
            _progressArgs.State = ProgressState.End;
            _progressArgs.Current = 0;

            OnProgressUpdate?.Invoke(this, _progressArgs);
        }

        protected void UpdateProgress(ProgressState state, int total, int current)
        {
            _progressArgs.State = state;
            _progressArgs.Total = total;
            _progressArgs.Current = current;

            OnProgressUpdate?.Invoke(this, _progressArgs);
        }

        protected void UpdateStatus(string message)
        {
            SystemTrace.Instance.Debug(message);

            OnStatusUpdate?.Invoke(this, new StatusEventArgs(message));
        }

        protected bool CancelPending()
        {
            if (Worker.CancellationPending)
            {
                SendCancellation();
                return true;
            }

            return false;
        }

        protected void SendCancellation()
        {
            if (OnCancellation != null)
            {
                UpdateStatus("Installation cancelled.");
                OnCancellation(this, new EventArgs());
            }
        }

    }
}

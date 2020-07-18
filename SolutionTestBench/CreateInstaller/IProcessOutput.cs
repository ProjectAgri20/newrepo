using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateInstaller
{
    public interface IProcessOutput
    {
        event EventHandler<InstallEventArgs> OnMessageUpdate;

        void Execute();

        void Cancel();

        void Dispose();

        string Label { get; }

        bool Processing { get; }

        string Configuration { get; }

    }

    public class InstallEventArgs : EventArgs
    {
        public InstallEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}

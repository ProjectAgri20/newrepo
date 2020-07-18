using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Virtualization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Currently not used, but could support a physical host used to 
    /// run resources as part of a session.
    /// </summary>
    [ObjectFactory(ManagedMachineType.WindowsPhysical)]
    public class WindowsMachinePhysical : HostMachine
    {
        private ManagedMachine _machine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsMachineVirtual" /> class.
        /// </summary>
        public WindowsMachinePhysical(ManagedMachine machine, SystemManifest manifest)
            : base(manifest)
        {
            _machine = machine;
        }

        public override string Name
        {
            get { return _machine.Name; }
        }

        public override void Validate()
        {
            var sender = new Ping();
            var options = new PingOptions();

            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            PingReply reply = sender.Send(_machine.Name, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                TraceFactory.Logger.Debug("{0} - RTT:{1} TTL:{2} BSZ:{3}"
                    .FormatWith(reply.Address, reply.RoundtripTime, reply.Options.Ttl, reply.Buffer.Length));
            }
            else
            {
                throw new InvalidOperationException("Unable to ping host {0}".FormatWith(_machine.Name));
            }
        }

        /// <summary>
        /// Sets up the machine which may involve booting, configuration, etc.
        /// </summary>
        public override void Setup()
        {
            var delimiter = new string[] { Environment.NewLine };
            var credential = GlobalSettings.Items.DomainAdminCredential;

            TraceFactory.Logger.Debug("Starting PHYSICAL machine " + _machine.Name);
            string dispatcher = Dns.GetHostEntry("").HostName;

            var setupCommand = Properties.Resources.WindowsClientSetup.FormatWith(dispatcher, Manifest.SessionId);

            // Split out the commands by line, then remove all the comments (start with "::")
            // and join them together with the && operator to make it a single statement
            var setupCommandList = setupCommand.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Remove all the comments and join the commands together with the && operator to make it a single statement
            setupCommandList.RemoveAll(n => n.StartsWith("::", StringComparison.Ordinal));

            List<string> commandList = new List<string>();
            commandList.Add(Properties.Resources.TaskKill.FormatWith("PrintMonitorService"));
            commandList.Add(Properties.Resources.TaskKill.FormatWith("ClientFactoryConsole"));
            commandList.Add(Properties.Resources.TaskKill.FormatWith("OfficeWorkerConsole"));
            commandList.AddRange(setupCommandList);

            string command = string.Join(" && ", commandList);

            RunCommand(command, credential);
        }

        public override void Shutdown(ShutdownOptions options)
        {
            var credential = GlobalSettings.Items.DomainAdminCredential;

            List<string> commands = new List<string>();
            commands.Add(Properties.Resources.TaskKill.FormatWith("PrintMonitorService"));
            commands.Add(Properties.Resources.TaskKill.FormatWith("ClientFactoryConsole"));
            string command = string.Join(" && ", commands);

            TraceFactory.Logger.Debug("Killing services on {0}".FormatWith(_machine.Name));
            RunCommand(command, credential);
        }

        private void RunCommand(string command, NetworkCredential credential)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var psExec = Path.Combine(directory, "psexec.exe");

            var args = @"-accepteula \\{0} -H -I -U {1}\{2} -P {3} CMD.EXE /C {4}"
                       .FormatWith(_machine.Name, credential.Domain, credential.UserName, credential.Password, command);

            TraceFactory.Logger.Debug(args);

            var processStart = new ProcessStartInfo(psExec, args)
            {
                UseShellExecute = false,
                ErrorDialog = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                var process = Process.Start(processStart);
                TraceFactory.Logger.Debug("Started PID {0} on {1}".FormatWith(process.Id, _machine.Name));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                throw;
            }
        }


        public override void Release()
        {
            _machine.ReleaseReservation();
        }
    }
}

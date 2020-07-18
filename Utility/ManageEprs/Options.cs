using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HP.Epr.Client;

namespace HP.ScalableTest.ManageEprs
{
	/// <summary>
	/// Provides an object to manage and present command line options
	/// </summary>
	class Options
	{

		public enum ToolActions
		{
			NoAction = 0,
			List = 1,
			ListUnused = 2,
			ListUsed = 3,
			Start = 4,
			Stop = 5,
			Snapshot = 6,
			Load = 7,
			Shutdown = 8
		};

        public string DEFAULT_LOGFILE = ".\\ManageEPRs.Log.txt";

		private readonly Dictionary<string, Protocol> protocolMap;

		public ToolActions Action { get; private set;}

		public ToolActions ListAction { get; private set;}

		public List<string> VMs;
		public string Profile;
		public int StartNum;
		public bool IsJedi;
		public bool IsOz;
		public bool IsForce;
		public bool TakeSnapshot;
		public bool UseSnapshot;
		public bool ForceStart;
        public bool AllowMix;
		public string Snapshot;
        public int WorkerThreads;
		public Collection<Protocol> EPRProtocols;

		/// <summary>
		/// Searches the command line for an action command. 
		/// </summary>
		/// <param name="pArgs">Command line arguments</param>
		private void findAction(string[] pArgs)
		{
			int _found = 0;
			Action = ToolActions.NoAction;
			int _ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Start", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				Action = ToolActions.Start;
				++_found;
			}
			_ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Stop", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				Action = ToolActions.Stop;
				++_found;
			}
			_ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Shutdown", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				Action = ToolActions.Shutdown;
				++_found;
			}
			_ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-SnapShot", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				TakeSnapshot = true;
			}
			_ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Load", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				Action = ToolActions.Load;
				++_found;
				UseSnapshot = true;
			}
			if (_found > 1)
			{
				throw new Exception("Too many actions listed on command line?");
			}
			if ((TakeSnapshot) || (UseSnapshot))
			{
				_ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Name", StringComparison.OrdinalIgnoreCase) >= 0);
				if ((_ndx >= 0) && (_ndx < (pArgs.Length - 1)))
				{
					Snapshot = pArgs[_ndx + 1];
				}
				else
				{
					throw new Exception("Missing filename for " + (TakeSnapshot ? "-Snapshot" : "-Load") + " command?");
				}
			}
		}

		/// <summary>
		/// Searches the command line for a List 'command'.  List commands can be appended to any action command
		/// </summary>
		/// <param name="pArgs">Command line args</param>
		private void findList(string[] pArgs)
		{
			ListAction = ToolActions.NoAction;
			int _ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-List", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				ListAction = ToolActions.List;
			}
			_ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-ListUnused", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				ListAction = ToolActions.ListUnused;
			}
			_ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-ListUsed", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				ListAction = ToolActions.ListUsed;
			}
		}

		/// <summary>
		/// Searches command line for VM specification (when the user wants to limit the action to specific VMs or IPs)
		/// </summary>
		/// <param name="pArgs">command line args</param>
		private void findVMs(string[] pArgs)
		{
			string[] _tokens;
			char[] _delims = {','};

			for (int _ndx = 0; _ndx < pArgs.Length - 1; _ndx++)
			{
				if (string.Compare(pArgs[_ndx], @"-VM", true) == 0)
				{
					_tokens = pArgs[++_ndx].Split(_delims);
					foreach (string _vm in _tokens)
					{
						VMs.Add(_vm);
					}
				}
			}
		}

		/// <summary>
		/// Loads the default VM file in the case where there were no VMs mentione onthe command line
		/// </summary>
		/// <param name="pPath">Path to default hosts file</param>
		private void defaultVMs(string pPath)
		{
			if (!File.Exists(pPath))
			{
				throw new Exception("Cannot find the EPR Hosts file that contains the default names for all EPR host VMs.");
			}
			string _buf;
			using (StreamReader _rdr = new StreamReader(pPath))
			{
				while((_buf = _rdr.ReadLine()) != null)
				{
                    if (!string.IsNullOrWhiteSpace(_buf))
                    {
					    VMs.Add(_buf);
                    }
				}
			}
		}
		
		/// <summary>
		/// Searches the command line for a profile option and the profile name or name fragment.
		/// Profiles will be resolved by checking for an exact match of the specified profile or
		/// looking for a fragment match (so that the user could specify 'M5035' or 'Kona' instead of
		/// 'Oz - HP LaserJet M5035 MFP (Kona)')
		/// </summary>
		/// <param name="pArgs">Command line args</param>
		private void findProfile(string[] pArgs)
		{
			int _ndx =  Array.FindIndex(pArgs, s => s.IndexOf(@"-Profile", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx < 0)
			{
				throw new Exception("-Profile <Profile> not found?");
			}
			else if (_ndx > (pArgs.Length - 1))
			{
				throw new Exception ("Unknown / Unspecified Profile ?");
			}
			else
			{
				Profile = pArgs[++_ndx];
			}
		}

		/// <summary>
		/// Searches the command line for a protocol option and the protocol names &/or starting fragments.
		/// Protocols will be resolved by checking for an exact match of the specified protocol or
		/// looking for a starting fragment match (so that the user could specify 'Web' instead of
		/// 'WebServices')
		/// </summary>
		/// <param name="pArgs">Command line args</param>
		private void findProtocols(string[] pArgs)
		{
			string[] _tokens;
			char[] _delims = { ',' };
			EPRProtocols = new Collection<Protocol>();
			string _pcol = "";
			int _ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Protocols", StringComparison.OrdinalIgnoreCase) >= 0);

			if ((_ndx >= 0) && (_ndx < (pArgs.Length - 1)))
			{
				try
				{
					_tokens = pArgs[++_ndx].Split(_delims);
					foreach (string _prot in _tokens) 
					{
						_pcol = _prot.Trim().ToUpper();
						EPRProtocols.Add(protocolMap.First(pm => pm.Key.Contains(_pcol)).Value);
					}
				}
				catch
				{
					throw new Exception("Unknown Protocol specified : " + _pcol);
				}
			}
			else if (IsJedi)
			{
				EPRProtocols.Add(Protocol.Ews);
				EPRProtocols.Add(Protocol.SNMP);
				EPRProtocols.Add(Protocol.WebServices);
				EPRProtocols.Add(Protocol.WSDiscovery);
			}
			else if (IsOz)
			{
				EPRProtocols.Add(Protocol.Ews);
				EPRProtocols.Add(Protocol.SNMP);
			}
		}

		/// <summary>
		/// Searches the command line for an architecture &/or the number of EPR instancs to start.
		/// The Architecture helps us order the search for the where to place EPRS (because there
		/// are specific VM Hosts that are mainly for Oz and others that are mainly for Jedi).
		/// </summary>
		/// <param name="pArgs">Command Line Args</param>
		private void findStartNum(string[] pArgs)
		{
			int _ndx =  Array.FindIndex(pArgs, s => s.IndexOf(@"-Jedi", StringComparison.OrdinalIgnoreCase) >= 0);
			if (_ndx >= 0)
			{
				IsJedi = true;
				StartNum = 255;		// DEFAULT NUmber on any one VM
			}
			else
			{
				_ndx = Array.FindIndex(pArgs, s =>s.IndexOf(@"-Oz", StringComparison.OrdinalIgnoreCase) >= 0);
				if (_ndx >= 0)
				{
					if (IsJedi)
					{
						throw new Exception("Cannot specify that a Profile is both a Jedi device and an Oz device...");
					}
					IsOz = true;
					StartNum = 765;			// DEFAULT number for OZ on one VM
				}
			}
			_ndx = Array.FindIndex(pArgs, s =>s.IndexOf(@"-Num", StringComparison.OrdinalIgnoreCase) >= 0);
			if ((_ndx >= 0) && (_ndx < (pArgs.Length - 1)))
			{
				StartNum = Convert.ToInt32(pArgs[++_ndx]);
			}
			if ((!IsJedi) && (!IsOz) && (StartNum == 0))
			{
				throw new Exception("Cannot determine how many EPRs to start because no platform (-Jedi or -Oz) was specified, nor was a number to start specified.");
			}
		}

        /// <summary>
        /// Sets the maximum number of worker threads that can be used.  If the blades are maxed out
        /// lowering the number of threads may prevent some timeout errors. If not specified on the
        /// command line, will default to the number of network drive letters that are supported.
        /// </summary>
        /// <param name="pArgs">List of command line arguments</param>
        private void SetWorkerThreads(string[] pArgs)
        {
            WorkerThreads = NetDrive.Instance.MaxThreads;
            int _ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Threads", StringComparison.OrdinalIgnoreCase) >= 0);
            if (_ndx >= 0)
            {
                try
                {
                    NetDrive.Instance.MaxThreads = (++_ndx) < pArgs.Length ? Convert.ToInt32(pArgs[_ndx]) : NetDrive.Instance.MaxThreads;
                }
                catch
                {
                    NetDrive.Instance.MaxThreads = NetDrive.Instance.MaxThreads;
                }
            }
        }


		/// <summary>
		/// Constructor.  Build the runtime environment that is based on the command line args.
		/// </summary>
		/// <param name="pArgs">Command Line Args</param>
		public Options(string[] pArgs)
		{
			protocolMap = new Dictionary<string,Protocol>()
				{
					{"EWS", Protocol.Ews},
					{"LEDM", Protocol.LEDM},
					{"SNMP", Protocol.SNMP},
					{"OXPD", Protocol.OXPd},
					{"WEBSERVICES", Protocol.WebServices},
					{"WSDISCOVERY", Protocol.WSDiscovery}
				};
			StartNum = 0;
			IsJedi = IsOz = IsForce = TakeSnapshot = UseSnapshot = false;
            GenUtils.LogFile = DEFAULT_LOGFILE;
            int _ndx = Array.FindIndex(pArgs, s => s.IndexOf(@"-Log", StringComparison.OrdinalIgnoreCase) >= 0);
            if (_ndx >= 0)
            {
                GenUtils.LogFile = (++_ndx) < pArgs.Length ? pArgs[_ndx] : DEFAULT_LOGFILE;
            }
            try
            {
                using (System.IO.StreamWriter _file = new System.IO.StreamWriter(GenUtils.LogFile, false))
                {
                    _file.Write("{0:yyyyMMddHHmmss.fff} : Start ManageEPRs", DateTime.Now);
                    foreach (string _token in pArgs)
                    {
                        _file.Write(" " + _token);
                    }
                    _file.WriteLine();
                }
            }
            catch (Exception _ex)
            {
                GenUtils.LogFile = null;
                GenUtils.Exclaim("Logging Disabled. Cannot Write to Log File :: " + _ex.Message);
            }
			findAction(pArgs);
			findList(pArgs);
			ForceStart = Array.FindIndex(pArgs, s => s.IndexOf(@"-Skip", StringComparison.OrdinalIgnoreCase) >= 0) < 0;
            AllowMix = Array.FindIndex(pArgs, s => s.IndexOf(@"-AllowMix", StringComparison.OrdinalIgnoreCase) >= 0) >= 0;
            SetWorkerThreads(pArgs);

            VMs = new List<string>();
			if (((Action == ToolActions.NoAction) && (ListAction == ToolActions.NoAction)) && (!TakeSnapshot))
			{
				throw new Exception("There are no valid actions specified?");
			}
			findVMs(pArgs);
			if (VMs.Count == 0)
			{
				GenUtils.Exclaim("No VMs specified. Will Load and utilize all default VMs.");
				defaultVMs(Program.HOSTSFILE);
			}
			if (Action == ToolActions.Start)
			{
				findProfile(pArgs);
				findStartNum(pArgs);
				IsForce = Array.FindIndex(pArgs, s => s.IndexOf(@"-Force", StringComparison.OrdinalIgnoreCase) >= 0) >= 0;
			}
			findProtocols(pArgs);
		}
	}
}

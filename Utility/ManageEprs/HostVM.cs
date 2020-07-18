using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using HP.Epr.Client;
using HP.ScalableTest.EndpointResponder;
using HP.ScalableTest.Network;

namespace HP.ScalableTest.ManageEprs
{
	/// <summary>
	/// Implements object that represents/manages a VM system that can host one or more EPRs.
	/// </summary>
	class HostVM
	{
        public static string DEFAULT_DOMAIN = ".etl.boi.rd.adapps.hp.com";
		public enum HostOperations
		{
			Idle = 0,
			Connect = 1,
			Start = 2,
			Stop = 3,
			Clone = 4
		};

		public enum HostVMStatus
		{
			Idle = 0,
			Pending = 1,
			Working = 2,
			Done = 3,
			Error = 4,
			NotConnected = 5
		};

		private string netName;
        private IPAddress mainIPAddress;
		private int ipsAvailable;
		private readonly List<HostOperations> workQ;
		private HostVMStatus workStatus;
		private List<EPRInstance> eprs;
		private readonly List<string> specifiedIPs;
		private Collection<DeviceProfile> availProfiles;
		private DeviceSimulatorChannel eprHostChannel;
		private Thread CommThread;
		private readonly bool forceConn;
		private int numToStart;
		private string startProfile;
		private Collection<Protocol> startProtocols;

		#region Properties
			/// <summary>
			/// Property for setting or getting the hast name (Net FQDN) of the VM that is hosting EPR's
			/// </summary>
			public string HostName
			{
				get
				{
					return netName;
				}
                set
                {
                    if (value.Contains('.'))
                    {
                        netName = value;
                    }
                    else
                    {
                        netName = value + DEFAULT_DOMAIN;
                    }
                }
			}

			/// <summary>
			/// Property returning (from the EPR Host) how many of the EPR's are available.
			/// Cannot be set except by the object itself.
			/// </summary>
			public int EPRInstancesAvailable
			{
				get
				{
					return specifiedIPs.Count == 0 ? ipsAvailable : specifiedIPs.Count;
				}
				private set
				{
					ipsAvailable = value;
				}	
			}

			/// <summary>
			/// Property returning (from the EPR Host) how many of the EPR's are in use. Cannot be publically set
			/// </summary>
//			public int EPRInstancesInUse
//			{
//				get
//				{
//					return ipsInUse;
//				}
//				private set
//				{
//					ipsInUse = value;
//				}
//			}

			/// <summary>
			/// Proerty returning the list of IP addresses that were specifically called out. Cannot be publically set.
			/// </summary>
			public List<string>SpecifiedIPs
			{
				get
				{
					return specifiedIPs;
				}
			}

			/// <summary>
			/// Property returning the list of all EPR instances (whether active or not)
			/// </summary>
			public List<EPRInstance> EPRs
			{
				get
				{
					return eprs;
				}
				set
				{
					eprs = value;
				}
			}

			/// <summary>
			/// Property returning the list of profiles that have been installed on the EPR Host.
			/// </summary>
			public Collection<DeviceProfile> InstalledProfiles
			{
				get
				{
					return availProfiles;
				}
				set
				{
					availProfiles = value;
				}
			}

			/// <summary>
			/// Property returning the WorkQ for this Host.  There is not "Set" property as once the 
			/// workQ is created at object instantiation, the queue may be emptied, but not destroyed
			/// </summary>
			public List<HostOperations> WorkQ
			{
				get
				{
					return workQ;
				}
			}

			/// <summary>
			/// Property that manages the state of the WorkQ operation.
			/// NOTE:  Other objects are allowed to manipulate the state of the WorkQ because this is
			/// not really a state machine.
			/// </summary>
			public HostVMStatus WorkStatus
			{
				get
				{
					return workStatus;
				}
				set
				{
					workStatus = value;
				}
			}

			/// <summary>
			/// Property that handles how many EPRs to start. This is the "old school" way of passing parameter
			/// into an MS Thread - which is through member variables.  Note that this property will not 
			/// allow itself to be set too high
			/// and will change the number based on how many EPRs can actually be started.
			/// </summary>
			public int EPRsToStart
			{
				get
				{
					return numToStart;
				}
				set
				{
					numToStart = value > EPRInstancesAvailable ? EPRInstancesAvailable : value;
				}
			}

			/// <summary>
			/// Property that handles what Profile should be started.  This is the  "old school" way of passing 
			/// parameter into an MS Thread - which is through member variables.
			/// </summary>
			public string ProfileToStart
			{
				get
				{
					return startProfile;
				}
				set
				{
					startProfile = value;
				}
			}

			/// <summary>
			/// Property that handles what Protocols should be used.  This is the  "old school" way of passing 
			/// parameter into an MS Thread - which is through member variables.
			/// </summary>
			public Collection<Protocol> ProtocolsToStart
			{
				get
				{
					return startProtocols;
				}
				set
				{
					startProtocols = value;
				}
			}
		#endregion

        /// <summary>
        /// Perform a remote opertaion with a set number of timed retries. This is an attempt to
        /// prevent simple network failures and micro-outages from causing havoc.
        /// </summary>
        /// <param name="pAction">The network action to perform</param>
        /// <param name="pWait">Amount of time to wait betweem requests</param>
        /// <param name="pRetries"># of tretries before we report the failure</param>
        private void RemOpWithTimedRetries(Action pAction, TimeSpan pWait, int pRetries)
        {
            if (pAction == null)
            {
                throw new Exception("Cannot attempt retries on a null Action?");
            }
            Exception _origException = null;
            for (int _attempt = 0; _attempt < pRetries; _attempt++)
            {
                try
                {
                    pAction();
                    return;
                }
                catch (Exception _ex)
                {
                    if (_origException == null)
                    {
                        _origException = _ex;
                    }
                    if (_attempt < pRetries)
                    {
                        Thread.Sleep(pWait);
                    }
                }
            }
            throw new Exception("Remote Action failed after " + pRetries.ToString() + " attempts:: " + _origException.Message);
        }

        /// <summary>
        /// Perform a remote opertaion with a set number of timed retries. This is an attempt to
        /// prevent simple network failures and micro-outages from causing havoc.
        /// </summary>
        /// <typeparam name="T">Result of the network operation (this is what the network operation returns if successful)</typeparam>
        /// <param name="pAction">network oepration to perform</param>
        /// <param name="pWait">how log to wait between attempts</param>
        /// <param name="pRetries">how many time to try</param>
        /// <returns>result of pAction</returns>
        private T RemOpWithTimedRetries<T>(Func<T> pAction, TimeSpan pWait, int pRetries)
        {
            if (pAction == null)
            {
                throw new Exception("Cannot attempt retries on a null Action?");
            }
            Exception _origException = null;
            for (int _attempt = 0; _attempt < pRetries; _attempt++)
            {
                try
                {
                    return pAction();
                }
                catch (Exception _ex)
                {
                    if (_origException == null)
                    {
                        _origException = _ex;
                    }
                    if (_attempt < pRetries)
                    {
                        Thread.Sleep(pWait);
                    }
                }
            }
            throw new Exception("Remote Action failed after " + pRetries.ToString() + " attempts:: " + _origException.Message);
        }



		/// <summary>
		/// ThreadStart method.  Executed in a worker thread to connect to the VM associated with this object,
		/// possibly start the EPR service and/or to pull information regarding the EPR's and profiles
		/// that this Host has.
		/// NOTE: Uses synchronized object of a Netowrk Drive Letter.
		/// </summary>
		private void Connect()
		{
			string _tnd = null;
			bool _connected = false;
            IPAddress[] _localIPs;

		//-------------------------------------------------------------------------------------------------------
		//Verify that the hostname is good.  if we can't find it via dns - then remove it from the list.
			try
			{
				_localIPs = Dns.GetHostAddresses(HostName);
                if (_localIPs.Length > 0)
                {
                    var _isOn = new System.Net.NetworkInformation.Ping();
                    if (_isOn.Send(HostName).Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        mainIPAddress = _localIPs[0];
                    }
                    else
                    {
    					WorkStatus = HostVMStatus.Error;
	    				GenUtils.HangError("Cannot connect with " + HostName + ". Make sure it is powered up." );
                        return;
                    }
                }
			}
			catch (Exception)
			{
				GenUtils.HangError(HostName + " is an unknown host?");
				WorkStatus = HostVMStatus.Error;
				return;
			}
		//-------------------------------------------------------------------------------------------------------
		// In order to connect, we will first assume that the EPR Service is actually running on the Host VM
		// and just attempt to establish communications.  If they fail for a timeout, then we will know that
		// we need to atempt to start the service on the remote host.
			try
			{
//				eprHostChannel = new DeviceSimulatorChannel(HostName);
				eprHostChannel = RemOpWithTimedRetries(() => (new DeviceSimulatorChannel(HostName)), TimeSpan.FromSeconds(3), 3);
				InstalledProfiles = eprHostChannel.IntalledProfiles;
				_connected = true;
			}
			catch (Exception _ex)
			{
			//-----------------------------------------------------------------------------------------------------
			// Landing here *usually* means that the EPR service is not running on the remote system.  So, we will
			// attempt to start the service. In order to start the remote service, we will check to see if the
			// remote dservice is running.  If it is, then there is prossibly a firewall blocking us.  If it is
			// not, then we check to see if the executable exists. If it does not, then the EPR is not installed
			// (or usable) on the remote system.  If the executable does exist, then start it up.
				if (((_ex.Message.Contains("timed out")) || _ex.Message.Contains("No endpoint", StringComparison.OrdinalIgnoreCase)) && (forceConn))
				{ 
					if (!GenUtils.CheckForRemoteProcess(HostName, Program.EPRSERVICE))
					{
						try 
						{	        
							_tnd = GenUtils.MapNetworkDrive(Path.Combine(@"\\", HostName, "C$"), @"ETL\jawa", "!QAZ2wsx");
							bool _canStart = File.Exists(Path.Combine(_tnd, Program.EPRSERVICEFILEPATH));
							GenUtils.DisconnectNetworkDrive(_tnd, true);
							_tnd = null;
							if(_canStart)
							{
								GenUtils.Exclaim("EPR Service is not running on " + HostName + ".  Attempting to start service...");
								GenUtils.ExecuteRemoteProcess(HostName, Path.Combine(@"C:\",Program.EPRSERVICEFILEPATH), true, false);
								eprHostChannel = new DeviceSimulatorChannel(HostName);
								InstalledProfiles = eprHostChannel.IntalledProfiles;
								_connected = true;
								GenUtils.Exclaim("EPR Service started on " + HostName);
							}
							else
							{
								GenUtils.HangError("Does not appear that the EPR Service is installed on " + HostName);
								WorkStatus = HostVMStatus.Error;
							}
						}
						catch (Exception _inner)
						{
							if (!string.IsNullOrEmpty(_tnd))
							{
								GenUtils.DisconnectNetworkDrive(_tnd, true);
								_tnd = null;
							}
							GenUtils.HangError("General network failure starting remote EPR service on  " + HostName + " :: " + _inner.Message);
							WorkStatus = HostVMStatus.Error;
						}
					}
					else
					{
						GenUtils.HangError("Could not connect with remote service on " + HostName + 
												". Verify the VM IP, and that it is powered on, or check for possible firewall blocking communications.");
						WorkStatus = HostVMStatus.Error;
					}
				}
				else if (forceConn)
				{
					WorkStatus = HostVMStatus.Error;
					GenUtils.HangError("General Network failure on " + HostName + " :: " + _ex.Message);
				}
				else
				{
					WorkStatus = HostVMStatus.NotConnected;
					GenUtils.HangError("Could not connect with " + HostName + " (-Skip specified). Host will be removed from Main Action list");
				}
			}
			//-----------------------------------------------------------------------------------------------------
			// If the connection is made, then pull all of the relevent information about the EPRs from the Host
			// VM.
			if (_connected)
			{
				try
				{
					EPRs.Clear();
					InstalledProfiles.Clear();
					InstalledProfiles = eprHostChannel.IntalledProfiles;
					foreach (IPAddress _ip in eprHostChannel.AvailableIPAddresses)
					{
//						if ((!_ip.ToString().EndsWith(".0")) && (HP.ScalableTest.Network.NetworkUtil.IsNonRoutableIpAddress(_ip)))
						if ((!_ip.ToString().EndsWith(".0")) && 
                                    (((_localIPs.Length == 1) && (HP.ScalableTest.Network.NetworkUtil.IsNonRoutableIpAddress(_ip))) ||
                                    ((_localIPs.Length > 1) && (!_ip.Equals(mainIPAddress)))))
						{
						//-----------------------------------------------------------------------------------------------
						// If the user did not specify any IP address on the command line 
						//     (format -vm Host/IP1/IP2/...,Host,Host/IP1/IP2...)
						// then we default to ALL of the IP addresses returned both in use and those that are available.
						// If the user DID supply some IP's - then everything on this Host must be limited to those IP's 
						// ONLY - so skip anything that is not on that list.
							if (specifiedIPs.Count == 0)
							{
								EPRs.Add(new EPRInstance(_ip));
							}
							else
							{
								if (specifiedIPs.Contains(_ip.ToString()))
								{
									EPRs.Add(new EPRInstance(_ip));
								}
							}
						}
					}
					EPRInstancesAvailable = eprHostChannel.AvailableIPAddresses.Count - 1;
					foreach(DeviceInstance _di in eprHostChannel.Instances)
					{
					//-----------------------------------------------------------------------------------------------
					// See the comment above... 
						if (specifiedIPs.Count == 0)
						{
							EPRs.Add(new EPRInstance(_di, true));
						}
						else
						{
							if (specifiedIPs.Contains(_di.Address.ToString()))
							{
								EPRs.Add(new EPRInstance(_di, true));
							}
						}
					}
					if (specifiedIPs.Count > 0)
					{
					//-------------------------------------------------------------------------------------------------
					// If the user specified some IP's for this host on the command line, those are the ONLY IPs that
					// can be operated on. So - check and see if they are valid.  Hang messages if not and if none 
					// are valid - then return an error so this host is removed from the Action list. This also
					// prevents us from having to check the IP validity anywhere else!
						int _sNdx = 0;
						do
						{
							for ( ; _sNdx < specifiedIPs.Count; _sNdx++)
							{
								string _ip = specifiedIPs[_sNdx];
								if (EPRs.FindIndex(_inst => string.Compare(_ip, _inst.IPAddress.ToString()) == 0) < 0)
								{
									GenUtils.Exclaim(_ip + " is not a valid IP for host " + _ip + ". Removing from the IP List");
									break;
								}
							}
							if (_sNdx < specifiedIPs.Count)
							{
								specifiedIPs.RemoveAt(_sNdx);
							}
						}
						while (_sNdx < specifiedIPs.Count);
						WorkStatus = specifiedIPs.Count > 0 ? HostVMStatus.Done : HostVMStatus.Error;
					}
					else
					{						
						WorkStatus = HostVMStatus.Done;
					}
				}
				catch (Exception _ex)
				{
					WorkStatus = HostVMStatus.Error;
					GenUtils.HangError(HostName + " : Connection Exception caught: " + _ex.Message);
					GenUtils.DisconnectNetworkDrive(_tnd, true);
				}
			}
		}

/*			
			private string getInstalledVersion(string pHost, string pNetDrive)
			{
				string _curRel = "V0.0.0";
				string _eprPath = pNetDrive + "\\Users\\jawa\\desktop\\EPR";

				if (!Directory.Exists(_eprPath))
				{
					throw new Exception("Expected EPR home directory not found on this VM");
				}
				var _dirList = Directory.GetDirectories(_eprPath, "V*",	SearchOption.TopDirectoryOnly);
				foreach (string _dir in _dirList)
				{
					if (string.Compare(_dir, _curRel, true) > 0)
					{
						_curRel = _dir;
					}
				}
				return Path.GetFileName(_curRel);
			}
**/

		/// <summary>
		/// Used to parse the sepcifid path and pull out any IP addresses that were on the
		///	command line.  Format: -VM Host/IP/IP/IP...
		/// </summary>
		/// <param name="pNetPath">Host portion that may or may not contain IP addresses.</param>
		private void processNetPath(string pNetPath)
		{
			string[] _tokens;
			char[] _delims = { '/' };

			_tokens = pNetPath.Split(_delims);
			HostName = _tokens[0];
			for (int _ndx = 1; _ndx < _tokens.Length; _ndx++)
			{
				specifiedIPs.Add(_tokens[_ndx].Trim());
			}
		}

		/// <summary>
		/// ThreadStart method.  Will manage the tearing down of one or more EPR instancs on this Host VM.
		/// This is executed in a thread - even though the operation itself is fairly quick.
		/// </summary>
		private void destroy()
		{
		//-------------------------------------------------------------------------------------------------------
		// Follows use rule - if no IP addresses have been specifically specified on the command line, then
		// operation will affect ALL EPR IP's on this host.
			try
			{
				if (specifiedIPs.Count == 0)
				{
					RemOpWithTimedRetries(() => eprHostChannel.DestroyAllInstances(), TimeSpan.FromSeconds(3), 3);
				}
				else
				{
					foreach (EPRInstance _epr in EPRs)
					{
						RemOpWithTimedRetries(() => eprHostChannel.DestroyInstance(_epr.IPAddress), TimeSpan.FromSeconds(3), 3);
					}
				}
			//-----------------------------------------------------------------------------------------------------
			// If IP's have been specified, it is probably only a few (< 5) so running this loop twice is not
			// terrible.  If no IPs were specified, we would have to do this anyhow.
				foreach (EPRInstance _epr in EPRs)
				{
					_epr.ClearDevice();
				}
				WorkStatus = HostVMStatus.Done;
			}
			catch (Exception _ex)
			{
				GenUtils.HangError("Error when destroying EPRs : " + _ex.Message);
				WorkStatus = HostVMStatus.Error;
			}
		}

		/// <summary>
		/// ThreadStart method.  manages starting one or more EPRs on this host. All needed parameters are
		/// passed through member variables (old school) - vis numToStart, startProfile and startProtocols.
		/// </summary>
		private void startEPRs()
		{
			try
			{ 
				int _started = numToStart;
				for (int _ndx = 0; (_ndx < EPRs.Count) && (_started > 0); _ndx++)
				{
					if (!EPRs[_ndx].InUse)
					{ 
						DeviceInstance _di = RemOpWithTimedRetries(() => eprHostChannel.CreateInstance(startProfile, EPRs[_ndx].IPAddress, startProtocols), TimeSpan.FromSeconds(3), 3);
						--_started;
						EPRs[_ndx].ResetDevice(_di);
					}
					else
					{
						GenUtils.HangError(EPRs[_ndx].HostName + " is already in use as " + EPRs[_ndx].Profile + ". Start command ignored for this EPR");
					}
				}
				WorkStatus = HostVMStatus.Done;
			}
			catch (Exception _ex)
			{
				GenUtils.HangError("Encountered error whie starting EPRs on " + HostName + " :: " + _ex.Message);
				WorkStatus = HostVMStatus.Error;
			}
		}

		/// <summary>
		/// ThreadStart method. Will clone all of the EPR's in the EPR list back tot he Host.  
		/// This is a special case, set up by Loading a snapshot - - where the EPR List will 
		/// contain what the EPR's *SHOULD BE*, rather than	what they actually are.
		/// </summary>
		private void clone()
		{
			try
			{
				eprHostChannel = RemOpWithTimedRetries(() => (new DeviceSimulatorChannel(HostName)), TimeSpan.FromSeconds(3), 3);
			    try
			    { 
				    foreach (EPRInstance _epr in EPRs)
				    {
					    if (string.Compare(_epr.Profile, EPRInstance.NOPROFILE) != 0)
					    {
						    DeviceInstance _di = RemOpWithTimedRetries(() => eprHostChannel.CreateInstance(_epr.Profile, _epr.IPAddress, _epr.Protocols), TimeSpan.FromSeconds(3), 3);
						    _epr.ResetDevice(_di);
				    	}
				    }
				    WorkStatus = HostVMStatus.Done;
			    }
			    catch (Exception _ex)
			    {
				    GenUtils.HangError("Encountered error while cloning EPR " + HostName + " :: " + _ex.Message);
				    WorkStatus = HostVMStatus.Error;
			    }
			}
			catch (Exception _ex)
			{
				GenUtils.HangError("Encountered error while connect (for cloning) to EPR " + HostName + " :: " + _ex.Message);
				WorkStatus = HostVMStatus.Error;
			}
		}

		/// <summary>
		/// Will return a profile name based on either a matching string or a partial match,
		/// If the profile is supported by the Host.
		/// </summary>
		/// <param name="pTarget">Profile (fullname or fragment) to find</param>
		/// <returns>Actual profile name or null if not found</returns>
		public string FindProfile(string pTarget)
		{
			try
			{ 
				return InstalledProfiles.FirstOrDefault(_prof => ((string.Compare(_prof.Name, pTarget, true) == 0) || (_prof.Name.Contains(pTarget)))).Name;
			}
			catch (Exception)
			{
				GenUtils.HangError(pTarget + " Cannot be resolved into an installed profile on " + HostName + "?");
				return null;
			}
		}

		/// <summary>
		/// Dispatcher method for using threading to accomplish work that must happen on the actual (remote) Host.
		/// Using threads to increase efficiency and reduce busy waits.  All work that requires communication
		/// with the remote host, is done on a worker thread.  Each Host has a unique thread, but the threads
		/// are throttled from a system wide standpoint, based on the number of available network drive letters.
		/// </summary>
		public void DoWork()
		{
			if (workQ.Count > 0)
			{
				WorkStatus = HostVMStatus.Working;
				HostOperations _op = workQ[0];
				workQ.RemoveAt(0);
				switch (_op)
				{
					case HostOperations.Connect:
						CommThread = new Thread(Connect);
						break;
					case HostOperations.Start:
						CommThread = new Thread(startEPRs);
						break;
					case HostOperations.Stop:
						CommThread = new Thread(destroy);
						break;
					case HostOperations.Clone:
						CommThread = new Thread(clone);
						break;
					default:
						throw new Exception("Unknown HostOperation in DoWork: " + (int)_op);
				}
				CommThread.Start();
			}
		}

		/// <summary>
		/// Constructor. Just does LOCAL initialization.  No remote communication happens in here.
		/// </summary>
		/// <param name="pNetPath">network Path to the Remote host</param>
		/// <param name="pWork">Initial operation -- usually a Connect</param>
		/// <param name="pForce">Whether to "force" a connection.  Menas to start the remote EPR service if it is not executing</param>
		public HostVM(string pNetPath, HostOperations pWork, bool pForce)
		{
			specifiedIPs = new List<string>();
			workQ = new List<HostOperations>();
			EPRs = new List<EPRInstance>();
			availProfiles = new Collection<DeviceProfile>();
			processNetPath(pNetPath);	
			forceConn = pForce;
			if (pWork != HostOperations.Idle)
			{
				workQ.Add(pWork);
			}
			workStatus = WorkQ.Count > 0 ? HostVMStatus.Pending : HostVMStatus.Idle;
			CommThread = null;

		}
	}
}

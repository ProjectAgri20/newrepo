using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using System.IO;
using System.Threading;

namespace HP.ScalableTest.ManageEprs
{
	class Program
	{

		public const string HOSTSFILE= ".\\EPRHosts.txt";  // Use a file to identify the EPR systems right now.....
		public const string EPRCLIENTUI = "HP.Epr.ClientUI.exe";
		public const string EPRSERVICE = "EndPointResponderService.exe";
		public const string EPRSERVICEFILEPATH = @"Program Files (x86)\HP\HP End Point Responder\EndPointResponderService.exe";

		public enum EPRReports
		{
			List = 0,
			Unused = 1,
			Used = 2
		};

		/// <summary>
		/// MainLine.  Enter here.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			List<HostVM> Hosts = new List<HostVM>();
			Options _opts = null;

			try
			{
				_opts = new Options(args);
			}
			catch(Exception _ex)
			{
				GenUtils.HangError(_ex.Message);
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.WriteLine("Usage:\nManageEPRs");
				Console.WriteLine("  -Start <Options>  -- Start EPR instances. Options are explained below");
				Console.WriteLine("    -VM \"list of vms to use\"  -- Optional. EPR_FQDN,EPR_FQDN....");
				Console.WriteLine("          Can also specify a specific VM and IP - EPR_FQDN/IPAddr");
				Console.WriteLine("    -Profile \"<Profile Name>\" -- Required. Name of profile to start");
				Console.WriteLine("    -Jedi | -Oz -- Required. Must know architecture to load balance EPRS.");
				Console.WriteLine("    -Num #EPRsTostart -- Optional. -Jedi will start 255, -Oz will start 765");
				Console.WriteLine("    -Protocols \"Comma Separated List of Protocols to support\"");
				Console.WriteLine("  -Stop <Options>  -- Stop EPR instances. Options are explained below");
				Console.WriteLine("    -VM \"list of vms to use\"  -- Optional. EPR_FQDN,EPR_FQDN....");
				Console.WriteLine("          Can also specify a specific VM and IP - EPR_FQDN/IPAddr");
				Console.WriteLine("  -Shutdown <Options>  -- Stops all EPR instances and halts the EPR service.");
				Console.WriteLine("    -VM \"list of vms to use\"  -- Optional. EPR_FQDN,EPR_FQDN....");
				Console.WriteLine("    NOTE: This functionality is not yet implemented...");
				Console.WriteLine("  -Snapshot <Options>  -- Create snapshot for recreating a configuration.");
				Console.WriteLine("    -Name \"SnapshotFileName\"  -- Required. Snapshot filename (for Replay)");
				Console.WriteLine("    -VM \"list of vms to use\"  -- Optional. EPR_FQDN,EPR_FQDN....");
				Console.WriteLine("  -Load <Options>  -- Load a snapshot to recreate it's environment.");
				Console.WriteLine("    -Name \"SnapshotFileName\"  -- Required. Snapshot filename (to load)");
				Console.WriteLine("    -VM \"list of vms to use\"  -- Optional. EPR_FQDN,EPR_FQDN....");
				Console.WriteLine("  -List -- Will list all EPRs (used and unused) on all VMs specified");
				Console.WriteLine("  -ListUsed -- Will list Used EPRs on all VMs specified");
				Console.WriteLine("  -ListUnused -- Will list Unused EPRs on all VMs specified\n");
                Console.WriteLine("  -Log \"LogFile\"  - Change the name of the default log file to 'LogFile'.");
                Console.WriteLine("        Default log file is named .\\ManageEPRs.Log.txt and is always created.");
                Console.WriteLine("  -Threads # -- Sets the maximum number of threads to use to #. # <= 10.");
				Console.WriteLine("NOTES:");
				Console.WriteLine("  -List, -ListUsed or -ListUnused can be used with -Start, -Stop or -Shutdown");
				Console.WriteLine("  If -VM is NOT specified, then ALL EPR Host VM's will be contacted");
				Console.WriteLine("    and operated on.  Be careful with -Stop and -Shutdown!!");
				Console.WriteLine("  If a -Start request cannot be satisfied with the VMs specified,");
				Console.WriteLine("    or with all VMs, then as many EPRS as can be started will be started.");
				Console.WriteLine("\n\nPress Enter to exit.");
				Console.ResetColor();
				Console.ReadLine();
				Environment.Exit(1);
			}

		//-------------------------------------------------------------------------------------------------------
		// Create the initial ist of Host VM's to use.  This will just create the initial objects - it does not
		// attempt to communicate with the VM's yet.  that will be done through the thread dispatcher bcause
		// the communication process can be cumebrsome and time consuming so we wat to multi-thread it.
			foreach(string _host in _opts.VMs)
			{
				Hosts.Add(new HostVM(_host, HostVM.HostOperations.Connect, _opts.ForceStart));
			}

		//---------------------------------------------------------------------------------------------------------
		// Now, we are ready to start communicating with the VM's and build the info from each viable VM.  If
		// -Skip was included on the command line, then we will not force the EPRService to start on the VM if
		// if it is not already running.  VM's that cannot be communicated with will be removed from the Hosts
		// list.  Also, VM's that had invalid IP addresses specified will also be removed from the Hosts list.
		// When the execution of this segment is complete, Hosts will contain all VM's to be used in this session.
            GenUtils.Exclaim("Contacting EPR Host Vm's.  This can take a few minutes....");
			try
			{
				Dispatch(Hosts);
			}
			catch (Exception _ex)
			{
                GenUtils.HangError("Error during initial communication - " + _ex.Message);
				Environment.Exit(5);
			}

		//---------------------------------------------------------------------------------------------------------
		// Some of the operations will require further processing of the Hosts list. For example, if we are
		// starting EPR's then we have to find the correct Hosts to use based on several things such as number,
		// profiles and specified ip addresses. Once we are done with that, we can dispatch all of the operations
		// together and let the thread dispatcher handle not over-taxing the system.
			switch (_opts.Action)
			{
				case Options.ToolActions.Start:
					FitEPRs(Hosts, _opts);
					break;

				case Options.ToolActions.Stop:
					foreach (HostVM _hst in Hosts)
					{
						_hst.WorkQ.Add(HostVM.HostOperations.Stop);
						_hst.WorkStatus = HostVM.HostVMStatus.Pending;
					}
					break;

				case Options.ToolActions.Shutdown:
                    GenUtils.HangError("Sorry.  This feature has not yet been implemented.");
					break;

				case Options.ToolActions.Load:
					LoadSnapShot(Hosts, _opts.Snapshot);
					break;

				case Options.ToolActions.NoAction:
					break;
			}
			if (_opts.Action != Options.ToolActions.NoAction)
			{
				if (Hosts.Count > 0)
				{
					Dispatch(Hosts);
				}
				else
				{
                    GenUtils.HangError("There are no VM's that can fulfill the request?");
					Environment.Exit(7);
				}
			}

		//---------------------------------------------------------------------------------------------------------
		// These are "non-action" commands because they do not make any changes to the VM's ot EPR instances.
			if (_opts.TakeSnapshot)
			{
				CaptureSnapshot(Hosts, _opts.Snapshot);
			}
			if (_opts.ListAction != Options.ToolActions.NoAction)
			{
				Report(_opts.ListAction, Hosts);
			}
		}


		/// <summary>
		/// Manages operations we want to perform on Host VM's.  Each VM utilizes a separate
		/// thread for the operation - but we want to be careful we do not get so many threads 
		/// active that we just slow to a crawl or crash because we are out of resources.  
		/// </summary>
		/// <param name="pHosts">List of Host VM's to use for the operation</param>
		/// <param name="pWait">Flag indicating to Wait until all threads complete. (Default = true)</param>
		static void Dispatch(List<HostVM>pHosts, bool pWait = true)
		{
			int _threadsAvailable = NetDrive.Instance.MaxThreads;
			bool _done;
			bool _allStarted;
			int _ndx;
			int _removeNdx;
			do
			{
			//-------------------------------------------------------------------------------------------------------
			// We may have to traverse the inner loop multiple times - either if we run out of threads &/Or if we
			// are supposed to wait here until all threads complete. Reset )removeNdx so that we know not to remove]
			// any of the hosts.
				_done = _allStarted = true;
				_removeNdx = -1;
				for (_ndx = 0; _ndx < pHosts.Count; _ndx++)
				{
					HostVM _vm = pHosts[_ndx];
				//-----------------------------------------------------------------------------------------------------
				// The Workstatus field will tell us what we want to know.  Idle means there is nothing for the VM 
				// Object to do.  Pending means that there is an operation waiting to be executed (the normal state).
				// Working means that the thread is currently active.  Done means that the thread has completed its
				// task.
					switch (_vm.WorkStatus)
					{
					//---------------------------------------------------------------------------------------------------
					// If the work status is pending, then the WorkQ has one or more items on it that need to be started.
					// In order to start an operation, we must be under the thread threshold. if we are at the threshold
					// then we just skip it and try again next time through.  Because we set _allStarted to false, the
					// Loop will not exit.
						case HostVM.HostVMStatus.Pending:
							_allStarted = _done = false;
							if (_threadsAvailable > 0)
							{
								--_threadsAvailable;
                                GenUtils.Exclaim("Starting thread for " + _vm.WorkQ[0].ToString() + " on " + _vm.HostName);
								_vm.DoWork();
							}
							break;

					//---------------------------------------------------------------------------------------------------
					// If the VM has a thread running, then we are not _done (if that matters) - so just set _done to
					// false.
						case HostVM.HostVMStatus.Working:
							_done = false;
							break;

					//---------------------------------------------------------------------------------------------------
					// If this VM's thread has completed, then we can return that thread (logically) to the available
					// pool and potentially use it to start other threads.  We change the WorkStatus here ebcause this
					// is where the threads are managed from.
						case HostVM.HostVMStatus.Done:
							_vm.WorkStatus = HostVM.HostVMStatus.Idle;
							++_threadsAvailable;
							GenUtils.Exclaim(_vm.HostName + " worker thread has finished" );
							break;

					//---------------------------------------------------------------------------------------------------
					// Idle is the state that means that the VM has nothing on its workQ.
						case HostVM.HostVMStatus.Idle:
							break;

					//---------------------------------------------------------------------------------------------------
					// NotConnected means that we can not use this VM.  We were either unable to find it (perhaps it is
					// powered down, or the Hostname was incorrect...) or we could not find/start the EPR Service.  When
					// this happens, we have to remove the VM from out Host List because we cannot use it.
						case HostVM.HostVMStatus.NotConnected:
						case HostVM.HostVMStatus.Error:
							_removeNdx = _ndx;
							_allStarted = false;
							GenUtils.Exclaim(_vm.HostName + " is being removed from the use list.");
							break;
					}
					if (_removeNdx >= 0)
					{
					// This is a bit of guerilla code -- we need to remove this VM from the list - but we can't remove
					// it from within the for loop or the handler will die.  So - we have to trap it here, then break 
					// out of the for loop and then trap it again so we can remove it. Sigh.  Sometimes I REALLY miss c++
						break;
					}
				}
			//-----------------------------------------------------------------------------------------------------
			// If there is still stuff to do, and we arrive here, then sleep for a second to allow some other
			// work to happen.  Might be better to sleep for 2 or 3 seconds?
				if ((!_allStarted) || ((pWait) && (!_done)))
				{
					if (_removeNdx >= 0)
					{
						pHosts.RemoveAt(_removeNdx);
					}
					Thread.Sleep(1000);
				}
			}
			while ((!_allStarted) || ((pWait) && (!_done)));

			if (pHosts.Count == 0)
			{
				throw new Exception("There are no VM's that can be used to satisfy the request");
			}
		}


		/// <summary>
		/// Finds open VM's/IP addresses for EPRS.  Will follow these placement rules:  
		/// (1) Use targeted VM's whenever possible (for Jedi and Oz - #of network connections)
		/// (2) Use small pools first to reduce fragmentation
		/// (3) Start as many EPRs as possible - notify if # cannot be fully satisfied
		/// </summary>
		/// <param name="pHosts">List of VM's to use</param>
		/// <param name="pOpts">Options form the command line</param>
		/// <returns>true if EPRs started, false if there was a problem</returns>
		static bool FitEPRs(List<HostVM> pHosts, Options pOpts)
		{
			List<int> _sorted = new List<int>();
			int _hostNdx;
			int _sortNdx;
			bool _inserted;
			string _targetVMIdentifier = pOpts.IsJedi ? "255" : "765";
			string _foreignVMIdentifier = pOpts.IsJedi ? "765" : "255";
			int _numEPRs = pOpts.StartNum > 0 ? pOpts.StartNum : pOpts.IsOz ? 765 : 255;
			
		//---------------------------------------------------------------------------------------------------------
		// First thing - remove all VM's from the list that either do not have any EPR instancs available, or
		// do not support the profile that was requested.
			_hostNdx = 0;
            bool _good2Use = false;
			do
			{
				for ( ; _hostNdx < pHosts.Count; _hostNdx++)
				{
				//-----------------------------------------------------------------------------------------------------
				// Because we allow the use of a Partial string for the Profile name, we need to resolve what was
				// specified on the command line to a real profile (if it exists).  So - resolve the profile.
                    _good2Use = true;
					string _tProf = pHosts[_hostNdx].FindProfile(pOpts.Profile);
					if ((pHosts[_hostNdx].EPRInstancesAvailable == 0) || (string.IsNullOrEmpty(_tProf)))
					{
                        _good2Use = false;
						break;
					}
                    if (!pOpts.AllowMix)
                    {
                        foreach (EPRInstance _locEpr in pHosts[_hostNdx].EPRs.Where(_inst => _inst.WasAlreadyExecuting))
                        {
//                            if ((_locEpr.WasAlreadyExecuting) && (string.Compare(_locEpr.Profile, _tProf, true) != 0))
                            if (string.Compare(_locEpr.Profile, _tProf, true) != 0)
                            {
                                _good2Use = false;
                                break;
                            }
                        }
                    }
                    if (!_good2Use)
                    {
                        break;
                    }
                    pHosts[_hostNdx].ProfileToStart = _tProf;
				}
				if (!_good2Use)
				{
					GenUtils.Exclaim("Removing VM " + pHosts[_hostNdx].HostName + " because it does not match requirements.");
					pHosts.RemoveAt(_hostNdx);
				}
			} 
			while (_hostNdx < pHosts.Count);
			if (pHosts.Count == 0)
			{
				GenUtils.HangError("There are no VM's that match the requirements ?");
				return false;
			}
	
		//---------------------------------------------------------------------------------------------------------
		// Sort the VM's that *could* be used according to the following rules:
		// 1 - The VM's that are "tailored" for the specified architecture, should be first in the list
		// 2 - The VM's with the fewest available EPR's should be before those with more available EPRS (to reduce
		//				fragmentation
			for(_hostNdx = 0; _hostNdx < pHosts.Count; _hostNdx++)
			{
				_inserted = false;
				for (_sortNdx = 0; _sortNdx < _sorted.Count; _sortNdx++)
				{
				//-----------------------------------------------------------------------------------------------------
				// Check for the case where what is on the front of the sorted list is NOT the targeted VM - and what 
				// is being placed now is a targeted VM - then put the targeted VM in the first spot  regardless of
				// how many available EPRs it has.
					if ((pHosts[_hostNdx].HostName.Contains(_targetVMIdentifier)) && (pHosts[_sorted[_sortNdx]].HostName.Contains(_foreignVMIdentifier)))
					{
						_inserted = true;
						_sorted.Insert(_sortNdx, _hostNdx);
						break;
				//-----------------------------------------------------------------------------------------------------
				// Ok - if the current Vm is the same tagretted platform as the one in the sorted list, then compare
				// the numebr of available VM's - and sort them smallest to greatest.
					} else if (((pHosts[_hostNdx].HostName.Contains(_targetVMIdentifier)) && (pHosts[_sorted[_sortNdx]].HostName.Contains(_targetVMIdentifier))) ||
											((pHosts[_hostNdx].HostName.Contains(_foreignVMIdentifier)) && (pHosts[_sorted[_sortNdx]].HostName.Contains(_foreignVMIdentifier))))
					{
						if (pHosts[_hostNdx].EPRInstancesAvailable <= pHosts[_sorted[_sortNdx]].EPRInstancesAvailable)
						{ 
							_inserted = true;
							_sorted.Insert(_sortNdx, _hostNdx);
							break;
						}
					}
				}
			//-------------------------------------------------------------------------------------------------------
			// It may not be obvious, BUT if we reach here and have not yet inserted the current Host, then it has to
			// go at the end of the _sorted list.
				if (!_inserted)
				{
					_sorted.Add(_hostNdx);
				}
			}

			// Now that we have the VM's sorted such that the VM's with the fewest available EPRs are first
			// in the list, we can start to use those VM's to bring up the EPRs requested. This is a cross between a first fit
			// and a best fit algorithm - and it should result in reducing "fragmentation" of EPR usage across Host VMs.
			_hostNdx = 0;
			int _cnt = _sorted.Count;
			do
			{
			//-------------------------------------------------------------------------------------------------------
			// The VM will not allow us to assign more EPR's to start than it has available - so, we can just assign
			// how many we need and then subtract what VM reorts is the number we can start.
				HostVM _vm = pHosts[_sorted[_hostNdx]];
				_vm.EPRsToStart = _numEPRs;
				_vm.ProtocolsToStart = pOpts.EPRProtocols;
				_vm.WorkQ.Add(HostVM.HostOperations.Start);
				_vm.WorkStatus = HostVM.HostVMStatus.Pending;
				_numEPRs -= _vm.EPRsToStart;
				++_hostNdx;										
			}
			while ((_numEPRs > 0) && (_hostNdx < _cnt));

			if (_numEPRs > 0)
			{
				GenUtils.Exclaim("There are not enough available EPR instances. Only " + (pOpts.StartNum - _numEPRs).ToString() + 
										" will be started!\n\n");
			}
			return true;
		}

		/// <summary>
		/// Will capture a snapshot of the specified (or default) VMs.  This includes what each
		/// EPRInstance is doing - and then that information is recorded to an xml file so that it
		/// can be re-loaded (-Load option).
		/// </summary>
		/// <param name="pHosts">List of VM's to snapshot</param>
		/// <param name="pSnap">Name of the snapshot file</param>
		static void CaptureSnapshot(List<HostVM> pHosts, string pSnap)
		{

		//---------------------------------------------------------------------------------------------------------
		// We want the filename to always use .xml as an extension.  So - if the filename does not end in .xml - 
		// append .xml to the filename.  Then, if the file exists, just delete it.  If we need to have a dialog
		// confirming the overwrite,  will add it later.
			string _extension = Path.GetExtension(pSnap);
			if (string.Compare(_extension, @".xml", true) != 0)
			{
				GenUtils.Exclaim("Snapshot file name must use .xml extension. Appending .xml to " + pSnap);
				pSnap += ".xml";
			}
			if (File.Exists(pSnap))
			{
				File.Delete(pSnap);
			}

		//---------------------------------------------------------------------------------------------------------
		// This is pretty straightforward here.  Just build the file and write it.  This way is not as elegent,
		// but has other advantages - such as being very clear what we are doing and why.
			using (StreamWriter _ss = new StreamWriter(pSnap))
			{
				_ss.WriteLine("<EPRSnapShot>");
				foreach (HostVM _vm in pHosts)
				{
//					if(_vm.Ready)
//					{
						_ss.WriteLine("<EPRHost HostName=\"" + _vm.HostName + "\">");
						foreach (EPRInstance _epr in _vm.EPRs)
						{
							if (_epr.InUse)
							{
								_ss.WriteLine("<EPRInstance EPR=\"{0}\" Profile=\"{1}\" Protocols=\"{2}\" />",
														 _epr.IPAddress, _epr.Profile, _epr.ProtocolString);
							}
							else
							{
								_ss.WriteLine("<EPRInstance EPR=\"{0}\" />", _epr.IPAddress);
							}
						}
						_ss.WriteLine("</EPRHost>");
//					}
				}
				_ss.WriteLine("</EPRSnapShot>");
			}

		}

		/// <summary>
		/// Will read in a snapshot file and then exactly duplicate that snapshot on all specified VM's 
		/// (if they are also in the snapshot file), or on all VM's that were in the snapshot file
		/// </summary>
		/// <param name="pHosts">List of VMs to load (specified or from the xml file)</param>
		/// <param name="pSnap"></param>
		static void LoadSnapShot(List<HostVM> pHosts, string pSnap)
		{
		//---------------------------------------------------------------------------------------------------------
		// We want the filename to always use .xml as an extension.  So - if the filename does not end in .xml - 
		// append .xml to the filename.  Then, see if the file exists and carry on.
			string _extension = Path.GetExtension(pSnap);
			if (string.Compare(_extension, @".xml", true) != 0)
			{
				GenUtils.Exclaim("Snapshot file name must use .xml extension. Appending .xml to " + pSnap);
				pSnap += ".xml";
			}
			if (!File.Exists(pSnap))
			{
				GenUtils.HangError("Cannot find file : " + pSnap);
				GenUtils.Exclaim("Press Enter to exit...");
				Console.ReadLine();
				Environment.Exit(2);
			}
			GenUtils.Exclaim("Loading " + pSnap + "....");
			List<HostVM> Loaded = new List<HostVM>();
			XmlDocument _xReader = new XmlDocument();
			_xReader.Load(pSnap);

		//---------------------------------------------------------------------------------------------------------
		//Read the XML file and reconstruct the list of VM's and all the EPRs on each VM (EPR Host) from the file.
		// 
			foreach (XmlNode _vmHost in _xReader.SelectSingleNode(@"EPRSnapShot"))
			{
				HostVM _tmpVM = new HostVM(_vmHost.Attributes.GetNamedItem(@"HostName").FirstChild.Value.Trim(), HostVM.HostOperations.Connect, true);
				foreach (XmlNode _eprInst in _vmHost.ChildNodes)
				{
					string _ip = _eprInst.Attributes.GetNamedItem(@"EPR").FirstChild.Value;
					EPRInstance _epr = new EPRInstance(IPAddress.Parse(_ip));
					_tmpVM.EPRs.Add(_epr);
					if(_eprInst.Attributes.GetNamedItem(@"Profile") != null)
					{
						string _prots = _eprInst.Attributes.GetNamedItem(@"Protocols").FirstChild.Value;
						_epr.Profile = _eprInst.Attributes.GetNamedItem(@"Profile").FirstChild.Value;
						_epr.SetProtocols(_prots);
					}
				}
				Loaded.Add(_tmpVM);
			}

		//---------------------------------------------------------------------------------------------------------
		// To apply the snapshot, we have to follow this pattern:
		//	For every EPRHostVM that remains in the List of VMs (from the -VM option or the default hosts file)
		//					If there is not a match for it in the Snapshot file - remove it from the Hosts List
		//	For every EPRHostVM that remains in the list:
		//		Stop all EPR instances on the host
		//		Replicate, EPR Instance for EPR instance from the snapshot file to the host VM
			GenUtils.Exclaim("Applying snapshot....");
			bool _done;
			int _hostNdx = 0;
		//-------------------------------------------------------------------------------------------------------
		// First - eliminate any Hosts that are in the main list and the not snapshot file...
			do
			{
				_done = true;
				for ( ; _hostNdx < pHosts.Count; _hostNdx++)
				{
					if (Loaded.Find(_hst => string.Compare(_hst.HostName, pHosts[_hostNdx].HostName, true) == 0) == null)
					{
						_done = false;
						break;
					}
				}
				if (_hostNdx < pHosts.Count)
				{
					GenUtils.Exclaim(pHosts[_hostNdx].HostName + " was not found in the Snapshot.  Removing Host from main list.");
					pHosts.RemoveAt(_hostNdx);
				}
			}
			while (!_done);
			if (pHosts.Count == 0)
			{
				GenUtils.HangError("There are no EPR Host VMs left in the list of specified Hosts.");
				return;
			}

		//-------------------------------------------------------------------------------------------------------
		// Next, report on any hosts that were in the Snapshot file, but are NOT in the main Action List...
			_hostNdx = 0;
			do
			{
				_done = true;
				for ( ; _hostNdx < Loaded.Count; _hostNdx++)
				{
					if (pHosts.Find(_hst => string.Compare(_hst.HostName, Loaded[_hostNdx].HostName, true) == 0) == null)
					{
						_done = false;
						break;
					}
				}
				if (_hostNdx < pHosts.Count)
				{
					GenUtils.Exclaim(Loaded[_hostNdx].HostName + " was not found in the main List of EPR Host VMs.  Removing Host from snapshot list.");
					Loaded.RemoveAt(_hostNdx);
				}
			}
			while (!_done);

		//---------------------------------------------------------------------------------------------------------
		// For the VMs that remain, they have been started because we could communciate with them and get their EPR
		// info) -- but now we must destroy any existing EPR instances.
			foreach (HostVM _host in pHosts)
			{
				_host.WorkQ.Add(HostVM.HostOperations.Stop);
				_host.WorkStatus = HostVM.HostVMStatus.Pending;
			}
			Dispatch(pHosts, true);

        //---------------------------------------------------------------------------------------------------------
		// Finally, we need to clone the snapshot Host EPR instances in the new HostVMs.
			foreach (HostVM _host in pHosts)
			{
				// Find the matching Host from the snapshot file...
				HostVM _clone = Loaded.Find(_hst => string.Compare(_hst.HostName, _host.HostName, true) == 0);
				if (_clone == null)
				{
					throw new Exception("Could not find Expected EPR Host VM to clone (" + _host.HostName + ") ?!");
				}
				_host.EPRs = _clone.EPRs;
				_host.WorkQ.Add(HostVM.HostOperations.Clone);
				_host.WorkStatus = HostVM.HostVMStatus.Pending;
			}
			Dispatch(pHosts, true);

		}

		/// <summary>
		/// Handles dumping various reports on the EPRs (Hosts and/or EPR Instances).  Can dump all, only the
		/// Used EPRs or only the unused EPRs.  Probably the only useful one is to dump all.
		/// </summary>
		/// <param name="pRep">Report to dump</param>
		/// <param name="pHosts">The Main "actio0n" list of VM's to use for the report</param>
		static void Report(Options.ToolActions pRep, List<HostVM> pHosts)
		{
			foreach (HostVM _vm in pHosts)
			{
				foreach (EPRInstance _epr in _vm.EPRs)
				{
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.Write(_vm.HostName);
					Console.ResetColor();
					Console.Write(" - ");
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write(_epr.IPAddress.ToString());
					Console.ResetColor();
					Console.Write(" :: ");
					if ((_epr.InUse) && (pRep != Options.ToolActions.ListUnused))
					{
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write(_epr.Profile);
						Console.ResetColor();
						Console.Write(" :: ");
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.WriteLine(_epr.ProtocolString);
						Console.ResetColor();
                        GenUtils.LogIt(_vm.HostName + " - " + _epr.IPAddress.ToString() + " :: " + _epr.Profile + " :: " + _epr.ProtocolString);  
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.WriteLine("Available");
						Console.ResetColor();
                        GenUtils.LogIt(_vm.HostName + " - " + _epr.IPAddress.ToString() + " :: Available");
                    }
				}
			}
			Console.WriteLine("\n\nPress Enter to Exit");
			Console.ReadLine();
		}
	}
}

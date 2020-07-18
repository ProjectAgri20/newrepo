using System;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Management;
using System.Threading;

namespace HP.ScalableTest.ManageEprs
{
	/// <summary>
	/// General Utility class.  These are methods that are used in many placs so they are abstracted
	/// out into a class that allows access from anywhere in the program.
	/// NOTE: A lot of the network constants that are defined in theis class are not currently]
	///   used.  However, they are included in case this code is leveraged and in that new code,
	///   they might be needed.
	/// </summary>
	public sealed class GenUtils
	{
		private static readonly GenUtils instance = new GenUtils();
        private static string logPath;

		/// <summary>
		/// Private constructor only called once at startup.
		/// </summary>
		private GenUtils()
		{
            logPath = null;	
		}

		/// <summary>
		/// Just used to enforce the singleton on this class.
		/// </summary>
		public static GenUtils Instance
		{
			get
			{
				return instance;
			}
		}

		private enum ResourceScope
		{
			RESOURCE_CONNECTED = 1,
			RESOURCE_GLOBALNET,
			RESOURCE_REMEMBERED,
			RESOURCE_RECENT,
			RESOURCE_CONTEXT
		}
		private enum ResourceType
		{
			RESOURCETYPE_ANY = 0,
			RESOURCETYPE_DISK,
			RESOURCETYPE_PRINT,
			RESOURCETYPE_RESERVED
		}
		private enum ResourceUsage
		{
			RESOURCEUSAGE_CONNECTABLE = 0x00000001,
			RESOURCEUSAGE_CONTAINER = 0x00000002,
			RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
			RESOURCEUSAGE_SIBLING = 0x00000008,
			RESOURCEUSAGE_ATTACHED = 0x00000010
		}
		private enum ResourceDisplayType
		{
			RESOURCEDISPLAYTYPE_GENERIC,
			RESOURCEDISPLAYTYPE_DOMAIN,
			RESOURCEDISPLAYTYPE_SERVER,
			RESOURCEDISPLAYTYPE_SHARE,
			RESOURCEDISPLAYTYPE_FILE,
			RESOURCEDISPLAYTYPE_GROUP,
			RESOURCEDISPLAYTYPE_NETWORK,
			RESOURCEDISPLAYTYPE_ROOT,
			RESOURCEDISPLAYTYPE_SHAREADMIN,
			RESOURCEDISPLAYTYPE_DIRECTORY,
			RESOURCEDISPLAYTYPE_TREE,
			RESOURCEDISPLAYTYPE_NDSCONTAINER
		}

	//-----------------------------------------------------------------------------------------------------------
	// It is absolutely critical that the compiler NOT optimize the layout of this object.  Otherwise, it is
	// quite probable that all calls will hang.  This is an old structure and an old API - but there so far I
	// have not found a better way to manipulate network drives/connections.  .NET seems defficient in that area
	// The field names come from the original structure.  
		[StructLayout(LayoutKind.Sequential)]
		public struct NETRESOURCE
		{
			public uint dwScope;
			public uint dwType;
			public uint dwDisplayType;
			public uint dwUsage;
			public string lpLocalName;
			public string lpRemoteName;
			public string lpComment;
			public string lpProvider;
		}

		[DllImport("mpr.dll")]
		private static extern int WNetAddConnection2
				(ref NETRESOURCE pNetRsc, string pPswd, string pUser, int pFlags);

		[DllImport("mpr.dll")]
		private static extern int WNetCancelConnection2	(string pLocalName, uint pFlags, int pForce);

		/// <summary>
		/// Given a netowork path, a username (can include Domain) and a password, creates a drive mapping.
		/// This can be important for a number of reasons - but is used in this utility in order to
		/// remotely access/execute processes. This gives this local process authenticated access to a
		/// remote systems file system!
		/// </summary>
		/// <param name="pNetPath">The network path to map (e.g. \\SomeSystem.Domain\SomePath)</param>
		/// <param name="pUser">Domain\Username</param>
		/// <param name="pPswd">Password</param>
		/// <returns>The Drive letter for the mapping</returns>
		public static string  MapNetworkDrive(string pNetPath, string pUser, string pPswd)
		{
		//---------------------------------------------------------------------------------------------------------
		// We have to find an unused drive letter - so use the net utility to grab one.
			string _driveLetter = NetDrive.Instance.GetDriveLetter();

			if (pNetPath.EndsWith(@"\"))	// This would be an illegal path!
			{
				pNetPath = pNetPath.Substring(0, pNetPath.Length - 1);
			}

			NETRESOURCE _netRsrc = new NETRESOURCE();
			_netRsrc.dwType = (uint)ResourceType.RESOURCETYPE_DISK;
			_netRsrc.lpLocalName = _driveLetter;
			_netRsrc.lpRemoteName = pNetPath;
			_netRsrc.lpProvider = null;

			int _errStat = WNetAddConnection2(ref _netRsrc, pPswd, pUser, 4);
			if (_errStat > 0)
			{
				throw new Exception("System Error Code : " + _errStat.ToString());
			}
			return _driveLetter;
		}

		/// <summary>
		/// Removes the network mapping and returns the drive letter to the free list.
		/// </summary>
		/// <param name="pDrive">Drive letter being freed</param>
		/// <param name="pForce">Flag to force the unmapping</param>
		/// <returns>Status from the WNet call</returns>
		public static int DisconnectNetworkDrive(string pDrive, bool pForce)
		{
			if (string.IsNullOrEmpty(pDrive))
			{
				return 1;
			}
			int _ret = WNetCancelConnection2(pDrive, 0, pForce ? 1 : 0);
			NetDrive.Instance.ReturnDriveLetter(pDrive);
			return _ret;
		}

/*
 * // Originally part of the design included the ability to perform an install and to move profiles
 * // from a network server to all the different Host VMs.  This requirement has been removed, but
 * // just in case it comes back....
 * 
		public static void CopyDirAndContents(string pSrc, string pDst, bool pOverWrite = true, bool pIncludeSubDirs = true)
		{
			DirectoryInfo _dirInfo = new DirectoryInfo(pSrc);
			DirectoryInfo[] _subDirs = _dirInfo.GetDirectories();

			if (!Directory.Exists(pDst))
			{
				Directory.CreateDirectory(pDst);
			}

			FileInfo[] _fileList = _dirInfo.GetFiles();
			foreach (FileInfo _file in _fileList)
			{
				_file.CopyTo(Path.Combine(pDst, _file.Name), pOverWrite);
			}
			if (pIncludeSubDirs)
			{
				foreach (DirectoryInfo _di in _subDirs)
				{
					CopyDirAndContents(_di.FullName, Path.Combine(pDst, _di.Name), pOverWrite, pIncludeSubDirs);
				}
			}
		}
*/

		/// <summary>
		/// Connects to a KNOWN EPR VM (must have user jawa) and looks for a specific process to be active.
		/// </summary>
		/// <param name="pHost">FQDN of the EPR Host e.g. EPR255-001.etl.boi.rd.adapps.hp.com</param>
		/// <param name="pProcess">The name of the process to look for</param>
		/// <param name="pTries">Limit the number of times to try. Default is 1</param>
		/// <returns>true if the process is found, otherwise false.  Can throw exceptions if the remote host cannot be reached or refuses connection</returns>
		public static bool CheckForRemoteProcess(string pHost, string pProcess, int pTries = 1)
		{
			bool _procFound = false;
			ConnectionOptions _cOpts = new ConnectionOptions();
			_cOpts.Username = "ETL\\jawa";
			_cOpts.Password = "!QAZ2wsx";

			try
			{ 
			//-------------------------------------------------------------------------------------------------------
			// In order to do this, use a Management Scope on the remote system and then query cimv2 for the
			// process (execution name) that is being looked for. Note that all processes, 32 and 64 bit should
			// be found when we query Win32_Process.
				ManagementScope _ms = new ManagementScope(@"\\" + pHost + @"\root\cimv2");
				_ms.Options = _cOpts;
				SelectQuery _query = new SelectQuery("select * from Win32_Process where name = '" + pProcess + "'");
				for (int i = 0; i < pTries; i++)
				{
				//-----------------------------------------------------------------------------------------------------
				// With the managementScope (remote) and the Query, the remote information can be retrieved.
					using (ManagementObjectSearcher _search = new ManagementObjectSearcher(_ms, _query))
					{
						ManagementObjectCollection _col = _search.Get();		// This call actually performs the query and pulls the results
						if (_col.Count > 0)
						{
							_procFound = true;	// If we get here, we know we found it.
							break;
						}
					}
				}
			}
			catch (Exception _ex)
			{
				_procFound = false;
				GenUtils.HangError("Looking for Remote Service on " + pHost + " :: Exception thrown: " + _ex.Message);
			}
			return _procFound;
		}

        /// <summary>
        /// Stops a remote process.  Not used currently, but if Stop is implemented, or if install is implemented
        /// then this would be needed.
        /// </summary>
        /// <param name="pHost">Host</param>
        /// <param name="pProcess">Process to stop</param>
		public static void KillRemoteProcess(string pHost, string pProcess)
		{
			ConnectionOptions _cOpts = new ConnectionOptions();
			_cOpts.Username = "ETL\\jawa";
			_cOpts.Password = "!QAZ2wsx";

			ManagementScope _ms = new ManagementScope(@"\\" + pHost + @"\root\cimv2");
			_ms.Options = _cOpts;
			SelectQuery _query = new SelectQuery("select * from Win32_Process where name = '" + pProcess + "'");
			using (ManagementObjectSearcher _search = new ManagementObjectSearcher(_ms, _query))
			{
				ManagementObjectCollection _col = _search.Get();
				if (_col.Count > 0)
				{
					foreach (ManagementObject _mo in _col)
					{
						_mo.InvokeMethod("Terminate", null);
					}
				}
			}
		}

		/// <summary>
		/// Connects to a known EPR VM (must have user jawa) and starts a process remotely.
		/// </summary>
		/// <param name="pHost">FQDN of the EPR Host e.g. EPR255-001.etl.boi.rd.adapps.hp.com</param>
		/// <param name="pProcess">The name of the process (executable) to launch</param>
		/// <param name="pWaitStart">Flag to force us to wait until the process has started</param>
		/// <param name="pWaitDone">Flag to force waiting until the remote process has started AND exited.</param>
		/// <returns>True if process start and return conditions have been satisfied</returns>
		public static bool ExecuteRemoteProcess(string pHost, string pProcess, bool pWaitStart = true, bool pWaitDone = false)
		{
			ConnectionOptions _cOpts = new ConnectionOptions();
			_cOpts.Username = "ETL\\jawa";
			_cOpts.Password = "!QAZ2wsx";
		
		//---------------------------------------------------------------------------------------------------------
		// As expected, must use root\cimv2 (Commin Information Model V2) on the remote system (using WMI)
		// to gain access to the remote system so processes can be started.  However, there are some MAJOR
		// limitations to this approach.  Processes started remotely cannot use a UI - in fact, they cannot show
		// aUI under any circumstances!
			ManagementScope _ms = new ManagementScope(@"\\" + pHost + @"\root\cimv2");
			_ms.Options = _cOpts;

		//---------------------------------------------------------------------------------------------------------
		// !! KLUDGE ALERT !!
		// Ok - here is the low down.  For this utility, the process that needs to be executed is always the 
		// EPRService - that is a known cnostant.  Unfortunately, the EPRService makes ana ssumption about its
		// working directory - and assumes that it is where ever the process was started from.  In this case -
		// that is a bad assumption because the process is beign started from a remote system!  So - in order to
		// set the "working directory" for this process, the remote process cannot be simply built and started -
		// the managament base object "attributes" for the "CommandLine" and the "CurrentDirectory" MUST be set.
		// This allows the EPRService to start and find the profiles - otherwise, the EPRService cannot EVER find
		// any profiles.  The KLUDGE here is that the CurrentDirectory is obviously hardcoded to a path that would
		// not be valid for almost anything else.  As an improvement, maybe someday this can be done right and be
		// part of an object passed in that defines the remtoe process more concisely.
//			object[] _runProc = {pProcess};
			ManagementClass _mc = new ManagementClass(_ms, new ManagementPath("Win32_Process"), new ObjectGetOptions());
//			_mc.InvokeMethod("Create", _runProc);
			ManagementBaseObject _mbo = _mc.GetMethodParameters("Create");
			_mbo["CommandLine"] = pProcess;
			_mbo["CurrentDirectory"] = @"C:\Program Files (x86)\HP\HP End Point Responder";
			_mc.InvokeMethod("Create", _mbo, null);

		//---------------------------------------------------------------------------------------------------------
		// This weird piece of code/path manipulation may seem superfluous BUT in the case where the process being
		// started HAS ARGUEMENTS as a part of pProcess, it is necessary to stip off the arguments and just get
		// the process (exe) name.
			string _remProc = Path.GetFileName(pProcess.Substring(0, (pProcess.IndexOf(".exe") + 4)));

			pWaitStart = pWaitDone ? true : pWaitStart;
			bool _started = false;
			bool _done = false;
			int _hardStop = 0;
		//---------------------------------------------------------------------------------------------------------
		// Logically, we cannot waiot for a process to end if we do not wait for it to start.
			if (pWaitStart)
			{
				SelectQuery _query = new SelectQuery("select * from Win32_Process where name = '" + _remProc + "'");
				//-----------------------------------------------------------------------------------------------------
				// Just keep executing this same query.  Unless the process is VERY fast (and yes, that timing hole
				// does exist and may need to be resolved some day), it should be possible to see it start (the query
				// actually returns somethign), and then if needed, keep querying until the query returns nothing - at
				// which point it can be known that the process started, executed and stopped. A hardstop is included
				// to keep this loop from hanging forever in case somethign goes wrong - or more likely, the process
				// starts, executes and exits so fast (all in under ~2 sec) that it is just never seen.
				do
				{
					using (ManagementObjectSearcher _search = new ManagementObjectSearcher(_ms, _query))
					{
						try
						{ 
							ManagementObjectCollection _col = _search.Get();
							if (_col.Count == 0)
							{
								if ((pWaitDone) && (_started))		// Process was seen and now is not - so it is done
								{
									return true;
								}
								Thread.Sleep(2000);		// This will be executed while waiting for the process to start
							}
							else if(!pWaitDone)		// Process is seen - so if not waiting until it exits, then done!
							{
								return true;
							}
							else
							{
									// Process is seen, but waiting for it to execute and exit -- so sleep a bit.
							}
							{
								_started = true;
								Thread.Sleep(2000);
							}
						}
						catch	(Exception)		// We can get here is there is a problem getting the query results - so wait and retry
						{
							Thread.Sleep(2000);
						}
					}
				}
				while ((!_done) && (++_hardStop <= 10));
			}
			return !pWaitStart;
		}

		/// <summary>
		/// Changes the foreground color to RED, dumps the message and then reset the colors.
		/// </summary>
		/// <param name="pMsg">Message to dump on the console</param>
		public static void HangError(string pMsg)
		{
            LogIt(pMsg);
			Console.ForegroundColor = ConsoleColor.Red; 
    		Console.WriteLine(pMsg);
			Console.ResetColor();
		}

		/// <summary>
		/// Changes foreground color to Cyan, dumps the message and then resets the colors
		/// </summary>
		/// <param name="pMsg">Message to dump tot he console</param>
		public static void Exclaim(string pMsg)
		{
            LogIt(pMsg);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine(pMsg);
			Console.ResetColor();
		}

        /// <summary>
        /// Writes the message to the log file
        /// </summary>
        /// <param name="pMsg">Message</param>
        public static void LogIt(string pMsg)
        {
            if (!string.IsNullOrEmpty(logPath))
            {
                try
                {
                    using (System.IO.StreamWriter _file = new System.IO.StreamWriter(logPath, true))
                    {
                        _file.WriteLine("{0:yyyyMMddHHmmss.fff} : {1}", DateTime.Now, pMsg);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine("!! Cannot Write to Log File :: " + _ex.Message);
                }
            }
        }


        /// <summary>
        /// Property for the logfile
        /// </summary>
        public static string LogFile
        {
            get
            {
                return logPath;
            }
            set
            {
                logPath = value;
            }
        }
	}
}

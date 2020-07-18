using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.ManageEprs
{
	/// <summary>
	/// Implements a very simple singleton class for managing network drive letters across
	/// multiple threads
	/// </summary>
	public sealed class NetDrive
	{
		private static readonly NetDrive instance = new NetDrive();

		private readonly List<string> availableDrives;
		private readonly List<string> inUseDrives;
		private readonly Object drvLock;
        private int workerThreads;

		/// <summary>
		/// Static Instance - ensures that only one instance of this object exists
		/// </summary>
		public static NetDrive Instance
		{
			get
			{
				return instance;
			}
		}

		/// <summary>
		/// Property that returns the maximum number of threads useable.  This will be
		/// either the number of network drives we could use, or the number passed in for
        /// the '-Threads #' command line option.
		/// </summary>
		public int MaxThreads
		{
			get
			{
				return workerThreads;
			}
            set
            {
                workerThreads = value <= availableDrives.Count ? value >= 1 ? value : availableDrives.Count : availableDrives.Count; 
            }
		}

		/// <summary>
		/// Private constructor. Since we will always need this object, use eager initialization
		/// to create the drive letter list at start up.
		/// </summary>
		private NetDrive()
		{
//			availableDrives = new List<string>() { "Z:", "Y:", "X:", "W:", "V:", "U:", "T:", "S:", "R:", "Q:", "P:", "O:", "N:", "M:", "L:" };
            availableDrives = new List<string>() { "X:", "W:", "V:", "U:", "T:", "S:", "R:", "Q:", "P:", "O:" };
            inUseDrives = new List<string>();

			drvLock = new Object();
			foreach (DriveInfo _drv in DriveInfo.GetDrives())
			{
			//-------------------------------------------------------------------------------------------------------
			// One potential issue here is that if there was an actual system that had enough local (non-network)
			// drives to use past the letter L:, the we could collide. COuld modify this to look at all types of
			// drive use...
				if (_drv.DriveType == DriveType.Network)
				{
					string _ltr = _drv.Name.Substring(0,2);
					if (availableDrives.IndexOf(_ltr) >= 0)
					{
						availableDrives.Remove(_ltr);
						inUseDrives.Add(_ltr);
					}
				}
			}
            workerThreads = availableDrives.Count;
		}

		/// <summary>
		/// Returns an available drive lettter. Uses synchronized access to the drive letter
		/// Lists to prevent multiple threads from obtaining the same drive letter.
		/// Here are the "rules" for this utilities use of drive letters:
		///  Number of worker threads is limited by the number of drive letters available
		///  Each worker thread can request one, and only one, drive letter at a time.
		///  Each Worker thread must release its drive letter when finished.
		/// </summary>
		/// <returns>Drive Letter to use for a network share map</returns>
		public string GetDriveLetter()
		{
			string _drv = null;

		//---------------------------------------------------------------------------------------------------------
		// Multiple threads can (and most likely will) hit this code within their time slices.  Because of that,
		// the assignment of the drive letter must be synchronized so that multiple threads do not end up with the
		// same drive letter.  It **should** be the case that there are NEVER more threads than there are available
		// drive letters. 
			lock(drvLock)
			{
//				if(availableDrives.Count > 0)
//				{ 
					_drv = availableDrives[0];
					inUseDrives.Add(_drv);
					availableDrives.Remove(_drv);
//				}
			}
			return _drv;
		}

		public void ReturnDriveLetter(string _drv)
		{
			inUseDrives.Remove(_drv);
			availableDrives.Add(_drv);
		}

	}
}

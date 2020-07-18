using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Linq;
using HP.Epr.Client;
using System.Text;
using System.Runtime.Serialization;

namespace HP.ScalableTest.ManageEprs
{
	/// <summary>
	/// Implements a single instance of an End Point Responder.  Allows tracking of individuals and / or groups.
	/// </summary>
	[DataContract]
	class EPRInstance
	{
		public static string NOPROFILE = "----";

		private string host;
		private string profile;
		private IPAddress netAddress;
		private Collection<Protocol> protos;
		private static Dictionary<string, Protocol> protocolMap;
        private bool wasRunning;

		#region Properties
		/// <summary>
		/// Property for host - manages the host name or ip of the EPR instance
		/// </summary>
		public string HostName
		{
			get
			{
				return host;
			}
			set
			{
				host = value;
			}
		}

		/// <summary>
		/// Property for profile - manages the Profile that this particular EPR instance is using (or not if idle/down)
		/// </summary>
		public string Profile
		{
			get
			{
				return profile;
			}
			set
			{
				profile = value;
			}
		}

		/// <summary>
		/// Profile for netAddress - manages the IP address of thie EPR instance
		/// </summary>
		public IPAddress IPAddress
		{
			get
			{
				return netAddress;
			}
			set
			{
				netAddress = value;
			}
		}

		/// <summary>
		/// Property for inUse - manages reporting on whether this EPR instance is active or available.
		/// </summary>
		public bool InUse
		{
			get
			{
				bool _wrk = !string.IsNullOrEmpty(profile);
				if (_wrk)
				{
					_wrk = !profile.StartsWith("-");
				}
				return _wrk;
			}
		}

		/// <summary>
		/// Property for protos - manages the protocols for this EPR instance
		/// </summary>
		public Collection<Protocol> Protocols
		{
			get
			{
				return protos;
			}
			set
			{
				protos = value;
			}
		}

		/// <summary>
		/// Property that will return a string representation of the Protocols that this instance of the EPR is using
		/// </summary>
		public string ProtocolString
		{
			get
			{
				StringBuilder _tmp = new StringBuilder();
				string _sep  = "";
				foreach (Epr.Client.Protocol _prot in Protocols)
				{
					_tmp.Append(_sep + _prot.ToString());
					_sep = ",";
				}
				return _tmp.ToString();
			}
		}

        /// <summary>
        /// Property that will return the state of this EPR Instance when the command line tool was first started
        /// </summary>
        public bool WasAlreadyExecuting
        {
            get
            {
                return wasRunning;
            }
        }
		#endregion

		/// <summary>
		/// Utility method that is used to build mapping from protocol strings to the Protocol objects themselves.
		/// </summary>
		private void setupProtoMap()
		{
			protocolMap = new Dictionary<string, Protocol>()
						{
							{"EWS", Protocol.Ews},
							{"LEDM", Protocol.LEDM},
							{"SNMP", Protocol.SNMP},
							{"OXPd", Protocol.OXPd},
							{"WEBSERVICES", Protocol.WebServices},
							{"WSDISCOVERY", Protocol.WSDiscovery}
						};
		}
		
		/// <summary>
		/// Constructor that takes a DeviceInstance to initialize all EPR Instance info
		/// </summary>
		/// <param name="pDev">Device Instance describing an active EPR Instance</param>
		public EPRInstance(DeviceInstance pDev, bool pRunning = false)
		{
			setupProtoMap();
			Protocols = new Collection<Protocol>();
			HostName = pDev.HostName;
			Profile = pDev.Name;
			IPAddress = pDev.Address;
            wasRunning = pRunning;
			foreach (ProtocolService _prot in pDev.ImplementedProtocolServices)
			{
				Protocols.Add(_prot.Protocol);
			}
		}

		/// <summary>
		/// Constructor that uses an IP address - indicates that the EPR Instance is not active
		/// </summary>
		/// <param name="pIP">IP address for the EPR</param>
		public EPRInstance(IPAddress pIP, bool pRunning = false)
		{
			setupProtoMap();
			HostName = pIP.ToString();
			Profile = NOPROFILE;
			IPAddress = pIP;
            wasRunning = pRunning;
			Protocols = new Collection<Protocol>();
		}

		/// <summary>
		/// Will reset the device information and initialize with the info fromteh DeviceInstance
		/// </summary>
		/// <param name="pDev">DeviceInstance that has the EPR info</param>
		public void ResetDevice(DeviceInstance pDev)
		{
			HostName = pDev.HostName;
			Profile = pDev.Name;
			Protocols.Clear();
			foreach (ProtocolService _prot in pDev.ImplementedProtocolServices)
			{
				Protocols.Add(_prot.Protocol);
			}
		}

		/// <summary>
		/// Utility method that clears the protocol and profile fields - basically making the EPR Instance appear idle
		/// or as it would be if it was idle.
		/// </summary>
		public void ClearDevice()
		{
			Protocols.Clear();
			Profile = NOPROFILE;
		}

		/// <summary>
		/// Given a comma-separated string of rotocols, will convert to the collection of actual protocol values.
		/// </summary>
		/// <param name="pProts">Comman separated string of protocols (or protocol name fragements)</param>
		public void SetProtocols(string pProts)
		{
			string[] _tokens;
			char[] _delims = { ',' };
			string _pcol = "";
			_tokens = pProts.Split(_delims);
			setupProtoMap();

			try
			{ 
				foreach(string _tkn in _tokens)
				{
					_pcol = _tkn.ToUpper();
					Protocols.Add(protocolMap.First(pm => pm.Key.Contains(_pcol)).Value);
				}
			}
			catch (Exception)
			{
				throw new Exception ("Unknown Protocol specified: " + _pcol);
			}
		}
	}
}

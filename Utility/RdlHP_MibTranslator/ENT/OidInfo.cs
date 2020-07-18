using System.Collections.Generic;

namespace HP.RDL.RdlHPMibTranslator
{
	public class OidInfo
	{
		public string NameOid { get; set; }
		public string OIDValue { get; set; }
		public string ExpectedValue { get; set; }
		public string InvalidSetValue { get; set; }
		public string Implementation { get; set; }

		public List<string> SupportedIndexes { get; set; }
		public List<string> NonSupportedIndexes { get; set; }
		public List<string> ValidSetValues { get; set; }

		public bool IsReadable { get; set; }
		public bool IsWriteable { get; set; }

		public CAPABILITIES Capability { get; set; }
		public SNMP_DATATYPES DataType { get; set; }
		public SNMP_CALL SnmpCallType { get; set; }

		public OidInfo()
		{
			NameOid = string.Empty;
			OIDValue = string.Empty;
			ExpectedValue = string.Empty;
			InvalidSetValue = string.Empty;
			Implementation = string.Empty;

			IsReadable = false;
			IsWriteable = false;

			SupportedIndexes = new List<string>();
			NonSupportedIndexes = new List<string>();
			ValidSetValues = new List<string>();

			DataType = SNMP_DATATYPES.ANone;
			SnmpCallType = SNMP_CALL.Get;
		}

		public OidInfo(OidInfo oi)
		{
			// TODO: Complete member initialization
			this.NameOid = oi.NameOid;
			this.OIDValue = oi.OIDValue;
			this.ExpectedValue = oi.ExpectedValue;
			this.InvalidSetValue = oi.InvalidSetValue;
			this.Implementation = oi.Implementation;

			this.SupportedIndexes = SetStringList(oi.SupportedIndexes);
			this.NonSupportedIndexes = SetStringList(oi.NonSupportedIndexes);
			this.ValidSetValues = SetStringList(oi.ValidSetValues);

			this.IsReadable = oi.IsReadable;
			this.IsWriteable = oi.IsWriteable;

			this.DataType = oi.DataType;
			this.SnmpCallType = oi.SnmpCallType;
		}

		private List<string> SetStringList(List<string> list)
		{
			List<string> lstNew = new List<string>();

			foreach (string str in list)
			{
				lstNew.Add(str);
			}

			return lstNew;
		}
		public string SupportedIdxStr()
		{
			string values = string.Empty;

			foreach (string val in this.SupportedIndexes)
			{
				if (!string.IsNullOrEmpty(values))
				{
					values += "&" + val;
				}
				else
				{
					values = val;
				}
			}
			return values;
		}

		public string Access()
		{
			string access = "read-only";

			if (IsReadable && IsWriteable)
			{
				access = "read-write";
			}
			else if (IsWriteable)
			{
				access = "write-only";
			}
			return access;
		}
	}
	// NAME,OID,INDEXES_SUPPORTED,INDEXES_NOT_SUPPORTED,EXPECTED_VALUE,READABLE,WRITABLE,DATA_TYPE,VALID_SET_VALUE,INVALID_SET_VALUE,IMPLEMENTATION,CAPABILITY
	public enum SNMP_TITLES
	{
		[EnumValue("Name")]
		Name = 0,
		[EnumValue("OID")]
		OID = 1,
		[EnumValue("INDEXES_SUPPORTED")]
		SupportedIndexes = 2,
		[EnumValue("INDEXES_NOT_SUPPORTED")]
		NonSupportedIndexes = 3,
		[EnumValue("EXPECTED_VALUE")]
		ExpectedValues = 4,
		[EnumValue("READABLE")]
		Readable = 5,
		[EnumValue("WRITABLE")]
		Writeable = 6,
		[EnumValue("DATA_TYPE")]
		DataType = 7,
		[EnumValue("VALID_SET_VALUE")]
		ValidSetValue = 8,
		[EnumValue("INVALID_SET_VALUE")]
		InvalidSetValue = 9,
		[EnumValue("IMPLEMENTATION")]
		Implementation = 10,
		[EnumValue("Count")]
		Capability = 11,
		[EnumValue("CAPABILITY")]
		Count = 12
	};
	public enum SNMP_DATATYPES
	{
		[EnumValue("None")]
		ANone = 0,
		[EnumValue("OCTETSTR")]
		OCTETSTR,
		[EnumValue("INTEGER")]
		INTEGER,
		[EnumValue("IPADDR")]
		IPADDR,
		[EnumValue("TIMETICKS")]
		TIMETICKS,
		[EnumValue("OBJECTID")]
		OBJECTID
	};
	public enum SNMP_CALL
	{
		[EnumValue("None")]
		ANone = 0,
		[EnumValue("Get")]
		Get,
		[EnumValue("GetNext")]
		GetNext,
		[EnumValue("Set")]
		Set
	}
	public enum CAPABILITIES
	{
		[EnumValue("ANone")]
		ANone = 0,
		[EnumValue("*")]
		All = 1,
		[EnumValue("ACCESS_CONTROL")]
		AccessControl = 2,
		[EnumValue("AIRPRINT")]
		AirPrint = 3,
		[EnumValue("COLOR")]
		Color = 4,
		[EnumValue("COLOR&DUPLEX")]
		ColorDuplex = 5,
		[EnumValue("CONSUMABLE_STATUS")]
		ConsumableStatus = 6,
		[EnumValue("DUPLEX")]
		Duplex = 7,
		[EnumValue("FAX")]
		Fax = 8,
		[EnumValue("IPv6")]
		IPv6 = 9,
		[EnumValue("JOB")]
		Job = 10,
		[EnumValue("JOB&COLOR")]
		JobColor = 11,
		[EnumValue("JOB&DUPLEX")]
		JobDuplex = 12,
		[EnumValue("OUTPUT_BIN")]
		OutputBin = 13,
		[EnumValue("PHD_MODEL")]
		PhdModel = 14,
		[EnumValue("PRINT_MODES")]
		PrintModes = 15,
		[EnumValue("SCAN")]
		Scan = 16,
		[EnumValue("SCAN&ADF")]
		ScanADF = 17,
		[EnumValue("SCAN&COPY")]
		ScanCopy = 18,
		[EnumValue("SCAN&COPY-JOB")]
		ScanCopyJob = 19,
		[EnumValue("SCAN&FLAT_BED")]
		ScanFlatBed = 20,
		[EnumValue("SCAN&SCAN_TO_FOLDER")]
		ScanScanToFolder = 21,
		[EnumValue("TBD")]
		TDB = 22,
		[EnumValue("TRAY")]
		Tray = 23,
		[EnumValue("TRAY1")]
		Tray1 = 24,
		[EnumValue("TRAY2")]
		Tray2 = 25,
		[EnumValue("TRAY3")]
		Tray3 = 26,
		[EnumValue("TRAY5")]
		Tray5 = 27,
		[EnumValue("WIRELESS_CARD")]
		WirelessCard = 28
	}
}

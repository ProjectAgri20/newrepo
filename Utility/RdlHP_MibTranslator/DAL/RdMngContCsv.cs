using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HP.RDL.RdlHPMibTranslator
{
	class RdMngContCsv
	{
		private string FileName { get; set; }
		public string GetLastError { get; private set; }

		public RdMngContCsv(string file)
		{
			if (!string.IsNullOrEmpty(file))
			{
				FileName = file;
				GetLastError = string.Empty;
			}
			else
			{
				GetLastError = "ReadOidFile: Can't instantiate with an empty or null file name.";
			}
		}

		public bool IsError
		{
			get
			{
				return (string.IsNullOrEmpty(GetLastError) == false) ? true : false;
			}
		}

		public bool RetrieveOidInfo(OidInfoList lstOidInfo)
		{
			if (!IsError)
			{
				ProcessFile(lstOidInfo);
			}
			return !IsError;
		}
		/// <summary>
		/// does the initial work of parsing the input file
		/// </summary>
		/// <param name="lstOidInfo">OidInfoList</param>
		private void ProcessFile(OidInfoList lstOidInfo)
		{
			FileStream fs = null;
			// NAME, OID, INDEXES_SUPPORTED, INDEXES_NOT_SUPPORTED, EXPECTED_VALUE,READABLE, WRITABLE, DATA_TYPE, VALID_SET_VALUE, INVALID_SET_VALUE, IMPLEMENTATION, CAPABILITY
			try
			{
				fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
				StreamReader sr = new StreamReader(fs);

				if (IsHeaderLine(sr.ReadLine()))
				{
					while (sr.Peek() != -1)
					{
						string line = sr.ReadLine();
						string[] buffer = line.Split(',');

						if (buffer.Length == (int)SNMP_TITLES.Count)
						{
							OidInfo oi = new OidInfo();

							oi.NameOid = buffer[(int)SNMP_TITLES.Name];
							oi.OIDValue = buffer[(int)SNMP_TITLES.OID];
							ParseSupportedIndexes(oi, buffer[(int)SNMP_TITLES.SupportedIndexes]);
							ParseNonSupportedIndexes(oi, buffer[(int)SNMP_TITLES.NonSupportedIndexes]);
							oi.ExpectedValue = buffer[(int)SNMP_TITLES.ExpectedValues];
							oi.IsReadable = GetIsReadWriteable(buffer[(int)SNMP_TITLES.Readable]);
							oi.IsWriteable = GetIsReadWriteable(buffer[(int)SNMP_TITLES.Writeable]);
							oi.DataType = GetDataType(buffer[(int)SNMP_TITLES.DataType]);
							ParseValidSetValues(oi, buffer[(int)SNMP_TITLES.ValidSetValue]);
							oi.InvalidSetValue = buffer[(int)SNMP_TITLES.InvalidSetValue];
							oi.Implementation = buffer[(int)SNMP_TITLES.Implementation];
							oi.Capability = GetCapability(buffer[(int)SNMP_TITLES.Capability]);

							if (!IsError) { lstOidInfo.Add(oi); }
						}
					}

				}
			}
			catch (FileNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (DirectoryNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (IOException e) { GetLastError = e.JoinAllErrorMessages(); }
			finally
			{
				if (fs != null) { fs.Close(); }
			}
		}

		/// <summary>
		/// parses the valid set of values to be used for the SNMP set call
		/// </summary>
		/// <param name="oi"></param>
		/// <param name="validSet"></param>
		private void ParseValidSetValues(OidInfo oi, string validSet)
		{
			string value = validSet.Trim();

			if (value.Length == 1)
			{
				oi.ValidSetValues.Add(value);
			}
			else if (value.Contains('&'))
			{
				GetIndexes(oi.ValidSetValues, value);
			}
			else if (value.Length > 0)
			{
				oi.ValidSetValues.Add(value);
			}
		}
		/// <summary>
		/// Translates the string into a enumeration
		/// </summary>
		/// <param name="capability">string</param>
		/// <returns>CAPABILITIES</returns>
		private CAPABILITIES GetCapability(string capability)
		{
			CAPABILITIES cap = Enum<CAPABILITIES>.Parse(capability);

			if (cap.Equals(CAPABILITIES.ANone))
			{
				GetLastError = "GetCapability: Unknown SNMP Capability type = " + capability;
			}
			return cap;
		}
		/// <summary>
		/// the input file has what the expected data type is supposed to be but
		/// the returned byte field also is a way to tell.
		/// </summary>
		/// <param name="dataType">string</param>
		/// <returns>SNMP_DataTypes</returns>
		private SNMP_DATATYPES GetDataType(string dataType)
		{
			SNMP_DATATYPES sdt = Enum<SNMP_DATATYPES>.Parse(dataType);

			if(sdt.Equals(SNMP_DATATYPES.ANone))
			{
				GetLastError = "GetDataType: Unknown SNMP data type - " + dataType;
			}

			return sdt;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool GetIsReadWriteable(string value)
		{
			bool isReadable = true;
			if (!value.ToUpper().Equals("YES"))
			{
				isReadable = false;
			}
			return isReadable;
		}

		/// <summary>
		/// Four types of non supported indexes
		/// 1. Single digit
		/// 2. String values separated by '&'
		/// 4. string with more than one digit
		/// 3. empty string
		/// </summary>
		/// <param name="oi">OidInfo</param>
		/// <param name="nonSupportedIdx">string</param>
		private void ParseNonSupportedIndexes(OidInfo oi, string nonSupportedIdx)
		{
			string value = nonSupportedIdx.Trim();
			if (value.Length == 1)
			{
				oi.NonSupportedIndexes.Add(value);
			}
			else if (value.Contains('&'))
			{
				GetIndexes(oi.NonSupportedIndexes, value);
			}
			else if (value.Length > 0)
			{
				oi.NonSupportedIndexes.Add(value);
			}
		}
		/// <summary>
		/// Five Types of supported indexes:
		/// 1. single digit
		/// 2. string values separated by '&'
		/// 3. JobId that signals using the SNMP Get Next
		/// 4. string with more than 1 digit
		/// 5. empty string
		/// </summary>
		/// <param name="oi">OidInfo</param>
		/// <param name="supportedIdx">string</param>
		private void ParseSupportedIndexes(OidInfo oi, string supportedIdx)
		{
			string value = supportedIdx.Trim();
			if (value.Length == 1)
			{
				oi.SupportedIndexes.Add(value);
			}
			else if (value.Contains('&'))
			{
				GetIndexes(oi.SupportedIndexes, value);
			}
			else if (value.ToUpper().Contains("JOBID"))
			{
				GetJobIdIndex(oi, value);
			}
			else if (value.Length > 0)
			{
				oi.SupportedIndexes.Add(value);
			}
			else // no index to add to OID but empty string required for processing
			{
				oi.SupportedIndexes.Add("");
			}
		}
		/// <summary>
		/// GetNext calls start with an index of JobID.0
		/// </summary>
		/// <param name="oi">OidInfo</param>
		/// <param name="value">string</param>
		private void GetJobIdIndex(OidInfo oi, string value)
		{
			string[] indexes = value.Split('.');
			if (indexes.Count() == 2)
			{
				oi.SnmpCallType = SNMP_CALL.GetNext;
				oi.SupportedIndexes.Add(indexes[1]);
			}
			else
			{
				GetLastError = "GetJobIdIndex: Unexpected number of values for JobID: " + indexes.Count().ToString();
			}
		}
		/// <summary>
		/// splits the index column to single values
		/// </summary>
		/// <param name="lstIndexes"></param>
		/// <param name="value"></param>
		private static void GetIndexes(List<string> lstIndexes, string value)
		{
			string[] indexes = value.Split('&');
			foreach (string idx in indexes)
			{
				lstIndexes.Add(idx);
			}
		}

		private bool IsHeaderLine(string header)
		{
			if (!string.IsNullOrEmpty(header))
			{
				string[] buffer = header.Split(',');
				if (buffer.Length == (int)SNMP_TITLES.Count)
				{
					if ((!buffer[(int)SNMP_TITLES.Name].ToUpper().Equals("NAME")) || (!buffer[(int)SNMP_TITLES.OID].ToUpper().Equals("OID")))
					{
						GetLastError = "Expecting Header/Column Titles, not: " + header;
					}
				}
				else
				{
					GetLastError = "IsHeaderLine: Expecting " + SNMP_TITLES.Count.ToString() + " columns, not " + buffer.Length.ToString() + ".";
				}
			}
			return !IsError;
		}
	}
}

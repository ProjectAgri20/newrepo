using System;
using HP.DeviceAutomation;

namespace HP.RDL.RdlHPMibTranslator
{
    // wrap ...
    class RDLSnmpGns
    {
        public int NumAttempts { get; set; }
        public int Timeout { get; set; }
        public string OID { get; set; }
        public string GetLastError { get; private set; }

        private Lazy<Snmp> _snmp;

        private Snmp SNMP
        {
            get { return _snmp.Value; }
        }
        
        private string IPAddress { get; set; }
        private bool SNMPVersion1 { get; set; }
        /// <summary>
        /// Processes a single SNMP OID against the given IP Address. Utilizes the STF SNMP Library.
        /// Default SNMP version is V1, Timeout is 7 seconds, and retries are set to 3. If a bad
        /// version is given the system will default to version 1.
        /// </summary>
        /// <param name="pOID">string</param>
        /// <param name="pIP">string</param>
        /// <param name="iRepeats">int</param>
        /// <param name="timeout">int</param>
        /// <param name="snmpVersion">int</param>
        public RDLSnmpGns(string pOID, string pIP, int iRepeats = 3, int timeout = 7, bool snmpVer1 = false)
        {
            NumAttempts = iRepeats;
            Timeout = timeout;
            OID = pOID;
            IPAddress = pIP;
            SNMPVersion1 = snmpVer1;

            SetSNMPDefaults();
        }
        /// <summary>
        /// Secondary constructor that requires the user to set the public property OID prior to calling GetSNMPData()
        /// </summary>
        /// <param name="pIP"></param>
        /// <param name="vc"></param>
        public RDLSnmpGns(string pIP)
        {
            NumAttempts = 3;
            Timeout = 7;
            IPAddress = pIP;

            SetSNMPDefaults();
        }

        /// <summary>
        /// Returns true if error info is contained in property GetLastError
        /// </summary>
        public bool IsError
        {
            get
            {
                return (string.IsNullOrEmpty(GetLastError) == false) ? true : false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exeTime"></param>
        /// <param name="pOID"></param>
        /// <returns></returns>
        public string GetNextSnmp(out int exeTime, string pOID, ref string nextString)
        {
            string result = string.Empty;
            exeTime = 0;
            GetLastError = string.Empty;

            int startCnt = Environment.TickCount;

            try
            {
                SnmpOidValue oidData = SNMP.GetNext(pOID);
                exeTime = Environment.TickCount - startCnt;
                nextString = oidData.Oid;
                result = oidData.Value.ToString();
                if (nextString[0] == '.')
                {
                    nextString = nextString.Substring(1);
                }
            }
            catch (SnmpException se)
            {
                SetLextmError(se);
            }
            catch (Exception ex)
            {
                GetLastError = "ERROR: " + ex.JoinAllErrorMessages();
            }
            finally
            {
                if (exeTime == 0 && startCnt != 0)
                {
                    exeTime = Environment.TickCount - startCnt;
                }
                if (result.Contains("NoSuch"))
                {
                    GetLastError = "ERROR: " + result;
                }
            }

            return result;
        }
        /// <summary>
        /// Utilizes the Get function from STF Library to retrieve results for the given IP Address and OID
        /// </summary>
        /// <returns></returns>
        public string GetSNMPData(out int exeTime, string oid = "")
        {
            string result = string.Empty;
            GetLastError = string.Empty;
            exeTime = 0;
            
            if (!string.IsNullOrEmpty(oid)) { OID = oid; }
            int startCnt = 0;
            try
            {
                startCnt = Environment.TickCount;

                result = SNMP.Get(OID);
                exeTime = Environment.TickCount - startCnt;
            }
            catch (SnmpException ex)
            {
                SetLextmError(ex);
            }
            catch (Exception ex)
            {
                GetLastError = "ERROR: " + ex.JoinAllErrorMessages();
            }
            finally
            {
                result = ProcessFinally(ref exeTime, result, startCnt);
            }
            return result;
        }

        private string ProcessFinally(ref int exeTime, string result, int startCnt)
        {
            if (exeTime == 0 && startCnt != 0)
            {
                exeTime = Environment.TickCount - startCnt;
            }
            if (result.Contains("NoSuch")) { GetLastError = "ERROR: " + result; }
            if (IsError)
            {
                result = GetLastError;
            }
            return result;
        }

        private void SetLextmError(SnmpException ex)
        {          
            if (ex.Message.Contains("NoSuchName"))
            {
                GetLastError = string.Format("ERROR: <NoSuchName> {0}", ex.Message);
            }
            else
            {
                GetLastError = "ERROR: " + ex.Message;
            }
        }
        private void SetSNMPDefaults()
        {
            _snmp = new Lazy<Snmp>(() => new Snmp(IPAddress,SNMPVersion1));

            SNMP.Timeout = TimeSpan.FromSeconds(Timeout);
            SNMP.Retries = NumAttempts;

            GetLastError = string.Empty;
        }
        /// <summary>
        /// Converts the bytes to something readable
        /// </summary>
        /// <param name="myBytes">byte[]</param>
        /// <returns>string</returns>
        private string GetOctalString(byte[] myBytes)
        {
            string result = string.Empty;
            int byteLength = myBytes.Length;

            if (byteLength > 6 && myBytes[2] == 1 && myBytes[3] == 21)
            {
                result = GetPrintableString(myBytes);
            }
            else if (byteLength > 75 && myBytes[1] == 129 && myBytes[3] == 1 && myBytes[4] == 21)
            {
                result = GetPrintableString(myBytes).Replace(',', ' ');
            }
            else if ((byteLength > 2) && (myBytes[2] == 253) && (myBytes[1] > 2))
            {
                result = GetPrintableString(myBytes);
            }
            else if ((byteLength > 3) && IsPrintableChar(myBytes[3]))
            {
                result = GetPrintableString(myBytes).Replace(',', ' ');
            }
            else if ((byteLength > 50) && (myBytes[3] == 237 || myBytes[3] == 253))
            {
                result = GetPrintableString(myBytes).Replace(',', ' ');
            }
            else
            {
                result = BitConverter.ToString(myBytes).Replace("-", " ");
                if (result.Length > 6)
                {
                    result = result.Substring(6);
                }
            }
            return result;
        }
        /// <summary>
        /// if the OID result starts with a period, will remove and send back result
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>string</returns>
        private string GetOIDResult(string data)
        {
            if (data[0] == '.')
            {
                data = data.Substring(1);
            }
            return data;
        }
        /// <summary>
        /// Retrieves the IP address from the byte array
        /// </summary>
        /// <param name="myBytes">byte[]</param>
        /// <returns>string</returns>
        private string GetIPAddress(byte[] myBytes)
        {
            string result = string.Empty;
            int len = myBytes[1] + 2;

            for (int idx = 2; idx < len; idx++)
            {
                result += myBytes[idx].ToString();
                if (idx < len - 1)
                {
                    result += ".";
                }
            }
            return result;
        }
        /// <summary>
        /// Goes through the byte array looking for printable characters.
        /// </summary>
        /// <param name="myBytes"></param>
        /// <returns></returns>
        private string GetPrintableString(byte[] myBytes)
        {
            string result = string.Empty;
            //Encoding code = new System.Text.ASCIIEncoding();

            for (int idx = 2; idx < myBytes.Length; idx++)
            {
                if (IsPrintableChar(myBytes[idx]))
                {
                    result += Convert.ToChar(myBytes[idx]).ToString();
                }
            }
            return result;
        }
        /// <summary>
        /// Last resort to figure out the returned bytes. Simply just convert the bytes to a string.
        /// </summary>
        /// <param name="myBytes">byte[]</param>
        /// <returns>string</returns>
        private string GetUnknownResults(byte[] myBytes)
        {
            string result = BitConverter.ToString(myBytes).Replace("-", " ");
            if (result.Length > 6)
            {
                result = result.Substring(6);
            }
            return result;
        }
        /// <summary>
        /// Takes the integer value from the byte array element and checks if it is
        /// a printable character from the ASCII Code table. The printable characters are 
        /// described in the ASCII table as decimal values between 32 and 126.
        /// </summary>
        /// <param name="decValue">int</param>
        /// <returns>bool</returns>
        private bool IsPrintableChar(int decValue)
        {
            bool printable = false;

            if ((decValue >= 32) && (decValue <= 126))
            {
                printable = true;
            }
            return printable;
        }
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
    }
}

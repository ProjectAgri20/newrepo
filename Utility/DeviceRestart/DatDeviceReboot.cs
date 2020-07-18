using System;
using System.Text.RegularExpressions;
using HP.DeviceAutomation;

namespace HP.STF.DeviceRestart
{
    public class DatDeviceReboot
    {
        private Lazy<Snmp> _snmp;

        /// <summary>
        /// Sets the type of Reboot.
        /// 4 - General reboot
        /// 5 - NVRAM
        /// 6 - NVRAM and Factory reset
        /// </summary>
        public int RebootStyle { get; set; }
        public int NumAttempts { get; set; }
        public int Timeout { get; set; }
        public string OID { get; set; }

        private Snmp SNMP
        {
            get { return _snmp.Value; }
        }
        public string IPAddress { get; set; }
        private bool SNMPVersion1 { get; set; }
        /// <summary>
        /// Gets the get last error.
        /// </summary>
        /// <value>
        /// The get last error.
        /// </value>
        public string GetLastError { get; private set; }
        /// <summary>
        /// Property (bool) for error
        /// Returns true if GetfLastError is set.
        /// </summary>
        public bool IsError
        {
            get
            {
                return (string.IsNullOrEmpty(GetLastError) == false) ? true : false;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DatDeviceReboot"/> class.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="oid">The oid.</param>
        /// <param name="iRepeats">The i repeats.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="snmpVer1">if set to <c>true</c> [SNMP ver1].</param>
        public DatDeviceReboot(string ipAddress, string oid, int iRepeats = 3, int timeout = 7, bool snmpVer1 = false)
        {
            IPAddress = ipAddress;
            OID = oid;
            NumAttempts = iRepeats;
            Timeout = timeout;
            SNMPVersion1 = snmpVer1;
            RebootStyle = 4;

            SetSNMPDefaults();
        }
        /// <summary>
        /// Sets the SNMP defaults.
        /// </summary>
        private void SetSNMPDefaults()
        {
            _snmp = new Lazy<Snmp>(() => new Snmp(IPAddress, SNMPVersion1));

            SNMP.Timeout = TimeSpan.FromSeconds(Timeout);
            SNMP.Retries = NumAttempts;

            GetLastError = string.Empty;
        }
        /// <summary>
        /// Utilizes the Get function from DAT services to retrieve results for the given IP Address and OID
        /// </summary>
        /// <returns>string</returns>
        public string GetSNMPData(out int exeTime, string oid)
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
                GetLastError = ex.JoinAllErrorMessages();
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
        /// <summary>
        /// Creates the DAT SnmpOidValue using type int for use in the DAT SNMP Set
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <param name="value">The value.</param>
        /// <param name="exeTime">The executable time.</param>
        /// <returns>string</returns>
        public string SetDeviceValue(string oid, int value, out int exeTime)
        {
            SnmpOidValue sov = new SnmpOidValue(oid, value);
            return SnmpSet(sov, out exeTime);
        }

        /// <summary>
        ///Creates the DAT SnmpOidValue using type string for use in the DAT SNMP Set
        /// </summary>
        /// <param name="oid">The OID.</param>
        /// <param name="value">The value.</param>
        /// <param name="exeTime">The executable time.</param>
        /// <returns></returns>
        public string SetDeviceValue(string oid, string value, out int exeTime)
        {
            SnmpOidValue sov = new SnmpOidValue(oid, value);
            return SnmpSet(sov, out exeTime);
        }
        /// <summary>
        /// Creates the DAT SnmpOidValue using type int for use in the DAT SNMP Set
        /// Sets the SnmpOidValue OID with the desired reboot style.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <param name="exeTime">The executable time.</param>
        /// <returns></returns>
        public string RebootDevice(string oid, out int exeTime)
        {
            SnmpOidValue sov = new SnmpOidValue(oid, RebootStyle);
            return SnmpSet(sov, out exeTime);
        }
        /// <summary>
        /// Uses DAT SNMP set
        /// </summary>
        /// <param name="sov">The sov.</param>
        /// <param name="exeTime">The executable time.</param>
        /// <returns></returns>
        private string SnmpSet(SnmpOidValue sov, out int exeTime)
        {
            string result = string.Empty;
            exeTime = 0;
            int startCnt = 0;
            try
            {
                startCnt = Environment.TickCount;
                result = SNMP.Set(sov).ToString();
                exeTime = Environment.TickCount - startCnt;
            }
            catch (Exception ex)
            {
                GetLastError = ex.JoinAllErrorMessages();
            }
            return result;
        }
        /// <summary>
        /// Processes the finally.
        /// </summary>
        /// <param name="exeTime">The executable time.</param>
        /// <param name="result">The result.</param>
        /// <param name="startCnt">The start count.</param>
        /// <returns></returns>
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
        /// Verifies that IP is properly formatted
        /// </summary>
        /// <param name="ipAddr">string</param>
        /// <returns>bool: true if formatted correctly</returns>
        public bool ValidIPAddress(string ipAddr = "")
        {
            GetLastError = string.Empty;
            String pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-5]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            Regex validate = new Regex(pattern);

            if (ipAddr != "")
            {
                if (!validate.IsMatch(ipAddr, 0))
                {
                    GetLastError = "Invalid IP Address.";
                }
            }
            else
            {
                GetLastError = "Please provide the IP address for the printer to be power cycled.";
            }
            return !IsError;
        }
    }
}

using System;
using System.Linq;

using HP.DeviceAutomation;

using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.RDL.RdlHPMibTranslator
{
    class DeviceOids
    {
        public string IPAddress { get; set; }
        public string StartOid { get; set; }

        public bool SnmpVer2 { get; set; }
        public string ReleaseName { get; private set; }
        public string GetLastError { get; private set; }

        public ListDeviceOidEnt ListDeviceOids { get; set; }
		public string Password { get; set; }

        private IDevice _device = null;

        public DeviceOids(string pwd, string ipAddr, string startOid, bool snmpVer2 = true)
        {
            IPAddress = ipAddr;
            StartOid = startOid;
            SnmpVer2 = snmpVer2;

			Password = pwd;

            ReleaseName = string.Empty;

            ListDeviceOids = new ListDeviceOidEnt();
        }

        public bool IsError
        {
            get
            {
                return (string.IsNullOrEmpty(GetLastError) == false) ? true : false;
            }
        }
        public bool ValidateEngine()
        {
            GetLastError = string.Empty;

            try
            {
                _device = DeviceFactory.Create(IPAddress, Password);
                WakeDevice();
                ReleaseName = _device.GetDeviceInfo().ModelName;
            }
            catch(DeviceCommunicationException dce)
            {
                GetLastError = dce.Message;
            }
            catch(UnknownDeviceTypeException udte)
            {
                GetLastError = udte.Message;
            }
            catch(Exception ex)
            {
                GetLastError = ex.JoinAllErrorMessages();
            }
            return !IsError;
        }
        private void WakeDevice()
        {
            if(_device is JediDevice)
            {
                (_device as JediDevice).PowerManagement.Wake();
            }
            else if(_device is OzDevice)
            {
                (_device as OzDevice).PowerManagement.Wake();
            }
            else if(_device is PhoenixDevice)
            {
                // rats, now wake
            }
            else if(_device is SiriusDevice)
            {
                (_device as SiriusDevice).PowerManagement.Wake();
            }
        }
        public void GetDeviceOidListings()
        {
            string sValue = string.Empty;
            int exeTime = 0;
            bool isFamily = true;
            int count = 0;
            string rtnOID = StartOid;
            string OID = StartOid;

            RDLSnmpGns rDLSnmpGns = new RDLSnmpGns(IPAddress);
            ListDeviceOids.Clear();
            GetLastError = string.Empty;

            while (isFamily && !IsError)
            {
                DeviceOidEnt oidInfo = new DeviceOidEnt();
                count++;
                int execTm = 0;
                sValue = rDLSnmpGns.GetNextSnmp(out execTm, OID, ref rtnOID);
                exeTime += execTm;
                GetLastError = rDLSnmpGns.GetLastError;
                if (!IsError)
                {
                    isFamily = CheckForFamily(StartOid, rtnOID);
                    if (IsError && count > 1)
                    {
                        GetLastError = string.Empty;
                    }
                }

                oidInfo.OidString = rtnOID;
                oidInfo.OidValue = sValue.Replace(',', ';');
                ListDeviceOids.Add(oidInfo);

                OID = rtnOID;
            }
        }
        private bool CheckForFamily(string pOID, string rtnResult)
        {
            bool isFamily = false;
            if (rtnResult.Length > 0)
            {
                if (rtnResult[0] == '.')
                {
                    rtnResult = rtnResult.Substring(1);
                }
                if (rtnResult.StartsWith(pOID))
                {
                    isFamily = true;
                }
                else
                {
                    rtnResult = "ERROR: Family not found = " + rtnResult;
                    GetLastError = rtnResult;
                }
            }
            return isFamily;
        }
    }

}

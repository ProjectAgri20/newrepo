using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.RDL.RdlHPMibTranslator
{


    class BuildOIDString
    {
        private const string HP_OID = "1.3.6.1.4.1.11";
        private const string DM_OID = "2.3.9.4.2";

        private ListMibEnt _ParentMIB_Values;

        public BuildOIDString(ListMibEnt listMibEnt)
        {
            _ParentMIB_Values = listMibEnt;
        }

        public string BuildOidStringValue(OidEnt oid)
        {
            string oidValue = string.Empty;

            oidValue = GetOidValue(oid.Parent);
            if (!string.IsNullOrEmpty(oidValue))
            {
                if (oidValue[0].Equals('.'))
                {
                    oidValue = oidValue.Substring(1);
                }

                if (!oidValue.StartsWith(HP_OID))
                {
                    string value = HP_OID;

                    if (!oidValue.Contains(DM_OID))
                    {
                        value += "." + DM_OID;
                    }
                    oidValue = value + "." + oidValue; 
                }
           }
            oidValue += "." + oid.Value;

            return oidValue;
        }

        public string GetOidValue(string title)
        {
            string myValue = string.Empty;
            foreach (MibEnt mib in _ParentMIB_Values)
            {
                if (mib.Title.Equals(title))
                {
                    myValue = GetOidValue(mib.Parent) + "." + mib.Value.ToString();
                }
            }

            return myValue;
        }
    }
}

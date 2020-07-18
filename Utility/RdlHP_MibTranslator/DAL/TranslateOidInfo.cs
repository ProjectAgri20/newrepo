using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace HP.RDL.RdlHPMibTranslator
{
    class TranslateOidInfo
    {
        //private const int EOF = -1;
        private readonly string[] SEPARATORS = { ":", "=", " ", "\t", "{", "}", "," };
        private readonly string _OIDPrivate = "1.3.6.1.4.1";
        private readonly string _OIDMgmt = "1.3.6.1.2.1";

        private const string HP_OID = "1.3.6.1.4.1.11";
        private const string DM_OID = "2.3.9.4.2";

        private OidEnt _oidInfo = null;

        private string MibFile { get; set; }
        private string CurrentLine { get; set; }

        public string GetLastError { get; set; }
        public ListOidEnt OidsListing { get; set; }
        private OID_TYPES OidType { get; set; }

        private ListMibEnt _listMibParents;

        private enum OID_TYPES
        {
            unKnown = 0,
            name,
            oidList,
            syntax,
            access,
            status,
            description,
            parentLine,
            mibEnt
        };


        public TranslateOidInfo(string fileName, ListMibEnt mibList)
        {
            MibFile = fileName;
            _listMibParents = mibList;
            GetLastError = string.Empty;
            CurrentLine = string.Empty;
        }
        public void CreateOidListing()
        {
            OidsListing = new ListOidEnt();

            FileStream fs = null;

            try
            {
                fs = new FileStream(MibFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                MovePointerToStart(sr);
    
                while (sr.Peek() != -1)
                {
                    if (!string.IsNullOrEmpty(CurrentLine) && !CurrentLine.Contains("-- XREF:"))
                    {
                        SetOidType(CurrentLine);
                        BuildOidObject(sr, CurrentLine);
                    }
                    else
                    {
                        CurrentLine = sr.ReadLine().Trim();
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

        private void BuildOidObject(StreamReader sr, string line)
        {
            if (_oidInfo == null) { _oidInfo = new OidEnt(); }
            CurrentLine = string.Empty;

            switch (OidType)
            {
                case OID_TYPES.oidList: _oidInfo.OidString = GetOidString(line);
                    break;
                case OID_TYPES.name: _oidInfo.OidName = GetOidName(line);
                    break;
                case OID_TYPES.syntax: _oidInfo.Syntax = GetSyntaxListing(sr, line);
                    break;
                case OID_TYPES.access: _oidInfo.Access = GetStatusOrAccess(line);
                    break;
                case OID_TYPES.status: _oidInfo.Status = GetStatusOrAccess(line);
                    break;
                case OID_TYPES.description: _oidInfo.Description = GetDescription(sr, line);
                    break;
                case OID_TYPES.parentLine: GetParentValue(_oidInfo, line);
                    break;
                case OID_TYPES.mibEnt: AddToMibListing(sr, line);
                    break;
            }
            if (OidInfoComplete(_oidInfo))
            {
                _oidInfo.Usage = USAGES.eMib;
                if(string.IsNullOrEmpty(_oidInfo.OidString))
                {
                    BuildOidString();
                }
                OidsListing.Add(_oidInfo);
                _oidInfo = null;
            }
        }

        private void AddToMibListing(StreamReader sr, string line)
        {
            TranslateMibInfo tmi = new TranslateMibInfo("");
            MibEnt mib = new MibEnt();
            OidEnt oid = new OidEnt();
            bool bDone = false;

            mib.Title = GetOidName(line);

            while (sr.Peek() != -1 && !bDone)
            {
                
                if (line.Contains("::="))
                {
                    GetParentValue(oid, line);
                    mib.Parent = oid.Parent;
                    mib.Value = int.Parse(oid.Value);

                    if(!tmi.FoundMibEnt(_listMibParents, mib))
                    {
                        _listMibParents.Add(mib);
                    }
                    bDone = true;
                }
                line = sr.ReadLine().Trim();
            }
        }

        private void BuildOidString()
        {
            BuildOIDString clsOid = new BuildOIDString(_listMibParents);
            _oidInfo.OidString = clsOid.BuildOidStringValue(_oidInfo);
        }

        private string GetOidString(string line)
        {
            int idx = line.IndexOf(_OIDPrivate);
            if (idx < 0)
            {
                idx = line.IndexOf(_OIDMgmt);
            }
            string value = line.Substring(idx).Trim();
            return value;
        }

        private bool OidInfoComplete(OidEnt oidInfo)
        {
            bool bComplete = false;

            if (!string.IsNullOrEmpty(oidInfo.Value) && !string.IsNullOrEmpty(oidInfo.Parent)
                && !string.IsNullOrEmpty(oidInfo.OidName) && !string.IsNullOrEmpty(oidInfo.Access)
                && !string.IsNullOrEmpty(oidInfo.Status) && !string.IsNullOrEmpty(oidInfo.Description)
                && oidInfo.Syntax.Count() > 0)
            {
                bComplete = true;
            }

            return bComplete;
        }
  
        private void GetParentValue(OidEnt oidInfo, string line)
        {
            string[] data = line.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

            // If there is a space in the parent name...
            int last = data.Count() - 1;
            oidInfo.Value = data[last];

            for (int idx = 0; idx < last; idx++)
            {
                oidInfo.Parent += data[idx];
            }
        }

        private string GetDescription(StreamReader sr, string descr)
        {
            if(descr.Length > 12)
            {
                descr = descr.Substring(12).Trim();
            }
            else
            {
                descr = string.Empty;
            }
            
            bool bDone = false;

            while (sr.Peek() != -1 && !bDone)
            {
                string line = sr.ReadLine().Trim();
                if (!line.Contains("::="))
                {
                    if (!string.IsNullOrEmpty(descr))
                    {
                        descr += " " + line;
                    }
                    else
                    {
                        descr = line;
                    }
                }
                else
                {
                    CurrentLine = line;
                    bDone = true;
                }
            }
            return descr;
        }

        private string GetStatusOrAccess(string line)
        {
            string[] data = line.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
            string value = string.Empty;

            int last = data.Count();

            for (int idx = 1; idx < last; idx++)
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = data[idx];
                }
                else
                {
                    value += " " + data[idx];
                }
            }
            return value;
        }

        private List<string> GetSyntaxListing(StreamReader sr, string line)
        {
            List<string> syntaxes = new List<string>();
            string temp = string.Empty;

            if (!line.Contains("{"))
            {
                temp = line.Substring(7).Trim();
                syntaxes.Add(temp);
            }
            else
            {
                // we have enumerations which may be on more than one line.
                int idx = line.IndexOf('{') - 7;
                syntaxes.Add(line.Substring(7, idx).Trim());
                temp = line.Substring(idx+7);
                string[] data = temp.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in data)
                {
                    syntaxes.Add(str);
                }
                // enumerations are on more than one line
                if (!temp.Contains("}"))
                {
                    GetEnumerations(syntaxes, sr);
                }				
            }

            return syntaxes;
        }

        private void GetEnumerations(List<string> syntaxes, StreamReader sr)
        {
            bool bDone = false;

            while (sr.Peek() != -1 && !bDone)
            {
                string line = sr.ReadLine().Trim();
                if (line.Contains("}")) { bDone = true; }

                string[] data = line.Split(SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
                foreach (string enumeration in data)
                {
                    syntaxes.Add(enumeration);
                }
            }
        }


        private string GetOidName(string line)
        {		
	
            int idx = line.IndexOf("Object-type", StringComparison.OrdinalIgnoreCase) -1;
            string name = line.Substring(0, idx).Trim();

            if (name.Equals("clearable-warning"))
            {
                int waiting = 0;
            }

            return name;
        }

        private void MovePointerToStart(StreamReader sr)
        {
            bool bFound = false;
            string line = string.Empty;

            while (sr.Peek() != -1 && !bFound)
            {
                line = sr.ReadLine().Trim();
                if (StartProductMIB(line) || StartJediGenericMIB(line) || StartJetDirect(line) || StartLaserJetCommon(line))
                {
                    bFound = true;
                }				
            }
            CurrentLine = line.Trim();
        }
        private bool StartProductMIB(string line)
        {
            bool bStart = false;
            if ((line.Contains(_OIDPrivate) || line.Contains(_OIDMgmt)) && (line.Length > _OIDPrivate.Length))
            {
                bStart = true;
            }
            return bStart;
        }

        private bool StartJediGenericMIB(string line)
        {
            bool bStart = false;

            if(line.Contains("-- XREF:") && !line.Contains("xxxx") )
            {
                bStart = true;
            }

            return bStart;
        }
        private bool StartJetDirect(string line)
        {
            bool bStart = false;

            if (line.Contains("-- HP proprietary objects"))
            {
                bStart = true;
            }

            return bStart;
        }
        private bool StartLaserJetCommon(string line)
        {
            bool bStart = false;
            if (line.Trim().Contains("OBJECT-TYPE") && !line.StartsWith("OBJECT-TYPE"))
            {
                bStart = true;
            }
            return bStart;
        }
        private void SetOidType(string line)
        {
            string data = line.ToUpper();

            if (data.Contains(_OIDPrivate) || data.Contains(_OIDMgmt))
            {
                OidType = OID_TYPES.oidList;
            }
            else if (data.Contains("OBJECT-TYPE"))
            {
                OidType = GetNameType(line.ToLower());
            }
            else if (data.StartsWith("SYNTAX") )
            {
                OidType = OID_TYPES.syntax;
            }
            else if (data.StartsWith("ACCESS") || data.StartsWith("MAX-ACCESS"))
            {
                OidType = OID_TYPES.access;
            }
            else if (data.StartsWith("STATUS"))
            {
                OidType = OID_TYPES.status;
            }
            else if (data.StartsWith("DESCRIPTION"))
            {
                OidType = OID_TYPES.description;
            }
            else if (line.StartsWith("::="))
            {
                OidType = OID_TYPES.parentLine;
            }
            else
            {
                OidType = OID_TYPES.unKnown;
            }

        }

        private OID_TYPES GetNameType(string line)
        {
            OID_TYPES ot = OID_TYPES.name; 

            if(line.ToLower().Contains("table") || line.Contains("entry"))
            {
                ot = OID_TYPES.mibEnt;
            }
            return ot;
        }
    }
}

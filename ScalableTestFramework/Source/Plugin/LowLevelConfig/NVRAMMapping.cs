using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.LowLevelConfig
{
    public class NVRAMMapping
    {
        public Dictionary<string, Dictionary<string, string>> keyMapping = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, string> textMapping = new Dictionary<string, string>();
        public Dictionary<string, string> validationMapping = new Dictionary<string, string>();

        public NVRAMMapping()
        {

            //keyMapping.Add("PartialClean", "Clean");

            keyMapping.Add("JDIMfgReset", new Dictionary<string, string>());
            keyMapping["JDIMfgReset"].Add("True", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""JDIMfgReset"",""hex"", ""01000000""]");
            keyMapping["JDIMfgReset"].Add("False", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""JDIMfgReset"",""hex"", ""00000000""]");

            keyMapping.Add("ExecutionMode", new Dictionary<string, string>());
            keyMapping["ExecutionMode"].Add("Production", @"[""0429e79e-d9ba-412e-a2bc-1f3d245041ce"",""ExecutionMode"",""str"", ""Production""]");
            keyMapping["ExecutionMode"].Add("Development", @"[""0429e79e-d9ba-412e-a2bc-1f3d245041ce"",""ExecutionMode"",""str"", ""Development""]");

            keyMapping.Add("SaveRecoverFlags", new Dictionary<string, string>());
            keyMapping["SaveRecoverFlags"].Add("None", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""SaveRecoverFlags"",""hex"", ""0000""]");
            keyMapping["SaveRecoverFlags"].Add("Save", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""SaveRecoverFlags"",""hex"", ""0100""]");
            keyMapping["SaveRecoverFlags"].Add("Recover", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""SaveRecoverFlags"",""hex"", ""0001""]");
            keyMapping["SaveRecoverFlags"].Add("SaveRecover", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""SaveRecoverFlags"",""hex"", ""0101""]");

            keyMapping.Add("RebootCrashMode", new Dictionary<string, string>());
            keyMapping["RebootCrashMode"].Add("Enable", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""RebootCrashMode"",""hex"", ""01000000""]");
            keyMapping["RebootCrashMode"].Add("Disable", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""RebootCrashMode"",""hex"", ""00000000""]");

            keyMapping.Add("Language", new Dictionary<string, string>());
            keyMapping["Language"].Add("English", @"[""942ab8a2-f9be-4455-b641-113b1f5391f3"",""CRLang"",""hex"", ""09040000""]");
            keyMapping["Language"].Add("Korean", @"[""942ab8a2-f9be-4455-b641-113b1f5391f3"",""CRLang"",""hex"", ""12040000""]");

            keyMapping.Add("SuppressUsed", new Dictionary<string, string>());
            keyMapping["SuppressUsed"].Add("True", @"[""47C51269-08AD-491A-A318-97CF64CAF012"",""SuppressUsed"",""hex"", ""01""]");
            keyMapping["SuppressUsed"].Add("False", @"[""47C51269-08AD-491A-A318-97CF64CAF012"",""SuppressUsed"",""hex"", ""00""]");

            textMapping.Add("SerialNumberBool", @"[""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""SerialNumber"",""str"", ");
            textMapping.Add("ModelNumberBool", @"[""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""ModelNumber"",""str"", ");
            textMapping.Add("CRDefSleep", @"[""942ab8a2-f9be-4455-b641-113b1f5391f3"",""CRDefSleep"",""hex"", ");

            validationMapping.Add("JDIMfgReset", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""JDIMfgReset""]");
            validationMapping.Add("ExecutionMode", @"[""0429e79e-d9ba-412e-a2bc-1f3d245041ce"",""ExecutionMode""]");
            validationMapping.Add("SaveRecoverFlags", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""SaveRecoverFlags""]");
            validationMapping.Add("RebootCrashMode", @"[""845E3285-C67C-4F4B-9AA4-0AE91BD35089"",""RebootCrashMode""]");
            validationMapping.Add("Language", @"[""942ab8a2-f9be-4455-b641-113b1f5391f3"",""CRLang""]");
            validationMapping.Add("SuppressUsed", @"[""47C51269-08AD-491A-A318-97CF64CAF012"",""SuppressUsed""]");
            validationMapping.Add("SerialNumber", @"[""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""SerialNumber""]");
            validationMapping.Add("CRDefSleep", @"[""942ab8a2-f9be-4455-b641-113b1f5391f3"",""CRDefSleep""]");
            validationMapping.Add("ModelNumber", @"[""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""ModelNumber""]");

        }

        public string GetValidation(string field)
        {
            return validationMapping[field];
        }

        public string GetNVRAM(string field, string value)
        {
            return textMapping[field] + @"""" + value + @"""]";
        }

        //public string GetLocation(string location)
        //{
        //    return @"""fim_bundle"": """ + location + @"""";
        //}

        public string GetCRSleep(string timeval)
        {
            string jsonString = "";
            if (!string.IsNullOrEmpty(timeval))
            {
                int temp = int.Parse(timeval);
                string hexVal = temp.ToString("X");
                while (hexVal.Length < 8)
                {
                    hexVal += "00";
                }
                jsonString = GetNVRAM("CRDefSleep", hexVal);

            }
            return jsonString;
        }


        public string GetPartialClean()
        {
            //string json = @"[""845E3285-C67C-4f4b-9AA4-0AE91BD35089"",""LLDataStoreBootAction"",""hex"", ""01000000""],[""845E3285-C67C-4f4b-9AA4-0AE91BD35089"",""PMDataStoreBootAction"",""hex"", ""01000000""],
            //                [""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""clear_all_data"",""str"",""DUMMY""]";

            string json = @"[""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""clear_all_data"",""str"",""DUMMY""], [""1429e79e-d9ba-412e-a2bc-1f3d245041ce"",""REBOOT_SYSTEM"",""str"",""DUMMY""]";

            return json;

        }

    }
}

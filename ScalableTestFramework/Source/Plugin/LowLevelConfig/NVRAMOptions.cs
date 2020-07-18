using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.LowLevelConfig
{
    public class NVRAMOptions
    {
        //public Dictionary<string, string> JDIOptions = new Dictionary<string, string>();
        //public Dictionary<string, string> ExecutionModeOptions = new Dictionary<string, string>();
        //public Dictionary<string, string> SaveRecoverFlagsOptions = new Dictionary<string, string>();
        //public Dictionary<string, string> RebootCrashModeOptions = new Dictionary<string, string>();
        //public Dictionary<string, string> CRDefSleepOptions = new Dictionary<string, string>();
        //public Dictionary<string, string> LanguageOptions = new Dictionary<string, string>();
        //public Dictionary<string, string> SuppressUsed = new Dictionary<string, string>();

        public List<string> JDIOptions = new List<string>(new string[] { "", "True", "False" });
        public List<string> ExecutionModeOptions = new List<string>(new string[] { "", "Production", "Development" });
        public List<string> SaveRecoverFlagsOptions = new List<string>(new string[] { "", "None", "Save", "Recover", "SaveRecover" });
        public List<string> RebootCrashModeOptions = new List<string>(new string[] { "", "Enable", "Disable" });
        public List<string> CRDefSleepOptions = new List<string>(new string[] { "", "0", "15", "30", "60", "120" });
        public List<string> LanguageOptions = new List<string>(new string[] { "", "English", "Korean" });
        public List<string> SuppressUsedOptions = new List<string>(new string[] { "", "True", "False" });
        public List<string> PartialCleanOptions = new List<string>(new string[] { "", "Clean" });

        public NVRAMOptions()
        {
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    class HpkInstallTableData
    {

        public string PackageName { get; set; }
        public string Uuid { get; set; }
        public string FilePath { get; set; }

        public HpkInstallTableData(string packagename, string uuid, string filepath)
        {
            PackageName = packagename;
            Uuid = uuid;
            FilePath = filepath;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ShareTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Win32Share w = new Win32Share();

            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            Console.WriteLine(name != null ? name.ToString() : "Unknown");

            //Create(args[1], args[0]);

            SharedDrive drive = new SharedDrive(args[0]);
            drive.Create(args[1], "This is the share", "Everyone", FileSystemRights.ReadAndExecute, FileSystemRights.Synchronize);

            //SharedDrive drive1 = new SharedDrive(args[0]);
            //drive1.SetUserRights("AMERICAS", "youngmak", FileSystemRights.ReadAndExecute, FileSystemRights.Synchronize);
            //drive1.SetUserRights("AMERICAS", "schroath", FileSystemRights.ReadAndExecute, FileSystemRights.Synchronize);

            //var y = Environment.OSVersion;
            //if ((y.Version.Major == 6 && y.Version.Minor < 2) || y.Version.Minor < 6)
            //{

            //}
            //else if (y.Version.Minor)
            //Win32Share.Foo();
        }

        static void Create(string name, string path)
        {
            //Win32Share share = new Win32Share();
            //share.Description = "This is the shared";
            //share.Name = name;
            //share.Path = path;
            //share.Type = Win32Share.ShareType.DiskDrive;
            //share.MaximumAllowed = null;
            //share.P

            //    input["Description"] = description;
            //input["Name"] = name;
            //input["Path"] = path;
            //input["Type"] = type;
            //input["MaximumAllowed"] = null;
            //input["Password"] = null;
            //input["Access"] = null;
        }
    }
}

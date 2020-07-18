using HP.ScalableTest.Data.TraceLog;
using HP.ScalableTest.Framework.Settings;
using System;
using System.IO;
using System.Linq;

namespace DirectoryMonitor
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                GlobalSettings.Clear();
                GlobalSettings.Load(args[1]);
                // Console.WriteLine(args[0]);
                // Console.WriteLine(args[1]);

                string sessionid = args[2];

                using (TraceLogContext context = new TraceLogContext())
                {
                    var activity = ActivityExecution.SelectBySession(context, sessionid);
                    var activityCount = activity.Count(x => x.ActivityType.Equals("ScanToFolder"));

                    if (activityCount > 0)
                    {
                        string[] files = Directory.GetFiles(args[0], sessionid, SearchOption.AllDirectories);

                        if (activityCount <= files.Count())
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TraceFactory.Logger.Error(ex.Message);
            }
            return -1;
        }
    }
}
using System;
using System.Globalization;
using System.IO;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.PluginSupport.Connectivity.KiwiSyslog
{
    /// <summary>
    /// Contains Kiwi Syslog service operations like Start, Stop and validate entry
    /// </summary>
    public static class KiwiSyslogApplication
    {
        /// <summary>
        /// Starting Kiwi Syslog Service
        /// </summary>
        /// <returns>Returns true if the service is started else false</returns>
        public static bool StartSyslogServer()
        {
            return NetworkUtil.ExecuteCommandAndValidate("net start \"Kiwi Syslog Server\"");
        }

        /// <summary>
        /// Stop the Kiwi Syslog Service
        /// </summary>
        /// <returns>Returns true if the service is stopped else false</returns>
        public static bool StopSyslogServer()
        {
            return NetworkUtil.ExecuteCommandAndValidate("net stop \"Kiwi Syslog Server\"");
        }

        /// <summary>
        /// Tells whether the given entry is found between the date range.
        /// </summary>
        /// <param name="startTime">Start Time</param>
        /// <param name="endTime">End Time</param>
        /// <param name="entry">Entry to be found in the sys log</param>
        /// <returns>Returns true if the entry is found between the given dates else return false</returns>
        public static bool ValidateEntry(DateTime startTime, DateTime endTime, string entry)
        {
            string[] lines = File.ReadAllLines(@"C:\Program Files (x86)\Syslogd\Logs\SyslogCatchAll.txt");

            Logger.LogInfo(@"Searching the following Syslog entry in C:\Program Files (x86)\Syslogd\Logs\SyslogCatchAll.txt");
            Logger.LogInfo("Start Time: {0}  End Time: {1}  Entry: {2}".FormatWith(startTime.ToString(CultureInfo.CurrentCulture), endTime.ToString(CultureInfo.CurrentCulture), entry));

            foreach (string line in lines)
            {
                string[] fileds = line.Split('\t');

                // first field should be date time
                DateTime time;

                if (DateTime.TryParse(fileds[0], out time))
                {
                    if (time >= startTime && time <= endTime && line.Contains(entry.Trim()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
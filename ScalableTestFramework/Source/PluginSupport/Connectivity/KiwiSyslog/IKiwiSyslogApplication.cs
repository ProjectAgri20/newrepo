using System;
using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Connectivity.KiwiSyslog
{
    [ServiceContract]
    public interface IKiwiSyslogApplication
    {
        /// <summary>
        /// Starting Kiwi Syslog Service
        /// </summary>
        /// <returns>Returns true if the service is started else false</returns>
        [OperationContract]
        bool StartKiwiSyslogServer();

        /// <summary>
        /// Stop the Kiwi Syslog Service
        /// </summary>
        /// <returns>Returns true if the service is stopped else false</returns>
        [OperationContract]
        bool StopKiwiSyslogServer();

        /// <summary>
        /// Tells whether the given entry is found between the date range.
        /// </summary>
        /// <param name="startTime">Start Time</param>
        /// <param name="endTime">End Time</param>
        /// <param name="entry">Entry to be found in the sys log</param>
        /// <returns>Returns true if the entry is found between the given dates else return false</returns>
        [OperationContract]
        bool ValidateEntry(DateTime startTime, DateTime endTime, string entry);
    }
}

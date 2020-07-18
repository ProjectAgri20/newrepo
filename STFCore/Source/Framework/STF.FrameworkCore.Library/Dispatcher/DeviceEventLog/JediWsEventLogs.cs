using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.Dispatcher.DeviceEventLog
{
    /// <summary>
    /// Jedi Web Service Event Logs
    /// </summary>
    public class JediWsEventLogs
    {
        private readonly JediDevice _device = null;
        private readonly DateTime _startTime;

        private readonly string _deviceServiceUri = "urn:hp:imaging:con:service:systemconfiguration:SystemConfigurationService:Logs";
        private readonly string _deviceTarget = "systemconfiguration";

        /// <summary>
        /// Indicates if the device is Jedi device
        /// </summary>
        public bool IsJediDevice { get; private set; }

        private int _idCounter = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWsEventLogs"/> class.
        /// </summary>
        /// <param name="deviceIPAddress">The device IP address.</param>
        /// <param name="startTime">The start time of the testing session.</param>
        public JediWsEventLogs(JediDevice device, DateTime startTime)
        {
            _device = device;
            _startTime = startTime;
            IsJediDevice = true;
        }
        /// <summary>
        /// Public interface for retrieving the device event logs that are generated on or after the given startup time.
        /// </summary>
        /// <returns>WSEventLogList</returns>
        public WSEventLogList RetrieveEventLogs()
        {
            WSEventLogList listLogEvents = new WSEventLogList();
            XElement eLogs = GetDeviceEventLogs();
            if (eLogs != null && CountChildName(eLogs, "Log") > 0)
            {
                ProcessEventLogs(eLogs, listLogEvents);
            }
            return listLogEvents;
        }
        /// <summary>
        /// Start of the process for parsing the XML data containing the device event logs.
        /// </summary>
        /// <param name="eLogs">XElement data of events</param>
        /// <param name="listLogEvents">The list of events.</param>
        private void ProcessEventLogs(XElement eLogs, WSEventLogList listLogEvents)
        {
            List<XElement> xeList = GetElements(eLogs, "Log");
            foreach (XElement pEntryLog in xeList)
            {
                ProcessEvents(pEntryLog, listLogEvents);
            }
        }
        /// <summary>
        /// Have the actual event from the XML. Determine if it meets the date and time requirements; if so add to the list.
        /// </summary>
        /// <param name="pEntryLog">XElement data of events.</param>
        /// <param name="listLogEvents">WSEventLogList</param>
        private void ProcessEvents(XElement pEntryLog, WSEventLogList listLogEvents)
        {
            string eventType = GetChildElementValue(pEntryLog, "Type");
            List<XElement> xeEntries = GetElements(pEntryLog, "Entry");

            foreach (XElement eCode in xeEntries)
            {
                WSEventLog log = GetEventInformation(eCode);
                log.EventType = eventType;

                // only add the event if its time is equal or greater then the member device startup time.
                if (log.EventDateTime >= _startTime)
                {
                    listLogEvents.Add(log);
                }
            }
        }
        /// <summary>
        /// Uses XElement methods to retrieve element values; creates the log and sets member variables
        /// </summary>
        /// <param name="eCode">XElement Data</param>
        /// <returns>WSEventLog</returns>
        private WSEventLog GetEventInformation(XElement eCode)
        {
            WSEventLog log = new WSEventLog();

            log.EventId = _idCounter++;
            log.EventCode = GetChildElementValue(eCode, "EventCode");
            log.EventDateTime = FixEventDateTime(GetChildElementValue(eCode, "TimeStamp"));
            log.EventDescription = GetChildElementValue(eCode, "Description");
            log.EventDescription += " " + GetEventMessage(GetElements(eCode, "KeyValuePair"));

            return log;
        }
        private string GetEventMessage(List<XElement> payloads)
        {
            string msg = string.Empty;
            foreach (XElement pv in payloads)
            {
                string key = GetChildElementValue(pv, "Key");
                if (key.Equals("MESSAGE"))
                {
                    msg = GetChildElementValue(pv, "ValueString");
                    break;
                }
            }
            return msg;
        }
        /// <summary>
        /// Fixes the event date time from the device.
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <returns>DateTime</returns>
        private DateTime FixEventDateTime(string timeStamp)
        {
            timeStamp = timeStamp.Replace('T', ' ');
            return DateTime.Parse(timeStamp);
        }
        /// <summary>
        /// Attempts to retrieve the device event logs. If the device is not Jedi will set the IsJediDevice property to false.
        /// </summary>
        /// <returns></returns>
        private XElement GetDeviceEventLogs()
        {
            XElement xeEventLogs = null;

            if (_device != null)
            {
                xeEventLogs = _device.WebServices.GetDeviceTicket(_deviceTarget, _deviceServiceUri);
            }
            else
            {
                IsJediDevice = false;
            }
            return xeEventLogs;
        }
        /// <summary>
        /// Returns how many times the given child name appears in the given XElement list.
        /// </summary>
        /// <param name="parentLog">XElement</param>
        /// <param name="childLocalName">string</param>
        /// <returns>int</returns>
        public int CountChildName(XElement parentLog, string childLocalName)
        {
            int count = 0;
            if (parentLog.HasElements)
            {
                count = parentLog.Elements().Count(x => x.Name.LocalName.Equals(childLocalName, StringComparison.OrdinalIgnoreCase));

            }
            return count;
        }
        /// <summary>
        /// Returns the value of the given element name held int he XElement list
        /// </summary>
        /// <param name="parent">XElement</param>
        /// <param name="childLocalName">string</param>
        /// <returns>string</returns>
        public string GetChildElementValue(XElement parent, string childLocalName)
        {
            string result = string.Empty;
            if (parent.HasElements)
            {
                var childElem = parent.Elements().FirstOrDefault(x => x.Name.LocalName.Equals(childLocalName, StringComparison.OrdinalIgnoreCase));
                if (childElem != null)
                {
                    result = childElem.Value;
                }
            }
            return result;
        }
        /// <summary>
        /// Returns a list of XElements that do NOT contain the given parent name
        /// </summary>
        /// <param name="root">XElement</param>
        /// <param name="localName">string</param>
        /// <returns>List[XElement]</returns>
        public List<XElement> GetElements(XElement root, string localName)
        {
            var result = (
                        from el in root.Descendants().Where(x => x.Name.LocalName == localName)
                        select el
                        ).ToList();
            return result;
        }
    }
}

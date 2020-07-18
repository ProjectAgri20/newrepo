using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using HP.RDL.EDT.ContractDDS;
using HP.RDL.DurationDataCoreLibrary;
using HP.RDL.EDT.Common;

namespace HP.RDL.EDT.ClientDDS
{
    public class AccessDDS
    {
        private string _dBServer { get; set; }
        private readonly string _database = "ctsl_duration";
        private readonly string _userId = "duration_admin";
        private readonly string _password = "duration_admin";
        public string GetLastError { get; private set; }

        public string FaultEventId { get; set; }
        public string FirmwareId { get; set; }
        public string IFaceId { get; set; }
        public string MemoryValueId { get; set; }
        public string PowerBootId { get; set; }
        public string SleepWakeId { get; set; }
        public string TestInstanceId { get; set; }

        private readonly ContractServiceClient _ddsAccess = null;

        public bool IsError
        {
            get { return (string.IsNullOrEmpty(GetLastError) == false) ? true : false; }
        }

        /// <summary>
        /// Constructor: Requires which database server to use prior to instantiating
        /// </summary>
        /// <param name="dbs">DATABASE</param>
        public AccessDDS(string environment)
        {

           
            NameValueCollection systems = ConfigurationManager.GetSection("DDS") as NameValueCollection;
            _dBServer = systems[environment];
            _ddsAccess = ContractServiceClient.Create(_dBServer);
            TestInstanceId = string.Empty;
        }

        /// <summary>
        /// Destructor: will close the database connection if not null.
        /// </summary>
        ~AccessDDS()
        {
            if (_ddsAccess != null)
            {
                _ddsAccess.Close();
            }
        }

        /// <summary>
        /// Sanity check to ensure the service is running
        /// </summary>
        internal void AccessDDSReady()
        {
            _ddsAccess.Channel.AccessDDSReady();
        }

        /// <summary>
        /// Closes the provided test instance.
        /// Returns true on success.
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="stopPrnCnt">int</param>
        /// <param name="stopScnCnt">int</param>
        /// <param name="endDt">DateTime</param>
        /// <param name="stopReason">string</param>
        /// <returns>bool</returns>
        internal bool CloseTestInstance(string testInstanceId, int stopPrnCnt, int stopScnCnt, DateTime endDt,
            string stopReason)
        {
            DDSResult dr =
                _ddsAccess.Channel.CloseTestInstance(testInstanceId, stopPrnCnt, stopScnCnt, endDt, stopReason);
            GetLastError = dr.ErrorMessage;

            return !IsError;
        }

        /// <summary>
        /// Creates a new test instance using the provided information. Returns true on success. The new test instance ID will be
        /// available in the Test Instance ID accessor. 
        /// </summary>
        /// <param name="testType">string</param>
        /// <param name="ipAddress">string</param>
        /// <param name="loginId">string</param>
        /// <param name="startPrnCnt">int</param>
        /// <param name="startScnCnt">int</param>
        /// <param name="vmpsNumber">string</param>
        /// <param name="startDt">date</param>
        /// <returns>bool</returns>
        internal bool CreateTestInstance(string testType, string ipAddress, string loginId, int startPrnCnt,
            int startScnCnt, string vmpsNumber, DateTime startDt)
        {
            if (!ValidDeviceForRun(ipAddress, testType))
            {
                return false;
            }

            DDSResult dr = _ddsAccess.Channel.CreateTestInstance(testType, ipAddress, loginId, startPrnCnt, startScnCnt,
                vmpsNumber, startDt);
            if (!dr.IsError)
            {
                TestInstanceId = dr.EventIdToString;
            }
            else
            {
                GetLastError = dr.ErrorMessage;
            }

            return !IsError;
        }

        /// <summary>
        /// Returns true if a fault event exists for the given test instance ID and event time.
        /// If true the property FaultEventId will be set.
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="eventTime">DateTime</param>
        /// <returns>bool</returns>
        internal bool ExistFaultEvent(string testInstanceId, DateTime eventTime)
        {
            DDSResult dr = _ddsAccess.Channel.ExistFaultEvent(testInstanceId, eventTime);
            if (dr.IsError)
            {
                FaultEventId = dr.EventIdToString;
            }

            return dr.IsError;
        }

        /// <summary>
        /// Returns true if a FIM load event exists for the given test instance ID and event time.
        /// If true the property FirmwareId will be set.
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="eventTime">DateTime</param>
        /// <returns>bool</returns>
        internal bool ExistFIM(string testInstanceId, DateTime eventTime)
        {
            DDSResult dr = _ddsAccess.Channel.ExistFIM(testInstanceId, eventTime);
            if (dr.IsError)
            {
                FirmwareId = dr.EventIdToString;
            }

            return dr.IsError;
        }

        /// <summary>
        /// Returns true if an IFace exists for the given values.
        /// If true, the property IFaceId will be set.
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="iFaceType">string</param>
        /// <param name="iFaceOp">string</param>
        /// <param name="iFaceSubOp">string</param>
        /// <returns>bool</returns>
        internal bool ExistIFace(string testInstanceId, string iFaceType, string iFaceOp, string iFaceSubOp)
        {
            DDSResult dr = _ddsAccess.Channel.ExistIFace(testInstanceId, iFaceType, iFaceOp, iFaceSubOp);
            if (dr.IsError)
            {
                IFaceId = dr.EventIdToString;
            }

            return dr.IsError;
        }

        /// <summary>
        /// Returns true if a Memory value exists for the given firmware version and date time
        /// </summary>
        /// <param name="fwVersion"></param>
        /// <param name="insertDate"></param>
        /// <returns>bool</returns>
        internal bool ExistMemoryValue(string fwVersion, DateTime insertDate)
        {
            DDSResult dr = _ddsAccess.Channel.ExistMemoryValue(fwVersion, insertDate);
            if (dr.IsError)
            {
                MemoryValueId = dr.EventIdToString;
            }

            return dr.IsError;
        }

        /// <summary>
        /// Returns true if a power boot event exists for the given test instance ID and event time.
        /// If true the property PowerBootId will be set.
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="eventTime">DateTime</param>
        /// <returns>bool</returns>
        internal bool ExistPowerBoot(string testInstanceId, DateTime eventTime)
        {
            DDSResult dr = _ddsAccess.Channel.ExistPowerBoot(testInstanceId, eventTime);
            if (dr.IsError)
            {
                PowerBootId = dr.EventIdToString;
            }

            return dr.IsError;
        }

        /// <summary>
        /// Returns true if a power boot event exists for the given test instance ID and event time.
        /// If true the property PowerBootId will be set.
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="eventTime">DateTime</param>
        /// <returns>bool</returns>
        internal bool ExistSleepWake(string testInstanceId, DateTime eventTime)
        {
            DDSResult dr = _ddsAccess.Channel.ExistSleepWake(testInstanceId, eventTime);
            if (dr.IsError)
            {
                SleepWakeId = dr.EventIdToString;
            }

            return dr.IsError;
        }

        /// <summary>
        /// Determines if a test instance exists for the given IP Address and start time.
        /// If it does, the property TestInstanceId will by set.
        /// </summary>
        /// <param name="ipAddress">string</param>
        /// <param name="startDt">DateTime</param>
        /// <returns></returns>
        internal bool ExistTestInstance(string ipAddress, DateTime startDt)
        {
            DDSResult dr = _ddsAccess.Channel.ExistTestInstance(ipAddress, startDt);
            if (dr.IsError)
            {
                TestInstanceId = dr.EventIdToString;
            }

            return dr.IsError;
        }

        /// <summary>
        /// Retrieves Domain Value for the specified Domain Value Name
        /// </summary>
        /// <param name="domainValueName"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        internal List<string> GetDomainValues(string domainValueName, bool active = true)
        {
            var dr = _ddsAccess.Channel.GetDomainValuesByName(domainValueName);
            if (dr.IsError)
            {
                GetLastError = dr.ErrorMessage;
            }

            var drValues = dr.Value.Split(new[]{"#"}, StringSplitOptions.RemoveEmptyEntries);
            List<string> domainValues = new List<string>();
            foreach (var drValue in drValues)
            { 
                var drColumns = drValue.Split(new[] { "," }, StringSplitOptions.None);
                if (!active)
                {
                    domainValues.Add(drColumns.ElementAt(2));
                }
                else
                {
                    if (drColumns.ElementAt(4).Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        domainValues.Add(drColumns.ElementAt(2));
                    }
                }
                
            }

            return domainValues;
        }

        /// <summary>
        /// Retrieves Domain Value 2 info for the specified Domain Value Name
        /// </summary>
        /// <param name="domainValueName"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        internal List<string> GetDomainValue2(string domainValueName, bool active = true)
        {
            var dr = _ddsAccess.Channel.GetDomainValuesByName(domainValueName);
            if (dr.IsError)
            {
                GetLastError = dr.ErrorMessage;
            }

            var drValues = dr.Value.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> domainValues = new List<string>();
            foreach (var drValue in drValues)
            {
                var drColumns = drValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (!active)
                {
                    domainValues.Add(drColumns.ElementAt(3));
                }
                else
                {
                    if (drColumns.ElementAt(4).Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        domainValues.Add(drColumns.ElementAt(3));
                    }
                }
            }

            return domainValues;
        }

        /// <summary>
        /// Retrieves the values for the given domain value name. Caller will need to parse the returned records.
        /// Each record is comma separated and end of record is marked by a #.
        /// </summary>
        /// <param name="dvName">string</param>
        /// <returns>string</returns>
        internal string GetDomainValueInfo(string dvName)
        {
            var dr = _ddsAccess.Channel.GetDomainValuesByName(dvName);
            if (dr.IsError)
            {
                GetLastError = dr.ErrorMessage;
            }

            return dr.Value;
        }

        /// <summary>
        /// Ensures the given device (by IP Address) has been initialized within the DDS database for the given test suit (Sleep Wake, Power Boot, Firmware Load).
        /// Returns true if initialized.
        /// </summary>
        /// <param name="ipAddress">string</param>
        /// <param name="testSuit">string</param>
        /// <returns>bool</returns>
        internal bool ValidDeviceForRun(string ipAddress, string testSuit)
        {
            DDSResult dr = _ddsAccess.Channel.ValidDeviceForRun(ipAddress, testSuit);
            GetLastError = dr.ErrorMessage;

            return !IsError;
        }

        /// <summary>
        /// Returns true if the given HP Login ID is a member of the DDS system.
        /// </summary>
        /// <param name="loginId">string</param>
        /// <returns>bool</returns>
        internal bool ValidLoginId(string loginId)
        {
            DDSResult dr = _ddsAccess.Channel.ValidLoginId(loginId);
            GetLastError = dr.ErrorMessage;

            return !IsError;
        }

        /// <summary>
        /// Returns true if able to insert a new fault event and chain it to the given event ID
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="previousEventId">string</param>
        /// <param name="eventType">string</param>
        /// <param name="eventSubType">string</param>
        /// <param name="opInProgress">string</param>
        /// <param name="recovery">string</param>
        /// <param name="jobDisposition">string</param>
        /// <param name="eventDateTime">DateTime</param>
        /// <param name="timeToRecovery">string</param>
        /// <param name="comments">string</param>
        /// <param name="qcCR">string</param>
        /// <param name="faultCode">string</param>
        /// <param name="rootCause">string</param>
        /// <returns>bool</returns>
        internal bool InsertChainedFault(string testInstanceId, string previousEventId,
            string eventType /*Error or Jam*/, string eventSubType, string opInProgress,
            string recovery, string jobDisposition, DateTime eventDateTime, int timeToRecovery, string comments,
            string qcCR, string faultCode, string rootCause = "")
        {
            DDSResult dr = _ddsAccess.Channel.InsertChainedFault(testInstanceId, previousEventId, eventType,
                eventSubType, opInProgress, recovery, jobDisposition, eventDateTime, timeToRecovery, comments, qcCR,
                faultCode, rootCause);
            GetLastError = dr.ErrorMessage;
            if (!dr.IsError)
            {
                FaultEventId = dr.EventIdToString;
            }

            return !dr.IsError;
        }

        /// <summary>
        /// Returns true if success in creating and inserting a new fault event for the given test instance
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="eventType">string</param>
        /// <param name="eventSubType">string</param>
        /// <param name="opInProgress">string</param>
        /// <param name="recovery">string</param>
        /// <param name="jobDisposition">string</param>
        /// <param name="eventDateTime">DateTime</param>
        /// <param name="timeToRecovery">string</param>
        /// <param name="comments">string</param>
        /// <param name="qcCR">string</param>
        /// <param name="faultCode">string</param>
        /// <param name="rootCause">string</param>
        /// <returns>bool</returns>
        internal bool InsertFault(string testInstanceId, string eventType /*Error or Jam*/, string eventSubType,
            string opInProgress, string recovery,
            string jobDisposition, DateTime eventDateTime, int timeToRecovery, string comments, string qcCR,
            string faultCode, string rootCause = "")
        {
            DDSResult dr = _ddsAccess.Channel.InsertFaultEvent(testInstanceId, eventType, eventSubType, opInProgress,
                recovery, jobDisposition, eventDateTime, timeToRecovery, comments, qcCR, faultCode, rootCause);
            GetLastError = dr.ErrorMessage;
            if (!dr.IsError)
            {
                FaultEventId = dr.EventIdToString;
            }

            return !dr.IsError;
        }
       

        /// <summary>
        /// Returns true if successful in creating and inserting a new FIM event for the given test instance
        /// </summary>
        /// <param name="tid">string</param>
        /// <param name="fimMethod">string</param>
        /// <param name="stamp">string</param>
        /// <param name="oldFW">string</param>
        /// <param name="newFW">string</param>
        /// <param name="timeToReady">DateTime</param>
        /// <param name="comments">string</param>
        /// <returns>bool</returns>
        internal bool InsertFIM(string tid, string fimMethod, DateTime stamp, string oldFW, string newFW,
            int timeToReady, string comments)
        {
            DDSResult dr = _ddsAccess.Channel.InsertFIM(tid, fimMethod, stamp, oldFW, newFW, timeToReady, comments);
            GetLastError = dr.ErrorMessage;
            if (!dr.IsError)
            {
                FirmwareId = dr.EventIdToString;
            }

            return !dr.IsError;
        }

        /// <summary>
        /// Returns true if successful in creating and inserting a new IFace event for the given test instance
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="iFaceType">string</param>
        /// <param name="iFaceOperation">string</param>
        /// <param name="iFaceSubOperation">string</param>
        /// <param name="totalTime">int</param>
        /// <param name="exeCount">int</param>
        /// <param name="dataCount">int</param>
        /// <param name="comments">string</param>
        /// <returns>bool</returns>
        internal bool InsertIFace(string testInstanceId, string iFaceType, string iFaceOperation,
            string iFaceSubOperation, int totalTime, int exeCount, int dataCount, string comments)
        {
            DDSResult dr = _ddsAccess.Channel.InsertIFace(testInstanceId, iFaceType, iFaceOperation, iFaceSubOperation,
                totalTime, exeCount, dataCount, comments);
            GetLastError = dr.ErrorMessage;
            if (!dr.IsError)
            {
                IFaceId = dr.EventIdToString;
            }

            return !dr.IsError;
        }

        /// <summary>
        /// Returns true if successful in creating and inserting a new Memory Value for the given IP Address and memory pool.
        /// </summary>
        /// <param name="ipAddress">string</param>
        /// <param name="firmware">string</param>
        /// <param name="memoryPool">string</param>
        /// <param name="bytesAvailable">int</param>
        /// <param name="highWaterMark">int</param>
        /// <param name="lowWaterMark">int</param>
        /// <param name="eventDt">DateTime</param>
        /// <param name="notes">string</param>
        /// <param name="buildType">string</param>
        /// <param name="reason">string</param>
        /// <returns>bool</returns>
        internal bool InsertMemoryValues(string ipAddress, string firmware, string memoryPool, int bytesAvailable,
            int highWaterMark, int lowWaterMark, DateTime eventDt,
            string notes = "", string buildType = "JIFT Adhoc1", string reason = "JediMemProfile")
        {
            DDSResult dr = _ddsAccess.Channel.InsertMemoryValues(ipAddress, firmware, memoryPool, bytesAvailable,
                highWaterMark, lowWaterMark, eventDt, notes, buildType, reason);
            GetLastError = dr.ErrorMessage;
            if (!dr.IsError)
            {
                MemoryValueId = dr.EventIdToString;
            }

            return !dr.IsError;
        }

        /// <summary>
        /// Returns true if successful in creating and inserting a new Power Boot event for the given test instance
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="method">string</param>
        /// <param name="reason">string</param>
        /// <param name="eventDateTime">DateTime</param>
        /// <param name="timeToReady">int</param>
        /// <param name="comments">string</param>
        /// <returns>bool</returns>
        internal bool InsertPowerBoot(string testInstanceId, string method, string reason, DateTime eventDateTime,
            int timeToReady, string comments)
        {
            DDSResult dr = _ddsAccess.Channel.InsertPowerBoot(testInstanceId, method, reason, eventDateTime,
                timeToReady, comments);
            GetLastError = dr.ErrorMessage;
            if (!dr.IsError)
            {
                PowerBootId = dr.EventIdToString;
            }

            return !dr.IsError;
        }

        /// <summary>
        /// Returns true if successful in creating and inserting a new Sleep Wake event for the given test instance.
        /// </summary>
        /// <param name="testInstanceId">string</param>
        /// <param name="sleepReason">string</param>
        /// <param name="wakeReason">string</param>
        /// <param name="eventDateTime">DateTime</param>
        /// <param name="timeToReady">int</param>
        /// <param name="comments">string</param>
        /// <returns>bool</returns>
        internal bool InsertSleepWake(string testInstanceId, string sleepReason, string wakeReason,
            DateTime eventDateTime, int timeToReady, string comments)
        {
            DDSResult dr = _ddsAccess.Channel.InsertSleepWake(testInstanceId, sleepReason, wakeReason, eventDateTime,
                timeToReady, comments);
            GetLastError = dr.ErrorMessage;
            if (!dr.IsError)
            {
                SleepWakeId = dr.EventIdToString;
            }

            return !dr.IsError;
        }

        internal List<Build> GetBuilds()
        {
           BuildList activeBuilds;
            using (DurationDataEntities dds = new DurationDataEntities(GetConnectionString()))
            {
              activeBuilds =  Build.GetBuilds(dds, true);
            }
            
            return activeBuilds.OrderBy(x=>x.Release).ToList();
        }

        internal List<Product> GetProducts()
        {
            ProductList products;
            using (DurationDataEntities dds = new DurationDataEntities(GetConnectionString()))
            {
                products = Product.GetProducts(dds, true);
            }

            products.Sort(products.Compare);
            return products;
        }

        internal List<Run> GetRuns(Guid buildId, Guid productId)
        {
            using (DurationDataEntities dds = new DurationDataEntities(GetConnectionString()))
            {
                var runs = Run.GetRunsByBuild(dds, buildId);
                return runs.FindAll(x => x.Product.ProductId == productId);
                
            }
        }

        internal Run GetRun(Guid runId)
        {
            using (DurationDataEntities dds = new DurationDataEntities(GetConnectionString()))
            {
                return Run.GetRunByRunId(dds, runId);

            }
        }

        private string GetConnectionString()
        {
            string connectionString = new System.Data.EntityClient.EntityConnectionStringBuilder
            {
                Metadata = "res://*",
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder
                {
                    InitialCatalog = _database,
                    DataSource = _dBServer,
                    IntegratedSecurity = false,
                    UserID = _userId,
                    Password = _password,
                }.ConnectionString
            }.ConnectionString;

            return connectionString;
        }

    }
}

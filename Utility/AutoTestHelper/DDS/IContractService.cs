using System;
using System.ServiceModel;
using HP.RDL.EDT.Common;

// defines the methods within the service

namespace HP.RDL.EDT.ContractDDS
{
	[ServiceContract]
	public interface IContractService
	{
		[OperationContract]
		void AccessDDSReady();

		[OperationContract]
		DDSResult CloseTestInstance(string testInstanceId, int stopPrnCnt, int stopScnCnt, DateTime endDt, string stopReason);

		[OperationContract]
		DDSResult CreateTestInstance(string testType, string ipAddress, string loginId, int startPrnCnt, int startScnCnt, string vmpsNumber, DateTime startDt);

		[OperationContract]
		DDSResult ExistFaultEvent(string testInstanceId, DateTime eventTime);

		[OperationContract]
		DDSResult ExistFIM(string testInstanceId, DateTime eventTime);

		[OperationContract]
		DDSResult ExistIFace(string testInstanceId, string iFaceType, string iFaceOperation, string iFaceSubOperation);

		[OperationContract]
		DDSResult ExistMemoryValue(string firmwareVersion, DateTime eventTime);

		[OperationContract]
		DDSResult ExistPowerBoot(string testInstanceId, DateTime eventTime);

		[OperationContract]
		DDSResult ExistSleepWake(string testInstanceId, DateTime eventTime);

		[OperationContract]
		DDSResult ExistTestInstance(string ipAddress, DateTime startDt);

		[OperationContract]
		DDSResult GetDomainValuesByName(string dvName);

		[OperationContract]
		DDSResult InsertChainedFault(string testInstanceId, string previousEventId, string eventType /*Error or Jam*/, string eventSubType, string opInProgress,
			string recovery, string jobDisposition, DateTime eventDateTime, int timeToRecovery, string comments, string qcCR, string faultCode, string rootCause = "");

		[OperationContract]
		DDSResult InsertFaultEvent(string testInstanceId, string eventType /*Error or Jam*/, string eventSubType, string opInProgress, string recovery,
			string jobDisposition, DateTime eventDateTime, int timeToRecovery, string comments, string qcCR, string faultCode, string rootCause = "");

		[OperationContract]
		DDSResult InsertFIM(string testInstanceId, string updMethod, DateTime eventDt, string oldFIMVersion, string newFIMVersion, int timeToReady, string comments);

		[OperationContract]
		DDSResult InsertIFace(String testInstanceId, String iFaceType, String iFaceOperation, String iFaceSubOperation, int totalTime, int exeCount, int dataCount, String comments);

		[OperationContract]
		DDSResult InsertMemoryValues(string ipAddress, string firmware, string memoryPool, int bytesAvailable, int highWaterMark, int lowWaterMark, DateTime eventDt,
			string notes = "", string buildType = "JIFT Adhoc1", string reason = "JediMemProfile");

		[OperationContract]
		DDSResult InsertPowerBoot(string testInstanceId, string method, string reason, DateTime eventDateTime, int timeToReady, string comments);

		[OperationContract]
		DDSResult ValidDeviceForRun(String ipAddress, string runTestType);

		[OperationContract]
		DDSResult ValidLoginId(string loginId);

		[OperationContract]
		DDSResult InsertSleepWake(string testInstanceId, string sleepReason, string wakeReason, DateTime eventDateTime, int timeToReady, string comments);

	}
}

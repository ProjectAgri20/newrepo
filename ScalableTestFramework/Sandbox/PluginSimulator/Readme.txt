Internal Plugin Simulator Readme

The internal plugin simulator leverages the same development code that is shipped with the SDK, but 
has been enhanced to support the internal framework service extensions.  It is also able to connect 
to production Asset Inventory and Document Library databases, removing the need to enter devices 
and documents into the mock.

There are several known limitations to the simulator, which are documented below.  These 
limitations may be changed or removed in the future as development resources allow.


Configuration Services
----------------------------------------------------------------------------------------------------
Asset Inventory:
  - Asset reservation keys are not populated, and descriptions are constructed differently.
  - GetServers(string) does not perform any filtering.
  - GetPrintDriverConfigurations(IDeviceInfo, PrintDriverInfo) does not perform any filtering.

Document Library:
  - GetDocuments(DocumentQuery) does not perform any filtering.
  - GetDocumentSets() methods are not implemented and return empty lists.

Environment Configuration:
  - The environment configuration mock does not pull from a database.  Any values that are to be 
    returned must be added to the collections in the mock object.
  - None of the methods perform any filtering.


Execution Services
----------------------------------------------------------------------------------------------------
Critical Section:
  - Mock does not connect to the global lock service.  Lock is always granted unless the mock
    is configured to throw an exception.

Data Logger:
  - Mock does not connect to the data log service.  Data is stored locally for review.
  - Methods that accept a FrameworkDataLog object are not implemented and will not add logs to the 
    mock data log tables.

File Repository:
  - Document share path must be entered manually via the DocumentShare property.  This path will 
    not be pulled from a database setting.

Session Runtime:
  - ReportAssetError, CollectDeviceMemoryProfile, MonitorForDocuments, and WaitIfPaused methods
    do not do anything.
  - GetOfficeWorkerEmailAddresses returns addresses from the OfficeWorkerEmailAddresses property.
  - RequestToSendPrintJob always returns true.
  - IsCitrixEnvironment always returns false.


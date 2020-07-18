:: This script us called on the client VMs to download all
:: the components needed for running virtual resources on the
:: client.

NETSH FIREWALL SET OPMODE DISABLE

CALL STF.InstallComponent.cmd %1 OfficeWorkerConsole
CALL STF.InstallComponent.cmd %1 ClientFactoryService
CALL STF.InstallComponent.cmd %1 PrintMonitorService
CALL STF.InstallComponent.cmd %1 Plugin
CALL STF.InstallComponent.cmd %1 PerfMonCollector

CD C:\VirtualResource\Distribution\ClientFactoryService
ClientFactoryConsole.exe %1 %2

:: This script is called on the client VMs to reboot.

NETSH FIREWALL SET OPMODE DISABLE

CD C:\VirtualResource\Distribution\ClientFactoryService
ClientFactoryConsole.exe %1 %2 %3

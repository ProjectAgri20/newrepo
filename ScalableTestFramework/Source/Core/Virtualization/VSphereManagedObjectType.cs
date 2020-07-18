namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// Managed object types in the vSphere API.
    /// </summary>
    /// <remarks>
    /// Source: https://www.vmware.com/support/developer/converter-sdk/conv61_apireference/index-mo_types.html#
    /// </remarks>
    public enum VSphereManagedObjectType
    {
        /// <summary>
        /// An alarm.
        /// </summary>
        Alarm,

        /// <summary>
        /// An alarm manager.
        /// </summary>
        AlarmManager,

        /// <summary>
        /// An authorization manager.
        /// </summary>
        AuthorizationManager,

        /// <summary>
        /// A certificate manager.
        /// </summary>
        CertificateManager,

        /// <summary>
        /// A cluster compute resource.
        /// </summary>
        ClusterComputeResource,

        /// <summary>
        /// A cluster EVC manager.
        /// </summary>
        ClusterEVCManager,

        /// <summary>
        /// A cluster profile.
        /// </summary>
        ClusterProfile,

        /// <summary>
        /// A cluster profile manager.
        /// </summary>
        ClusterProfileManager,

        /// <summary>
        /// A compute resource.
        /// </summary>
        ComputeResource,

        /// <summary>
        /// A container view.
        /// </summary>
        ContainerView,

        /// <summary>
        /// A custom fields manager.
        /// </summary>
        CustomFieldsManager,

        /// <summary>
        /// A customization spec manager.
        /// </summary>
        CustomizationSpecManager,

        /// <summary>
        /// A datacenter.
        /// </summary>
        Datacenter,

        /// <summary>
        /// A datastore.
        /// </summary>
        Datastore,

        /// <summary>
        /// A datastore namespace manager.
        /// </summary>
        DatastoreNamespaceManager,

        /// <summary>
        /// A diagnostic manager.
        /// </summary>
        DiagnosticManager,

        /// <summary>
        /// A distributed virtual portgroup.
        /// </summary>
        DistributedVirtualPortgroup,

        /// <summary>
        /// A distributed virtual switch.
        /// </summary>
        DistributedVirtualSwitch,

        /// <summary>
        /// A distributed virtual switch manager.
        /// </summary>
        DistributedVirtualSwitchManager,

        /// <summary>
        /// An environment browser.
        /// </summary>
        EnvironmentBrowser,

        /// <summary>
        /// An event history collector.
        /// </summary>
        EventHistoryCollector,

        /// <summary>
        /// An event manager.
        /// </summary>
        EventManager,

        /// <summary>
        /// An extensible managed object.
        /// </summary>
        ExtensibleManagedObject,

        /// <summary>
        /// An extension manager.
        /// </summary>
        ExtensionManager,

        /// <summary>
        /// A file manager.
        /// </summary>
        FileManager,

        /// <summary>
        /// A folder.
        /// </summary>
        Folder,

        /// <summary>
        /// A guest alias manager.
        /// </summary>
        GuestAliasManager,

        /// <summary>
        /// A guest authentication manager.
        /// </summary>
        GuestAuthManager,

        /// <summary>
        /// A guest file manager.
        /// </summary>
        GuestFileManager,

        /// <summary>
        /// A guest operations manager.
        /// </summary>
        GuestOperationsManager,

        /// <summary>
        /// A guest process manager.
        /// </summary>
        GuestProcessManager,

        /// <summary>
        /// A guest windows registry manager.
        /// </summary>
        GuestWindowsRegistryManager,

        /// <summary>
        /// A history collector.
        /// </summary>
        HistoryCollector,

        /// <summary>
        /// A host access manager.
        /// </summary>
        HostAccessManager,

        /// <summary>
        /// A host active directory authentication.
        /// </summary>
        HostActiveDirectoryAuthentication,

        /// <summary>
        /// A host authentication manager.
        /// </summary>
        HostAuthenticationManager,

        /// <summary>
        /// A host authentication store.
        /// </summary>
        HostAuthenticationStore,

        /// <summary>
        /// A host automatic start manager.
        /// </summary>
        HostAutoStartManager,

        /// <summary>
        /// A host boot device system.
        /// </summary>
        HostBootDeviceSystem,

        /// <summary>
        /// A host cache configuration manager.
        /// </summary>
        HostCacheConfigurationManager,

        /// <summary>
        /// A host certificate manager.
        /// </summary>
        HostCertificateManager,

        /// <summary>
        /// A host CPU scheduler system.
        /// </summary>
        HostCpuSchedulerSystem,

        /// <summary>
        /// A host datastore browser.
        /// </summary>
        HostDatastoreBrowser,

        /// <summary>
        /// A host datastore system.
        /// </summary>
        HostDatastoreSystem,

        /// <summary>
        /// A host date time system.
        /// </summary>
        HostDateTimeSystem,

        /// <summary>
        /// A host diagnostic system.
        /// </summary>
        HostDiagnosticSystem,

        /// <summary>
        /// A host directory store.
        /// </summary>
        HostDirectoryStore,

        /// <summary>
        /// A host ESX agent host manager.
        /// </summary>
        HostEsxAgentHostManager,

        /// <summary>
        /// A host firewall system.
        /// </summary>
        HostFirewallSystem,

        /// <summary>
        /// A host firmware system.
        /// </summary>
        HostFirmwareSystem,

        /// <summary>
        /// A host graphics manager.
        /// </summary>
        HostGraphicsManager,

        /// <summary>
        /// A host health status system.
        /// </summary>
        HostHealthStatusSystem,

        /// <summary>
        /// A host image configuration manager.
        /// </summary>
        HostImageConfigManager,

        /// <summary>
        /// A host kernel module system.
        /// </summary>
        HostKernelModuleSystem,

        /// <summary>
        /// A host local account manager.
        /// </summary>
        HostLocalAccountManager,

        /// <summary>
        /// A host local authentication.
        /// </summary>
        HostLocalAuthentication,

        /// <summary>
        /// A host memory system.
        /// </summary>
        HostMemorySystem,

        /// <summary>
        /// A host network system.
        /// </summary>
        HostNetworkSystem,

        /// <summary>
        /// A host patch manager.
        /// </summary>
        HostPatchManager,

        /// <summary>
        /// A host PCI passthru system.
        /// </summary>
        HostPciPassthruSystem,

        /// <summary>
        /// A host power system.
        /// </summary>
        HostPowerSystem,

        /// <summary>
        /// A host profile.
        /// </summary>
        HostProfile,

        /// <summary>
        /// A host profile manager.
        /// </summary>
        HostProfileManager,

        /// <summary>
        /// A host service system.
        /// </summary>
        HostServiceSystem,

        /// <summary>
        /// A host SNMP system.
        /// </summary>
        HostSnmpSystem,

        /// <summary>
        /// A host storage system.
        /// </summary>
        HostStorageSystem,

        /// <summary>
        /// A host system.
        /// </summary>
        HostSystem,

        /// <summary>
        /// A host VFlash manager.
        /// </summary>
        HostVFlashManager,

        /// <summary>
        /// A host virtual NIC manager.
        /// </summary>
        HostVirtualNicManager,

        /// <summary>
        /// A host VMotion system.
        /// </summary>
        HostVMotionSystem,

        /// <summary>
        /// A host VSAN internal system.
        /// </summary>
        HostVsanInternalSystem,

        /// <summary>
        /// A host VSAN system.
        /// </summary>
        HostVsanSystem,

        /// <summary>
        /// An HTTP NFC lease.
        /// </summary>
        HttpNfcLease,

        /// <summary>
        /// An inventory view.
        /// </summary>
        InventoryView,

        /// <summary>
        /// An IO filter manager.
        /// </summary>
        IoFilterManager,

        /// <summary>
        /// An IP pool manager.
        /// </summary>
        IpPoolManager,

        /// <summary>
        /// An ISCSI manager.
        /// </summary>
        IscsiManager,

        /// <summary>
        /// A license assignment manager.
        /// </summary>
        LicenseAssignmentManager,

        /// <summary>
        /// A license manager.
        /// </summary>
        LicenseManager,

        /// <summary>
        /// A list view.
        /// </summary>
        ListView,

        /// <summary>
        /// A localization manager.
        /// </summary>
        LocalizationManager,

        /// <summary>
        /// A managed entity.
        /// </summary>
        ManagedEntity,

        /// <summary>
        /// A managed object view.
        /// </summary>
        ManagedObjectView,

        /// <summary>
        /// A message bus proxy.
        /// </summary>
        MessageBusProxy,

        /// <summary>
        /// A network.
        /// </summary>
        Network,

        /// <summary>
        /// An opaque network.
        /// </summary>
        OpaqueNetwork,

        /// <summary>
        /// An option manager.
        /// </summary>
        OptionManager,

        /// <summary>
        /// An overhead memory manager.
        /// </summary>
        OverheadMemoryManager,

        /// <summary>
        /// An OVF manager.
        /// </summary>
        OvfManager,

        /// <summary>
        /// A performance manager.
        /// </summary>
        PerformanceManager,

        /// <summary>
        /// A profile.
        /// </summary>
        Profile,

        /// <summary>
        /// A profile compliance manager.
        /// </summary>
        ProfileComplianceManager,

        /// <summary>
        /// A profile manager.
        /// </summary>
        ProfileManager,

        /// <summary>
        /// A property collector.
        /// </summary>
        PropertyCollector,

        /// <summary>
        /// A property filter.
        /// </summary>
        PropertyFilter,

        /// <summary>
        /// A resource planning manager.
        /// </summary>
        ResourcePlanningManager,

        /// <summary>
        /// A resource pool.
        /// </summary>
        ResourcePool,

        /// <summary>
        /// A scheduled task.
        /// </summary>
        ScheduledTask,

        /// <summary>
        /// A scheduled task manager.
        /// </summary>
        ScheduledTaskManager,

        /// <summary>
        /// A search index.
        /// </summary>
        SearchIndex,

        /// <summary>
        /// A service instance.
        /// </summary>
        ServiceInstance,

        /// <summary>
        /// A service manager.
        /// </summary>
        ServiceManager,

        /// <summary>
        /// A session manager.
        /// </summary>
        SessionManager,

        /// <summary>
        /// A simple command.
        /// </summary>
        SimpleCommand,

        /// <summary>
        /// A storage pod.
        /// </summary>
        StoragePod,

        /// <summary>
        /// A storage resource manager.
        /// </summary>
        StorageResourceManager,

        /// <summary>
        /// A task.
        /// </summary>
        Task,

        /// <summary>
        /// A task history collector.
        /// </summary>
        TaskHistoryCollector,

        /// <summary>
        /// A task manager.
        /// </summary>
        TaskManager,

        /// <summary>
        /// A user directory.
        /// </summary>
        UserDirectory,

        /// <summary>
        /// A view.
        /// </summary>
        View,

        /// <summary>
        /// A view manager.
        /// </summary>
        ViewManager,

        /// <summary>
        /// A virtual application.
        /// </summary>
        VirtualApp,

        /// <summary>
        /// A virtual disk manager.
        /// </summary>
        VirtualDiskManager,

        /// <summary>
        /// A virtualization manager.
        /// </summary>
        VirtualizationManager,

        /// <summary>
        /// A virtual machine.
        /// </summary>
        VirtualMachine,

        /// <summary>
        /// A virtual machine compatibility checker.
        /// </summary>
        VirtualMachineCompatibilityChecker,

        /// <summary>
        /// A virtual machine provisioning checker.
        /// </summary>
        VirtualMachineProvisioningChecker,

        /// <summary>
        /// A virtual machine snapshot.
        /// </summary>
        VirtualMachineSnapshot,

        /// <summary>
        /// A VMware distributed virtual switch.
        /// </summary>
        VmwareDistributedVirtualSwitch,

        /// <summary>
        /// A VSAN upgrade system.
        /// </summary>
        VsanUpgradeSystem
    }
}

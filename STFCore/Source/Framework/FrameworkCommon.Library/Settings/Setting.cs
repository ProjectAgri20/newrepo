using System.ComponentModel;

namespace HP.ScalableTest.Framework.Settings
{
    /// <summary>
    /// Settings that are available using <see cref="GlobalSettings"/>.
    /// </summary>
    public enum Setting
    {
        /// <summary>
        /// The address of the SMTP server used to send admin emails
        /// </summary>
        AdminEmailServer,

        /// <summary>
        /// A comma separated list of email addresses for Admins
        /// </summary>
        AdminEmailAddress,

        /// <summary>
        /// The network location of the main EnterpriseTest database.
        /// </summary>
        EnterpriseTestDatabase,

        /// <summary>
        /// Enabled enterprise features of STB.
        /// </summary>
        EnterpriseEnabled,

        /// <summary>
        /// The network location of the DataLog database
        /// </summary>
        DataLogDatabase,

        /// <summary>
        /// The network location of the Document Library database.
        /// </summary>
        DocumentLibraryDatabase,

        /// <summary>
        /// The network location of external software used by plugins
        /// </summary>
        ExternalSoftware,

        /// <summary>
        /// The network location of the Asset Inventory database.
        /// </summary>
        AssetInventoryDatabase,

        /// <summary>
        /// VM state transition timout in seconds
        /// </summary>
        VmStateTransitionTimeoutInSeconds,

        /// <summary>
        /// The VM shutdown wait timeout in seconds
        /// </summary>
        VmShutdownWaitTimeoutInSeconds,

        /// <summary>
        /// VM bootup batch size
        /// </summary>
        VmBootUpBatchSize,

        /// <summary>
        /// VMWare server Uri
        /// </summary>
        VMWareServerUri,

        /// <summary>
        /// Office Worker password
        /// </summary>
        OfficeWorkerPassword,

        /// <summary>
        /// Report template repository
        /// </summary>
        ReportTemplateRepository,

        /// <summary>
        /// Document Library server
        /// </summary>
        DocumentLibraryServer,

        /// <summary>
        /// Document Library username
        /// </summary>
        DocumentLibraryUserName,

        /// <summary>
        /// Document Library password
        /// </summary>
        DocumentLibraryPassword,

        /// <summary>
        /// Document Library website
        /// </summary>
        DocumentLibraryWebSite,

        /// <summary>
        /// Fully-qualified DNS domain name
        /// </summary>
        DnsDomain,

        /// <summary>
        /// Domain for administrator account
        /// </summary>
        Domain,

        /// <summary>
        /// Domain admin username
        /// </summary>
        DomainAdminUserName,

        /// <summary>
        /// Domain admin password
        /// </summary>
        DomainAdminPassword,

        /// <summary>
        /// The name of this Environment (PROD, BETA, DEV, etc...)
        /// </summary>
        Environment,

        /// <summary>
        /// The organization this instance belongs to
        /// </summary>
        Organization,

        /// <summary>
        /// Print Driver Server.
        /// </summary>
        PrintDriverServer,

        /// <summary>
        /// Location of where UPD drivers can be found.
        /// </summary>
        UniversalPrintDriverBaseLocation,

        /// <summary>
        /// Asset inventory device pools (comma-delimited list)
        /// </summary>
        AssetInventoryPools,

        /// <summary>
        /// Name of the asset inventory service to connect to, e.g. AssetInventoryServiceDev
        /// </summary>
        AssetInventoryServiceVersion,

        /// <summary>
        /// Defines the location where CFM files are kept
        /// </summary>
        PrintDriverConfigFileLocation,

        /// <summary>
        /// Defines all the browsers supported for web automation in this installation
        /// </summary>
        WebAutomationBrowsers,

        /// <summary>
        /// Defines the name of the server hosting the Citrix Farm.
        /// </summary>
        CitrixFarm,

        /// <summary>
        /// Defines what the maximum displayed log selection time is displayed
        /// </summary>
        ///[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
        MaxLogDefault,

        /// <summary>
        /// Defines whether scenarios can be created without a parent
        /// </summary>
        AllowRootScenarioCreation,

        /// <summary>
        /// Defines the default value to be displayed for data log retention
        /// </summary>
        DefaultLogRetention,

        /// <summary>
        /// Defines the location to which the dispatcher will copy session logs on shutdown
        /// </summary>
        DispatcherLogCopyLocation,

        /// <summary>
        /// Defines the path relative to the executing assembly for all plugin assemblies
        /// </summary>
        PluginRelativeLocation,

        /// <summary>
        /// Defines the file name pattern used for all plugin assemblies
        /// </summary>
        PluginFileNamePattern,

        /// <summary>
        /// The network location of the trace logging database
        /// </summary>
        TraceLogDatabase,

        /// <summary>
        /// The network address to publish to the logging service
        /// </summary>
        LoggingServicePublishAddress,

        /// <summary>
        /// The network address to subscribe to the logging service
        /// </summary>
        LoggingServiceSubscribeAddress,

        /// <summary>
        /// The location of printing certificates for installation
        /// </summary>
        PrintingCertsInstaller,

        /// <summary>
        /// The back channel subnet address for infrastructure and support communication
        /// </summary>
        BackChannelSubNetAddress,

        /// <summary>
        /// The front channel subnet address for communication typical of the user environment being simulated
        /// </summary>
        FrontChannelSubNetAddress,

        /// <summary>
        /// Defines how the STF will run, either standalone without virtualization (STFLite) or distributed, which will
        /// use virtualization to scale tests.
        /// </summary>
        FrameworkDeployment,

        /// <summary>
        /// Intended for runtime use by worker console to communicate executing worker to plugins.
        /// </summary>
        RuntimeWorker,

        /// <summary>
        /// Sets how often the dispatcher checks the health of the client processes.
        /// </summary>
        HealthCheckInterval,

        /// <summary>
        /// The database server used for reports.
        /// </summary>
        ReportingDatabaseServer,

        /// <summary>
        /// The name of the database used for reporting.
        /// </summary>
        ReportingDatabase,

        /// <summary>
        /// The default domain account pool
        /// </summary>
        DomainAccountPoolDefault,

        /// <summary>
        /// The proxy address for internet access
        /// </summary>
        GlobalProxy,

        /// <summary>
        /// The number of days past the expiration date to delay before deleting the session records.
        /// </summary>
        SessionRemovalDelay_Days,

        /// <summary>
        /// The values that can be selected in the Scenario Tags field.
        /// </summary>
        ScenarioTags,

        /// <summary>
        /// The admin password for devices in this environment
        /// </summary>
        DeviceAdminPassword,

        /// <summary>
        /// The Web UI Uri and port of the common server where the service is installed
        /// </summary>
        WebUiUri
    }

    /// <summary>
    /// Segregates different System Settings used for different purposes within the STF.
    /// </summary>
    public enum SettingType
    {
        /// <summary>
        /// Plugin Setting
        /// </summary>
        [Description("Plugin Setting")]
        PluginSetting,

        /// <summary>
        /// Framework Server Setting
        /// </summary>
        [Description("Server Setting")]
        ServerSetting,

        /// <summary>
        /// SystemSetting
        /// </summary>
        [Description("System Setting")]
        SystemSetting
    }
}
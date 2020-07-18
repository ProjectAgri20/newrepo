
namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Describes different types of servers used in the STF environment.  This doesn't include all types but
    /// only those that are referenced in code.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// Citrix server
        /// </summary>
        Citrix,

        /// <summary>
        /// STF Dispatcher
        /// </summary>
        Dispatcher,

        /// <summary>
        /// ePrint server
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "e")]
        ePrint,

        /// <summary>
        /// Equitrac server
        /// </summary>
        Equitrac,

        /// <summary>
        /// Target server to run EventLog collecting
        /// </summary>
        EventLog,

        /// <summary>
        /// Exchange Server
        /// </summary>
        Exchange,

        /// <summary>
        /// File server
        /// </summary>
        FileServer,

        /// <summary>
        /// HPAC server
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HPAC")]
        HPAC,

        /// <summary>
        /// HPCR server (Capture and Route)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HPCR")]
        HPCR,

        /// <summary>
        /// HPEC server
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HPEC")]
        HPEC,

        /// <summary>
        /// mPrint server
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "m")]
        mPrint,

        /// <summary>
        /// Target server to run PerfMon collecting
        /// </summary>
        PerfMon,

        /// <summary>
        /// Pharos server
        /// </summary>
        Pharos,

        /// <summary>
        /// Print server
        /// </summary>
        Print,

        /// <summary>
        /// SafeCom server
        /// </summary>
        SafeCom,

        /// <summary>
        /// Solution Server
        /// </summary>
        Solution,

        /// <summary>
        /// Host that runs virtual printer processes
        /// </summary>
        VPrint,

        /// <summary>
        /// Terminal server
        /// </summary>
        TerminalServer,
		
		/// <summary>
        /// Database server
        /// </summary>
        STFDatabase,

        /// <summary>
        /// Dart Collector Service Server
        /// </summary>
        Dart
    }
}

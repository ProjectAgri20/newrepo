using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DriverlessPrinting
{
    /// <summary>
    /// Class containing the metadata items to be serialized for the plug-in.
    /// </summary>
    /// <remarks>
    /// The STF framework will use <see cref="NetDataContractSerializer"/>, which stores CLR type
    /// information in the serialized XML. This has the advantage of handling polymorphic types,
    /// but requires it to be more stringent regarding type names, namespaces, and assemblies of
    /// the types being serialized. Changes to the plug-in assembly name, plug-in namespace, or
    /// metadata class name will break existing activity data. Some changes to the members of the
    /// plug-in class may also break existing activity data. Ensure the assembly name, namespace,
    /// and plug-in metadata class are as desired before using the plug-in.
    /// 
    /// •  The metadata class must be decorated with a <c>DataContract</c> attribute.
    /// •  Any data that should be persisted during serialization must be decorated with a
    ///    <c>DataMember</c> attribute. This attribute can be applied to fields or properties; any
    ///    properties with this attribute must have a getter and a setter, though these do not have
    ///    to be public.
    /// •  The new metadata class should not include information about selected assets or
    ///    documents. This data is stored elsewhere and will be automatically persisted by the
    ///    framework.
    /// •  The new metadata class should minimize references to types outside of the plug-in,
    ///    other than .NET native types.
    /// 
    /// <seealso cref="System.Runtime.Serialization.NetDataContractSerializer"/>
    /// </remarks>
    [DataContract]
    public class DriverlessPrintingActivityData
    {
        [DataMember]
        public PrintMethod PrintMethod { get; set; }

        /// <summary>
        /// Gets or sets whether the list of documents to print is shuffled prior to each run.
        /// </summary>
        [DataMember]
        public bool ShuffleDocuments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to print a separator between jobs.
        /// </summary>
        /// <value>The print job separator.</value>
        [DataMember]
        public bool PrintJobSeparator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the job is pin protected and destined for Job Storage or print
        /// </summary>
        [DataMember]
        public bool PinProtected { get; set; }

        /// <summary>
        /// Pin to be used for protection (Optional)
        /// </summary>
        [DataMember]
        public int Pin { get; set; }

        public DriverlessPrintingActivityData()
        {
            Pin = 1000;
            PinProtected = false;
        }

    }

    /// <summary>
    /// Print Method to be used
    /// </summary>
    public enum PrintMethod
    {
        /// <summary>
        /// 9100 Printing
        /// </summary>
        Port9100,
        /// <summary>
        /// Print with FTP
        /// </summary>
        Ftp,
        /// <summary>
        /// Print using IPP protocol (eprint)
        /// </summary>
        Ipp,
        /// <summary>
        /// Print using EWS
        /// </summary>
        Ews,
        /// <summary>
        /// Print using a random method
        /// </summary>
        Random
    }
}

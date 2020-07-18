using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.ePrintAdmin
{
    [DataContract]
    public class ePrintAdminActivityData
    {
        /// Eprint admin user name

        [DataMember]
        public string ePrintAdminUser { get; set; }

        /// Eprint admin user password

        [DataMember]
        public string ePrintAdminPassword { get; set; }

        /// Eprint admin task

        [DataMember]
        public Collection<EprintAdminTask> ePrintAdminTasks { get; set; }

        /// Initializes a new instance of the <see cref="ePrintAdminActivityData"/> class.
        public ePrintAdminActivityData()
        {
            ePrintAdminUser = string.Empty;

            ePrintAdminTasks = new Collection<EprintAdminTask>();
        }
    }

    [DataContract]
    public class EprintAdminTask
    {
        /// The type of operation to be done. Eg: Add,Delete
        [DataMember]
        public EprintAdminToolOperation Operation { get; set; }

        /// Optional argument to the operation, eg: filename
        [DataMember]
        public string TargetObject { get; set; }

        /// used to store value needed to add hpac printer
        [DataMember]
        public HpacInputValues HpacInputValue { get; set; }

        /// The description of the task used to display in editor
        [DataMember]
        public string Description { get; set; }

        /// The status of the task upon execution, used in execution controller only
        [IgnoreDataMember]
        public string Status { get; set; }
        /// dummy constructor

        public EprintAdminTask()
        {
            TargetObject = string.Empty;
            Description = string.Empty;
            Status = string.Empty;
        }
    }

    /// <summary>
    /// admin tool operations
    /// </summary>
    public enum EprintAdminToolOperation
    {
        AddPrinteripv4,
        AddPrinterHpac,
        DeletePrinter,
        ImportPrinter,
        RegularUser,
        GuestUser,
        SendPrintJob,
        AddPrinterPJL
    }

    /// <summary>
    /// used to store value needed to add hpac printer
    /// </summary>
    [DataContract]
    public class HpacInputValues
    {
        /// <summary>
        /// Specifies the printer Name to be added
        /// </summary>
        [DataMember]
        public string PrinterName { get; set; }

        /// <summary>
        /// Network address of the HPAC server
        /// </summary>
        [DataMember]
        public string NetworkAddress { get; set; }

        /// <summary>
        /// Print queue name
        /// </summary>
        [DataMember]
        public string QueueName { get; set; }

        /// <summary>
        /// HPAC domain user
        /// </summary>
        [DataMember]
        public string DomainUser { get; set; }

        /// <summary>
        /// HPAC domain user password
        /// </summary>
        [DataMember]
        public string DomainPassword { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public HpacInputValues()
        {
            DomainUser = string.Empty;
            DomainPassword = string.Empty;
            QueueName = string.Empty;
            PrinterName = string.Empty;
            NetworkAddress = string.Empty;
        }


    }
}
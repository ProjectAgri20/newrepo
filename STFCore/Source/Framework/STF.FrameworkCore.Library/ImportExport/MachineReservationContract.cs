using System.Runtime.Serialization;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (used for import/export) for MachineReservation.
    /// </summary>
    [DataContract(Name = "MachineReservation", Namespace = "")]
    [ObjectFactory(VirtualResourceType.MachineReservation)]
    public class MachineReservationContract : ResourceContract
    {
    }
}

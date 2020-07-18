
using HP.ScalableTest.Framework;
namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    
    public partial class MachineReservation : VirtualResource
    {
        public MachineReservation()
            : this("MachineReservation")
        {
        }

        public MachineReservation(string resourceType)
            : base(resourceType)
        {
        }
    }
}

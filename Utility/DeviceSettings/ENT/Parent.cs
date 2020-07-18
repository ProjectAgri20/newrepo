
namespace HP.RDL.STF.DeviceSettings
{
    /// <summary>
    /// Used to hold the parent name as well as the paired values that are retrieved from 
    /// the device. Parent may contain a parent but have yet to implement.
    /// </summary>
    public class Parent
    {
        public string ParentName { get; set; }
        public Parents ListParents { get; set; }
        public PairedValues ListPairedValues { get; set; }
        public Parent()
        {
            ListParents = new Parents();
            ListPairedValues = new PairedValues();
            ParentName = string.Empty;
        }
        public override string ToString()
        {
            return this.ParentName;
        }
    }
}

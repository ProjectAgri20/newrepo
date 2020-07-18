
namespace HP.RDL.STF.DeviceSettings
{
    /// <summary>
    /// Used to store data in the grid and write/read to and from file.
    /// </summary>
    public class DataFim
    {
        public string EndPoint { get; set; }
        public string Parent { get; set; }
        public string Element { get; set; }
        public string ValueOrig { get; set; }
        public string ValueNew { get; set; }

        public bool SameValue { get; set; }

        public DataFim()
        {
            EndPoint = string.Empty;
            Parent = string.Empty;
            Element = string.Empty;
            ValueOrig = string.Empty;
            ValueNew = string.Empty;
            SameValue = true;
        }
    }
}


namespace HP.RDL.STF.DeviceSettings
{
    /// <summary>
    /// Used to collect values from the WS* retrieval
    /// </summary>
    public class EndPoint
    {
        public string EndPointName { get; set; }
        public string ResourceURI { get; set; }

        public Parents ListParents { get; set; }
        public bool HasValues { get; set; }

        public EndPoint()
        {
            EndPointName = string.Empty;
            ResourceURI = string.Empty;
            HasValues = false;

            ListParents = new Parents();
        }
        public override string ToString()
        {
            return EndPointName;
        }
    }
}

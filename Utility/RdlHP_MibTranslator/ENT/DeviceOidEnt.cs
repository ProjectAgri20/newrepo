
namespace HP.RDL.RdlHPMibTranslator
{
	public class DeviceOidEnt
	{
		public string OidString { get; set; }
		public string OidValue { get; set; }

		public DeviceOidEnt()
		{
			OidString = string.Empty;
			OidValue = string.Empty;
		}
	}
}

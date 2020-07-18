using System.Text;

namespace HP.ScalableTest.Plugin.TelnetSnmp
{
    internal class XMLUtilities
    {
        public static string CreateConfigurationData(TelnetSnmpActivityData activityData)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Configurations>");
            builder.Append("<Configuration id=\"Mamba 1.1\">");
            builder.Append("<Browser>Firefox</Browser>");
            builder.Append("<Version>Version 1.1</Version>");
            builder.Append("<Product>" + activityData.ProductName + "</Product>");
            builder.Append("<DeviceAddress>" + activityData.PrinterIP + "</DeviceAddress>");
            builder.Append("<ProductType>" + activityData.ProductCategory + "</ProductType>");
            builder.Append("<HttpRemoteControlHost>localhost</HttpRemoteControlHost>");
            builder.Append("<HttpRemoteControlPort>4444</HttpRemoteControlPort>");
            builder.Append("</Configuration>");
            builder.Append("</Configurations>");

            return (builder.ToString());
        }
    }
}

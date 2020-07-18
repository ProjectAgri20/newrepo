using System.Linq;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.TopCat;

namespace HP.ScalableTest.Plugin.TopCat
{
    internal sealed class TopCatActivityDataConverter : IPluginMetadataConverter
    {
        public string OldVersion => "1.0";
        public string NewVersion => "1.1";

        public XElement Convert(XElement xml)
        {
            XNamespace nsRoot = xml.GetDefaultNamespace();
            XElement scriptElement = xml.Element(nsRoot + "Script");
            var scriptNamespaceAttribute = scriptElement.Attributes().Where(n => n.IsNamespaceDeclaration).First();
            XNamespace nsScript = XNamespace.Get(scriptNamespaceAttribute.Value);

            TopCatScript topCatScript = new TopCatScript
            (
                (string)scriptElement.Element(nsScript + "FileName"),
                (string)scriptElement.Element(nsScript + "ScriptName")
            );

            XElement selectedTestElement = scriptElement.Element(nsScript + "SelectedTests");
            var tests = selectedTestElement.Descendants().Where(n => n.Name.LocalName == "string").Select(n => (string)n);
            foreach (string test in tests.Where(n => !string.IsNullOrEmpty(n)))
            {
                topCatScript.SelectedTests.Add(test);
            }

            var propertyNodes = scriptElement.Descendants(nsScript + "TopCatProperties");
            foreach (var propertyNode in propertyNodes.Where(n => n.FirstNode != null))
            {
                string propertyName = (string)propertyNode.Element(nsScript + "PropertyName");
                string propertyValue = (string)propertyNode.Element(nsScript + "PropertyValue");
                topCatScript.Properties.Add(propertyName, propertyValue);
            }

            TopCatActivityData activityData = new TopCatActivityData()
            {
                CopyDirectory = (bool?)xml.Element(nsRoot + "CopyDirectory") ?? false,
                RunOnce = (bool?)xml.Element(nsRoot + "RunOnce") ?? false,
                SetupFileName = (string)xml.Element(nsRoot + "SetupFileName"),
                Script = topCatScript
            };

            return Serializer.Serialize(activityData);
        }
    }
}

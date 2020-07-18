using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace HP.ScalableTest.Xml
{
    /// <summary>
    /// Collection of static methods to manage XML data.
    /// </summary>
    public static class XmlUtil
    {
        /// <summary>
        /// Compares two xml strings to determine if they are semantically equivalent.
        /// </summary>
        /// <param name="first">The first XML string.</param>
        /// <param name="second">The second XML string.</param>
        /// <param name="orderInvariant">if set to <c>true</c> ignore node order.</param>
        /// <returns>If set to true they are equivalent, otherwise false</returns>
        public static bool AreEqual(string first, string second, bool orderInvariant = true)
        {
            try
            {
                XDocument firstDoc = XDocument.Parse(first);
                XDocument secondDoc = XDocument.Parse(second);

                return AreEqual(firstDoc, secondDoc, orderInvariant);
            }
            catch (XmlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Compares two <see cref="XDocument"/> objects to determine if they are semantically equivalent.
        /// </summary>
        /// <param name="first">The first <see cref="XDocument"/> object.</param>
        /// <param name="second">The second <see cref="XDocument"/> object.</param>
        /// <param name="orderInvariant">if set to <c>true</c> ignore node order.</param>
        /// <returns>If set to true they are equivalent, otherwise false</returns>
        public static bool AreEqual(XDocument first, XDocument second, bool orderInvariant)
        {
            if (orderInvariant)
            {
                first.Root.Sort();
                second.Root.Sort();
            }

            return XNode.DeepEquals(first, second);
        }

        /// <summary>
        /// Sorts all nodes in the specified <see cref="XElement"/> recursively.
        /// </summary>
        /// <param name="element">The XML element to be sorted.</param>
        /// <example>
        /// This example shows how the sort method works.
        /// <code>
        /// string xml = "&lt;List>&lt;A>3&lt;/A>&lt;A>2&lt;/A>&lt;A>1&lt;/A>&lt;/List>";
        /// XElement x = XElement.Parse(xml);
        /// Console.WriteLine(x.Value);
        /// x.Sort();
        /// Console.WriteLine(x.Value);
        /// 
        /// OUTPUT:
        /// 321
        /// 123
        /// </code>
        /// </example>
        public static void Sort(this XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (element.Elements().Any())
            {
                // Sort the element by name and then value - this ensures that <string> tags get sorted correctly
                List<XElement> sortedChildren = element.Elements().OrderBy(n => (n.Name.ToString() + n.Value)).ToList();
                element.RemoveNodes();
                sortedChildren.ForEach(n => element.Add(n));
                sortedChildren.ForEach(n => n.Sort());
            }
            if (element.Attributes().Any())
            {
                // Sort the attributes by name
                List<XAttribute> sortedAttributes = element.Attributes().OrderBy(n => n.Name.ToString()).ToList();
                element.RemoveAttributes();
                sortedAttributes.ForEach(n => element.Add(n));
            }
        }

        /// <summary>
        /// Converts an <see cref="IXPathNavigable"/> object to a corresponding <see cref="XDocument"/> object.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <returns>An XDocument class from the provided XML data</returns>
        public static XDocument CreateXDocument(IXPathNavigable xmlDocument)
        {
            using (XmlReader reader = xmlDocument.CreateNavigator().ReadSubtree())
            {
                reader.MoveToContent();
                return XDocument.Load(reader);
            }
        }

        /// <summary>
        /// Converts an <see cref="IXPathNavigable"/> object to a corresponding <see cref="XDocument"/> object.
        /// </summary>
        /// <param name="xmlString">The XML as string.</param>
        /// <returns></returns>
        public static XDocument CreateXDocument(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            return CreateXDocument(xmlDoc);
        }

        /// <summary>
        /// Creates a new XML node based on the provided XML data.
        /// </summary>
        /// <param name="xmlDoc">The XML doc.</param>
        /// <param name="xmlData">The XML string data.</param>
        /// <returns>An <see cref="IXPathNavigable"/> node created from the provide XML data</returns>
        /// <exception cref="System.ArgumentNullException">document</exception>
        public static IXPathNavigable CreateNode(IXPathNavigable xmlDoc, string xmlData)
        {
            if (xmlDoc == null)
            {
                throw new ArgumentNullException("xmlDoc");
            }

            XmlNode node = null;

            using (StringReader reader = new StringReader(xmlData))
            {
                XmlReader xmlReader = XmlReader.Create(reader);
                node = ((XmlDocument)xmlDoc).ReadNode(xmlReader);
            }

            return node;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Base class for <see cref="IPluginMetadataConverter" /> implementations that assists with XML parsing and modification.
    /// </summary>
    public abstract class PluginDataConverter
    {
        /// <summary>
        /// Gets the namespace of the specified <see cref="XElement" />.
        /// </summary>
        /// <param name="element">The <see cref="XElement" />.</param>
        /// <returns>The <see cref="XNamespace" /> for the specified element.</returns>
        protected static XNamespace GetNamespace(XElement element)
        {
            if (element == null)
            {
                return null;
            }

            XAttribute attr = element.Attributes().Where(n => n.IsNamespaceDeclaration).First();
            return XNamespace.Get(attr.Value);
        }

        /// <summary>
        /// Gets the first attribute with the specified XName.LocalName.
        /// </summary>
        /// <param name="element">The <see cref="XElement" />.</param>
        /// <param name="attributeName">The attribute name.</param>
        /// <returns>The <see cref="XAttribute" /> with the specified name.</returns>
        protected static XAttribute GetAttribute(XElement element, string attributeName)
        {
            IEnumerable<XAttribute> attributes = element?.Attributes();
            XAttribute attribute = attributes.Where(a => a.Name.LocalName.Equals(attributeName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return attribute;
        }

        /// <summary>
        /// Returns whether a descendant of the specified <see cref="XElement" /> exists.
        /// </summary>
        /// <param name="element">The <see cref="XElement" />.</param>
        /// <param name="elementName">The name of the descendent node.</param>
        /// <param name="immediateOnly">Whether or not to search only the element's immediate descendants.</param>
        /// <returns><c>true</c> if the node exists or <c>false</c> otherwise.</returns>
        protected static bool Exists(XContainer element, string elementName, bool immediateOnly)
        {
            if (immediateOnly)
            {
                return element?.Elements().Any(d => d.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase)) ?? false;
            }

            return element?.Descendants().Any(d => d.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase)) ?? false;
        }

        /// <summary>
        /// Gets a <see cref="bool" /> value from a descendant of the specified <see cref="XElement" /> with the specified name.
        /// </summary>
        /// <param name="element">The <see cref="XElement" />.</param>
        /// <param name="elementName">The name of the descendent node with the <see cref="bool" /> property.</param>
        /// <returns>A <see cref="bool" /> value for the specified descendent node, or <c>false</c> if no such node was found.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bool")]
        protected static bool GetBool(XContainer element, string elementName)
        {
            try
            {
                XElement valueElement = element?.Descendants().Where(d => d.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                return bool.Parse(valueElement.Value);
            }
            catch (ArgumentNullException)
            {
            }
            catch (NullReferenceException)
            {
            }
            return false;
        }

        /// <summary>
        /// Gets an <see cref="int" /> value from a descendant of the specified <see cref="XElement" /> with the specified name.
        /// </summary>
        /// <param name="element">The <see cref="XElement" />.</param>
        /// <param name="elementName">The name of the descendent node with the <see cref="int" /> property.</param>
        /// <returns>An <see cref="int" /> value for the specified descendent node, or 0 if no such node was found.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        protected static int GetInt(XContainer element, string elementName)
        {
            try
            {
                XElement valueElement = element?.Descendants().Where(d => d.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (valueElement != null)
                {
                    return int.Parse(valueElement.Value);
                }
            }
            catch (ArgumentNullException)
            {
            }
            catch (NullReferenceException)
            {
            }
            return 0;
        }

        /// <summary>
        /// Gets a <see cref="string" /> value from a descendant of the specified <see cref="XElement" /> with the specified name.
        /// </summary>
        /// <param name="element">The <see cref="XElement" />.</param>
        /// <param name="elementName">The name of the descendent node with the <see cref="string" /> property.</param>
        /// <returns>A <see cref="string" /> value for the specified descendent node, or an empty string if no such node was found.</returns>
        protected static string GetValue(XContainer element, string elementName)
        {
            try
            {
                XElement valueElement = element?.Descendants().Where(d => d.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (valueElement != null)
                {
                    return valueElement.Value;
                }
            }
            catch (ArgumentNullException)
            {
            }
            catch (NullReferenceException)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan" /> value from a descendant of the specified <see cref="XElement" /> with the specified name.
        /// </summary>
        /// <param name="element">The <see cref="XElement" />.</param>
        /// <param name="elementName">The name of the descendent node with the <see cref="TimeSpan" /> property.</param>
        /// <returns>A <see cref="TimeSpan" /> value for the specified descendent node, or <see cref="TimeSpan.MinValue" /> if no such node was found.</returns>
        protected static TimeSpan GetTimeSpan(XContainer element, string elementName)
        {
            string xmlValue = GetValue(element, elementName);

            if (!string.IsNullOrEmpty(xmlValue))
            {
                return XmlConvert.ToTimeSpan(xmlValue);
            }
            return TimeSpan.Zero;
        }

        /// <summary>
        /// Gets a <see cref="LockTimeoutData" /> value from a descendant of the specified <see cref="XElement" /> with the specified name.
        /// </summary>
        /// <param name="rootNS">The root <see cref="XNamespace" />.</param>
        /// <param name="xml">The <see cref="XElement" />.</param>
        /// <returns>A <see cref="LockTimeoutData" /> value for the specified descendent node.</returns>
        protected static LockTimeoutData GetLockTimeoutData(XNamespace rootNS, XContainer xml)
        {
            if (xml == null)
            {
                //No data provided.  Return default.
                return new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            }

            XElement locTimElement = xml.Element(rootNS + "LockTimeouts");

            if (locTimElement == null)
            {
                //LockTimeout data not found.  Return default.
                return new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            }

            return new LockTimeoutData(GetTimeSpan(locTimElement, "_acquireTimeout"), GetTimeSpan(locTimElement, "_holdTimeout"));
        }
    }
}

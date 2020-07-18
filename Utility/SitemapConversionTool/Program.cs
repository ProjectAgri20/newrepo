using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SitemapConversionTool
{
    class Program
    {
        static string _family = string.Empty;
        static string _product = string.Empty;
        static string _version = string.Empty;
        static string _oldSiteMapsPath = string.Empty;
        static string _newSiteMapsPath = string.Empty;

        static void Main(string[] args)
        {
            // Convert existing sitemaps to new format

            _oldSiteMapsPath = args[0];
            _newSiteMapsPath = args[1];

            // walk thru each product family
            foreach (string family in Directory.GetDirectories(Path.Combine(_oldSiteMapsPath, "URL")))
            {
                _family = family.Substring(family.LastIndexOf('\\') + 1);

                // walk thru each product
                foreach (string product in Directory.GetDirectories(family))
                {
                    _product = product.Substring(product.LastIndexOf('\\') + 1);

                    // walk thru each version
                    foreach (string version in Directory.GetDirectories(product))
                    {
                        _version = version.Substring(version.LastIndexOf('\\') + 1);
                        // Load the URL xml file
                        foreach (string file in Directory.GetFiles(version, "*.xml", SearchOption.TopDirectoryOnly))
                        {
                            ConvertSitemap(file);
                        }
                    }
                }
            }
            
        }

        static void ConvertSitemap( string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlDocument newDoc = new XmlDocument();
            XmlElement root = newDoc.CreateElement("Sitemaps");
            newDoc.AppendChild(root);

            XmlElement pages = newDoc.CreateElement("Pages");
            root.AppendChild(pages);

            foreach (XmlNode url in doc.DocumentElement.SelectNodes("//URL"))
            {
                AddPage(newDoc, pages, url);
            }

            // save the new site maps into the new location
            Directory.CreateDirectory(_newSiteMapsPath + "\\" + _family + "\\" + _product + "\\" + _version);
            newDoc.Save(_newSiteMapsPath + "\\" + _family + "\\" + _product + "\\" + _version + "\\" + "SiteMaps.xml");
        }

        static void AddPage(XmlDocument newDoc, XmlElement pages, XmlNode url)
        {
            XmlElement page = newDoc.CreateElement("Page");
            pages.AppendChild(page);

            AddAttribute(newDoc, page, "key", url.Attributes["key"].Value);
            AddAttribute(newDoc, page, "relative_path", url.Attributes["value"].Value);

             string objFilePath = _oldSiteMapsPath +  "\\OID\\" + _family + "\\" + _product + "\\" + _version + "\\" + url.Attributes["IDFileName"].Value;

             AddElements(newDoc, page, objFilePath);
        }

        static void AddElements(XmlDocument newDoc, XmlElement page, string objectIDsFileName)
        {
            if (File.Exists(objectIDsFileName))
            {
                XmlDocument objectID = new XmlDocument();
                objectID.Load(objectIDsFileName);

                XmlElement elements = newDoc.CreateElement("Elements");
                page.AppendChild(elements);

                foreach (XmlNode obj in objectID.DocumentElement.SelectNodes("//ObjectID"))
                {
                    XmlElement element = newDoc.CreateElement("Element");

                    AddAttribute(newDoc, element, "key", obj.Attributes["key"].Value);
                    AddAttribute(newDoc, element, "id", obj.Attributes["value"].Value);
                    AddAttribute(newDoc, element, "name", "");
                    AddAttribute(newDoc, element, "type", "");

                    elements.AppendChild(element);
                }
            }
        }

        static void AddAttribute(XmlDocument newDoc, XmlElement ele, string name, string value)
        {
            XmlAttribute attr = newDoc.CreateAttribute(name);

            attr.Value = value;

            //ele.AppendChild(attr);
            ele.Attributes.Append(attr);
        }        
    }
}

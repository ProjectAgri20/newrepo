using System.Xml;

namespace HP.RDL.STF.DeviceSettings
{
    /// <summary>
    /// Used to read and process the XML files located in the input directory of this project.
    /// NOTE: All Element IdentNames must be listed before a Element Parent. 
    /// </summary>
    public class ProcessXML
    {
        private enum XML_TYPES
        {
            DeviceSettings = 0,
            EndPoint,
            ResourceUri,
            Parent,
            IdentName,
            EndXml,
            Unknown
        };
        public DeviceSettings ListDeviceEndPoints = new DeviceSettings();
        public static string ErrorMessage { get; private set; }

        private XML_TYPES _xmlSetting = XML_TYPES.Unknown;
        public ProcessXML()
        {
            ErrorMessage = string.Empty;
        }
        public static bool IsError
        {
            get
            {
                return (string.IsNullOrEmpty(ErrorMessage) == false) ? true : false;
            }
        }
        public void GetFirmwareInfo()
        {
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(ResXML.FirmwareInfo));
            ReadXML(reader);
        }
        public void GetFIM_TestingInfo()
        {
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(ResXML.FIM_Testing));
            ReadXML(reader);
        }
        public void GetTestData()
        {
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(ResXML.TestData));
            ReadXML(reader);
        }
        public void GetDeviceInformation()
        {
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(ResXML.DeviceInformation));
            ReadXML(reader);
        }
        /// <summary>
        /// Reads the XML into the Device Setting List. The Parent node would probably
        /// be a good candidate for recursion but currently have it solved with some well
        /// placed Booleans. 
        /// </summary>
        /// <param name="reader">XmlTextReader</param>
        public void ReadXML(XmlTextReader reader )
        {            
            EndPoint endPoint = null;

            while(reader.Read())
            {

                string nodeName = reader.Name;
                switch(reader.NodeType)
                {
                     
                    case XmlNodeType.Element: // The node is an element.
                        
                        switch(nodeName)
                        {
                            case "DeviceSettings": _xmlSetting = XML_TYPES.DeviceSettings;
                                break;
                            case "EndPoint": _xmlSetting = XML_TYPES.EndPoint;
                                endPoint = new EndPoint();
                                break;
                            case "ResourceURI": _xmlSetting = XML_TYPES.ResourceUri;
                                break;
                            case "Parent": _xmlSetting = XML_TYPES.Parent;
                                ProcessParent(reader, endPoint.ListParents);
                                break;
                            default: _xmlSetting = XML_TYPES.Unknown;
                                break;
                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.

                        string xValue = reader.Value.Trim();

                        switch(_xmlSetting)
                        {
                            case XML_TYPES.EndPoint: endPoint.EndPointName = xValue;
                                break;
                            case XML_TYPES.ResourceUri: endPoint.ResourceURI = xValue;
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.

                        switch (nodeName)
                        {
                            case "EndPoint": ListDeviceEndPoints.Add(endPoint);
                                endPoint = null;
                                break;
                        }
                        break;
                }

            }
        }
        /// <summary>
        /// Processes the parent nodes. Since a parent may contain a parent, there may be 
        /// a recursive call to self.
        /// </summary>
        /// <param name="reader">XmlTextReader</param>
        /// <param name="listParents">Parents</param>
        private void ProcessParent(XmlTextReader reader, Parents listParents)
        {
            string nodeName = string.Empty;
            bool bParent = true;
            Parent p = new Parent();

            while(bParent && reader.Read())
            {
                nodeName = reader.Name;
                switch(reader.NodeType)
                {
                    case XmlNodeType.Text:
                        p.ParentName = reader.Value.Trim();
                        break;
                    case XmlNodeType.Element:
                        if(nodeName.Equals("IdentName"))
                        {
                            nodeName = ProcessIdentityNode(reader, p.ListPairedValues);
                        }
                        // Parent containing parent, may also occur after processing the identities
                        if(nodeName.Equals("Parent"))
                        {
                            ProcessParent(reader, p.ListParents);
                        }
                        else if(nodeName.Equals("EndParent"))
                        {
                            // this may occur after processing the identities. Will either be a new parent (above) or finished the parent
                            bParent = false;
                            listParents.Add(p);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        bParent = false;
                        listParents.Add(p);
                        break;
                }
            }
        }
        /// <summary>
        /// Processes the identity node of the parent
        /// </summary>
        /// <param name="reader">XmlTextReader</param>
        /// <param name="pvList">PairedValues</param>
        /// <returns>string</returns>
        private string ProcessIdentityNode(XmlTextReader reader, PairedValues pvList)
        {
           bool bElement = true;
           string nodeName = string.Empty;

           while (bElement && reader.Read())
           {
               nodeName = reader.Name;
               if (reader.NodeType.Equals(XmlNodeType.Text))
               {
                   PairedValue pv = new PairedValue();
                   pv.IdentName = reader.Value.Trim();

                   pvList.Add(pv);
               }
               else if (reader.NodeType.Equals(XmlNodeType.EndElement))
               {
                   if (nodeName.Equals("Parent"))
                   {
                       nodeName = "End" + nodeName;
                       bElement = false;
                   }
               }
               else if (reader.NodeType.Equals(XmlNodeType.Element))
               {
                   if (nodeName.Equals("Parent"))
                   {
                       bElement = false;
                   }
               }
           }
           return nodeName;
        }
    }
}

using System.Text.RegularExpressions;
using System.Xml;


namespace HP.Epr.WebServicesResponder.WSStandard
{
    public partial class Locale
    {
        /// <summary>
        /// Default xs:lang value
        /// </summary>
        public const string Default = "en-US";

        /// <summary>
        /// Name of the xml language attribute
        /// </summary>
        public const string Name = "lang";

        /// <summary>
        /// Prefix for xml:lang attribute
        /// </summary>
        public const string Prefix = "xml";

        /// <summary>
        /// Namespace for the xml: prefix
        /// </summary>
        public const string Namespace = "http://www.w3.org/XML/1998/namespace";

        /// <summary>
        /// Qualified name for the xml:lang attribute
        /// </summary>
        public const string QualifiedName = Prefix + ":" + Name;


        /// <summary>
        /// default constructor - required for serialization
        /// </summary>
        public Locale()
            : this(Locale.Default)
        {
        }

        /// <summary>
        /// Constructor that accepts a ws:language string to initialize AnyAttr
        /// </summary>
        /// <param name="language">a ws:language string</param>
        public Locale(string language)
        {
            XmlDocument doc = new XmlDocument();
            XmlAttribute lang = doc.CreateAttribute(Locale.QualifiedName, Locale.Namespace);
            lang.Value = language;

            XmlAttribute mustUnderstand = doc.CreateAttribute("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope");
            mustUnderstand.Value = "false";
            this.AnyAttr = new XmlAttribute[2] { lang, mustUnderstand };
        }

        /// <summary>
        /// Returns the culture (e.g. en-US) of the Locale.
        /// </summary>
        public string Culture
        {
            get
            {
                if (this.AnyAttr != null)
                {
                    foreach (XmlAttribute attr in this.AnyAttr)
                    {
                        if (attr.Name.Equals(Locale.QualifiedName))
                        {
                            return attr.Value;
                        }
                    }
                }
                return Locale.Default;
            }
        }

        /// <summary>
        /// Returns the value of the MustUnderstand attribute
        /// </summary>
        public bool MustUnderstand
        {
            get
            {
                if (this.AnyAttr != null)
                {
                    foreach (XmlAttribute attr in this.AnyAttr)
                    {
                        if (attr.LocalName.Equals("mustUnderstand") && attr.NamespaceURI.Equals("http://www.w3.org/2003/05/soap-envelope"))
                        {
                            return XmlConvert.ToBoolean(attr.Value);
                        }
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// Validates a language string complies with ws:language - RFC 3066
        /// </summary>
        /// <param name="language">A language to be validate</param>
        /// <returns>Whether it is a valid language</returns>
        public static bool ValidateLanguage(string language)
        {
            // Per R6.3-3, language value should be valid RFC 3066.
            Regex expression = new Regex(@"^[a-zA-Z]{1,8}(-[a-zA-Z0-9]{1,8})*");
            return expression.IsMatch(language);
        }

    }
}

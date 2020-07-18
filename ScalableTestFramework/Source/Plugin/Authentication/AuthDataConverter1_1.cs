using System;
using System.Xml.Linq;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;


namespace HP.ScalableTest.Plugin.Authentication
{
    public class AuthDataConverter1_1 : PluginDataConverter, IPluginMetadataConverter
    {
        public string OldVersion => "1.0";
        public string NewVersion => "1.1";

        /// <summary>
        /// Converts the specified XML metadata to the new version.
        /// </summary>
        /// <param name="xml">The XML metadata.</param>
        /// <returns>System.Xml.Linq.XElement.</returns>
        public XElement Convert(XElement xml)
        {
            AuthenticationData resultData = new AuthenticationData();

            // Get XML root namespace for manual conversion
            XNamespace rootNS = xml.GetDefaultNamespace();

            // If the values are not found in the XML, don't overwrite the default values set by the AuthenticationData constructor.

            string stringValue = GetValue(xml, "InitiationButton");
            if (!string.IsNullOrEmpty(stringValue))
            {
                resultData.InitiationButton = stringValue;
            }

            resultData.AuthProvider = GetAuthProvider(resultData.InitiationButton);

            stringValue = GetUnAuthenticateMethod(rootNS, xml);
            if (!string.IsNullOrEmpty(stringValue))
            {
                resultData.UnAuthenticateMethod = stringValue;
            }

            return Serializer.Serialize(resultData);
        }

        private string GetUnAuthenticateMethod(XNamespace rootNS, XElement xml)
        {
            XElement unAuthElement = xml.Element(rootNS + "UnauthenticationData");
            return GetValue(unAuthElement, "UnauthenticateMethod");
        }

        private AuthenticationProvider GetAuthProvider(string buttonText)
        {
            InitiationMethod im = AuthInitMethod.GetInitiationMethod(buttonText);
            switch (im)
            {
                case InitiationMethod.HPAC:
                    return AuthenticationProvider.HpacIrm;
                case InitiationMethod.Equitrac:
                    return AuthenticationProvider.Equitrac;
                case InitiationMethod.SafeCom:
                    return AuthenticationProvider.SafeCom;
                case InitiationMethod.Badge:
                    return AuthenticationProvider.Card;
                default:
                    return AuthenticationProvider.Auto;
            }
        }

    }
}

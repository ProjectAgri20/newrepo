using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HP.Epr.WebServicesResponder.WSStandard
{
    /// <summary>
    /// Class to be started automatically to ignore certificate hostname checking.
    /// Used when calling the client sink with HTTPS.
    /// NOTE: THIS IS NOT PRODUCTION CODE - THIS IS HERE SIMPLY TO AVOID HAVING
    ///      TO MUCK AROUND WITH CERTIFICATES DURING DEVELOPMENT. DO NOT PUT THIS IN
    ///      PRODUCTION CODE

    /// </summary>
    public static class InvalidHttpsCertificateIgnorer
    {
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }

        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(
           object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}

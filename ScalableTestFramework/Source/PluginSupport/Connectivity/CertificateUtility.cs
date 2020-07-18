using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Certificate Details Structure 
    /// </summary>
    public struct CertificateDetails
    {
        /// <summary>
        /// Gets or sets Certificate Serial Number
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Certificate Issued By 
        /// </summary>
        public string IssuedBy { get; set; }
        /// <summary>
        /// Certificate Issued To
        /// </summary>
        public string IssuedTo { get; set; }
        /// <summary>
        /// Certificate Issuer
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Certificate Start Date
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Certificate Expiry Date
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// The public key length
        /// </summary>
        public int KeyLength { get; set; }

        /// <summary>
        /// The Signature algorithm
        /// </summary>
        public Oid SignatureAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the value indicating if the private key is exportable.
        /// Applicable only for Id certificates.
        /// </summary>
        public bool PrivateKeyExportable { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            CertificateDetails other = (CertificateDetails)obj;
            return (this.IssuedTo == other.IssuedTo && this.Issuer == other.Issuer && this.IssuedBy == other.IssuedBy && this.StartDate == other.StartDate && this.ExpiryDate == other.ExpiryDate);
        }
    }

    /// <summary>
    /// Certificate Utility class provide details about certificate 
    /// </summary>
    public static class CertificateUtility
    {
        /// <summary>
        /// Gets the certificate details
        /// </summary>
        /// <param name="certificatePath">Certificate File Path</param>
        /// <param name="password">Password is set any</param>
        /// <returns>Certificate Details <see cref="CertificateDetails"/></returns>
        public static CertificateDetails GetCertificateDetails(string certificatePath, string password = null)
        {
            X509Certificate2 _certificate = new X509Certificate2(certificatePath, password);
            CertificateDetails details = new CertificateDetails();

            details.SerialNumber = _certificate.SerialNumber;
            details.IssuedBy = _certificate.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains("CN")).FirstOrDefault().Replace("CN=", string.Empty);
            details.IssuedTo = _certificate.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains("CN")).FirstOrDefault().Replace("CN=", string.Empty);
            details.Issuer = _certificate.Issuer;
            details.StartDate = _certificate.NotBefore;
            details.ExpiryDate = _certificate.NotAfter;
            details.SignatureAlgorithm = new Oid("", _certificate.SignatureAlgorithm.FriendlyName);
            details.PrivateKeyExportable = _certificate.HasPrivateKey;
            details.KeyLength = _certificate.PublicKey.Key.KeySize;

            return details;
        }
    }
}

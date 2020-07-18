using System;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.SolutionTester)]
    public partial class SolutionTester
    {
        private static string _key = "Hew3u1Vhiky4cyTgVvvksA4VKRbsfkl0";

        public SolutionTester()
            : this("SolutionTester")
        {
        }

        public SolutionTester(string resourceType)
            : base(resourceType)
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            AccountType = SolutionTesterCredentialType.DefaultDesktop;
            Platform = "LOCAL";
        }

        protected override void LoadChildDetail(ResourceDetailBase detail)
        {
            base.LoadChildDetail(detail);

            SetDefaults();

            SolutionTesterDetail resourceDetail = detail as SolutionTesterDetail;
            if (resourceDetail != null)
            {
                UseCredential = resourceDetail.UseCredential;
                CredentialType = resourceDetail.CredentialType;
                CredentialName = resourceDetail.Username;
                CredentialDomain = resourceDetail.Domain;
                Password = resourceDetail.Password;
            }
        }

        public override void CopyResourceProperties(VirtualResource resource)
        {
            base.CopyResourceProperties(resource);

            SetDefaults();

            SolutionTester tester = resource as SolutionTester;
            if (tester != null)
            {
                UseCredential = tester.UseCredential;
                CredentialType = tester.CredentialType;
                CredentialName = tester.UserName;
                CredentialDomain = tester.Domain;
                Password = tester.Password;
            }
            else
            {
                throw new ArgumentException("Resource must be of type SolutionTester.", "resource");
            }
        }

        public SolutionTesterCredentialType AccountType
        {
            get { return (SolutionTesterCredentialType)Enum.Parse(typeof(SolutionTesterCredentialType), CredentialType); }
            set { CredentialType = value.ToString(); }
        }

        public string UserName
        {
            get { return CredentialName; }
            set { CredentialName = value; }
        }

        public string Domain
        {
            get { return CredentialDomain; }
            set { CredentialDomain = value; }
        }
 
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(CredentialPassword))
                {
                    return string.Empty;
                }
                else
                {
                    return BasicEncryption.Decrypt(CredentialPassword, _key);
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    CredentialPassword = null;
                }
                else
                {
                    CredentialPassword = BasicEncryption.Encrypt(value, _key);
                }
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on the Solution Tester for the <see cref="SystemManifest"/>
    /// </summary>
    internal class SolutionTesterDetailBuilder : OfficeWorkerDetailBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionTesterDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public SolutionTesterDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.SolutionTester)
        {
        }

        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            LoadResources<SolutionTesterDetail>(resources, manifest);
        }

        internal override OfficeWorkerCredential AddCredential(VirtualResource resource, OfficeWorkerDetail detail)
        {
            SolutionTester tester = resource as SolutionTester;

            OfficeWorkerCredential credential = null;
            switch (tester.AccountType)
            {
                case SolutionTesterCredentialType.AccountPool:
                    credential = ManifestAgent.UserAccounts.NextUserCredential(((OfficeWorker)resource).UserPool);
                    credential.ResourceInstanceId = credential.UserName;
                    //credential.ResourceInstanceId = SystemManifestAgent.CreateUniqueId(credential.UserName);
                    break;
                case SolutionTesterCredentialType.DefaultDesktop:
                    credential = new OfficeWorkerCredential();
                    credential.Domain = Environment.UserDomainName;
                    credential.UserName = Environment.UserName;
                    credential.Password = string.Empty;
                    credential.ResourceInstanceId = SystemManifestAgent.CreateUniqueId(Environment.UserName);
                    break;
                case SolutionTesterCredentialType.ManuallyEntered:
                    credential = new OfficeWorkerCredential();
                    credential.Domain = tester.Domain;
                    credential.UserName = tester.UserName;
                    credential.Password = tester.Password;
                    credential.ResourceInstanceId = SystemManifestAgent.CreateUniqueId(tester.UserName);
                    break;
            }

            return credential;
        }

        public override OfficeWorkerDetail CreateDetail(VirtualResource resource)
        {
            SolutionTester tester = resource as SolutionTester;

            SolutionTesterDetail detail = new SolutionTesterDetail();
            CreateBaseWorkerDetail(tester, detail);

            detail.UseCredential = tester.UseCredential;
            detail.CredentialType = tester.CredentialType;
            detail.Username = tester.CredentialName;
            detail.Domain = tester.CredentialDomain;
            detail.Password = tester.Password;

            CreateMetadataDetail(resource, detail);
            return detail;
        }
    }
}
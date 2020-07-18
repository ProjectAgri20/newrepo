using System;
using System.ServiceModel;

namespace HP.ScalableTest.Framework.DigitalSendDirectoryVerifier
{
    [ServiceContract]
    public interface IDigitalSendDirectoryVerifierService
    {
        [OperationContract]
        bool IsValidPath(string path);
    }
}

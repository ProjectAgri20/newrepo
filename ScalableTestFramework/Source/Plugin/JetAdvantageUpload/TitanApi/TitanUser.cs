using System;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    /// <summary>
    /// Class for holding Titan user credential information
    /// </summary>
    public class TitanUser
    {
        public TitanUser(String emailAddress, String password)
        {
            this.EmailAddress = emailAddress;
            this.Password = password;
        }

        public String EmailAddress { get; private set; }

        public String Password { get; private set; }

        public override String ToString()
        {
            return "User: email [" + EmailAddress + "]";
        }
    }
}
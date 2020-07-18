namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class FtpUser : System.Net.NetworkCredential
    {
        public FtpUser(string username, string password, string homeDirectory)
        {
            UserName = username;
            Password = password;
            HomeDirectory = homeDirectory;
        }

        public FtpUser()
        {
            HomeDirectory = "/";
        }

        public string HomeDirectory
        {
            get;
            set;
        }
    }
}

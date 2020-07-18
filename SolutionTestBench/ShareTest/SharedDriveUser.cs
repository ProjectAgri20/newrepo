using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace ShareTest
{
    public class SharedDriveUser
    {
        public string Name { get; private set; }

        public string Domain { get; private set; }

        public Collection<FileSystemRights> Rights { get; private set; }

        public SharedDriveUser(string username, string domain)
        {
            Name = username;
            Domain = domain;
            Rights = new Collection<FileSystemRights>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    //<jasi:UserInfo>
    //  <security:AuthenticationAgentIdentifier>HPAuthAgent_EmbeddedPrint_v1</security:AuthenticationAgentIdentifier>
    //  <dd:UserName>u00042</dd:UserName>
    //  <dd:FullyQualifiedUserName>ETL\u00042</dd:FullyQualifiedUserName>
    //  <dd:UserDomain>ETL</dd:UserDomain>
    //  <dd:DisplayName>ETL\u00042</dd:DisplayName>
    //  <dd:Department>u00042</dd:Department>
    //  <dd:DepartmentAccessCode></dd:DepartmentAccessCode>
    //  <jasi:AuthenticationAgentUUID>16d1e6a6-d444-4df7-a173-265a0eeaf39c</jasi:AuthenticationAgentUUID>
    //  <dd:AuthenticationAgentName>Print</dd:AuthenticationAgentName>
    //  <jasi:AuthorizationAgentUUID>1ea05632-7cbd-11d7-81f0-00108378928a</jasi:AuthorizationAgentUUID>
    //  <jasi:AuthorizationAgentName>Authentication and Permission (Device)</jasi:AuthorizationAgentName>
    //  <jasi:AuthenticationCategory>Other</jasi:AuthenticationCategory>
    //</jasi:UserInfo>

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    public class UserLogItem
    {
        public string AuthenticationAgentIdentifier { get; set; }
        public string UserName { get; set; }
        public string FullyQualifiedUserName { get; set; }
        public string UserDomain { get; set; }
        public string DisplayName { get; set; }
        public string Department { get; set; }
        public string DepartmentAccessCode { get; set; }
        public string AuthenticationAgentUUID { get; set; }
        public string AuthenticationAgentName { get; set; }
        public string AuthorizationAgentUUID { get; set; }
        public string AuthorizationAgentName { get; set; }
        public string AuthenticationCategory { get; set; }

        public UserLogItem()
        {
            AuthenticationAgentIdentifier = string.Empty;
            UserName = string.Empty;
            FullyQualifiedUserName = string.Empty;
            UserDomain = string.Empty;
            DisplayName = string.Empty;
            Department = string.Empty;
            DepartmentAccessCode = string.Empty;
            AuthenticationAgentName = string.Empty;
            AuthenticationAgentUUID = string.Empty;
            AuthenticationCategory = string.Empty;
            AuthorizationAgentName = string.Empty;
            AuthorizationAgentUUID = string.Empty;
        }
        public override string ToString()
        {
            return "UserName={0}, AppAgentID={1}".FormatWith(UserName, AuthenticationAgentIdentifier);
        }
    }
}

using System;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    public partial class UserGroup
    {
        public UserGroup()
        {
            Creator = Environment.UserName;
        }

        public override string ToString()
        {
            return GroupName;
        }
    }
}

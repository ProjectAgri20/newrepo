using System;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    public partial class UserGroup
    {
        public static IQueryable<UserGroup> SelectAll(EnterpriseTestEntities entities)
        {
            return from u in entities.UserGroups.Include("VirtualMachineGroupAssocs").Include("Users")
                   select u;
        }
        
        public static bool InScenarioEditors(EnterpriseTestEntities entities, string userName)
        {
            return
                (
                    from g in entities.UserGroups
                    where g.GroupName.Equals("Scenario Editors")
                    select g into editors
                    from e in editors.Users
                    where e.UserName.Equals(userName)
                    select e
                ).Any();
        }
    }
}

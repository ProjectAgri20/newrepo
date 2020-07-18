using System;
using System.Linq;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    /// <summary>
    /// Class User.
    /// </summary>
    public partial class User
    {
        private string _userGroups = string.Empty;

        /// <summary>
        /// initializes the class
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// constructs an user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domain"></param>
        public User(string userName, string domain)
        {
            UserName = userName;
            Domain = domain;
            Role = UserRole.Guest;
        }

        /// <summary>
        /// Represents the role of the user
        /// </summary>
        public UserRole Role
        {
            get
            {
                return EnumUtil.Parse<UserRole>(RoleName);
            }

            set
            {
                RoleName = value.ToString();
            }
        }

        /// <summary>
        /// Determines whether the specified user is in the defined role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns><c>true</c> if the user is in the specified role; otherwise, <c>false</c>.</returns>
        public bool InRole(UserRole role)
        {
            return (int)role <= (int)EnumUtil.Parse<UserRole>(RoleName);
        }

        /// <summary>
        /// Selects a <see cref="User"/> of the given user name and domain.
        /// </summary>
        /// <param name="entities">The data context</param>
        /// <param name="username">The user name</param>
        /// <param name="domain">The user domain</param>
        /// <returns></returns>
        public static User Select(EnterpriseTestEntities entities, string username, string domain)
        {
            return
                (entities.Users.FirstOrDefault(
                    u =>
                        u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                        u.Domain.ToLower().StartsWith(domain.ToLower())));
        }

        /// <summary>
        /// Selects a distinct list of domains across all registered STF users.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IQueryable<string> SelectDistinctDomain(EnterpriseTestEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from u in entities.Users
                    select u.Domain).Distinct();
        }

        /// <summary>
        /// Commits changes to the specified user.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Commit(EnterpriseTestEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            User user = entities.Users.FirstOrDefault(n => n.UserName == this.UserName);
            if (user == null)
            {
                entities.Users.AddObject(this);
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="userName">The username.</param>
        public static void DeleteUser(EnterpriseTestEntities entities, string userName)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            User user = entities.Users.FirstOrDefault(n => n.UserName == userName);
            if (user != null)
            {
                entities.DeleteObject(user);
            }
        }

        public void JoinGroups()
        {
            _userGroups = string.Join(", ", UserGroups.Select(x => x.GroupName));
        }

        /// <summary>
        /// Gets the user groups string.
        /// </summary>
        /// <value>The user groups string.</value>
        public string UserGroupsString
        {
            get
            {
                if (string.IsNullOrEmpty(_userGroups))
                {
                    JoinGroups();
                }

                return _userGroups;
            }
        }
    }
}
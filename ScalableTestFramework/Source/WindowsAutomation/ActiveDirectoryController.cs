using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.WindowsAutomation
{
    /// <summary>
    /// Provides methods for working with Active Directory services via LDAP.
    /// </summary>
    public static class ActiveDirectoryController
    {
        /// <summary>
        /// Retrieves a list of all Active Directory groups for the specified context.
        /// </summary>
        /// <param name="context">The Active Directory <see cref="PrincipalContext" />.</param>
        /// <returns>A list of Active Directory <see cref="GroupPrincipal" /> objects.</returns>
        public static IEnumerable<GroupPrincipal> RetrieveGroups(PrincipalContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            LogDebug("Retrieving ActiveDirectory groups.");
            GroupPrincipal queryByExampleGroup = new GroupPrincipal(context);
            PrincipalSearcher searcher = new PrincipalSearcher(queryByExampleGroup);
            return searcher.FindAll().OfType<GroupPrincipal>();
        }

        /// <summary>
        /// Adds the specified user to the specified group.
        /// </summary>
        /// <param name="user">The <see cref="UserPrincipal" /> representing the user to add.</param>
        /// <param name="group">The <see cref="GroupPrincipal" /> representing the group.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user" /> is null.
        /// <para>or</para>
        /// <paramref name="group" /> is null.
        /// </exception>
        public static void AddUserToGroup(UserPrincipal user, GroupPrincipal group)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (!group.Members.Contains(user))
            {
                LogDebug($"Adding user {user.Name} to group {group.Name}");
                group.Members.Add(user);
                group.Save();
            }
        }

        /// <summary>
        /// Adds the specified user to the specified group using LDAP.
        /// </summary>
        /// <param name="domainName">The domain name.</param>
        /// <param name="userName">The username to add to the group.</param>
        /// <param name="groupName">The group name.</param>
        public static void AddUserToGroupLdap(string domainName, string userName, string groupName)
        {
            using (DirectoryEntry group = new DirectoryEntry($"WinNT://{Environment.MachineName}/{groupName},group"))
            {
                IEnumerable members = (IEnumerable)group.Invoke("Members");
                if (!MemberOf(userName, members))
                {
                    LogDebug($"Adding user {userName} to group {groupName}");
                    group.Invoke("Add", $"WinNT://{domainName}/{userName},user");
                    group.CommitChanges();
                }
            }
        }

        /// <summary>
        /// Checks the collection of <see cref="DirectoryEntry" /> objects for the specified user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="members">The collection of <see cref="DirectoryEntry" /></param>
        /// <returns>A value indicating whether the specified user is in the members collection.</returns>
        private static bool MemberOf(string userName, IEnumerable members)
        {
            foreach (object member in members)
            {
                using (DirectoryEntry user = new DirectoryEntry(member))
                {
                    if (user.Name.Equals(userName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the specified user to all the specified groups.
        /// </summary>
        /// <param name="user">The <see cref="UserPrincipal" /> representing the user to add.</param>
        /// <param name="groups">The <see cref="GroupPrincipal" /> collection representing the groups.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user" /> is null.
        /// <para>or</para>
        /// <paramref name="groups" /> is null.
        /// </exception>
        public static void AddUserToGroups(UserPrincipal user, IEnumerable<GroupPrincipal> groups)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (groups == null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            foreach (GroupPrincipal group in groups)
            {
                AddUserToGroup(user, group);
            }
        }

        /// <summary>
        /// Removes the specified user from the specified group.
        /// </summary>
        /// <param name="user">The <see cref="UserPrincipal" /> representing the user to remove.</param>
        /// <param name="group">The <see cref="GroupPrincipal" /> representing the group.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user" /> is null.
        /// <para>or</para>
        /// <paramref name="group" /> is null.
        /// </exception>
        public static void RemoveUserFromGroup(UserPrincipal user, GroupPrincipal group)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (group.Members.Contains(user))
            {
                LogDebug($"Removing user {user.Name} from group {group.Name}");
                group.Members.Remove(user);
                group.Save();
            }
        }

        /// <summary>
        /// Removes the specified user from all the specified groups.
        /// </summary>
        /// <param name="user">The <see cref="UserPrincipal" /> representing the user to remove.</param>
        /// <param name="groups">The <see cref="GroupPrincipal" /> collection representing the groups.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user" /> is null.
        /// <para>or</para>
        /// <paramref name="groups" /> is null.
        /// </exception>
        public static void RemoveUserFromGroups(UserPrincipal user, IEnumerable<GroupPrincipal> groups)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (groups == null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            foreach (GroupPrincipal group in groups)
            {
                RemoveUserFromGroup(user, group);
            }
        }
    }
}

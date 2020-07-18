using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// A composite class that combines <see cref="VirtualMachineReservation"/> data with Platform usage and User/Group usage.
    /// </summary>
    public class VirtualMachine
    {
        /// <summary>
        /// This is an arbitrary string used to allow default parameters in select statements for this class.
        /// This string can be changed at will, but should be complex enough that there is no conceivable situation
        /// in which a user might use this string as search criteria for HoldId, SessionId, PlatformId, or HostName.
        /// </summary>
        private const string NoCriteria = "NOCRITERIAFORTHISPARAMETER";

        public VirtualMachine(FrameworkClient reservation)
        {
            this.Name = reservation.FrameworkClientHostName;
            this.PowerState = reservation.PowerState;
            this.UsageState = reservation.UsageState;
            this.PlatformUsage = reservation.PlatformUsage;
            this.HoldId = reservation.HoldId;
            this.LastUpdated = reservation.LastUpdated;
            this.SortOrder = reservation.SortOrder;
            this.SessionId = reservation.SessionId;
            this.MachineType = "WindowsVirtual";
            this.Platforms = new Collection<FrameworkClientPlatform>(reservation.Platforms.ToList());
        }

        /// <summary>
        /// The VM Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Power State.
        /// </summary>
        public string PowerState { get; set; }

        /// <summary>
        /// The Usage State.
        /// </summary>
        public string UsageState { get; set; }

        /// <summary>
        /// The Platform Usage.
        /// </summary>
        public string PlatformUsage { get; set; }

        /// <summary>
        /// The Hold Id.
        /// </summary>
        public string HoldId { get; set; }

        /// <summary>
        /// The last time the state was updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// The Sort Order value.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// The Session Id.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The Operating System Type
        /// </summary>
        public string MachineType { get; set; }

        /// <summary>
        /// The Platforms collection.
        /// </summary>
        public Collection<FrameworkClientPlatform> Platforms { get; }

        /// <summary>
        /// Selects all <see cref="VirtualMachine"/>s.  Does not load UserGroups and Platforms.
        /// </summary>
        /// <returns>An <see cref="IQueryable"/> collection of <see cref="VirtualMachine"/> objects.</returns>
        public static IEnumerable<VirtualMachine> SelectAll()
        {
            Collection<VirtualMachine> result = new Collection<VirtualMachine>();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (FrameworkClient reservation in context.FrameworkClients)
                {
                    result.Add(new VirtualMachine(reservation));
                }
            }

            return result;
        }

        /// <summary>
        /// Selects all <see cref="VirtualMachine"/>s with the specified parameters.
        /// </summary>
        /// <param name="powerState">If specified, filters results to those with the specified <see cref="VMPowerState"/>.</param>
        /// <param name="usageState">If specified, filters results to those with the specified <see cref="VMUsageState"/>.</param>
        /// <param name="sessionId">If specified, filters results to those with the specified session id.</param>
        /// <param name="holdId">If specified, filters results to those with the specified hold id.</param>
        /// <param name="platform">If specified, filters results to those that have an association with the specified platform.</param>
        /// <returns></returns>
        public static IEnumerable<VirtualMachine> Select
            (
                VMPowerState powerState = VMPowerState.None,
            VMUsageState usageState = VMUsageState.None,
            string sessionId = NoCriteria,
            string holdId = NoCriteria,
            string platform = NoCriteria)
        {
            Collection<VirtualMachine> result = new Collection<VirtualMachine>();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (FrameworkClient reservation in Select(context, powerState, usageState, sessionId, holdId))
                {
                    result.Add(new VirtualMachine(reservation));
                }
            }
            if (platform != NoCriteria)
            {
                return result.Where(v => v.Platforms.Any(p => p.FrameworkClientPlatformId == platform)).OrderBy(n => n.SortOrder);
            }

            return result.OrderBy(n => n.SortOrder);
        }

        /// <summary>
        /// Selects all <see cref="VirtualMachineReservation"/>s with the specified parameters.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="powerState">If specified, filters results to those with the specified <see cref="VMPowerState"/>.</param>
        /// <param name="usageState">If specified, filters results to those with the specified <see cref="VMUsageState"/>.</param>
        /// <param name="sessionId">If specified, filters results to those with the specified session id.</param>
        /// <param name="holdId">If specified, filters results to those with the specified hold id.</param>
        /// <returns></returns>
        public static IQueryable<FrameworkClient> Select
            (
                AssetInventoryContext entities,
                VMPowerState powerState = VMPowerState.None,
                VMUsageState usageState = VMUsageState.None,
                string sessionId = NoCriteria,
                string holdId = NoCriteria
            )
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            IQueryable<FrameworkClient> results = entities.FrameworkClients.Include(n => n.Platforms);

            if (powerState != VMPowerState.None)
            {
                string powerStateValue = EnumUtil.GetDescription(powerState);
                results = results.Where(n => n.PowerState == powerStateValue);
            }

            if (usageState != VMUsageState.None)
            {
                string usageStateValue = EnumUtil.GetDescription(usageState);
                results = results.Where(n => n.UsageState == usageStateValue);
            }

            if (sessionId != NoCriteria)
            {
                results = (string.IsNullOrEmpty(sessionId)) ?
                    results.Where(n => string.IsNullOrEmpty(n.SessionId)) :
                    results.Where(n => n.SessionId == sessionId);
            }

            if (holdId != NoCriteria)
            {
                results = (string.IsNullOrEmpty(holdId)) ?
                    results.Where(n => string.IsNullOrEmpty(n.HoldId)) :
                    results.Where(n => n.HoldId == holdId);
            }

            return results.OrderBy(n => n.SortOrder);
        }

        /// <summary>
        /// Selects the <see cref="VirtualMachine"/> with the given host name.
        /// </summary>
        /// <param name="hostName">The host name</param>
        /// <returns></returns>
        public static VirtualMachine Select(string hostName)
        {
            FrameworkClient reservation = null;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                reservation = context.FrameworkClients.FirstOrDefault(n => n.FrameworkClientHostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
            }

            return (reservation == null) ? null : new VirtualMachine(reservation);

        }

        /// <summary>
        /// Selects a replacement <see cref="VirtualMachine"/> of the given platform usage.
        /// </summary>
        /// <param name="platformUsage"></param>
        /// <returns></returns>
        public static VirtualMachine SelectReplacement(string platformUsage, string holdId)
        {
            //Get a list of VMs that contain the same platform and hold setting as the platform being used by the VM being replaced.
            IEnumerable<VirtualMachine> availableVMs = Select(VMPowerState.PoweredOff, VMUsageState.Available, holdId: holdId, platform: platformUsage);

            // Return the first available VM.
            return availableVMs.FirstOrDefault();
        }

        /// <summary>
        /// Selects a relationship between Virtual Machines and current Sessions
        /// </summary>
        /// <returns>A list that represents the left outer join of these two tables</returns>
        public static dynamic SelectVirtualMachineSessionInfo()
        {
            List<FrameworkClient> virtualMachines = null;
            List<SessionInfo> sessionSummarySet = null;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                virtualMachines = context.FrameworkClients.ToList();
            }

            //Get the list of sessions that are currently consuming VMs
            IEnumerable<string> sessionIds = virtualMachines.Select(s => s.SessionId).Distinct();

            using (DataLogContext context = DbConnect.DataLogContext())
            {
                sessionSummarySet = context.Sessions.Where(s => sessionIds.Contains(s.SessionId)).ToList();
            }

            dynamic results = null;
            results =
            (
                from v in virtualMachines
                from s in sessionSummarySet.Where(s => s.SessionId == v.SessionId).DefaultIfEmpty()
                orderby v.SortOrder
                select new
                {
                    HoldId = v.HoldId,
                    LastUpdated = v.LastUpdated,
                    Name = v.FrameworkClientHostName,
                    PlatformUsage = v.PlatformUsage,
                    PowerState = v.PowerState,
                    SessionId = v.SessionId,
                    SortOrder = v.SortOrder,
                    UsageState = v.UsageState,
                    Environment = v.Environment,
                    Owner = (s == null ? string.Empty : s.Owner),
                    Status = (s == null ? string.Empty : s.Status),
                    StartDate = s?.StartDateTime?.LocalDateTime
                }
            ).ToList();

            return results;
        }

        /// <summary>
        /// Selects a summary of sessions and their Virtual Machine usage
        /// </summary>
        /// <returns></returns>
        public static dynamic SelectVirtualMachineSessionUsageSummary()
        {
            List<FrameworkClient> virtualMachines = null;
            List<SessionInfo> sessionSummarySet = null;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                virtualMachines = context.FrameworkClients.ToList();
            }

            //Get the list of sessions that are currently consuming VMs
            IEnumerable<string> sessionIds = virtualMachines.Select(s => s.SessionId).Distinct();

            using (DataLogContext context = DbConnect.DataLogContext())
            {
                sessionSummarySet = context.Sessions.Where(s => sessionIds.Contains(s.SessionId)).ToList();
            }

            var now = DateTime.Now;
            var workingSet =
            (
                from v in virtualMachines
                from s in sessionSummarySet.Where(s => s.SessionId == v.SessionId)
                orderby v.SortOrder
                select new
                {
                    SessionId = s.SessionId,
                    ScenarioName = s.SessionName,
                    Owner = s.Owner,
                    Status = s.Status,
                    Dispatcher = s.Dispatcher,
                    StartDate = s.StartDateTime?.LocalDateTime,
                    EndDate = s.ProjectedEndDateTime?.LocalDateTime,
                    TimeRunning = (s.StartDateTime == null ? "Unknown" : "{0:%d} Days {0:%h} Hours".FormatWith(now - s.StartDateTime)),
                    XP = (v.FrameworkClientHostName != null && v.FrameworkClientHostName.StartsWith("X", StringComparison.OrdinalIgnoreCase)) ? 1 : 0,
                    W7 = (v.FrameworkClientHostName != null && v.FrameworkClientHostName.StartsWith("W7", StringComparison.OrdinalIgnoreCase)) ? 1 : 0,
                    W8 = (v.FrameworkClientHostName != null && v.FrameworkClientHostName.StartsWith("W8", StringComparison.OrdinalIgnoreCase)) ? 1 : 0,
                }
            ).ToList();

            // summarize the results by grouping and counting
            dynamic result = (from f in workingSet
                              group f by f.SessionId into s
                              select new
                              {
                                  SessionId = s.Key,
                                  ScenarioName = s.First().ScenarioName,
                                  Owner = s.First().Owner,
                                  Status = s.First().Status,
                                  Dispatcher = s.First().Dispatcher,
                                  StartDate = s.First().StartDate,
                                  EndDate = s.First().EndDate,
                                  TimeRunning = s.First().TimeRunning,
                                  XPCount = s.Sum(x => x.XP),
                                  WSevCount = s.Sum(x => x.W7),
                                  W8Count = s.Sum(x => x.W8),
                              }).ToList();
            return result;
        }

        /// <summary>
        /// Returns the count of VMs available for the given HoldId, platformId and User role.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="platformId"></param>
        /// <param name="holdId"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static int GetUsageCountByHold(string platformId, string holdId, UserCredential user)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                // Query for clients that have the requested platform and hold ID
                var clients = context.FrameworkClients.Where(n => n.Platforms.Any(m => m.FrameworkClientPlatformId == platformId) && n.HoldId == holdId);

                // For administrators, return all clients
                if (user.HasPrivilege(UserRole.Administrator))
                {
                    return clients.Count();
                }
                else
                {
                    // For non-admins, return only clients associated with a group that the user is in
                    List<string> clientNames = clients.Select(n => n.FrameworkClientHostName).ToList();
                    List<string> allowedClients = GetAllowedClients(user);
                    return clientNames.Intersect(allowedClients).Count();
                }
            }
        }

        /// <summary>
        /// Returns the count of VMs available for the given platformId and User role.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="platformId"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static int GetUsageCountByPlatform(string platformId, UserCredential user)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                // Query for clients that have the requested platform
                var clients = context.FrameworkClients.Where(n => n.Platforms.Any(m => m.FrameworkClientPlatformId == platformId));

                // For administrators, return all clients
                if (user.HasPrivilege(UserRole.Administrator))
                {
                    return clients.Count();
                }
                else
                {
                    // For non-admins, return only clients associated with a group that the user is in
                    List<string> clientNames = clients.Select(n => n.FrameworkClientHostName).ToList();
                    List<string> allowedClients = GetAllowedClients(user);
                    return clientNames.Intersect(allowedClients).Count();
                }
            }
        }

        private static List<string> GetAllowedClients(UserCredential user)
        {
            using (var enterpriseTestContext = DbConnect.EnterpriseTestContext())
            {
                return enterpriseTestContext.UserGroups
                    .Where(n => n.Users.Any(m => m.UserName == user.UserName))
                    .SelectMany(n => n.FrameworkClients)
                    .Select(n => n.FrameworkClientHostName).ToList();
            }
        }

        /// <summary>
        /// Selects a list of <see cref="VirtualMachine"/>s that have the given HoldId
        /// </summary>
        /// <param name="holdId">The hold Id</param>
        /// <returns></returns>
        private static List<VirtualMachine> SelectByHoldId(string holdId)
        {
            List<VirtualMachine> result = new List<VirtualMachine>();

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (FrameworkClient reservation in Select(context, holdId: holdId))
                {
                    result.Add(new VirtualMachine(reservation));
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Enumeration describing different power states for Virtual Machines
    /// </summary>
    public enum VMPowerState
    {
        /// <summary>
        /// No power state selected - for use in queries
        /// </summary>
        /// <remarks>
        /// Do not add an Description - this prevents it from showing up in UI lists
        /// </remarks>
        None,

        /// <summary>
        /// Describes a Virtual Machine in a Powered On state
        /// </summary>
        [Description("Powered On")]
        PoweredOn,

        /// <summary>
        /// Describes a Virtual Machine in a Powered Off state
        /// </summary>
        [Description("Powered Off")]
        PoweredOff
    }

    /// <summary>
    /// Enumeration describing different usage states for Virtual Machines
    /// </summary>
    public enum VMUsageState
    {
        /// <summary>
        /// No usage state selected - for use in queries
        /// </summary>
        /// <remarks>
        /// Do not add an Description - this prevents it from showing up in UI lists
        /// </remarks>
        None,

        /// <summary>
        /// Describes a Virtual Machine in an available state and accessible by general users
        /// </summary>
        [Description("Available")]
        Available,

        /// <summary>
        /// Describes a Virtual Machine in a reserved state where it is not accessible by general users
        /// </summary>
        [Description("Reserved")]
        Reserved,

        /// <summary>
        /// Describes a Virtual Machine that is already booted and in use
        /// </summary>
        [Description("In Use")]
        InUse,

        /// <summary>
        /// Describes a Virtual Machine that cannot be scheduled by users
        /// </summary>
        [Description("Do Not Schedule")]
        DoNotSchedule,

        /// <summary>
        /// Describes a Virtual Machine that is unavailable or down for maintenance
        /// </summary>
        /// <remarks>
        /// Do not add an Description - this prevents it from showing up in UI lists
        /// </remarks>
        Unavailable,
    }
}

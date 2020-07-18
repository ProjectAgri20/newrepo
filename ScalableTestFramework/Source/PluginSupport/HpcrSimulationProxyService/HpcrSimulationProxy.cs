using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Hpcr;

namespace HP.ScalableTest.PluginSupport.HpcrSimulationProxyService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    public sealed class HpcrSimulationProxy : IHpcrConfigurationProxyService, IHpcrExecutionProxyService, IDisposable
    {
        private Dictionary<string, CaptureRouteController> _controllerCache = new Dictionary<string, CaptureRouteController>();

        private CaptureRouteController GetCaptureRouteController(string hpcrServerAddress)
        {
            Logger.LogDebug(string.Format("{0} controllers currently in the cache", _controllerCache.Count));
            CaptureRouteController result = null;
            Logger.LogDebug(string.Format("Looking up CaptureRouteController for {0}...", hpcrServerAddress));
            if (!string.IsNullOrEmpty(hpcrServerAddress))
            {
                if (_controllerCache.ContainsKey(hpcrServerAddress))
                {
                    result = _controllerCache[hpcrServerAddress];
                }
                else
                {
                    Logger.LogDebug(string.Format("Adding new CaptureRouteController to cache for {0}", hpcrServerAddress));
                    result = new CaptureRouteController(hpcrServerAddress);
                    _controllerCache.Add(hpcrServerAddress, result);
                    Logger.LogDebug(string.Format("{0} controllers currently in the cache", _controllerCache.Count));
                }
            }
            return result;
        }

        public Collection<string> GetConfiguredUsers(string hpcrServerAddress)
        {
            Logger.LogDebug("Fetching configured users from CaptureRouteController");
            var result = GetCaptureRouteController(hpcrServerAddress).GetConfiguredUsers();
            return result;
        }

        public Collection<Membership> GetGroupMemberships(string hpcrServerAddress, string userEmailAddress)
        {
            Logger.LogDebug("Fetching group memberships from CaptureRouteController");
            var result = GetCaptureRouteController(hpcrServerAddress).GetGroupMemberships(userEmailAddress);
            return result;
        }

        public Collection<Distribution> GetDistributions(string hpcrServerAddress, string userEmailAddress)
        {
            Logger.LogDebug("Fetching distributions from CaptureRouteController for " + userEmailAddress);
            var result = GetCaptureRouteController(hpcrServerAddress).GetUserDistributions(userEmailAddress);
            Logger.LogDebug(string.Format("Found {0} distributions for {1}", result.Count, userEmailAddress));
            return result;
        }

        public void DeliverToDistribution(string hpcrServerAddress, string documentPath, string originatorEmail, string distributionTitle)
        {
            Logger.LogDebug(string.Format("DeliverToDistribution: document = {0}, originator = {1}, distribution = {2}", documentPath, originatorEmail, distributionTitle));
            GetCaptureRouteController(hpcrServerAddress).DeliverToDistribution(documentPath, originatorEmail, distributionTitle);
        }

        public void DeliverToEmailByDocument(string hpcrServerAddress, string documentPath, string originatorEmail, string destinationEmail)
        {
            Logger.LogDebug(string.Format("DeliverToEmailByDocument: document = {0}, originator = {1}, distribution = {2}", documentPath, originatorEmail, destinationEmail));
            GetCaptureRouteController(hpcrServerAddress).DeliverToEmail(documentPath, originatorEmail, destinationEmail);
        }

        public void Dispose()
        {
            if (_controllerCache != null)
            {
                foreach (var key in _controllerCache.Keys)
                {
                    var item = _controllerCache[key];
                    if (item != null)
                    {
                        item.Dispose();
                        item = null;
                    }
                }
                _controllerCache.Clear();
                Logger.LogDebug("HpcrProxyService instance has been disposed");
            }
        }
    }
}

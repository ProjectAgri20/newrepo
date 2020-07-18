using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Hpcr;
using HP.ScalableTest.Utility;
using Omtool.MOCI;
using Omtool.Properties;
using Omtool.Server;

namespace HP.ScalableTest.PluginSupport.HpcrSimulationProxyService
{
    internal class CaptureRouteController : IDisposable
    {
        public enum DistributionType { myDistribution = 0, publicDistribution = 1 };
        private MessageServer _server = null;
        private string _serverAddress = string.Empty;


        public CaptureRouteController(string connection)
        {
            _serverAddress = connection;
            _server = MessageServer.Connect(_serverAddress);
        }

        public string ServerAddress
        {
            get { return _serverAddress; }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_server != null)
                {
                    _server.Dispose();
                    _server = null;
                }
            }
        }

        /// <summary>
        /// Delivers to distribution.
        /// </summary>
        /// <param name="documentFilename">The document filename.</param>
        /// <param name="inUserEmail">The in user email.</param>
        /// <param name="selectedDistributionTitle">The selected distribution title.</param>
        public void DeliverToDistribution(string documentFilename, string inUserEmail, string selectedDistributionTitle)
        {
            DeliverToDistribution(documentFilename, inUserEmail, selectedDistributionTitle, true);
        }

        /// <summary>
        /// Delivers to HPCR distribution.
        /// </summary>
        /// <param name="documentFilename">The document filename.</param>
        /// <param name="inUserEmail">The in user email.</param>
        /// <param name="selectedDistributionTitle">The selected distribution title.</param>
        /// <param name="convertToConfiguredUser">Whether to configure inUserEmail as an HPCR configured user first (throws if user equivalent not found)</param>
        private void DeliverToDistribution(string documentFilename, string inUserEmail, string selectedDistributionTitle, bool convertToConfiguredUser)
        {
            try
            {
                Logger.LogDebug(string.Format("Deliver {0} to {1} distribution {2}", documentFilename, inUserEmail, selectedDistributionTitle));
                var userEmail = (convertToConfiguredUser ? GetConfiguredUser(inUserEmail) : inUserEmail);
                EmbeddedDirective directive;
                Dictionary<string, EmbeddedDirective> usersDistributions = new Dictionary<string, EmbeddedDirective>();
                this.GetUsersDistributions(userEmail, usersDistributions);
                Logger.LogDebug(string.Format("Found {0} distributions for {1}", usersDistributions.Count, userEmail));
                if (usersDistributions.TryGetValue(selectedDistributionTitle, out directive))
                {
                    PropertySet routingRecipients = directive.RoutingRecipients;
                    this.Deliver(documentFilename, userEmail, routingRecipients);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Delivers to HPCR distribution.
        /// </summary>
        /// <param name="documentPaths">The document paths.</param>
        /// <param name="inOriginatorEmail">The in originator email.</param>
        /// <param name="distributionTitle">The distribution title.</param>
        public void DeliverToDistribution(Collection<string> documentPaths, string inOriginatorEmail, string distributionTitle)
        {
            Logger.LogDebug(string.Format("DeliverToDistribution: originator={0}, distributionTitle={1}", inOriginatorEmail, distributionTitle));
            var originatorEmail = GetConfiguredUser(inOriginatorEmail);
            documentPaths.ToList().ForEach(x => DeliverToDistribution(x, originatorEmail, distributionTitle, false));
        }

        /// <summary>
        /// Gets the user configured on the HPCR server (ignores case)
        /// </summary>
        /// <param name="requestedUser">The requested user.</param>
        /// <returns></returns>
        private string GetConfiguredUser(string requestedUser)
        {
            var user = GetConfiguredUsers().FirstOrDefault(x => x.EqualsIgnoreCase(requestedUser));
            if (string.IsNullOrEmpty(user))
            {
                throw new Exception(string.Format("User [{0}] not configured in HPCR server {1}", requestedUser, ServerAddress));
            }
            return user;
        }

        public Collection<Distribution> GetUserDistributions(string inUserEmail)
        {
            Logger.LogDebug("GetUserDistributions");
            Collection<Distribution> collection = new Collection<Distribution>();
            try
            {
                var userEmail = GetConfiguredUser(inUserEmail);
                Logger.LogDebug("Getting distributions for " + userEmail);
                Dictionary<string, EmbeddedDirective> usersDistributions = new Dictionary<string, EmbeddedDirective>();
                this.GetUsersDistributions(userEmail, usersDistributions);
                Distribution item = null;
                Logger.LogDebug(string.Format("Converting {0} embedded directives to distribution objects", usersDistributions.Count));
                foreach (EmbeddedDirective directive in usersDistributions.Values)
                {
                    TaggedValueList list2;
                    item = new Distribution();
                    PropertyList source = directive.Source;
                    item.Title = source.Get(PropID.Description);
                    item.ApplicationName = source.Get(PropID.ApplicationName);
                    item.ApplicationTag = source.Get(PropID.ApplicationTag);
                    item.UseOnlyOnce = this.ParseBool(source.Get(PropID.UseOnlyOnce));
                    item.Public = this.ParseBool(source.Get(PropID.PublicEmbeddedDirective));
                    item.ShowDeviceAtTop = this.ParseBool(source.Get(PropID.DeviceShowAtTop));
                    item.ScanSettings = source.Get(PropID.ScanSettings);
                    source.Get(PropID.TemplateVars, out list2);
                    item.Subject = list2.Get("SUBJECT");
                    item.Comment = list2.Get("COMMENT");
                    PropertyList general = directive.General;
                    int sValue = 0;
                    general.Get(PropID.Type, out sValue);
                    item.Type = Enum.GetName(typeof(RecipientType), sValue);
                    item.DateCreated = DateTime.Parse(general.Get(PropID.DateCreated));
                    PropertySet routingRecipients = directive.RoutingRecipients;
                    item.Recipients = routingRecipients.Xml;
                    collection.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error", ex);
                throw;
            }
            return collection;
        }

        /// <summary>
        /// This is the original method, to be deleted... or not.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="usersDistributions"></param>
        private void GetUsersDistributions(string userEmail, Dictionary<string, EmbeddedDirective> usersDistributions)
        {
            Dictionary<string, DistributionType> subscribedUsers = new Dictionary<string, DistributionType>();
            subscribedUsers.Add(userEmail, DistributionType.myDistribution);
            this.getSubscribedUsers(userEmail, subscribedUsers);
            ServerContainer container = this._server.GetContainer(ObjectType.EmbeddedDirectivesV2);
            foreach (PropertyList list in container.Items(null))
            {
                EntryID yid;
                list.Get(PropID.EntryID, out yid);
                EmbeddedDirective directive = (EmbeddedDirective)container.Open(yid);
                string key = directive.Source.Get(PropID.Description);
                string str2 = directive.Source.Get(PropID.ApplicationTag);
                if (str2 == userEmail)
                {
                    usersDistributions.Add(key, directive);
                }
                else
                {
                    DistributionType type;
                    if (subscribedUsers.TryGetValue(str2, out type) && (type == DistributionType.publicDistribution))
                    {
                        usersDistributions.Add(key, directive);
                    }
                }
            }
        }

        private void getSubscribedUsers(string userEmail, Dictionary<string, DistributionType> subscribedUsers)
        {
            ServerContainer container = this._server.GetContainer(ObjectType.ApplicationData);
            foreach (PropertyList list in container.Items(null))
            {
                if ((list.Get(PropID.Type) == "USER") && (list.Get(PropID.ApplicationTag) == userEmail))
                {
                    PropertySet ps = new PropertySet();
                    list.Get(PropID.PublicEDUsersList, out ps);
                    DistributionType publicDistribution = DistributionType.publicDistribution;
                    foreach (PropertyList list2 in ps)
                    {
                        string key = list2.Get(PropID.ApplicationTag);
                        if (!subscribedUsers.TryGetValue(key, out publicDistribution))
                        {
                            subscribedUsers.Add(key, DistributionType.publicDistribution);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the configured users.
        /// </summary>
        /// <returns>collection of strings</returns>
        public Collection<string> GetConfiguredUsers()
        {
            var result = new Collection<string>();
            try
            {
                ServerContainer container = this._server.GetContainer(ObjectType.ApplicationData);
                foreach (PropertyList list in container.Items(null))
                {
                    if ((list.Get(PropID.Type) == "USER"))
                    {
                        result.Add(list.Get(PropID.ApplicationTag));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error", ex);
                throw;
            }
            return result;
        }

        private void Deliver(string filename, string originatorEmailAddress, PropertySet psRecipients)
        {
            try
            {
                Logger.LogDebug(string.Format("Delivering {0} for {1}", string.Join(", ", filename), originatorEmailAddress));
                using (PropertyList plMessage = new PropertyList())
                {
                    plMessage.Set(PropID.Originator, originatorEmailAddress);
                    using (var documents = new Documents())
                    {
                        Logger.LogDebug("Pull files into HPCR Document object");
                        using (Stream stream = File.Open(filename, FileMode.Open))
                        {
                            documents.Add(Path.GetFileName(filename), stream, Document.DocumentFlags.None);
                            Logger.LogDebug("Deliver the documents to HPCR");

                            // Note that order seems to matter:  
                            // Disposing the file stream before attempting to deliver the message seems to cause errors
                            using (var message = this._server.Deliver(plMessage, psRecipients, documents))
                            {
                                Logger.LogDebug("Message delivered to server");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error", ex);
                throw;
            }
        }

        private void Deliver(Collection<string> filenames, string originatorEmailAddress, PropertySet psRecipients)
        {
            // It appears we have to deliver files one at a time or else encounter an error.
            Logger.LogDebug(string.Format("Delivering {0} for {1}", string.Join(", ", filenames), originatorEmailAddress));
            filenames.ToList().ForEach(x => Deliver(x, originatorEmailAddress, psRecipients));
        }

        /// <summary>
        /// Delivers to email.
        /// </summary>
        /// <param name="documentFilename">The document filename.</param>
        /// <param name="originatorEmail">The originator email.</param>
        /// <param name="destinationEmail">The destination email.</param>
        public void DeliverToEmail(string documentFilename, string originatorEmail, string destinationEmail)
        {
            Logger.LogDebug(string.Format("Delivering {0} for {1} to {2}", documentFilename, originatorEmail, destinationEmail));
            DeliverToEmail(new Collection<string>() { documentFilename }, originatorEmail, destinationEmail);
        }

        /// <summary>
        /// Delivers to email.
        /// </summary>
        /// <param name="documentFilePaths">The document file paths.</param>
        /// <param name="originatorEmail">The originator email.</param>
        /// <param name="destinationEmail">The destination email.</param>
        public void DeliverToEmail(Collection<string> documentFilePaths, string originatorEmail, string destinationEmail)
        {
            Logger.LogDebug(string.Format("Delivering {0} documents for {1} to {2}", documentFilePaths.Count, originatorEmail, destinationEmail));
            PropertySet psRecipients = GetRecipientForEmailDelivery(destinationEmail);
            this.Deliver(documentFilePaths, originatorEmail, psRecipients);
        }

        private PropertySet GetRecipientForEmailDelivery(string destinationEmail)
        {
            PropertySet psRecipients = new PropertySet();

            var destinations = destinationEmail.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string destination in destinations)
            {
                PropertyList pl = new PropertyList();
                pl.Set(PropID.Destination, destination.Trim());
                pl.Set(PropID.RecipientType, RecipientType.Email);
                psRecipients.Add(pl);
            }
            return psRecipients;
        }

        /// <summary>
        /// Gets the group memberships.
        /// </summary>
        /// <param name="userEmailAddress">The user email address.</param>
        /// <returns></returns>
        public Collection<Membership> GetGroupMemberships(string userEmailAddress)
        {
            Logger.LogDebug("Getting group memberships for " + userEmailAddress);
            var result = new Collection<Membership>();
            PropertyList pl = new PropertyList();
            pl.Set(PropID.TokenType, TokenType.Email);
            pl.Set(PropID.TokenId, userEmailAddress);
            PropertySet psToken = new PropertySet();
            psToken.Add(pl);
            PropertySet memberOf = this._server.GetMemberOf(psToken);
            var doc = this.formatXmlPropertyList(memberOf.Xml);

            var nodes = doc.SelectNodes("//prMembers");
            Logger.LogDebug(string.Format("Converting {0} to membership objects", nodes.Count));
            foreach (XmlNode node in nodes)
            {
                try
                {
                    var propNode = node.SelectSingleNode("PropertyList");
                    var member = new Membership()
                    {
                        Guid = propNode.SelectSingleNode("prGUID").InnerText,
                        Name = propNode.SelectSingleNode("prName").InnerText,
                        DistinguishedName = propNode.SelectSingleNode("prDistinguishedName").InnerText,
                    };
                    result.Add(member);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        private XmlDocument formatXmlPropertyList(string propertiesXml)
        {
            XmlDocument result = new XmlDocument();
            result.LoadXml(@"<ed>" + propertiesXml + @"</ed>");
            return result;
        }

        private bool ParseBool(string value)
        {
            switch (value)
            {
                case "0":
                    return false;

                case "1":
                    return true;
            }
            return bool.Parse(value);
        }
    }
}

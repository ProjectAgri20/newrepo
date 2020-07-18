using System;
using System.ServiceModel;
using System.Xml.Linq;
using HP.Epr.Framework;
using HP.Epr.WebServicesResponder.WSStandard;

namespace HP.Epr.WebServicesResponder
{
    /// <summary>
    /// WS-Transfer client.
    /// </summary>
    public class WSTransferClient
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the role's password.
        /// </summary>
        public string RolePassword { get; set; }

        /// <summary>
        /// Gets the locale.
        /// </summary>
        /// <value>The locale.</value>
        public Locale Locale { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WSTransferClient"/> class.
        /// </summary>
        /// <param name="role">The role.</param>
        public WSTransferClient(string role, string rolePassword)
        {
            Role = role;
            RolePassword = rolePassword;
            Locale = new Locale("en-US");

            // These calls won't work unless we ignore cert issues.
            InvalidHttpsCertificateIgnorer.SetCertificatePolicy();
        }

        /// <summary>
        /// Gets the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="urn">The urn.</param>
        /// <param name="selectorName">Name of the selector.</param>
        /// <param name="selectorValue">The selector value.</param>
        /// <returns></returns>
        public XElement Get(Uri service, Uri urn, string selectorName = null, string selectorValue = null)
        {
            var request = CreateGetRequest(urn, selectorName, selectorValue);

            var factory = CreateFactory(service);
            var resource = factory.CreateChannel();

            try
            {
                return resource.Get(request).Body.ToXElement();
            }
            finally
            {
                CleanupChannelResources(factory, resource);
            }
        }

        /// <summary>
        /// Pushes new data to a service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="urn">The urn.</param>
        /// <param name="body">The body.</param>
        /// <param name="selectorName">Name of the selector.</param>
        /// <param name="selectorValue">The selector value.</param>
        /// <returns></returns>
        public XElement Put(Uri service, Uri urn, XElement body, string selectorName = null, string selectorValue = null)
        {
            var selectorSet = CreateSelectorSet(selectorName, selectorValue);
            var resourceUri = CreateResourceUri(urn);
            var request = new PutRequest(resourceUri, Locale, selectorSet, body == null ? null : body.ToXmlElement());

            var factory = CreateFactory(service);
            var resource = factory.CreateChannel();
            try
            {
                var result = resource.Put(request).Body;
                return result == null ? null : result.ToXElement();
            }
            finally
            {
                CleanupChannelResources(factory, resource);
            }
        }

        /// <summary>
        /// Creates new data for a service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="urn">The urn.</param>
        /// <param name="body">The body.</param>
        /// <param name="selectorName">Name of the selector.</param>
        /// <param name="selectorValue">The selector value.</param>
        /// <returns></returns>
        public XElement Create(Uri service, Uri urn, XElement body, string selectorName = null, string selectorValue = null)
        {
            var selectorSet = CreateSelectorSet(selectorName, selectorValue);
            var resourceUri = CreateResourceUri(urn);
            var request = new PutRequest(resourceUri, Locale, selectorSet, body == null ? null : body.ToXmlElement());

            var factory = CreateResourceFactoryFactory(service);
            var resource = factory.CreateChannel();
            try
            {
                var result = resource.Create(request).Any;
                return result == null ? null : result.ToXElement();
            }
            finally
            {
                CleanupChannelResources(factory, resource);
            }
        }

        /// <summary>
        /// Deletes a ticket from the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="urn">The urn.</param>
        /// <param name="selectorName">Name of the selector.</param>
        /// <param name="selectorValue">The selector value.</param>
        public void Delete(Uri service, Uri urn, string selectorName = null, string selectorValue = null)
        {
            var request = CreateGetRequest(urn, selectorName, selectorValue);

            var factory = CreateFactory(service);
            var resource = factory.CreateChannel();
            try
            {
                resource.Delete(request);
            }
            finally
            {
                CleanupChannelResources(factory, resource);
            }
        }

        #region Support methods.
        private ChannelFactory<Resource> CreateFactory(Uri service)
        {
            var factory = new ChannelFactory<Resource>(BindingUtil.CreateBinding(service), new EndpointAddress(service));

            // username must be admin, password can be anything.
            factory.Credentials.UserName.UserName = Role;
            factory.Credentials.UserName.Password = RolePassword;
            return factory;
        }

        private ChannelFactory<ResourceFactory> CreateResourceFactoryFactory(Uri service)
        {
            var factory = new ChannelFactory<ResourceFactory>(BindingUtil.CreateBinding(service), new EndpointAddress(service));

            // username must be admin, password can be anything.
            factory.Credentials.UserName.UserName = Role;
            factory.Credentials.UserName.Password = RolePassword;
            return factory;
        }

        private GetRequest CreateGetRequest(Uri urn, string selectorName, string selectorValue)
        {
            var selectorSet = CreateSelectorSet(selectorName, selectorValue);
            var resourceUri = CreateResourceUri(urn);
            var request = new GetRequest(resourceUri, Locale, selectorSet);
            return request;
        }

        private static ResourceURI CreateResourceUri(Uri urn)
        {
            var resourceUri = new ResourceURI()
            {
                Value = urn.ToString()
            };
            return resourceUri;
        }

        private static SelectorSet CreateSelectorSet(string selectorName, string selectorValue)
        {
            SelectorSet selectorSet = null;
            if (selectorName != null)
            {
                selectorSet = new SelectorSet();
                selectorSet.Selector = new Selector[] {
                    new Selector() {
                        Name = selectorName,
                        Text = new string[] {
                            selectorValue
                        }
                    }
                };
            }
            return selectorSet;
        }

        private static void CleanupChannelResources(ChannelFactory<Resource> factory, Resource resource)
        {
            var channel = (ICommunicationObject)resource;
            if (channel.State != CommunicationState.Faulted)
                channel.Close();
            if (factory.State != CommunicationState.Faulted)
                factory.Close();
        }

        private static void CleanupChannelResources(ChannelFactory<ResourceFactory> factory, ResourceFactory resource)
        {
            var channel = (ICommunicationObject)resource;
            if (channel.State != CommunicationState.Faulted)
                channel.Close();
            if (factory.State != CommunicationState.Faulted)
                factory.Close();
        }
        #endregion

    }
}

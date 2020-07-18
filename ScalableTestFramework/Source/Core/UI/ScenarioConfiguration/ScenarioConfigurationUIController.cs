using System;
using HP.ScalableTest.Core.EnterpriseTest;
using HP.ScalableTest.Core.EnterpriseTest.Configuration;

namespace HP.ScalableTest.Core.UI.ScenarioConfiguration
{
    /// <summary>
    /// Manages changes made to configuration objects via the UI and propagates those changes to the database and to subscribed UI elements.
    /// </summary>
    public sealed class ScenarioConfigurationUIController : IDisposable
    {
        private readonly EnterpriseTestConfigController _enterpriseTestController;

        /// <summary>
        /// Occurs when a change has been made to one or more configuration objects.
        /// </summary>
        public event EventHandler<ConfigurationChangeSetEventArgs> ConfigurationObjectsChanged;

        /// <summary>
        /// Occurs when a configuration object has been selected for editing.
        /// </summary>
        public event EventHandler<ConfigurationObjectEditEventArgs> ConfigurationObjectEditing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioConfigurationUIController" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="EnterpriseTestConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public ScenarioConfigurationUIController(EnterpriseTestConnectionString connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _enterpriseTestController = new EnterpriseTestConfigController(connectionString);
        }

        /// <summary>
        /// Loads all configuration objects from the database.
        /// </summary>
        public void Load()
        {
            ConfigurationObjectChangeSet loadedObjects = _enterpriseTestController.LoadConfigurationObjects();
            OnConfigurationObjectsChanged(loadedObjects);
        }

        /// <summary>
        /// Signals the UI to begin editing the specified configuration object.
        /// </summary>
        /// <param name="configurationObject">The <see cref="ConfigurationObjectTag" /> for the object to be edited.</param>
        /// <exception cref="ArgumentNullException"><paramref name="configurationObject" /> is null.</exception>
        public void BeginEditObject(ConfigurationObjectTag configurationObject)
        {
            if (configurationObject == null)
            {
                throw new ArgumentNullException(nameof(configurationObject));
            }

            EnterpriseTestContext editContext = _enterpriseTestController.GetEditContext();
            ConfigurationObjectEditing?.Invoke(this, new ConfigurationObjectEditEventArgs(configurationObject, editContext));
        }

        /// <summary>
        /// Saves all pending changes to configuration objects.
        /// </summary>
        public void Save()
        {
            _enterpriseTestController.CommitChanges();
        }

        private void OnConfigurationObjectsChanged(ConfigurationObjectChangeSet changeSet)
        {
            ConfigurationChangeSetEventArgs eventArgs = new ConfigurationChangeSetEventArgs(changeSet);
            ConfigurationObjectsChanged?.Invoke(this, eventArgs);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _enterpriseTestController.Dispose();
        }

        #endregion
    }
}

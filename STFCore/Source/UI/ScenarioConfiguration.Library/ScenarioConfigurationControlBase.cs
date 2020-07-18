using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Defines basic interface for a control used to edit a scenario configuration object.
    /// This class should not be instantiated directly.
    /// </summary>
    /// <remarks>
    /// This class cannot be marked abstract or the designer will not work with derived classes.
    /// </remarks>
    public class ScenarioConfigurationControlBase : FieldValidatedControl, IScenarioConfigurationControl
    {
        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public virtual EntityObject EntityObject
        {
            get { return null; }
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        public virtual void Initialize()
        {
            // By default, do nothing
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object based on the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public virtual void Initialize(ConfigurationObjectTag tag)
        {
            // By default, use the simple initializer
            Initialize();
        }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        public virtual string EditFormTitle
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Initializes this instance with the specified object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        /// </exception>
        public virtual void Initialize(object entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        public virtual void FinalizeEdit()
        {
            // By default, do nothing
        }

        /// <summary>
        /// Validates this control.
        /// </summary>
        /// <returns>A <see cref="ValidationResult" /> representing the outcome of validation.</returns>
        public virtual new ValidationResult Validate()
        {
            if (base.ValidateChildren())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(false, ErrorList);
            }
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public EnterpriseTestContext Context { get; set; }

        protected IEnumerable<MetadataType> GetMetadataTypes(VirtualResourceType resourceType)
        {
            string stringType = resourceType.ToString();

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var resourceItem = context.ResourceTypes.Where(e => e.Name.Equals(stringType)).FirstOrDefault();
                if (resourceItem != null)
                {
                    foreach (var item in resourceItem.MetadataTypes)
                    {
                        yield return item;
                    }
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}

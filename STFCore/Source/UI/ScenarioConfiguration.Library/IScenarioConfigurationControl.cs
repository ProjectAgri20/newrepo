using System.Data.Objects.DataClasses;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Defines the interface for a control used to edit a scenario configuration object.
    /// </summary>
    public interface IScenarioConfigurationControl
    {
        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        EntityObject EntityObject { get; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        EnterpriseTestContext Context { get; set; }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        string EditFormTitle { get; }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Initializes this instance for configuration of a new object based on the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        void Initialize(ConfigurationObjectTag tag);

        /// <summary>
        /// Initializes this instance with the specified object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        /// </exception>
        void Initialize(object entity);

        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        void FinalizeEdit();

        /// <summary>
        /// Validates this control.
        /// </summary>
        /// <returns>A <see cref="ValidationResult" /> representing the outcome of validation.</returns>
        ValidationResult Validate();
    }
}

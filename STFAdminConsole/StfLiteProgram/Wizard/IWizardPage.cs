
namespace HP.SolutionTest.WorkBench
{
    /// <summary>
    /// Interface for pages in the <see cref="SessionConfigurationWizard"/>.
    /// </summary>
    internal interface IWizardPage
    {
        /// <summary>
        /// Initializes the wizard page with the specified <see cref="WizardConfiguration" />.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>True if the page needs to be shown; false otherwise.</returns>
        bool Initialize(WizardConfiguration configuration);

        /// <summary>
        /// Performs final validation before allowing the user to navigate away from the page.
        /// </summary>
        /// <returns>True if this page was successfully validated.</returns>
        bool Complete();
    }
}
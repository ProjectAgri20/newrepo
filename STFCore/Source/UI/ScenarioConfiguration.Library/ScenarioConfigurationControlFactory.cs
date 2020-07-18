using System;
using System.Reflection;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Factory class for creating scenario configuration controls.
    /// </summary>
    public static class ScenarioConfigurationControlFactory
    {
        public static IScenarioConfigurationControl Create(ConfigurationObjectTag tag, EnterpriseTestContext context)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            IScenarioConfigurationControl control = null;

            switch (tag.ObjectType)
            {
                case ConfigurationObjectType.EnterpriseScenario:
                    control = new EnterpriseScenarioControl();
                    break;
                case ConfigurationObjectType.VirtualResource:
                    switch (tag.ResourceType)
                    {
                        case VirtualResourceType.OfficeWorker:
                            control = new OfficeWorkerControl();
                            break;

                        case VirtualResourceType.CitrixWorker:
                            control = new CitrixWorkerControl();
                            break;

                        case VirtualResourceType.AdminWorker:
                            control = new AdminWorkerControl();
                            break;

                        case VirtualResourceType.EventLogCollector:
                            control = new EventLogCollectorControl();
                            break;

                        case VirtualResourceType.PerfMonCollector:
                            control = new PerfMonCollectorControl();
                            break;

                        case VirtualResourceType.MachineReservation:
                            control = new MachineReservationControl();
                            break;

                        case VirtualResourceType.SolutionTester:
                            control = new SolutionTesterControl();
                            break;

                        case VirtualResourceType.LoadTester:
                            control = new LoadTesterControl();
                            break;
                    }
                    break;

                case ConfigurationObjectType.ResourceMetadata:
                    switch (tag.ResourceType)
                    {
                        case VirtualResourceType.OfficeWorker:
                        case VirtualResourceType.CitrixWorker:
                        case VirtualResourceType.SolutionTester:
                        case VirtualResourceType.AdminWorker:
                        case VirtualResourceType.LoadTester:
                            control = new WorkerActivityMetadataControl();
                            break;
                    }
                    break;
            }

            if (control != null)
            {
                control.Context = context;
            }

            return control;
        }

        /// <summary>
        /// Creates a scenario configuration control for the specified <see cref="ConfigurationObjectTag"/>.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static IScenarioConfigurationControl Create(ConfigurationObjectTag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            switch (tag.ObjectType)
            {
                case ConfigurationObjectType.EnterpriseScenario:
                    return new EnterpriseScenarioControl();

                case ConfigurationObjectType.VirtualResource:
                    switch (tag.ResourceType)
                    {
                        case VirtualResourceType.OfficeWorker:
                            return new OfficeWorkerControl();

                        case VirtualResourceType.CitrixWorker:
                            return new CitrixWorkerControl();

                        case VirtualResourceType.AdminWorker:
                            return new AdminWorkerControl();

                        case VirtualResourceType.EventLogCollector:
                            return new EventLogCollectorControl();

                        case VirtualResourceType.PerfMonCollector:
                            return new PerfMonCollectorControl();

                        case VirtualResourceType.MachineReservation:
                            return new MachineReservationControl();

                        case VirtualResourceType.SolutionTester:
                            return new SolutionTesterControl();

                        case VirtualResourceType.LoadTester:
                            return new LoadTesterControl();
                    }
                    break;

                case ConfigurationObjectType.ResourceMetadata:
                    switch (tag.ResourceType)
                    {
                        case VirtualResourceType.OfficeWorker:
                        case VirtualResourceType.CitrixWorker:
                        case VirtualResourceType.AdminWorker:
                            return new WorkerActivityMetadataControl();
                    }
                    break;
            }

            // We didn't find a control for this tag - return null
            return null;
        }
    }
}

using System;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Enumeration describing different Resource Types
    /// </summary>
    public enum VirtualResourceType
    {
        /// <summary>
        /// No virtual resource type
        /// </summary>
        Unknown,

        /// <summary>
        /// Admin Worker
        /// </summary>
        AdminWorker,

        /// <summary>
        /// Citrix Worker
        /// </summary>
        CitrixWorker,

        /// <summary>
        /// EventLogCollector
        /// </summary>
        EventLogCollector,

        /// <summary>
        /// Load Tester
        /// </summary>
        LoadTester,

        /// <summary>
        /// Machine Reservation
        /// </summary>
        MachineReservation,

        /// <summary>
        /// Office Worker
        /// </summary>
        OfficeWorker,

        /// <summary>
        /// PerfMonCollector
        /// </summary>
        PerfMonCollector,

        /// <summary>
        /// SolutionTester
        /// </summary>
        SolutionTester,

        /// <summary>
        /// VirtualResource
        /// </summary>
        VirtualResource,
    }

    /// <summary>
    /// Details which resources are continuous (run indefinitely) such as PerfMon and EventLog Collectors
    /// </summary>
    public static class ContinuousResources
    {
        /// <summary>
        /// Determines if the given resource type is continuous
        /// </summary>
        /// <param name="thisType">The resource type</param>
        /// <returns>A boolean indicating if the type is continuous</returns>
        public static bool IsContinuous(this VirtualResourceType thisType)
        {
            switch (thisType)
            {
                case VirtualResourceType.EventLogCollector:
                    return true;
                case VirtualResourceType.PerfMonCollector:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the given resource type is not continuous
        /// </summary>
        /// <param name="thisType">The resource type</param>
        /// <returns>A boolean indicating if the type is not continuous</returns>
        public static bool IsNotContinuous(this VirtualResourceType thisType)
        {
            switch (thisType)
            {
                case VirtualResourceType.EventLogCollector:
                    return false;
                case VirtualResourceType.PerfMonCollector:
                    return false;
                default:
                    return true;
            }
        }
    }

    /// <summary>
    /// Helper class for VirtualResource Type
    /// </summary>
    public static class VirtualResourceTypeHelper
    {
        /// <summary>
        /// Gets the Detail Type
        /// </summary>
        /// <param name="thisType"></param>
        /// <returns></returns>
        public static Type DetailType(this VirtualResourceType thisType)
        {
            string typeName = "HP.ScalableTest.Framework.Manifest.{0}Detail".FormatWith(thisType);
            return Type.GetType(typeName);
        } 

        /// <summary>
        /// Checks if the Virtual Resource is an Admin Worker
        /// </summary>
        /// <param name="thisType"></param>
        /// <returns></returns>
        public static bool IsAdminWorker(this VirtualResourceType thisType)
        {
            return thisType == VirtualResourceType.AdminWorker;
        }

        /// <summary>
        /// Checks if the Virtual Resource is an Office Worker
        /// </summary>
        /// <param name="thisType"></param>
        /// <returns></returns>
        public static bool IsOfficeWorker(this VirtualResourceType thisType)
        {
            return thisType == VirtualResourceType.OfficeWorker;
        }

        /// <summary>
        /// Checks if the Virtual Resource is a Citrix Worker
        /// </summary>
        /// <param name="thisType"></param>
        /// <returns></returns>
        public static bool IsCitrixWorker(this VirtualResourceType thisType)
        {
            return thisType == VirtualResourceType.CitrixWorker;
        }

        /// <summary>
        /// Checks if the Virtual Resource is an Unknown Worker
        /// </summary>
        /// <param name="thisType"></param>
        /// <returns></returns>
        public static bool IsUnknown(this VirtualResourceType thisType)
        {
            return thisType == VirtualResourceType.Unknown;
        }

        /// <summary>
        /// Compares the given Virtual Resource type to a known type
        /// </summary>
        /// <param name="thisType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Is(this VirtualResourceType thisType, VirtualResourceType type)
        {
            return thisType == type;
        }
    }

    

}

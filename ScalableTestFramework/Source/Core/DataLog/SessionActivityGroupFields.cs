using System;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Properties of <see cref="SessionActivityData" /> that can be used in a composite <see cref="SessionActivityGroupKey" />.
    /// </summary>
    [Flags]
    public enum SessionActivityGroupFields
    {
        /// <summary>
        /// No grouping.
        /// </summary>
        None = 0,

        /// <summary>
        /// Group by <see cref="SessionActivityData.ActivityName" />.
        /// </summary>
        ActivityName = 0x1,

        /// <summary>
        /// Group by <see cref="SessionActivityData.ActivityType" />.
        /// </summary>
        ActivityType = 0x2,

        /// <summary>
        /// Group by <see cref="SessionActivityData.UserName" />.
        /// </summary>
        UserName = 0x4,

        /// <summary>
        /// Group by <see cref="SessionActivityData.HostName" />.
        /// </summary>
        HostName = 0x8,

        /// <summary>
        /// Group by <see cref="SessionActivityData.StartDateTime" /> (to the nearest minute).
        /// </summary>
        StartDateTime = 0x10,

        /// <summary>
        /// Group by <see cref="SessionActivityData.EndDateTime" /> (to the nearest minute).
        /// </summary>
        EndDateTime = 0x20,

        /// <summary>
        /// Group by <see cref="SessionActivityData.Status" />.
        /// </summary>
        Status = 0x40,

        /// <summary>
        /// Group by <see cref="SessionActivityData.ResultMessage" />.
        /// </summary>
        ResultMessage = 0x80,

        /// <summary>
        /// Group by <see cref="SessionActivityData.ResultCategory" />.
        /// </summary>
        ResultCategory = 0x100
    }
}

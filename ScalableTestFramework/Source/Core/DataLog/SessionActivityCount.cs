using System;
using System.Linq;
using System.Linq.Expressions;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// A count of activities grouped according to a <see cref="SessionActivityGroupKey" />.
    /// </summary>
    public sealed class SessionActivityCount
    {
        /// <summary>
        /// Gets the <see cref="SessionActivityGroupKey" /> used to group activities.
        /// </summary>
        public SessionActivityGroupKey Key { get; private set; }

        /// <summary>
        /// Gets the number of activities matching the specified <see cref="SessionActivityGroupKey" />.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a LINQ-to-entities compatible expression to generate a <see cref="SessionActivityCount" /> object from a grouping operation.
        /// </summary>
        internal static Expression<Func<IGrouping<SessionActivityGroupKey, SessionActivityData>, SessionActivityCount>> BuildFromGrouping
        {
            get
            {
                return n => new SessionActivityCount
                {
                    Key = n.Key,
                    Count = n.Count()
                };
            }
        }
    }
}

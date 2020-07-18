using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    /// <summary>
    /// Utility class that contains methods for working with entity framework objects.
    /// </summary>
    public static class EntityUtil
    {
        /// <summary>
        /// Checks to see if the specified <see cref="EntityObject"/> has modified properties.
        /// If all properties have the same values, set the state to Unchanged.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        public static void CheckIfModified(this ObjectContext context, EntityObject entity)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (entity.EntityState == EntityState.Modified)
            {
                ObjectStateEntry state = context.ObjectStateManager.GetObjectStateEntry(entity);
                DbDataRecord originalValues = state.OriginalValues;
                CurrentValueRecord currentValues = state.CurrentValues;

                for (int i = 0; i < originalValues.FieldCount; i++)
                {
                    object original = originalValues.GetValue(i);
                    object current = currentValues.GetValue(i);
                    if (!original.Equals(current))// && (!(original is byte[]) || !((byte[])original).SequenceEqual((byte[])current)))
                    {
                        // Something has actually changed.  We can return now.
                        return;
                    }
                }

                // We made it through the loop without finding any changed properties
                state.ChangeState(EntityState.Unchanged);
            }
        }

        /// <summary>
        /// Gets all <see cref="EntityObject"/>s in the specified state.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="state">The state.</param>
        public static IEnumerable<EntityObject> GetObjectsInState(this ObjectContext context, EntityState state)
        {
            IEnumerable<ObjectStateEntry> entries = context.ObjectStateManager.GetObjectStateEntries(state);
            foreach (var entry in entries)
            {
                // It is possible for entities returned from GetObjectStateEntries() to be in a different state than 
                // when we retrieved them. For instance, if the first item is a parent of the second item, and
                // the calling code changes the first item's entity state to deleted, the second item's state will change also.
                // For this reason, double-check the state of each object before we proceed.
                if ((entry.State & state) > 0)
                {
                    yield return entry.Entity as EntityObject;
                }
            }
        }

        /// <summary>
        /// Determines whether this instance has objects in the specified state.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <c>true</c> if this instance has objects in the specified state; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasObjectsInState(this ObjectContext context, EntityState state)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context.ObjectStateManager.GetObjectStateEntries(state).Count() != 0 ;
        }
    }
}

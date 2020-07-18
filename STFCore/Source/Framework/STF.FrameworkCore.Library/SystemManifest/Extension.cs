using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;

namespace HP.ScalableTest.Framework.Dispatcher
{
    internal static class Extension
    {
        /// <summary>
        /// Eagerly load properties and then clone the entity object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        public static T CloneWithEagerLoad<T>(this T source)
            where T : EntityObject
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            return (T)(source.LoadAllChildProperties().Clone());
        }

        /// <summary>
        /// Loads all child properties of the entity object.
        /// Refer to http://www.codeproject.com/Articles/137853/Cloning-the-Entity-object-and-all-related-children
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        public static T LoadAllChildProperties<T>(this T source)
            where T : EntityObject
        {
            if (source != null)
            {
                List<PropertyInfo> PropList = (from a in source.GetType().GetProperties()
                                               where a.PropertyType.Name == "EntityCollection`1"
                                               select a).ToList(); 
                foreach (PropertyInfo prop in PropList)
                {
                    object instance = prop.GetValue(source, null);
                    bool isLoad =
                        (bool)instance.GetType().GetProperty("IsLoaded").GetValue(instance, null);
                    if (!isLoad)
                    {
                        MethodInfo mi = (from a in instance.GetType().GetMethods()
                                         where a.Name == "Load" && a.GetParameters().Length == 0
                                         select a).FirstOrDefault();

                        mi.Invoke(instance, null);
                    }
                }
            }
            return (T)source;
        }
    }
}

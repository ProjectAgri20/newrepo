using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public static class Extensions
    {
        /// <summary>
        /// Performs an action on a control, marshaling to the creating thread if necessary.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="control" />.</typeparam>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
        public static void SmartInvoke<T>(this T control, InvokeMethodology invokeMethodology, Action<T> action) where T : ISynchronizeInvoke
        {
            if (control.InvokeRequired)
            {
                switch (invokeMethodology)
                {
                    case InvokeMethodology.Invoke:
                        control.Invoke(action, new object[] { control });
                        break;
                    case InvokeMethodology.BeginInvoke:
                        control.BeginInvoke(action, new object[] { control });
                        break;
                    default:
                        throw new NotSupportedException($"{invokeMethodology.GetType().Name} '{invokeMethodology}' not supported.");
                }
            }
            else
            {
                action(control);
            }
        }

        public static string GetAttributeValue<THavingValuePropertyOfTypeString>(this Enum thing)
        {
            string returnValue = thing.GetAttributeProperty<THavingValuePropertyOfTypeString, string>("Value");

            if (null != returnValue)
            {
                return returnValue;
            }

            return thing.ToString();
        }

        public static TReturnType GetAttributeProperty<TAttribute, TReturnType>(this Enum thing, string propertyName)
        {
            // Get fieldinfo for this type
            FieldInfo fieldInfo = thing.GetType().GetField(thing.ToString()); /// only works with enums since the type of the enum and the name of the field are derived from the same object.
            if (object.ReferenceEquals(fieldInfo, null))
            {
                return default(TReturnType);
            }

            // Get the stringvalue attributes
            TAttribute[] attribs = fieldInfo.GetCustomAttributes(typeof(TAttribute), false).OfType<TAttribute>().ToArray();

            if (attribs.Any())
            {
                // Return the first if there was a match.
                var propertyInfo = attribs[0].GetType().GetProperty(propertyName);

                if (null == propertyInfo)
                {
                    return default(TReturnType);
                }

                var propertyValue = propertyInfo.GetValue(attribs[0], null);

                if (propertyValue == null)
                {
                    bool isNullable = Nullable.GetUnderlyingType(typeof(TReturnType)) != null;
                    if (!isNullable)
                    {
                        return default(TReturnType);
                    }
                }

                return (TReturnType)propertyValue;
            }

            return default(TReturnType);
        }
    }

    public enum InvokeMethodology
    {
        /// <summary>
        /// Default
        /// </summary>
        Invoke,
        /// <summary>
        /// Use this when the results of the call are not needed before proceeding and the thread that will handle the call may be very slow or blocked.
        /// </summary>
        BeginInvoke,
    }
}
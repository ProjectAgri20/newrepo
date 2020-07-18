using System;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Used on any collection property in the overall session map to 
    /// indicate this property is a session map collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SessionMapElementCollectionAttribute : Attribute
    {
    }
}
using System;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    ///  Class level attribute that defines the name of the Dispatcher "Host" class
    ///  used to create the defined asset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AssetHostAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the corresponding AssetHost class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetHostAttribute" /> class.
        /// </summary>
        /// <param name="className">Name of the AssetHost class in dispatcher that performs initialization of this asset.</param>
        public AssetHostAttribute(string className)
        {
            ClassName = className;
        }
    }
}
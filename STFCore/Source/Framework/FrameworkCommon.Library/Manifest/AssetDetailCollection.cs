using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// A collection of <see cref="AssetDetail"/> objects.
    /// </summary>
    public class AssetDetailCollection : Collection<AssetDetail>
    {
        public void Replace(IEnumerable<AssetDetail> replacement)
        {
            Clear();
            foreach (AssetDetail item in replacement)
            {
                Add(item);
            }
        }
    }
}

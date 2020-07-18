using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    /// <summary>
    /// Class that defines the combination of an associated product and a product version relationship
    /// </summary>
    public class ScenarioProduct
    {
        public Guid ProductId { get; set; }
        public Guid ScenarioId { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string Version { get; set; }

        public bool Active { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScenarioProduct"/>
        /// </summary>
        public ScenarioProduct()
        {
            ProductId = SequentialGuid.NewGuid();
            ScenarioId = SequentialGuid.NewGuid();
            Name = string.Empty;
            Vendor = string.Empty;
            Version = string.Empty;
            Active = false;
        }



        public void Update(EnterpriseTestContext context)
        {
            var temp = context.AssociatedProductVersions.Where(x => x.AssociatedProductId == ProductId && x.EnterpriseScenarioId == ScenarioId);
            if (temp == null || temp.Count() == 0)
            {
                AssociatedProductVersion version = new AssociatedProductVersion();
                version.AssociatedProductId = ProductId;
                version.EnterpriseScenarioId = ScenarioId;
                version.Active = Active;
                version.Version = Version;

                context.AssociatedProductVersions.AddObject(version);
                context.SaveChanges();
            }
            else
            {
                AssociatedProductVersion version = context.AssociatedProductVersions.Where(x => x.AssociatedProductId == ProductId && x.EnterpriseScenarioId == ScenarioId).First();
                version.AssociatedProductId = ProductId;
                version.EnterpriseScenarioId = ScenarioId;
                version.Active = Active;
                version.Version = Version;

                context.SaveChanges();
            }
        }

    }

    public class ScenarioProductEqualityComparer : IEqualityComparer<ScenarioProduct>
    {
        public bool Equals(ScenarioProduct x, ScenarioProduct y)
        {
            // Two items are equal if their keys are equal.
            return x.Name == y.Name &&
                x.ProductId == y.ProductId &&
                x.ScenarioId == y.ScenarioId;

        }

        public int GetHashCode(ScenarioProduct obj)
        {
            return obj.Name.GetHashCode() ^
            obj.ProductId.GetHashCode() ^
            obj.ScenarioId.GetHashCode();
        }
    }
}

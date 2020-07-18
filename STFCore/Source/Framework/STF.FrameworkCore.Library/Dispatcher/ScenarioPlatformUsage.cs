using System;
using System.Linq;

namespace HP.ScalableTest.Framework.Dispatcher
{
    public class ScenarioPlatformUsage
    {
        public string PlatformId { get; set; }
        public string Description { get; set; }
        public int RequiredCount { get; set; }
        public int AuthorizedCount { get; set; }
    }
}

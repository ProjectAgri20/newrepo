using System;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public class UsageResolverForm : Form
    {
        protected UsageResolverForm()
        { }

        public UsageResolverForm(UsageResolverData data)
        {
            Data = data;
        }

        protected UsageResolverData Data { get; private set; }
    }
}

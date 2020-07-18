using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Data.AssetInventory;
using HP.ScalableTest.Data.AssetInventory.Model;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.SessionExecution.Wizard;

namespace HP.ScalableTest.STFU
{
    /// <summary>
    /// Interaction logic for ScenarioSelection.xaml
    /// </summary>
    public partial class ScenarioSelection : Window
    {
        public Guid ScenarioId { get; private set; }
        public string ScenarioName { get; private set; }
        public ScenarioSelection()
        {
            InitializeComponent();
        }

        private void ScenarioSelection_Loaded(object sender, RoutedEventArgs e)
        {
           
            EnterpriseTestContext context = new EnterpriseTestContext();
            {
                var folders = ConfigurationTreeFolder.Select(context, "ScenarioFolder").Where( x => x.ParentId == null);
                ScenarioModel model = new ScenarioModel(folders.ToArray());

                ScenarioTree.DataContext = model;
            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            ScenarioViewModel selectedScenario = ScenarioTree.SelectedItem as ScenarioViewModel;
            if (selectedScenario != null)
            {
                ScenarioId = selectedScenario.ScenarioId;
                ScenarioName = selectedScenario.ScenarioName;
                this.DialogResult = true;
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       

       
    }
}

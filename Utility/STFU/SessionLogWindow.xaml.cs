using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HP.ScalableTest
{
    /// <summary>
    /// Interaction logic for SessionLogWindow.xaml
    /// </summary>
    public partial class SessionLogWindow : Window
    {
        public string DebugLog { get; set; }
        public SessionLogWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sessionLog.AppendText(DebugLog);

        }
    }
}

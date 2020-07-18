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
using System.Collections.ObjectModel;

namespace HP.ScalableTest
{
    /// <summary>
    /// Interaction logic for MailingListWindow.xaml
    /// </summary>
    public partial class MailingListWindow : Window
    {
        ObservableCollection<string> _mailingList = new ObservableCollection<string>();
        public MailingListWindow()
        {
            InitializeComponent();
            _mailingList = STFU.STFUWindow.MailingList;
            
            mailingListBox.DataContext = _mailingList;
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e)
        {
            STFU.STFUWindow.MailingList = _mailingList;
            this.Close();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(mailAddressTextBox.Text))
            {
                _mailingList.Add(mailAddressTextBox.Text);
            }

        }
    }
}

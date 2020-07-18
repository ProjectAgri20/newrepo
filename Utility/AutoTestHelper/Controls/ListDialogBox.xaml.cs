using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HP.RDL.EDT.AutoTestHelper.Controls
{
    /// <summary>
    /// Interaction logic for ListDialogBox.xaml
    /// </summary>
    public partial class ListDialogBox : INotifyPropertyChanged
    {
        private string _prompt = "Select an item from the list.";
        private string _selectText = "Select";
        private ObservableCollection<object> _items;
        private object _selectedItem = null;

        public ListDialogBox()
        {
            InitializeComponent();
           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ListDialogBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        public string Prompt
        {
            get { return _prompt; }
            set
            {
                if (_prompt == value) return;
                _prompt = value;
                RaisePropertyChanged("Prompt");
            }
        }

        public string SelectText
        {
            get { return _selectText; }
            set
            {
                if (_selectText == value) return;
                _selectText = value;
                RaisePropertyChanged("SelectText");
            }
        }

        public ObservableCollection<Object> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        public object SelectedItem
        {
            get { return _selectedItem; }
            private set { _selectedItem = value; }
        }

        public bool IsCancelled
        {   //A simple null-check is performed (the caller can do this too).
            get { return (SelectedItem == null); }
        }

        public void SetDropDownList()
        {
            foreach (var item in Items)
            {
                ListComboBox.Items.Add(item);
            }
        }

        public void SetPrompt()
        {
            PromptTextBlock.Text = Prompt;
        }

        private void ListComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = ListComboBox.SelectedItem;
        }

        private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedItem = ListComboBox.SelectedItem;
            this.DialogResult = true;
            Close();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void ListDialogBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            ListComboBox.ItemsSource = _items;
            PromptTextBlock.Text = _prompt;
            if (_items.Count > 0)
                ListComboBox.SelectedIndex = 0;
        }
    }
}
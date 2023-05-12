using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ZdravoCorp;

namespace ZdravoCorp.ViewModels.ManagerWindows
{
    
    public partial class InventoryListWindow : Window
    {
        public ObservableCollection<InventoryItem> FilteredItems;
        public ObservableCollection<InventoryItem> OriginalItems;
        public InventoryListWindow( )
        {
            InitializeComponent();
            DataContext = this;

            FilteredItems = new ObservableCollection<InventoryItem>(InventoryRepository.Instance.Items);
            InventoryList.ItemsSource = FilteredItems;
            OriginalItems = FilteredItems;
        }
        private void SearchTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBox? textBox  = sender as TextBox;
            if (textBox != null) 
            { 
                if(!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    string textBoxText = textBox.Text.ToLower();
                    List<InventoryItem> itemsSource = InventoryList.ItemsSource.Cast<InventoryItem>().ToList();
                    
                    FilteredItems = new ObservableCollection<InventoryItem>(
                       itemsSource.Where(item =>
                            item.Equipment.Name.ToLower().Contains(textBoxText) ||
                            item.Equipment.TypeTranslation.ToLower().Contains(textBoxText) ||
                            item.Room.Id.ToString().ToLower().Contains(textBoxText) ||
                            item.Room.Name.ToLower().Contains(textBoxText) ||
                            item.Quantity.ToString().ToLower().Contains(textBoxText))
                        .ToList());

                    InventoryList.ItemsSource = FilteredItems;

                }else
                {
                 InventoryList.ItemsSource = OriginalItems;
                }
            }
        }

        public void CleareFilterParameters_click(object sender, RoutedEventArgs e)
        {
            InventoryList.ItemsSource = InventoryRepository.Instance.Items;
            OriginalItems = new ObservableCollection<InventoryItem>(InventoryRepository.Instance.Items);
        }
        public void FilterInventoryItems(object sender, RoutedEventArgs e)
        {
            FilterWindow filterWindow = new FilterWindow(InventoryList);
            filterWindow.ShowDialog();
            OriginalItems = new ObservableCollection<InventoryItem>(InventoryList.ItemsSource.Cast<InventoryItem>());
        }
    }
}

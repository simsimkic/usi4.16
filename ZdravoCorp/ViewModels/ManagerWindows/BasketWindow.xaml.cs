using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoCorp.Classes.Users;

namespace ZdravoCorp.ViewModels.ManagerWindows
{
    /// <summary>
    /// Interaction logic for BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        public OrderRequest OrderRequest { get; private set; }
        public LowEquipmentWindow LowEquipmentWindow { get; private set; }
        public BasketWindow(OrderRequest orderRequest, LowEquipmentWindow lowEquipmentWindow)
        {
            InitializeComponent();
            DataContext = this;
            OrderRequest= orderRequest;
            BasketList.ItemsSource = new ObservableCollection<Order>(orderRequest.Orders);
            LowEquipmentWindow= lowEquipmentWindow;
        }

        public void DeleteFromBasket_Click(object sender, RoutedEventArgs e)
        {
            Order? order = BasketList.SelectedItem as Order;
            if (order == null)
            {
                MessageBox.Show("Morate izabrati opremu", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OrderRequest.Orders.Remove(order);
            BasketList.ItemsSource = new ObservableCollection<Order>(OrderRequest.Orders);
            MessageBox.Show("Uspesno obrisano iz korpe!");
        }

        public void SendOrder_Click(object sender, RoutedEventArgs e)
        {
            
            if(OrderRequest.Orders.Count == 0) 
            {
                MessageBox.Show("Korpa je prazna", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            OrderRequest orderRequest = new OrderRequest();
            orderRequest.Orders= OrderRequest.Orders;
            orderRequest.Send();
            MessageBox.Show("Uspesno ste porucili opremu!");
            this.Close();
            LowEquipmentWindow.Close();
        }
    }
}

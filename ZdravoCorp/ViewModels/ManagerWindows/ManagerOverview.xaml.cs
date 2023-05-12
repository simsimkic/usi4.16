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
namespace ZdravoCorp.ViewModels.ManagerWindows
{
    /// <summary>
    /// Interaction logic for ManagerOverview.xaml
    /// </summary>
    public partial class ManagerOverview : Window
    {
        
        public ManagerOverview()
        {
            InitializeComponent();
        }

        private void ViewInventory_Click(object sender, RoutedEventArgs e)
        {
            InventoryListWindow inventoryListWindow = new InventoryListWindow();

            this.Hide();
            // Show the inventory list window
            inventoryListWindow.ShowDialog();

            this.ShowDialog();
        }

        private void OrderEquipment_Click(object sender, RoutedEventArgs e)
        {
            LowEquipmentWindow lowEquipment = new LowEquipmentWindow();
            this.Hide();
            lowEquipment.ShowDialog();
            this.ShowDialog();
        }

        private void MoveEquipment_Click(Object sender, RoutedEventArgs e)
        {
            MoveEquipmentWindow moveEquipmentWindow = new MoveEquipmentWindow();
            this.Hide();
            moveEquipmentWindow.ShowDialog();
            this.ShowDialog();
        }


    }
}

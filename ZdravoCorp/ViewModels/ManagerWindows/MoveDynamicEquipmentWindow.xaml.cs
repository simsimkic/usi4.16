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
using ZdravoCorp.Classes.Users;

namespace ZdravoCorp.ViewModels.ManagerWindows
{
    /// <summary>
    /// Interaction logic for MoveDynamicEquipmentWindow.xaml
    /// </summary>
    public partial class MoveDynamicEquipmentWindow : Window
    {
        public InventoryItem SelectedItem { get; private set; }
        public Room FromRoom { get; private set; }
        public Room ToRoom { get; private set; }
        public MoveDynamicEquipmentWindow(InventoryItem selectedItem, Room fromRoom, Room toRoom)
        {
            InitializeComponent();
            SelectedItem = selectedItem;
            FromRoom = fromRoom;
            ToRoom = toRoom;
        }

        public void Move_Click(object sender, EventArgs e)
        {
            string input = QuantityTextBox.Text;
            int quantity;
            try
            {
                quantity = Convert.ToInt32(input);
                if (quantity <= 0)
                {
                    MessageBox.Show("Greska: Neispravan unos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (quantity > SelectedItem.Quantity)
                {
                    MessageBox.Show("Greska: Uneli ste vecu kolicinu!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Transfer transfer = new Transfer(FromRoom, ToRoom, quantity, DateTime.Now, SelectedItem.Equipment, true);
                bool isDone = TransferRepository.Instance.AddTransfer(transfer);
                if (!isDone)
                {
                    MessageBox.Show("Greska: Vec postoji takav zakazan transfer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Uspesno ste izvrsili premestaj");
                this.Close();
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                MessageBox.Show("Greska: Neispravan unos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}

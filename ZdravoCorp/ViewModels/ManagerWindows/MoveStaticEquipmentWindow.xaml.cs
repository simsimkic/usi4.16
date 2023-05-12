using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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
    /// Interaction logic for MoveStaticEquipmentWindow.xaml
    /// </summary>
    public partial class MoveStaticEquipmentWindow : Window
    {
        public InventoryItem SelectedItem { get; private set; }
        public Room FromRoom { get; private set; }
        public Room ToRoom { get;private set; }
        public MoveStaticEquipmentWindow(InventoryItem selectedItem, Room fromRoom, Room toRoom)
        {
            InitializeComponent();
            SelectedItem = selectedItem;
            FromRoom = fromRoom;
            ToRoom = toRoom;
        }

        public void Move_Click(object sender, EventArgs e)
        {
            int[] moveTime = ExtractTime(MoveTime);
            if (moveTime.Length == 0) return;
            if (QuantityTextBox.Text == null || MoveDate.SelectedDate == null)
            {
                MessageBox.Show("Greska", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime moveDate = ExtractDate(moveTime, MoveDate.SelectedDate.Value.Date);
            if (moveDate <= DateTime.Now)
            {
                MessageBox.Show("Ne mozete da zakazete premestaj u prošlosti!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string input = QuantityTextBox.Text;
            int quantity = 0;
            try
            {
                quantity = Convert.ToInt32(input);
                if (quantity <= 0)
                {
                    MessageBox.Show("Greska: Neispravan unos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }else if(quantity > SelectedItem.Quantity)
                {
                    MessageBox.Show("Greska: Uneli ste vecu kolicinu!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Transfer transfer = new Transfer(FromRoom, ToRoom, quantity, moveDate,SelectedItem.Equipment ,false);
                bool isDone = TransferRepository.Instance.AddTransfer(transfer);
                if (!isDone) 
                {
                    MessageBox.Show("Greska: Vec postoji takav zakazan transfer", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Uspesno ste zakazali premestaj");
                this.Close();
            }
            catch(Exception ex)
            {
                var message = ex.ToString();
                MessageBox.Show("Greska: Neispravan unos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

                
        }

        public static int[] ExtractTime(TextBox timeString)
        {
            try
            {
                string[] time = timeString.Text.Split(":");
                if (!int.TryParse(time[0], out int hours) ||
                    !int.TryParse(time[1], out int minutes))
                {
                    MessageBox.Show("Vreme mora biti u formatu hh:mm!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return Array.Empty<int>();
                }
                if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
                {
                    MessageBox.Show("Pogrešno vreme!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return Array.Empty<int>(); ;
                }
                return new int[] { hours, minutes };
            }
            catch (Exception) { return Array.Empty<int>(); }
        }

        public static DateTime ExtractDate(int[] time, DateTime date)
        {
            int hours = time[0];
            int minutes = time[1];
            date = new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);
            return date;
        }
    }
}

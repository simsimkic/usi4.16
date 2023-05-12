using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class DoctorsRoomEquipmentWindow : Window
    {

        public Room Room { get; private set; }
        private Dictionary<Equipment, int> EquipmentQuantity { get; set; } = new Dictionary<Equipment, int>();

        public DoctorsRoomEquipmentWindow(Room room)
        {
            InitializeComponent();
            Room = room;
            List<InventoryItem> inventoryItems =
                InventoryRepository.Instance.Items.Where(item => item.Room.Id == room.Id).ToList();
            Dictionary<string, bool> isDynamic = Equipment.GetEquipmentNames();
            foreach (InventoryItem item in inventoryItems)
            {
                if (!isDynamic[item.Equipment.Name]) continue;
                EquipmentListView.Items.Add(item);
                EquipmentComboBox.Items.Add(item.Equipment);
                EquipmentQuantity[item.Equipment] = item.Quantity;
            }
        }

        private void FormSubmitted(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void MoreUsedEquipment(object sender, RoutedEventArgs e)
        {
            Equipment equipment = (Equipment) EquipmentComboBox.SelectedItem;
            if (equipment == null)
            {
                MessageBox.Show("Morate izabrati koju opremu ste potrošili!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int originalQuantity = EquipmentQuantity[equipment];
            if (!int.TryParse(EquipmentUsedTextBox.Text, out int quantity))
            {
                MessageBox.Show("Morate uneti broj za količinu!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (quantity <= 0)
            {
                MessageBox.Show("Ne možete potrošiti " + quantity + " opreme!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (quantity > originalQuantity)
            {
                MessageBox.Show("Niste mogli potrošiti više opreme od onoliko koliko je prvobitno bilo u sobi!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            InventoryRepository.Instance.Remove(new InventoryItem(equipment, Room, quantity));

            EquipmentUsedTextBox.Clear();
            EquipmentComboBox.Items.Remove(equipment);
            EquipmentComboBox.SelectedIndex = -1;
        }

        private void SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (EquipmentListView.SelectedIndex != -1) EquipmentListView.SelectedIndex = -1;
        }
    }
}

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

namespace ZdravoCorp.ViewModels.ManagerWindows
{
    /// <summary>
    /// Interaction logic for MoveEquipmentWindow.xaml
    /// </summary>
    /// 
    public partial class MoveEquipmentWindow : Window
    {
        public class DisplayLowQuantityItem
        {
            public Room Room { get; private set; }
            public Equipment Equipment { get; private set; }
            public int Quantity { get; private set; }
            public DisplayLowQuantityItem(Room room, Equipment equipment, int quantity)
            {
                Room = room;
                Equipment = equipment;
                Quantity = quantity;
            }
        }

        public MoveEquipmentWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void FromRoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Room selectedFromRoom = (Room)fromRoomComboBox.SelectedItem;
            List<InventoryItem> inventoryItems = InventoryRepository.Instance.Items.Where(item => item.Room.Id == selectedFromRoom.Id).ToList();
            selectedFromRoomListView.ItemsSource = new ObservableCollection<InventoryItem>(inventoryItems);
        }

        private void ToRoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Room selectedToRoom = (Room)toRoomComboBox.SelectedItem;
            selectedToRoomListView.ItemsSource = GetDisplayLowEquipmentItems(selectedToRoom);
        }
        public void Move_Click(object sender, RoutedEventArgs e)
        {
            Room fromRoom = (Room)fromRoomComboBox.SelectedItem;
            Room toRoom = (Room)toRoomComboBox.SelectedItem;
            InventoryItem? selectedItem = selectedFromRoomListView.SelectedItem as InventoryItem;
            if (fromRoom == null || toRoom == null || fromRoom == toRoom || selectedItem == null)
            {
                MessageBox.Show("Greska", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Equipment.GetEquipmentNames()[selectedItem.Equipment.Name] == false)
            {
                MoveStaticEquipmentWindow moveStaticEquipmentWindow = new MoveStaticEquipmentWindow(selectedItem, fromRoom, toRoom);
                moveStaticEquipmentWindow.ShowDialog();
            }
            else
            {
                MoveDynamicEquipmentWindow moveDynamicEquipmentWindow = new MoveDynamicEquipmentWindow(selectedItem, fromRoom, toRoom);
                moveDynamicEquipmentWindow.ShowDialog();
                this.Close();
            }
        }

        public static ObservableCollection<DisplayLowQuantityItem> GetDisplayLowEquipmentItems(Room selectedRoom)
        {
            List<DisplayLowQuantityItem> items = new List<DisplayLowQuantityItem>();
            Dictionary<string, bool> equipmentNames = Equipment.GetEquipmentNames();
            foreach (Room room in RoomRepository.Instance.Rooms)
            {
                foreach (string equipmentName in equipmentNames.Keys)
                {
                    InventoryItem? matchingItem = InventoryRepository.Instance.Items.FirstOrDefault(item => item.Room.Id == room.Id && item.Equipment.Name == equipmentName);
                    if (matchingItem != default)
                    {
                        items.Add(new DisplayLowQuantityItem(room, matchingItem.Equipment, matchingItem.Quantity));
                    }
                    else
                    {
                        items.Add(new DisplayLowQuantityItem(room, new Equipment(equipmentName, GetEquipmentType(equipmentName)), 0));
                    }
                }
            }
            items = items.Where(item => item.Quantity < 5 && item.Room.Id == selectedRoom.Id).ToList();
            return new ObservableCollection<DisplayLowQuantityItem>(items);
        }

        public static EquipmentType GetEquipmentType(string name)
        {
            if (name == "Gaza" || name == "Kopca" || name == "Hanzaplast" || name == "Injekcija")
            {
                return EquipmentType.SurgeryEquipment;
            }
            else if (name == "Sto" || name == "Stolica" || name == "Ormar" || name == "Krevet")
            {
                return EquipmentType.IndoorFurniture;
            }
            else if (name == "Papir" || name == "Olovka")
            {
                return EquipmentType.ExaminationEquipment;
            }
            else
            {
                return EquipmentType.HallwayEquipment;
            }
        }
    }
}

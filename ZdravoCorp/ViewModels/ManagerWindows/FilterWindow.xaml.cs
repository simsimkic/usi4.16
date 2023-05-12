using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp;

namespace ZdravoCorp.ViewModels.ManagerWindows
{
    // Class used for mapping value to a Text displayed in comboboxes
    public class DisplayItem
    {
        public string? DisplayText { get;  set; }
        public RoomType? ValueRoomType { get;  set; }
        public EquipmentType? ValueEquimpentType { get;  set; }
    }
    public partial class FilterWindow : Window
    {
        public List<InventoryItem> FilteredItems { get; private set; }
        public ListView InventoryList { get; private set; }
        public FilterWindow(ListView inventoryList)
        {
            InitializeComponent();

            InventoryList = inventoryList;

            List<string> quantityOptions = new List<string>();
            quantityOptions.Add("Nema na stanju");
            quantityOptions.Add("0-10");
            quantityOptions.Add("10+");

            cmbRoomType.ItemsSource = GetTranslatedRoomTypes();
            cmbRoomType.DisplayMemberPath = "DisplayText";

            cmbEquipmentType.ItemsSource = GetTranslatedEquipmentTypes();
            cmbEquipmentType.DisplayMemberPath = "DisplayText";
            cmbQuantity.ItemsSource = quantityOptions;

            FilteredItems = InventoryRepository.Instance.Items;
        }

        public void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Filtrate(object sender, RoutedEventArgs e)
        {
            return;
        }

        public string TranslateRoomType(RoomType roomType)
        {
            switch (roomType)
            {
                case RoomType.OperatingRoom:
                    return "Operaciona sala";
                case RoomType.ExaminatingRoom:
                    return "Sala za preglede";
                case RoomType.PatientRoom:
                    return "Sala za smestaj bolesnika";
                case RoomType.WaitingRoom:
                    return "Cekaonica";
                case RoomType.StorageRoom:
                    return "Magacin";
            }

            return "";
        }

        public string TranslateEquimpentType(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.ExaminationEquipment:
                    return "Oprema za preglede";
                case EquipmentType.SurgeryEquipment:
                    return "Oprema za operacije";
                case EquipmentType.IndoorFurniture:
                    return "Sobni namestaj";
                case EquipmentType.HallwayEquipment:
                    return "Oprema za hodnike";
            }
            return "";
        }
        public List<DisplayItem> GetTranslatedRoomTypes()
        {
            List<DisplayItem> translatedRoomTypes = new List<DisplayItem>();

            foreach (RoomType roomType in Enum.GetValues(typeof(RoomType)))
            {
                
                translatedRoomTypes.Add(new DisplayItem { DisplayText = TranslateRoomType(roomType), ValueRoomType = roomType});
            }

            return translatedRoomTypes;
        }

        public List<DisplayItem> GetTranslatedEquipmentTypes()
        {
            List<DisplayItem> translatedEquimpentTypes = new List<DisplayItem>();

            foreach (EquipmentType equipmentType in Enum.GetValues(typeof(EquipmentType)))
            {
                translatedEquimpentTypes.Add(new DisplayItem { DisplayText = TranslateEquimpentType(equipmentType), ValueEquimpentType = equipmentType });
            }

            return translatedEquimpentTypes;
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            if (cmbRoomType.SelectedItem != null)
            {
                DisplayItem? selectedItem = (DisplayItem)cmbRoomType.SelectedItem;
                if (selectedItem != null)
                {
                    RoomType? selectedRoomType = selectedItem.ValueRoomType;

                    FilteredItems = FilteredItems.Where(item => item.Room.Type== selectedRoomType).ToList();
                }
            }

            if(cmbEquipmentType.SelectedItem != null)
            {
                DisplayItem? selectedItem = (DisplayItem)cmbEquipmentType.SelectedItem;
                if (selectedItem != null)
                {
                    EquipmentType? selectedEquimpentType = selectedItem.ValueEquimpentType;

                    FilteredItems = FilteredItems.Where(item => item.Equipment.Type== selectedEquimpentType).ToList();
                }
            }

            if(cmbQuantity.SelectedItem != null)
            {
                switch(cmbQuantity.SelectedIndex)
                {
                    case 0:
                        FilteredItems = FilteredItems.Where(item => item.Quantity.Equals(0)).ToList(); 
                        break;
                    case 1:
                        FilteredItems = FilteredItems.Where(item => item.Quantity > 0 && item.Quantity <= 10).ToList();
                        break;
                    case 2:
                        FilteredItems = FilteredItems.Where(item => item.Quantity > 10).ToList();
                        break;
                }
            }
            if (checkBoxOutStorage.IsChecked != null && checkBoxOutStorage.IsChecked == true)
            {
                FilteredItems = FilteredItems.Where(item => item.Room.Type != RoomType.StorageRoom).ToList();
            }

            InventoryList.ItemsSource = FilteredItems;
            this.Close();
        }
    }
}

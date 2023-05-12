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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoCorp;
using ZdravoCorp.Classes.Users;

namespace ZdravoCorp.ViewModels.ManagerWindows
{
    /// <summary>
    /// Interaction logic for LowEquipment.xaml
    /// </summary>
    /// 
    public class DisplayLowEquipmentItem{
        public string Name { get; set; }
        public int Quantity { get; set; }

        public DisplayLowEquipmentItem(string name, int quantity) 
        {
            Name = name;
            Quantity = quantity;
        }
    }
    public partial class LowEquipmentWindow : Window
    {

        public OrderRequest OrderRequest { get; set; }
        public LowEquipmentWindow()
        {
            InitializeComponent();
            DataContext = this;
            InventoryList.ItemsSource = GetLowEquipment();
            OrderRequest= new OrderRequest();
        }

        private static ObservableCollection<DisplayLowEquipmentItem> GetLowEquipment()
        {
            List<InventoryItem> dynamicEquipment = InventoryRepository.Instance.Items.Where(item=> Equipment.GetEquipmentNames()[item.Equipment.Name] == true).ToList();

            List<DisplayLowEquipmentItem> filteredItems = dynamicEquipment
            .GroupBy(item => item.Equipment.Name)
            .Select(g => new DisplayLowEquipmentItem(g.Key, g.Sum(item => item.Quantity))).ToList();

            foreach(KeyValuePair<string,bool> kvp in Equipment.GetEquipmentNames())
            {
                if(kvp.Value == false) { continue; }
                if(filteredItems.Any(displayItem => displayItem.Name == kvp.Key)){ continue; }
                filteredItems.Add(new DisplayLowEquipmentItem(kvp.Key, 0));
            }
            filteredItems = filteredItems.Where(item => item.Quantity < 5).ToList();
            return new ObservableCollection<DisplayLowEquipmentItem>(filteredItems);
        }

        public void AddToBasket_Click(object sender, EventArgs e)
        {
            if(InventoryList.SelectedItem == null) 
            {
                MessageBox.Show("Morate izabrati opremu","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DisplayLowEquipmentItem selectedItem = (DisplayLowEquipmentItem)InventoryList.SelectedItem;
            QuantityDialog quantityDialog = new QuantityDialog();
            Equipment equipment = new Equipment(selectedItem.Name, GetEquipmentType(selectedItem.Name));
            quantityDialog.ShowDialog();
            int quantity = quantityDialog.Quantity;
            Order order = new Order(equipment, quantity, DateTime.Now);
            OrderRequest.Add(order);
        }

        public void Basket_Click(object sender, EventArgs e)
        {
            if (OrderRequest.Orders.Count == 0) 
            {
                MessageBox.Show("Korpa je prazna", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }
            BasketWindow basketWindow = new BasketWindow(OrderRequest,this);
            this.Hide();
            basketWindow.ShowDialog();
            

        }

        public EquipmentType GetEquipmentType(string name) 
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

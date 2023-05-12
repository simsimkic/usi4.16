using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace ZdravoCorp.Classes.Users
{
    public class OrderRequest
    {
        public List<Order> Orders;
        readonly string FilePath =("..\\..\\..\\Data\\orders.json");

        public OrderRequest()
        {
            Orders = new List<Order>();
        }
        public bool Has(Order order)
        {
            foreach(Order _order in Orders)
            {
                if(_order.Equipment.Name == order.Equipment.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Add(Order order)
        {
            if (Has(order))
            {
                foreach (Order _order in Orders)
                {
                    if (_order.Equipment.Name == order.Equipment.Name)
                    {
                        _order.Quantity += order.Quantity;
                        return true;
                    }
                }
            }
            Orders.Add(order);
            return true;
        }
        public void Send()
        {
            List<string> serializedOrders = new List<string>();
            File.WriteAllText(FilePath, string.Empty);
            foreach (Order order in Orders)
            {
                string serializedOrder = JsonConvert.SerializeObject(order, Formatting.Indented);
                serializedOrders.Add(serializedOrder);
            }

            string jsonContent = "[" + string.Join(",", serializedOrders) + "]";

            File.WriteAllText(FilePath, jsonContent);
        }

        public void IsDayPassedFromOrderDate()
        {
            string jsonContent = File.ReadAllText(FilePath);
            Orders = JsonConvert.DeserializeObject<List<Order>>(jsonContent) ?? new List<Order>();
            if(Orders == null || Orders.Count == 0) { return; }
            foreach(Order order in Orders)
            {
                TimeSpan timeSinceOrder = DateTime.Now - order.OrderDate;
                if(timeSinceOrder >= TimeSpan.FromDays(1))
                {
                    Room? storageRoom = FindStorageRoom();
                    if (storageRoom == null) return;
                    InventoryItem inventoryItem = new InventoryItem(order.Equipment, storageRoom, order.Quantity);
                    InventoryRepository.Instance.Add(inventoryItem);
                    Orders.Remove(order);
                    Send();
                    if(Orders.Count == 0) { break; }
                }
            }
        }


        private static Room? FindStorageRoom()
        {
            foreach(Room room in RoomRepository.Instance.Rooms)
            {
                if (room.Type == RoomType.StorageRoom)
                {
                    return room;
                }
            }
            return null;
        }
    }
}

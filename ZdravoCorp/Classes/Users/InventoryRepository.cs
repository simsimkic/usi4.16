using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using ZdravoCorp.Classes.Users;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace ZdravoCorp
{
    public sealed class InventoryRepository
    {
        private static readonly InventoryRepository instance = new InventoryRepository();

        public static InventoryRepository Instance
        {
            get
            {
                return instance;
            }
        }

        private InventoryRepository()
        {
            Items = new List<InventoryItem>();
        }

        public List<InventoryItem> Items { get; set; }

        readonly string FilePath = Path.Combine("..\\..\\..\\Data\\inventory.json");

        public bool Has(InventoryItem item)
        {
            foreach (InventoryItem it in Items) 
            {
                if (it.Equipment.Name.ToLower().Equals(item.Equipment.Name.ToLower()) && it.Room.Id == item.Room.Id) return true;
            }
            return false;
        }
        public void Add(InventoryItem item)
        {
            if (!Has(item))
            {
                Items.Add(item);
                SaveInventory();
                return;
            }
  
            foreach (InventoryItem it in Items) 
            {
                if (it.Equipment.Name.ToLower().Equals(item.Equipment.Name.ToLower()) && it.Room.Id == item.Room.Id)
                {
                    it.Quantity += item.Quantity;
                    SaveInventory();
                    return;
                }
            }
            return;
            
            
        }
        public bool Remove(InventoryItem item)
        {
            if (!Has(item))
            {
                return false;
            }
            foreach(InventoryItem it in Items)
            {
                if (it.Equipment.Name.ToLower().Equals(item.Equipment.Name.ToLower()) && it.Room.Id == item.Room.Id && it.Quantity >= item.Quantity)
                {
                    it.Quantity -= item.Quantity;
                    SaveInventory();
                    return true;
                }
            }
            return false;
        }

        public void LoadInventory()
        {
            string jsonContent = File.ReadAllText(FilePath);
            Items = JsonConvert.DeserializeObject<List<InventoryItem>>(jsonContent) ?? new List<InventoryItem>();
        }
        public void SaveInventory()
        {
            File.WriteAllText(FilePath, string.Empty);

            List<string> serializedItems = new List<string>();
            Items = Items.Where(item => item.Quantity > 0).ToList();
            foreach(InventoryItem item in Items)
            {
                string serializedItem = JsonConvert.SerializeObject(item,Formatting.Indented);
                serializedItems.Add(serializedItem);
            }
            string jsonContent = "[" + string.Join(",", serializedItems) + "]";

            File.WriteAllText(FilePath, jsonContent);
        }   
    }
}

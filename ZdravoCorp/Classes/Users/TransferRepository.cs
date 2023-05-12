using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZdravoCorp.Classes.Users
{
    public sealed class TransferRepository
    {
        private static readonly TransferRepository instance = new TransferRepository();

        public static TransferRepository Instance
        {
            get
            {
                return instance;
            }
        }

        private TransferRepository()
        {
            Transfers = new List<Transfer>();
        }

        public List<Transfer> Transfers { get; set; }

        string Filepath = Path.Combine("..\\..\\..\\Data\\transfers.json");

        public bool Has(Transfer transfer)
        {
            Transfer? foundTransfer = Transfers.Where(item => item.Equals(transfer)).FirstOrDefault();
            if(foundTransfer != null)
            {
                return true;
            }
            return false;
        }

        public bool AddTransfer(Transfer transfer)
        {
            if (Has(transfer))
            {
                return false;
            }
            if(transfer.IsDynamic == true)
            {
                ExecuteTransfer(transfer);
                return true;
            }
            Transfers.Add(transfer);
            SaveTransfers();
            return true;
        }
        public void SaveTransfers()
        {
            List<string> serializedTransfers = new List<string>();
            File.WriteAllText(Filepath, string.Empty);
            foreach (Transfer transfer in Transfers)
            {
                string serializedTransfer = JsonConvert.SerializeObject(transfer, Formatting.Indented);
                serializedTransfers.Add(serializedTransfer);
            }

            string jsonContent = "[" + string.Join(",", serializedTransfers) + "]";

            File.WriteAllText(Filepath, jsonContent);
            LoadTransfers();
        }

        public List<Transfer> LoadTransfers()
        {
            string jsonContent = File.ReadAllText(Filepath);
            List<Transfer> transfers = JsonConvert.DeserializeObject<List<Transfer>>(jsonContent) ?? new List<Transfer>();
            if (transfers == null || transfers.Count == 0)
            {
                return new List<Transfer>();
            }
            foreach (Transfer transfer in transfers)
            {
                TimeSpan timeSinceOrder = DateTime.Now - transfer.MoveDate;
                if (timeSinceOrder >= TimeSpan.Zero)
                {
                    ExecuteTransfer(transfer);
                    transfers.Remove(transfer);
                    if (transfers.Count == 0) break;
                    SaveTransfers();
                    continue;
                }
            }
            return transfers;
        }

        public static void ExecuteTransfer(Transfer transfer)
        {
            InventoryItem? fromInventoryItem = InventoryRepository.Instance.Items.FirstOrDefault(item => item.Room.Id == transfer.FromRoom.Id && item.Equipment.Name == transfer.Equipment.Name);
            if (fromInventoryItem != null)
            {
                if(fromInventoryItem.Quantity < transfer.Quantity)
                {
                    return;
                }
                fromInventoryItem.Quantity -= transfer.Quantity;
                if(fromInventoryItem.Quantity == 0)
                {
                    InventoryRepository.Instance.Items.Remove(fromInventoryItem);
                }
            }
            InventoryItem? toInventoryItem = InventoryRepository.Instance.Items.FirstOrDefault(item => item.Room.Id == transfer.ToRoom.Id && item.Equipment.Name == transfer.Equipment.Name);
            if (toInventoryItem != null)
            {
                toInventoryItem.Quantity += transfer.Quantity;
            }
            else
            {
                InventoryItem inventoryItem = new InventoryItem(transfer.Equipment,transfer.ToRoom,transfer.Quantity);
                InventoryRepository.Instance.Items.Add(inventoryItem);
            }
            InventoryRepository.Instance.SaveInventory();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.IO;

namespace ZdravoCorp.Classes.Users
{
    public class Transfer
    {
        public Room FromRoom { get; private set; }
        public Room ToRoom { get; private set; }
        public int Quantity { get; private set; }
        public Equipment Equipment { get; private set; }
        public DateTime MoveDate { get; private set; }
        public bool IsDynamic { get; private set; }
        public Transfer(Room fromRoom, Room toRoom, int quantity, DateTime moveDate,Equipment equipment, bool isDynamic)
        {
            FromRoom = fromRoom;
            ToRoom = toRoom;
            Quantity = quantity;
            MoveDate = moveDate;
            Equipment= equipment;
            IsDynamic = isDynamic;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Classes.Users
{
    public class Order
    {
        public Equipment Equipment { get; private set; }
        public int Quantity { get;  set; }
        public DateTime OrderDate { get; private set; }
        public Order(Equipment equipment, int quantity, DateTime orderDate)
        {
            Equipment = equipment;
            Quantity = quantity;
            OrderDate = orderDate;
        }
    }
}

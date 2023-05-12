namespace ZdravoCorp
{
    public class InventoryItem
    {
        public Equipment Equipment { get; private set; }
        public Room Room { get; private set; }
        public int Quantity { get;  set; }

        public InventoryItem(Equipment equipment, Room room, int quantity)
        {
            Equipment = equipment;
            Room = room;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"Equipment: {Equipment}, Room: {Room}, Quantity: {Quantity}";
        }

    }
}

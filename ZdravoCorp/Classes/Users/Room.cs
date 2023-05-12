using System;

namespace ZdravoCorp
{
    public enum RoomType 
    { 
        OperatingRoom,
        ExaminatingRoom,
        PatientRoom,
        WaitingRoom,
        StorageRoom

    }
    public class Room
    {
        public int Id { get;  set; }
        public string Name { get; private set; }
        public RoomType Type { get; private set; }

        public Room(string name, RoomType type)
        {
            Name = name;
            Type = type;
        }


        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name} , Type: {Type}";
        }


    }
}

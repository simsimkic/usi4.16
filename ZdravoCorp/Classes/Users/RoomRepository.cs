using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Windows;

namespace ZdravoCorp
{
    public sealed class RoomRepository
    {
        private static readonly RoomRepository instance = new RoomRepository();

        public static RoomRepository Instance
        {
            
            get
            {
                return instance;
            }
        }

        private RoomRepository()
        {
            Rooms = new List<Room>();
        }

        public List<Room> Rooms { get; private set; }
        string filePath = ("..\\..\\..\\Data\\rooms.json");

        public void Add(Room room)
        {
            GenerateId(room);
            Rooms.Add(room);
            SaveRooms();
        }

        public void Remove(int roomId)
        {
            foreach (Room r in Rooms)
            {
                if (roomId == r.Id)
                {
                    Rooms.Remove(r);
                    break;
                }
            }
            SaveRooms();
        }

        public void GenerateId(Room room)
        {
            if (Rooms.Count == 0) { room.Id = 1; }

            for (int i = 1; i < Rooms.Count; i++)
            {
                if (i != Rooms[i - 1].Id) { room.Id = i; return; }
            }
            room.Id = Rooms.Count + 1;
            return;
        }
        public void LoadRooms()
        {

            string jsonContent = File.ReadAllText(filePath);
            Rooms = JsonConvert.DeserializeObject<List<Room>>(jsonContent);
        }

        public void SaveRooms()
        {
            File.WriteAllText(filePath, string.Empty);

            List<string> serializedRooms = new List<string>();
            Rooms.Sort((room1, room2) => room1.Id.CompareTo(room2.Id));

            foreach (Room room in Rooms)
            {
                string serializedRoom = JsonConvert.SerializeObject(room, Formatting.Indented);
                serializedRooms.Add(serializedRoom);
            }

            string jsonContent = "[" + string.Join(",", serializedRooms) + "]";

            File.WriteAllText(filePath, jsonContent);

        }




    }
}
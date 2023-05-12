using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using Newtonsoft.Json;

namespace ZdravoCorp
{
    public enum EquipmentType
    {
        ExaminationEquipment,
        SurgeryEquipment,
        IndoorFurniture,
        HallwayEquipment
    }

    public class Equipment
    {
        public string Name { get;  set; }
        public EquipmentType Type { get;  set; }
        public string TypeTranslation { get;  set; }
        public Equipment(string name, EquipmentType equipmentType)
        {

            Type = equipmentType;
            Name = name;
            TypeTranslation = TranslateType();
        }
        public override string ToString()
        {
            return $"Name: {Name}, Type: {Type}";
        }
        public string TranslateType()
        {
            switch (Type)
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

        public static Dictionary<string,bool> GetEquipmentNames() 
        {
            string filePath = ("..\\..\\..\\Data\\equipmentNames.json");
            string jsonContent = File.ReadAllText(filePath);
            Dictionary<string,bool>? equipmentNames = JsonConvert.DeserializeObject<Dictionary<string, bool>>(jsonContent) ?? new Dictionary<string, bool>();

            return equipmentNames;
        }

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    //Singleton class
    public sealed class Archive
    {
        private static readonly Archive instance = new Archive();
        public static Archive Instance
        {
            get
            {
                return instance;
            }
        }
        private Archive()
        {
            ArchiveRecords = new List<ArchiveRecord>();
        }
        public List<ArchiveRecord> ArchiveRecords { get; private set; }

        public void createRecord(ArchiveRecord archiveRecord)
        {
            ArchiveRecords.Add(archiveRecord);
            string json = JsonConvert.SerializeObject(ArchiveRecords);
            File.WriteAllText("..\\..\\..\\Data\\archive.json", json);
        }
        public void ReadArchive()
        {
            string json = File.ReadAllText("..\\..\\..\\Data\\archive.json");
            ArchiveRecords = JsonConvert.DeserializeObject<List<ArchiveRecord>>(json);
        }
    }
}

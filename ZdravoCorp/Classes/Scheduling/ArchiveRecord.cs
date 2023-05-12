using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class ArchiveRecord
    {
        public User User { get; set; }

        public DateTime Date;

        public string ChangeType;

        public ArchiveRecord(User user,DateTime date, string changeType)
        {
            User = user;
            Date = date;
            ChangeType = changeType;
        }  
        public ArchiveRecord() { }
    

    }

}

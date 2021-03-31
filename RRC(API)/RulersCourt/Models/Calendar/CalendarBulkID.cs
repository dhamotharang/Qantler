using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarBulkID
    {
        [DataMember(Name = "CalendarID")]
        public int CalendarID { get; set; }
    }
}

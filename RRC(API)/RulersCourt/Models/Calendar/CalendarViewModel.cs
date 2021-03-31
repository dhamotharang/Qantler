using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarViewModel
    {
        [DataMember(Name = "Collection")]
        public List<CalendarViewListModel> Collection { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }
    }
}

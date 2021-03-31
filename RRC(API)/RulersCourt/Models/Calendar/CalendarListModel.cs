using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarListModel
    {
        [DataMember(Name = "Collection")]
        public List<CalendarListViewModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }
    }
}

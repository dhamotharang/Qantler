using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_HolidayListModel
    {
        [DataMember(Name = "Collections")]
        public List<M_HolidayModel> Collections { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}

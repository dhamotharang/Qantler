using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingCalendarModel
    {
        [DataMember(Name = "Collection")]
        public List<MeetingCalendarViewModel> Collection { get; set; }
    }
}

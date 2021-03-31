using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarHistoryModel
    {
        [DataMember(Name = "CommunicationID")]
        public int? CommunicationID { get; set; }

        [DataMember(Name = "CalendarID")]
        public int? CalendarID { get; set; }

        [DataMember(Name = "Comment")]
        public string Comment { get; set; }

        [DataMember(Name = "ParentCommunicationID")]
        public int? ParentCommunicationID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}

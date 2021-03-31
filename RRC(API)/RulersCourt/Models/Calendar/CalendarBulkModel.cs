using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarBulkModel
    {
        [DataMember(Name = "CalendarID")]
        public int? CalendarID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "EventRequestor")]
        public string EventRequestor { get; set; }

        [DataMember(Name = "EventType")]
        public string EventType { get; set; }

        [DataMember(Name = "EventDetails")]
        public string EventDetails { get; set; }

        [DataMember(Name = "DateFrom")]
        public DateTime? DateFrom { get; set; }

        [DataMember(Name = "DateTo")]
        public DateTime? DateTo { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarPostModel
    {
        [DataMember(Name = "EventRequestor")]
        public string EventRequestor { get; set; }

        [DataMember(Name = "EventType")]
        public int? EventType { get; set; }

        [DataMember(Name = "EventDetails")]
        public string EventDetails { get; set; }

        [DataMember(Name = "DateFrom")]
        public DateTime? DateFrom { get; set; }

        [DataMember(Name = "DateTo")]
        public DateTime? DateTo { get; set; }

        [DataMember(Name = "AllDayEvents")]
        public bool? AllDayEvents { get; set; }

        [DataMember(Name = "Location")]
        public int? Location { get; set; }

        [DataMember(Name = "City")]
        public int? City { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "ParentReferenceNumber")]
        public string ParentReferenceNumber { get; set; }
    }
}

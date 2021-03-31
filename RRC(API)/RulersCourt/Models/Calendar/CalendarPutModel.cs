using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarPutModel
    {
        [DataMember(Name = "CalendarID")]
        public int? CalendarID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

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

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "ParentReferenceNumber")]
        public string ParentReferenceNumber { get; set; }

        [DataMember(Name = "IsApologySent")]
        public bool? IsApologySent { get; set; }
    }
}

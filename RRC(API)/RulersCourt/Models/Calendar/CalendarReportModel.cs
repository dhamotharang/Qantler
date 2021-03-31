using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarReportModel
    {
        [DataMember(Name = "CalendarID")]
        public int? CalendarID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "EventRequestor")]
        public string EventRequestor { get; set; }

        [DataMember(Name = "EventType")]
        public int? EventType { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "DateFrom")]
        public DateTime? DateFrom { get; set; }

        [DataMember(Name = "DateTo")]
        public DateTime? DateTo { get; set; }

        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }
    }
}

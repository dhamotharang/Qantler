using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarViewListModel
    {
        [DataMember(Name = "CalendarID")]
        public int? CalendarID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "EventRequestor")]
        public string EventRequestor { get; set; }

        [DataMember(Name = "EventType")]
        public int? EventType { get; set; }

        [DataMember(Name = "DateFrom")]
        public DateTime? DateFrom { get; set; }

        [DataMember(Name = "DateTo")]
        public DateTime? DateTo { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "Holiday")]
        public DateTime? Holiday { get; set; }

        [DataMember(Name = "HolidayMessage")]
        public string HolidayMessage { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "AllDayEvents")]
        public bool? AllDayEvents { get; set; }
    }
}

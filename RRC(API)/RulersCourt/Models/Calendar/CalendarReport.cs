using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Calendar
{
    [DataContract]
    public class CalendarReport
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("طالب الحدث/ الفعالية")]
        [DataMember(Name = "EventRequestor")]
        public string EventRequestor { get; set; }

        [DisplayName("نوع الحدث/ الفعالية")]
        [DataMember(Name = "EventType")]
        public string EventType { get; set; }

        [DisplayName("الموقع")]
        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DisplayName("اسم المستخدم")]
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }
    }
}

using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Maintenance
{
    [DataContract]
    public class MaintenanceReportModel
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("المصدر")]
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DisplayName("الموضوع")]
        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("تاريخ الطلب")]
        [DataMember(Name = "RequestedDateTime")]
        public string RequestedDateTime { get; set; }

        [DisplayName("مقدم من")]
        [DataMember(Name = "AttendedBy")]
        public string AttendedBy { get; set; }

        [DisplayName("الأولويات")]
        [DataMember(Name = "Priority")]
        public string Priority { get; set; }
    }
}

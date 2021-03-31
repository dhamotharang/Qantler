using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Legal
{
    [DataContract]
    public class LegalReport
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("المصدر")]
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DisplayName("الموضوع")]
        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DisplayName("تاريخ الطلب")]
        [DataMember(Name = "RequestDate")]
        public string RequestDate { get; set; }

        [DisplayName("مقدم من")]
        [DataMember(Name = "AttendedBy")]
        public string AttendedBy { get; set; }

        [DisplayName("أنشئت من قبل")]
        [DataMember(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [DisplayName("تمت الموافقة عليه من قبل")]
        [DataMember(Name = "ApprovedBy")]
        public string ApprovedBy { get; set; }
    }
}

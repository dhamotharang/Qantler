using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class Report
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("الموضوع")]
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DisplayName("صادر من")]
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DisplayName("الوجهة")]
        [DataMember(Name = "DestinationOU")]
        public string DestinationOU { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("المشاركة مع الزملاء؟")]
        [DataMember(Name = "Private")]
        public string Private { get; set; }

        [DisplayName("الأولويات")]
        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DisplayName("المنشئ")]
        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DisplayName("اسم المعتمد")]
        [DataMember(Name = "Approver")]
        public string Approver { get; set; }

        [DisplayName("اسم المرسل إليه - الوجهة")]
        [DataMember(Name = "DestinationUserName")]
        public string DestinationUserName { get; set; }
    }
}

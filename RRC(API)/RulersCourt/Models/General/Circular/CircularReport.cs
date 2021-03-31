using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Circular
{
    [DataContract]
    public class CircularReport
    {
        [DisplayName("رقم التعميم")]
        [DataMember(Name = "CircularID")]
        public int? CircularID { get; set; }

        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("العنوان")]
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DisplayName("المصدر")]
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DisplayName("الوجهة")]
        [DataMember(Name = "DestinationOU")]
        public string DestinationOU { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("الأولويات")]
        [DataMember(Name = "Priority")]
        public string Priority { get; set; }
    }
}

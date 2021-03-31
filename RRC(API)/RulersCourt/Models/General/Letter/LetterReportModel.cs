using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class LetterReportModel
    {
        [DisplayName("رقم الكتاب")]
        [DataMember(Name = "LetterID")]
        public int? LetterID { get; set; }

        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("العنوان")]
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DisplayName("المصدر")]
        [DataMember(Name = "Source")]
        public string Source { get; set; }

        [DisplayName("الوجهة")]
        [DataMember(Name = "Destination")]
        public string Destination { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("الربط مع كتاب اخر")]
        [DataMember(Name = "LinkedToOtherLetter")]
        public string LinkedToOtherLetter { get; set; }

        [DisplayName("الأولويات")]
        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DisplayName("نوع الكتاب")]
        [DataMember(Name = "LetterType")]
        public string LetterType { get; set; }
    }
}

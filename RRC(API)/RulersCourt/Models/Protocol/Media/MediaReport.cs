using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol.Media
{
    public class MediaReport
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "RefID")]
        public string RefID { get; set; }

        [DisplayName("صادرة عن")]
        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DisplayName("اسم المصدر")]
        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("نوع الطلب")]
        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DisplayName("تاريخ الطلب")]
        [DataMember(Name = "RequestDate")]
        public string RequestDate { get; set; }
    }
}

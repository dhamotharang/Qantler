using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.ITSupport
{
    [DataContract]
    public class ITReport
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("منشئ المهمة")]
        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DisplayName("نوع الطلب")]
        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("تاريخ الطلب")]
        [DataMember(Name = "RequestDate")]
        public string RequestDate { get; set; }
    }
}
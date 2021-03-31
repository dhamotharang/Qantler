using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.CitizenAffair
{
    [DataContract]
    public class CitizenAffairReportModel
    {
        [DisplayName("رقم الطلب")]
        [DataMember(Name = "CitizenAffairID")]
        public int? CitizenAffairID { get; set; }

        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("نوع الطلب")]
        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("تاريخ الطلب")]
        [DataMember(Name = "RequestedDateTime")]
        public string RequestedDateTime { get; set; }

        [DisplayName("إسم الشخص / الموقع")]
        [DataMember(Name = "PersonalLocationName")]
        public string PersonalLocationName { get; set; }

        [DisplayName("رقم الهاتف")]
        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}

using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class VehicleRequestReportListModel
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("نوع الطلب")]
        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DisplayName("اسم مقدم الطلب")]
        [DataMember(Name = "RequestorName")]
        public string RequestorName { get; set; }

        [DisplayName("إدارة/ مكتب صاحب الطلب")]
        [DataMember(Name = "RequestorOfficeDepartment")]
        public string RequestorOfficeDepartment { get; set; }
    }
}

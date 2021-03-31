using System.ComponentModel;
using System.Runtime.Serialization;

namespace RulersCourt.Repository.Gift
{
    [DataContract]
    public class GiftReportModel
    {
        [DisplayName("الرقم المرجعي")]
        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DisplayName("نوع الهدية")]
        [DataMember(Name = "GiftType")]
        public string GiftType { get; set; }

        [DisplayName("وردت من / تم شراؤها بواسطة")]
        [DataMember(Name = "RecievedFrom/PurchasedBy")]
        public string RecievedFromPurchasedBy { get; set; }

        [DisplayName("الحالة")]
        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DisplayName("أنشئت من قبل")]
        [DataMember(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
    }
}

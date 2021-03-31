using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftModel
    {
        [DataMember(Name = "GiftID")]
        public int? GiftID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "GiftType")]
        public string GiftType { get; set; }

        [DataMember(Name = "PurchasedBy")]
        public string PurchasedBy { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }
    }
}

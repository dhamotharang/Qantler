using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftReportPostModel
    {
        [DataMember(Name = "GiftType")]
        public int? GiftType { get; set; }

        [DataMember(Name = "RecievedPurchasedBy")]
        public string RecievedPurchasedBy { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftSaveResponseModel
    {
        [DataMember(Name = "GiftID")]
        public int? GiftID { get; set; }

        [DataMember(Name = "ReferenceNumber")]        public string ReferenceNumber { get; set; }        [DataMember(Name = "Status")]        public int? Status { get; set; }
    }
}

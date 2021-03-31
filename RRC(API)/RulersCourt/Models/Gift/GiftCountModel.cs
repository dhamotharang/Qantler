using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftCountModel
    {
        [DataMember(Name = "GiftsReceived")]
        public int? GiftsReceived { get; set; }

        [DataMember(Name = "GiftsPurchased")]
        public int? GiftsPurchased { get; set; }
    }
}

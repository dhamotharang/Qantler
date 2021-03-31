using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter.LetterInbound
{
    [DataContract]
    public class LetterInboundRelatedOutgoingModel
    {
        [DataMember(Name = "OutgoingLetterReferenceNo")]
        public string OutgoingLetterReferenceNo { get; set; }

        [DataMember(Name = "LetterID")]
        public string LetterID { get; set; }
    }
}

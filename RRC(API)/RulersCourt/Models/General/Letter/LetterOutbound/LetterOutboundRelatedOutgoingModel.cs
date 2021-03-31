using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundRelatedOutgoingModel
    {
        [DataMember(Name = "OutgoingLetterReferenceNo")]
        public string OutgoingLetterReferenceNo { get; set; }

        [DataMember(Name = "LetterID")]
        public string LetterID { get; set; }
    }
}

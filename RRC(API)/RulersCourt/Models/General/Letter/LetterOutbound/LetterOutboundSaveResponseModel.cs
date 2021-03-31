using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundSaveResponseModel
    {
        [DataMember(Name = "LetterID")]
        public int? LetterID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int? Status { get; set; }

        [DataMember(Name = "InvalidLetters")]
        public string InvalidLetters { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class LinkToLetterModel
    {
        [DataMember(Name = "LetterReferenceNumber")]
        public string LetterReferenceNumber { get; set; }

        [DataMember(Name = "LetterID")]
        public string LetterID { get; set; }

        [DataMember(Name = "LetterType")]
        public int? LetterType { get; set; }
    }
}
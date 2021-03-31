using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Letter
{
    [DataContract]
    public class LetterRelatedOutgoingLetterModel
    {
        [DataMember(Name = "ReferenceNo")]
        public string ReferenceNo { get; set; }

        [DataMember(Name = "LetterID")]
        public string LetterID { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundIDModel
    {
        [DataMember(Name = "LetterID")]
        public int LetterID { get; set; }
    }
}

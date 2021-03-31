using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter.LetterInbound
{
    [DataContract]
    public class LetterInboundKeywordsModel
    {
        [DataMember(Name = "Keywords")]
        public string Keywords { get; set; }
    }
}

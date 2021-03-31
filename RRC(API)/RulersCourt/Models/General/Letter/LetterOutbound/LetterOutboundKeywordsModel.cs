using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundKeywordsModel
    {
        [DataMember(Name = "Keywords")]
        public string Keywords { get; set; }
    }
}

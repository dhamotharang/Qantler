using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundReferenceModel
    {
        [DataMember(Name = "ReferenceNumbers")]
        public string ReferenceNumbers { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentLegalHeadModel
    {
        [DataMember(Name = "LegalHeadUsedId")]
        public int? LegalHeadUsedId { get; set; }
    }
}

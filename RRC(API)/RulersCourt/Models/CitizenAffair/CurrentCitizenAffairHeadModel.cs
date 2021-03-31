using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentCitizenAffairHeadModel
    {
        [DataMember(Name = "CAHeadUsedId")]
        public int? CAHeadUsedId { get; set; }
    }
}

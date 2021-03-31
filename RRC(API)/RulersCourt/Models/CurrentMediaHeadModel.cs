using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentMediaHeadModel
    {
        [DataMember(Name = "MediaHeadUsedId")]
        public int? MediaHeadUsedId { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentHRHeadModel
    {
        [DataMember(Name = "HRHeadUsedId")]
        public int? HRHeadUsedId { get; set; }
    }
}

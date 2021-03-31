using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_SpecializationModel
    {
        [DataMember(Name = "SpecializationID")]
        public int? SpecializationID { get; set; }

        [DataMember(Name = "SpecializationName")]
        public string SpecializationName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_ExperienceModel
    {
        [DataMember(Name = "ExperienceID")]
        public int? ExperienceID { get; set; }

        [DataMember(Name = "ExperienceName")]
        public string ExperienceName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

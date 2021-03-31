using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_EducationModel
    {
        [DataMember(Name = "EducationID")]
        public int? EducationID { get; set; }

        [DataMember(Name = "EducationName")]
        public string EducationName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_GradeModel
    {
        [DataMember(Name = "GradeID")]
        public int? GradeID { get; set; }

        [DataMember(Name = "GradeName")]
        public string GradeName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

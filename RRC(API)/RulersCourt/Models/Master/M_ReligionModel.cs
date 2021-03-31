using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_ReligionModel
    {
        [DataMember(Name = "ReligionID")]
        public int? ReligionID { get; set; }

        [DataMember(Name = "ReligionName")]
        public string ReligionName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

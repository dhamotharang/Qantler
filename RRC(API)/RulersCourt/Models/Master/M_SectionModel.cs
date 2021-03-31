using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_SectionModel
    {
        [DataMember(Name = "SectionID")]
        public int? SectionID { get; set; }

        [DataMember(Name = "SectionName")]
        public string SectionName { get; set; }
    }
}
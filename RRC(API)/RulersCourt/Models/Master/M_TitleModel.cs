using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_TitleModel
    {
        [DataMember(Name = "TitleID")]
        public int? TitleID { get; set; }

        [DataMember(Name = "TitleName")]
        public string TitleName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

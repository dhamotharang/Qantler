using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_LookupsModel
    {
        [DataMember(Name = "LookupsID")]
        public int? LookupsID { get; set; }

        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

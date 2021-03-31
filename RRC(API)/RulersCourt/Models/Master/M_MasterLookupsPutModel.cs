using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_MasterLookupsPutModel
    {
        [DataMember(Name = "LookupsID")]
        public int? LookupsID { get; set; }

        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "ArDisplayName")]
        public string ArDisplayName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

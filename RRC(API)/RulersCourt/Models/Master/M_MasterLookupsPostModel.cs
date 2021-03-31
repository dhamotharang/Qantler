using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_MasterLookupsPostModel
    {
        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "ArDisplayName")]
        public string ArDisplayName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

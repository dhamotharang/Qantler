using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_EmiratesModel
    {
        [DataMember(Name = "EmiratesID")]
        public int? EmiratesID { get; set; }

        [DataMember(Name = "EmiratesName")]
        public string EmiratesName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

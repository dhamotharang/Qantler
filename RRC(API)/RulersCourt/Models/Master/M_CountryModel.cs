using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_CountryModel
    {
        [DataMember(Name = "CountryID")]
        public int? CountryID { get; set; }

        [DataMember(Name = "CountryName")]
        public string CountryName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

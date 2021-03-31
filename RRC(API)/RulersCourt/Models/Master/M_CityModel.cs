using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_CityModel
    {
        [DataMember(Name = "CityID")]
        public int? CityID { get; set; }

        [DataMember(Name = "CityName")]
        public string CityName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

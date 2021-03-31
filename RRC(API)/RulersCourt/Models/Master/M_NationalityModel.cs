using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_NationalityModel
    {
        [DataMember(Name = "NationalityID")]
        public int? NationalityID { get; set; }

        [DataMember(Name = "NationalityName")]
        public string NationalityName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

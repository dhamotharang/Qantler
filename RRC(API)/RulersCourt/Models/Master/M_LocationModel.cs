using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_LocationModel
    {
        [DataMember(Name = "LocationID")]
        public int? LocationID { get; set; }

        [DataMember(Name = "LocationName")]
        public string LocationName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

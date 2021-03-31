using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Vehicle
{
    [DataContract]
    public class M_VehicleModel
    {
        [DataMember(Name = "VehicleModelID")]
        public int? VehicleModelID { get; set; }

        [DataMember(Name = "VehicleModelName")]
        public string VehicleModelName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

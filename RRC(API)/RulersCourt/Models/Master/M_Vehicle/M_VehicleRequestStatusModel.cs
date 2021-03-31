using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Vehicle
{
    [DataContract]
    public class M_VehicleRequestStatusModel
    {
        [DataMember(Name = "StatusID")]
        public int? StatusID { get; set; }

        [DataMember(Name = "StatusName")]
        public string StatusName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

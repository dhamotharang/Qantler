using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Vehicle
{
    [DataContract]
    public class M_TripReleaseLocationModel
    {
        [DataMember(Name = "TripReleaseLocationID")]
        public int? TripReleaseLocationID { get; set; }

        [DataMember(Name = "TripReleaseLocationName")]
        public string TripReleaseLocationName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

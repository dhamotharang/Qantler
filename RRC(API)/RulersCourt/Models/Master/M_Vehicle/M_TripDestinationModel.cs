using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Vehicle
{
    [DataContract]
    public class M_TripDestinationModel
    {
        [DataMember(Name = "TripDestinationID")]
        public int? TripDestinationID { get; set; }

        [DataMember(Name = "TripDestinationName")]
        public string TripDestinationName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class TripVehicleIssuesSaveResponseModel
    {
        [DataMember(Name = "TripVehicleIssueID")]
        public int? TripVehicleIssueID { get; set; }
    }
}

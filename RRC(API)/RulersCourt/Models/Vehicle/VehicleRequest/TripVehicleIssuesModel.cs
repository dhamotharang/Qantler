using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class TripVehicleIssuesModel
    {
        [DataMember(Name = "IssueID")]
        public int? IssueID { get; set; }

        [DataMember(Name = "IssueName")]
        public string IssueName { get; set; }
    }
}

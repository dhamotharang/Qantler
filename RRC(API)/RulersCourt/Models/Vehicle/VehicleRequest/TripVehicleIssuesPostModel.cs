using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.TripVehicleIssues
{
    [DataContract]
    public class TripVehicleIssuesPostModel
    {
        [DataMember(Name = "VehicleReqID")]
        public int? VehicleReqID { get; set; }

        [DataMember(Name = "IssueID")]
        public int? IssueID { get; set; }

        [DataMember(Name = "IssueName")]
        public string IssueName { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }
    }
}

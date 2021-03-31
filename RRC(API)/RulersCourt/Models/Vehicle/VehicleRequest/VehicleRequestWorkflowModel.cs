using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class VehicleRequestWorkflowModel
    {
        [DataMember(Name = "VehicleReqID")]
        public int? VehicleReqID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }

        [DataMember(Name = "VehicleTeamID")]
        public List<UserModel> VehicleTeamID { get; set; }

        [DataMember(Name = "VehicleTeamHeadID")]
        public List<UserModel> VehicleTeamHeadID { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }

        [DataMember(Name = "RequestorType")]
        public int? RequestorType { get; set; }

        [DataMember(Name = "RequestorID")]
        public int? RequestorID { get; set; }

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "DriverID")]
        public int? DriverID { get; set; }
    }
}

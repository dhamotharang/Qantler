using RulersCourt.Models.Vehicle.TripCoPassengers;
using RulersCourt.Models.Vehicle.TripVehicleIssues;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleRequest
{
    [DataContract]
    public class VehicleRequestPutModel
    {
        [DataMember(Name = "VehicleReqID")]
        public int? VehicleReqID { get; set; }

        [DataMember(Name = "RequestType")]
        public int? RequestType { get; set; }

        [DataMember(Name = "Requestor")]
        public int? Requestor { get; set; }

        [DataMember(Name = "RequestDateTime")]
        public DateTime? RequestDateTime { get; set; }

        [DataMember(Name = "DriverID")]
        public int? DriverID { get; set; }

        [DataMember(Name = "TobeDrivenbyDepartmentID")]
        public int? TobeDrivenbyDepartmentID { get; set; }

        [DataMember(Name = "TobeDrivenbyDriverID")]
        public int? TobeDrivenbyDriverID { get; set; }

        [DataMember(Name = "TripTypeID")]
        public int? TripTypeID { get; set; }

        [DataMember(Name = "TripTypeOthers")]
        public string TripTypeOthers { get; set; }

        [DataMember(Name = "Emirates")]
        public int? Emirates { get; set; }

        [DataMember(Name = "City")]
        public int? City { get; set; }

        [DataMember(Name = "Destination")]
        public int? Destination { get; set; }

        [DataMember(Name = "DestinationOthers")]
        public string DestinationOthers { get; set; }

        [DataMember(Name = "TripPeriodFrom")]
        public DateTime? TripPeriodFrom { get; set; }

        [DataMember(Name = "TripPeriodTo")]
        public DateTime? TripPeriodTo { get; set; }

        [DataMember(Name = "VehicleModelID")]
        public int? VehicleModelID { get; set; }

        [DataMember(Name = "ApproverDepartment")]
        public int? ApproverDepartment { get; set; }

        [DataMember(Name = "ApproverName")]
        public int? ApproverName { get; set; }

        [DataMember(Name = "ReleaseDateTime")]
        public DateTime? ReleaseDateTime { get; set; }

        [DataMember(Name = "LastMileageReading")]
        public int? LastMileageReading { get; set; }

        [DataMember(Name = "ReleaseLocationID")]
        public int? ReleaseLocationID { get; set; }

        [DataMember(Name = "ReturnDateTime")]
        public DateTime? ReturnDateTime { get; set; }

        [DataMember(Name = "CurrentMileageReading")]
        public int? CurrentMileageReading { get; set; }

        [DataMember(Name = "HavePersonalBelongings")]
        public bool? HavePersonalBelongings { get; set; }

        [DataMember(Name = "PersonalBelongingsText")]
        public string PersonalBelongingsText { get; set; }

        [DataMember(Name = "TripCoPassengers")]
        public List<TripCoPassengersModel> TripCoPassengers { get; set; }

        [DataMember(Name = "TripVehicleIssues")]
        public List<TripVehicleIssuesPostModel> TripVehicleIssues { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Notes")]
        public string Notes { get; set; }

        [DataMember(Name = "VehicleID")]
        public int? VehicleID { get; set; }

        [DataMember(Name = "ReturnLocationID")]
        public int? ReturnLocationID { get; set; }

        [DataMember(Name = "Reason")]
        public string Reason { get; set; }
    }
}

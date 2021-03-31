using RulersCourt.Models.Vehicle.TripVehicleIssues;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Vehicles
{
    [DataContract]
    public class VehiclePreviewModel
    {
        [DataMember(Name = "VehicleID")]
        public int? VehicleID { get; set; }

        [DataMember(Name = "PlateNumber")]
        public string PlateNumber { get; set; }

        [DataMember(Name = "PlateCode")]
        public string PlateCode { get; set; }

        [DataMember(Name = "VehicleMake")]
        public string VehicleMake { get; set; }

        [DataMember(Name = "VehicleModel")]
        public string VehicleModel { get; set; }

        [DataMember(Name = "DriverName")]
        public string DriverName { get; set; }

        [DataMember(Name = "ReleaseDate")]
        public string ReleaseDate { get; set; }

        [DataMember(Name = "ReleaseTime")]
        public string ReleaseTime { get; set; }

        [DataMember(Name = "ReleaseMeridiem")]
        public string ReleaseMeridiem { get; set; }

        [DataMember(Name = "ReleaseLocation")]
        public string ReleaseLocation { get; set; }

        [DataMember(Name = "LastMileageOnRelease")]
        public string LastMileageOnRelease { get; set; }

        [DataMember(Name = "ReturnDate")]
        public string ReturnDate { get; set; }

        [DataMember(Name = "ReturnTime")]
        public string ReturnTime { get; set; }

        [DataMember(Name = "ReturnMeridiem")]
        public string ReturnMeridiem { get; set; }

        [DataMember(Name = "ReturnLocation")]
        public string ReturnLocation { get; set; }

        [DataMember(Name = "YearofManufacture")]
        public string YearofManufacture { get; set; }

        [DataMember(Name = "LastMileageOnReturn")]
        public string LastMileageOnReturn { get; set; }

        [DataMember(Name = "ReleasedBy")]
        public string ReleasedBy { get; set; }

        [DataMember(Name = "ReturnedBy")]
        public string ReturnedBy { get; set; }

        [DataMember(Name = "Note")]
        public string Note { get; set; }

        [DataMember(Name = "PersonalBelonging")]
        public string PersonalBelonging { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "TripIssues")]
        public List<TripVehicleIssuesPostModel> TripIssues { get; set; }
    }
}

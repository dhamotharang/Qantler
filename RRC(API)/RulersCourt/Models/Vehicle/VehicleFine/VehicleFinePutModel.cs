using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.VehicleFine
{
    [DataContract]
    public class VehicleFinePutModel
    {
        [DataMember(Name = "VehicleID")]
        public int? VehicleID { get; set; }

        [DataMember(Name = "VehicleFineID")]
        public int? VehicleFineID { get; set; }

        [DataMember(Name = "VehicleModelID")]
        public string VehicleModelID { get; set; }

        [DataMember(Name = "FineNumber")]
        public string FineNumber { get; set; }

        [DataMember(Name = "FinedDate")]
        public DateTime? FinedDate { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "BlackPoints")]
        public int? BlackPoints { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "EmailTo")]
        public string EmailTo { get; set; }

        [DataMember(Name = "EmailCCDepartmentID")]
        public int? EmailCCDepartmentID { get; set; }

        [DataMember(Name = "EmailCCUserID")]
        public int? EmailCCUserID { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}

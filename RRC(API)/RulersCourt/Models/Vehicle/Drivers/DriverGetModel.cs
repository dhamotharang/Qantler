using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.Drivers
{
    [DataContract]
    public class DriverGetModel
    {
        [DataMember(Name = "DriverID")]
        public int? DriverID { get; set; }

        [DataMember(Name = "UserProfileID")]
        public int? UserProfileID { get; set; }

        [DataMember(Name = "TotalHour")]
        public int? TotalHour { get; set; }

        [DataMember(Name = "DriverName")]
        public string DriverName { get; set; }

        [DataMember(Name = "MobileNumber")]
        public string MobileNumber { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "CompensateExtra")]
        public List<DriverReport> CompensateExtra { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool? DeleteFlag { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "DriverTrips")]
        public List<DriverGetTripDaysModel> DriverTrips { get; set; }
    }
}

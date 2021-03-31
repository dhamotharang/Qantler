using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Maintenance
{
    [DataContract]
    public class MaintenanceSaveResponseModel
    {
        [DataMember(Name = "MaintenanceID")]
        public int? MaintenanceID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "MaintenanceManagerUserID")]
        public int? MaintenanceManagerUserID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace RulersCourt.Models
{
    public class WorkflowUserModel
    {
        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }

        [DataMember(Name = "EmployeeName")]
        public string EmployeeName { get; set; }

        [DataMember(Name = "AREmployeeName")]
        public string AREmployeeName { get; set; }

        [DataMember(Name = "OrgUnitID")]
        public int? OrgUnitID { get; set; }

        [DataMember(Name = "IsOrgHead")]
        public bool? IsOrgHead { get; set; }

        [DataMember(Name = "IsHOD")]
        public bool? IsHOD { get; set; }

        [DataMember(Name = "Gender")]
        public string Gender { get; set; }

        [DataMember(Name = "Age")]
        public string Age { get; set; }

        [DataMember(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        [DataMember(Name = "MobileNumber")]
        public string MobileNumber { get; set; }

        [DataMember(Name = "Religion")]
        public string Religion { get; set; }

        [DataMember(Name = "OfficialEmailID")]
        public string OfficialEmailID { get; set; }

        [DataMember(Name = "CanSendSMS")]
        public bool? CanSendSMS { get; set; }

        [DataMember(Name = "CanSendEmail")]
        public bool? CanSendEmail { get; set; }

        [DataMember(Name = "DepartmentID")]
        public int? DepartmentID { get; set; }
    }
}

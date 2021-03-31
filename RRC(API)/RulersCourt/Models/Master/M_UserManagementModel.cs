using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_UserManagementModel
    {
        [DataMember(Name = "UserProfileID")]
        public int? UserProfileID { get; set; }

        [DataMember(Name = "EmployeeName")]
        public string EmployeeName { get; set; }

        [DataMember(Name = "DepartmentID")]
        public int? DepartmentID { get; set; }

        [DataMember(Name = "SectionID")]
        public int? SectionID { get; set; }

        [DataMember(Name = "UnitID")]
        public int? UnitID { get; set; }

        [DataMember(Name = "HOD")]
        public bool? HOD { get; set; }

        [DataMember(Name = "HOS")]
        public bool? HOS { get; set; }

        [DataMember(Name = "HOU")]
        public bool? HOU { get; set; }

        [DataMember(Name = "CanRaiseOfficalRequest")]
        public bool? CanRaiseOfficalRequest { get; set; }

        [DataMember(Name = "CanManageNews")]
        public bool? CanManageNews { get; set; }

        [DataMember(Name = "CanEditContact")]
        public bool? CanEditContact { get; set; }

        [DataMember(Name = "balanceLeave")]
        public int? BalanceLeave { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace RulersCourt.Services
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "ADUserName")]
        public string ADUserName { get; set; }

        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "Password")]
        public string Password { get; set; }

        [DataMember(Name = "Token")]
        public string Token { get; set; }

        [DataMember(Name = "IsAdmin")]
        public bool IsAdmin { get; set; }

        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }

        [DataMember(Name = "UnitID")]
        public int? UnitID { get; set; }

        [DataMember(Name = "UnitName")]
        public string UnitName { get; set; }

        [DataMember(Name = "DepartmentName")]
        public string DepartmentName { get; set; }

        [DataMember(Name = "DepartmentID")]
        public int? DepartmentID { get; set; }

        [DataMember(Name = "SectionName")]
        public string SectionName { get; set; }

        [DataMember(Name = "SectionID")]
        public int? SectionID { get; set; }

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

        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentName")]
        public string AttachmentName { get; set; }

        [DataMember(Name = "InTime")]
        public DateTime? InTime { get; set; }

        [DataMember(Name = "OutTime")]
        public DateTime? OutTime { get; set; }

        [DataMember(Name = "IsWrdUser")]
        public bool? IsWrdUser { get; set; } = false;
    }
}

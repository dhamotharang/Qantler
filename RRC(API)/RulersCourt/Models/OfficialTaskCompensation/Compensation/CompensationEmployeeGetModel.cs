using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficialTaskCompensation.Compensation
{
    [DataContract]
    public class CompensationEmployeeGetModel
    {
        [DataMember(Name = "EmployeeID")]
        public int? EmployeeID { get; set; }

        [DataMember(Name = "EmployeeName")]
        public string EmployeeName { get; set; }

        [DataMember(Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }

        [DataMember(Name = "Grade")]
        public string Grade { get; set; }

        [DataMember(Name = "EmployeePosition")]
        public string EmployeePosition { get; set; }

        [DataMember(Name = "EmployeeDepartment")]
        public string EmployeeDepartment { get; set; }
    }
}

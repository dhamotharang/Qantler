using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskAssigneeDepartmentModel
    {
        [DataMember(Name = "TaskAssigneeDepartmentID")]
        public int TaskAssigneeDepartmentID { get; set; }

        [DataMember(Name = "TaskAssigneeDepartmentName")]
        public string TaskAssigneeDepartmentName { get; set; }
    }
}

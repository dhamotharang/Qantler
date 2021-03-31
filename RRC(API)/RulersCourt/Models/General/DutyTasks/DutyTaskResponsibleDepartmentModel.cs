using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskResponsibleDepartmentModel
    {
        [DataMember(Name = "TaskResponsibleDepartmentID")]
        public int? TaskResponsibleDepartmentID { get; set; }

        [DataMember(Name = "TaskResponsibleDepartmentName")]
        public string TaskResponsibleDepartmentName { get; set; }
    }
}
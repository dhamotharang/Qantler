using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskResponsibleUsersModel
    {
        [DataMember(Name = "TaskResponsibleUsersID")]
        public int? TaskResponsibleUsersID { get; set; }

        [DataMember(Name = "TaskResponsibleUsersName")]
        public string TaskResponsibleUsersName { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTask
{
    [DataContract]
    public class DutyTaskAssigneeUsersModel
    {
        [DataMember(Name = "TaskAssigneeUsersID")]
        public int TaskAssigneeUsersID { get; set; }

        [DataMember(Name = "TaskAssigneeUsersName")]
        public string TaskAssigneeUsersName { get; set; }
    }
}

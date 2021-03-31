using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTasks
{
    [DataContract]
    public class DutyTaskAssigneeAndCreatorModel
    {
        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
    }
}

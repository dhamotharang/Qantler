using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTasks
{
    [DataContract]
    public class DutyTaskTaggedUserIDModel
    {
        [DataMember(Name = "TaggedUsersID")]
        public int? TaggedUsersID { get; set; }

        [DataMember(Name = "TaggedUsersName")]
        public string TaggedUsersName { get; set; }
    }
}

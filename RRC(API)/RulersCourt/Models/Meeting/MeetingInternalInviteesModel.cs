using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingInternalInviteesModel
    {
        [DataMember(Name = "MeetingInternalInviteesID")]
        public int? MeetingInternalInviteesID { get; set; }

        [DataMember(Name = "DepartmentID")]
        public int? DepartmentID { get; set; }

        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }
    }
}

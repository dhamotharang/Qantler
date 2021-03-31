using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class ShareparticipationUsersModel
    {
        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }
    }
}

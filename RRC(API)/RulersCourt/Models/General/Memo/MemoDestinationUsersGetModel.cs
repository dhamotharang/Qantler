using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class MemoDestinationUsersGetModel
    {
        [DataMember(Name = "MemoDestinationUsersID")]
        public int? MemoDestinationUsersID { get; set; }

        [DataMember(Name = "MemoDestinationUsersName")]
        public string MemoDestinationUsersName { get; set; }
    }
}

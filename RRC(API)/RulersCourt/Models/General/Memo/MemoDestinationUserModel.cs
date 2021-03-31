using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Memo
{
    [DataContract]
    public class MemoDestinationUserModel
    {
        [DataMember(Name = "Collection")]
        public List<MemoDestinationUsersGetModel> Collection { get; set; }
    }
}

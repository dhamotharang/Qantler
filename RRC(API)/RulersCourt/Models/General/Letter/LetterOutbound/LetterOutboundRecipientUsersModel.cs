using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Letter.LetterOutbound
{
    [DataContract]
    public class LetterOutboundRecipientUsersModel
    {
        [DataMember(Name = "Collection")]
        public List<RecipientUsersModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}

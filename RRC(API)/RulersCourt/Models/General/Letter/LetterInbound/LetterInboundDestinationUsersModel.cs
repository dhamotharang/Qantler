using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter.LetterInbound
{
    [DataContract]
    public class LetterInboundDestinationUsersModel
    {
        [DataMember(Name = "LetterDestinationUsersID")]
        public int? LetterDestinationUsersID { get; set; }

        [DataMember(Name = "LetterDestinationUsersName")]
        public string LetterDestinationUsersName { get; set; }
    }
}

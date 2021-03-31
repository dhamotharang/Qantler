using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class LetterUserNameAndCreatorModel
    {
        [DataMember(Name = "UserID")]
        public int? UserID { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
    }
}

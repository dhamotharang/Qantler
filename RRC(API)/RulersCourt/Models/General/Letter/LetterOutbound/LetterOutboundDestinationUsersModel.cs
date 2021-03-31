using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundDestinationUsersModel
    {
        [DataMember(Name = "LetterDestinationUsersName")]
        public string LetterDestinationUsersName { get; set; }
    }
}

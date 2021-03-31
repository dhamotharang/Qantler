using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter
{
    [DataContract]
    public class LetterOutboundDestinationDepartmentModel
    {
        [DataMember(Name = "LetterDestinationID")]
        public int? LetterDestinationID { get; set; }

        [DataMember(Name = "LetterDestinationEntityID")]
        public int? LetterDestinationEntityID { get; set; }

        [DataMember(Name = "IsGovernmentEntity")]
        public bool? IsGovernmentEntity { get; set; }

        [DataMember(Name = "LetterDestinationEntityName")]
        public string LetterDestinationEntityName { get; set; }

        [DataMember(Name = "LetterDestinationUserName")]
        public string LetterDestinationUserName { get; set; }
    }
}

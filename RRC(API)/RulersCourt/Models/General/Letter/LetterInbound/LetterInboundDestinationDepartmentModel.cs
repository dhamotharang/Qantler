using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter.LetterInbound
{
    [DataContract]
    public class LetterInboundDestinationDepartmentModel
    {
        [DataMember(Name = "LetterDestinationDepartmentID")]
        public int? LetterDestinationDepartmentID { get; set; }

        [DataMember(Name = "LetterDestinationDepartmentName")]
        public string LetterDestinationDepartmentName { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentLegalAssigneeModel
    {
        [DataMember(Name = "AssigneeId")]
        public int? AssigneeId { get; set; }
    }
}

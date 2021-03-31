using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentMediaAssigneeModel
    {
        [DataMember(Name = "AssigneeId")]
        public int? AssigneeId { get; set; }
    }
}

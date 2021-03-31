using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentCitizenAffairAssigneeModel
    {
        [DataMember(Name = "AssigneeId")]
        public int? AssigneeId { get; set; }
    }
}

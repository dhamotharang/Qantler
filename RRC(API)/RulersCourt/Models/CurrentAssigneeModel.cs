using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentAssigneeModel
    {
        [DataMember(Name = "AssigneeId")]
        public int? AssigneeId { get; set; }
    }
}
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class CurrentApproverModel
    {
        [DataMember(Name = "ApproverId")]
        public int? ApproverId { get; set; }
    }
}

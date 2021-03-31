using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class GovernmentEntityModel
    {
        [DataMember(Name = "ID")]
        public int? ID { get; set; }

        [DataMember(Name = "EntityName")]
        public string EntityName { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
    }
}

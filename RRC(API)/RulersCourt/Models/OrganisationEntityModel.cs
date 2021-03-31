using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class OrganisationEntityModel
    {
        [DataMember(Name = "ContactID")]
        public int? ContactID { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "UserID")]
        public string UserID { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class OrganizationModel
    {
        [DataMember(Name = "OrganizationID")]
        public int? OrganizationID { get; set; }

        [DataMember(Name = "OrganizationUnits")]
        public string OrganizationUnits { get; set; }
    }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class ApproverDeparmentModel
    {
        [DataMember(Name = "OrganizationID")]
        public int? OrganizationID { get; set; }

        [DataMember(Name = "OrganizationUnits")]
        public string OrganizationUnits { get; set; }
    }
}
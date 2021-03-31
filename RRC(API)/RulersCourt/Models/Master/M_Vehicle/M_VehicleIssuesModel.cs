using System.Runtime.Serialization;

namespace RulersCourt.Models.Master.M_Vehicle
{
    [DataContract]
    public class M_VehicleIssuesModel
    {
        [DataMember(Name = "IssueID")]
        public int? IssueID { get; set; }

        [DataMember(Name = "IssueName")]
        public string IssueName { get; set; }

        [DataMember(Name = "DisplayOrder")]
        public string DisplayOrder { get; set; }
    }
}

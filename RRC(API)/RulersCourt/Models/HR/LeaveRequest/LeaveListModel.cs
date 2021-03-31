using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.LeaveRequest
{
    [DataContract]
    public class LeaveListModel
    {
        [DataMember(Name = "Collection")]
        public List<LeaveGetModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_OrganizationList")]
        public List<OrganizationModel> OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> LookupsList { get; set; }
    }
}

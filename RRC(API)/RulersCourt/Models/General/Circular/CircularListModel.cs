using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Circular
{
    [DataContract]
    public class CircularListModel
    {
        [DataMember(Name = "Collection")]
        public List<CircularGetModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_OrganizationList")]
        public List<OrganizationModel> OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> LookupsList { get; set; }
    }
}

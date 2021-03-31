using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR
{
    [DataContract]
    public class MaintenanceHomeListModel
    {
        [DataMember(Name = "Collection")]
        public List<MaintenanceHomeDashboardListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> LookupsList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }
    }
}

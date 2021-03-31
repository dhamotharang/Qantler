using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Letter.LetterInbound
{
    [DataContract]
    public class LetterInboundListModel
    {
        [DataMember(Name = "Collection")]
        public List<LetterInboundDashboardListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_OrganizationList")]
        public List<OrganizationModel> OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> LookupsList { get; set; }

        [DataMember(Name = "Creator")]
        public List<LetterUserNameAndCreatorModel> Creator { get; set; }

        [DataMember(Name = "UserName")]
        public List<LetterUserNameAndCreatorModel> UserName { get; set; }
    }
}

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.HR.UserProfile
{
    [DataContract]
    public class UserProfileListModel
    {
        [DataMember(Name = "Collection")]
        public List<UserProfileDashBoardListModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_OrganizationList")]
        public List<OrganizationModel> OrganizationList { get; set; }
    }
}

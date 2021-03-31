using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftListModel
    {
        [DataMember(Name = "Collection")]
        public List<GiftModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }
    }
}

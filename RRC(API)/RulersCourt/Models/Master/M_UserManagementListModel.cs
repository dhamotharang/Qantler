using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class M_UserManagementListModel
    {
        [DataMember(Name = "Collection")]
        public List<M_UserManagementModel> Collection { get; set; }

        [DataMember(Name = "Count")]
        public string Count { get; set; }
    }
}

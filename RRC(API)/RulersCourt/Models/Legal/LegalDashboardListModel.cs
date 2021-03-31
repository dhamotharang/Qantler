using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Legal
{
    [DataContract]
    public class LegalDashboardListModel
    {
        [DataMember(Name = "LegalID")]
        public int? LegalID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime? RequestDate { get; set; }

        [DataMember(Name = "Attendedby")]
        public string Attendedby { get; set; }

        [DataMember(Name = "AssignedTo")]
        public string AssignedTo { get; set; }
    }
}

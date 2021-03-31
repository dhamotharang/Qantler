using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.ITSupport
{
    [DataContract]
    public class ITSupportReportModel
    {
        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "RequestDateFrom")]
        public DateTime? RequestDateFrom { get; set; }

        [DataMember(Name = "RequestDateTo")]
        public DateTime? RequestDateTo { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID")]
        public string UserID { get; set; }
    }
}

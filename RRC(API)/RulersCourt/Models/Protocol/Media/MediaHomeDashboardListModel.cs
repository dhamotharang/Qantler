using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol.Media
{
    [DataContract]
    public class MediaHomeDashboardListModel
    {
        [DataMember(Name = "RefID")]
        public string RefID { get; set; }

        [DataMember(Name = "RequestID")]
        public int? RequestID { get; set; }

        [DataMember(Name = "Source")]
        public string Source { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime? RequestDate { get; set; }

        [DataMember(Name = "CreatorID")]
        public int? CreatorID { get; set; }

        [DataMember(Name = "AssignedTo")]
        public string AssignedTo { get; set; }
    }
}

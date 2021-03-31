using System.Runtime.Serialization;

namespace RulersCourt.Models.HR
{
    [DataContract]
    public class HRReportRequestModel
    {
        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "ReqDateFrom")]
        public string ReqDateFrom { get; set; }

        [DataMember(Name = "ReqDateTo")]
        public string ReqDateTo { get; set; }

        [DataMember(Name = "SmartSearch")]
        public string SmartSearch { get; set; }

        [DataMember(Name = "UserID")]
        public string UserID { get; set; }
    }
}

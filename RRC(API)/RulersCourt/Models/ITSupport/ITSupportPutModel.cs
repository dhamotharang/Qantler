using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.ITSupport
{
    [DataContract]
    public class ITSupportPutModel
    {
        [DataMember(Name = "ITSupportID")]
        public int? ITSupportID { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "RequestorName")]
        public string RequestorName { get; set; }

        [DataMember(Name = "RequestorDepartment")]
        public string RequestorDepartment { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "RequestType")]
        public string RequestType { get; set; }

        [DataMember(Name = "RequestDetails")]
        public string RequestDetails { get; set; }

        [DataMember(Name = "Priority")]
        public int? Priority { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "Attachments")]
        public List<ITSupportAttachmentGetModel> Attachments { get; set; }
    }
}

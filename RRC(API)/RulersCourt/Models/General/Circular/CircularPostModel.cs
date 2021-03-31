using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Circular
{
    [DataContract]
    public class CircularPostModel
    {
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "ApproverId")]
        public int? ApproverId { get; set; }

        [DataMember(Name = "ApproverDepartmentId")]
        public int? ApproverDepartmentId { get; set; }

        [DataMember(Name = "Details")]
        public string Details { get; set; }

        [DataMember(Name = "Priority")]
        public string Priority { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DestinationDepartmentId")]
        public List<CircularDestinationDepartmentGetModel> DestinationDepartmentId { get; set; }

        [DataMember(Name = "Attachments")]
        public List<CircularAttachmentGetModel> Attachments { get; set; }
    }
}

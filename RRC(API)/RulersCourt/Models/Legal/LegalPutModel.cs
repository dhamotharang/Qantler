using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Legal
{
    [DataContract]
    public class LegalPutModel
    {
        [DataMember(Name = "LegalID")]
        public int? LegalID { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "RequestDetails")]
        public string RequestDetails { get; set; }

        [DataMember(Name = "Keywords")]
        public List<LegalKeywordsModel> Keywords { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        [DataMember(Name = "Attachments")]
        public List<LegalAttachmentGetModel> Attachments { get; set; }
    }
}

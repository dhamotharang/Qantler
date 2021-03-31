using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Design
{
    [DataContract]
    public class DesignPostModel
    {
        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "Project")]
        public string Project { get; set; }

        [DataMember(Name = "DiwansRole")]
        public string DiwansRole { get; set; }

        [DataMember(Name = "OtherParties")]
        public string OtherParties { get; set; }

        [DataMember(Name = "TargetGroups")]
        public string TargetGroups { get; set; }

        [DataMember(Name = "DateofDeliverable")]
        public DateTime? DateofDeliverable { get; set; }

        [DataMember(Name = "TypeofDesignRequired")]
        public string TypeofDesignRequired { get; set; }

        [DataMember(Name = "Languages")]
        public int? Languages { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "GeneralObjective")]
        public string GeneralObjective { get; set; }

        [DataMember(Name = "MainObjective")]
        public string MainObjective { get; set; }

        [DataMember(Name = "StrategicObjective")]
        public string StrategicObjective { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Attachments")]
        public List<DesignAttachmentGetModel> Attachments { get; set; }
    }
}

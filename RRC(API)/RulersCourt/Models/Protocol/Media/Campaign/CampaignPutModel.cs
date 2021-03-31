using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Campaign
{
    [DataContract]
    public class CampaignPutModel
    {
        [DataMember(Name = "CampaignID")]
        public int? CampaignID { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "InitiativeProjectActivity")]
        public string InitiativeProjectActivity { get; set; }

        [DataMember(Name = "CampaignStartDate")]
        public DateTime? CampaignStartDate { get; set; }

        [DataMember(Name = "CampaignPeriod")]
        public string CampaignPeriod { get; set; }

        [DataMember(Name = "DiwansRole")]
        public string DiwansRole { get; set; }

        [DataMember(Name = "OtherEntities")]
        public string OtherEntities { get; set; }

        [DataMember(Name = "TargetGroup")]
        public string TargetGroup { get; set; }

        [DataMember(Name = "TargetAudience")]
        public string TargetAudience { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "Languages")]
        public int? Languages { get; set; }

        [DataMember(Name = "MediaChannels")]
        public int? MediaChannels { get; set; }

        [DataMember(Name = "Notes")]
        public string Notes { get; set; }

        [DataMember(Name = "RequestDetails")]
        public string RequestDetails { get; set; }

        [DataMember(Name = "GeneralInformation")]
        public string GeneralInformation { get; set; }

        [DataMember(Name = "MainObjective")]
        public string MainObjective { get; set; }

        [DataMember(Name = "MainIdea")]
        public string MainIdea { get; set; }

        [DataMember(Name = "StrategicGoals")]
        public string StrategicGoals { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "DeleteFlag")]
        public bool DeleteFlag { get; set; }

        [DataMember(Name = "Attachments")]
        public List<CampaignAttachmentGetModel> Attachments { get; set; }
    }
}

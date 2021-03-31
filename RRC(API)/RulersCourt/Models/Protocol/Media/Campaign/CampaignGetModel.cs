using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Campaign
{
    [DataContract]
    public class CampaignGetModel
    {
        [DataMember(Name = "CampaignID")]
        public int? CampaignID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

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

        [DataMember(Name = "CurrentApprover")]
        public List<CurrentApproverModel> CurrentApprover { get; set; }

        [DataMember(Name = "AssigneeId")]
        public List<CurrentMediaAssigneeModel> AssigneeId { get; set; }

        [DataMember(Name = "MediaHeadUsedId")]
        public List<CurrentMediaHeadModel> MediaHeadUsedId { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "Attachments")]
        public List<CampaignAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_ApproverDepartmentList")]
        public List<ApproverDeparmentModel> M_ApproverDepartmentList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "CampaignCommunicationHistory")]
        public List<CampaignCommunicationHistory> CommunicationHistory { get; set; }
    }
}

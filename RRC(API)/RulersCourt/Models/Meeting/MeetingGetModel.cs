using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingGetModel
    {
        [DataMember(Name = "MeetingID")]
        public int? MeetingID { get; set; }

        [DataMember(Name = "MomID")]
        public int? MomID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "OrganizerDepartmentID")]
        public int? OrganizerDepartmentID { get; set; }

        [DataMember(Name = "OrganizerUserID")]
        public int? OrganizerUserID { get; set; }

        [DataMember(Name = "Subject")]
        public string Subject { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "StartDateTime")]
        public DateTime? StartDateTime { get; set; }

        [DataMember(Name = "EndDateTime")]
        public DateTime? EndDateTime { get; set; }

        [DataMember(Name = "MeetingType")]
        public int? MeetingType { get; set; }

        [DataMember(Name = "IsInternalInvitees")]
        public bool? IsInternalInvitees { get; set; }

        [DataMember(Name = "IsExternalInvitees")]
        public bool? IsExternalInvitees { get; set; }

        [DataMember(Name = "InternalInvitees")]
        public List<MeetingInternalInviteesModel> InternalInvitees { get; set; }

        [DataMember(Name = "ExternalInvitees")]
        public List<MeetingExternalInviteesModel> ExternalInvitiees { get; set; }

        [DataMember(Name = "RemindMeAt")]
        public List<MeetingRemindMeAtModel> RemindMeAt { get; set; }

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
        public List<MeetingAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "MeetingCommunicationHistory")]
        public List<MeetingCommunicationHistoryModel> CommunicationHistory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingPostModel
    {
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
        public List<MeetingExternalInviteesModel> ExternalInvitees { get; set; }

        [DataMember(Name = "RemindMeAt")]
        public List<MeetingRemindMeAtModel> RemindMeAt { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Attachments")]
        public List<MeetingAttachmentGetModel> Attachments { get; set; }
    }
}

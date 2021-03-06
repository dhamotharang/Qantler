using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MOMGetModel
    {
        [DataMember(Name = "MOMID")]
        public int? MOMID { get; set; }

        [DataMember(Name = "MeetingID")]
        public int? MeetingID { get; set; }

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

        [DataMember(Name = "Attendees")]
        public string Attendees { get; set; }

        [DataMember(Name = "StartDateTime")]
        public DateTime? StartDateTime { get; set; }

        [DataMember(Name = "EndDateTime")]
        public DateTime? EndDateTime { get; set; }

        [DataMember(Name = "PointsDiscussed")]
        public string PointsDiscussed { get; set; }

        [DataMember(Name = "PendingPoints")]
        public string PendingPoints { get; set; }

        [DataMember(Name = "Suggestion")]
        public string Suggestion { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "MeetingCommunicationHistory")]
        public List<MeetingCommunicationHistoryModel> CommunicationHistory { get; set; }
    }
}
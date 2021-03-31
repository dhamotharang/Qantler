using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.DutyTasks
{
    [DataContract]
    public class DutyTaskCommunicationHistoryGetModel
    {
        [DataMember(Name = "CommunicationID")]
        public int? CommunicationID { get; set; }

        [DataMember(Name = "TaskID")]
        public int? TaskID { get; set; }

        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentName")]
        public string AttachmentName { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "TaggedUserID")]
        public List<DutyTaskTaggedUserIDModel> TaggedUserID { get; set; }

        [DataMember(Name = "Creator")]
        public string Creator { get; set; }

        [DataMember(Name = "Photo")]
        public string Photo { get; set; }
    }
}

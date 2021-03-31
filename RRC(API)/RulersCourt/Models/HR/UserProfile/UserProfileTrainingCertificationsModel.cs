using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.UserProfile
{
    [DataContract]
    public class UserProfileTrainingCertificationsModel
    {
        [DataMember(Name = "TrainingID")]
        public int? TrainingID { get; set; }

        [DataMember(Name = "TrainingName")]
        public string TrainingName { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "Attachment")]
        public List<UserProfileAttachmentGetModel> Attachment { get; set; }

        [DataMember(Name = "TrainingRequestID")]
        public int? TrainingRequestID { get; set; }
    }
}

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Training
{
    [DataContract]
    public class TrainingWorkflowModel
    {
        [DataMember(Name = "TrainingID")]
        public int? TrainingID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        [DataMember(Name = "CreatorID")]
        public int? CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int? FromID { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "HRHeadUsedID")]
        public List<UserModel> HRHeadUsedID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }

        [DataMember(Name = "TraineeID")]
        public int? TraineeID { get; set; }

        [DataMember(Name = "TraineeFor")]
        public bool? TraineeFor { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficalTaskCompensation.Compensation
{
    [DataContract]
    public class CompensationWorkflowModel
    {
        [DataMember(Name = "CompensationID")]
        public int? CompensationID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }

        [DataMember(Name = "TaskCreatorID")]
        public int? TaskCreatorID { get; set; }

        [DataMember(Name = "EmployeeID")]
        public int? EmployeeID { get; set; }

        [DataMember(Name = "EmployeeDepartmentID")]
        public int? EmployeeDepartmentID { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "HRHeadUsedID")]
        public List<UserModel> HRHeadUsedID { get; set; }

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }
    }
}

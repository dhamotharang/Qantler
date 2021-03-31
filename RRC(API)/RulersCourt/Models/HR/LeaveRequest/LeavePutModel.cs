using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.LeaveRequest
{
    [DataContract]
    public class LeavePutModel
    {
        [DataMember(Name = "LeaveID")]
        public int? LeaveID { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "LeaveType")]
        public string LeaveType { get; set; }

        [DataMember(Name = "LeaveTypeOther")]
        public int? LeaveTypeOther { get; set; }

        [DataMember(Name = "Reason")]
        public string Reason { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "DOANameID")]
        public int? DOANameID { get; set; }

        [DataMember(Name = "DOADepartmentID")]
        public int? DOADepartmentID { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "AssigneeID")]
        public int? AssigneeID { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "BalanceLeave")]
        public string BalanceLeave { get; set; }

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
        public List<LeaveAttachmentGetModel> Attachments { get; set; }

        [DataMember(Name = "HRManagerUserID")]
        public int? HRManagerUserID { get; set; }
    }
}

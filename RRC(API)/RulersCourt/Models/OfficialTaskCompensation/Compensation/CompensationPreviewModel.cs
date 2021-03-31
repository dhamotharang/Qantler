using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficalTaskCompensation.Compensation
{
    [DataContract]
    public class CompensationPreviewModel
    {
        [DataMember(Name = "OfficialTaskReferenceNo")]
        public string OfficialTaskReferenceNo { get; set; }

        [DataMember(Name = "OfficialTaskDescription")]
        public string OfficialTaskDescription { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "OfficialTaskCreatorName")]
        public string OfficialTaskCreatorName { get; set; }

        [DataMember(Name = "OfficialTaskCreatorDesignation")]
        public string OfficialTaskCreatorDesignation { get; set; }

        [DataMember(Name = "AssigneeName")]
        public string AssigneeName { get; set; }

        [DataMember(Name = "AssigneeEmployeeID")]
        public string AssigneeEmployeeID { get; set; }

        [DataMember(Name = "NoOfDays")]
        public int? NoOfDays { get; set; }

        [DataMember(Name = "CompensationReferenceNo")]
        public string CompensationReferenceNo { get; set; }

        [DataMember(Name = "EmployeeDetails")]
        public string EmployeeDetails { get; set; }
    }
}

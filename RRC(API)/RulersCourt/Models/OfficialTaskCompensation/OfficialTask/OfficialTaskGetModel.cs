using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.OfficalTask
{
    [DataContract]
    public class OfficialTaskGetModel
    {
        [DataMember(Name = "OfficialTaskID")]
        public int? OfficialTaskID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "Date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "EmployeeNameID")]
        public List<OfficialTaskEmployeeGetModel> EmployeeNameID { get; set; }

        [DataMember(Name = "OfficialTaskType")]
        public string OfficialTaskType { get; set; }

        [DataMember(Name = "StartDate")]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "EndDate")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "NumberofDays")]
        public int? NumberofDays { get; set; }

        [DataMember(Name = "OfficialTaskDescription")]
        public string OfficialTaskDescription { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "IsCompensationAvailable")]
        public bool? IsCompensationAvailable { get; set; }

        [DataMember(Name = "IsClosed")]
        public bool? IsClosed { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "OfficialTaskCommunicationHistory")]
        public List<OfficialTaskCommunicationHistoryModel> CommunicationHistory { get; set; }
    }
}

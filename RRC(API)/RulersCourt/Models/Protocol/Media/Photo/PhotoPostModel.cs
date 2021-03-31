using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Photo
{
    [DataContract]
    public class PhotoPostModel
    {
        [DataMember(Name = "Date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "SourceOU")]
        public string SourceOU { get; set; }

        [DataMember(Name = "SourceName")]
        public string SourceName { get; set; }

        [DataMember(Name = "EventDate")]
        public DateTime EventDate { get; set; }

        [DataMember(Name = "Location")]
        public string Location { get; set; }

        [DataMember(Name = "EventName")]
        public string EventName { get; set; }

        [DataMember(Name = "PhotoDescription")]
        public string PhotoDescription { get; set; }

        [DataMember(Name = "ApproverID")]
        public int? ApproverID { get; set; }

        [DataMember(Name = "ApproverDepartmentID")]
        public int? ApproverDepartmentID { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }

        [DataMember(Name = "Attachments")]
        public List<PhotoAttachmentGetModel> Attachments { get; set; }
    }
}

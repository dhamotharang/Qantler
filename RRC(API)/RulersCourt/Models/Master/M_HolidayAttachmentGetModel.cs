using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Master
{
    [DataContract]
    public class M_HolidayAttachmentGetModel
    {
        [DataMember(Name = "AttachmentGuid")]
        public string AttachmentGuid { get; set; }

        [DataMember(Name = "AttachmentsName")]
        public string AttachmentsName { get; set; }

        [DataMember(Name = "HolidayID")]
        public int? HolidayID { get; set; }
    }
}

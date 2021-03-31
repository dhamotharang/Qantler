using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftConfirmModel
    {
        [DataMember(Name = "GiftID")]
        public int? GiftID { get; set; }

        [DataMember(Name = "HandedOverTo")]
        public string HandedOverTo { get; set; }

        [DataMember(Name = "HandedOverDate")]
        public DateTime? HandedOverDate { get; set; }

        [DataMember(Name = "Attachment")]
        public List<GiftAttachmentGetModel> Attachment { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }
    }
}

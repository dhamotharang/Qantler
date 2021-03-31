using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Gift
{
    [DataContract]
    public class GiftPutModel
    {
        [DataMember(Name = "GiftID")]
        public int? GiftID { get; set; }

        [DataMember(Name = "GiftPhotos")]
        public List<GiftAttachmentGetModel> GiftPhotos { get; set; }

        [DataMember(Name = "GiftType")]
        public int? GiftType { get; set; }

        [DataMember(Name = "RecievedFromOrganization")]
        public string RecievedFromOrganization { get; set; }

        [DataMember(Name = "RecievedFromName")]
        public string RecievedFromName { get; set; }

        [DataMember(Name = "RecievedDate")]
        public DateTime? RecievedDate { get; set; }

        [DataMember(Name = "PurchasedBy")]
        public int? PurchasedBy { get; set; }

        [DataMember(Name = "PurchasedToName")]
        public string PurchasedToName { get; set; }

        [DataMember(Name = "PurchasedToOrganisation")]
        public string PurchasedToOrganisation { get; set; }

        [DataMember(Name = "Action")]
        public string Action { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "Comments")]
        public string Comments { get; set; }
    }
}

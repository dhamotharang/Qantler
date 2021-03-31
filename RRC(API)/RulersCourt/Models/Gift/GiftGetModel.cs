using RulersCourt.Models;
using RulersCourt.Models.CitizenAffair;
using RulersCourt.Models.Gift;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Repository.Gift
{
    [DataContract]
    public class GiftGetModel
    {
        [DataMember(Name = "GiftID")]
        public int? GiftID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

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

        [DataMember(Name = "IsSend")]
        public bool? IsSend { get; set; }

        [DataMember(Name = "Status")]
        public int? Status { get; set; }

        [DataMember(Name = "CreatedBy")]
        public int? CreatedBy { get; set; }

        [DataMember(Name = "CreatedDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember(Name = "UpdatedBy")]
        public int? UpdatedBy { get; set; }

        [DataMember(Name = "UpdatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [DataMember(Name = "OrganizationList")]
        public List<OrganizationModel> M_OrganizationList { get; set; }

        [DataMember(Name = "M_LookupsList")]
        public List<M_LookupsModel> M_LookupsList { get; set; }

        [DataMember(Name = "HistoryLog")]
        public List<GiftHistoryLogModel> HistoryLog { get; set; }
    }
}

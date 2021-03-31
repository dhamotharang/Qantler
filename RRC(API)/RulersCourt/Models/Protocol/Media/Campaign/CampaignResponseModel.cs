﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol.Media.Campaign
{
    [DataContract]
    public class CampaignResponseModel
    {
        [DataMember(Name = "CampaignID")]
        public int? CampaignID { get; set; }

        [DataMember(Name = "ReferenceNumber")]

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }
    }
}
﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MeetingResponseModel
    {
        [DataMember(Name = "MeetingID")]
        public int? MeetingID { get; set; }

        [DataMember(Name = "MOMID")]
        public int? MOMID { get; set; }

        [DataMember(Name = "ReferenceNumber")]

        [DataMember(Name = "Action")]

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }

        [DataMember(Name = "InternalInvitees")]
        public List<MeetingInternalInviteesModel> InternalInvitees { get; set; }

        [DataMember(Name = "ExternalInvitees")]
        public List<MeetingExternalInviteesModel> ExternalInvitees { get; set; }
    }
}
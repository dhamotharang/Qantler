﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol.Media.PressRelease
{
    [DataContract]
    public class PressReleaseResponseModel
    {
        [DataMember(Name = "PressReleaseID")]
        public int? PressReleaseID { get; set; }

        [DataMember(Name = "ReferenceNumber")]

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }
    }
}
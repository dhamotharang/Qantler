﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Protocol.Media.Design
{
    [DataContract]
    public class DesignResponseModel
    {
        [DataMember(Name = "DesignID")]
        public int? DesignID { get; set; }

        [DataMember(Name = "ReferenceNumber")]

        [DataMember(Name = "CurrentStatus")]
        public int CurrentStatus { get; set; }

        [DataMember(Name = "PreviousApproverID")]
        public int? PreviousApproverID { get; set; }
    }
}
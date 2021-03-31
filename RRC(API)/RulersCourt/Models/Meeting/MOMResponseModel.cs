﻿using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Meeting
{
    [DataContract]
    public class MOMResponseModel
    {
        [DataMember(Name = "MOMID")]
        public int? MOMID { get; set; }

        [DataMember(Name = "Action")]

        [DataMember(Name = "CreatorID")]
        public int CreatorID { get; set; }

        [DataMember(Name = "FromID")]
        public int FromID { get; set; }
    }
}
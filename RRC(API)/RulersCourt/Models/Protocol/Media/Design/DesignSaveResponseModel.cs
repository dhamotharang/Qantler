﻿using System.Runtime.Serialization;

namespace RulersCourt.Models.Design
{
    [DataContract]
    public class DesignSaveResponseModel
    {
        [DataMember(Name = "DesignID")]
        public int? DesignID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
    }
}
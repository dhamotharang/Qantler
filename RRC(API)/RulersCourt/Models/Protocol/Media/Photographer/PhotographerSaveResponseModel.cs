using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Photographer
{
    [DataContract]
    public class PhotographerSaveResponseModel
    {
        [DataMember(Name = "PhotoGrapherID")]
        public int? PhotoGrapherID { get; set; }

        [DataMember(Name = "ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}

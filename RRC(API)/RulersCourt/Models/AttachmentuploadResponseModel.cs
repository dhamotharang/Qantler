using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RulersCourt.Models
{
    [DataContract]
    public class AttachmentuploadResponseModel
    {
        [DataMember(Name = "Guid")]
        public string Guid { get; set; }

        [DataMember(Name = "FileName")]
        public List<string> FileName { get; set; }
    }
}

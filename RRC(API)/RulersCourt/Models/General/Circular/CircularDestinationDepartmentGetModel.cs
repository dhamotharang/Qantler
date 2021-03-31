using System;
using System.Runtime.Serialization;

namespace RulersCourt.Models.Circular
{
    [DataContract]
    public class CircularDestinationDepartmentGetModel
    {
        [DataMember(Name = "CircularDestinationDepartmentID")]
        public int? CircularDestinationDepartmentID { get; set; }

        [DataMember(Name = "CircularDestinationDepartmentName")]
        public string CircularDestinationDepartmentName { get; set; }
  }
}

using System.Runtime.Serialization;

namespace RulersCourt.Models.General.Letter.LetterOutbound
{
    [DataContract]
    public class RecipientUsersModel
    {
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "DepartmentName")]
        public string DepartmentName { get; set; }

        [DataMember(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "ProfilePhotoID")]
        public string ProfilePhotoID { get; set; }

        [DataMember(Name = "ProfilePhotoName")]
        public string ProfilePhotoName { get; set; }
    }
}

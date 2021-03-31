using System.Runtime.Serialization;

namespace RulersCourt.Models.Vehicle.CarCompany
{
    [DataContract]
    public class CarCompanySaveResponseModel
    {
        [DataMember(Name = "CarCompanyID")]
        public int? CarCompanyID { get; set; }
    }
}

using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Threading.Tasks;


namespace Request.API.Services.Commands.Certificate360
{
  public class GetCertByCertNoCommand : IUnitOfWorkCommand<Model.Certificate360>
  {
    readonly string _certNo;
    
    public GetCertByCertNoCommand(string CertificateNo)
    {
      _certNo = CertificateNo;     
    }

    public async Task<Model.Certificate360> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360.GetCertificateByCertNo(_certNo); 
    }

  } 
}

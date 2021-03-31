using System;
using Core.API.Repository;

namespace Identity.API.Repository
{
  public class DbContext
  {
    readonly IUnitOfWork _unitOfWork;

    public static DbContext From(IUnitOfWork unitOfWork)
    {
      return new DbContext(unitOfWork);
    }

    public DbContext(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    IChecklistRepository _checklist;
    public IChecklistRepository Checklist =>
      _checklist ??= new ChecklistRepository(_unitOfWork);

    IClusterRepository _cluster;
    public IClusterRepository Cluster =>
      _cluster ??= new ClusterRepository(_unitOfWork);

    ICustomerRepository _customer;
    public ICustomerRepository Customer =>
      _customer ??= new CustomerRepository(_unitOfWork);

    IIdentityRepository _identity;
    public IIdentityRepository Identity =>
      _identity ??= new IdentityRepository(_unitOfWork);

    ISettingRepository _settings;
    public ISettingRepository Settings =>
      _settings ??= new SettingRepository(_unitOfWork);

    IPersonRepository _person;
    public IPersonRepository Person =>
      _person ??= new PersonRepository(_unitOfWork);

    ITranslationRepository _transaltion;
    public ITranslationRepository Translation =>
      _transaltion ??= new TranslationRepository(_unitOfWork);

    ILogRepository _log;
    public ILogRepository Log =>
      _log ??= new LogRepository(_unitOfWork);

    ICredentialRepository _credential;
    public ICredentialRepository Credential =>
      _credential ??= new CredentialRepository(_unitOfWork);

    IEmailTemplateRepository _emailTemplate;
    public IEmailTemplateRepository EmailTemplate =>
      _emailTemplate ??= new EmailTemplateRepository(_unitOfWork);

    IPremiseRepository _premise;
    public IPremiseRepository Premise =>
      _premise ??= new PremiseRepository(_unitOfWork);
  }
}

using Core.API.Repository;

namespace JobOrder.API.Repository
{
  public class DbContext
  {
    readonly IUnitOfWork _unitOfWork;

    public static DbContext From(IUnitOfWork uow)
    {
      return new DbContext(uow);
    }

    public DbContext(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    IFindingsRepository _findings;
    public IFindingsRepository Findings =>
      _findings ??= new FindingsRepository(_unitOfWork);

    IJobOrderRepository _jobOrder;
    public IJobOrderRepository JobOrder =>
      _jobOrder ??= new JobOrderRepository(_unitOfWork);

    ILogRepository _log;
    public ILogRepository Log =>
      _log ??= new LogRepository(_unitOfWork);

    ISettingsRepository _settings;
    public ISettingsRepository Settings =>
      _settings ??= new SettingsRepository(_unitOfWork);

    ITranslationRepository _translation;
    public ITranslationRepository Translation =>
      _translation ??= new TranslationRepository(_unitOfWork);

    IMasterRepository _master;
    public IMasterRepository Master =>
      _master ??= new MasterRepository(_unitOfWork);

    IUserRepository _user;
    public IUserRepository User =>
      _user ??= new UserRepository(_unitOfWork);

    INotesRepository _notes;
    public INotesRepository Notes =>
      _notes ??= new NotesRepository(_unitOfWork);

    IAttachmentRepository _attachment;
    public IAttachmentRepository Attachment =>
      _attachment ??= new AttachmentRepository(_unitOfWork);

    ICertificateRepository _certificate;
    public ICertificateRepository Certificate =>
      _certificate ??= new CertificateRepository(_unitOfWork);

    IPremiseRepository _premise;
    public IPremiseRepository Premise =>
      _premise ??= new PremiseRepository(_unitOfWork);

    IPeriodicSchedulerRepository _periodicScheduler;
    public IPeriodicSchedulerRepository PeriodicScheduler =>
      _periodicScheduler ??= new PeriodicSchedulerRepository(_unitOfWork);

    ICustomerRepository _customer;
    public ICustomerRepository Customer =>
      _customer ??= new CustomerRepository(_unitOfWork);

    IEmailRepository _email;
    public IEmailRepository Email =>
      _email ??= new EmailRepository(_unitOfWork);

    IPersonRepository _person;
    public IPersonRepository Person =>
      _person ??= new PersonRepository(_unitOfWork);

    IContactInfoRepository _contactInfo;
    public IContactInfoRepository ContactInfo =>
      _contactInfo ??= new ContactInfoRepository(_unitOfWork);
  }
}

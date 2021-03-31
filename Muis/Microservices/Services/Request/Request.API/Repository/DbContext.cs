using Core.API.Repository;

namespace Request.API.Repository
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

    ICertificateRepository _certificate;
    public ICertificateRepository Certificate =>
      _certificate ??= new CertificateRepository(_unitOfWork);

    ICertificate360Repository _certificate360;
    public ICertificate360Repository Certificate360 =>
      _certificate360 ??= new Certificate360Repository(_unitOfWork);

    ICustomerRepository _customer;
    public ICustomerRepository Customer =>
      _customer ??= new CustomerRepository(_unitOfWork);

    IEmailRepository _email;
    public IEmailRepository Email =>
      _email ??= new EmailRepository(_unitOfWork);

    IIngredientRepository _ingredient;
    public IIngredientRepository Ingredient =>
      _ingredient ??= new IngredientRepository(_unitOfWork);

    ILogRepository _log;
    public ILogRepository Log =>
      _log ??= new LogRepository(_unitOfWork);

    IMenuRepository _menu;
    public IMenuRepository Menu =>
      _menu ??= new MenuRepository(_unitOfWork);

    IRequestRepository _request;
    public IRequestRepository Request =>
      _request ??= new RequestRepository(_unitOfWork);

    IRFARepository _rfa;
    public IRFARepository RFA =>
      _rfa ??= new RFARepository(_unitOfWork);

    ISettingsRepository _settings;
    public ISettingsRepository Settings =>
      _settings ??= new SettingsRepository(_unitOfWork);

    ITranslationRepository _translation;
    public ITranslationRepository Transalation =>
      _translation ??= new TranslationRepository(_unitOfWork);

    IMasterRepository _master;
    public IMasterRepository Master =>
      _master ??= new MasterRepository(_unitOfWork);

    ICodeRepository _code;
    public ICodeRepository Code =>
      _code ??= new CodeRepository(_unitOfWork);

    IUserRepository _user;
    public IUserRepository User =>
      _user ??= new UserRepository(_unitOfWork);

    INotesRepository _notes;
    public INotesRepository Notes =>
      _notes ??= new NotesRepository(_unitOfWork);

    IAttachmentRepository _attachment;
    public IAttachmentRepository Attachment =>
      _attachment ??= new AttachmentRepository(_unitOfWork);

    IStatisticsRepository _statistics;
    public IStatisticsRepository Statistics =>
      _statistics ??= new StatisticsRepository(_unitOfWork);

    IPremisesRepository _premises;
    public IPremisesRepository Premises =>
      _premises ??= new PremisesRepository(_unitOfWork);
  }
}

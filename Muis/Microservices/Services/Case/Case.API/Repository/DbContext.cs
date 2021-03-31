using Core.API.Repository;

namespace Case.API.Repository
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

    ICaseRepository _case;
    public ICaseRepository Case =>
      _case ??= new CaseRepository(_unitOfWork);

    IMasterRepository _master;
    public IMasterRepository Master =>
      _master ??= new MasterRepository(_unitOfWork);

    IAttachmentRepository _attachment;
    public IAttachmentRepository Attachment =>
      _attachment ??= new AttachmentRepository(_unitOfWork);

    IUserRepository _user;
    public IUserRepository User =>
      _user ??= new UserRepository(_unitOfWork);

    IReporterRepository _reporter;
    public IReporterRepository Reporter =>
      _reporter ??= new ReporterRepository(_unitOfWork);

    IOffenderRepository _offender;
    public IOffenderRepository Offender =>
      _offender ??= new OffenderRepository(_unitOfWork);

    IPremiseRepository _premise;
    public IPremiseRepository Premise =>
      _premise ??= new PremiseRepository(_unitOfWork);

    ICertificateRepository _certificate;
    public ICertificateRepository Certificate =>
      _certificate ??= new CertificateRepository(_unitOfWork);

    IActivityRepository _activity;
    public IActivityRepository Activity =>
      _activity ??= new ActivityRepository(_unitOfWork);

    ITranslationRepository _translation;
    public ITranslationRepository Translation =>
      _translation ??= new TranslationRepository(_unitOfWork);

    IEmailRepository _email;
    public IEmailRepository Email =>
      _email ??= new EmailRepository(_unitOfWork);

    ILetterRepository _letter;
    public ILetterRepository Letter =>
      _letter ??= new LetterRepository(_unitOfWork);

    ISanctionInfoRepository _sanction;
    public ISanctionInfoRepository Sanction =>
      _sanction ??= new SanctionInfoRepository(_unitOfWork);
  }
}

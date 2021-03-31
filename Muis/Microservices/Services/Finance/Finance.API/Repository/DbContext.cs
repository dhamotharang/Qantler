using System;
using Core.API.Repository;

namespace Finance.API.Repository
{
  public class DbContext : IDisposable
  {
    IUnitOfWork _unitOfWork;

    public static DbContext From(IUnitOfWork uow)
    {
      return new DbContext(uow);
    }

    public DbContext(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    IPaymentRepository _payment;
    public IPaymentRepository Payment =>
      _payment ??= new PaymentRepository(_unitOfWork);

    IBillRepository _bill;
    public IBillRepository Bill =>
      _bill ??= new BillRepository(_unitOfWork);

    ISettingsRepository _settings;
    public ISettingsRepository Settings =>
      _settings ??= new SettingsRepository(_unitOfWork);

    ITransactionCodeRepository _transactionCode;
    public ITransactionCodeRepository Transactioncode =>
      _transactionCode ??= new TransactionCodeRepository(_unitOfWork);

    IUserRepository _user;
    public IUserRepository User =>
      _user ??= new UserRepository(_unitOfWork);

    INotesRepository _note;
    public INotesRepository Note =>
      _note ??= new NotesRepository(_unitOfWork);

    IAttachmentRepository _attachment;
    public IAttachmentRepository Attachment =>
      _attachment ??= new AttachmentRepository(_unitOfWork);

    ITranslationRepository _translation;
    public ITranslationRepository Translation =>
      _translation ??= new TranslationRepository(_unitOfWork);

    ILogRepository _log;
    public ILogRepository Log =>
      _log ??= new LogRepository(_unitOfWork);

    IAccountRepository _account;
    public IAccountRepository Account =>
      _account ??= new AccountRepository(_unitOfWork);

    IPersonRepository _person;
    public IPersonRepository Person =>
      _person ??= new PersonRepository(_unitOfWork);

    IContactInfoRepository _contactInfo;
    public IContactInfoRepository ContactInfo =>
      _contactInfo ??= new ContactInfoRepository(_unitOfWork);

    IBankRepository _bank;
    public IBankRepository Bank =>
      _bank ??= new BankRepository(_unitOfWork);

    IMasterRepository _master;
    public IMasterRepository Master =>
      _master ??= new MasterRepository(_unitOfWork);

    public void Dispose()
    {
      _unitOfWork = null;
    }
  }
}

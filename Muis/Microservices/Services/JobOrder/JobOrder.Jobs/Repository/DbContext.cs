using Core.Base.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Jobs.Repository
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

    IPeriodicSchedulerRepository _periodicScheduler;
    public IPeriodicSchedulerRepository PeriodicScheduler =>
      _periodicScheduler ??= new PeriodicSchedulerRepository(_unitOfWork);

    IJobOrderRepository _jobOrder;
    public IJobOrderRepository JobOrder =>
      _jobOrder ??= new JobOrderRepository(_unitOfWork);

    ICertificateRepository _certificate;
    public ICertificateRepository Certificate =>
      _certificate ??= new CertificateRepository(_unitOfWork);

    ITranslationRepository _translation;
    public ITranslationRepository Translation =>
      _translation ??= new TranslationRepository(_unitOfWork);

    ILogRepository _log;
    public ILogRepository Log =>
      _log ??= new LogRepository(_unitOfWork);
  }
}

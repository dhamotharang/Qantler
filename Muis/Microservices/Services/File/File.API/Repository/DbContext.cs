using System;
using Core.API.Repository;

namespace File.API.Repository
{
  public class DbContext
  {
    readonly IUnitOfWork _unitOfWork;

    public DbContext(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    IFileRepository _file;
    public IFileRepository File => _file ??= new FileRepository(_unitOfWork);
  }
}

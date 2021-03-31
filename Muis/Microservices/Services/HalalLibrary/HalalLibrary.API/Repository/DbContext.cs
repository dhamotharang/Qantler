using Core.API.Repository;

namespace HalalLibrary.API.Repository
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

    IIngredientRepository _ingredient;
    public IIngredientRepository Ingredient =>
      _ingredient ??= new IngredientRepository(_unitOfWork);

    ISupplierRepository _supplier;
    public ISupplierRepository Supplier =>
      _supplier ??= new SupplierRepository(_unitOfWork);

    ICertifyingBodyRepository _certifyingbody;
    public ICertifyingBodyRepository CertifyingBody =>
      _certifyingbody ??= new CertifyingBodyRepository(_unitOfWork);

    ITranslationRepository _transaltion;
    public ITranslationRepository Translation =>
      _transaltion ??= new TranslationRepository(_unitOfWork);
  }
}

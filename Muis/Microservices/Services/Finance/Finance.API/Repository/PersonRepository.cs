using Core.API.Repository;
using Dapper;
using Finance.API.Repository.Mappers;
using Finance.Model;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public class PersonRepository : IPersonRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PersonRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task Insert(Person entity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", entity.ID);
      param.Add("@Name", entity.Name);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertPerson",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task Update(Person entity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", entity.ID);
      param.Add("@Name", entity.Name);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdatePerson",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<Person> GetByID(Guid id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var personMapper = new PersonMapper();

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetPersonByID",
        new[]
        {
          typeof(Person),
          typeof(ContactInfo)
        },
        obj =>
        {
          var person = obj[0] as Person;
          var contactinfo = obj[1] as ContactInfo;

          return personMapper.Map(person, contactinfo);
        },
        param,
        splitOn: "ID,ContactID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction))?.FirstOrDefault();
    }

    public async Task MapContactInfo(Guid personID, long contactID)
    {
      var param = new DynamicParameters();
      param.Add("@PersonID", personID);
      param.Add("@ContactID", contactID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapPersonContactInfo",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task RemoveContactInfo(Guid personID, long contactID)
    {
      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        $"DELETE FROM [PersonContacts] WHERE [PersonID] = @PersonID AND [ContactID] = @ContactID",
        new
        {
          PersonID = personID,
          ContactID = contactID
        },
        transaction: _unitOfWork.Transaction);
    }
  }
}

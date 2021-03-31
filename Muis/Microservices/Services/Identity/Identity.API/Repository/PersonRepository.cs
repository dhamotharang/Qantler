using Core.API.Repository;
using Dapper;
using Identity.API.Repository.Converters;
using Identity.API.Repository.Mappers;
using Identity.Model;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public class PersonRepository : IPersonRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PersonRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Person> GetPersonByAltID(string altID)
    {
      await Task.CompletedTask;

      var param = new DynamicParameters();
      param.Add("@AltID", altID);

      return SqlMapper.QueryFirst<Person>(
        _unitOfWork.Connection,
        "GetPerson",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<bool> InsertPerson(Person person)
    {
      await Task.CompletedTask;

      var dr = DataConverter.ToPerson(person);
      var ci = DataConverter.ToContactInfo(person);

      var param = new DynamicParameters();

      param.Add("@Person", dr.AsTableValuedParameter("dbo.PersonType"));

      param.Add("@ContactInfo",
        ci.AsTableValuedParameter("dbo.ContactInfoType"));

      SqlMapper.Execute(_unitOfWork.Connection,
        "InsertPerson",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return true;

    }

    public async Task<bool> UpdatePerson(Person person)
    {
      await Task.CompletedTask;

      var dr = DataConverter.ToPerson(person);
      var ci = DataConverter.ToContactInfo(person);

      var param = new DynamicParameters();

      param.Add("@Person", dr.AsTableValuedParameter("dbo.PersonType"));

      param.Add("@ContactInfo",
        ci.AsTableValuedParameter("dbo.ContactInfoType"));

      SqlMapper.Execute(_unitOfWork.Connection,
        "UpdatePerson",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return true;

    }

    public bool ValidatePersonStatus(string altID)
    {
      var param = new DynamicParameters();
      param.Add("@AltID", altID);
      param.Add(
        "@valid",
        dbType: DbType.Boolean,
        direction: ParameterDirection.Output);

      SqlMapper.Execute(_unitOfWork.Connection,
        "ValidatePersonStatus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<bool>("@valid");

    }

    public async Task<Person> GetPersonByID(Guid id)
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
  }
}

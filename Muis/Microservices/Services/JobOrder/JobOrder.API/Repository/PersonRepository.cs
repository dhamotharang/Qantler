using Core.API.Repository;
using Dapper;
using JobOrder.API.Repository.Mappers;
using JobOrder.Model;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public class PersonRepository : IPersonRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PersonRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task InsertPerson(Person person)
    {
      var param = new DynamicParameters();
      param.Add("@ID", person.ID);
      param.Add("@Salutation", person.Salutation);
      param.Add("@Name", person.Name);
      param.Add("@DOB", person.DOB);
      param.Add("@Designation", person.Designation);
      param.Add("@DesignationOther", person.DesignationOther);
      param.Add("@IDType", person.IDType);
      param.Add("@AltID", person.AltID);
      param.Add("@PassportIssuingCountry", person.PassportIssuingCountry);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertPerson",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
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

    public async Task MapContactInfoToPerson(Guid personID, long contactID)
    {
      var param = new DynamicParameters();
      param.Add("@PersonID", personID);
      param.Add("@ContactID", contactID);      

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapContactInfoToPerson",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}

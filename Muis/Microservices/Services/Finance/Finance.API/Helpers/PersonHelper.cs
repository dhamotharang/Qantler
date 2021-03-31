using System;
using System.Linq;
using System.Threading.Tasks;
using Finance.API.Repository;
using Finance.Model;

namespace Finance.API.Helpers
{
  public static class PersonHelper
  {
    public static async Task InsertOrReplace (DbContext dbContext, Person person)
    {
      if (person == null)
      {
        return;
      }

      var existing = await dbContext.Person.GetByID(person.ID);
      if (existing != null)
      {
        await dbContext.Person.Update(person);

        if (existing.ContactInfos?.Any() ?? false)
        {
          foreach (var contactInfo in existing.ContactInfos)
          {
            await dbContext.Person.RemoveContactInfo(person.ID, contactInfo.ID);

            await dbContext.ContactInfo.DeleteByID(contactInfo.ID);
          }
        }
      }
      else
      {
        await dbContext.Person.Insert(person);
      }

      if (person.ContactInfos?.Any() ?? false)
      {
        foreach (var contactInfo in person.ContactInfos)
        {
          contactInfo.ID = await dbContext.ContactInfo.Insert(contactInfo);

          await dbContext.Person.MapContactInfo(person.ID, contactInfo.ID);
        }
      }
    }
  }
}

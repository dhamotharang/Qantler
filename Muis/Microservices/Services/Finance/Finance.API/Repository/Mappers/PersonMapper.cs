﻿using Finance.Model;
using System;
using System.Collections.Generic;

namespace Finance.API.Repository.Mappers
{
  public class PersonMapper
  {
    readonly Dictionary<Guid,Person> _personCache
      = new Dictionary<Guid, Person>();

    public Person Map(Person person, ContactInfo contact)
    {
      if (!_personCache.TryGetValue(person.ID, out Person result))
      {
        person.ContactInfos = new List<ContactInfo>();
        
        _personCache[person.ID] = person;
        result = person;
      }

      if (contact.ID != 0)
      {
        result.ContactInfos.Add(contact);
      }

      return result;
    }

  }
}
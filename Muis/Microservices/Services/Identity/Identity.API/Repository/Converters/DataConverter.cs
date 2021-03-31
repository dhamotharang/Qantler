
﻿using Identity.Model;
﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository.Converters
{
  public class DataConverter
  {
    public static DataTable ToPerson(Person person)
    {
      DataTable dr = new DataTable();

      dr.Columns.Add("ID", typeof(Guid));
      dr.Columns.Add("Salutation", typeof(string));
      dr.Columns.Add("Name", typeof(string));
      dr.Columns.Add("Nationality", typeof(string));
      dr.Columns.Add("Gender", typeof(string));
      dr.Columns.Add("DOB", typeof(DateTimeOffset));
      dr.Columns.Add("Designation", typeof(string));
      dr.Columns.Add("DesignationOther", typeof(string));
      dr.Columns.Add("IDType", typeof(int));
      dr.Columns.Add("AltID", typeof(string));
      dr.Columns.Add("PassportIssuingCountry", typeof(string));
      dr.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dr.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      dr.Rows.Add(
        person.ID,
        person.Salutation,
        person.Name,
        person.Nationality,
        person.Gender,
        person.DOB,
        person.Designation,
        person.DesignationOther,
        person.IDType,
        person.AltID,
        person.PassportIssuingCountry,
        person.CreatedOn.ToUniversalTime(),
        person.ModifiedOn.ToUniversalTime()

        );

      return dr;
    }

    public static DataTable ToContactInfo(Person person)
    {
      DataTable dr = new DataTable();

      dr.Columns.Add("Type", typeof(int));
      dr.Columns.Add("Value", typeof(string));
      dr.Columns.Add("IsPrimary", typeof(bool));
      dr.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dr.Columns.Add("ModifiedOn", typeof(DateTimeOffset));
      dr.Columns.Add("PersonID", typeof(Guid));

      if (person.ContactInfos != null)
      {
        foreach (ContactInfo item in person.ContactInfos)
        {
          dr.Rows.Add(
            item.Type,
            item.Value,
            item.IsPrimary,
            item.CreatedOn.ToUniversalTime(),
            item.ModifiedOn.ToUniversalTime(),
            item.PersonID);
        }
      }
      return dr;
    }
  }
}

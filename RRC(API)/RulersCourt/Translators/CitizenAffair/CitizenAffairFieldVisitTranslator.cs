using RulersCourt.Models.CitizenAffair;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class CitizenAffairFieldVisitTranslator
    {
        public static CitizenAffairFieldVisitModel TranslateAsGetCitizenAffairFieldVisit(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var fieldVisit = new CitizenAffairFieldVisitModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                fieldVisit.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("Date"))
            {
                fieldVisit.Date = SqlHelper.GetDateTime(reader, "Date");
            }

            if (reader.IsColumnExists("Location"))
            {
                fieldVisit.Location = SqlHelper.GetNullableInt32(reader, "Location");
            }

            if (reader.IsColumnExists("RequetsedBy"))
            {
                fieldVisit.RequetsedBy = SqlHelper.GetNullableString(reader, "RequetsedBy");
            }

            if (reader.IsColumnExists("VisitObjective"))
            {
                fieldVisit.VisitObjective = SqlHelper.GetNullableString(reader, "VisitObjective");
            }

            if (reader.IsColumnExists("FindingNotes"))
            {
                fieldVisit.FindingNotes = SqlHelper.GetNullableString(reader, "FindingNotes");
            }

            if (reader.IsColumnExists("ForWhom"))
            {
                fieldVisit.ForWhom = SqlHelper.GetNullableString(reader, "ForWhom");
            }

            if (reader.IsColumnExists("EmiratesID"))
            {
                fieldVisit.EmiratesID = SqlHelper.GetNullableString(reader, "EmiratesID");
            }

            if (reader.IsColumnExists("Name"))
            {
                fieldVisit.Name = SqlHelper.GetNullableString(reader, "Name");
            }

            if (reader.IsColumnExists("PhoneNumber"))
            {
                fieldVisit.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");
            }

            if (reader.IsColumnExists("City"))
            {
                fieldVisit.City = SqlHelper.GetNullableInt32(reader, "City");
            }

            if (reader.IsColumnExists("LocationName"))
            {
                fieldVisit.LocationName = SqlHelper.GetNullableString(reader, "LocationName");
            }

            if (reader.IsColumnExists("CityID"))
            {
                fieldVisit.CityID = SqlHelper.GetNullableInt32(reader, "CityID");
            }

            if (reader.IsColumnExists("Emirates"))
            {
                fieldVisit.Emirates = SqlHelper.GetNullableInt32(reader, "Emirates");
            }

            return fieldVisit;
        }

        public static CitizenAffairFieldVisitModel TranslateAsCitizenAffairFieldVisitList(this SqlDataReader reader)
        {
            var fieldVisitList = new CitizenAffairFieldVisitModel();
            while (reader.Read())
            {
                fieldVisitList = TranslateAsGetCitizenAffairFieldVisit(reader, true);
            }

            return fieldVisitList;
        }

        public static CitizenAffairFieldVisitModel TranslateAsPatchCitizenAffairFieldVisit(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var fieldVisit = new CitizenAffairFieldVisitModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                fieldVisit.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("FieldVisitCity"))
            {
                fieldVisit.City = SqlHelper.GetNullableInt32(reader, "FieldVisitCity");
            }

            if (reader.IsColumnExists("FieldVisitDate"))
            {
                fieldVisit.Date = SqlHelper.GetDateTime(reader, "FieldVisitDate");
            }

            if (reader.IsColumnExists("FieldVisitEmiratesID"))
            {
                fieldVisit.EmiratesID = SqlHelper.GetNullableString(reader, "FieldVisitEmiratesID");
            }

            if (reader.IsColumnExists("FieldVisitFindingNotes"))
            {
                fieldVisit.FindingNotes = SqlHelper.GetNullableString(reader, "FieldVisitFindingNotes");
            }

            if (reader.IsColumnExists("FieldVisitForWhom"))
            {
                fieldVisit.ForWhom = SqlHelper.GetNullableString(reader, "FieldVisitForWhom");
            }

            if (reader.IsColumnExists("FieldVisitLocation"))
            {
                fieldVisit.Location = SqlHelper.GetNullableInt32(reader, "FieldVisitLocation");
            }

            if (reader.IsColumnExists("Emirates"))
            {
                fieldVisit.Emirates = SqlHelper.GetNullableInt32(reader, "Emirates");
            }

            if (reader.IsColumnExists("CityID"))
            {
                fieldVisit.CityID = SqlHelper.GetNullableInt32(reader, "CityID");
            }

            if (reader.IsColumnExists("LocationName"))
            {
                fieldVisit.LocationName = SqlHelper.GetNullableString(reader, "LocationName");
            }

            if (reader.IsColumnExists("FieldVisitName"))
            {
                fieldVisit.Name = SqlHelper.GetNullableString(reader, "FieldVisitName");
            }

            if (reader.IsColumnExists("FieldVisitPhoneNumber"))
            {
                fieldVisit.PhoneNumber = SqlHelper.GetNullableString(reader, "FieldVisitPhoneNumber");
            }

            if (reader.IsColumnExists("FieldVisitRequestedBy"))
            {
                fieldVisit.RequetsedBy = SqlHelper.GetNullableString(reader, "FieldVisitRequestedBy");
            }

            if (reader.IsColumnExists("FieldVisitObjective"))
            {
                fieldVisit.VisitObjective = SqlHelper.GetNullableString(reader, "FieldVisitObjective");
            }

            return fieldVisit;
        }

        public static CitizenAffairFieldVisitModel TranslatePatchCitizenAffairFieldVisitList(this SqlDataReader reader)
        {
            var fieldVisitList = new CitizenAffairFieldVisitModel();
            while (reader.Read())
            {
                fieldVisitList = TranslateAsPatchCitizenAffairFieldVisit(reader, true);
            }

            return fieldVisitList;
        }
    }
}

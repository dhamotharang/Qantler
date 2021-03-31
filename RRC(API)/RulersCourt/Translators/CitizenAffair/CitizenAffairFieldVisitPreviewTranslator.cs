using RulersCourt.Models.CitizenAffair;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class CitizenAffairFieldVisitPreviewTranslator
    {
        public static CitizenAffairFieldVisitPreviewModel TranslateAsGetCitizenAffairFieldVisit(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var fieldVisit = new CitizenAffairFieldVisitPreviewModel();

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
                fieldVisit.Location = SqlHelper.GetNullableString(reader, "Location");
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
                fieldVisit.City = SqlHelper.GetNullableString(reader, "City");
            }

            if (reader.IsColumnExists("LocationName"))
            {
                fieldVisit.LocationName = SqlHelper.GetNullableString(reader, "LocationName");
            }

            if (reader.IsColumnExists("LocationID"))
            {
                fieldVisit.LocationID = SqlHelper.GetNullableInt32(reader, "LocationID");
            }

            if (reader.IsColumnExists("LocationEmirites"))
            {
                fieldVisit.LocationEmirites = SqlHelper.GetNullableString(reader, "LocationEmirites");
            }

            return fieldVisit;
        }

        public static CitizenAffairFieldVisitPreviewModel TranslateAsCitizenAffairFieldVisitPreviewList(this SqlDataReader reader)
        {
            var fieldVisitList = new CitizenAffairFieldVisitPreviewModel();
            while (reader.Read())
            {
                fieldVisitList = TranslateAsGetCitizenAffairFieldVisit(reader, true);
            }

            return fieldVisitList;
        }

        public static CitizenAffairFieldVisitPreviewModel TranslateAsPatchCitizenAffairFieldVisit(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var fieldVisit = new CitizenAffairFieldVisitPreviewModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                fieldVisit.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("FieldVisitCity"))
            {
                fieldVisit.City = SqlHelper.GetNullableString(reader, "FieldVisitCity");
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
                fieldVisit.Location = SqlHelper.GetNullableString(reader, "FieldVisitLocation");
            }

            if (reader.IsColumnExists("FieldVisitLocationID"))
            {
                fieldVisit.LocationID = SqlHelper.GetNullableInt32(reader, "FieldVisitLocationID");
            }

            if (reader.IsColumnExists("FieldVisitLocationName"))
            {
                fieldVisit.LocationName = SqlHelper.GetNullableString(reader, "FieldVisitLocationName");
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

        public static CitizenAffairFieldVisitPreviewModel TranslatePatchCitizenAffairFieldVisitPreviewList(this SqlDataReader reader)
        {
            var fieldVisitList = new CitizenAffairFieldVisitPreviewModel();
            while (reader.Read())
            {
                fieldVisitList = TranslateAsPatchCitizenAffairFieldVisit(reader, true);
            }

            return fieldVisitList;
        }
    }
}
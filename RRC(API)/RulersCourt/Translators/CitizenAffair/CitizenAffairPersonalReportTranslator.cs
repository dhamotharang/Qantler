using RulersCourt.Models.CitizenAffair;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class CitizenAffairPersonalReportTranslator
    {
        public static CitizenAffairPersonalReportModel TranslateAsGetPersonalReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var personalReport = new CitizenAffairPersonalReportModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                personalReport.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("ProfilePhotoID"))
            {
                personalReport.ProfilePhotoID = SqlHelper.GetNullableString(reader, "ProfilePhotoID");
            }

            if (reader.IsColumnExists("ProfilePhotoName"))
            {
                personalReport.ProfilePhotoName = SqlHelper.GetNullableString(reader, "ProfilePhotoName");
            }

            if (reader.IsColumnExists("Name"))
            {
                personalReport.Name = SqlHelper.GetNullableString(reader, "Name");
            }

            if (reader.IsColumnExists("Employer"))
            {
                personalReport.Employer = SqlHelper.GetNullableString(reader, "Employer");
            }

            if (reader.IsColumnExists("Destination"))
            {
                personalReport.Destination = SqlHelper.GetNullableString(reader, "Destination");
            }

            if (reader.IsColumnExists("MonthlySalary"))
            {
                personalReport.MonthlySalary = SqlHelper.GetNullableString(reader, "MonthlySalary");
            }

            if (reader.IsColumnExists("EmiratesID"))
            {
                personalReport.EmiratesID = SqlHelper.GetNullableString(reader, "EmiratesID");
            }

            if (reader.IsColumnExists("MaritalStatus"))
            {
                personalReport.MaritalStatus = SqlHelper.GetNullableString(reader, "MaritalStatus");
            }

            if (reader.IsColumnExists("NoOfChildrens"))
            {
                personalReport.NoOfChildrens = SqlHelper.GetNullableString(reader, "NoOfChildrens");
            }

            if (reader.IsColumnExists("PhoneNumber"))
            {
                personalReport.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");
            }

            if (reader.IsColumnExists("Emirates"))
            {
                personalReport.Emirates = SqlHelper.GetNullableInt32(reader, "Emirates");
            }

            if (reader.IsColumnExists("City"))
            {
                personalReport.City = SqlHelper.GetNullableInt32(reader, "City");
            }

            if (reader.IsColumnExists("Age"))
            {
                personalReport.Age = SqlHelper.GetNullableString(reader, "Age");
            }

            if (reader.IsColumnExists("ReportObjectives"))
            {
                personalReport.ReportObjectives = SqlHelper.GetNullableString(reader, "ReportObjectives");
            }

            if (reader.IsColumnExists("FindingNotes"))
            {
                personalReport.FindingNotes = SqlHelper.GetNullableString(reader, "FindingNotes");
            }

            if (reader.IsColumnExists("Recommendation"))
            {
                personalReport.Recommendation = SqlHelper.GetNullableString(reader, "Recommendation");
            }

            if (reader.IsColumnExists("PersonalProfilePhotoID"))
            {
                personalReport.ProfilePhotoID = SqlHelper.GetNullableString(reader, "PersonalProfilePhotoID");
            }

            if (reader.IsColumnExists("PersonalProfileName"))
            {
                personalReport.ProfilePhotoName = SqlHelper.GetNullableString(reader, "PersonalProfileName");
            }

            return personalReport;
        }

        public static CitizenAffairPersonalReportModel TranslateAsPersonalReportList(this SqlDataReader reader)
        {
            var personalReportList = new CitizenAffairPersonalReportModel();
            while (reader.Read())
            {
                personalReportList = TranslateAsGetPersonalReport(reader, true);
            }

            return personalReportList;
        }

        public static CitizenAffairPersonalReportModel TranslateAsPatchPersonalReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var personalReport = new CitizenAffairPersonalReportModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                personalReport.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("PersonalReportAge"))
            {
                personalReport.Age = SqlHelper.GetNullableString(reader, "PersonalReportAge");
            }

            if (reader.IsColumnExists("PersonalReportCity"))
            {
                personalReport.City = SqlHelper.GetNullableInt32(reader, "PersonalReportCity");
            }

            if (reader.IsColumnExists("PersonalReportDestination"))
            {
                personalReport.Destination = SqlHelper.GetNullableString(reader, "PersonalReportDestination");
            }

            if (reader.IsColumnExists("PersonalReportEmirates"))
            {
                personalReport.Emirates = SqlHelper.GetNullableInt32(reader, "PersonalReportEmirates");
            }

            if (reader.IsColumnExists("PersonalReportEmirateID"))
            {
                personalReport.EmiratesID = SqlHelper.GetNullableString(reader, "PersonalReportEmirateID");
            }

            if (reader.IsColumnExists("PersonalReportEmployer"))
            {
                personalReport.Employer = SqlHelper.GetNullableString(reader, "PersonalReportEmployer");
            }

            if (reader.IsColumnExists("PersonalReportFindingNotes"))
            {
                personalReport.FindingNotes = SqlHelper.GetNullableString(reader, "PersonalReportFindingNotes");
            }

            if (reader.IsColumnExists("PersonalReportMaritalStatus"))
            {
                personalReport.MaritalStatus = SqlHelper.GetNullableString(reader, "PersonalReportMaritalStatus");
            }

            if (reader.IsColumnExists("PersonalReportMonthlySalary"))
            {
                personalReport.MonthlySalary = SqlHelper.GetNullableString(reader, "PersonalReportMonthlySalary");
            }

            if (reader.IsColumnExists("PersonalReportName"))
            {
                personalReport.Name = SqlHelper.GetNullableString(reader, "PersonalReportName");
            }

            if (reader.IsColumnExists("PersonalReportNoOfChildrens"))
            {
                personalReport.NoOfChildrens = SqlHelper.GetNullableString(reader, "PersonalReportNoOfChildrens");
            }

            if (reader.IsColumnExists("PersonalReportPhoneNumber"))
            {
                personalReport.PhoneNumber = SqlHelper.GetNullableString(reader, "PersonalReportPhoneNumber");
            }

            if (reader.IsColumnExists("PersonalReportRecommendation"))
            {
                personalReport.Recommendation = SqlHelper.GetNullableString(reader, "PersonalReportRecommendation");
            }

            if (reader.IsColumnExists("PersonalReportReportObjectives"))
            {
                personalReport.ReportObjectives = SqlHelper.GetNullableString(reader, "PersonalReportReportObjectives");
            }

            if (reader.IsColumnExists("PersonalProfilePhotoID"))
            {
                personalReport.ProfilePhotoID = SqlHelper.GetNullableString(reader, "PersonalProfilePhotoID");
            }

            if (reader.IsColumnExists("PersonalProfileName"))
            {
                personalReport.ProfilePhotoName = SqlHelper.GetNullableString(reader, "PersonalProfileName");
            }

            return personalReport;
        }

        public static CitizenAffairPersonalReportModel TranslateAsPatchPersonalReportList(this SqlDataReader reader)
        {
            var personalReportList = new CitizenAffairPersonalReportModel();
            while (reader.Read())
            {
                personalReportList = TranslateAsPatchPersonalReport(reader, true);
            }

            return personalReportList;
        }
    }
}

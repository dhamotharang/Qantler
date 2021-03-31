using RulersCourt.Models.CVBank;
using RulersCourt.Models.HR.CVBank;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR.CVBank
{
    public static class CVBankTranslator
    {
        public static CVBankDashBoardListModel CVBankDashBoardListSet(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cVBankModel = new CVBankDashBoardListModel();

            if (reader.IsColumnExists("CVBankId"))
                cVBankModel.CVBankId = SqlHelper.GetNullableInt32(reader, "CVBankId");

            if (reader.IsColumnExists("ReferenceNumber"))
                cVBankModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CandidateName"))
                cVBankModel.CandidateName = SqlHelper.GetNullableString(reader, "CandidateName");

            if (reader.IsColumnExists("Position"))
                cVBankModel.Position = SqlHelper.GetNullableString(reader, "Position");

            if (reader.IsColumnExists("YearsofExperience"))
                cVBankModel.YearsofExperience = SqlHelper.GetNullableString(reader, "YearsofExperience");

            if (reader.IsColumnExists("Specialization"))
                cVBankModel.Specialization = SqlHelper.GetNullableString(reader, "Specialization");

            if (reader.IsColumnExists("Date"))
                cVBankModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("CountryofResidence"))
                cVBankModel.CountryofResidence = SqlHelper.GetNullableString(reader, "CountryofResidence");

            return cVBankModel;
        }

        public static List<CVBankDashBoardListModel> TranslateAsCVBankDashBoardList(this SqlDataReader reader)
        {
            var list = new List<CVBankDashBoardListModel>();
            while (reader.Read())
            {
                list.Add(CVBankDashBoardListSet(reader, true));
            }

            return list;
        }

        public static CVBankCandidateModel TranslateAsGetUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new CVBankCandidateModel();

            if (reader.IsColumnExists("CandidateName"))
                user.CandidateName = SqlHelper.GetNullableString(reader, "CandidateName");

            return user;
        }

        public static List<CVBankCandidateModel> TranslateAsGetUserList(this SqlDataReader reader)
        {
            var userList = new List<CVBankCandidateModel>();
            while (reader.Read())
            {
                userList.Add(TranslateAsGetUser(reader, true));
            }

            return userList;
        }

        public static CVBankGetModel TranslateAsCVBank(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cVBankModel = new CVBankGetModel();

            if (reader.IsColumnExists("CVBankId"))
                cVBankModel.CVBankId = SqlHelper.GetNullableInt32(reader, "CVBankId");

            if (reader.IsColumnExists("ReferenceNumber"))
                cVBankModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CandidateName"))
                cVBankModel.CandidateName = SqlHelper.GetNullableString(reader, "CandidateName");

            if (reader.IsColumnExists("EmailId"))
                cVBankModel.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("JobTitle"))
                cVBankModel.JobTitle = SqlHelper.GetNullableString(reader, "JobTitle");

            if (reader.IsColumnExists("Specialization"))
                cVBankModel.Specialization = SqlHelper.GetNullableInt32(reader, "Specialization");

            if (reader.IsColumnExists("EducationalQualification"))
                cVBankModel.EducationalQualification = SqlHelper.GetNullableInt32(reader, "EducationalQualification");

            if (reader.IsColumnExists("Gender"))
                cVBankModel.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("YearsofExperience"))
                cVBankModel.YearsofExperience = SqlHelper.GetNullableString(reader, "YearsofExperience");

            if (reader.IsColumnExists("AreaofExpertise"))
                cVBankModel.AreaofExpertise = SqlHelper.GetNullableString(reader, "AreaofExpertise");

            if (reader.IsColumnExists("CountryofResidence"))
                cVBankModel.CountryofResidence = SqlHelper.GetNullableInt32(reader, "CountryofResidence");

            if (reader.IsColumnExists("CityofResidence"))
                cVBankModel.CityofResidence = SqlHelper.GetNullableInt32(reader, "CityofResidence");

            if (reader.IsColumnExists("Address"))
                cVBankModel.Address = SqlHelper.GetNullableString(reader, "Address");

            if (reader.IsColumnExists("CreatedBy"))
                cVBankModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                cVBankModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                cVBankModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                cVBankModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return cVBankModel;
        }

        public static List<CVBankGetModel> TranslateAsCVBankList(this SqlDataReader reader)
        {
            var cVBankList = new List<CVBankGetModel>();
            while (reader.Read())
            {
                cVBankList.Add(TranslateAsCVBank(reader, true));
            }

            return cVBankList;
        }

        public static CVBankPutModel TranslateAsPutCVBank(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cVBankModel = new CVBankPutModel();

            if (reader.IsColumnExists("CVBankId"))
                cVBankModel.CVBankId = SqlHelper.GetNullableInt32(reader, "CVBankId");

            if (reader.IsColumnExists("CandidateName"))
                cVBankModel.CandidateName = SqlHelper.GetNullableString(reader, "CandidateName");

            if (reader.IsColumnExists("EmailId"))
                cVBankModel.EmailId = SqlHelper.GetNullableString(reader, "EmailId");

            if (reader.IsColumnExists("JobTitle"))
                cVBankModel.JobTitle = SqlHelper.GetNullableString(reader, "JobTitle");

            if (reader.IsColumnExists("Specialization"))
                cVBankModel.Specialization = SqlHelper.GetNullableInt32(reader, "Specialization");

            if (reader.IsColumnExists("EducationalQualification"))
                cVBankModel.EducationalQualification = SqlHelper.GetNullableInt32(reader, "EducationalQualification");

            if (reader.IsColumnExists("Gender"))
                cVBankModel.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("YearsofExperience"))
                cVBankModel.YearsofExperience = SqlHelper.GetNullableString(reader, "YearsofExperience");

            if (reader.IsColumnExists("AreaofExpertise"))
                cVBankModel.AreaofExpertise = SqlHelper.GetNullableString(reader, "AreaofExpertise");

            if (reader.IsColumnExists("CountryofResidence"))
                cVBankModel.CountryofResidence = SqlHelper.GetNullableInt32(reader, "CountryofResidence");

            if (reader.IsColumnExists("CityofResidence"))
                cVBankModel.CityofResidence = SqlHelper.GetNullableInt32(reader, "CityofResidence");

            if (reader.IsColumnExists("Address"))
                cVBankModel.Address = SqlHelper.GetNullableString(reader, "Address");

            if (reader.IsColumnExists("UpdatedBy"))
                cVBankModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                cVBankModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return cVBankModel;
        }

        public static List<CVBankPutModel> TranslateAsPutCVBankList(this SqlDataReader reader)
        {
            var cVBankList = new List<CVBankPutModel>();
            while (reader.Read())
            {
                cVBankList.Add(TranslateAsPutCVBank(reader, true));
            }

            return cVBankList;
        }
    }
}

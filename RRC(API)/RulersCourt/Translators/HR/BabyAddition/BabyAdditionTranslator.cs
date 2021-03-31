using RulersCourt.Models.BabyAddition;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.BabyAddition
{
    public static class BabyAdditionTranslator
    {
        public static object BabyAdditioneModel { get; private set; }

        public static BabyAdditionGetModel TranslateAsBabyAdditionGetbyID(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var babyAdditionModel = new BabyAdditionGetModel();

            if (reader.IsColumnExists("BabyAdditionID"))
                babyAdditionModel.BabyAdditionID = SqlHelper.GetNullableInt32(reader, "BabyAdditionID");

            if (reader.IsColumnExists("ReferenceNumber"))
                babyAdditionModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                babyAdditionModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                babyAdditionModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("Status"))
                babyAdditionModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("BabyName"))
                babyAdditionModel.BabyName = SqlHelper.GetNullableString(reader, "BabyName");

            if (reader.IsColumnExists("Gender"))
                babyAdditionModel.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("Birthday"))
                babyAdditionModel.Birthday = SqlHelper.GetDateTime(reader, "Birthday");

            if (reader.IsColumnExists("HospitalName"))
                babyAdditionModel.HospitalName = SqlHelper.GetNullableString(reader, "HospitalName");

            if (reader.IsColumnExists("CountryOfBirth"))
                babyAdditionModel.CountryOfBirth = SqlHelper.GetNullableString(reader, "CountryOfBirth");

            if (reader.IsColumnExists("CityOfBirth"))
                babyAdditionModel.CityOfBirth = SqlHelper.GetNullableString(reader, "CityOfBirth");

            if (reader.IsColumnExists("CreatedBy"))
                babyAdditionModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                babyAdditionModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                babyAdditionModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                babyAdditionModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return babyAdditionModel;
        }

        public static List<BabyAdditionGetModel> TranslateAsBabyAdditionList(this SqlDataReader reader)
        {
            var babyAdditionList = new List<BabyAdditionGetModel>();
            while (reader.Read())
            {
                babyAdditionList.Add(TranslateAsBabyAdditionGetbyID(reader, true));
            }

            return babyAdditionList;
        }

        public static BabyAdditionPutModel TranslateAsPutBabyAddition(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var babyAdditionPutModel = new BabyAdditionPutModel();

            if (reader.IsColumnExists("BabyAdditionID"))
                babyAdditionPutModel.BabyAdditionID = SqlHelper.GetNullableInt32(reader, "BabyAdditionID");

            if (reader.IsColumnExists("ReferenceNumber"))
                babyAdditionPutModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                babyAdditionPutModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                babyAdditionPutModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("BabyName"))
                babyAdditionPutModel.BabyName = SqlHelper.GetNullableString(reader, "BabyName");

            if (reader.IsColumnExists("Gender"))
                babyAdditionPutModel.Gender = SqlHelper.GetNullableString(reader, "Gender");

            if (reader.IsColumnExists("Birthday"))
                babyAdditionPutModel.Birthday = SqlHelper.GetDateTime(reader, "Birthday");

            if (reader.IsColumnExists("HospitalName"))
                babyAdditionPutModel.HospitalName = SqlHelper.GetNullableString(reader, "HospitalName");

            if (reader.IsColumnExists("CountryOfBirth"))
                babyAdditionPutModel.CountryOfBirth = SqlHelper.GetNullableString(reader, "CountryOfBirth");

            if (reader.IsColumnExists("CityOfBirth"))
                babyAdditionPutModel.CityOfBirth = SqlHelper.GetNullableString(reader, "CityOfBirth");

            if (reader.IsColumnExists("UpdatedBy"))
                babyAdditionPutModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                babyAdditionPutModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                babyAdditionPutModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                babyAdditionPutModel.Comments = SqlHelper.GetNullableString(reader, "Comments");
            return babyAdditionPutModel;
        }

        public static List<BabyAdditionPutModel> TranslateAsPutBabyAdditionList(this SqlDataReader reader)
        {
            var babyAdditionPutList = new List<BabyAdditionPutModel>();
            while (reader.Read())
            {
                babyAdditionPutList.Add(TranslateAsPutBabyAddition(reader, true));
            }

            return babyAdditionPutList;
        }
    }
}
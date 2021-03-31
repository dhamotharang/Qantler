using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_MasterLookupTranslator
    {
        public static M_MasterLookupsGetModel TranslateAsGetMasterLookup(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var master = new M_MasterLookupsGetModel();

            if (reader.IsColumnExists("LookupsID"))
                master.LookupsID = SqlHelper.GetNullableInt32(reader, "LookupsID");

            if (reader.IsColumnExists("CountryID"))
                master.CountryID = SqlHelper.GetNullableInt32(reader, "CountryID");

            if (reader.IsColumnExists("DisplayName"))
                master.DisplayName = SqlHelper.GetNullableString(reader, "DisplayName");

            if (reader.IsColumnExists("ArDisplayName"))
                master.ArDisplayName = SqlHelper.GetNullableString(reader, "ArDisplayName");

            if (reader.IsColumnExists("DisplayOrder"))
                master.DisplayOrder = SqlHelper.GetNullableInt32(reader, "DisplayOrder");

            if (reader.IsColumnExists("CreatedBy"))
                master.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                master.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                master.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                master.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("EmiratesID"))
                master.EmiratesID = SqlHelper.GetNullableInt32(reader, "EmiratesID");

            return master;
        }

        public static List<M_MasterLookupsGetModel> TranslateAsMasterLookups(this SqlDataReader reader)
        {
            var masters = new List<M_MasterLookupsGetModel>();
            while (reader.Read())
            {
                masters.Add(TranslateAsGetMasterLookup(reader, true));
            }

            return masters;
        }

        public static M_DepartmentModel TranslateAsGetDepartment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var department = new M_DepartmentModel();

            if (reader.IsColumnExists("DepartmentID"))
                department.DepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            if (reader.IsColumnExists("DepartmentName"))
                department.DepartmentName = SqlHelper.GetNullableString(reader, "DepartmentName");

            return department;
        }

        public static List<M_DepartmentModel> TranslateAsDepartments(this SqlDataReader reader)
        {
            var departments = new List<M_DepartmentModel>();
            while (reader.Read())
            {
                departments.Add(TranslateAsGetDepartment(reader, true));
            }

            return departments;
        }

        public static M_SectionModel TranslateAsGetSection(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var section = new M_SectionModel();

            if (reader.IsColumnExists("SectionID"))
                section.SectionID = SqlHelper.GetNullableInt32(reader, "SectionID");

            if (reader.IsColumnExists("SectionName"))
                section.SectionName = SqlHelper.GetNullableString(reader, "SectionName");

            return section;
        }

        public static List<M_SectionModel> TranslateAsSections(this SqlDataReader reader)
        {
            var sections = new List<M_SectionModel>();
            while (reader.Read())
            {
                sections.Add(TranslateAsGetSection(reader, true));
            }

            return sections;
        }

        public static M_UnitModel TranslateAsGetUnit(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var unit = new M_UnitModel();

            if (reader.IsColumnExists("UnitID"))
                unit.UnitID = SqlHelper.GetNullableInt32(reader, "UnitID");

            if (reader.IsColumnExists("UnitName"))
                unit.UnitName = SqlHelper.GetNullableString(reader, "UnitName");

            return unit;
        }

        public static List<M_UnitModel> TranslateAsUnits(this SqlDataReader reader)
        {
            var units = new List<M_UnitModel>();
            while (reader.Read())
            {
                units.Add(TranslateAsGetUnit(reader, true));
            }

            return units;
        }

        public static M_UserModel TranslateAsGetUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var user = new M_UserModel();

            if (reader.IsColumnExists("UserProfileID"))
                user.UserProfileID = SqlHelper.GetNullableInt32(reader, "UserProfileID");

            if (reader.IsColumnExists("EmployeeName"))
                user.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            return user;
        }

        public static List<M_UserModel> TranslateAsUsers(this SqlDataReader reader)
        {
            var users = new List<M_UserModel>();
            while (reader.Read())
            {
                users.Add(TranslateAsGetUser(reader, true));
            }

            return users;
        }
    }
}

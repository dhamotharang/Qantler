using RulersCourt.Models.ITSupport;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.ITSupportTranslators
{
    public static class ITSupportTranslator
    {
        public static ITSupportTranslatorModel TranslateAsITSupportSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var iTSupportSaveResponseModel = new ITSupportTranslatorModel();

            if (reader.IsColumnExists("ITSupportID"))
                iTSupportSaveResponseModel.ITSupportID = SqlHelper.GetNullableInt32(reader, "ITSupportID");

            if (reader.IsColumnExists("ReferenceNumber"))
                iTSupportSaveResponseModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                iTSupportSaveResponseModel.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                iTSupportSaveResponseModel.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return iTSupportSaveResponseModel;
        }

        public static ITSupportTranslatorModel TranslateAsITSupportSaveResponseList(this SqlDataReader reader)
        {
            var iTSupportSaveResponseModel = new ITSupportTranslatorModel();
            while (reader.Read())
            {
                iTSupportSaveResponseModel = TranslateAsITSupportSaveResponse(reader, true);
            }

            return iTSupportSaveResponseModel;
        }

        public static ITSupportPutModel TranslateAsPutITSuppport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var iTSupportPutModel = new ITSupportPutModel();

            if (reader.IsColumnExists("ITSupportID"))
                iTSupportPutModel.ITSupportID = SqlHelper.GetNullableInt32(reader, "ITSupportID");

            if (reader.IsColumnExists("SourceOU"))
                iTSupportPutModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                iTSupportPutModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("RequestorName"))
                iTSupportPutModel.RequestorName = SqlHelper.GetNullableString(reader, "RequestorName");

            if (reader.IsColumnExists("RequestorDepartment"))
                iTSupportPutModel.RequestorDepartment = SqlHelper.GetNullableString(reader, "RequestorDepartment");

            if (reader.IsColumnExists("Subject"))
                iTSupportPutModel.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("RequestType"))
                iTSupportPutModel.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("RequestDetails"))
                iTSupportPutModel.RequestDetails = SqlHelper.GetNullableString(reader, "RequestDetails");

            if (reader.IsColumnExists("Status"))
                iTSupportPutModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("Action"))
                iTSupportPutModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("UpdatedBy"))
                iTSupportPutModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                iTSupportPutModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return iTSupportPutModel;
        }

        public static List<ITSupportPutModel> TranslateAsPutITSupportList(this SqlDataReader reader)
        {
            var photoRequestList = new List<ITSupportPutModel>();
            while (reader.Read())
            {
                photoRequestList.Add(TranslateAsPutITSuppport(reader, true));
            }

            return photoRequestList;
        }

        public static ITSupportGetModel TranslateITSupportGetModel(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var iTSupportGetModelDetail = new ITSupportGetModel();

            if (reader.IsColumnExists("ITSupportID"))
                iTSupportGetModelDetail.ITSupportID = SqlHelper.GetNullableInt32(reader, "ITSupportID");

            if (reader.IsColumnExists("Status"))
                iTSupportGetModelDetail.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("ReferenceNumber"))
                iTSupportGetModelDetail.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Priority"))
                iTSupportGetModelDetail.Priority = SqlHelper.GetNullableString(reader, "Priority");

            if (reader.IsColumnExists("SourceOU"))
                iTSupportGetModelDetail.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                iTSupportGetModelDetail.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("RequestorName"))
                iTSupportGetModelDetail.RequestorName = SqlHelper.GetNullableString(reader, "RequestorName");

            if (reader.IsColumnExists("RequestorDepartment"))
                iTSupportGetModelDetail.RequestorDepartment = SqlHelper.GetNullableString(reader, "RequestorDepartment");

            if (reader.IsColumnExists("Subject"))
                iTSupportGetModelDetail.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("RequestType"))
                iTSupportGetModelDetail.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("RequestDetails"))
                iTSupportGetModelDetail.RequestDetails = SqlHelper.GetNullableString(reader, "RequestDetails");

            if (reader.IsColumnExists("CreatedDateTime"))
                iTSupportGetModelDetail.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("CreatedBy"))
                iTSupportGetModelDetail.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                iTSupportGetModelDetail.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("UpdatedBy"))
                iTSupportGetModelDetail.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            return iTSupportGetModelDetail;
        }

        public static List<ITSupportGetModel> TranslatePhotoGetModelList(this SqlDataReader reader)
        {
            var iTSupportGetModelList = new List<ITSupportGetModel>();
            while (reader.Read())
            {
                iTSupportGetModelList.Add(TranslateITSupportGetModel(reader, true));
            }

            return iTSupportGetModelList;
        }

        public static ITSupportHomeModel TranslateITSupportHomeDashboardModel(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var iTSupportHomeModel = new ITSupportHomeModel();

            if (reader.IsColumnExists("ServicesNew"))
                iTSupportHomeModel.ServicesNew = SqlHelper.GetNullableInt32(reader, "ServicesNew");

            if (reader.IsColumnExists("ServicesInProgress"))
                iTSupportHomeModel.ServicesInProgress = SqlHelper.GetNullableInt32(reader, "ServicesInProgress");

            if (reader.IsColumnExists("ServicesClose"))
                iTSupportHomeModel.ServicesClose = SqlHelper.GetNullableInt32(reader, "ServicesClose");

            if (reader.IsColumnExists("SupportNew"))
                iTSupportHomeModel.SupportNew = SqlHelper.GetNullableInt32(reader, "SupportNew");

            if (reader.IsColumnExists("SupportInprogress"))
                iTSupportHomeModel.SupportInprogress = SqlHelper.GetNullableInt32(reader, "SupportInprogress");

            if (reader.IsColumnExists("SupportClose"))
                iTSupportHomeModel.SupportClose = SqlHelper.GetNullableInt32(reader, "SupportClose");

            if (reader.IsColumnExists("ComponentsNew"))
                iTSupportHomeModel.ComponentsNew = SqlHelper.GetNullableInt32(reader, "ComponentsNew");

            if (reader.IsColumnExists("ComponentsInprogress"))
                iTSupportHomeModel.ComponentsInprogress = SqlHelper.GetNullableInt32(reader, "ComponentsInprogress");

            if (reader.IsColumnExists("ComponentsClose"))
                iTSupportHomeModel.ComponentsClose = SqlHelper.GetNullableInt32(reader, "ComponentsClose");

            if (reader.IsColumnExists("MyClosedRequest"))
                iTSupportHomeModel.MyClosedRequest = SqlHelper.GetNullableInt32(reader, "MyClosedRequest");

            if (reader.IsColumnExists("MyOwnRequest"))
                iTSupportHomeModel.MyOwnRequest = SqlHelper.GetNullableInt32(reader, "MyOwnRequest");

            return iTSupportHomeModel;
        }

        public static ITSupportHomeModel TranslateITSupportHomeModel(this SqlDataReader reader)
        {
            var iTSupportHomeModel = new ITSupportHomeModel();
            while (reader.Read())
            {
                iTSupportHomeModel = TranslateITSupportHomeDashboardModel(reader, true);
            }

            return iTSupportHomeModel;
        }
    }
}

using RulersCourt.Models.Protocol.Media;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media
{
    public static class MediaTranslator
    {
        public static MediaHomeDashboardListModel TranslateAsMediaDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var mediaDashboardModel = new MediaHomeDashboardListModel();

            if (reader.IsColumnExists("RefID"))
            {
                mediaDashboardModel.RefID = SqlHelper.GetNullableString(reader, "RefID");
            }

            if (reader.IsColumnExists("RequestID"))
            {
                mediaDashboardModel.RequestID = SqlHelper.GetNullableInt32(reader, "RequestID");
            }

            if (reader.IsColumnExists("Source"))
            {
                mediaDashboardModel.Source = SqlHelper.GetNullableString(reader, "Source");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                mediaDashboardModel.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("Status"))
            {
                mediaDashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("RequestDate"))
            {
                mediaDashboardModel.RequestDate = SqlHelper.GetDateTime(reader, "RequestDate");
            }

            if (reader.IsColumnExists("AssignedTo"))
            {
                mediaDashboardModel.AssignedTo = SqlHelper.GetNullableString(reader, "AssignedTo");
            }

            if (reader.IsColumnExists("CreatorID"))
            {
                mediaDashboardModel.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID");
            }

            return mediaDashboardModel;
        }

        public static MediaHomeModel TranslateAsMediaDashboardCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var mediahomemodel = new MediaHomeModel();

            if (reader.IsColumnExists("RequestforPhoto"))
            {
                mediahomemodel.RequestforPhoto = SqlHelper.GetNullableInt32(reader, "RequestforPhoto");
            }

            if (reader.IsColumnExists("RequestforDesign"))
            {
                mediahomemodel.RequestforDesign = SqlHelper.GetNullableInt32(reader, "RequestforDesign");
            }

            if (reader.IsColumnExists("RequestforPressRelease"))
            {
                mediahomemodel.RequestforPressRelease = SqlHelper.GetNullableInt32(reader, "RequestforPressRelease");
            }

            if (reader.IsColumnExists("RequestforCampaign"))
            {
                mediahomemodel.RequestforCampaign = SqlHelper.GetNullableInt32(reader, "RequestforCampaign");
            }

            if (reader.IsColumnExists("RequestforPhotographer"))
            {
                mediahomemodel.RequestforPhotographer = SqlHelper.GetNullableInt32(reader, "RequestforPhotographer");
            }

            if (reader.IsColumnExists("RequesttouseDiwanIdentity"))
            {
                mediahomemodel.RequesttouseDiwanIdentity = SqlHelper.GetNullableInt32(reader, "RequesttouseDiwanIdentity");
            }

            if (reader.IsColumnExists("MyPendingRequest"))
            {
                mediahomemodel.MyPendingRequest = SqlHelper.GetNullableInt32(reader, "MyPendingRequest");
            }

            if (reader.IsColumnExists("MyOwnRequest"))
            {
                mediahomemodel.MyOwnRequest = SqlHelper.GetNullableInt32(reader, "MyOwnRequest");
            }

            if (reader.IsColumnExists("MyProcessedRequest"))
            {
                mediahomemodel.MyProcessedRequest = SqlHelper.GetNullableInt32(reader, "MyProcessedRequest");
            }

            return mediahomemodel;
        }

        public static List<MediaHomeDashboardListModel> TranslateAsMediaDashboardList(this SqlDataReader reader)
        {
            var mediaDashboardList = new List<MediaHomeDashboardListModel>();
            while (reader.Read())
            {
                mediaDashboardList.Add(TranslateAsMediaDashboard(reader, true));
            }

            return mediaDashboardList;
        }

        public static MediaHomeModel TranslateasMediaDashboardcount(this SqlDataReader reader)
        {
            var mediahomemodel = new MediaHomeModel();
            while (reader.Read())
            {
                mediahomemodel = TranslateAsMediaDashboardCount(reader, true);
            }

            return mediahomemodel;
        }
    }
}

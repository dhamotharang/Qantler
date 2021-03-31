using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class NotificationListTranslator
    {
        public static NotificationListModel TranslateAsGetNotificationList(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var location = new NotificationListModel();

            if (reader.IsColumnExists("ReferenceNumber"))
                location.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("ID"))
                location.ID = SqlHelper.GetNullableInt32(reader, "ID");

            if (reader.IsColumnExists("ServiceID"))
                location.ServiceID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("Service"))
                location.Service = SqlHelper.GetNullableString(reader, "Service");

            if (reader.IsColumnExists("Process"))
                location.Process = SqlHelper.GetNullableString(reader, "Process");

            if (reader.IsColumnExists("IsRead"))
                location.IsRead = SqlHelper.GetBoolean(reader, "IsRead");

            if (reader.IsColumnExists("LastUpdateDatetime"))
                location.LastUpdateDatetime = SqlHelper.GetDateTime(reader, "LastUpdateDatetime");

            if (reader.IsColumnExists("FromName"))
                location.FromName = SqlHelper.GetNullableString(reader, "FromName");

            if (reader.IsColumnExists("IsAnonymous"))
                location.IsAnonymous = SqlHelper.GetBoolean(reader, "IsAnonymous");

            return location;
        }

        public static List<NotificationListModel> TranslateAsNotificationList(this SqlDataReader reader)
        {
            var notificationList = new List<NotificationListModel>();
            while (reader.Read())
            {
                notificationList.Add(TranslateAsGetNotificationList(reader, true));
            }

            return notificationList;
        }

        public static NotificationGetModel TranslateAsGetNotification(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var notification = new NotificationGetModel();

            if (reader.IsColumnExists("ReferenceNumber"))
                notification.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("ID"))
                notification.ID = SqlHelper.GetNullableInt32(reader, "ID");

            if (reader.IsColumnExists("EnFromName"))
                notification.EnFromName = SqlHelper.GetNullableString(reader, "EnFromName");

            if (reader.IsColumnExists("ArFromName"))
                notification.ArFromName = SqlHelper.GetNullableString(reader, "ArFromName");

            if (reader.IsColumnExists("ToName"))
                notification.ToName = SqlHelper.GetNullableString(reader, "ToName");

            if (reader.IsColumnExists("EnDelegateFromName"))
                notification.EnDelegateFromName = SqlHelper.GetNullableString(reader, "EnDelegateFromName");

            if (reader.IsColumnExists("ArDelegateFromName"))
                notification.ArDelegateFromName = SqlHelper.GetNullableString(reader, "ArDelegateFromName");

            if (reader.IsColumnExists("DelegateToName"))
                notification.DelegateToName = SqlHelper.GetNullableString(reader, "DelegateToName");

            if (reader.IsColumnExists("Process"))
                notification.Process = SqlHelper.GetNullableString(reader, "Process");

            if (reader.IsColumnExists("Service"))
                notification.Service = SqlHelper.GetNullableString(reader, "Service");

            if (reader.IsColumnExists("ServiceID"))
                notification.ServiceID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("Content"))
                notification.Content = SqlHelper.GetNullableString(reader, "Content");

            if (reader.IsColumnExists("Subject"))
                notification.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("PlateNumber"))
                notification.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("IsAnonymous"))
                notification.IsAnonymous = SqlHelper.GetBoolean(reader, "IsAnonymous");

            return notification;
        }

        public static NotificationGetModel TranslateAsNotification(this SqlDataReader reader)
        {
            var notification = new NotificationGetModel();
            while (reader.Read())
            {
                notification = TranslateAsGetNotification(reader, true);
            }

            return notification;
        }
    }
}

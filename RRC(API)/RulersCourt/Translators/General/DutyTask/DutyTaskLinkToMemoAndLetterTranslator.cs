using RulersCourt.Models.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskLinkToMemoAndLetterTranslator
    {
        public static LinkToLetterModel TranslateAsGetLinkToLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var linkToLetter = new LinkToLetterModel();

            if (reader.IsColumnExists("LetterReferenceNumber"))
                linkToLetter.LetterReferenceNumber = SqlHelper.GetNullableString(reader, "LetterReferenceNumber");

            if (reader.IsColumnExists("LetterID"))
                linkToLetter.LetterID = SqlHelper.GetNullableString(reader, "LetterID");

            if (reader.IsColumnExists("LetterType"))
                linkToLetter.LetterType = SqlHelper.GetNullableInt32(reader, "LetterType");

            return linkToLetter;
        }

        public static List<LinkToLetterModel> TranslateAsLinkToLetterList(this SqlDataReader reader)
        {
            var linkToLetterrList = new List<LinkToLetterModel>();
            while (reader.Read())
            {
                linkToLetterrList.Add(TranslateAsGetLinkToLetter(reader, true));
            }

            return linkToLetterrList;
        }

        public static LinkToMemoModel TranslateAsGetLinkToMemo(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var linkToMemo = new LinkToMemoModel();

            if (reader.IsColumnExists("MemoReferenceNumber"))
                linkToMemo.MemoReferenceNumber = SqlHelper.GetNullableString(reader, "MemoReferenceNumber");

            if (reader.IsColumnExists("MemoID"))
                linkToMemo.MemoID = SqlHelper.GetNullableString(reader, "MemoID");

            return linkToMemo;
        }

        public static List<LinkToMemoModel> TranslateAsLinkToMemoList(this SqlDataReader reader)
        {
            var linkToMemoList = new List<LinkToMemoModel>();
            while (reader.Read())
            {
                linkToMemoList.Add(TranslateAsGetLinkToMemo(reader, true));
            }

            return linkToMemoList;
        }

        public static LinkToMeetingModel TranslateAsGetLinkToMeeting(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var linkToMeeting = new LinkToMeetingModel();

            if (reader.IsColumnExists("MeetingReferenceNumber"))
                linkToMeeting.MeetingReferenceNumber = SqlHelper.GetNullableString(reader, "MeetingReferenceNumber");

            if (reader.IsColumnExists("MeetingID"))
                linkToMeeting.MeetingID = SqlHelper.GetNullableString(reader, "MeetingID");

            return linkToMeeting;
        }

        public static List<LinkToMeetingModel> TranslateAsLinkToMeetingList(this SqlDataReader reader)
        {
            var linkToMeetingList = new List<LinkToMeetingModel>();
            while (reader.Read())
            {
                linkToMeetingList.Add(TranslateAsGetLinkToMeeting(reader, true));
            }

            return linkToMeetingList;
        }
    }
}

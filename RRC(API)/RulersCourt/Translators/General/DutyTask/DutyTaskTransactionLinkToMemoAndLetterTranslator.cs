using RulersCourt.Models.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskTransactionLinkToMemoAndLetterTranslator
    {
        public static DutyTaskMemoReferenceNumberModel TranslateAsGetLinkToMemo(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var referenceNumber = new DutyTaskMemoReferenceNumberModel();

            if (reader.IsColumnExists("MemoReferenceNo"))
                referenceNumber.MemoReferenceNumber = SqlHelper.GetNullableString(reader, "MemoReferenceNo");

            if (reader.IsColumnExists("MemoID"))
                referenceNumber.MemoID = SqlHelper.GetNullableString(reader, "MemoID");

            return referenceNumber;
        }

        public static List<DutyTaskMemoReferenceNumberModel> TranslateAsGetLinkToMemoList(this SqlDataReader reader)
        {
            var linkToMemoList = new List<DutyTaskMemoReferenceNumberModel>();
            while (reader.Read())
            {
                linkToMemoList.Add(TranslateAsGetLinkToMemo(reader, true));
            }

            return linkToMemoList;
        }

        public static DutyTaskLetterReferenceNumberModel TranslateAsGetLinkToLetter(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var referenceNumber = new DutyTaskLetterReferenceNumberModel();

            if (reader.IsColumnExists("LetterReferenceNo"))
                referenceNumber.LetterReferenceNumber = SqlHelper.GetNullableString(reader, "LetterReferenceNo");

            if (reader.IsColumnExists("LetterID"))
                referenceNumber.LetterID = SqlHelper.GetNullableString(reader, "LetterID");

            if (reader.IsColumnExists("LetterType"))
                referenceNumber.LetterType = SqlHelper.GetNullableInt32(reader, "LetterType");

            return referenceNumber;
        }

        public static List<DutyTaskLetterReferenceNumberModel> TranslateAsGetLinkToLetterList(this SqlDataReader reader)
        {
            var linkToLetterList = new List<DutyTaskLetterReferenceNumberModel>();
            while (reader.Read())
            {
                linkToLetterList.Add(TranslateAsGetLinkToLetter(reader, true));
            }

            return linkToLetterList;
        }

        public static DutyTaskMeetingReferenceNumberModel TranslateAsGetLinkToMeeting(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var referenceNumber = new DutyTaskMeetingReferenceNumberModel();

            if (reader.IsColumnExists("MeetingReferenceNumber"))
                referenceNumber.MeetingReferenceNumber = SqlHelper.GetNullableString(reader, "MeetingReferenceNumber");

            if (reader.IsColumnExists("MeetingID"))
                referenceNumber.MeetingID = SqlHelper.GetNullableString(reader, "MeetingID");

            return referenceNumber;
        }

        public static List<DutyTaskMeetingReferenceNumberModel> TranslateAsGetLinkToMeetingList(this SqlDataReader reader)
        {
            var linkToMeetingList = new List<DutyTaskMeetingReferenceNumberModel>();
            while (reader.Read())
            {
                linkToMeetingList.Add(TranslateAsGetLinkToMeeting(reader, true));
            }

            return linkToMeetingList;
        }
    }
}

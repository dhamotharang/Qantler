using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Enums
{
    public enum DbReturnValue
    {
        [EnumMember(Value = "Create Success")]
        [Description("Record created successfully")]
        CreateSuccess = 100,

        [EnumMember(Value = "Update Success")]
        [Description("Record updated successfully")]
        UpdateSuccess = 101,

        [EnumMember(Value = "Not Exists")]
        [Description("Record does not exists")]
        NotExists = 102,

        [EnumMember(Value = "Delete Success")]
        [Description("Record deleted successfully")]
        DeleteSuccess = 103,

        [EnumMember(Value = "Active Try Delete")]
        [Description("Active record can not be deleted")]
        ActiveTryDelete = 104,

        [EnumMember(Value = "Record Exists")]
        [Description("Record exists in database")]
        RecordExists = 105,

        [EnumMember(Value = "Updation Failed")]
        [Description("Record updation failed")]
        UpdationFailed = 106,

        [EnumMember(Value = "Creation Failed")]
        [Description("Record creation failed")]
        CreationFailed = 107,

        [EnumMember(Value = "Email Exists")]
        [Description("Email Already Exists")]
        EmailExists = 108,

        [EnumMember(Value = "Email Not Exists")]
        [Description("Email Does not exists")]
        EmailNotExists = 109,

        [EnumMember(Value = "Password Incorrect")]
        [Description("Password Does not match")]
        PasswordIncorrect = 110,

        [EnumMember(Value = "Authentication Success")]
        [Description("Customer Authentication Success")]
        AuthSuccess = 111,

        [EnumMember(Value = "Reason Unknown")]
        [Description("Operation Failed Due to Unknown Reason")]
        ReasonUnknown = 112,

        [EnumMember(Value = "No Records")]
        [Description("No Records Found")]
        NoRecords = 113,

        [EnumMember(Value = "DeleteFailed")]
        [Description("Failed to Delete Record")]
        DeleteFailed = 114,

        [EnumMember(Value = "Approved")]
        [Description("Approved")]
        Approved = 115,

        [EnumMember(Value = "AlreadyApproved")]
        [Description("Already Approved")]
        AlreadyApproved = 116,

        [EnumMember(Value = "ReasouceOnlyHiddenByAuthor")]
        [Description("Reasouce/Course comments can only be made hidden by author")]
        ReasouceOnlyHiddenByAuthor = 117,

        [EnumMember(Value = "Already Replied")]
        [Description("Already Replied to this query")]
        AlreadyReplied = 118,

        [EnumMember(Value = "Can not Withdrawal")]
        [Description("You can not widthdrawal this content")]
         NotWidthdrawal = 119,

        [EnumMember(Value = "Can not Delete Resource")]
        [Description("Unable to delete as this resource have been used for further content creation.")]
         ResourceReferred = 120,


        [EnumMember(Value = "Active Not Delete")]
        [Description("Record Can't be deleted, records mapped to transcations can be only deactivated.")]
        ActiveTryNotDelete = 121,

        [EnumMember(Value = "Elastic request successfull")]
        [Description("Successfully retrieved results from Elastic Search.")]
        SearchSuccessful = 200,

        [EnumMember(Value = "Elastic request unsuccessfull")]
        [Description("Could not retrieve results from Elastic Search.")]
        SearchUnsuccessful = 201,

        [EnumMember(Value = "Published")]
        [Description("Published")]
        Published = 203,

        [EnumMember(Value = "Rejected")]
        [Description("Rejected")]
        Rejected = 204,

        [EnumMember(Value = "Action already taken")]
        [Description("Action already taken")]
        ActionAlreadyTaken = 205,

        [EnumMember(Value = "Rejected and sent to Drafts")]
        [Description("Rejected and sent to Drafts")]
        RejectedAndSentToDrafts = 206,

        [EnumMember(Value = "Reviewed by other users")]
        [Description("Thanks, this educational resource has been reviewed by other users")]
        ReviewedByOtherUsers = 207,
    }
}

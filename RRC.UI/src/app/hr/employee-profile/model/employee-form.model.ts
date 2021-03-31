export class EmployeeFormModel {
    Date :  Date
    EmployeeName : string
    EmployeeCode : string
    OfficialMailId : string
    PersonalMailId : string
    Gender : string
    BirthDate : Date
    Age : 0
    CountryofResidence : string
    MobileNumber : string
    EmployeePhoneNumber : string
    Religion : string
    Nationality : string
    PreviousNationality : string
    JoinDate : Date
    Title : string
    Grade : string
    EmployeePosition : string
    EmploymentStatus : string
    Resigned : string
    ResignationDate : Date
    BalanceLeave : 0
    NotificationPreferences : string
    Education : [{
        Degree : string
        SchoolCollege: string
        FieldStudy: string
        TimePeriodFrom: Date
        TimePeriodTo: Date
    }]
    WorkExperience : [{
        JobTitle : string
        Company: string
        City: string
        TimePeriodFrom: Date
        TimePeriodTo: Date
    }]
    TrainingCertifications : [{
        TrainingName: string
        StartDate: Date
        EndDate: Date
    }]
    PassportNumber : string
    PassportIssuePlace : string
    PassportIssueDate :  Date
    PassportExpiryDate : Date
    VisaNumber : string
    VisaIssueDate : Date
    VisaExpiryDate : Date
    EmiratesIdNumber : string
    EmiratesIdIssueDate : Date
    EmiratesIdExpiryDate : Date
    InsuranceNumber : string
    InsuranceIssueDate : Date
    InsuranceExpiryDate : Date
    LabourContractNumber : string
    LabourContractIssueDate : Date
    LaborContractExpiryDate : Date
    Attachment: [{
        AttachmentGuid: string
        AttachmentsName: string
        AttachmentsType: string
        UserProfileId: 0
    }]
    CreatedBy : 0
    CreatedDatetime : Date
    ProfilePhotoID: string
    ProfilePhotoName: string
    ContractTypes: string
    OrgUnitID: 0
    RoleId: string
    Action: string
}

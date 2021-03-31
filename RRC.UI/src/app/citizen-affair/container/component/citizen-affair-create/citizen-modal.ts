export class CitizenCreateModal {
    "CitizenAffairID":Number
    "SourceOU": String
    "SourceName": String
    "Staus": String
    "ReferenceNumber": String
    "RequestType": String
    "ApproverId": Number
    "ApproverDepartmentId": Number
    "NotifyUpon": String
    "InternalRequestorID": Number
    "InternalRequestorDepartmentID": Number
    "ExternalRequestEmailID": String
    "CreatedBy": Number
    "CreatedDateTime": Date
    "UpdatedBy":Number
    "UpdatedDateTime":Date
    "Action": String
    "Comments": String
    "Documents": [
        {
            "AttachmentGuid": String
            "AttachmentsName": String
            "CitizenAffairID": Number
        }
    ]
    "Photos": [
        {
            "AttachmentGuid": String
            "AttachmentsName": String
            "CitizenAffairID": Number
        }
    ]
    PersonalReport:{
        "CitizenAffairID": Number
        "ProfilePhotoID":String
        "ProfilePhotoName":String
        "Name": String
        "Employer": Number
        "Destination": String
        "MonthlySalary": String
        "EmiratesID": String
        "MaritalStatus": String
        "NoOfChildrens": String
        "PhoneNumber": String
        "Emirates": Number
        "City": String
        "Age": String
        "ReportObjectives": String
        "FindingNotes": String
        "Recommendation": String
    }
    FieldVisit: {
        "CitizenAffairID": Number
        "Date": any
        "Location": String
        "RequetsedBy": String
        "VisitObjective": String
        "FindingNotes": String
        "ForWhom": String
        "EmiratesID": String
        "Name": String
        "PhoneNumber": String
        "City": Number
        "Emirates":String
        "LocationName": String
        "CityID": Number
    }
}
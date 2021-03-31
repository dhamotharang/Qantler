import { Attachment } from './attachment.model';

export class Cvbank {
  Date: Date
  CandidateName: string
  EmailId: string
  JobTitle: string
  Specialization: string
  EducationalQualification: string
  Gender: string
  YearsofExperience: string
  AreaofExpertise: string
  CountryofResidence: string
  CityofResidence: string
  Address: string
  Action: string
  CreatedBy: Number
  UpdatedBy: Number
  CreatedDateTime: Date
  Comments: string
  Attachments: Array<Attachment>
}

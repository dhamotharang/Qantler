import { Employee } from './employee.model';

export class OfficialTask {
  Date: Date
  SourceOU: string
  SourceName: string
  EmployeeNameID: Array<Employee>
  OfficialTaskType: string
  StartDate: Date
  EndDate: Date
  NumberofDays: Number
  OfficialTaskDescription: string
  CreatedBy: Number
  CreatedDateTime: Date
  Action: string
  Comments: string
}
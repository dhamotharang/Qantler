import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LeaveRequestService {

  leaveRequestData = {
    SourceOU: 'HR',
    SourceName: 'Mohammed',
    approverName: 'Azhar',
    approverDepartment: 'Information Technology',
    doaName: 'Safeer',
    doaDepartment: 'Legal',
    reason: '',
    Attachments: [],
    AttachmentName: '',
    DeleteFlag: '',
    CreatedBy: '',
    UpdatedBy: '',
    CreatedDateTime: new Date(),
    UpdatedDateTime: '',
    Status: 0,
    startDate:'03-04-2019',
    endDate:'03-04-2019',
    leaveBalance:'15',
    leaveType:'Sick Leave'
  };

  constructor() { }

  getLeaveRequestData() {
    return this.leaveRequestData;
  }

}

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class AdminService {
  currentUserData: any = JSON.parse(localStorage.getItem('User'));
  constructor(private http:HttpClient,private endpointService:EndPointService) { }
  
  master_api_url = this.endpointService.apiHostingURL+'/Master';
  attachment_api_url = this.endpointService.apiHostingURL+'/attachment';

  getDepartments(userID:any):Observable<any>{
    return this.http.get(`${this.master_api_url}/Departments`,{params:{UserID:userID}}).pipe(map((res:any)=>res));
  }

  getSections(userID:any):Observable<any>{
    return this.http.get(`${this.master_api_url}/Sections`,{params:{UserID:userID}}).pipe(map((res:any)=>res));
  }

  getUnits(userID:any):Observable<any>{
    return this.http.get(`${this.master_api_url}/Units`,{params:{UserID:userID}}).pipe(map((res:any)=>res));
  }
  
  getAllUsers(userID:any,departmentID?:any,searchStr?:string):Observable<any>{
    if(!searchStr){
      searchStr  = null;
    }
    if(!departmentID){
      departmentID = 0;
    }
    return this.http.get(`${this.master_api_url}/Users`,{params:{UserID:userID,DepartmentID:departmentID}}).pipe(map((res:any)=>res));
  }

  getMailRemainder(userID:any):Observable<any>{
    return this.http.get(`${this.master_api_url}/MailRemainder`,{params:{UserID:userID}}).pipe(map((res:any)=>res));
  }

  saveMailRemainder(mailRemainderDuration:any,userID:any):Observable<any>{
    return this.http.post(`${this.master_api_url}/MailRemainder`,mailRemainderDuration,{params:{UserID:userID}}).pipe(map((res:any)=>res));
  }

  updateMailRemainder(mailRemainderDuration:any,userID:any):Observable<any>{
    return this.http.put(`${this.master_api_url}/MailRemainder`,mailRemainderDuration,{params:{UserID:userID}}).pipe(map((res:any)=>res));
  }

  createMasterData(masterDataObj:any,userID:any,options:any):Observable<any>{
    let toSendParams:any = {};
    if(userID){
      toSendParams.UserID = userID;
    }
    if(options.Type){
      toSendParams.Type = options.Type;
    }
    if(options.Country){
      toSendParams.Country = options.Country;
    }
    if(options.Emirates){
      toSendParams.Emirates = options.Emirates;
    }
    return this.http.post(`${this.master_api_url}`,masterDataObj,{params:toSendParams}).pipe(map((res:any)=>res));
  }

  updateMasterData(masterDataUpdateObj:any,userID:any,options:any):Observable<any>{
    let toSendParams:any = {};
    if(userID){
      toSendParams.UserID = userID;
    }
    if(options.Type){
      toSendParams.Type = options.Type;
    }
    if(options.Country){
      toSendParams.Country = options.Country;
    }
    if(options.Emirates){
      toSendParams.Emirates = options.Emirates;
    }
    return this.http.put(`${this.master_api_url}`,masterDataUpdateObj,{params:toSendParams}).pipe(map((res:any)=>res));
  }

  getMasterData(options:any,userID:any):Observable<any>{
    let toSendParams:any = {};
    if(userID){
      toSendParams.UserID = userID;
    }
    if(options.Type){
      toSendParams.Type = options.Type;
    }
    if(options.searchStr){
      toSendParams.Search = options.searchStr
    }
    
    return this.http.get(`${this.master_api_url}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

  removeMasterData(options:any,userID:any):Observable<any>{
    let toSendParams:any = {};
    if(userID){
      toSendParams.UserID = userID;
    }
    if(options.Type){
      toSendParams.Type = options.Type;
    }
    if(options.searchStr){
      toSendParams.Search = options.searchStr
    }
    return this.http.delete(`${this.master_api_url}/${options.LookupsID}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

  getApproverList(userID:any,departmentID:any):Observable<any>{
    return this.http.get(`${this.master_api_url}/Approver`,{params:{UserID:userID,DepartmentID:departmentID}}).pipe(map((res:any)=>res));
  }

  canApproverRemoved(approverList:any) {
    let dataToSend = {
      ApproverID:approverList
    }
    return this.http.post(`${this.master_api_url}/CanApproverRemoved`, dataToSend);
  }

  saveApproverList(approversList:any,userID:any,departmentID:any):Observable<any>{
    let toSendData = {
      ApproverID:approversList
    };
    return this.http.post(`${this.master_api_url}/Approver`,toSendData,{params:{UserID:userID,DepartmentID:departmentID}}).pipe(map((res:any) => res));
  }

  getUserManagementList(options:any,userID:any):Observable<any>{
    let toSendParams:any = {};
    if(userID){
      toSendParams.UserID = userID;
    }
    if(options.searchStr){
      toSendParams.Search = options.searchStr
    }
    return this.http.get(`${this.master_api_url}/UserManagement${options.PageNumber},${options.PageSize}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

  saveUserManagementData(userManagementData:any,userID:any):Observable<any>{
    return this.http.post(`${this.master_api_url}/UserManagement`,userManagementData,{params:{UserID:userID}}).pipe(map((res:any) => res));
  }

  uploadHolidaysAttachment(data:any):Observable<any>{
    let payload = new FormData();    
    var headers = new HttpHeaders();
    for (var i = 0; i < data.length; i++) {
      payload.append('files', data[i]);
    }
    headers.append("Accept", 'application/json');
    headers.delete("Content-Type");
    return this.http.post(`${this.attachment_api_url}/Upload`,payload,{
      headers:headers,
      reportProgress: true,
      observe:'events'
    }).pipe(map((res:any)=>res));
  }

  downloadHolidaysAttachment(attachmentData:any):Observable<any>{
    return this.http.post(this.attachment_api_url + '/download?filename='+attachmentData.AttachmentsName+'&guid='+attachmentData.AttachmentGuid,'');
  }

  sendHolidayAttachmentForImport(attachmentFilesInfo:any):Observable<any>{
    return this.http.post(`${this.master_api_url}/Holiday`,{},{params:attachmentFilesInfo}).pipe(map((res:any) => res));
  }

  getLatestHolidayFile() {
    return this.http.get(`${this.master_api_url}/Holiday`);
  }

  prepareAttachmentUrl(item:any) {
    return `${this.attachment_api_url}/download?filename=${item.AttachmentsName}&guid=${item.AttachmentGuid}`;
  }
  getLeaveTypes(userID:any, leaveType?:any):Observable<any>{
    return this.http.get(`${this.master_api_url}`,{params:{UserID:userID,Type:leaveType}}).pipe(map((res:any)=>res));
  }
}

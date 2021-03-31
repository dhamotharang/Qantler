import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Observable } from 'rxjs';
import { LeaveRequest } from '../model/leave-request/leave-request.model';
import { map } from 'rxjs/operators';
import { Attachment } from 'src/app/shared/model/attachment/attachment.model';


@Injectable()
export class LeaveService {

  currentUserData: any = JSON.parse(localStorage.getItem('User'));
  constructor(private http:HttpClient,private endpointService:EndPointService) { }

  leave_request_api_url = this.endpointService.apiHostingURL+'/Leave';
  attachment_api_url = this.endpointService.apiHostingURL+'/attachment';
  user_profile_api_url = this.endpointService.apiHostingURL+'/UserProfile';

  getLeaveRequestList(pageNumber:number,pageSize:number):Observable<Array<LeaveRequest>>{
    return this.http.get(`${this.leave_request_api_url}/list/${pageNumber}/${pageSize}`).pipe(map((res:any)=>res));
  }

  getLeaveRequestById(id:number,currentUserId:any):Observable<LeaveRequest>{
    let param = {
      UserID:currentUserId
    };
    return this.http.get<LeaveRequest>(`${this.leave_request_api_url}/${id}`,{params:param}).pipe(map((res:any)=>res));
  }

  addLeaveRequest(leaveRequestData:LeaveRequest):Observable<any>{
    return this.http.post(`${this.leave_request_api_url}`,leaveRequestData).pipe(map((res:any)=>res));
  }

  resubmitLeaveRequest(leaveRequestData:LeaveRequest):Observable<any>{
    return this.http.put(`${this.leave_request_api_url}`,leaveRequestData).pipe(map((res:any)=>res));
  }

  updateLeaveRequestStatus(id:number,updateLeaveRequestData:any):Observable<any>{
    return this.http.patch(`${this.leave_request_api_url}/${id}`,updateLeaveRequestData).pipe(map((res:any)=>res));
  }

  downloadLeaveRequestReport(id):Observable<any>{
    return this.http.post(`${this.leave_request_api_url}/Download`,{LeaveRequestId:id}).pipe(map((res:any)=>res));
  }

  uploadLeaveRequestAttachment(data:any):Observable<Attachment>{
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

  downloadLeaveRequestAttachment(attachmentData:Attachment):Observable<any>{
    return this.http.post(this.attachment_api_url + '/download?filename='+attachmentData.AttachmentsName+'&guid='+attachmentData.AttachmentGuid,'');
  }

  getUserProfileForBalanceLeave(creatorId:number):Observable<any>{
    let params:any= {
      UserID:this.currentUserData.id
    }
    return this.http.get(`${this.user_profile_api_url}/${creatorId}`,{params:params}).pipe(map((res:any)=>res));
  }
}

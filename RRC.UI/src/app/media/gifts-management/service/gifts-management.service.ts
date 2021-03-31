import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Attachment } from 'src/app/shared/model/attachment/attachment.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { GiftRequest } from '../model/gift-request/gift-request.model';

@Injectable()
export class GiftsManagementService {
  currentUserData: any = JSON.parse(localStorage.getItem('User'));
  constructor(private http:HttpClient,private endpointService:EndPointService) { }
  
  gift_request_api_url = this.endpointService.apiHostingURL+'/Gift';
  attachment_api_url = this.endpointService.apiHostingURL+'/attachment';
  

  getGiftRequestList(options:any):Observable<any>{
    let reqParams:any = {};

    if(options.UserID >=0){
      reqParams.UserID = options.UserID;
    }

    if(options.Status){
      reqParams.Status = options.Status;
    }

    if(options.GiftType){
      reqParams.GiftType = options.GiftType;
    }

    if(options.UserName){
      reqParams.UserName = options.UserName;
    }

    if(options.RecievedPurchasedBy){
      reqParams.RecievedPurchasedBy = options.RecievedPurchasedBy;
    }

    if(options.SmartSearch){
      reqParams.SmartSearch = options.SmartSearch;
    }

   return this.http.get(`${this.gift_request_api_url}/ListView${options.PageNumber},${options.PageSize}`,{params:reqParams}).pipe(map((res:any)=>res));
  }

  getGiftRequestById(id:number,currentUserId:any):Observable<GiftRequest>{
    let param = {
      UserID:currentUserId
    };
    return this.http.get<GiftRequest>(`${this.gift_request_api_url}/${id}`,{params:param}).pipe(map((res:any)=>res));
  }

  addGiftRequest(giftRequestData:GiftRequest):Observable<any>{
    return this.http.post(`${this.gift_request_api_url}`,giftRequestData).pipe(map((res:any)=>res));
  }

  resubmitGiftRequest(giftRequestData:GiftRequest):Observable<any>{
    return this.http.put(`${this.gift_request_api_url}`,giftRequestData).pipe(map((res:any)=>res));
  }

  updateGiftRequestStatus(id:number,updateGiftRequestData:any):Observable<any>{
    return this.http.patch(`${this.gift_request_api_url}/${id}`,updateGiftRequestData).pipe(map((res:any)=>res));
  }

  downloadGiftRequestReport(id):Observable<any>{
    return this.http.post(`${this.gift_request_api_url}/Download`,{GiftRequestId:id}).pipe(map((res:any)=>res));
  }

  uploadGiftRequestAttachment(data:any):Observable<Attachment>{
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

  downloadGiftRequestAttachment(attachmentData:Attachment):Observable<any>{
    return this.http.post(this.attachment_api_url + '/download?filename='+attachmentData.AttachmentsName+'&guid='+attachmentData.AttachmentGuid,'');
  }

  sendGiftForDelivery(id:any,giftRequestData:any):Observable<any>{
    return this.http.post(`${this.gift_request_api_url}/SendForDelivery/${id}`,giftRequestData).pipe(map((res:any)=>res));
  }

  downloadDeliveryNote(referenceNo):Observable<any>{
    return this.http.get(`${this.endpointService.pdfDownloads}` + '/'+ referenceNo + '.pdf', { responseType: 'blob' });
  }

  confirmGiftDelivery(giftRequestData:any){
    return this.http.post(`${this.gift_request_api_url}/confirm`,giftRequestData).pipe(map((res:any)=>res));
  }
}

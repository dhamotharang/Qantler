import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Observable } from 'rxjs';
import { LegalRequest } from '../model/legal-request/legal-request.model';
import { map } from 'rxjs/operators';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';

@Injectable()
export class LegalService {
  constructor(private http:HttpClient, private endpointService:EndPointService, public _ScrollToService:ScrollToService) { }
  triggerScrollTo()
    {
      const config: ScrollToConfigOptions = {
        target: 'destination',
        offset: 200
      };
     
      this._ScrollToService.scrollTo(config);
    }
    
  legal_request_api_url = this.endpointService.apiHostingURL+'/Legal';
  legal_homepage_api_url = this.endpointService.apiHostingURL+'/LegalHomePage/Legal';

  getAllLegalRequestsList(options:any):Observable<Array<LegalRequest>>{
    let reqParams:any = {};

    if(options.UserID >=0){
      reqParams.UserID = options.UserID;
    }

    if(options.Status){
      reqParams.Status = options.Status;
    }

    if(options.AttendedBy){
      reqParams.AttendedBy = options.AttendedBy;
    }

    if(options.UserName){
      reqParams.UserName = options.UserName;
    }

    if(options.SourceOU){
      reqParams.SourceOU = options.SourceOU;
    }

    if(options.Subject){
      reqParams.Subject = options.Subject;
    }

    if(options.ReqDateFrom){
      reqParams.ReqDateFrom = options.ReqDateFrom;
    }

    if(options.ReqDateTo){
      reqParams.ReqDateTo = options.ReqDateTo;
    }

    if(options.Label){
      reqParams.Label = options.Label;
    }

    if(options.Type){
      reqParams.Type = options.Type;
    }

    if(options.SmartSearch){
      reqParams.SmartSearch = options.SmartSearch;
    }
    
    return this.http.get(`${this.legal_homepage_api_url}/${options.PageNumber},${options.PageSize}`,{params:reqParams}).pipe(map((res:any)=>res));
  }

  getLegalRequestById(id:number,currentUserId:any):Observable<LegalRequest>{
    let param = {
      UserID:currentUserId
    };
    return this.http.get<LegalRequest>(`${this.legal_request_api_url}/${id}`,{params:param}).pipe(map((res:any)=>res));
  }

  addLegalRequest(legalRequestData:LegalRequest):Observable<any>{
    return this.http.post(`${this.legal_request_api_url}`,legalRequestData).pipe(map((res:any)=>res));
  }

  resubmitLegalRequest(legalRequestData:LegalRequest):Observable<any>{
    return this.http.put(`${this.legal_request_api_url}`,legalRequestData).pipe(map((res:any)=>res));
  }

  updateLegalRequestStatus(id:number,updateLegalRequestData:any):Observable<any>{
    return this.http.patch(`${this.legal_request_api_url}/${id}`,updateLegalRequestData).pipe(map((res:any)=>res));
  }

  downloadLegalRequestReport(id):Observable<any>{
    return this.http.post(`${this.legal_request_api_url}/Download`,{LegalRequestId:id}).pipe(map((res:any)=>res));
  }

  getLegalRequestsCount(userId):Observable<any>{
    return this.http.get(`${this.legal_homepage_api_url}/Home/Count/${userId}`).pipe(map((res:any)=>res));
  }

  saveLegalKeywords(keywordsData:any[],legalID:any,userid):Observable<any>{
    return this.http.post(`${this.legal_request_api_url}/Labels/${legalID}?UserID=${userid}`,keywordsData).pipe(map((res:any)=>res));
  }

  getLegalDocumentsList(options):Observable<any>{
    let toSendParams:any = {     
      Type:'Legal',
      UserID:options.UserID
    };
    if(options.Creator){
      toSendParams.Creator = options.Creator;
    }

    if(options.SmartSearch){
      toSendParams.SmartSearch = options.SmartSearch;
    }
    return this.http.get(`${this.legal_homepage_api_url}/Document/${options.PageNumber},${options.PageSize}`,{params:toSendParams}).pipe(map((res:any) => res));
  }


  deleteLegalDocument(options):Observable<any>{
    let toSendParams = {
      UserID:options.UserID
    };
    return this.http.delete(`${this.legal_homepage_api_url}/Document/${options.AttachmentID}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

}

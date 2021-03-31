import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';

@Injectable()
export class HrService {

  constructor(private http:HttpClient, private endpointService:EndPointService, public _ScrollToService:ScrollToService) { }
  triggerScrollTo()
  {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 300
    };

    this._ScrollToService.scrollTo(config);
  }

  hr_home_api_url = this.endpointService.apiHostingURL+'/HR/Home';
  hr_api_url = this.endpointService.apiHostingURL+'/HR';

  getHRAllHRModulesList(options:any):Observable<any>{
    let reqParams:any = {};

    if(options.UserID >=0){
      reqParams.UserID = options.UserID;
    }

    if(options.Status){
      reqParams.Status = options.Status;
    }

    if(options.RequestType){
      reqParams.RequestType = options.RequestType;
    }

    if(options.UserName){
      reqParams.UserName = options.UserName;
    }

    if(options.Creator){
      reqParams.Creator = options.Creator;
    }

    if(options.ReqDateFrom){
      reqParams.ReqDateFrom = options.ReqDateFrom;
    }

    if(options.ReqDateTo){
      reqParams.ReqDateTo = options.ReqDateTo;
    }

    if(options.SmartSearch){
      reqParams.SmartSearch = options.SmartSearch;
    }

   return this.http.get(`${this.hr_home_api_url}/AllModulesPending/${options.PageNumber},${options.PageSize}`,{params:reqParams}).pipe(map((res:any)=>res));
  }
  getHRMyProcessedRequestList(options:any):Observable<any>{
    let reqParams:any = {};

    if(options.UserID >=0){
      reqParams.UserID = options.UserID;
    }

    if(options.Status){
      reqParams.Status = options.Status;
    }

    if(options.RequestType){
      reqParams.RequestType = options.RequestType;
    }

    if(options.UserName){
      reqParams.UserName = options.UserName;
    }

    if(options.Creator){
      reqParams.Creator = options.Creator;
    }

    if(options.ReqDateFrom){
      reqParams.ReqDateFrom = options.ReqDateFrom;
    }

    if(options.ReqDateTo){
      reqParams.ReqDateTo = options.ReqDateTo;
    }

    if(options.SmartSearch){
      reqParams.SmartSearch = options.SmartSearch;
    }

    return this.http.get(`${this.hr_home_api_url}/MyProcessed/${options.PageNumber},${options.PageSize}`,{params:reqParams}).pipe(map((res:any)=>res));
  }

  getHRMyPendingRequestList(options:any):Observable<any>{
    let reqParams:any = {};

    if(options.UserID >=0){
      reqParams.UserID = options.UserID;
    }

    if(options.Status){
      reqParams.Status = options.Status;
    }

    if(options.RequestType){
      reqParams.RequestType = options.RequestType;
    }

    if(options.UserName){
      reqParams.UserName = options.UserName;
    }

    if(options.Creator){
      reqParams.Creator = options.Creator;
    }

    if(options.ReqDateFrom){
      reqParams.ReqDateFrom = options.ReqDateFrom;
    }

    if(options.ReqDateTo){
      reqParams.ReqDateTo = options.ReqDateTo;
    }

    if(options.SmartSearch){
      reqParams.SmartSearch = options.SmartSearch;
    }

    return this.http.get(`${this.hr_home_api_url}/MyPending/${options.PageNumber},${options.PageSize}`,{params:reqParams}).pipe(map((res:any)=>res));
  }

  getHRMyOwnRequestList(options:any):Observable<any>{
    let reqParams:any = {};

    if(options.UserID >=0){
      reqParams.UserID = options.UserID;
    }

    if(options.Status){
      reqParams.Status = options.Status;
    }

    if(options.RequestType){
      reqParams.RequestType = options.RequestType;
    }

    if(options.UserName){
      reqParams.UserName = options.UserName;
    }

    if(options.Creator){
      reqParams.Creator = options.Creator;
    }

    if(options.ReqDateFrom){
      reqParams.ReqDateFrom = options.ReqDateFrom;
    }

    if(options.ReqDateTo){
      reqParams.ReqDateTo = options.ReqDateTo;
    }

    if(options.SmartSearch){
      reqParams.SmartSearch = options.SmartSearch;
    }

   return this.http.get(`${this.hr_home_api_url}/MyOwnRequest/${options.PageNumber},${options.PageSize}`,{params:reqParams}).pipe(map((res:any)=>res));
  }


  getHRDashboardCount(id:number):Observable<any>{
   return this.http.get(`${this.hr_home_api_url}/AllModulesPendingCount/${id}`).pipe(map((res:any)=>res));
  }

  downloadHrReport(options):Observable<any>{
    return this.http.post(`${this.hr_api_url}`,options).pipe(map((res:any) =>res));
  }


  getHrDocumentsList(options):Observable<any>{
    let toSendParams:any = {
      Type:'HR',
      UserID:options.UserID
    };
    if(options.Creator){
      toSendParams.Creator = options.Creator;
    }

    if(options.SmartSearch){
      toSendParams.SmartSearch = options.SmartSearch;
    }
    return this.http.get(`${this.hr_api_url}/Document/${options.PageNumber},${options.PageSize}`,{params:toSendParams}).pipe(map((res:any) => res));
  }


  deleteHrDocument(options):Observable<any>{
    let toSendParams = {
      UserID:options.UserID
    };
    return this.http.delete(`${this.hr_api_url}/Document/${options.AttachmentID}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

}

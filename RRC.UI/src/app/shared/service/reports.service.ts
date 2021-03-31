import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ReportsService {

  hr_home_api_url = this.endpointService.apiHostingURL+'/HR/Home';
  hr_homepage_api_url = this.endpointService.apiHostingURL+'/HR';
  it_homepage_api_url = this.endpointService.apiHostingURL+'/ITSupport';
  leave_request_api_url = this.endpointService.apiHostingURL+'/Leave';
  legal_request_api_url = this.endpointService.apiHostingURL+'/Legal';
  legal_homepage_api_url = this.endpointService.apiHostingURL+'/LegalHomePage/Legal';
  gift_homepage_api_url = this.endpointService.apiHostingURL+'/Gift';
  calendar_homepage_api_url = this.endpointService.apiHostingURL + '/Calendar';
  vehicle_management_homepage_api_url = this.endpointService.apiHostingURL + '/VehicleManagement';
  vehicle_request_homepage_api_url = this.endpointService.apiHostingURL + '/vehicleRequest';


  constructor(private http:HttpClient, private endpointService:EndPointService) { }


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

  getLegalRequestsList(options:any):Observable<any>{
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

   return this.http.get(`${this.gift_homepage_api_url}/ListView${options.PageNumber},${options.PageSize}`,{params:reqParams}).pipe(map((res:any)=>res));
  }

  downloadHrReport(options):Observable<any>{
    let toSendParams:any = {
      type:'Excel'
    };
    if(options.reportType){
      toSendParams.type = options.reportType
    }
    return this.http.post(`${this.hr_homepage_api_url}/Report`,options,{ responseType:'blob',params:toSendParams}).pipe(map((res:any) =>res));
  }

  downloadItReport(options):Observable<any>{
    let toSendParams:any = {
      type:'Excel'
    };
    if(options.reportType){
      toSendParams.type = options.reportType
    }
    return this.http.post(`${this.it_homepage_api_url}/Report?type=Excel`,options,{ responseType:'blob'}).pipe(map((res:any) =>res));
  }

  downloadVehicleReport(options):Observable<any>{
    let toSendParams:any = {
      type:'Excel'
    };
    if(options.reportType){
      toSendParams.type = options.reportType
    }
    return this.http.post(`${this.vehicle_request_homepage_api_url}/Report?type=Excel`,options,{ responseType:'blob'}).pipe(map((res:any) =>res));
  }

  downloadModuleReport(options,moduleName?:string):Observable<any>{
    let reqUrl = '';
    if(moduleName){
      reqUrl = this[moduleName+'_homepage_api_url'];
    }else{
      reqUrl = this.hr_homepage_api_url;
    }
    let toSendParams:any = {
      type:'Excel'
    };
    if(options.reportType){
      toSendParams.type = options.reportType
    }
    return this.http.post(`${reqUrl}/Report`,options,{responseType:'blob',params:toSendParams}).pipe(map((res:any)=> res));
  }

  getLeaveRequestById(id:number,currentUserId:any):Observable<any>{
    let param = {
      UserID:currentUserId
    };
    return this.http.get<any>(`${this.leave_request_api_url}/${id}`,{params:param}).pipe(map((res:any)=>res));
  }
  getById(id:any, UserID: any) {
    let param = {
      UserID: UserID
    };
    return this.http.get(this.it_homepage_api_url+ '/' + id,{params:param});
  }
}

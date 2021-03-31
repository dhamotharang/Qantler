import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { IT } from '../model/it.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';

@Injectable()
export class ItService {
  itUrl: string = '/ITSupport/';
  attachment_api_url = this.endpoint.apiHostingURL+'/attachment';
  it_homepage_api_url = this.endpoint.apiHostingURL+'/ITSupport/Home/';

  constructor(private endpoint: EndPointService, private httpClient: HttpClient,public _ScrollToService:ScrollToService) { }

  triggerScrollTo()
  {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 200
    };
    
    this._ScrollToService.scrollTo(config);
  }
  
  create(it: IT) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.itUrl, it);
  }

  update(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + this.itUrl + id, dataToUpdate);
  }

  getById(id:any, UserID: any) {
    let param = {
      UserID: UserID
    };
    return this.httpClient.get(this.endpoint.apiHostingURL + this.itUrl + '/' + id,{params:param});
  }

  getList(query:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + this.itUrl + query);
  }

  getDepartments(id:any, userID:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + this.itUrl + id +'?UserID=' +  userID);
  }

  uploadAttachment(data:any):Observable<any>{
    var formData = new FormData();
    var headers = new HttpHeaders();
    for (var i = 0; i < data.length; i++) {
      formData.append('files', data[i]);
    }
    headers.append("Accept", 'application/json');
    headers.delete("Content-Type");
    const requestOptions = {
      headers: headers,
      reportProgress: true,
      observe:'events'
    };
    return this.httpClient.post(`${this.attachment_api_url}/Upload`,formData,{
      headers: headers,
      reportProgress: true,
      observe:'events'
    }).pipe(map((res:any)=>res));
  }

  getItRequestList(pageNumber:number, pageSize:number, RequestType, UserID, username, Status, SmartSearch, ReqDateFrom, ReqDateTo, SourceOU, Subject, priority,Type) {
    let toSendParams:any = {
      RequestType:RequestType,
      Priority: priority ,
      UserID : UserID ,
      Status: Status,
      SmartSearch:SmartSearch,
      ReqDateFrom:ReqDateFrom, 
      ReqDateTo:ReqDateTo,
      username:username,
      Subject:Subject,
      SourceOU:SourceOU,
      Type:Type
    };
    return this.httpClient.get(this.it_homepage_api_url+ '/List/'+ pageNumber + ',' + pageSize,{params:toSendParams});
  }

  getItDashboardCount(id:number):Observable<any>{
    return this.httpClient.get(`${this.it_homepage_api_url}Count/${id}`).pipe(map((res:any)=>res));
  }

  downloadItReport(options):Observable<any>{
    return this.httpClient.post(`${this.it_homepage_api_url}`,options).pipe(map((res:any) =>res));
  }

  getItDocumentsList(options):Observable<any>{
    let toSendParams:any = {
      Type:'IT',
      UserID:options.UserID
    };
    if(options.Creator){
      toSendParams.Creator = options.Creator;
    }

    if(options.SmartSearch){
      toSendParams.SmartSearch = options.SmartSearch;
    }
    return this.httpClient.get(`${this.it_homepage_api_url}Document/${options.PageNumber},${options.PageSize}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

  deleteItDocument(options):Observable<any>{
    let toSendParams = {
      UserID:options.UserID
    };
    return this.httpClient.delete(`${this.it_homepage_api_url}Document/${options.AttachmentID}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

  syncDate() {
    return this.httpClient.get(this.endpoint.apiHostingURL + this.itUrl +'/SyncDate');
  }

}

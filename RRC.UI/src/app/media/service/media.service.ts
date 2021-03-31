import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Observable } from 'rxjs';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  headers = new Headers();
  APIString = "Media";
  constructor(private httpClient: HttpClient, private endpoint: EndPointService,public _ScrollToService:ScrollToService) { }
  
  triggerScrollTo()
  {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 300
    };
    
    this._ScrollToService.scrollTo(config);
  }

  getMediaList(pageNumber:number, pageSize:number, RequestType, UserID, Status, SmartSearch, ReqDateFrom, ReqDateTo, SourceOU, requestName) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/Home/AllModulesPending/'+ pageNumber + ',' + pageSize + "?RequestType="+RequestType + "&Type="+ requestName + "&UserID=" + UserID + "&Status="+Status+ "&SmartSearch="+SmartSearch+ "&ReqDateFrom="+ReqDateFrom+ "&ReqDateTo="+ReqDateTo+ "&SourceOU="+SourceOU);
  }

  getMediaListCount(userId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/Home/AllModulesPendingCount/'+ userId);
  }

  getExport(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString+ '/Export?type=Excel', Data, { responseType: 'blob' });
  }

  getProtocolListCount(userId){
    return this.httpClient.get(this.endpoint.apiHostingURL + '/Protocol/Home/AllModulesPendingCount/'+ userId);
  }

  // getHrDocumentsList(options):Observable<any>{
  //   let toSendParams:any = {     
  //     Type:'HR',
  //     UserID:options.UserID
  //   };
  //   if(options.Creator){
  //     toSendParams.Creator = options.Creator;
  //   }

  //   if(options.SmartSearch){
  //     toSendParams.SmartSearch = options.SmartSearch;
  //   }
  //   return this.http.get(`${this.hr_api_url}/Document/${options.PageNumber},${options.PageSize}`,{params:toSendParams}).pipe(map((res:any) => res));
  // }


  // deleteHrDocument(options):Observable<any>{
  //   let toSendParams = {
  //     UserID:options.UserID
  //   };
  //   return this.http.delete(`${this.hr_api_url}/Document/${options.AttachmentID}`,{params:toSendParams}).pipe(map((res:any) => res));
  // }

}

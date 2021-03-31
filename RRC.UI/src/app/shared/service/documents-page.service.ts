import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { map } from 'rxjs/operators';
import { Attachment } from '../model/attachment/attachment.model';

@Injectable()
export class DocumentsPageService {
  attachment_api_url = this.endpointService.apiHostingURL+'/attachment';
  hr_homepage_api_url = this.endpointService.apiHostingURL+'/HR';
  media_homepage_api_url = this.endpointService.apiHostingURL+'/Media';
  legal_homepage_api_url = this.endpointService.apiHostingURL+'/LegalHomepage/Legal';
  itsupport_homepage_api_url = this.endpointService.apiHostingURL + '/ITSupport';
  maintenance_homepage_api_url = this.endpointService.apiHostingURL+'/Maintenance';
  vehicle_homepage_api_url = this.endpointService.apiHostingURL+'/vehicleRequest';

  documentUploadDetector:BehaviorSubject<boolean> = new BehaviorSubject(false);
  documentUploadObservable:Observable<any> = this.documentUploadDetector.asObservable();
  constructor(private http:HttpClient,private endpointService:EndPointService) { }

  getModuleDocumentsList(options,moduleName:string):Observable<any>{
    let toSendParams:any = {     
      Type:moduleName,
      UserID:options.UserID
    };
    if(options.Creator){
      toSendParams.Creator = options.Creator;
    }

    if(options.SmartSearch){
      toSendParams.SmartSearch = options.SmartSearch;
    }
    let reqUrl = '';
    debugger;
    if(moduleName){
      reqUrl = this[moduleName.toLowerCase()+'_homepage_api_url'];
    }else{
      reqUrl = this.hr_homepage_api_url;
    }
    return this.http.get(`${reqUrl}/Document/${options.PageNumber},${options.PageSize}`,{params:toSendParams}).pipe(map((res:any) => res));
  }


  deleteModuleDocument(options,moduleName):Observable<any>{
    let toSendParams = {
      UserID:options.UserID
    };
    let reqUrl = '';
    if(moduleName){
      reqUrl = this[moduleName.toLowerCase()+'_homepage_api_url'];
    }else{
      reqUrl = this.hr_homepage_api_url;
    }
    return this.http.delete(`${reqUrl}/Document/${options.AttachmentID}`,{params:toSendParams}).pipe(map((res:any) => res));
  }

  uploadModuleAttachment(attachmentOptions):Observable<any>{
    let payload = new FormData();    
    var headers = new HttpHeaders();
      payload.append('File', attachmentOptions.data);
    headers.append("Accept", 'application/json');
    headers.delete("Content-Type");
    return this.http.post(`${this.attachment_api_url}/Upload`,payload,{
      headers:headers,
      reportProgress: true,
      observe:'events'
    }).pipe(map((res:any)=>res));
  }

  sendModuleAttachments(fileData:any,moduleName?:string):Observable<Attachment>{
    let reqUrl = '';
    if(moduleName){
      reqUrl = this[moduleName.toLowerCase()+'_homepage_api_url'];
    }else{
      reqUrl = this.hr_homepage_api_url;
    }

    return this.http.post(`${reqUrl}/Document`,fileData).pipe(map((res:any)=>res));
  }
}

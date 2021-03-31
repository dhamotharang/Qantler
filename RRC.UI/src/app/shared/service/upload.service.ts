import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { Attachment } from '../model/attachment/attachment.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class UploadService {
  attachment_api_url = this.endpointService.apiHostingURL+'/attachment';
  hr_homepage_api_url = this.endpointService.apiHostingURL+'/HR';
  legal_homepage_api_url = this.endpointService.apiHostingURL+'/LegalHomepage/Legal';
  hrDocumentUploadDetector:BehaviorSubject <boolean> = new BehaviorSubject(false);
  hrDocumentuploaderObservable:Observable<any> = this.hrDocumentUploadDetector.asObservable();
  documentUploadDetector:BehaviorSubject<boolean> = new BehaviorSubject(false);
  documentUploadObservable:Observable<any> = this.documentUploadDetector.asObservable();
  itDocumentUploadDetector:BehaviorSubject <boolean> = new BehaviorSubject(false);
  itDocumentuploaderObservable:Observable<any> = this.itDocumentUploadDetector.asObservable();
  constructor(private http:HttpClient,private endpointService:EndPointService) { }

  uploadAttachment(files:any):Observable<Attachment>{
    let payload = new FormData();    
    let headers = new HttpHeaders();
    for (let i = 0; i < files.length; i++) {
      payload.append('files', files[i]);
    }
    headers.append("Accept", 'application/json');
    headers.delete("Content-Type");
    return this.http.post(`${this.attachment_api_url}/upload`,payload,{
      headers:headers,
      reportProgress: true,
      observe:'events'
    }).pipe(map((res:any)=>res));
  }

  downloadAttachment(attachmentData:any):Observable<Blob>{
    return this.http.get(`${this.attachment_api_url}/download`, {      
      params: {
        filename:  attachmentData.AttachmentsName,
        guid: attachmentData.AttachmentGuid
      },
      responseType:'blob'
    }).pipe(map((res:any)=>res));
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

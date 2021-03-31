import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';

@Injectable({
  providedIn: 'root'
})
export class MediapressService {
  APIString = "PressRelease";

  constructor(private httpClient: HttpClient, private endpoint: EndPointService) { }

  savePressRelease(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }
  getRequestById(id:any, userId:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + id + '?UserId=' + userId);
  }
  updateRequest(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + this.APIString + '/' + id, dataToUpdate);
  }
  resubmitRequest(Data:any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }
  
}

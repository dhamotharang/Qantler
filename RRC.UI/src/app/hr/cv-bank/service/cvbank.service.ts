import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';
import { Cvbank } from '../model/cvbank.model';

@Injectable({
  providedIn: 'root'
})
export class CvbankService {
  uploadUrl:string = '/Attachments/Upload/';
  cvbankUrl:string = '/CVBank/';
  listUrl:string = '/CVBank/Home/List/';
  
  constructor(private endpoint: EndPointService, private httpClient: HttpClient) { }

  uploadFile(formData) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.uploadUrl, formData);
  }

  create(cvbankData: Cvbank) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.cvbankUrl, cvbankData);
  }

  getCv(id:any, userId:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + this.cvbankUrl + id + '?userid=' + userId);
  }

  getCVList(page:Number, count:Number, query:string) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}${this.listUrl}${page},${count}${query}`);
  }

  getReport(reportData:any) {
    return this.httpClient.post(`${this.endpoint.apiHostingURL}${this.cvbankUrl}/Report?type=Excel`, reportData, {responseType: 'blob'});
  }
}

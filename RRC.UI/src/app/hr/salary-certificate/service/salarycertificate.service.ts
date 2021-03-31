import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../../api/endpoint.service';

@Injectable()

export class SalarycertificateService {
  headers = new Headers();
  APIString = "Certificate";
  // apiurl = "http://localhost:3000"
  constructor(private httpClient: HttpClient, private endpoint: EndPointService) {

  }

  saveSalaryCert(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }
  getCertificate(id, userId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + id + '?UserId=' + userId);
  }

  statusChange(APIString, id, Status: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, Status);
  }
}

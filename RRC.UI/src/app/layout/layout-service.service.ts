import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from '../api/endpoint.service';

@Injectable({
  providedIn: 'root'
})
export class LayoutServiceService {
  APIString = "UserProfile";
  constructor(private httpClient: HttpClient, private endpoint: EndPointService) { }

  moduleCount(id) {
    return this.httpClient.get(this.endpoint.apiHostingURL +'/Home/ModulesCount?UserID='+id);
  }

  getEmpDetails(id, userId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + id+ '?UserId=' + userId);
  }
}

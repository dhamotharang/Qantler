import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../../api/endpoint.service';
import { ScrollToConfigOptions, ScrollToService } from '@nicky-lenaers/ngx-scroll-to';

@Injectable()

export class EmployeeService {
  headers = new Headers();
  APIString = "UserProfile";
  // apiurl = "http://localhost:3000"
  constructor(private httpClient: HttpClient, private endpoint: EndPointService, public _ScrollToService: ScrollToService) {

  }

  triggerScrollTo()
  {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 300
    };
    
    this._ScrollToService.scrollTo(config);
  }

  saveEmpData(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }
  updateEmpData(Data: any, userId) {
    Data.UpdatedBy = userId;
    Data.UpdatedDateTime = new Date();
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + this.APIString+ '?UserID=' + userId, Data);
  }
  getDetails(APIString, userId ) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString+ '?Userid=' + userId);
  }

  statusChange(APIString, id, Status: any, userId) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id+ '?Userid=' + userId, Status);
  }

  getEmpDetails(id, userId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + id+ '?UserId=' + userId);
  }

  getUserProfileList(pageNumber:number,pageSize:number,department, username, smartsearch, jobTitle, type) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/Home/list/'+ pageNumber + ',' + pageSize + "?UserName="+username + "&OrgUnitID=" + department + "&SmartSearch="+smartsearch+ "&Type="+type+ "&JobTitle="+jobTitle);
  }

  deleteProfile(id, userId) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + id+ '?UserId=' + userId);
  }

  getEmpDashboard(userId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/Home/Count/'+ userId);
  }
}


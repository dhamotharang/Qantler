import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';

@Injectable()
export class DropdownsService {

  constructor(private httpClient: HttpClient, private endpoint: EndPointService) {}

  getCountries(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Country?UserID=${userId}`);
  }

  getBabyAdditionCountries(userId:any, modulename:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Country?UserID=${userId}&Module=${modulename}`);
  }

  getCities(userId:any, countryId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/City?UserID=${userId}&CountryID=${countryId}`);
  }

  getSpecializations(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Specialization?UserID=${userId}`);
  }

  getEducations(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Education?UserID=${userId}`);
  }

  getExperience(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Experience?UserID=${userId}`);
  }

  getUserList(organization: any) {
    let orgData = [{
      "OrganizationID": organization.OrganizationID,
      "OrganizationUnits": organization.OrganizationUnits
    }];
    return this.httpClient.post(`${this.endpoint.apiHostingURL}/User`,orgData);
  }

  getGrade(userId:any){
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Grade?UserId=${userId}`);
  }

  getTitle(userId:any){
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Title?UserId=${userId}`);
  }

  getEmpStatus(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/EmployeeStatus?UserID=${userId}`);
  }

  getLanguages(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Languages?UserID=${userId}`);
  }

  getMediaChannels(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/MediaChannel?UserID=${userId}`);
  }

  getOfficialTaskType(userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/OfficialTaskRequest?UserID=${userId}`);
  }

  getMeetingType(userId:any){
    return this.httpClient.get(`${this.endpoint.apiHostingURL}/Meeting?UserID=${userId}`);
  }
}

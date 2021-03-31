import { Injectable } from '@angular/core';
import { Maintenance } from '../model/maintenance.model';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';
import { Communication } from '../model/communication.model';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';

@Injectable({
  providedIn: 'root'
})
export class MaintenanceService {
  uploadUrl:string = '/Attachments/Upload/';
  maintenanceUrl:string = '/Maintenance/';
  chatUrl:string = '/Maintenance/CommunicationChat/';
  dashboardUrl:string = '/Maintenance/Home/Count/';
  homeUrl:string = '/Maintenance/Home/List/'

  constructor(private endpoint: EndPointService, private httpClient: HttpClient, public _ScrollToService:ScrollToService) { }
  triggerScrollTo()
  {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 200
    };
   
    this._ScrollToService.scrollTo(config);
  }
  
  create(maintenance: Maintenance) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.maintenanceUrl, maintenance);
  }

  reSubmit(dataToUpdate:any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + this.maintenanceUrl, dataToUpdate);
  }

  update(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + this.maintenanceUrl + id, dataToUpdate);
  }

  updateAttachment(id:any, userId:any, attachment:any) {
    return this.httpClient.put(`${this.endpoint.apiHostingURL}${this.maintenanceUrl}Attachment/${id}?UserID=${userId}`, attachment);
  }

  getById(id:any, userId:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}${this.maintenanceUrl}${id}?UserId=${userId}`);
  }

  getMaintenanceList(page:Number, pageSize:Number, query:any) {
    return this.httpClient.get(`${this.endpoint.apiHostingURL}${this.homeUrl}${page},${pageSize}${query}`);
  }

  createChat(communication: Communication) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.chatUrl, communication);
  }

  getMaintenanceDashboard(userId:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + this.dashboardUrl + userId);
  }

  getReport(reportData:any) {
    return this.httpClient.post(`${this.endpoint.apiHostingURL}${this.maintenanceUrl}/Report?type=Excel`, reportData, {responseType: 'blob'});
  }
}

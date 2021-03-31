import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EndPointService } from 'src/app/api/endpoint.service';
import { CalendarManagement } from '../model/calendar-management.model';
import { ScrollToService, ScrollToConfigOptions } from '@nicky-lenaers/ngx-scroll-to';

@Injectable({
  providedIn: 'root'
})
export class CalendarManagementService {
  calendarUrl = '/Calendar';
  constructor(
    public endPoint: EndPointService,
    public httpClient: HttpClient,
    public _ScrollToService:ScrollToService
  ) { }

  triggerScrollTo()
  {
    const config: ScrollToConfigOptions = {
      target: 'destination',
      offset: 200
    };

    this._ScrollToService.scrollTo(config);
  }

  holidays(UserID: any, Month: any, Year: any, searchText?: any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/Master/Holiday/' + Month + ',' + Year + '?UserID=' + UserID, {params: {SmartSearch: searchText}});
  }

  eventTypes(UserID: any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/Event?UserID=' + UserID);
  }

  cityList(UserID: any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/City?UserID=' + UserID);
  }

  locationList(UserID: any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/Location?UserID=' + UserID);
  }

  getById(id: any, userId: any) {
    return this.httpClient.get(`${this.endPoint.apiHostingURL}${this.calendarUrl}/${id}?UserId=${userId}`);
  }

  createEvent(calendarManagement: CalendarManagement) {
    return this.httpClient.post(this.endPoint.apiHostingURL + this.calendarUrl, calendarManagement);
  }

  updateEvent(calendarManagement: CalendarManagement) {
    return this.httpClient.put(this.endPoint.apiHostingURL  + this.calendarUrl, calendarManagement);
  }

  getevent(id: any, UserID: any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + this.calendarUrl + '/' + id + '?UserID=' + UserID);
  }

  getList(options:any) {
    let toSendParams :any = {
      UserID:options.UserID,
      Type:options.Type,
      ReferenceNumber:options.ReferenceNumber,
      EventType:options.EventType,
      EventRequestor:options.EventRequestor,
      DateFrom: options.StartDate,
      DateTo: options.EndDate,
      Status: options.Status,
      SmartSearch:options.SmartSearch
    }
    return this.httpClient.get(this.endPoint.apiHostingURL + this.calendarUrl + '/ListView/' + options.pageNumber + ',' + options.pageSize ,{params:toSendParams});
  }

  listBulkView(UserID: any, ReferenceNumber: any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + this.calendarUrl + '/ListBulkView' + '?UserID=' + UserID +
      '&ReferenceNumber=' + ReferenceNumber);
  }

  fullCalendarData(UserID: any, Month: any, Year: any, searchText?: any) {
    // return this.httpClient.get(this.endPoint.apiHostingURL + this.calendarUrl + '/CalendarView/' + id + '?UserID=' + UserID);
    return this.httpClient.get(this.endPoint.apiHostingURL + this.calendarUrl + '/CalendarView?UserID=' + UserID + '&Month=' + Month + '&Year=' + Year, {params: {SmartSearch: searchText}});
  }

  update(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endPoint.apiHostingURL + this.calendarUrl + '/' + id, dataToUpdate);
  }

  bulkEventUpdate(dataToUpload: any) {
    return this.httpClient.post(this.endPoint.apiHostingURL + this.calendarUrl + '/BulkAction/', dataToUpload );
  }

  downloadEventReport(data: any) {
    return this.httpClient.post(this.endPoint.apiHostingURL + this.calendarUrl + '/report', data);
  }

  getCardCount(UserID:any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/Calendar/Count?UserID=' + UserID);
  }

  bulkApology(dataToUpload: any){
    return this.httpClient.post(this.endPoint.apiHostingURL + this.calendarUrl + '/Apology/', dataToUpload );
  }
}

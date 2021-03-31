import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MeetingService {
  APIString = "Meeting";

  constructor(private httpClient: HttpClient, private endpoint: EndPointService) { }

  saveMeeting(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }
  reSheduleMeeting(Data: any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }
  getRequestById(id: any, userId: any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/' + id + '?UserId=' + userId);
  }

  getMeetingList(pageNumber:number, pageSize:number, smartSearch, UserID, MeetingID, Subject, StartDatetime, EndDatetime, MeetingType, location, invitees) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/ListView'+ pageNumber + ',' + pageSize + "?ReferenceNumber="+MeetingID + "&Subject="+ Subject +
    "&UserId=" + UserID + "&StartDatetime="+StartDatetime+
    "&EndDatetime="+EndDatetime+ "&MeetingType="+MeetingType+ "&Invitees="+invitees+ "&location="+location+ "&SmartSearch="+smartSearch);
  }
  getMeeting(id:any, userId:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString + '/' + id + '?UserId=' + userId);
  }
  saveMOM(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString + '/MOM', Data);
  }
  cancelMeeting(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + this.APIString + '/' + id, dataToUpdate);
  }
  getMOM(id:any, userId:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString + '/MOM/' + id + '?UserId=' + userId);
  }
  getExport(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString+ '/Report?type=Excel', Data, { responseType: 'blob' });
  }
  downloadMOM(id: any, userId: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString+ '/MOM/Download/' + id + '?UserId=' + userId,{ responseType: 'arraybuffer' });
  }
  downloadPDFMOM(meetingId: any) {
    return this.httpClient.get(`${this.endpoint.pdfDownloads}` + '/' + meetingId + '.pdf', { responseType: 'blob' });
  }
}

import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';
import { Announcement } from '../model/announcement.model';

@Injectable({
  providedIn: 'root'
})
export class AnnouncementService {

  annEndpoint: string = '/Announcement/';
  constructor(private endpoint: EndPointService, private httpClient: HttpClient) { }


  createAnnouncement(announcement: Announcement) {
    return this.httpClient.post(this.endpoint.apiHostingURL + this.annEndpoint, announcement);
  }

  getAnnoucement(id:any, userId:any) {
    return this.httpClient.get(this.endpoint.apiHostingURL + this.annEndpoint + id + '?UserID=' + userId);
  }

  updateAnnouncement(id:any, dataToupdate: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + this.annEndpoint + id, dataToupdate);
  }

}

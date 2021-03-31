import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';
import { Photographer } from '../model/photographer.model';

@Injectable()

export class PhotographerService {
  photographerUrl: string = '/Photographer/';

  constructor(
    public endPoint: EndPointService,
    public httpClient: HttpClient
  ) { }

  create(photographer: Photographer) {
    return this.httpClient.post(this.endPoint.apiHostingURL + this.photographerUrl, photographer);
  }

  view(id: string, UserID: any){
    return this.httpClient.get(`${this.endPoint.apiHostingURL}${this.photographerUrl}${id}?UserId=${UserID}`);
  }

  getAllData(id: any, UserID: any) {
    return this.httpClient.get(this.endPoint.apiHostingURL + this.photographerUrl + id + '?UserID=' + UserID);
  }

  update(id:any, dataToUpdate:any) {
    return this.httpClient.patch(this.endPoint.apiHostingURL + this.photographerUrl + id, dataToUpdate);
  }

  reSubmission(data:any){
    return this.httpClient.put(this.endPoint.apiHostingURL + this.photographerUrl, data);
  }
}

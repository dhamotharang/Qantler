import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ManagePhotoService {
  APIString = 'Photos';
  
  constructor(
    public endPoint: EndPointService,
    public httpClient: HttpClient
  ) { }

  create(data: any) {
    return this.httpClient.post(this.endPoint.apiHostingURL + '/' + this.APIString, data);
  }

  loadAllPhotos(page: number, pageSize: number) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/' + this.APIString + '/List/' + page + ',' + pageSize);
  }
  getPhotos(pageNumber:number, pageSize:number, UserID) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/' + this.APIString+ '/list/'+ pageNumber + ',' + pageSize+"?UserId=" + UserID);
  }
  deletePhotosById(newId) {
    let toSendParams = {
      UserID: newId
    };
    return this.httpClient.delete(this.endPoint.apiHostingURL + '/' + this.APIString+ '/'+ newId, { params:toSendParams });
  }
  createBanner(data: any, APIString) {
    return this.httpClient.post(this.endPoint.apiHostingURL + '/' + APIString, data);
  }
  getBanner(APIString) {
    return this.httpClient.get(this.endPoint.apiHostingURL + '/' + APIString);
  }
  deletePhotoById(newId) {
    return this.httpClient.delete(this.endPoint.apiHostingURL + '/' + this.APIString+ '/'+ newId);
  }
}

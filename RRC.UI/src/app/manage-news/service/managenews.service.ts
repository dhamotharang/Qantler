import { Injectable } from '@angular/core';
import { EndPointService } from 'src/app/api/endpoint.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ManagenewsService {
  APIString = "News";

  constructor(private httpClient: HttpClient, private endpoint: EndPointService) { }

  saveNews(Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }
  updateNews(Data: any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + this.APIString, Data);
  }  
  getNews(pageNumber:number, pageSize:number, UserID,Description) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/list/'+ pageNumber + ',' + pageSize+"?UserId=" + UserID+"&Description="+ Description);
  }
  getNewsById(UserID,newId) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + this.APIString+ '/'+ newId+"?UserId="+UserID);
  }
  deleteNewsById(newId) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + this.APIString+ '/'+ newId);
  }
}

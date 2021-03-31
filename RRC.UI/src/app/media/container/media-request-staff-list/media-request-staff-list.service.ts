import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { EndPointService } from './../../../api/endpoint.service';
import { Headers, RequestOptions, URLSearchParams, Http, HttpModule } from '@angular/http';

@Injectable({
  providedIn: 'root'
})
export class MediaRequestStaffListService {
  private headers = new Headers({ 'Content-Type': 'application/json', 'Accept': 'q=0.8;application/json;q=0.9' });

  constructor(public endpoint: EndPointService, public httpClient: HttpClient, private http: Http) { }
  memoList(APIString: string, pageNo, pageSize, type, userName) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName);
  }
  memoCombos(APIString: string, memo_id, user_id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + memo_id + '?userid=' + user_id);
  }
  // public memoFilterList(pageNo, pageSize, data): Promise<any> {
  //   let options = new RequestOptions({ headers: this.headers });
  //   //memos/1,10Type=&Username=&Status=&SourceOU=&DestinationOU=&Private=&Priority=&DateFrom=&DateTo=&SmartSearch=
  //   const url = this.endpoint.apiHostingURL + '/report/' + pageNo + ',' + pageSize;
  //   return this.http
  //     .post(url, data, options)
  //     .toPromise()
  //     .then(res => res.json() as any)
  //     .catch(this.handleError);

  //   //return this.httpClient.post(this.endpoint.apiHostingURL+'/User',{});
  // }

  userList(APIString: string, Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }

  memoFilterList(APIString: string, pageNo, pageSize, type, userName, Status, SourceOU, DestinationOU, Private, Priority, DateFrom, DateTo, SmartSearch) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName+ '&Status=' + Status+ '&SourceOU=' + SourceOU+ '&DestinationOU=' + DestinationOU+ '&Private=' + Private+ '&Priority=' + Priority+ '&DateFrom=' + DateFrom+ '&DateTo=' + DateTo+ '&SmartSearch=' + SmartSearch);
  }
  private handleError(error: any): Promise<any> {
    //console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}
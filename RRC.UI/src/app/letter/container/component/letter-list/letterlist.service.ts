import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { EndPointService } from '../../../../api/endpoint.service';
import { Headers, RequestOptions, URLSearchParams, Http, HttpModule } from '@angular/http';

@Injectable({
  providedIn: 'root'
})
export class LetterListService {
  private headers = new Headers({ 'Content-Type': 'application/json', 'Accept': 'q=0.8;application/json;q=0.9' });

  constructor(public endpoint: EndPointService, public httpClient: HttpClient, private http: Http) { }
  // memoList(APIString: string, pageNo, pageSize, type, userName) {
  //   return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName);
  // }

  memoList(APIString: string, pageNo, pageSize, type, userName) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName);
  }
  
  memoCombos(APIString: string, type, UserID) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + 1 + ',' + 1 + '?Type=' + type + '&UserID=' + UserID);
  }
  BulkApproval(APIString: string,user_id, Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString+ '?UserID=' + user_id, Data);
  }
 BulkApprovals(APIString: string,user_id, Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString+ '?UserID=' + user_id, Data,{responseType: 'blob'})
     .subscribe((resultBlob: Blob)=>{
          var a = document.createElement("a");
          a.href = URL.createObjectURL(resultBlob);
          a.download = 'Letter Download';
          // start download
          a.click()});
  }
  userList(APIString: string, Data: any) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }

  letterFilterList(APIString: string, pageNo, pageSize, type, UserID, Status, SourceOU, DestinationOU, UserName, DateFrom, DateTo, Priority,SenderName,SmartSearch) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&UserID=' + UserID+ '&Status=' + Status+ '&Source=' + SourceOU+ '&Destination=' + DestinationOU+ '&UserName=' + UserName+ '&DateRangeFrom=' + DateFrom+ '&DateRangeTo=' + DateTo+'&Priority=' + Priority+ '&SenderName=' + SenderName + '&SmartSearch=' + SmartSearch);
  }
  private handleError(error: any): Promise<any> {
    //console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
    downlaodExcel(data){
    var date = new Date,
    cur_date = date.getDate() +'-'+(date.getMonth()+1)+'-'+date.getFullYear();
    this.httpClient.post(this.endpoint.apiHostingURL + '/InboundLetter/Report?type=Excel',data,{responseType: 'blob'})
    .subscribe((resultBlob: Blob)=>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'Letters Report-'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
    });
  }
}
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { EndPointService } from '../../api/endpoint.service';
import { Headers, RequestOptions, URLSearchParams, Http, HttpModule } from '@angular/http';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class MemoListService {
  private headers = new Headers({ 'Content-Type': 'application/json', 'Accept': 'q=0.8;application/json;q=0.9' });

  constructor(public endpoint: EndPointService, public httpClient: HttpClient, private http: Http,private sanitizer: DomSanitizer) { }
  memoList(APIString: string, pageNo, pageSize, type, userName) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName);
  }
  memoCombos(APIString: string, memo_id, user_id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + memo_id + '?userid=' + user_id);
  }
  memoFilterList(APIString: string, pageNo, pageSize, type, userName, Status, SourceOU, DestinationOU, Private, Priority, DateFrom, DateTo, SmartSearch) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName+ '&Status=' + Status+ '&SourceOU=' + SourceOU+ '&DestinationOU=' + DestinationOU+ '&Private=' + Private+ '&Priority=' + Priority+ '&DateFrom=' + DateFrom+ '&DateTo=' + DateTo+ '&SmartSearch=' + SmartSearch);
  }
  downlaodExcel(data){
    var date = new Date,
    cur_date = date.getDate() +'-'+(date.getMonth()+1)+'-'+date.getFullYear();
    this.httpClient.post(this.endpoint.apiHostingURL + '/Memo/Report?type=Excel',data,{responseType: 'blob'})
    .subscribe((resultBlob: Blob)=>{
      var url = window.URL.createObjectURL(resultBlob);
      var a = document.createElement('a');
      document.body.appendChild(a);
      a.setAttribute('style', 'display: none');
      a.href = url;
      a.download = 'Memos Report-'+cur_date+'.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove(); // remove the element
      // const blob = new Blob([data], { type: 'application/octet-stream' });
      // var url:any = window.URL.createObjectURL(blob);
      // window.open(url);
    });
    
    // this.httpClient.post(this.endpoint.apiHostingURL + '/Memo/Report?type=Excel',data,{responseType: 'arraybuffer'} )
    //   .subscribe(response => this.downLoadFile(response, "application/ms-excel"));
  }

  //  /**
  //    * Method is use to download file.
  //    * @param data - Array Buffer data
  //    * @param type - type of the document.
  //    */
  //   downLoadFile(data: any, type: string) {
  //     let blob = new Blob([data], { type: type});
  //     let url = window.URL.createObjectURL(blob);
  //     let pwa = window.open(url);
  //     if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
  //         alert( 'Please disable your Pop-up blocker and try again.');
  //     }
  // }
  private handleError(error: any): Promise<any> {
    //console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}
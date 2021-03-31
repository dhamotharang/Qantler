import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Headers, RequestOptions, URLSearchParams, Http, HttpModule } from '@angular/http';
import { EndPointService } from '../../api/endpoint.service';
import { CommonService } from 'src/app/common.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CircularService {
  private headers = new Headers({ 'Content-Type': 'application/json', 'Accept': 'q=0.8;application/json;q=0.9' });

  constructor(public endpoint: EndPointService, public httpClient: HttpClient, private http: Http, public common: CommonService) { }

  private extractData(res: any) {
    let body = res.json();
    return body || [];
  }
  // memoCombos(APIString: string, memo_id, user_id) {
  //   return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + memo_id + '?userid=' + user_id);
  // }

  circularList(APIString: string, pageNo, pageSize, type, userName) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userName);
  }

  circularFilterList(APIString: string, pageNo, pageSize, type, userID, Status, SourceOU, DestinationOU, Priority, DateFrom, DateTo, SmartSearch) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + pageNo + ',' + pageSize + '?Type=' + type + '&Username=' + userID + '&Status=' + Status + '&Source=' + SourceOU + '&Destination=' + DestinationOU + '&Priority=' + Priority + '&DateRangeFrom=' + DateFrom + '&DateRangeTo=' + DateTo + '&SmartSearch=' + SmartSearch);
  }

  saveCircular(APIString: string, Data: any) {
    // const httpOptions = {
    //   headers: new HttpHeaders({
    //     'Content-Type':  'application/json',
    //     //'Authorization': 'my-auth-token'
    //   })
    // }
    // let headers=new HttpHeaders();
    // headers.append("Content-Type","multipart/form-data");
    // headers.append("Accept","application/json");
    // headers.append("Access-Control-Allow-Origin","true");
    // let options={headers:headers};
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }

  updateCircular(APIString: string, id, Data: any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }

  getCircular(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Userid=' + userid);
  }

  statusChange(APIString: string, id, Status: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, Status);

  }

  circularCombos(APIString: string, cirid, user_id) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + cirid + '?userid=' + user_id);
  }

  deleteCircular(APIString: string, id) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + APIString + '/' + id);
  }
  printPreview(APIString, id, userid) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Userid=' + userid, '');
  }

  cloneCircular(APIString: string, id: any, userid, data) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, data);
  }

  pdfToJson(refno) {
    return this.httpClient.get(environment.DownloadUrl + refno + '.pdf', { responseType: 'arraybuffer' as 'json' });
  }

  downlaodExcel(data) {
    var date = new Date,
      cur_date = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
    this.httpClient.post(this.endpoint.apiHostingURL + '/Circular/export?type=Excel', data, { responseType: 'blob' })
      .subscribe((resultBlob: Blob) => {
        var url = window.URL.createObjectURL(resultBlob);
        var a = document.createElement('a');
        document.body.appendChild(a);
        a.setAttribute('style', 'display: none');
        a.href = url;
        a.download = 'Circular Report-' + cur_date + '.xlsx';
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
  saveClone(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?UserID=' + userid);
  }

  breadscrumChange(status, type, id, lang = 'en') {
    switch (type) {
      case 'Create':
        if (lang == 'en')
          this.common.breadscrumChange('Internal Circulars', 'Circular Creation', '');
        else
          this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabic('circularcreate'), '');
        break;
      case 'List':
        if (lang == 'en')
          this.common.breadscrumChange('Internal Circulars', status, 'List');
        else
          this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabicNav(status), this.arabic('list'));
        break;
      default:
          let currentBreadcrumbs = 'Internal Circulars >> ' + status + ' >> ' + id;
          localStorage.setItem("BreadcrumbsURL",currentBreadcrumbs);
        switch (status) {
          case 1:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Circulars', 'My Pending Circulars', id);
            else
              this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabicNav('My Pending Circulars'), id);
            break;
          case 2:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Circulars', 'Outgoing Circulars', id);
            else
              this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabicNav('Outgoing Circulars'), id);
            break;
          case 3:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Circulars', 'Incoming Circulars', id);
            else
              this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabicNav('Incoming Circulars'), id);

            break;
          case 4:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Circulars', 'Draft Circulars', id);
            else
              this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabicNav('Draft Circulars'), id);
            break;
          case 5:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Circulars', 'Historical Circulars Incoming', id);
            else
              this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabicNav('Historical Circulars Incoming'), id);
            break;
          case 6:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Circulars', 'Historical Circulars Outgoing', id);
            else
              this.common.breadscrumChange(this.arabic('internalcirculars'), this.arabicNav('Historical Circulars Outgoing'), id);

            break;
        }
        break;
    }
  }

  arabicNav(word) {
    return this.common.arabic.sideNavArabic[word];
  }
  arabic(word) {
    return this.common.arabic.words[word];
  }


  private handleError(error: any): Promise<any> {
    //console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}

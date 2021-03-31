import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpRequest } from '@angular/common/http';
import { EndPointService } from '../../api/endpoint.service';
import { CommonService } from 'src/app/common.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MemoService {

  constructor(private httpClient: HttpClient, private endpoint: EndPointService, public common: CommonService) { }
  // url = "http://qtsp2016.centralus.cloudapp.azure.com/rrc/api/";
  headers = new Headers();

  saveMemo(APIString: string, Data: any) {
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

  cloneMemo(APIString, id, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Userid=' + userid);
  }

  updateMemo(APIString: string, id, Data: any) {
    return this.httpClient.put(this.endpoint.apiHostingURL + '/' + APIString, Data);
  }

  getMemo(APIString: string, id: any, userid) {
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Userid=' + userid);
  }

  statusChange(APIString: string, id, Status: any) {
    return this.httpClient.patch(this.endpoint.apiHostingURL + '/' + APIString + '/' + id, Status);
  }

  updateKeywords(APIString: string, id, Data: any, userid){
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString+ '/' + id + '?UserID=' + userid, Data);
  }

  deleteMemo(APIString: string, id) {
    return this.httpClient.delete(this.endpoint.apiHostingURL + '/' + APIString + '/' + id);
  }

  printPreview(APIString, id, userid) {
    return this.httpClient.post(this.endpoint.apiHostingURL + '/' + APIString + '/' + id + '?Userid=' + userid, '');
  }

  pdfToJson(refno) {
    return this.httpClient.get(environment.DownloadUrl + refno + '.pdf', { responseType: 'arraybuffer' as 'json' });
  }

  deleteGeneratedFiles(APIString, refNo){
    return this.httpClient.get(this.endpoint.apiHostingURL + '/' + APIString + '/' + refNo);
  }


  breadscrumChange(status, type, id, lang = 'en') {
    switch (type) {
      case 'Create':
        if (lang == 'en')
          this.common.breadscrumChange('Internal Memos', 'Memo Creation', '');
        else
          this.common.breadscrumChange(this.arabic('internalmemos'), this.arabic('memocreate'), '');
        break;
      case 'List':
        if (lang == 'en')
          this.common.breadscrumChange('Internal Memos', status, 'List');
        else
          this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav(status), this.arabic('list'));
        break;
      default:
          let currentBreadcrumbs = 'Internal Memos >> ' + status + ' >> Memo' + id;
          localStorage.setItem("BreadcrumbsURL",currentBreadcrumbs);
        switch (status) {
          case 1:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Memos', 'My Pending Actions', 'Memo ' + id);
            else
              this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav('Pending Memo List'), this.arabic('memo') + ' ' + id);
            break;
          case 2:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Memos', 'Outgoing Memos', 'Memo ' + id);
            else
              this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav('Outgoing Memos'), this.arabic('memo') + ' ' + id);
            break;
          case 3:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Memos', 'Incoming Memos', 'Memo ' + id);
            else
              this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav('Incoming Memos'), this.arabic('memo') + ' ' + id);
            break;
          case 4:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Memos', 'Draft Memos', 'Memo ' + id);
            else
              this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav('Draft Memos'), this.arabic('memo') + ' ' + id);
            break;
          case 5:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Memos', 'Historical Memos Incoming', 'Memo ' + id);
            else
              this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav('Historical Memos Incoming'), this.arabic('memo') + ' ' + id);
            break;
          case 6:
            if (lang == 'en')
              this.common.breadscrumChange('Internal Memos', 'Historical Memos Outgoing', 'Memo ' + id);
            else
              this.common.breadscrumChange(this.arabic('internalmemos'), this.arabicNav('Historical Memos Outgoing'), this.arabic('memo') + ' ' + id);

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

  //  createItem(APIString:string, Data:any){
  //   return this.httpClient.post(this.endpointService.apiHostingURL+APIString,Data);
  // }
}
// import { Injectable } from '@angular/core';
// import { Subject } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
// export class MemoService {

